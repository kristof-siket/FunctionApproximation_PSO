using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvAlg_FunctionApproximation
{
    class FunctionApproximationSolver
    {
        static int POP_COUNT = 3000;

        static double omega = 0.9;
        static double wGlobal = 0.6;
        static double wLocal = 0.3;

        FunctionApproximationProblem problem;
        List<List<double>> population;
        List<List<double>> localOpts;
        List<double> globalOpt;
        List<List<double>> pVelos;
        static Random rnd = new Random();
        static int ITERATIONS = 2000;

        public FunctionApproximationSolver()
        {
            problem = new FunctionApproximationProblem();
            problem.loadKnownValuesFromFile("../../../Input/Input.txt");
            InitializePopulation();

            Console.WriteLine("Initial population: ");
            foreach (List<double> member  in population)
            {
                foreach (double coeff in member)
                {
                    Console.WriteLine(coeff);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Initial global opt (random): ");

            foreach (double coeff in globalOpt)
            {
                Console.WriteLine(coeff);
            }
            Console.WriteLine("Fittness: " + Fittness(globalOpt));
            Console.WriteLine();
        }

        private double Fittness(List<double> coeffs)
        {
            return problem.Objective(coeffs);
        }

        private void InitializePopulation()
        {
            this.population = new List<List<double>>();
            this.localOpts = new List<List<double>>();
            this.globalOpt = new List<double>();
            for (int i = 0; i < POP_COUNT; i++)
            {
                List<double> next = new List<double>();
                List<double> nextLocal = new List<double>();
                for (int j = 0; j < FunctionApproximationProblem.COEFF_COUNT; j++)
                {
                    double coeff = (double)rnd.Next(0, 2000) / 100;
                    double coeff_opt = (double)rnd.Next(0, 2000) / 100; // mindenkihez egy-egy random lokális optimum
                    next.Add(coeff);
                    nextLocal.Add(coeff_opt);
                }
                localOpts.Add(nextLocal);
                population.Add(next);
            }
            this.pVelos = new List<List<double>>();
            for (int j = 0; j < POP_COUNT; j++)
            {
                pVelos.Add(new List<double>());
                for (int i = 0; i < FunctionApproximationProblem.COEFF_COUNT; i++)
                {
                    int sign = rnd.Next(0,2);
                    pVelos[j].Add(sign == 0 ? 1 : -1); // mondjuk...
                }
            }

            for (int i = 0; i < FunctionApproximationProblem.COEFF_COUNT; i++) // random generált kezdeti globális optimum
            {
                double coeff_opt = (double)rnd.Next(0, 2000) / 100;
                globalOpt.Add(coeff_opt);
            }

            for (int j = 0; j < POP_COUNT; j++)
            {
                localOpts[j] = new List<double>();
                for (int i = 0; i < FunctionApproximationProblem.COEFF_COUNT; i++)
                {
                    double coeff_opt = (double)rnd.Next(0, 2000) / 100; // mindenkihez egy-egy random lokális optimum
                    localOpts[j].Add(coeff_opt);
                }
            }
        }

        private void Evaluation()
        {
            for (int i = 0; i < POP_COUNT; i++)
            {
                if (Fittness(population[i]) < Fittness(localOpts[i]))
                {
                    CopyCoeff(population[i], localOpts[i]);
                    if (Fittness(population[i]) < Fittness(globalOpt))
                    {
                        CopyCoeff(population[i], globalOpt);
                        Console.WriteLine("New global opt: ");

                        foreach (double coeff in globalOpt)
                        {
                            Console.WriteLine(coeff);
                        }
                        Console.WriteLine("Fittness: " + Fittness(globalOpt));
                        Console.WriteLine();
                    }
                }
            }
        }

        private void CopyCoeff(List<double> src, List<double> dest)
        {
            for (int i = 0; i < FunctionApproximationProblem.COEFF_COUNT; i++)
            {
                dest[i] = src[i];
            }
        }

        private void CalculateVelocity() 
        {
            for (int j = 0; j < POP_COUNT; j++)
            {
                for (int  i= 0;  i< FunctionApproximationProblem.COEFF_COUNT; i++)
                {
                    double rp = (double)rnd.Next(1,1000) / 1000;
                    double rg = (double)rnd.Next(1, 1000) / 1000; // random szorzók a képletben
                    pVelos[j][i] = (omega * pVelos[j][i] +
                        (wLocal * rp * (localOpts[j][i] - population[j][i])) +
                        (wGlobal * rg * (globalOpt[i] - population[j][i])));
                }
            }
        }

        private void MoveCoeffs()
        {
            for (int i = 0; i < POP_COUNT; i++)
            {
                for (int j = 0; j < FunctionApproximationProblem.COEFF_COUNT; j++)
                {
                    population[i][j] = population[i][j] + pVelos[i][j];
                }
            }
        }

        public void PSOptimize()
        {
            Evaluation();

            int i = 0;
            while (i < ITERATIONS)
            {
                CalculateVelocity();
                MoveCoeffs();
                Evaluation();
                i++;
            }
        }
    }
}
