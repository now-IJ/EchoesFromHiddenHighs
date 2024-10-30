using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pin : MonoBehaviour
{
    private bool isPinned = false;

    public float speed = 20f;
    public Rigidbody2D rb;

    public float reduceStress = 5;

    void Update()
    {
        if (!isPinned)
            rb.MovePosition(rb.position + Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Rotator")
        {
            transform.SetParent(col.transform);
            Score.PinCount--;
            StressBehaviour.currentStress -= reduceStress;
            if(Score.PinCount <= 0)
            {
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            isPinned = true;
        }
        else if (col.tag == "Pin")
        {
            FindObjectOfType<aaGameManager>().EndGame();
        }
    }
}
