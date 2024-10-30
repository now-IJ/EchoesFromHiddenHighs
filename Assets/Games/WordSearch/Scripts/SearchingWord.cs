using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{

    public TextMeshProUGUI displayedText;
    public Image crossLine;

    private string word;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void SetWord(string Word)
    {
        word = Word;
        displayedText.text = word;
    }

    private void CorrectWord(string Word, List<int> squareIndexes)
    {
        if( Word == word )
        {
            crossLine.gameObject.SetActive(true);

        }
    }

}
