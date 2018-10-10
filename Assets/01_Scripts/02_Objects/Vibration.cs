using UnityEngine;

/// <summary>
/// Gère le toogle box du vibreur.
/// </summary>
public class Vibration : MonoBehaviour
{
	public void OnValueChange(bool canVibrate)
    {
        GameManager.SetVibration(canVibrate);
    }
}
