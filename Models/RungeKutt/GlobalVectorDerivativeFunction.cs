using KantorLr13.Models.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr13.Models.RungeKutt
{
	public class GlobalVectorDerivativeFunction
	{
		public DerivativeFunction[] DerivativeFunctions { get; set; }

		public Vector Calculate(double x, Vector derivativeArgs)
		{
			Vector result = new Vector(DerivativeFunctions.Length);
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = DerivativeFunctions[i].Calculate(x, derivativeArgs);
			}
			return result;
		}
	}
}
