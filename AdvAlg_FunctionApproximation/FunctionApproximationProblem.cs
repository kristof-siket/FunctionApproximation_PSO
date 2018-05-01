using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvAlg_FunctionApproximation
{
    public class ValuePair
    {
        public double Input { get; set; }
        public double Output { get; set; }
    }

    public class FunctionApproximationProblem
    {
        public static int COEFF_COUNT = 5;

        private List<ValuePair> knownValues;

        public FunctionApproximationProblem()
        {
            this.knownValues = new List<ValuePair>();
        }

        public List<ValuePair> KnownValues { get { return knownValues; } private set { knownValues = value; } }

        //OBJECTIVE
        public double Objective(List<double> coefficients)
        {
            double sum_diff = 0;
            foreach (ValuePair pair in knownValues)
            {
                double x = pair.Input;
                double y = coefficients[0] * Math.Pow(x - coefficients[1], 3) +
                    coefficients[2] * Math.Pow(x - coefficients[3], 2) + coefficients[4];

                double diff = (double)Math.Pow(y - pair.Output, 2);
                sum_diff += diff;
            }
            return sum_diff;
        }


        //IO
        public void loadKnownValuesFromFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                string[] values = line.Split('\t');
                ValuePair vp = new ValuePair() { Input = double.Parse(values[0], CultureInfo.InvariantCulture), Output = double.Parse(values[1], CultureInfo.InvariantCulture) };
                knownValues.Add(vp);
            }
        }
    }
}
