using System.Collections.Generic;
using System;
using UnityEngine;


/// <summary>
///  Classe regroupant toutes les données utilisateurs.
///  Permet la persistance des données.
/// </summary>
public class UserData
{
    /// <summary>
    /// Sous classe de UserData, cette classe permet de construire facilement
    /// et proprement une instance de UserData.
    /// Celle-ci utilise le pattern Builder
    /// </summary>
    public class Builder
	{
        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
		private string nameUser;

        /// <summary>
        /// Liste des sessions de jeux du mode bac-à-sable
        /// </summary>
		private List<SandBoxSession> sessionsSandBox; 

		private bool vibratorActivated;
		private bool soundActivated;

        /// <summary>
        ///  Instancie le Buider de UserData.
        /// </summary>
        /// <param name="nameUser">Nom de l'utilisateur</param>
		public Builder(string nameUser)
		{
			this.nameUser = nameUser;
			this.vibratorActivated = false;
			this.soundActivated = false;

			sessionsSandBox = new List<SandBoxSession>();

		}

        /// <summary>
        ///  Permet de définir que l'utilisateur a activé le vibreur
        /// </summary>
        /// <returns></returns>
		public UserData.Builder VibratorActivate()
		{
			vibratorActivated = true;
			return this;
		}

        /// <summary>
        ///  Permet de définir l'utilisateur a activé le son
        /// </summary>
        /// <returns></returns>
		public UserData.Builder SoundActivate()
		{
			soundActivated = true;
			return this;
		}

        /// <summary>
        /// Ajoute une Session de jeu du mode bac-à-sable
        /// </summary>
        /// <param name="sandBoxSession">la session de jeu</param>
        /// <returns></returns>
		public UserData.Builder AddSandBoxSession(SandBoxSession sandBoxSession)
		{
			sessionsSandBox.Add (sandBoxSession);

			return this;
		}

        /// <summary>
        /// Construit une instance de UserData à partir des informations renseignées.
        /// </summary>
        /// <returns></returns>
		public UserData Build ()
		{
			UserData user = new UserData ();

			user.nameUser = nameUser;
			user.sessionsSandBox = sessionsSandBox;
			user.vibratorActivated = vibratorActivated;
			user.soundActivated = soundActivated;
		
			return user;
		}

	}

	/// <summary>
	///  Nom de l'utilisateur
	/// </summary>
	public string nameUser;

	public bool vibratorActivated = false;
	public bool soundActivated = false;

	/// <summary>
	///  Sessions du bac à sable de l'utilisateur
	/// </summary>
	public List<SandBoxSession> sessionsSandBox;

    /// <summary>
    /// Actualise la sauvegare d'une partie de jeu déjà existante du mode bac-à-sable
    /// </summary>
    /// <param name="session"></param>
    /// <returns>
    /// Vrai si la sauvegarde à été effectué, sinon faux, 
    /// ce cas çi est possible si la session de jeu n'existe pas, 
    /// c'est-à-dire qu'aucune session de jeu existe avec le même nom
    /// </returns>
	public bool ReplaceSessionSandBox(SandBoxSession session)
	{
		int indexSession = sessionsSandBox.FindIndex (s => String.Compare (s.nameSave, session.nameSave) == 0);

		if (indexSession == -1)
			return false;
		
		sessionsSandBox [indexSession] = session;

		return true;
	}

    /// <summary>
    /// Créer une session de jeux bac-à-sable.
    /// </summary>
    /// <param name="session">La nouvelle session de jeu bac-à-sable</param>
    /// <returns>
    /// Vrai, si la création c'est effectué.
    /// Faux, s'il existe déjà une sauvegarde avec un même nom.
    /// </returns>
	public bool CreateSessionSandBox(SandBoxSession session)
	{
		int indexSession = sessionsSandBox.FindIndex (s => String.Compare (s.nameSave, session.nameSave) == 0);

		if (indexSession != -1)
			return false;
		
		sessionsSandBox.Add (session);

		return true;
	}

    /// <summary>
    /// Supprime une session de jeu bac-à-sable suivant son nom
    /// </summary>
    /// <param name="name">Nom de la session de jeu</param>
    /// <returns>
    /// Vrai, si la suppression s'est effectué
    /// Faux, si aucune session existe avec ce nom.
    /// </returns>
	public bool DeleteSessionSandBox(string name)
	{
		int indexSession = sessionsSandBox.FindIndex (s => String.Compare (s.nameSave, name) == 0);

		if (indexSession == -1)
			return false;

		sessionsSandBox.RemoveAt (indexSession);

		return true;
	}

	/// <summary>
	///  Recupère la session d'un bac à sable en fonction de son nom,
	///  si aucune session n'existe retourne null
	/// </summary>
	/// <returns>La session</returns>
	/// <param name="sessionName">Le nom de la session voulu</param>
	public SandBoxSession GetSessionSandBox(string sessionName)
	{
		int indexSession = sessionsSandBox.FindIndex (s => String.Compare (s.nameSave, sessionName) == 0);

		if (indexSession == -1)
			return null;

		return sessionsSandBox [indexSession];
	}

	/// <summary>
	///  Recupère la liste des noms des sessions bac à sable
	/// </summary>
	/// <returns>La liste des noms</returns>
	public List<string> GetNameOfSessionsSandBox()
	{
		List<string> sessionsName = new List<string> ();

		foreach (SandBoxSession session in sessionsSandBox) {
			sessionsName.Add(session.nameSave);
		}

		return sessionsName;
	}
}

