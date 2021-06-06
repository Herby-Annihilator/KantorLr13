using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics;
using KantorLr13.Models.Data;

namespace KantorLr13.Models.RungeKutt
{
	public class RungeKuttMethod
	{
		public List<Point>[] GetSystemSolution(Func<double, double, double>[] derivatives, double argumentStartCondition, double argumentEndCondition, double[] functionsStartConditions, int stepsCount)
		{
			CheckArguments(derivatives, argumentStartCondition, argumentEndCondition, functionsStartConditions);
			List<Point>[] result = new List<Point>[functionsStartConditions.Length];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = new List<Point>();
				result[i].Add(new Point(argumentStartCondition, functionsStartConditions[i]));
			}
			double k1, k2, k3, k4, currentX, currentY, nextY;
			double h = (argumentEndCondition - argumentStartCondition) / stepsCount;
			for (int i = 0; i < stepsCount; i++)
			{
				for (int j = 0; j < result.Length; j++)
				{
					currentY = result[j][i].Y;
					currentX = result[j][i].X;
					k1 = h * derivatives[j].Invoke(currentX, currentY);
					k2 = h * derivatives[j].Invoke(currentX + h / 2, currentY + k1 / 2);
					k3 = h * derivatives[j].Invoke(currentX + h / 2, currentY + k2 / 2);
					k4 = h * derivatives[j].Invoke(currentX + h, currentY + k3);
					nextY = currentY + 1 / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
					result[j].Add(new Point(currentX + h, nextY));
				}
			}
			return result;
		}

		private void CheckArguments(Func<double, double, double>[] derivatives, double argumentStartCondition, double argumentEndCondition, double[] functionsStartConditions)
		{
			if (derivatives.Length != functionsStartConditions.Length)
				throw new ArgumentException("Несоответствие размеров массива производных и массива начальных значений функций");
			if (argumentStartCondition == argumentEndCondition)
				throw new ArgumentException("Отрезок, на котором счиаем является точкой");
			if (argumentStartCondition > argumentEndCondition)
				throw new ArgumentException("Х0 > конец отрезка");
		}
	}

	public enum StepMode
	{
		Fixed,
		Auto
	}
}
