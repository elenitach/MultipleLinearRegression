using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MultipleLinearRegression
{    
    class NonLinearRegression
    {
        // все возможные функции
        public List<Func<double, double>> functions = new List<Func<double, double>>();
        // последовательность примененных для преобразования функций (для каждого Хi)
        public List<List<Func<double, double>>> functionsSequencesForAllXi =
            new List<List<Func<double, double>>>();
        List<List<double>> newX = new List<List<double>>();
        int maxIter;
        double eps;
        List<List<bool>> mutualInfluenceMatrix;
        public Dictionary<int, Tuple<double, double>> paramsLimits; // paramIdx, leftLimit, rightLimit
        public Model chosenForControlModel;
        List<Tuple<int, List<double>>> initialModelX; // X управляемой модели: глобальный индекс + Xi
        public Dictionary<int, Model> modelsX; // модели для каждого Хi
        public Dictionary<int, double> stateX; // текущие значения параметров Xi
        public double stateY; // текущее значение Y
        public List<double> transformedX; // преобразованные значения Х

        public NonLinearRegression(int maxIter, double eps)
        {
            this.maxIter = maxIter;
            this.eps = eps;
        }
        
        public bool FindParamsLimits(int k, int percents)
        {
            paramsLimits = new Dictionary<int, Tuple<double, double>>();
            var ok = true;
            foreach (var Xi in initialModelX)
            {
                var min = Xi.Item2.Min();
                var max = Xi.Item2.Max();
                var intervalLength = max - min;
                var avg = Xi.Item2.Average();
                var standardDeviation = Statistics.StandardDeviation(Xi.Item2.ToArray());
                var ratio = (avg - min) / intervalLength;

                var firstApproachLeftLimit = min - intervalLength * (1 - ratio) * percents / 100;
                var firstApproachRightLimit = max + intervalLength * ratio * percents / 100;

                var secondApproachLeftLimit = avg - k * standardDeviation;
                var secondApproachRightLimit = avg + k * standardDeviation;

                var leftLimit = Math.Max(0, firstApproachLeftLimit);
                var rightLimit = firstApproachRightLimit;

                paramsLimits[Xi.Item1] = Tuple.Create(leftLimit, rightLimit);

                if (leftLimit < secondApproachLeftLimit || rightLimit > secondApproachRightLimit)
                    ok = false;
            }
            return ok;
        }

        public void GenerateFunctions()
        {
            var positiveAlphas = new[] {
                0.001, 0.01, 0.1, 0.5, 1,
                1.5, 2, 2.5, 3, 3.5, 4,
                4.5, 5, 6, 7, 8, 9, 10 
            };
            var alphas = positiveAlphas.SelectMany(alpha => new[] { alpha, -alpha });
            foreach (var p in new[] { 2, 3, -1, -2, -3, 1 / 3, 1 / 2 })
                functions.Add(x => Math.Pow(x, p));
            functions.AddRange(
                new Func<double, double>[] 
                {
                    x => Math.Log(x),
                    x => Math.Sin(x),
                    x => Math.Tan(x),
                    x => Math.Atan(x)
                });
            foreach(var alpha in alphas)
            {
                functions.AddRange(new Func<double, double>[]
                {
                    x => Math.Exp(alpha * x),
                    x => Math.Exp(alpha * x * x),
                    x => 1 / (1 + Math.Exp(alpha * x)),
                    x => (Math.Exp(alpha * x) - 1) / (Math.Exp(alpha * x) + 1),
                    x => (Math.Exp(alpha * x) - Math.Exp(-alpha * x)) / 2
                });
            }
        }

        public void Calculate(ref List<List<double>> X, List<double> Y)
        {
            foreach (var Xi in X)
            {
                var functionsSequence = new List<Func<double, double>>();
                var Rxy = Math.Abs(Statistics.CorrelationCoefficient(Xi, Y));
                CalculateForXi(Xi, Y, 1, Rxy, functionsSequence);
                functionsSequencesForAllXi.Add(functionsSequence);
            }
            
            X = newX; // теперь данные в исходной программе преобразованы
        }

        public void CalculateForXi(List<double> Xi, List<double> Y, int iter, 
            double previousRxy, List<Func<double, double>> functionsSequence)
        {
            if (iter > maxIter)
            {
                newX.Add(Xi);
                return; 
            }
            // преобразуем к интервалу [2, 102]
            var xMin = Xi.Min();
            var xMax = Xi.Max();
            var normX = Xi.Select(x => (x - xMin) / (xMax - xMin) * 100 + 2);

            var maxRxy = 0.0;
            Func<double, double> fun = null;
            List<double> newXi = null;

            foreach (var f in functions)
            {
                var f_x = normX.Select(x => f(x)).ToList();
                var newRxy = Math.Abs(Statistics.CorrelationCoefficient(f_x, Y));
                if (newRxy > maxRxy)
                {
                    maxRxy = newRxy;
                    fun = f;
                    newXi = f_x;
                }
            }

            if (maxRxy > previousRxy + eps)
            {
                functionsSequence.Add(fun);
                CalculateForXi(newXi, Y, iter + 1, maxRxy, functionsSequence);
            }      
            else
            {
                newX.Add(Xi);
                return; 
            }
        }

        public void ReadMutualInfluenceData(List<int> paramsIndexes)
        {
            using (var sr = new StreamReader("Матрица взаимовлияний.txt"))
            {
                var allParamsInfluenceMatrix = new List<List<bool>>();
                mutualInfluenceMatrix = new List<List<bool>>();
               // var rowIdx = 0;
                while (true)
                {
                    var row = sr.ReadLine();
                    if (row == null) break;

                    //if (!paramsIndexes.Contains(rowIdx))
                    //{
                    //    rowIdx++;
                    //    continue;
                    //}
                    var data = row.Split('\t')
                        .Select(elem => elem == "0" ? false : true)
                        .ToList();
                    allParamsInfluenceMatrix.Add(data);
                    //var selectedData = new List<bool>();
                    //for (int i = 0; i < data.Count; i++)
                    //    if (paramsIndexes.Contains(i))
                    //        selectedData.Add(data[i]);
                    //mutualInfluenceMatrix.Add(selectedData);
                }

                foreach (var i in paramsIndexes)
                {
                    var row = new List<bool>();
                    foreach (var j in paramsIndexes)
                        row.Add(allParamsInfluenceMatrix[i][j]);
                    mutualInfluenceMatrix.Add(row);
                }

            }
        }

        // из начальной матрицы выбранных параметров берем те параметры, которые входят в модель
        public void FillMatrixX(List<List<double>> initialX, List<int> paramsIndexes)
        {
            initialModelX = new List<Tuple<int, List<double>>>();
            foreach (var idx in paramsIndexes)
                initialModelX.Add(Tuple.Create(idx, initialX[idx]));
        }

        // составляем уравнения зависимости параметров
        public void CreateModels()
        {
            modelsX = new Dictionary<int, Model>();
            for(int k=0; k<initialModelX.Count; k++)
            {
                var paramsIndexes = new List<int>(); // глобальные индексы параметров, от которых зависит Xi
                var dependentParamsIndexes = new List<int>(); // индексы параметров, зависящих от Xi
                for (int j = k + 1; j < initialModelX.Count; j++)
                    if (mutualInfluenceMatrix[k][j])
                        dependentParamsIndexes.Add(initialModelX[j].Item1);
                for (int j = 0; j < k; j++)
                    if (mutualInfluenceMatrix[j][k])
                        paramsIndexes.Add(initialModelX[j].Item1);

                var model = new Model(paramsIndexes);
                model.dependentParamsIdxs = dependentParamsIndexes;
                
                model.X = new double[initialModelX[0].Item2.Count, model.parametersIndexes.Count + 1];
                for (int i = 0; i < model.X.GetLength(0); i++)
                    model.X[i, 0] = 1;
                for (int i = 0; i < model.X.GetLength(0); i++)
                    for (int j = 1; j < model.X.GetLength(1); j++)
                        model.X[i, j] = initialModelX[j-1].Item2[i];

                model.Y = initialModelX[k].Item2.ToArray();
                model.BuildEquation();
                modelsX[initialModelX[k].Item1] = model;
            }
        }

        public void TransformX()
        {
            transformedX = new List<double>();
            foreach (var pIdx in chosenForControlModel.parametersIndexes)
                transformedX.Add(GetTransformedValue(pIdx, stateX[pIdx],
                    initialModelX.Find(x => x.Item1 == pIdx).Item2.ToArray()));
        }

        // получаем преобразованное значение Xi
        public double GetTransformedValue(int paramIdx, double x, double[] X)
        {
            var transformed_x = x;
            var newX = X.ToArray();
            foreach (var f in functionsSequencesForAllXi[paramIdx])
            {
                // преобразуем к интервалу [2, 102]
                var xMin = X.Min();
                var xMax = X.Max();
                var normX = X.Select(x_ => (x_ - xMin) / (xMax - xMin) * 100 + 2);
                transformed_x = (transformed_x - xMin) / (xMax - xMin) * 100 + 2;

                newX = newX.Select(x_ => f(x_)).ToArray();
                transformed_x = f(transformed_x);                
            }
            return transformed_x;
        }

        public void InitializeState(double y)
        {
            stateX = new Dictionary<int, double>();
            foreach (var paramIdx in paramsLimits.Keys)
            {
                // инициализируем значениями показателей первого региона в первый год
                stateX[paramIdx] = initialModelX.Find(p => p.Item1 == paramIdx).Item2[0];
            }
            stateY = y;
        }

        public bool IsInInterval(double value, double left, double right)
        {
            return value > left && value < right;
        }

        public List<int> DependentParamsInIncorrectRanges(int paramIdx)
        {
            var paramsOutOfRange = new List<int>();
            foreach (var dependentParamIdx in modelsX[paramIdx].dependentParamsIdxs)
                if (!IsInInterval(stateX[dependentParamIdx],
                    paramsLimits[dependentParamIdx].Item1,
                    paramsLimits[dependentParamIdx].Item2))
                    paramsOutOfRange.Add(dependentParamIdx);
            return paramsOutOfRange;
        }

        // сужает диапазон изменения параметра
        public void NarrowRange(int paramIdx, int influencingParamIdx)
        {
            var left = modelsX[paramIdx].paramsLimits[influencingParamIdx].Item1;// initialParamsLimits[paramIdx].Item1;
            var right = modelsX[paramIdx].paramsLimits[influencingParamIdx].Item2;//initialParamsLimits[paramIdx].Item2;
            var currentValue = stateX[paramIdx];
            var deltaLeft = currentValue - left;
            var deltaRight = right - currentValue;
            if (deltaLeft > deltaRight)
                left = currentValue - deltaRight;
            else
                right = currentValue + deltaLeft;
            paramsLimits[paramIdx] = Tuple.Create(left, right);
            var idx = modelsX[paramIdx].parametersIndexes.IndexOf(influencingParamIdx);
            for (int i = idx + 1; i < modelsX[paramIdx].parametersIndexes.Count; i++)
                modelsX[paramIdx].paramsLimits[modelsX[paramIdx].parametersIndexes[i]] = 
                    Tuple.Create(left, right);
        }
    }
}
