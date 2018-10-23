using System;
using System.Collections.Generic;
using System.Numerics;

using MathNet.Numerics.LinearAlgebra;

namespace QCS
{
    public class Stuff
    {
        public static double EPSILON = 0.000001f;

        public static bool IsPowerOfTwo(int x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        public static int Log2(int v)
        {
            int r = 0xFFFF - v >> 31 & 0x10;
            v >>= r;
            int shift = 0xFF - v >> 31 & 0x8;
            v >>= shift;
            r |= shift;
            shift = 0xF - v >> 31 & 0x4;
            v >>= shift;
            r |= shift;
            shift = 0x3 - v >> 31 & 0x2;
            v >>= shift;
            r |= shift;
            r |= (v >> 1);
            return r;
        }
        
        public static bool AlmostEquals(Complex a, Complex b)
        {
            return Complex.Abs(a - b) < EPSILON;
        }
    }

    public class LinearAlgebra
    {
        public static Matrix<Complex> Kronecker(Matrix<Complex> m1, Matrix<Complex> m2) { return m1.KroneckerProduct(m2); }
        public static Matrix<Complex> Kronecker(params Matrix<Complex>[] t) { return Kronecker((IEnumerable<Matrix<Complex>>)t); }
        public static Matrix<Complex> Kronecker(IEnumerable<Matrix<Complex>> t) { return FuncTools.Reduce1(Kronecker, t); }

        public static Matrix<Complex> Mult(Matrix<Complex> m1, Matrix<Complex> m2) { return m1 * m2; }
        public static Matrix<Complex> Mult(params Matrix<Complex>[] t) { return Mult((IEnumerable<Matrix<Complex>>)t); }
        public static Matrix<Complex> Mult(IEnumerable<Matrix<Complex>> t) { return FuncTools.Reduce1(Mult, t); }
    }

    public class FuncTools
    {
        public static IEnumerable<T> Take<T>(int n, IEnumerable<T> list)
        {
            IEnumerator<T> iterator = list.GetEnumerator();
            for (int i = 0; i < n && iterator.MoveNext(); i++)
                yield return iterator.Current;
        }

        public static IEnumerable<TResult> Map<T, TResult>(Func<T, TResult> func, IEnumerable<T> list)
        {
            foreach (var i in list)
                yield return func(i);
        }

        public static T Reduce<T, U>(T acc, Func<T, U, T> func, IEnumerable<U> list)
        {
            foreach (var i in list)
                acc = func(acc, i);

            return acc;
        }

        public static T Reduce1<T>(Func<T, T, T> func, IEnumerable<T> list)
        {
            IEnumerator<T> iterator = list.GetEnumerator();

            if (!iterator.MoveNext())
                throw new ArgumentException("Trying to Reduce1 on an empty IEnumerable :(");

            T acc = iterator.Current;

            while (iterator.MoveNext())
                acc = func(acc, iterator.Current);

            return acc;
        }

        public static int Sum(IEnumerable<int> list)
        {
            return Reduce1((int a, int b) => a + b, list);
        }

        public static bool AllEquals<T>(T value, IEnumerable<T> list) {
            IEnumerator<T> iterator = list.GetEnumerator();

            while (iterator.MoveNext())
                if (! value.Equals(iterator.Current))
                    return false;

            return true;
        }
        public static bool AllEquals<T>(IEnumerable<T> list)
        {
            IEnumerator<T> iterator = list.GetEnumerator();

            if (!iterator.MoveNext())
                return false;

            T value = iterator.Current;

            while (iterator.MoveNext())
                if (!value.Equals(iterator.Current))
                    return false;

            return true;
        }
    }
}