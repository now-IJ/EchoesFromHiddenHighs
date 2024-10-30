using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float stressReduce = 5;
    public float stressIncrease = 1.5f;

    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;

    private int countGuesses;
    private int countCorrentGuesses;
    private int gameGuesses;

    private int correntGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    private void Awake()
    {
        //puzzles = Resources.LoadAll<Sprite>("Images/Elizabeth,George Mason, Lincoln, Martin Luther King, Napoleon, Sejong");
    }
    // Start is called before the first frame update
    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);

        gameGuesses = gamePuzzles.Count / 2;
    }

    // Update is called once per frame
    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("puzzleBtn");
        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }


    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for(int i = 0; i< looper;i++)
        {
            if(index == looper/2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }
    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }

    public void PickPuzzle()
    {
        // string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        

        if(!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if(!secondGuess)
        { 
                secondGuess = true;
                secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

                secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

                btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            if(firstGuessPuzzle== secondGuessPuzzle)
            {
                print("Puzzle Match");
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();
                StressBehaviour.currentStress -= stressReduce;

                correntGuesses++;
                if(correntGuesses>=6) 
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            else
            {
                print("Puzzle don't Match");
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playLoseSound();
                StressBehaviour.currentStress += stressIncrease;
            }

            StartCoroutine(checkThePuzzleMatch());
        }
    }

    IEnumerator checkThePuzzleMatch()
    {

        yield return new WaitForSeconds(0.5f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.5f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckTheGameFinished();

        }
        else
        {
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(0.25f);

        firstGuess = secondGuess = false;
    }

    void CheckTheGameFinished()
    {
        countCorrentGuesses++;

        if(countCorrentGuesses == gameGuesses)
        {
            print("game finish");
            print("it took you" + countGuesses + "");
        }

    }

    void Shuffle(List<Sprite> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}
