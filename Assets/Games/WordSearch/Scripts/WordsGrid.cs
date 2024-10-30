using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 00.0f;
    public float topPosition;

    private List<GameObject> squareList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnGridSquares();
        SetSquarePosition();
    }

    private void SpawnGridSquares()
    {
        if(currentGameData != null)
        {
            var squareScale = GetSquareScale(new Vector3(0.25f, 0.25f, 0.1f));
            foreach(var square in currentGameData.boardData.Board) 
            {
                foreach(var squareLetter in square.row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareLetter);
                    var selectedLetterData = alphabetData.AlphabetHighlighted.Find(data => data.letter == squareLetter);
                    var correctLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareLetter);


                    if(normalLetterData.image == null || selectedLetterData.image == null)
                    {
                        print("Sprite Error");
                    }
                    else
                    {
                        squareList.Add(Instantiate(gridSquarePrefab));
                        squareList[squareList.Count - 1].GetComponent<GridSquare>().SetSprite(normalLetterData, correctLetterData, selectedLetterData);
                        squareList[squareList.Count - 1].transform.SetParent(this.transform);
                        squareList[squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        squareList[squareList.Count - 1].transform.localScale = squareScale;
                        squareList[squareList.Count -1 ].GetComponent<GridSquare>().SetIndex(squareList.Count - 1); 
                    }
                }
            }
        }
    }

    private void SetSquarePosition()
    {
        var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squareList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPosition = GetFirstSquarePosition();
        int columnNumber = 0;
        int rowNumber = 0;

        foreach (var square in squareList)
        {
            if (rowNumber + 1 > currentGameData.boardData.Rows)
            {
                columnNumber++;
                rowNumber = 0;
            }

            var positionX = startPosition.x + offset.x * columnNumber;
            var positionY = startPosition.y - offset.y * rowNumber;

            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++;

        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = squareList[0].GetComponent <SpriteRenderer>().sprite.rect;
        var squareTransform = squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var midWidthPosition = (((currentGameData.boardData.Columns - 1) * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = (((currentGameData.boardData.Rows - 1) * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0f) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midWidthHeight;

        return startPosition;
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finaleScale = defaultScale;
        var adjustment = 0.01f;

        while(ShouldScaleDown(finaleScale))
        {
            finaleScale.x -= adjustment;
            finaleScale.y -= adjustment;

            if(finaleScale.x <= 0 || finaleScale.y <= 0)
            {
                finaleScale.x = adjustment;
                finaleScale.y = adjustment;
                return finaleScale;
            }
        }
        return finaleScale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareSize.y * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameData.boardData.Columns * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = ((currentGameData.boardData.Rows * squareSize.y) / 2) * 0.01;

        startPosition.x = (midWidthPosition != 0f) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = (float)midWidthHeight;

        return (startPosition.x < GetHalfScreenWidth() * -1 || startPosition.y > topPosition);

    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
}
