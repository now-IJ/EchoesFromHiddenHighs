using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvents;

public class GridSquare : MonoBehaviour
{

    public int SquareIndex { get; set; }

    private AlphabetData.LetterData normalLetterData;
    private AlphabetData.LetterData selectedLetterData;
    private AlphabetData.LetterData correctLetterData;

    private SpriteRenderer displayedImage;

    private bool selected;
    private bool clicked;
    private bool correct;
    private int index = -1;

    public void SetIndex(int Index)
    {
        index = Index;
    }

    public int GetIndex()
    {
        return index;
    }

    // Start is called before the first frame update
    void Start()
    {
        selected = false;
        clicked = false;
        correct = false;
        displayedImage = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += SelectSquare;
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= SelectSquare;
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void OnEnableSquareSelection()
    {
        clicked = true;
        selected = false;
    }

    public void OnDisableSquareSelection()
    {
        clicked = false;
        selected = false;

        if (correct)
        {
            displayedImage.sprite = correctLetterData.image;
        }
        else
        {
            displayedImage.sprite = normalLetterData.image;
        }
    }

    private void SelectSquare(Vector3 position)
    {
        if(this.gameObject.transform.position == position)
        {
            displayedImage.sprite = selectedLetterData.image;
        }
    }

    public void SetSprite(AlphabetData.LetterData NormalLetterData, AlphabetData.LetterData SelectedLetterData, AlphabetData.LetterData CorrectLetterData)
    {
        normalLetterData = NormalLetterData;
        selectedLetterData = SelectedLetterData;
        correctLetterData = CorrectLetterData;

        GetComponent<SpriteRenderer>().sprite = normalLetterData.image;
    }

    private void OnMouseDown()
    {
        OnEnableSquareSelection();
        GameEvents.EnableSquareSeletionMethod();
        CheckSquare();
        displayedImage.sprite = selectedLetterData.image;
    }

    private void OnMouseEnter()
    {
        CheckSquare();
    }

    private void OnMouseUp()
    {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSeletionMethod();
    }

    public void CheckSquare()
    {
        if(selected == false && clicked == true)
        {
            selected = true;
            GameEvents.CheckSquareMethod(normalLetterData.letter, gameObject.transform.position, index);


        }
    }

    private void CorrectWord(string Word, List<int> squareIndexes)
    {
        if(selected && squareIndexes.Contains(index))
        {
            correct =true;
            displayedImage.sprite = correctLetterData.image;
        }

        selected = false;
        clicked = false;
    }

}
