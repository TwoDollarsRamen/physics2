using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ender : MonoBehaviour
{
	[SerializeField] private GameObject endGame;

	private void Start()
	{
		endGame.SetActive(false);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.other.gameObject.CompareTag("Player")) {
			endGame.SetActive(true);
			Time.timeScale = 0.0f;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
