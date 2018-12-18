using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera vcam;
	[SerializeField] private Camera mainCamera;
	private CinemachineBasicMultiChannelPerlin noise;

	private void Start()
	{
		noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	private void ShakeCamera(float amplitudeGain, float frequencyGain)
	{
		noise.m_AmplitudeGain = amplitudeGain;
		noise.m_FrequencyGain = frequencyGain;
	}

	public IEnumerator ShakeCameraFor(float time, float amplitudeGain, float frequencyGain)
	{
		ShakeCamera(amplitudeGain, frequencyGain);
		yield return new WaitForSeconds(time);
		ShakeCamera(0.0f, 0.0f);
	}

	public void ChangeColorBackground()
	{
		mainCamera.backgroundColor =
			new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
	}
}