using UnityEngine;

/// <summary>
/// Controls sound effects playback.
/// </summary>
public class AudioManager : MonoBehaviour
{
	public AudioClip switchEffect;
	public AudioClip scream;
	public AudioClip splat;

	static AudioManager instance;

	void Awake ()
	{
		instance = this;
	}

	public static void PlaySwitchEffect ()
	{
		instance.audio.PlayOneShot(instance.switchEffect);
	}

	public static void PlayDeath ()
	{
		instance.audio.PlayOneShot(instance.scream);
		instance.audio.PlayOneShot(instance.splat);
	}
}
