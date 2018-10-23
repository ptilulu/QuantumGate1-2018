using System;
using System.Collections.Generic;
using System.Numerics;

using MathNet.Numerics.LinearAlgebra;

namespace QCS
{
    public class Gate
    {
        public string Name
        {
            get;
            private set;
        }

        public int NbEntries
        {
            get;
            private set;
        }

        public Matrix<Complex> Matrix
        {
            get;
            private set;
        }

        public Gate(string name, int nb_entries, Matrix<Complex> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount || !Stuff.IsPowerOfTwo(matrix.ColumnCount))
                throw new ArgumentException("Gate Matrix must be 2^n rows and 2^n columns");

            // put identity on zeroed rows
            MathNet.Numerics.LinearAlgebra.Vector<Complex> row_absums = matrix.RowAbsoluteSums();
            for (int i = 0; i < matrix.RowCount; i++)
            {
                if (row_absums[i] == Complex.Zero)
                    matrix[i, i] = Complex.One;
            }

            this.NbEntries = nb_entries;
            this.Matrix = matrix.NormalizeRows(2.0);

            this.Name = name;
        }

		/// <summary>
		///  Constructeur à utiliser uniquement dans les contextes suivants :
		///  	- Création de portes statiques
		///     - Désérialisation de porte
		/// </summary>
		/// <param name="name"> Nom de la porte </param>
		/// <param name="nb_entries"> Nombre d'entrées de la porte</param>
		/// <param name="arr"> Matrice sous la forme d'un tableau à deux dimensions de la porte</param>
        public Gate(string name, int nb_entries, Complex[,] arr)
        {
            this.NbEntries = nb_entries;
            this.Matrix = Matrix<Complex>.Build.SparseOfArray(arr).NormalizeRows(2.0);

            this.Name = name;
        }

        public Gate(string name, IEnumerable<Gate> gates) : this(name, FuncTools.Sum(FuncTools.Map((Gate a) => a.NbEntries, gates)), FuncTools.Reduce1(LinearAlgebra.Kronecker, FuncTools.Map((Gate a) => a.Matrix, gates))) { }
        public Gate(string name, params Gate[] gates) : this(name, (IEnumerable<Gate>)gates) { }
        public Gate(IEnumerable<Gate> gates) : this(String.Join("*", FuncTools.Map((Gate gate) => gate.Name, gates)), (IEnumerable<Gate>)gates) { }
        public Gate(params Gate[] gates) : this(String.Join("*", FuncTools.Map((Gate gate) => gate.Name, gates)), (IEnumerable<Gate>)gates) { }

        public override string ToString()
        {
            return this.Name + this.Matrix.ToMatrixString();
        }

        public bool Equals(Gate gate)
        {
            if (this.NbEntries != gate.NbEntries)
                return false;

            for (int i = 0; i < this.Matrix.RowCount; i++)
                for (int j = 0; j < this.Matrix.ColumnCount; j++)
                    if (!Stuff.AlmostEquals(this.Matrix[i, j], gate.Matrix[i, j]))
                        return false;

            return true;
        }
        
        public override bool Equals(object o)
        {
            if (!(o is Gate))
                return false;
            return this.Equals((Gate)o);
        }

        public override int GetHashCode() { return this.ToString().GetHashCode(); }

        public static Gate Kron(IEnumerable<Gate> gates) { return new Gate(gates); }
        public static Gate Kron(params Gate[] gates) { return Kron((IEnumerable<Gate>) gates); }
        public static Gate operator *(Gate a, Gate b) { return Kron(a, b); }

        public static Gate Add(IEnumerable<Gate> gates)
        {
            IEnumerator<Gate> iterator = gates.GetEnumerator();

            if (!iterator.MoveNext())
                return null;

            int nbEntriesRef = iterator.Current.NbEntries;

            while (iterator.MoveNext())
                if (iterator.Current.NbEntries != nbEntriesRef)
                    throw new ArgumentException("cannot add gates of different lengths");                

            return new Gate("(" + String.Join("+", FuncTools.Map((Gate gate) => gate.Name, gates)) + ")", nbEntriesRef, LinearAlgebra.Mult(FuncTools.Map((Gate a) => a.Matrix, gates)));
        }
        public static Gate Add(params Gate[] gates) { return Add((IEnumerable<Gate>)gates); }
        public static Gate operator +(Gate a, Gate b)
        {
            return Add(a, b);
        }


        public static readonly Gate CONTROL = new Gate("•", 1, new Complex[,] {
            { Complex.Zero, Complex.Zero },
            { Complex.Zero, Complex.One }
        });

        public static readonly Gate IDENTITY = new Gate("1", 1, new Complex[,] {
            { Complex.One,  Complex.Zero },
            { Complex.Zero, Complex.One }
        });

        public static readonly Gate NOT = new Gate("X", 1, new Complex[,] {
            { Complex.Zero, Complex.One },
            { Complex.One,  Complex.Zero }
        });

        public static readonly Gate SWAP = new Gate("SWAP", 2, new Complex[,] {
            { Complex.One,  Complex.Zero,  Complex.Zero,  Complex.Zero },
            { Complex.Zero,  Complex.Zero,  Complex.One,  Complex.Zero },
            { Complex.Zero,  Complex.One,  Complex.Zero,  Complex.Zero },
            { Complex.Zero,  Complex.Zero,  Complex.Zero,  Complex.One }
        });

        public static readonly Gate HADAMARD = new Gate("H", 1, new Complex[,] {
            { Complex.One,  Complex.One },
            { Complex.One,  -Complex.One }
        });
    }
}
