using System;
using System.Collections.Generic;
using System.Linq;

namespace MultipleLinearRegression
{
    class Model
    {
        public List<int> parametersIndexes; // номера параметров, от которых зависит модель
        public double[] coefficients; // коэффициенты в уравнении регрессии
        public bool isSignificant; // является ли модель значимой
        public double[,] X; // значения независимых переменных
        public double[] Y; // значения зависимой переменной
        public double[,] XTXinv;
        public double determCoeff; // коэф-т детерминации модели
        public double correctedDetermCoeff; // скорректированный коэф-т детерминации
        public double[] predictedY; // предсказанные моделью значения зависимой переменной
        public bool isErrorDistributionNormal; // является ли распределение ошибки нормальным
        public double[] correctedDetermCoeffsArray; // коэф-ты детерминации для данных в различные годы
        public double avgCorrectedDetermCoeff;
        public double variationCoeff;
        public List<int> dependentParamsIdxs;
        // по индексу параметра, от которого зависит текущий, 
        // определяет границы изменения текущего параметра
        public Dictionary<int, Tuple<double, double>> paramsLimits = 
            new Dictionary<int, Tuple<double, double>>(); 

        public Model(List<int> p)
        {
            parametersIndexes = new List<int>(p);
        }
        
        public void BuildEquation()
        {
            if (parametersIndexes.Count > 0)
            {
                double[,] XT = Matrix.TransposedMatrix(X);
                double[,] XTX = Matrix.MultipliedMatrices(XT, X);
                XTXinv = Matrix.InversedMatrix(XTX);
                coefficients = Matrix.MultipliedMatrixVector(
                    Matrix.MultipliedMatrices(XTXinv, XT), Y);
            }
        }

        public string EquationString()
        {
            string s = string.Format("Y = {0:0.##}", coefficients[0]);
            //if (parametersIndexes.Count > 0)
           // {
                for (int i = 1; i < coefficients.Length; i++)
                {
                    string subStr = string.Format("{0:+ 0.####; - 0.####; + 0} * X{1}", coefficients[i], parametersIndexes[i-1] + 1);
                    s += subStr;
                }
            //}
            return s;
        }

        public void PredictY()
        {
            predictedY = Matrix.MultipliedMatrixVector(X, coefficients);
        }

        public double GetPredictedYForVector(double[] X)
        {
            var xWithFreeCoeff = new List<double>();
            xWithFreeCoeff.Add(1);
            xWithFreeCoeff.AddRange(X);
            return xWithFreeCoeff.Zip(coefficients, (xi, ai) => xi * ai).Sum();
        }
                
        public double CountNumbersInInterval(List<double> list, double mathExp, double stDev)
        {
            double leftLimit = mathExp - 3 * stDev;
            double rightLimit = mathExp + 3 * stDev;

            double counter = 0;
            foreach (var element in list)
                if (element < leftLimit || element > rightLimit)
                    counter++;
            return counter;
        }

        public double[] Hn(double[] Z)
        {
            var Hn_Z = Z.Select(z => Z.Where(z_ => z_ <= z).Count() / (double)Z.Count()).ToArray();
            return Hn_Z;                
        }

        public void CheckErrorDistribution(double alpha)
        {
            var error = Matrix.SubtractedVectors(Y, predictedY);
            var negativeError = error.Select(e => -e).ToArray();
            var Hn_Z = Hn(error);
            var Hn_negZ = Hn(negativeError);
            var w = Hn_Z.Zip(Hn_negZ, (err, negErr) => Math.Pow(err + negErr - 1, 2.0)).Sum();
            var wCritical = alpha == 0.01 ? 2.8 : alpha == 0.05 ? 1.66 : 1.2;
            isErrorDistributionNormal = (w <= wCritical);
        }

        public void GetAnnualInfo(int year, int numberOfRegions)
        {
            double[,] annualX = new double[numberOfRegions, X.GetLength(1)];
            double[] annualY = new double[numberOfRegions];
            double[] annualPredictedY;

            int firstRegionIdx = year * numberOfRegions;
            int lastRegionIdx = firstRegionIdx + numberOfRegions;
            for (int i = firstRegionIdx; i < lastRegionIdx; i++)
                for (int j = 0; j < parametersIndexes.Count + 1; j++)
                    annualX[i - firstRegionIdx, j] = X[i, j];
            for (int i = firstRegionIdx; i < lastRegionIdx; i++)
                annualY[i - firstRegionIdx] = Y[i];

            annualPredictedY = Matrix.MultipliedMatrixVector(annualX, coefficients);
            var annualDetermCoeff = Statistics.CorrectedDeterminationCoefficient(
                annualY, annualPredictedY, parametersIndexes.Count);
            correctedDetermCoeffsArray[year] = annualDetermCoeff;           

        }

    }
}
