using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void GameStart()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
    }
}
