using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{

    public GameObject credits;

    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditButton()
    {
        credits.SetActive(true);
    }

    public void BackButton()
    {
        credits.SetActive(false);
    }
}
