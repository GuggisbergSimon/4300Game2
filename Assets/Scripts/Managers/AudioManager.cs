using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioMixer generalMixer;

	public void UpdateAudio()
	{
		generalMixer.SetFloat("Pitch", Time.timeScale);
	}
}
