using KantorLr13.Models.Vectors.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr13.Models.Vectors
{
	public class Vector : Vector<double>
	{
		public Vector(int length)
		{
			items = new double[length];
		}
		public Vector(int lenght, double number)
		{
			items = new double[lenght];
			for (int i = 0; i < lenght; i++)
			{
				items[i] = number;
			}
		}
		public Vector(Vector vector)
		{
			items = new double[vector.Length];
			for (int i = 0; i < vector.Length; i++)
			{
				items[i] = vector[i];
			}
		}
		protected override Vector<double> Add(Vector<double> vector)
		{
			int size = GetMinSize(this, vector);
			Vector result = new Vector(size);
			for (int i = 0; i < size; i++)
			{
				result[i] = this[i] + vector[i];
			}
			return result;
		}
		public static Vector operator +(Vector left, Vector right) => (Vector)left.Add(right);

		protected override Vector<double> MultiplyBy(double item)
		{
			Vector result = new Vector(Length);
			for (int i = 0; i < Length; i++)
			{
				result[i] = items[i] * item;
			}
			return result;
		}
		public static Vector operator *(Vector vector, double num) => (Vector)vector.MultiplyBy(num);
		protected override Vector<double> Subtract(Vector<double> vector)
		{
			int size = GetMinSize(this, vector);
			Vector result = new Vector(size);
			for (int i = 0; i < size; i++)
			{
				result[i] = this[i] - vector[i];
			}
			return result;
		}
		public static Vector operator -(Vector left, Vector right) => (Vector)left.Subtract(right);
		public override object Clone() => new Vector(this);

		protected override Vector<double> DivideBy(double item) => MultiplyBy(1.0 / item);

		public override double EuclideanNorm()
		{
			double sum = 0;
			for (int i = 0; i < Length; i++)
			{
				sum += items[i] * items[i];
			}
			return Math.Sqrt(sum);
		}

		public static Vector operator /(Vector vector, double num) => (Vector)vector.DivideBy(num);
	}
}
