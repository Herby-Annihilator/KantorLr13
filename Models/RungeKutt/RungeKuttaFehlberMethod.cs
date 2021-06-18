using KantorLr13.Models.Data;
using KantorLr13.Models.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr13.Models.RungeKutt
{
	public class RungeKuttaFehlberMethod
	{
		private double[] _A = { 0.0,
			1.0 / 4.0,
			3.0 / 8.0,
			12.0 / 13.0,
			1.0,
			1.0 / 2.0 };
		private double[][] _B = { 
			new double[] { 1.0 / 4.0},
			new double[] { 3.0 / 32.0, 9.0 / 32.0},
			new double[] { 1932.0 / 2197.0, -7200.0 / 2197.0, 7296.0 / 2197.0 },
			new double[] { 439.0 / 216.0, -8.0, 3680.0 / 513.0, -845.0 / 4104.0 },
			new double[] { -8.0 / 27.0, 2.0, -3544.0 / 2565.0, 1859.0 / 4104.0, -11.0 / 40.0 }
		};
		private double[] _CH = { 16.0 / 135.0,
			0.0,
			6656.0 / 12825.0,
			28561.0 / 56430.0,
			-9.0 / 50.0,
			2.0 / 55.0
		};
		private double[] _CT = {
			1.0 / 360.0,
			0.0,
			-128.0 / 4275.0,
			-2197.0 / 75240.0,
			1.0 / 50.0,
			2.0 / 55.0
		};
		public List<Point>[] GetSystemSolution(GlobalVectorDerivativeFunction f, double argumentStartCondition, double argumentEndCondition, Vector functionsStartConditions, double precision)
		{
			List<Point>[] result = new List<Point>[functionsStartConditions.Length];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = new List<Point>();
				result[i].Add(new Point(argumentStartCondition, functionsStartConditions[i]));
			}
			Vector k1, k2, k3, k4, k5, k6, TE;
			double length = argumentEndCondition - argumentStartCondition;
			double hmin = length / 20000.0;
			double hmax = length / 5.0;
			double h = length / 100.0;
			double x = argumentStartCondition;
			double norm;
			Vector y = new Vector(functionsStartConditions);
			while (x < argumentEndCondition)
			{
				k1 = f.Calculate(x + _A[0] * h, y) * h;
				k2 = f.Calculate(x + _A[1] * h, y + k1 * _B[0][0]) * h;
				k3 = f.Calculate(x + _A[2] * h, y + k1 * _B[1][0] + k2 * _B[1][1]) * h;
				k4 = f.Calculate(x + _A[3] * h, y + k1 * _B[2][0] + k2 * _B[2][1] + k3 * _B[2][2]) * h;
				k5 = f.Calculate(x + _A[4] * h, y + k1 * _B[3][0] + k2 * _B[3][1] + k3 * _B[3][2] + k4 * _B[3][3]) * h;
				k6 = f.Calculate(x + _A[5] * h, y + k1 * _B[4][0] + k2 * _B[4][1] + k3 * _B[4][2] + k4 * _B[4][3] + k5 * _B[4][4]) * h;				

				TE = k1 * _CT[0] + k2 * _CT[1] + k3 * _CT[2] + k4 * _CT[3] + k5 * _CT[4] + k6 * _CT[5];
				norm = TE.EuclideanNorm();
				h = 0.9 * h * Math.Pow(precision / norm, 1.0 / 5.0);
				if (norm <= precision)
				{
					y += k1 * _CH[0] + k2 * _CH[1] + k3 * _CH[2] + k4 * _CH[3] + k5 * _CH[4] + k6 * _CH[5];
					for (int j = 0; j < result.Length; j++)
					{
						result[j].Add(new Point(x, y[j]));
					}
					x += h;
				}
			}
			return result;
		}
	}
}
