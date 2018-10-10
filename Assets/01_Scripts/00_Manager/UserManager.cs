using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Classe gerant les utilisateurs et leur données.
/// </summary>
public class UserManager {

	private string _defaultNameUser = "DefaultUser";

	/// <summary>
	///  DAO pour l'acces aux données des utilisateurs
	/// </summary>
	private UserDAO _userDAO;

	/// <summary>
	///  Utilisateur courrant
	/// </summary>
	private UserData _currentUser;

	/// <summary>
	///  Liste des noms des utilisateurs
	/// </summary>
	private List<string> _nameUsers;

	/// <summary>
	///  Constructeur de la classe UserManager
	///   - Initialise le DAO des données utilisateurs.
	///   - Recupère la liste des noms utilisateurs
	/// </summary>
	public UserManager() {
		_userDAO = new UserDAOJSON ();

		LoadAllNameUsersFromDAO();

		CreateDefaultUser ();

		InitCurrentUser ();
	}

	/// <summary>
	///  Recupère le nom des utilisateurs depuis les fichiers de données
	/// </summary>
	public void LoadAllNameUsersFromDAO()
	{
		_nameUsers = _userDAO.GetAllUsersName ();
	}

	/// <summary>
	///  Recupère le nom des utilisateurs
	/// </summary>
	public List<string> GetAllNameUsers()
	{
		return _nameUsers;
	}

    /// <summary>
    /// Initialise l'utilisateur courrant comme l'utilisateur par défaut.
    /// TODO: Il serai envisageable de stocker le dernier utilisateur présent dans la dernière session dans un fichier afin de pouvoir recharger celui-ci.
    /// </summary>
	private void InitCurrentUser()
	{
		UserData userData = _userDAO.GetUser (_defaultNameUser);

		_currentUser = userData;
	}

	/// <summary>
	///  Créé un utilisateur par défaut si aucun utilisateur existe.
	///  Et définit cet utilisateur comme utilisateur courrant.
	/// </summary>
	private void CreateDefaultUser()
	{
		if (_nameUsers.Count != 0)
			return;

		_currentUser = new UserData.Builder (_defaultNameUser).Build ();

		_userDAO.SetUser (_currentUser);
		_nameUsers.Add (_defaultNameUser);
	}

	/// <summary>
	///  Remplace l'utilisateur courrant par un utilisateur ayant le pseudo "username"
	/// </summary>
	/// <param name="username"> Pseudo de l'utilisateur à charger </param>
	public void LoadUser(string username)
	{
		_currentUser = _userDAO.GetUser (username);
	}

	/// <summary>
	///  Supprime l'utilisateur courrant.
	///  Il est impossible de supprimer l'utilisateur par défaut.
	/// </summary>
	/// <returns>Vrai si l'utilisateur à été supprimé, sinon faux</returns>
	public bool RemoveCurrentUser()
	{
		if (string.Compare (_defaultNameUser, _currentUser.nameUser) == 0)
			return false;

		_nameUsers.Remove (_currentUser.nameUser);
		_userDAO.RemoveUser (_currentUser.nameUser);

		return true;
	}

    /// <summary>
    /// Sauvegarde physiquement l'utilisateur courrant.
    /// </summary>
	public void SaveCurrentUser()
	{
		_userDAO.SetUser (_currentUser);
	}

    /// <summary>
    /// Méthode facade de la méthode "CreateSessionSandBox" de UserData
    /// </summary>
    /// <param name="sandBoxSession"></param>
    /// <returns></returns>
	public bool CreateSandBoxSessionOfCurrentUser(SandBoxSession sandBoxSession)
	{
		if (!_currentUser.CreateSessionSandBox (sandBoxSession))
			return false;

		SaveCurrentUser ();

		return true;
	}

    /// <summary>
    /// Méthode facade de la méthode "ReplaceSessionSandBox" de UserData
    /// </summary>
    /// <param name="sandBoxSession"></param>
    /// <returns></returns>
    public bool SaveSandBoxSessionOfCurrentUser(SandBoxSession sandBoxSession)
	{
		if (!_currentUser.ReplaceSessionSandBox (sandBoxSession))
			return false;

		SaveCurrentUser ();

		return true;
	}

    /// <summary>
    /// Méthode facade de la méthode "DeleteSessionSandBox" de UserData
    /// </summary>
    /// <param name="sandBoxSession"></param>
    /// <returns></returns>
	public bool DeleteSandBoxSessionOfCurrentUser(string name){
		if (!_currentUser.DeleteSessionSandBox (name))
			return false;

		Debug.Log ("toto");

		SaveCurrentUser ();

		return true;
	}

    /// <summary>
    /// Ajoute un utilisateur suivant son nom.
    /// Et définis ce nouvel utilisateur comme l'utilisateur courrant.
    /// </summary>
    /// <param name="nameUser">Nom de l'utilisateur</param>
    /// <returns>
    /// Vrai, si l'utilisateur est créé;
    /// Faux dans le cas où un utilisateur existe déjà avec ce nom
    /// </returns>
	public bool AddUser(string nameUser)
	{
		if (_nameUsers.Contains (nameUser))
			return false;
		
		SaveCurrentUser ();

		_nameUsers.Add (nameUser);

		_currentUser = new UserData.Builder (nameUser).Build ();

		_userDAO.SetUser (_currentUser);

		return true;
	}

    /// <summary>
    /// Change le nom de l'utilisateur courrant.
    /// Il n'est pas possible de renommer l'utilisateur par défaut.
    /// </summary>
    /// <param name="newName">Nouveau nom de l'utilisateur</param>
    /// <returns>
    /// Vrai, si l'utilisateur est renomé.
    /// Faux, si :
    ///     - L'utilisateur courrant est l'utilisateur par défaut.
    ///     - Le utilisateur possède déjà le nom "newName"
    /// </returns>
	public bool RenameCurrentUser(string newName)
	{
		if (string.Compare (_defaultNameUser, _currentUser.nameUser) == 0)
			return false;

		if (_nameUsers.Contains (newName))
			return false;

		_nameUsers [_nameUsers.IndexOf (_currentUser.nameUser)] = newName;

		_userDAO.RemoveUser (_currentUser.nameUser);

		_currentUser.nameUser = newName;

		_userDAO.SetUser (_currentUser);

		return true;
	}

    /// <summary>
    /// Recupère l'utilisateur courrant.
    /// </summary>
    /// <returns></returns>
	public UserData GetCurrentUser()
	{
		return _currentUser;
	}
}