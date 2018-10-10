using System.Collections.Generic;
using System;
using QCS;

/// <summary>
///  Interface pour la serialisation / deserialisation des données utilisateur
/// </summary>
public interface UserDAO
{

	/// <summary>
	///  Deserialise tous les données utilisateurs, s'il n'y en a pas, retourne une liste vite
	/// </summary>
	/// <returns> Les données utilisateurs </returns>
	List<UserData> GetAllUsers();

	/// <summary>
	///  Deserialise les données d'un utilisateur possédant un nom "name",
	///  s'il n'existe pas, la méthode retourne null
	/// </summary>
	/// <returns> L'utilisateur désérialisé, ou null</returns>
	/// <param name="name">Le nom de l'utilisateur</param>
	UserData GetUser (string name);

	/// <summary>
	///  Serialise plusieurs utilisateurs
	/// </summary>
	/// <param name="users">Les données des utilisateurs à sérialiser.</param>
	void SetUsers(List<UserData> users);

	/// <summary>
	/// Serialise les données d'un utilisateur
	/// </summary>
	/// <param name="user">Les données de l'utilisateur</param>
	void SetUser(UserData user);

	/// <summary>
	/// Supprime un utilisateur
	/// </summary>
	/// <param name="nameUser">Le nom de l'utilisateur à supprimer</param>
	void RemoveUser(string nameUser);

	/// <summary>
	///  Deserialise tous les données utilisateurs, s'il n'y en a pas, retourne une liste vite
	/// </summary>
	/// <returns> Les données utilisateurs </returns>
	List<string> GetAllUsersName();
}

