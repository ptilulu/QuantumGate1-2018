using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace QCS
{
    public class Qubit
    {
        public Matrix<Complex> vector;

        public Qubit(Complex a, Complex b)
        {
            this.vector = new DenseMatrix(1, 2, new Complex[] { a, b });
        }

        public override string ToString()
        {
            return vector.ToMatrixString();
        }

        public bool Equals(Qubit qubit)
        {

            return Stuff.AlmostEquals(this.vector[0, 0], qubit.vector[0, 0]) && Stuff.AlmostEquals(this.vector[0, 1], qubit.vector[0, 1]);
        }

        public override bool Equals(object o)
        {
            if (!(o is Qubit))
                return false;
            return this.Equals((Qubit)o);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static readonly Qubit Zero = new Qubit(Complex.One, Complex.Zero);
        public static readonly Qubit One = new Qubit(Complex.Zero, Complex.One);
    }
}
