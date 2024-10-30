using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class aaGameManager : MonoBehaviour {

	private bool gameHasEnded = false;

	public Rotator rotator;
	public Spawner spawner;

	public Animator animator;

	public void EndGame ()
	{
		if (gameHasEnded)
			return;

        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playLoseSound();

        rotator.enabled = false;
		spawner.enabled = false;

		animator.SetTrigger("EndGame");

		gameHasEnded = true;
	}

	public void RestartLevel ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

}
