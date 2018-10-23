using UnityEngine;

/// <summary>
/// Gere le toogle box pour le son.
/// </summary>
public class Sound : MonoBehaviour
{
	public void OnValueChange(bool ableSound)
    {
        AudioListener.pause = ableSound;
    }
}
