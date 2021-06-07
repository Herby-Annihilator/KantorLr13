using System;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics;
using KantorLr13.Models.Data;

namespace KantorLr13.Models.RungeKutt
{
	public class RungeKuttMethod
	{
		public List<Point>[] GetSystemSolution(Function[] derivatives, double argumentStartCondition, double argumentEndCondition, double[] functionsStartConditions, int stepsCount)
		{
			CheckArguments(derivatives, argumentStartCondition, argumentEndCondition, functionsStartConditions);
			List<Point>[] result = new List<Point>[functionsStartConditions.Length];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = new List<Point>();
				result[i].Add(new Point(argumentStartCondition, functionsStartConditions[i]));
			}
			double[] k1 = new double[derivatives.Length];
			double[] k2 = new double[derivatives.Length];
			double[] k3 = new double[derivatives.Length];
			double[] k4 = new double[derivatives.Length];
			double[] argsn = new double[functionsStartConditions.Length + 1];
			argsn[0] = argumentStartCondition;
			functionsStartConditions.CopyTo(argsn, 1);
			double h = (argumentEndCondition - argumentStartCondition) / stepsCount;
			for (int i = 0; i < stepsCount; i++)
			{
				RecalcK1(k1, h, argsn, derivatives);
				RecalcK2(k2, h, argsn, derivatives, k1);
				RecalcK3(k3, h, argsn, derivatives, k2);
				RecalcK4(k4, h, argsn, derivatives, k3);
				argsn[0] += h;
				for (int j = 1; j < argsn.Length; j++)
				{					
					argsn[j] += 1.0 / 6.0 * (k1[j - 1] + 2 * k2[j - 1] + 2 * k3[j - 1] + k4[j - 1]);
					result[j - 1].Add(new Point(argsn[0], argsn[j]));
				}				
			}
			return result;
		}

		private void CheckArguments(Function[] derivatives, double argumentStartCondition, double argumentEndCondition, double[] functionsStartConditions)
		{
			if (derivatives.Length != functionsStartConditions.Length)
				throw new ArgumentException("Несоответствие размеров массива производных и массива начальных значений функций");
			if (argumentStartCondition == argumentEndCondition)
				throw new ArgumentException("Отрезок, на котором счиаем является точкой");
			if (argumentStartCondition > argumentEndCondition)
				throw new ArgumentException("Х0 > конец отрезка");
		}

		private void RecalcK1(double[] k1, double h, double[] argsn, Function[] derivatives)
		{
			for (int i = 0; i < k1.Length; i++)
			{
				k1[i] = h * derivatives[i].calculate(argsn);
			}
		}
		private void RecalcK2(double[] k2, double h, double[] argsn, Function[] derivatives, double[] k1)
		{
			double[] args = (double[])argsn.Clone();
			args[0] += h / 2;  // x + h/2
			for (int i = 1; i < args.Length; i++)
			{
				args[i] += k1[i - 1] / 2;   // yn + k1yn/2; zn + k1zn/2 ...
			}
			for (int i = 0; i < k2.Length; i++)
			{
				k2[i] = h * derivatives[i].calculate(argsn);
			}
		}

		private void RecalcK3(double[] k3, double h, double[] argsn, Function[] derivatives, double[] k2)
		{
			double[] args = (double[])argsn.Clone();
			args[0] += h / 2;  // x + h/2
			for (int i = 1; i < args.Length; i++)
			{
				args[i] += k2[i - 1] / 2;   // yn + k2yn/2; zn + k2zn/2 ...
			}
			for (int i = 0; i < k3.Length; i++)
			{
				k3[i] = h * derivatives[i].calculate(argsn);
			}
		}
		private void RecalcK4(double[] k4, double h, double[] argsn, Function[] derivatives, double[] k3)
		{
			double[] args = (double[])argsn.Clone();
			args[0] += h;  // x + h
			for (int i = 1; i < args.Length; i++)
			{
				args[i] += k3[i - 1];   // yn + k2yn; zn + k2zn ...
			}
			for (int i = 0; i < k4.Length; i++)
			{
				k4[i] = h * derivatives[i].calculate(argsn);
			}
		}
	}

	public enum StepMode
	{
		Fixed,
		Auto
	}
}
