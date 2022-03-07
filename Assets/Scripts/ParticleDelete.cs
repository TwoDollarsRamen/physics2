using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDelete : MonoBehaviour {
	private void OnParticleSystemStopped()
	{
		Destroy(gameObject);
	}
}
