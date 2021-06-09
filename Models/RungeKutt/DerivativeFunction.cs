using KantorLr13.Models.Vectors;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr13.Models.RungeKutt
{
	public class DerivativeFunction
	{
		public string FunctionName { get; set; }
		public string Expression { get; set; }

		public DerivativeFunction(string functionName, string expression)
		{
			FunctionName = functionName;
			Expression = expression;
		}

		public double Calculate(double x, Vector derivativeArgs)
		{
			double[] args = new double[derivativeArgs.Length + 1];
			args[0] = x;
			for (int i = 1; i < args.Length; i++)
			{
				args[i] = derivativeArgs[i - 1];
			}
			Function function = new Function($"{FunctionName} = {Expression.Replace(',', '.')}");
			return function.calculate(args);
		}
	}
}
