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
        NonLinearRegression nonLinearRegression;

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

        // создает все возможные модели, выбирая по одному параметру из каждой группы коррелированных
        void CreateModels(int groupIdx, List<int> modelParameters)
        {
            if (groupIdx==groupsOfCorrelatedParameters.Count)
            {
                Model model = new Model(modelParameters);
                models.Add(model);
                return;
            }

            foreach (int p in groupsOfCorrelatedParameters[groupIdx])
            {
                modelParameters.Add(p);
                CreateModels(groupIdx + 1, modelParameters);
                modelParameters.RemoveAt(modelParameters.Count - 1);
            }
        }


        void CreateGroupsOfCorrelatedParameters(List<List<double>> data)
        {
            double[,] correlMatrix = new double[data.Count,data.Count];
            for (int i = 0; i < data.Count; i++)
                for (int j = i + 1; j < data.Count; j++)
                    correlMatrix[i,j] = (Math.Abs(Statistics.CorrelationCoefficient(data[i], data[j])) >= (double)numCorrelX.Value) ? 1 : 0;
                       
            groupsOfCorrelatedParameters = new List<List<int>>();
            for (int i = 0; i < data.Count; i++)
            {
                List<int> group = new List<int>();
                group.Add(i);
                for (int j = i + 1; j < data.Count; j++)
                {                    
                    if (correlMatrix[i, j] == 1)
                        group.Add(j);
                }
                groupsOfCorrelatedParameters.Add(group);
            }

            for (int i=0; i<groupsOfCorrelatedParameters.Count; i++)
                for (int j=i+1; j<groupsOfCorrelatedParameters.Count; j++)
                {
                    var res = groupsOfCorrelatedParameters[i].Intersect(groupsOfCorrelatedParameters[j]);
                    if (res.Count()!=0)
                    {
                        var res_ = groupsOfCorrelatedParameters[j].Except(groupsOfCorrelatedParameters[i]);
                        groupsOfCorrelatedParameters[i].AddRange(res_);
                        groupsOfCorrelatedParameters.RemoveAt(j);
                        i--;
                        break;
                    }
                }
        }

        void RemoveInsignificantParameters(Model model)
        {
            for (int i = model.parametersIndexes.Count-1; i>=0; i--)
            {
                if (Math.Abs(Statistics.CorrelationCoefficient
                    (chosenDataX[model.parametersIndexes[i]], chosenDataY[0])) < (double)numCorrelXY.Value)
                    model.parametersIndexes.RemoveAt(i);
            }
        }

        void BuildMatrixX(Model model)
        {
            model.X = new double[chosenDataX[0].Count, model.parametersIndexes.Count+1];
            for (int i = 0; i < model.X.GetLength(0); i++)
                model.X[i, 0] = 1;
            for (int i = 0; i < model.X.GetLength(0); i++)
                for (int j = 1; j < model.X.GetLength(1); j++)
                    model.X[i, j] = chosenDataX[model.parametersIndexes[j-1]][i];
        }
        
        bool CheckModelSignificance(Model model, bool isModified, double[] unchangedCoeffs)
        {
            double maxCoeff = 0;
            for (int i = 1; i < model.coefficients.Length; i++)
                if (Math.Abs(model.coefficients[i]) > maxCoeff)
                    maxCoeff = Math.Abs(model.coefficients[i]);
            if (maxCoeff < 0.000001)
            {
                unchangedCoeffs.CopyTo(model.coefficients, 0);
                return model.isSignificant = false;                
            }

            model.PredictY();
            model.determCoeff = Statistics.DeterminationCoefficient(model.Y, model.predictedY);
            model.correctedDetermCoeff = Statistics.CorrectedDeterminationCoefficient(
                model.Y, model.predictedY, model.parametersIndexes.Count);

            double Ft, Ff;
            int n = chosenDataY[0].Count;
            int m = model.parametersIndexes.Count; // число параметров модели
            Ff = (model.determCoeff / m) / ((1 - model.determCoeff) / (n - m - 1));

            Chart c = new Chart();
            Ft = c.DataManipulator.Statistics.InverseFDistribution(
                double.Parse(cbAlpha.Text), m , n - m - 1);

            model.isSignificant = Ff > Ft;

            if (!model.isSignificant && !isModified)
            {
                CheckCoefficientsSignificance(model, unchangedCoeffs);
            }

            return model.isSignificant;
        }

        void CheckCoefficientsSignificance(Model model, double[] unchangedCoeffs)
        {
            List<int> insignificantCoeffsIndexes = new List<int>();

            for (int i =0; i<model.parametersIndexes.Count; i++)
            {
                bool isSignificant = true;
                double SSres = model.Y.Zip(model.predictedY, (y, p) => Math.Pow(y - p, 2.0)).Sum();
                int n = chosenDataY[0].Count;
                int m = model.parametersIndexes.Count;
                // стандартная ошибка
                double sigma = SSres / (n - m - 1);
                // стандартная ошибка оценки i+1-го коэф-та
                double S_bi = Math.Sqrt(model.XTXinv[i+1, i+1] * sigma);
                // вычисленное значение Т-статистики
                double Tf = model.coefficients[i + 1] / S_bi;
                Chart c = new Chart();
                // табличное значение Т-статистики
                double Tt = c.DataManipulator.Statistics.InverseTDistribution(double.Parse(cbAlpha.Text), n - m - 1);
                isSignificant = Tf > Tt;

                if (!isSignificant)
                    insignificantCoeffsIndexes.Add(i);
            }

            ModifyModel(model, insignificantCoeffsIndexes, 0, unchangedCoeffs);
        }

        void ModifyModel(Model model, List<int> coeffsIndexes, int idx, double[] unchangedCoeffs)
        {
            if (idx == coeffsIndexes.Count || CheckModelSignificance(model, true, unchangedCoeffs))
                return;

            double coeff = model.coefficients[coeffsIndexes[idx]];
            model.coefficients[coeffsIndexes[idx]] = 0;
            ModifyModel(model, coeffsIndexes, idx + 1, unchangedCoeffs);

            if (model.isSignificant)
                return;

            model.coefficients[coeffsIndexes[idx]] = coeff;
            ModifyModel(model, coeffsIndexes, idx + 1, unchangedCoeffs);            
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

        bool AreArraysIdentical(List<int> arr1, List<int> arr2)
        {
            for (int i = 0; i < arr1.Count; i++)
                if (arr1[i] != arr2[i])
                    return false;
            return true;
        }

        void RemoveIdenticalModels()
        {
            for (int i=0; i< models.Count; i++)
            {
                for (int j=i+1; j< models.Count; j++)
                {
                    if (AreArraysIdentical(models[i].parametersIndexes, models[j].parametersIndexes))
                    {
                        models.RemoveAt(j--);                      
                    }
                }
            }
        }

        void RemoveModelsWithoutParameters()
        {
            for (int i=0; i<models.Count; i++)
            {
                if (models[i].parametersIndexes.Count == 0)
                {
                    models.RemoveAt(i--);
                }
            }
        }

        void FindBestModels()
        {
            bestModel = robustModel = null;
            foreach (var model in models)
            {
                model.correctedDetermCoeffsArray = new double[numberOfPeriods];
                for (int i = 0; i < numberOfPeriods; i++)
                    model.GetAnnualInfo(i, numberOfRegions);
                model.avgCorrectedDetermCoeff = model.correctedDetermCoeffsArray.Average();
                model.variationCoeff = 
                    Statistics.VariationCoeff(model.correctedDetermCoeffsArray);
            }

            foreach (var model in models)
            {
                if (model.isSignificant)
                {
                    bestModel = robustModel = model;
                    break;
                }
                if (model == models.Last())
                    return;
            }
            
            foreach (var model in models)
            {
                if (!model.isSignificant)
                    continue;
                if (model.avgCorrectedDetermCoeff > bestModel.avgCorrectedDetermCoeff)
                    bestModel = model;
                if (model.variationCoeff < robustModel.variationCoeff)
                    robustModel = model;
            }

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
                nonLinearRegression = new NonLinearRegression(
                    cbIterationsCount.Text == "max" ? int.MaxValue :
                    int.Parse(cbIterationsCount.Text),
                    (double)numDeltaR.Value);
                nonLinearRegression.GenerateFunctions();
                // данные Х преобразованы
                nonLinearRegression.Calculate(ref chosenDataX, chosenDataY[0]);
            }

            CreateGroupsOfCorrelatedParameters(chosenDataX);

            models = new List<Model>();
            CreateModels(0, new List<int>());

            double[] Y = new double[chosenDataY[0].Count];
            for (int i = 0; i < Y.Length; i++)
                Y[i] = chosenDataY[0][i];

            foreach (var model in models)
            {
                RemoveInsignificantParameters(model);
            }

            RemoveModelsWithoutParameters();
            RemoveIdenticalModels();

            foreach (var model in models)
            {
                BuildMatrixX(model);
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
                    CheckModelSignificance(model, false, unchangedCoeffs);
                }

                PrintModelInfo(model);                         
            }

            FindBestModels();
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
            nonLinearRegression.chosenForControlModel = chosenForControlModel;
            nonLinearRegression.ReadMutualInfluenceData(chosenForControlModel.parametersIndexes);
            nonLinearRegression.FillMatrixX(initialChosenDataX, chosenForControlModel.parametersIndexes);
            nonLinearRegression.CreateModels();
            //nonLinearRegression.FindParamsLimits((int)numK.Value, (int)numPercents.Value);
            CheckRangeWidth();
            nonLinearRegression.InitializeState(chosenDataY[0][0]);
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
            lblLeftLimit.Text =  FormatValue(nonLinearRegression.paramsLimits[paramIdx].Item1);
            lblRightLimit.Text = FormatValue(nonLinearRegression.paramsLimits[paramIdx].Item2);
        }

        private void lbParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lbDependentParams.Items.Clear();
                var paramIdx = parameterIdxFromName[lbParams.Text];
                foreach (var dependentParamIdx in nonLinearRegression.modelsX[paramIdx].dependentParamsIdxs)
                    lbDependentParams.Items.Add(parameterNameFromIdx[dependentParamIdx]);
                UpdateLimits();

                tbNewXValue.Text = FormatValue(nonLinearRegression.stateX[paramIdx]);
                lblNewYValue.Text = FormatValue(nonLinearRegression.stateY);
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
                if (!nonLinearRegression.IsInInterval(
                    double.Parse(tbNewXValue.Text),
                    double.Parse(lblLeftLimit.Text),
                    double.Parse(lblRightLimit.Text)))
                    epValueOutOfRange.SetError(tbNewXValue, "Значение вне допустимого диапазона!");
                else
                    epValueOutOfRange.Dispose();

                var paramIdx = parameterIdxFromName[lbParams.Text];
                nonLinearRegression.stateX[paramIdx] = double.Parse(tbNewXValue.Text);
                // если следующий параметр зависит от текущего и/или предыдущих, вычисляем его новое значение
                // и проверяем, что оно находится в ОДЗ
                foreach (var dependentParamIdx in nonLinearRegression.modelsX[paramIdx].dependentParamsIdxs)
                {
                    // значения параметров, от которых зависит следующий параметр текущего аааааааа
                    var paramsValues = new List<double>();
                    foreach (var pIdx in nonLinearRegression.modelsX[dependentParamIdx].parametersIndexes)
                        paramsValues.Add(nonLinearRegression.stateX[pIdx]);
                    nonLinearRegression.stateX[dependentParamIdx] =
                        nonLinearRegression.modelsX[dependentParamIdx].
                        GetPredictedYForVector(paramsValues.ToArray());
                    if (nonLinearRegression.modelsX[dependentParamIdx].paramsLimits.ContainsKey(paramIdx))
                        nonLinearRegression.paramsLimits[dependentParamIdx] =
                            nonLinearRegression.modelsX[dependentParamIdx].paramsLimits[paramIdx];
                    // проверка на принадлежность ОДЗ
                    if (!nonLinearRegression.IsInInterval(nonLinearRegression.stateX[dependentParamIdx],
                        nonLinearRegression.paramsLimits[dependentParamIdx].Item1,
                        nonLinearRegression.paramsLimits[dependentParamIdx].Item2))
                        epValueOutOfRange.SetError(tbNewXValue,
                            "Зависимые параметры выходят за допустимые границы");
                    else
                        nonLinearRegression.NarrowRange(dependentParamIdx, paramIdx);
                }
                nonLinearRegression.TransformX();
                nonLinearRegression.stateY = 
                    chosenForControlModel.GetPredictedYForVector(nonLinearRegression.transformedX.ToArray());
                lblNewYValue.Text = FormatValue(nonLinearRegression.stateY);
            }
            catch(Exception) { return; }
        }

        void CheckRangeWidth()
        {
            var ok = nonLinearRegression.FindParamsLimits((int)numK.Value, (int)(numPercents.Value));
            if (!ok)
                epTooWideRange.SetError(numPercents, "Интервал выходит за допустимые границы!");
            else
                epTooWideRange.Dispose();
            if (lbParams.SelectedItems.Count != 0)
                UpdateLimits();
            foreach (var pIdx in nonLinearRegression.chosenForControlModel.parametersIndexes)
            {
                foreach (var prevParamIdx in nonLinearRegression.modelsX[pIdx].parametersIndexes)
                {
                    nonLinearRegression.modelsX[pIdx].paramsLimits[prevParamIdx] =
                        nonLinearRegression.paramsLimits[pIdx];
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
