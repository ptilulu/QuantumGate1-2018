using System;
using System.Collections.Generic;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

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
            double min = 1.0f;

            Debug.Log("Min : " + min);
            for (int i = 0; i < this.Vector.ColumnCount; i++)
            {
                double p = (this.Vector[0, i] * this.Vector[0, i]).Real;
                if (p > 0)
                    if (p < min)
                        min = p;
            }
            string working_string = "";
            for (int i = 0; i < this.Vector.ColumnCount; i++)
            {
                double p = (this.Vector[0, i] * this.Vector[0, i]).Real;
                if (p > 0)
                {
                    if (string.Format("{0:N2}", (this.Vector[0, i]).Real)[0].Equals('-'))
                        working_string += "-";
                    for (double j = 0.0f; j < min; j += (this.Vector[0, i] * this.Vector[0, i]).Real) // 2*min?
                    {
                        string binary_string = Convert.ToString(i, 2).PadLeft(n, '0') + ", ";
                        for (int k = 0; k < binary_string.Length; k++)
                        {
                            working_string += binary_string[k];
                        }
                    }
                }
            }
            s += working_string.Substring(0, working_string.Length - 2);

            return s;
        }

        public string ToStringWithSprites()
        {
            string s = "";
            int n = Stuff.Log2(this.Vector.ColumnCount);
            double min = 1.0f;

            Debug.Log("Min : " + min);
            for (int i = 0; i < this.Vector.ColumnCount; i++)
            {
                double p = (this.Vector[0, i] * this.Vector[0, i]).Real;
                if (p > 0)
                    if (p < min)
                        min = p;
            }
            string working_string = "";
            for (int i = 0; i < this.Vector.ColumnCount; i++)
            {
                double p = (this.Vector[0, i] * this.Vector[0, i]).Real;
                if (p > 0)
                {
                    if (string.Format("{0:N2}", (this.Vector[0, i]).Real)[0].Equals('-'))
                        working_string += "-";
                    for (double j = 0.0f; j < min; j += (this.Vector[0, i] * this.Vector[0, i]).Real) // 2*min?
                    {
                        string binary_string = Convert.ToString(i, 2).PadLeft(n, '0') + ", ";
                        for (int k = 0; k < binary_string.Length; k++)
                        {

                            if (binary_string[k].Equals('0'))
                                working_string += "<sprite=\"boule_blanche\" name=\"boule_blanche\">";
                            else if (binary_string[k].Equals('1'))
                                working_string += "<sprite=\"boule_noir\" name=\"boule_noir\">";
                            else
                                working_string += binary_string[k];
                        }
                    }
                }
            }
            s += working_string.Substring(0, working_string.Length - 2);
            /*s += "\n";
            for (int i = 0; i < this.Vector.ColumnCount; i++)
            {
                double p = (this.Vector[0, i] * this.Vector[0, i]).Real;
                if (p > 0)
                {
                    s += string.Format("{0:N2}", (this.Vector[0, i]).Real) + "   |" + Convert.ToString(i, 2).PadLeft(n, '0') + "> " + "\n";
                }
            }*/

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
