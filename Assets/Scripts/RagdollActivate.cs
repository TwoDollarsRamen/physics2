using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivate : MonoBehaviour {
    void Start() {
	foreach (var i in GetComponentsInChildren<Rigidbody>()) {
			i.isKinematic = true;
	}
    }

	public void Activate()
	{
		foreach (var i in GetComponentsInChildren<Rigidbody>())
		{
			i.isKinematic = false;
		}
	}
}
