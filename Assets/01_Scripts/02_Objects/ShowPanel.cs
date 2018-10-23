using UnityEngine;

/// <summary>
/// Permet d'afficher ou non un panel.
/// </summary>
public class ShowPanel : MonoBehaviour
{

    public bool show;

    public GameObject panel;


	public void OnClick()
    {
        panel.SetActive(show);
    }
}
