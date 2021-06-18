﻿using KantorLr13.Models.Vectors;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr13.Models.RungeKutt
{
	public class DerivativeFunction
	{
		public string FunctionName { get; private set; }
		public string Expression { get; private set; }
		private Function _function;

		public DerivativeFunction(string functionName, string expression)
		{
			FunctionName = functionName;
			Expression = expression;
			_function = new Function($"{FunctionName} = {Expression.Replace(',', '.')}");
		}

		public void Refresh(string functionName, string expression)
		{
			FunctionName = functionName;
			Expression = expression;
			_function = new Function($"{FunctionName} = {Expression.Replace(',', '.')}");
		}

		public double Calculate(double x, Vector derivativeArgs)
		{
			double[] args = new double[derivativeArgs.Length + 1];
			args[0] = x;
			for (int i = 1; i < args.Length; i++)
			{
				args[i] = derivativeArgs[i - 1];
			}
			
			return _function.calculate(args);
		}
	}
}