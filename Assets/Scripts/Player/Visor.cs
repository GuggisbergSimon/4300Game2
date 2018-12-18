using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visor : MonoBehaviour
{
	[SerializeField] private Racket racket;

	private void Update()
	{
		transform.rotation = racket.RotationBeforeHitting;
	}
}
