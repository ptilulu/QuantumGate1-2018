using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
///  Module permettant de convertir les structures de données : 
///  Circuit, Gate, Qubit en structure de données serialisables
/// </summary>
namespace SerializableClass
{

    /// <summary>
    ///  Classe alternative de la classe Matrix du module MathNet.Numerics.LinearAlgebra
    ///  pour la serialisation
    /// </summary>
    [Serializable]
    public class Matrix
    {
        [Serializable]
        public class RowM
        {
            public List<Complex> cols;

            public RowM()
            {
                cols = new List<Complex>();
            }
        }

        public List<RowM> rows;

        public Matrix()
        {
            rows = new List<RowM>();
        }
    }

    /// <summary>
    ///  Classe alternative de la classe Complex du module System.Numerics
    ///  pour la serialisation
    /// </summary>
    [Serializable]
    public class Complex
    {
        public double imaginary;

        public double real;


        public Complex() { }
    }

    /// <summary>
    /// Classe alternative de la classe Gate du module QCS
    /// pour la serialisation
    /// </summary>
    [Serializable]
    public class Gate
    {
        public string name;

        public int nbEntries;

        public Matrix matrix;

        public Gate() { }
    }

    /// <summary>
    /// Classe alternative de la classe Qubit du module QCS
    /// pour la serialisation
    /// </summary>
    [Serializable]
    public class Qubit
    {
        public Matrix vector;

        public Qubit() { }
    }

    /// <summary>
    /// Classe alternative de la classe Circuit du module QCS
    /// pour la serialisation
    /// </summary>
    [Serializable]
    public class Circuit
    {
        /// <summary>
        /// Classe alternative de la classe Row du module QCS
        /// pour la serialisation
        /// </summary>
        [Serializable]
        public class Row
        {
            public List<string> cols;

            public Row()
            {
                cols = new List<string>();
            }
        }

        public List<Qubit> entries;
        public List<Row> rows;

        public Circuit()
        {
            entries = new List<Qubit>();
            rows = new List<Row>();
        }
    }

    /// <summary>
    ///  Classe permettant l'aide à la serialisation / deserialisation de la structure de donnée circuit de la librairie QCS.
    /// </summary>
    public class Helper
    {

		/**
		 * ######################################################################################################
		 * ############################################ CIRCUIT #################################################
		 * ######################################################################################################
		 */
		
        /// <summary>
        /// Transforme un circuit de la libraire QCS en un circuit du module SerializableClass, permettant ça serialisation
        /// </summary>
        public static Circuit ToCircuitSerialisable(QCS.Circuit circuit)
        {
            Circuit circuitSerialised = new Circuit();

            foreach(QCS.Circuit.EntryStruct entryStruct in circuit.entries)
            {
                Qubit qubitSerialised = ToQubitSerialisable(entryStruct.qubit);
           
                circuitSerialised.entries.Add(qubitSerialised);
            }

            foreach (List<QCS.Circuit.GateStruct> rows in circuit.rows)
            {
                circuitSerialised.rows.Add(new Circuit.Row());

                foreach (QCS.Circuit.GateStruct gate in rows)
                {
                    circuitSerialised.rows.Last().cols.Add(gate.gate.Name);
                }
            }

            return circuitSerialised;
        }


        /// <summary>
        /// Transforme un circuit du module SerialisableClass en un circuit de la librairie QCS afin de pouvoir l'utiliser correctement.
        /// Celui-ci a besoin, pour être transformer, de la liste des portes par défauts, et des portes customs représenté sous la forme d'un dictionnaire.
        /// </summary>
        public static QCS.Circuit ToUsableCircuit(Circuit circuitSerialised, Dictionary<string, QCS.Gate> defaultGates, Dictionary<string, QCS.Gate> customGates)
        {
			QCS.Circuit circuitUsuable = new QCS.Circuit ();

			int nbCols = circuitSerialised.entries.Count;
			int nbRows = circuitSerialised.rows.Count;


			for (int indexEntry = 0 ; indexEntry < nbCols; indexEntry++) 
			{
				QCS.Qubit usuableQubit = ToUsableQubit (circuitSerialised.entries [indexEntry]);

				QCS.Circuit.EntryStruct entryStruct = new QCS.Circuit.EntryStruct (indexEntry, usuableQubit);

				circuitUsuable.entries.Add(entryStruct);
			}

			for (int indexRow = 0 ; indexRow < nbRows ; indexRow++) 
			{
				circuitUsuable.rows.Add (new List<QCS.Circuit.GateStruct> ());

				QCS.Gate gateUsuable;

				for (int indexCol = 0 ; indexCol < nbCols ; indexCol += gateUsuable.NbEntries) 
				{
					string nameGate = circuitSerialised.rows [indexRow].cols [indexCol];

					if (defaultGates.ContainsKey (nameGate))
						gateUsuable = defaultGates [nameGate];

					else if (customGates.ContainsKey (nameGate))
						gateUsuable = customGates [nameGate];

					else 
						gateUsuable = QCS.Gate.IDENTITY;


					QCS.Circuit.GateStruct gateStruct = new QCS.Circuit.GateStruct (indexRow, indexCol, gateUsuable);

					for(int indexEntry = 0 ; indexEntry < gateUsuable.NbEntries ; indexEntry++)
						circuitUsuable.rows [indexRow].Add (gateStruct);
				}
			}


			return circuitUsuable;
        }


        /// <summary>
        /// Transforme un circuit du module SerialisableClass en un circuit de la librairie QCS afin de pouvoir l'utiliser correctement.
        /// Celui-ci a besoin, pour être transformer, de la liste des portes par défauts, et des portes customs.
        /// </summary>
        public static QCS.Circuit ToUsableCircuit(Circuit circuitSerialised, List<QCS.Gate> defaultGates, List<QCS.Gate> customGates)
		{
			Dictionary<string, QCS.Gate> defaultGatesD = new Dictionary<string, QCS.Gate>();
			Dictionary<string, QCS.Gate> customGatesD = new Dictionary<string, QCS.Gate>();

			foreach (QCS.Gate gate in defaultGates)
				defaultGatesD.Add (gate.Name, gate);


			foreach (QCS.Gate gate in customGates)
				customGatesD.Add (gate.Name, gate);
			

			return ToUsableCircuit(circuitSerialised, defaultGatesD, customGatesD);
		}


        /// <summary>
        /// Transforme une liste de circuit du module SerialisableClass en une liste de circuit de la librairie QCS afin de pouvoir les utiliser correctement.
        /// Ceux-ci ont besoin, pour être transformer, de la liste des portes par défauts, et des portes customs représenté sous la forme d'un dictionnaire.
        /// </summary>
        public static List<QCS.Circuit> ToUsableCircuits(List<Circuit> circuitsSerialised, Dictionary<string, QCS.Gate> defaultGates, Dictionary<string, QCS.Gate> customGates)
		{
			List<QCS.Circuit> usuableCircuits = new List<QCS.Circuit> ();

			foreach (Circuit circuitSerialised in circuitsSerialised) 
				usuableCircuits.Add(ToUsableCircuit(circuitSerialised, defaultGates, customGates));

			return usuableCircuits;
		}


        /// <summary>
        /// Transforme une liste de circuit du module SerialisableClass en une liste de circuit de la librairie QCS afin de pouvoir les utiliser correctement.
        /// Ceux-ci ont besoin, pour être transformer, de la liste des portes par défauts, et des portes customs.
        /// </summary>
        public static List<QCS.Circuit> ToUsableCircuits(List<Circuit> circuitsSerialised, List<QCS.Gate> defaultGates, List<QCS.Gate> customGates)
		{
			List<QCS.Circuit> usuableCircuits = new List<QCS.Circuit> ();

			foreach (Circuit circuitSerialised in circuitsSerialised) 
				usuableCircuits.Add(ToUsableCircuit(circuitSerialised, defaultGates, customGates));

			return usuableCircuits;
		}

        /**
		 * ######################################################################################################
		 * ############################################## QUBIT #################################################
		 * ######################################################################################################
		 */


        /// <summary>
        /// Transforme un qubit de la libraire QCS en un qubit du module SerializableClass, permettant ça serialisation
        /// </summary>
        private static Qubit ToQubitSerialisable(QCS.Qubit qubit)
        {
            Qubit qubitSerialisable = new Qubit();
            qubitSerialisable.vector = ToMatrixSerialisable(qubit.vector);

            return qubitSerialisable;
        }


        /// <summary>
        /// Transforme un qubit du module SerializableClass en un qubit de la librairie QCS afin de pouvoir l'utiliser
        /// </summary>
        private static QCS.Qubit ToUsableQubit(Qubit qubit)
		{
			Complex a = qubit.vector.rows [0].cols [0];
			Complex b = qubit.vector.rows [0].cols [1];

			return new QCS.Qubit (ToUsuableComplexNumber(a), ToUsuableComplexNumber(b));;
		}

        /**
		 * ######################################################################################################
		 * ############################################## GATE ##################################################
		 * ######################################################################################################
		 */


        /// <summary>
        /// Transforme une porte de la libraire QCS en une porte du module SerializableClass, permettant ça serialisation
        /// </summary>
        public static Gate ToGateSerialisable(QCS.Gate gate)
        {
            Gate gateSerialisable = new Gate();

            gateSerialisable.name = gate.Name;
            gateSerialisable.nbEntries = gate.NbEntries;
            gateSerialisable.matrix = ToMatrixSerialisable(gate.Matrix);
           
            return gateSerialisable;
        }


        /// <summary>
        /// Transforme une porte du module SerializableClass en une porte de la librairie QCS afin de pouvoir l'utiliser
        /// </summary>
        public static QCS.Gate ToUsableGate(Gate gate)
		{
			Matrix<System.Numerics.Complex> usuableMatrix = ToUsableMatrix (gate.matrix);

			QCS.Gate gateUsable = new QCS.Gate (gate.name, gate.nbEntries, usuableMatrix);

			return gateUsable;
		}


        /// <summary>
        /// Transforme une liste de porte du module SerializableClass en une liste de porte de la librairie QCS afin de pouvoir les utiliser
        /// </summary>
        public static List<QCS.Gate> ToUsableGates(List<Gate> gatesSerialised)
		{
			List<QCS.Gate> gates = new List<QCS.Gate> ();

			foreach (Gate gateSerialised in gatesSerialised) 
				gates.Add (ToUsableGate (gateSerialised));


			return gates;
		}


        /// <summary>
        /// Transforme une liste de porte du module SerializableClass en un dictionnaire de porte de la librairie QCS afin de pouvoir les utiliser
        /// </summary>
        public static Dictionary<string, QCS.Gate> ToUsableDictionnaryGates(List<Gate> gatesSerialised)
		{
			Dictionary<string, QCS.Gate> dictionnaryGates = new Dictionary<string, QCS.Gate> ();

			foreach (Gate gateSerialised in gatesSerialised) 
				dictionnaryGates.Add (gateSerialised.name, ToUsableGate (gateSerialised));
			

			return dictionnaryGates;
		}

        /**
		 * ######################################################################################################
		 * ######################################### COMPLEX NUMBER #############################################
		 * ######################################################################################################
		 */


        /// <summary>
        /// Transforme un complex du module System.Numerics en un complex du module SerializableClass, permettant ça serialisation
        /// </summary>
        private static Complex ToComplexSerialisable(System.Numerics.Complex complex)
        {
            Complex complexSerialised = new Complex();

            complexSerialised.imaginary = complex.Imaginary;
            complexSerialised.real = complex.Real;

            return complexSerialised;
        }


        /// <summary>
        /// Transforme un nombre complex du module SerializableClass en un nombre complex du module System.Numerics afin de pouvoir l'utiliser
        /// </summary>
        public static System.Numerics.Complex ToUsuableComplexNumber(Complex complexSerialised)
        {
            return new System.Numerics.Complex(complexSerialised.real, complexSerialised.imaginary);
        }

        /**
		 * ######################################################################################################
		 * ############################################ MATRIX ##################################################
		 * ######################################################################################################
		 */


        /// <summary>
        /// Transforme une matrice du module MathNet.Numerics.LinearAlgebra en une matrice du module SerializableClass, permettant ça serialisation
        /// </summary>
        private static Matrix ToMatrixSerialisable(Matrix<System.Numerics.Complex> matrix)
        {
            Matrix matrixSerialisable = new Matrix();

            for(int indexRow = 0; indexRow < matrix.RowCount; indexRow++)
            {
                Vector<System.Numerics.Complex> row = matrix.Row(indexRow);

                matrixSerialisable.rows.Add(new Matrix.RowM());

                for (int indexCol = 0; indexCol < row.Count; indexCol++)
                {
                    Complex complexSerialised = ToComplexSerialisable(row[indexCol]);

                    matrixSerialisable.rows[indexRow].cols.Add(complexSerialised);
                }   
            }

            return matrixSerialisable;
        }


        /// <summary>
        /// Transforme une matrice du module SerializableClass en une matrice du module MathNet.Numerics.LinearAlgebra afin de pouvoir l'utiliser
        /// </summary>
        public static Matrix<System.Numerics.Complex> ToUsableMatrix(Matrix matrixSerialised)
        {
			int nbRows = matrixSerialised.rows.Count;
			int nbCols = matrixSerialised.rows [0].cols.Count;

			System.Numerics.Complex[,] complexArray = new System.Numerics.Complex[nbRows, nbCols];

			for (int indexRow = 0; indexRow < nbRows; indexRow++) 
			{
				Matrix.RowM row = matrixSerialised.rows [indexRow];

				for (int indexCol = 0; indexCol < nbCols; indexCol++)
					complexArray [indexRow, indexCol] = ToUsuableComplexNumber (row.cols [indexCol]);
				
			}
				
			return Matrix<System.Numerics.Complex>.Build.SparseOfArray(complexArray).NormalizeRows(2.0);
        }

    }
}
