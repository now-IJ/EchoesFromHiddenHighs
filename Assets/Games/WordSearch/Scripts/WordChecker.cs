using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameEvents;

public class WordChecker : MonoBehaviour
{

    public GameData gameData;

    public float reduceStress = 5;
    public float increasedStress = 5;

    private string word;

    private int assignedPoints = 0;
    private int completedWords = 0;
    private Ray rayUp, rayDown;
    private Ray rayLeft, rayRight;
    private Ray rayLeftDown, rayRightDown ;
    private Ray rayLeftUp, rayRightUp;
    private Ray currentRay = new Ray();
    private Vector3 rayStartPosition;
    private List<int> correctSquareList = new List<int>();

    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SqaureSelected;
        GameEvents.OnClearSelection += ClearSelection;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SqaureSelected;
        GameEvents.OnClearSelection -= ClearSelection;
    }

    // Start is called before the first frame update
    void Start()
    {
        assignedPoints = 0;
        completedWords = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(assignedPoints >0 && Application.isEditor) 
        {
            Debug.DrawRay(rayUp.origin, rayUp.direction * 7);
            Debug.DrawRay(rayDown.origin, rayDown.direction * 7);
            Debug.DrawRay(rayLeft.origin, rayLeft.direction * 7);
            Debug.DrawRay(rayRight.origin, rayRight.direction * 7);
            Debug.DrawRay(rayLeftUp.origin, rayLeftUp.direction * 7);
            Debug.DrawRay(rayLeftDown.origin, rayLeftDown.direction * 7);
            Debug.DrawRay(rayRightUp.origin, rayRightUp.direction * 7);
            Debug.DrawRay(rayRightDown.origin, rayRightDown.direction * 7);
        }   
    }

    private void SqaureSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if(assignedPoints == 0)
        {
            rayStartPosition = squarePosition;
            correctSquareList.Add(squareIndex);
            word += letter;

            rayUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, 1));
            rayDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, -1));
            rayLeft = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, 0));
            rayRight = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, 0));
            rayLeftUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, 1f));
            rayLeftDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, -1f));
            rayRightUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, 1f));
            rayRightDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, -1f));
        
        }else if(assignedPoints == 1)
        {
            correctSquareList.Add(squareIndex);
            currentRay = SelectRay(rayStartPosition, squarePosition);
            GameEvents.SelectSquareMethod(squarePosition);
            word += letter;
            CheckWord();
        }
        else
        {
            if(IsPointOnRay(currentRay, squarePosition))
            {
                correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squarePosition);
                word += letter;
                CheckWord();
            }
        }

        assignedPoints++;

    }

    private void CheckWord()
    {
        foreach(var searchindWord in gameData.boardData.SearchWords)
        {
            if(word == searchindWord.Word)
            {
                GameEvents.CorrectWordMethod(word, correctSquareList);
                //Winning
                word = string.Empty;
                correctSquareList.Clear();

                StressBehaviour.currentStress -= reduceStress ;

                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();

                completedWords++;
                if(completedWords >= gameData.boardData.SearchWords.Count)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                return;
            }
            
        }
    }

    private bool IsPointOnRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100f);

        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
            {
                return true;
            }
        }

        return false;
    }

    private Ray SelectRay(Vector2 firstPosition,  Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.01f;

        if(Mathf.Abs(direction.x)< tolerance && Mathf.Abs(direction.y -1)< tolerance)
        {
            return rayUp;
        }
        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - (-1)) < tolerance)
        {
            return rayDown;
        }
        if (Mathf.Abs(direction.x - (-1)) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return rayLeft;
        }
        if (Mathf.Abs(direction.x - 1) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return rayRight;
        }
        if (direction.x < 0f && direction.y > 0f)
        {
            return rayLeftUp;
        }
        if (direction.x < 0f && direction.y < 0f)
        {
            return rayLeftDown;
        }
        if (direction.x > 0f && direction.y > 0f)
        {
            return rayRightUp;
        }
        if (direction.x > 0f && direction.y < 0f)
        {
            return rayRightDown;
        }

        return rayDown;

    }

    private void ClearSelection()
    {
        assignedPoints = 0;
        correctSquareList.Clear();
        word = string.Empty;
    }

}
