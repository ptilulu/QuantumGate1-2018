using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Permet la serialisation en JSON des utilisateurs.
/// 
/// Serialise les données utilisateurs au chemin suivant : 
/// "/saves/users/" dans le dossier des données persistantes de l'application (Application.persistentDataPath).
/// Les utilisateurs sont stockés dans des fichiers séparés :
/// un utilisateur possède un fichier de donné avec le nom suivant : pseudo.json
/// </summary>
public class UserDAOJSON : UserDAO
{
	public UserDAOJSON () { }


	public List<UserData> GetAllUsers(){
		List<UserData> usersData = new List<UserData> ();

		if(!Directory.Exists(Application.persistentDataPath + "/saves/users")){
			return usersData;
		}

		string[] usersFiles = Directory.GetFiles (Application.persistentDataPath + "/saves/users");

		foreach (string user in usersFiles) {
			usersData.Add(GetUser(Path.GetFileNameWithoutExtension(user)));
		}

		return usersData;
	}


	public UserData GetUser (string name)
	{
		if(!File.Exists(Application.persistentDataPath + "/saves/users/" + name + ".json"))
			return null;

		string jsonString = File.ReadAllText(Application.persistentDataPath + "/saves/users/" + name + ".json");

		UserData user = JsonUtility.FromJson<UserData>(jsonString);

		return user;
	}


	public void SetUsers(List<UserData> users)
	{
		if(!Directory.Exists(Application.persistentDataPath + "/saves/users"))
			Directory.CreateDirectory (Application.persistentDataPath + "/saves/users");


		foreach (UserData userData in users) 
			SetUser (userData);

	}
		
	public void SetUser(UserData user)
	{
		if(!Directory.Exists(Application.persistentDataPath + "/saves/users"))
			Directory.CreateDirectory (Application.persistentDataPath + "/saves/users");

		string jsonString = JsonUtility.ToJson(user);

		File.WriteAllText(Application.persistentDataPath + "/saves/users/" + user.nameUser + ".json", jsonString);
	}

	public void RemoveUser(string nameUser)
	{
		File.Delete (Application.persistentDataPath + "/saves/users/" + nameUser + ".json");
	}

	public List<string> GetAllUsersName()
	{
		List<string> usersname = new List<string> ();

		if (!Directory.Exists (Application.persistentDataPath + "/saves/users"))
			return usersname;

		string[] usersFiles = Directory.GetFiles (Application.persistentDataPath + "/saves/users");

		foreach (string filename in usersFiles) {
			usersname.Add(Path.GetFileNameWithoutExtension(filename));
		}

		return usersname;
	}
}


