using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleLinearRegression
{
    static class Statistics
    {
        public static double CorrelationCoefficient(List<double> values1, List<double> values2)
        {
            double avg1 = values1.Average();
            double avg2 = values2.Average();

            double sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            double sumSqr1 = values1.Sum(x => Math.Pow((x - avg1), 2.0));
            double sumSqr2 = values2.Sum(y => Math.Pow((y - avg2), 2.0));

            double result = sum1 / Math.Sqrt(sumSqr1 * sumSqr2);

            return result;
        }

        public static double DeterminationCoefficient(double[] Y, double[] predictedY)
        {
            double avgY = Y.Average();
            double SSres = Y.Zip(predictedY, (y, p) => Math.Pow(y - p, 2.0)).Sum();
            double SStot = Y.Sum(y => Math.Pow(y - avgY, 2.0));
            return 1 - SSres / SStot;
        }

        public static double CorrectedDeterminationCoefficient(double[] Y, double[] predictedY, int m)
        {
            int n = Y.Length;
            double R = DeterminationCoefficient(Y, predictedY);
            return 1 - (n - 1) * (1 - R) / (n - m);
        }

        public static double CentralMoment(double[] list, double k)
        {
            double avg = list.Average();
            return list.Sum(x => Math.Pow((x - avg), k)) / list.Length;
        }

        public static double StandardDeviation(double[] list)
        {
            return Math.Sqrt(CentralMoment(list, 2.0));
        }

        public static double VariationCoeff(double[] list)
        {
            return StandardDeviation(list) / (list.Average());
        }
    }
}
