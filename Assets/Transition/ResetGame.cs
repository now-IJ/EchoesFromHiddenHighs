using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StressBehaviour.currentStress = 0;
        ObjectMatchGame.collectedPuzzles = 0;
    }

}
