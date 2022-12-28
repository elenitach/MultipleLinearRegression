using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleLinearRegression
{
    class Solver
    {
        // создает все возможные модели, выбирая по одному параметру из каждой группы коррелированных
       public static void CreateModels(
           List<List<int>> groupsOfCorrelatedParameters,
           int groupIdx, List<int> modelParameters, List<Model> models)
        {
            if (groupIdx == groupsOfCorrelatedParameters.Count)
            {
                Model model = new Model(modelParameters);
                models.Add(model);
                return;
            }

            foreach (int p in groupsOfCorrelatedParameters[groupIdx])
            {
                modelParameters.Add(p);
                CreateModels(groupsOfCorrelatedParameters, groupIdx + 1, modelParameters, models);
                modelParameters.RemoveAt(modelParameters.Count - 1);
            }
        }


        public static List<List<int>> CreateGroupsOfCorrelatedParameters(List<List<double>> data, double correlationCoefficientForX)
        {
            double[,] correlMatrix = new double[data.Count, data.Count];
            for (int i = 0; i < data.Count; i++)
                for (int j = i + 1; j < data.Count; j++)
                    correlMatrix[i, j] = (Math.Abs(Statistics.CorrelationCoefficient(data[i], data[j])) >= correlationCoefficientForX) ? 1 : 0;

            var groupsOfCorrelatedParameters = new List<List<int>>();
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

            for (int i = 0; i < groupsOfCorrelatedParameters.Count; i++)
                for (int j = i + 1; j < groupsOfCorrelatedParameters.Count; j++)
                {
                    var res = groupsOfCorrelatedParameters[i].Intersect(groupsOfCorrelatedParameters[j]);
                    if (res.Count() != 0)
                    {
                        var res_ = groupsOfCorrelatedParameters[j].Except(groupsOfCorrelatedParameters[i]);
                        groupsOfCorrelatedParameters[i].AddRange(res_);
                        groupsOfCorrelatedParameters.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            return groupsOfCorrelatedParameters;
        }

        public static void RemoveIdenticalModels(List<Model> models)
        {
            for (int i = 0; i < models.Count; i++)
            {
                for (int j = i + 1; j < models.Count; j++)
                {
                    if (AreArraysIdentical(models[i].parametersIndexes, models[j].parametersIndexes))
                    {
                        models.RemoveAt(j--);
                    }
                }
            }
        }

        static bool AreArraysIdentical(List<int> arr1, List<int> arr2)
        {
            for (int i = 0; i < arr1.Count; i++)
                if (arr1[i] != arr2[i])
                    return false;
            return true;
        }

        public static void RemoveModelsWithoutParameters(List<Model> models)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].parametersIndexes.Count == 0)
                {
                    models.RemoveAt(i--);
                }
            }
        }
               
        public static (Model bestModel, Model robustModel) FindBestModels(
            List<Model> models, int numberOfPeriods, int numberOfRegions)
        {
            Model bestModel = null;
            Model robustModel = null;
            foreach (var model in models)
            {
                model.correctedDetermCoeffsArray = new double[numberOfPeriods];
                for (int i = 0; i < numberOfPeriods; i++)
                    model.GetAnnualInfo(i, numberOfRegions);
                model.avgCorrectedDetermCoeff = model.correctedDetermCoeffsArray.Average();
                model.variationCoeff =
                    Statistics.VariationCoefficient(model.correctedDetermCoeffsArray);
            }

            foreach (var model in models)
            {
                if (model.isSignificant)
                {
                    bestModel = robustModel = model;
                    break;
                }
                if (model == models.Last())
                    return (bestModel, robustModel);
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
            return (bestModel, robustModel);
        }
    }    
}



