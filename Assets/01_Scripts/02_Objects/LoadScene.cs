using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load une scene suivant son nom
/// </summary>
public class LoadScene : MonoBehaviour
{
    [Tooltip("Nom de la scène à charger")]
    public string sceneName;

    public void OnClick()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
