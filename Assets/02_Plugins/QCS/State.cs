using System;
using System.Collections.Generic;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace QCS
{
    public class State
    {
        public class Possibility
        {
            public double p;
            public bool[] values;

            public Possibility(double p, bool[] values)
            {
                this.p = p;
                this.values = values;
            }
        }

        public Matrix<Complex> Vector
        {
            get;
        }

        public State(Matrix<Complex> vector)
        {
            if (vector.RowCount > 1 || !Stuff.IsPowerOfTwo(vector.ColumnCount))
                throw new ArgumentException("State Matrix must be 1 row and 2^n columns");

            this.Vector = vector.NormalizeRows(2.0);
        }
        public State(Complex[] arr)
        {
            this.Vector = Matrix<Complex>.Build.DenseOfRowArrays(arr).NormalizeRows(2.0);
        }
        public State(params Qubit[] trucs) : this((IEnumerable<Qubit>)trucs) { }
        public State(IEnumerable<Qubit> trucs) : this(FuncTools.Reduce1(LinearAlgebra.Kronecker, FuncTools.Map((Qubit a) => a.vector, trucs))) { }

        public IEnumerable<Possibility> EnumeratePossibilities()
        {
            int n = Stuff.Log2(this.Vector.ColumnCount);

            for (int i = 0; i < this.Vector.ColumnCount; i++)
            {
                double p = (this.Vector[0, i] * this.Vector[0, i]).Real;
                bool[] values = new bool[n];

                for (int j = 0; j < n; j++)
                    values[j] = (i & (1 << j)) == 1;

                yield return new Possibility(p, values);
            }
        }

        public override string ToString()
        {
            string s = "";
            int n = Stuff.Log2(this.Vector.ColumnCount);

            for (int i = 0; i < this.Vector.ColumnCount; i++)
            {
                double p = (this.Vector[0, i] * this.Vector[0, i]).Real;
                if (p > 0)
                    s += "|" + Convert.ToString(i, 2).PadLeft(n, '0') + "> : " + p + "\n";
            }

            return s;
        }

        public bool Equals(State state)
        {
            if (this.Vector.ColumnCount != state.Vector.ColumnCount)
                return false;

            for (int i = 0; i < this.Vector.ColumnCount; i++)
                if (!Stuff.AlmostEquals(this.Vector[0, i], state.Vector[0, i]))
                    return false;

            return true;
        }
        public override bool Equals(object o)
        {
            if (!(o is State))
                return false;
            return this.Equals((State)o);
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static State operator +(State a, Gate b) {
            return new State(LinearAlgebra.Mult(a.Vector, b.Matrix));
        }
    }
}
