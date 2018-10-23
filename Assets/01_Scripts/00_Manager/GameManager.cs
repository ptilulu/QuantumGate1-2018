using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Classe permetant la gestion des données globales au jeu.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton permettant à la gestion des utilisateurs.
    /// </summary>
	public static UserManager UserManager {
		get;
		internal set;
	}
		
    IEnumerator Start ()
    {
		UserManager = new UserManager ();

        DontDestroyOnLoad(gameObject);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("Menu");
	}

    /// <summary>
    /// Active ou non le vibreur suivant le booleen pour l'utilisateur courrant.
    /// Et sauvegarde physiquement celui-ci.
    /// </summary>
    /// <param name="vibrate"></param>
    public static void SetVibration(bool vibrate)
    {
		UserManager.GetCurrentUser().vibratorActivated = vibrate;
		UserManager.SaveCurrentUser ();
    }

    public static bool CanVibrate()
    {
		return UserManager.GetCurrentUser().vibratorActivated;
    }
}
