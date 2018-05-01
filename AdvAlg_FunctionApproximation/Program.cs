using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvAlg_FunctionApproximation
{
    class Program
    {
        static void Main(string[] args)
        {
            FunctionApproximationSolver solver = new FunctionApproximationSolver();
            Console.ReadLine();
            solver.PSOptimize();
            Console.WriteLine("Optimization is over!");
            Console.ReadLine();
        }
    }
}
