using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StressBehaviour : MonoBehaviour
{

    public static float currentStress = 0;
    private float maxStress = 100;

    public float stressSpeed = 1;

    public GameObject stressbar;

    float greenR = (100f / 255f * 50f) /100f;
    float greenG = (100f / 255f * 180f) / 100f;
    float greenB = (100f / 255f * 75f) / 100f;

    private Color greenLevel = new Color((100f / 255f * 50f) / 100f, (100f / 255f * 180f) / 100f, (100f / 255f * 75f) / 100f);
    private Color yellowLevel = new Color((100f / 255f * 226f) / 100f,  (100f / 255f * 230f) / 100f, (100f / 255f * 99f) / 100f);
    private Color redLevel = new Color((100f / 255f * 193f) / 100f, (100f / 255f * 66f) / 100f, (100f / 255f * 31f) / 100f);

    public GameObject emote1;
    public GameObject emote2;
    public GameObject emote3;

    private void Start()
    {
        print(greenG);
        print(greenB);
        print(greenR);
    }

    // Update is called once per frame
    void Update()
    {
        currentStress += stressSpeed * Time.deltaTime;
        if(currentStress <= 0f) currentStress = 0;
        if (currentStress >= 100f) SceneManager.LoadScene(18);
        float stressPerecent = currentStress / maxStress;
        stressbar.transform.localScale = new Vector3(stressPerecent, 1, 1);

        
        

        if (stressPerecent <= 1/3f ) 
        {
            //Green Level
            stressbar.GetComponent<Image>().color = greenLevel;
            emote1.SetActive(true);
            emote2.SetActive(false);
            emote3.SetActive(false);
        }
        else if(stressPerecent > 1/3f && stressPerecent <= 2 / 3f)
        {
            //Yellow Level
            stressbar.GetComponent<Image>().color = yellowLevel;
            emote1.SetActive(false);
            emote2.SetActive(true);
            emote3.SetActive(false);
        }
        else
        {
            //Red Level
            stressbar.GetComponent<Image>().color = redLevel;
            emote1.SetActive(false);
            emote2.SetActive(false);
            emote3.SetActive(true);
        }

    }
}
