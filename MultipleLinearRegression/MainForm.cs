using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.UI.DataVisualization.Charting;

namespace MultipleLinearRegression
{
    public partial class MainForm : Form
    {
        List<List<double>> data;
        int numberOfRegions;
        int numberOfPeriods;
        List<List<int>> groupsOfCorrelatedParameters;
        List<Model> models;
        Dictionary<string, int> parameterIdxFromName;
        Dictionary<int, string> parameterNameFromIdx;
        List<int> chosenParametersIndexes;
        List<List<double>> initialChosenDataX;
        List<List<double>> chosenDataX;
        List<List<double>> chosenDataY;
        Model bestModel;
        Model robustModel;
        Model chosenForControlModel;

        public MainForm()
        {
            InitializeComponent();
            cbAlpha.SelectedIndex = 1;
            pbReadingData.Visible = false;
            numCorrelX.Value = 0.7m;
            numCorrelXY.Value = 0.5m;
            numDeltaR.Value = 0.01m;
            cbIsRegressionNonLinear.SelectedIndex = 0;
            cbIterationsCount.SelectedIndex = 0;
            btNext.Enabled = false;
            btParamsInfo.Hide();
            InitializeDGV();

        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                btNext.Enabled = true;
                lbAllParameters.Items.Clear();
                lbX.Items.Clear();
                lbY.Items.Clear();

                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelWB = excelApp.Workbooks.Open(ofd.FileName);
                Excel.Worksheet excelWS = (Excel.Worksheet)excelWB.Sheets[1];

                pbReadingData.Visible = true;
                pbReadingData.Maximum = 10;
                pbReadingData.Value = 0;
                pbReadingData.Step = 1;

                int rowIdx = 2, colIdx = 3;
                data = new List<List<double>>();
                numberOfPeriods = 0;
                int paramNameIndex = 0;
                parameterIdxFromName = new Dictionary<string, int>();
                parameterNameFromIdx = new Dictionary<int, string>();

                while (true)
                {
                    if (pbReadingData.Value == pbReadingData.Maximum)
                        pbReadingData.Value = 1;
                    pbReadingData.PerformStep();

                    string paramName = GetCellValue(1, colIdx, excelWS);
                    if (paramName == "")
                        break;
                    parameterIdxFromName.Add(paramName, paramNameIndex);
                    parameterNameFromIdx.Add(paramNameIndex, paramName);
                    paramNameIndex++;

                    List<double> parameterData = new List<double>();
                    while(GetCellValue(rowIdx, colIdx, excelWS)!="")
                    {
                        if (colIdx == 3 && GetCellValue(rowIdx, 1, excelWS) != "")
                            numberOfPeriods++;
                        parameterData.Add(double.Parse(GetCellValue(rowIdx, colIdx, excelWS)));
                        rowIdx++;
                    }

                    data.Add(parameterData);
                    rowIdx = 2;
                    colIdx++;
                }

                pbReadingData.Visible = false;

                foreach (var pName in parameterIdxFromName)
                    lbAllParameters.Items.Add(pName.Key);

                numberOfRegions = data[0].Count / numberOfPeriods;
                excelApp.Quit();
                                
            }
        }

        string GetCellValue(int rowIndex, int columnIndex, Excel.Worksheet ws)
        {
            Excel.Range cell = (Excel.Range)ws.Cells[rowIndex, columnIndex];
            if (cell.Value != null)
                return cell.Value.ToString();
            return "";
        }
 
        void PrintModelInfo(Model model)
        {
            if (model == null)
                return;
            string errorDistribution = (model.isErrorDistributionNormal) ? "N(0,1)" : "unknown";
            string significance = (model.isSignificant) ? "+" : "-";
            dgvModels.Rows.Add(
                model.EquationString(),
                significance,
                errorDistribution,
                model.determCoeff,
                model.correctedDetermCoeff
               // model.avgCorrectedDetermCoeff,
               // model.variationCoeff
                );                       
        }

        
        
        void Calculate()
        {
            chosenDataX = new List<List<double>>();
            chosenDataY = new List<List<double>>();
            chosenParametersIndexes = new List<int>();

            foreach (var item in lbX.Items)
                chosenParametersIndexes.Add(parameterIdxFromName[item.ToString()]);
            foreach (var item in lbX.Items)
                chosenDataX.Add(data[parameterIdxFromName[item.ToString()]]);
            foreach (var item in lbY.Items)
                chosenDataY.Add(data[parameterIdxFromName[item.ToString()]]);

            if (cbIsRegressionNonLinear.Text == "Да")
            {
                // сохраняем начальные значения Xi 
                initialChosenDataX = chosenDataX.Select(Xi => Xi.ToList()).ToList();
                NonLinearRegression.maxIter =
                    cbIterationsCount.Text == "max" ? int.MaxValue : int.Parse(cbIterationsCount.Text);
                NonLinearRegression.eps = (double)numDeltaR.Value;
                NonLinearRegression.GenerateFunctions();
                // данные Х преобразованы
                NonLinearRegression.Calculate(ref chosenDataX, chosenDataY[0]);
            }

            groupsOfCorrelatedParameters =
                Solver.CreateGroupsOfCorrelatedParameters(chosenDataX, (double)numCorrelX.Value);

            models = new List<Model>();
            Solver.CreateModels(groupsOfCorrelatedParameters, 0, new List<int>(), models);

            double[] Y = chosenDataY[0].ToArray();

            foreach (var model in models)
                model.RemoveInsignificantParameters(chosenDataX, chosenDataY, (double)numCorrelXY.Value);
            
            Solver.RemoveModelsWithoutParameters(models);
            Solver.RemoveIdenticalModels(models);

            foreach (var model in models)
            {
                model.BuildMatrixX(chosenDataX);
                model.Y = Y;
                model.BuildEquation();
                model.PredictY();

                model.CheckErrorDistribution(double.Parse(cbAlpha.Text));
                if (!model.isErrorDistributionNormal)
                    model.isSignificant = false;
                else
                {
                    double[] unchangedCoeffs = new double[model.coefficients.Length];
                    Array.Copy(model.coefficients, unchangedCoeffs, unchangedCoeffs.Length);
                    model.CheckModelSignificance(
                        false, unchangedCoeffs, chosenDataY, double.Parse(cbAlpha.Text));
                }

                PrintModelInfo(model);                         
            }

            var tmp = Solver.FindBestModels(models, numberOfPeriods, numberOfRegions);
            bestModel = tmp.bestModel;
            robustModel = tmp.robustModel;
        }

        private void btAddX_Click(object sender, EventArgs e)
        {
            if (lbAllParameters.SelectedItems.Count != 0)
            {
                foreach (var item in lbAllParameters.SelectedItems)
                    lbX.Items.Add(item);

                for (int i = lbAllParameters.SelectedItems.Count - 1; i >= 0; i--)
                    lbAllParameters.Items.Remove(lbAllParameters.SelectedItems[i]);
            }
        }

        private void btRemoveX_Click(object sender, EventArgs e)
        {
            if (lbX.SelectedItems.Count != 0)
            {
                foreach (var item in lbX.SelectedItems)
                    lbAllParameters.Items.Add(item);

                for (int i = lbX.SelectedItems.Count - 1; i >= 0; i--)
                    lbX.Items.Remove(lbX.SelectedItems[i]);
            }
        }

        private void btAddY_Click(object sender, EventArgs e)
        {
            if (lbAllParameters.SelectedItems.Count != 0)
            {
                foreach (var item in lbAllParameters.SelectedItems)
                    lbY.Items.Add(item);

                for (int i = lbAllParameters.SelectedItems.Count - 1; i >= 0; i--)
                    lbAllParameters.Items.Remove(lbAllParameters.SelectedItems[i]);
            }
        }

        private void btRemoveY_Click(object sender, EventArgs e)
        {
            if (lbY.SelectedItems.Count != 0)
            {
                foreach (var item in lbY.SelectedItems)
                    lbAllParameters.Items.Add(item);

                for (int i = lbY.SelectedItems.Count - 1; i >= 0; i--)
                    lbY.Items.Remove(lbY.SelectedItems[i]);
            }
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            switch (tc.SelectedIndex)
            {
                case 0:
                    btBack.Show();
                    break;
                case 1:
                    if (lbX.Items.Count == 0 || lbY.Items.Count == 0)
                    {
                        MessageBox.Show("Выберите параметры!");
                        return;
                    }
                    Calculate();
                    btParamsInfo.Show();
                    break;
                case 2:
                    if (dgvModels.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Выберите модель для управления!");
                        return;
                    } 
                    foreach (var model in models)
                        if (dgvModels.SelectedRows[0].Cells[0].FormattedValue.ToString() == model.EquationString())
                            chosenForControlModel = model;
                    lbParams.Items.Clear();
                    lbDependentParams.Items.Clear();
                    tbNewXValue.Text = "";
                    lblNewYValue.Text = "";
                    lblLeftLimit.Text = "";
                    lblRightLimit.Text = "";
                    foreach (var item in chosenForControlModel.parametersIndexes)
                    {
                        foreach (var name in parameterIdxFromName.Keys)
                            if (parameterIdxFromName[name] == item)
                                lbParams.Items.Add(name);
                    }
                    ControlCalculations();
                    btNext.Text = "Выход";
                    break;
                case 3:
                    this.Close();
                    break;
            }
            tc.SelectedIndex += 1;          
        }

        void ControlCalculations()
        {
            NonLinearRegression.chosenForControlModel = chosenForControlModel;
            NonLinearRegression.ReadMutualInfluenceData(chosenForControlModel.parametersIndexes);
            NonLinearRegression.FillMatrixX(initialChosenDataX, chosenForControlModel.parametersIndexes);
            NonLinearRegression.CreateModels();
            CheckRangeWidth();
            NonLinearRegression.InitializeState(chosenDataY[0][0]);
        }

        private void btBack_Click(object sender, EventArgs e)
        {
            dgvModels.Rows.Clear();
            switch (tc.SelectedIndex)
            {
                case 1:
                    btBack.Hide();
                    break;
                case 2:
                    btNext.Text = "Далее";
                    btParamsInfo.Hide();
                    break;
                case 3:
                    btNext.Text = "Далее";
                    break;
            }
            tc.SelectedIndex -= 1;
        }

        public void InitializeDGV()
        {
            int numberOfColumns = 5;
            var columns = new DataGridViewColumn[numberOfColumns];

            for (int i = 0; i < numberOfColumns; i++)
            {
                columns[i] = new DataGridViewTextBoxColumn();
            }

            columns[0].HeaderText = "Уравнение";
            columns[1].HeaderText = "Значимость";
            columns[2].HeaderText = "Распределение ошибок";
            columns[3].HeaderText = "Коэф-т детерминации";
            columns[4].HeaderText = "Скорр. коэф-т детерм.";
           // columns[5].HeaderText = "Коэф-т вариации";
            dgvModels.Columns.AddRange(columns);            
        }

        private void rbAllModels_Click(object sender, EventArgs e)
        {
            dgvModels.Rows.Clear();
            foreach (var model in models)
                PrintModelInfo(model);
        }

        private void rbRobustModels_Click(object sender, EventArgs e)
        {
            dgvModels.Rows.Clear();
            PrintModelInfo(robustModel);
        }

        private void rbBestModels_Click(object sender, EventArgs e)
        {
            dgvModels.Rows.Clear();
            PrintModelInfo(bestModel);
        }

        private void btParamInfo_Click(object sender, EventArgs e)
        {
            var formParamsInfo = new FormParamsInfo(parameterIdxFromName);
            formParamsInfo.Show();
            
        }

        void UpdateLimits()
        {
            var paramIdx = parameterIdxFromName[lbParams.Text];
            lblLeftLimit.Text =  FormatValue(NonLinearRegression.paramsLimits[paramIdx].Item1);
            lblRightLimit.Text = FormatValue(NonLinearRegression.paramsLimits[paramIdx].Item2);
        }

        private void lbParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lbDependentParams.Items.Clear();
                var paramIdx = parameterIdxFromName[lbParams.Text];
                foreach (var dependentParamIdx in NonLinearRegression.modelsX[paramIdx].dependentParamsIdxs)
                    lbDependentParams.Items.Add(parameterNameFromIdx[dependentParamIdx]);
                UpdateLimits();

                tbNewXValue.Text = FormatValue(NonLinearRegression.stateX[paramIdx]);
                lblNewYValue.Text = FormatValue(NonLinearRegression.stateY);
            }
            catch(Exception) { return; }
        }

                        
        private void tbNewXValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    double.Parse(tbNewXValue.Text);
                }
                catch (Exception)
                {
                    epValueOutOfRange.SetError(tbNewXValue, "Неверный формат данных!");
                    return;
                }
                if (!NonLinearRegression.IsInInterval(
                    double.Parse(tbNewXValue.Text),
                    double.Parse(lblLeftLimit.Text),
                    double.Parse(lblRightLimit.Text)))
                    epValueOutOfRange.SetError(tbNewXValue, "Значение вне допустимого диапазона!");
                else
                    epValueOutOfRange.Dispose();

                var paramIdx = parameterIdxFromName[lbParams.Text];
                NonLinearRegression.stateX[paramIdx] = double.Parse(tbNewXValue.Text);
                // если следующий параметр зависит от текущего и/или предыдущих, вычисляем его новое значение
                // и проверяем, что оно находится в ОДЗ
                foreach (var dependentParamIdx in NonLinearRegression.modelsX[paramIdx].dependentParamsIdxs)
                {
                    // значения параметров, от которых зависит следующий параметр текущего аааааааа
                    var paramsValues = new List<double>();
                    foreach (var pIdx in NonLinearRegression.modelsX[dependentParamIdx].parametersIndexes)
                        paramsValues.Add(NonLinearRegression.stateX[pIdx]);
                    NonLinearRegression.stateX[dependentParamIdx] =
                        NonLinearRegression.modelsX[dependentParamIdx].
                        GetPredictedYForVector(paramsValues.ToArray());
                    if (NonLinearRegression.modelsX[dependentParamIdx].paramsLimits.ContainsKey(paramIdx))
                        NonLinearRegression.paramsLimits[dependentParamIdx] =
                            NonLinearRegression.modelsX[dependentParamIdx].paramsLimits[paramIdx];
                    // проверка на принадлежность ОДЗ
                    if (!NonLinearRegression.IsInInterval(NonLinearRegression.stateX[dependentParamIdx],
                        NonLinearRegression.paramsLimits[dependentParamIdx].Item1,
                        NonLinearRegression.paramsLimits[dependentParamIdx].Item2))
                        epValueOutOfRange.SetError(tbNewXValue,
                            "Зависимые параметры выходят за допустимые границы");
                    else
                        NonLinearRegression.NarrowRange(dependentParamIdx, paramIdx);
                }
                NonLinearRegression.TransformX();
                NonLinearRegression.stateY = 
                    chosenForControlModel.GetPredictedYForVector(NonLinearRegression.transformedX.ToArray());
                lblNewYValue.Text = FormatValue(NonLinearRegression.stateY);
            }
            catch(Exception) { return; }
        }

        void CheckRangeWidth()
        {
            var ok = NonLinearRegression.FindParamsLimits((int)numK.Value, (int)(numPercents.Value));
            if (!ok)
                epTooWideRange.SetError(numPercents, "Интервал выходит за допустимые границы!");
            else
                epTooWideRange.Dispose();
            if (lbParams.SelectedItems.Count != 0)
                UpdateLimits();
            foreach (var pIdx in NonLinearRegression.chosenForControlModel.parametersIndexes)
            {
                foreach (var prevParamIdx in NonLinearRegression.modelsX[pIdx].parametersIndexes)
                {
                    NonLinearRegression.modelsX[pIdx].paramsLimits[prevParamIdx] =
                        NonLinearRegression.paramsLimits[pIdx];
                }
            }
        }

        private void numK_ValueChanged(object sender, EventArgs e)
        {
            CheckRangeWidth();
        }

        private void numPercents_ValueChanged(object sender, EventArgs e)
        {
            CheckRangeWidth();
        }

        string FormatValue(double value)
        {
            return string.Format("{0:0.000}", value);
        }
    }
}
