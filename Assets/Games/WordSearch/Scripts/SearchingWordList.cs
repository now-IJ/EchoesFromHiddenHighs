using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SearchingWordList : MonoBehaviour
{
    public GameData gameData;
    public GameObject searchingWordPrefab;
    public float offset = 0.0f;
    public int maxColumns = 5;
    public int maxRows = 4;

    private int columns = 2;
    private int rows;
    private int wordsNumber;

    private List<GameObject> words = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        wordsNumber = gameData.boardData.SearchWords.Count;
        if (wordsNumber < columns) 
        {
            rows = 1;
        }
        else
        {
            CalculateColumnsAndRowsNumber();
        }

        CreateWordObjects();
        SetWordsPosition();
    }

    private void CalculateColumnsAndRowsNumber()
    {
        do
        {
            columns++;
            rows = wordsNumber / columns;

        }while(rows >= maxRows);    
    
        if(columns > maxColumns)
        {
            columns = maxColumns;
            rows = wordsNumber / columns;
        }

    }

    private bool TryIncreaseColumnNumber()
    {
        columns++;
        rows = wordsNumber / columns;
        if(columns > maxColumns)
        {
            columns = maxColumns;
            rows = wordsNumber / columns;

            return false;
        }

        if(wordsNumber % columns > 0)
        {
            rows++;
        }

        return true;
    }

    private void CreateWordObjects()
    {
        var squareScale = GetSquareScale(new Vector3(1f,1f, 0.1f));

        for (var index = 0; index < wordsNumber; index++)
        {
            words.Add(Instantiate(searchingWordPrefab) as  GameObject);
            words[index].transform.SetParent(this.transform);
            words[index].GetComponent<RectTransform>().localScale = squareScale;
            words[index].GetComponent <RectTransform>().localPosition = new Vector3(0f,0f,0f);
            words[index].GetComponent<SearchingWord>().SetWord(gameData.boardData.SearchWords[index].Word);
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0f || finalScale.y <= 0f)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;

                return finalScale;
            }
        }
        return finalScale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = searchingWordPrefab.GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();

        var squareSize = new Vector2(0.0f, 0.0f);

        squareSize.x = squareRect.rect.width * targetScale.x + offset;
        squareSize.y = squareRect.rect.height * targetScale.y + offset;

        var totalSquaresHeight = squareSize.y * rows;

        if (totalSquaresHeight > parentRect.rect.height)
        {
            while (totalSquaresHeight > parentRect.rect.height)
            {
                if (TryIncreaseColumnNumber())
                {
                    totalSquaresHeight = squareSize.y * rows;
                }
                else
                {
                    return true;
                }
            }
        }

        var totalSquaresWidth = squareSize.x * columns;

        if (totalSquaresWidth > parentRect.rect.width)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void SetWordsPosition()
    {
        var squareRect = words[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + offset
        };

        int columnsNumber = 0;
        int rowsNumber = 0;
        var startPosition = GetFirstSquarePosition();

        foreach ( var word in words )
        {
            if(columnsNumber + 1 > columns)
            {
                columnsNumber = 0;
                rowsNumber++;
            }

            var positionX = startPosition.x + wordOffset.x * columnsNumber;
            var positionY = startPosition.y + wordOffset.y * rowsNumber;

            word.GetComponent<RectTransform>().localPosition = new Vector2(positionX, positionY);
            columnsNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var sqauareRect = words[0].GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();
        var squareSize = new Vector2(0.0f, 0.0f);

        squareSize.x = sqauareRect.rect.width * sqauareRect.transform.localScale.x + offset;
        squareSize.y = sqauareRect.rect.height * sqauareRect.transform.localScale.y + offset;

        var shiftBy = (parentRect.rect.width - (squareSize.x * columns)) / 2;

        startPosition.x = ((parentRect.rect.width - squareSize.x) / 2) * (-1);
        startPosition.x += shiftBy;
        startPosition.y = ((parentRect.rect.height - squareSize.y) / 2);

        return startPosition;

    }
}
