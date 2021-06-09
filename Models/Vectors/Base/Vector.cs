using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr13.Models.Vectors.Base
{
	public abstract class Vector<T> : ICloneable
	{
		public int Length { get => items.Length; }
		protected T[] items;
		protected abstract Vector<T> Add(Vector<T> vector);
		public static Vector<T> operator +(Vector<T> left, Vector<T> right) => left.Add(right);

		protected abstract Vector<T> Subtract(Vector<T> vector);
		public static Vector<T> operator -(Vector<T> left, Vector<T> right) => left.Subtract(right);

		protected abstract Vector<T> MultiplyBy(T item);

		public static Vector<T> operator *(Vector<T> vector, T item) => vector.MultiplyBy(item);

		protected abstract Vector<T> DivideBy(T item);

		public static Vector<T> operator /(Vector<T> vector, T item) => vector.DivideBy(item);

		public virtual T this[int index] { get => items[index]; set => items[index] = value; }

		protected virtual int GetMinSize(Vector<T> first, Vector<T> second)
		{
			int result;
			if (first.Length < second.Length)
				result = first.Length;
			else
				result = second.Length;
			return result;
		}

		public abstract object Clone();
	}
}
