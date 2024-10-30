using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointControll : MonoBehaviour
{
    private bool hitTop = false;
    private bool hitBottom = false;

    public float reduceStress = 5f;

    public TextMeshProUGUI scoreText;
    public float ballsNeeded = 3;

    private GameObject bottomCollider;

    private void Start()
    {
        scoreText.text = "Score: " + ballsNeeded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if(collision.tag == "TopCollider")
        {
            hitBottom = false ;
            hitTop = true ;
        }
        if(collision.tag == "BottomCollider")
        {
            hitBottom = true ;
            if(hitTop && hitBottom) getPoint();
            bottomCollider = collision.gameObject;
            hitTop = false ;
        }
        
        if(hitBottom && !hitTop)
        {
            bottomCollider.SetActive(false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {

            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playBallBounceSound();
            hitTop = false;
            hitBottom = false;   
            if(bottomCollider != null)
            {
                bottomCollider.SetActive(true);    
            }
        }
    }

    void getPoint()
    {
        print("You get a Point!");
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();
        StressBehaviour.currentStress -= 5;
        ballsNeeded--;
        scoreText.text = "Score: " + ballsNeeded;
        if(ballsNeeded <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

}
