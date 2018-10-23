using System.Collections.Generic;
using System;
using SerializableClass;

/// <summary>
/// Cette classe correspond à une partie de jeu bac à sable de l'utilisateur
/// Classe serialisable
/// </summary>
[Serializable]
public class SandBoxSession
{
    /// <summary>
    /// Sous classe de SandBoxSession, cette classe permet de construire facilement
    /// et proprement une instance de SandBoxSession.
    /// Celle-ci utilise le pattern Builder
    /// </summary>
	public class Builder
	{
        /// <summary>
        /// Nom de la session de jeu
        /// </summary>
		private string _nameSave;

        /// <summary>
        /// Liste des circuits de la session
        /// </summary>
		private List<Circuit> _circuits;

        /// <summary>
        /// Liste des portes customs de la session
        /// </summary>
		private List<Gate> _customGates;

        /// <summary>
        ///  Instancie le Buider de SandBoxSession.
        /// </summary>
        /// <param name="nameSave">Nom de la session</param>
		public Builder(string nameSave)
		{
			this._nameSave = nameSave;

			_circuits = new List<Circuit>();
			_customGates = new List<Gate>();
		}

        /// <summary>
        /// Ajoute un circuit de la librairie QCS.
        /// Celui-ci est transformé pour devenir Serialisable
        /// </summary>
        /// <param name="circuit">Circuit à ajouter</param>
        /// <returns></returns>
		public Builder AddCircuit(QCS.Circuit circuit)
		{
			Circuit circuitSerialisable = SerializableClass.Helper.ToCircuitSerialisable (circuit);
			_circuits.Add (circuitSerialisable);

			return this;
		}

        /// <summary>
        /// Ajoute une liste de circuit de la librairie QCS
        /// Ceux-ci est transformés pour devenir Serialisable
        /// </summary>
        /// <param name="circuits">Liste des circuits</param>
        /// <returns></returns>
		public Builder AddCircuits(List<QCS.Circuit> circuits)
		{
			foreach (QCS.Circuit circuit in circuits)
				AddCircuit (circuit);
			
			return this;
		}

        /// <summary>
        ///  Ajoute une porte custom de la librairie QCS
        ///  Celle-ci est transformée pour devenir Serialisable
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
		public Builder AddCustomGate(QCS.Gate gate)
		{
			Gate gateSerialisable = SerializableClass.Helper.ToGateSerialisable (gate);
			_customGates.Add (gateSerialisable);

			return this;
		}

        /// <summary>
        /// Ajoute une liste de porte custom de la librairie QCS
        /// sous la forme d'un dictionnaire
        /// Celui-ci est transformé pour devenir Serialisable
        /// </summary>
        /// <param name="gatesDictionnary">Dictionnaire de portes custom</param>
        /// <returns></returns>
		public Builder AddCustomGates(Dictionary<string, QCS.Gate> gatesDictionnary)
		{
			foreach (QCS.Gate gate in gatesDictionnary.Values)
				AddCustomGate (gate);
			
			return this;
		}
						
        /// <summary>
        /// Construit une instance de SandBoxSession
        /// </summary>
        /// <returns></returns>
		public SandBoxSession Build()
		{
			SandBoxSession sandBoxSession = new SandBoxSession ();

			sandBoxSession.nameSave = _nameSave;

			sandBoxSession.circuits = _circuits;

			sandBoxSession.customGates = _customGates;

			return sandBoxSession;
		}
	}

	public SandBoxSession()
	{
		circuits = new List<Circuit>();
		customGates = new List<Gate>();
	}

    /// <summary>
    /// Nom de la session
    /// </summary>
	public string nameSave;

	/// <summary>
	///  Les circuits utilisé lors de cette session
	/// </summary>
	public List<Circuit> circuits;

	/// <summary>
	///  Les portes custom créé
	/// </summary>
	public List<Gate> customGates;
}


