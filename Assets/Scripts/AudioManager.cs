using UnityEngine;

/// <summary>
/// Controls sound effects playback.
/// </summary>
public class AudioManager : MonoBehaviour
{
	public AudioClip switchEffect;

	static AudioManager instance;

	void Awake ()
	{
		instance = this;
	}

	public static void PlaySwitchEffect ()
	{
		instance.audio.PlayOneShot(instance.switchEffect);
	}
}
