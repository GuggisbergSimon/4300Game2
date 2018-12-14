using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioMixer generalMixer;

	public void UpdateAudio()
	{
		if (Time.timeScale.CompareTo(0) == 0)
		{
			PauseAudio();
		}
		else
		{
			AudioListener.pause = false;
			generalMixer.SetFloat("Pitch", Time.timeScale);
		}
	}

	private void PauseAudio()
	{
		AudioListener.pause = true;
	}
}
