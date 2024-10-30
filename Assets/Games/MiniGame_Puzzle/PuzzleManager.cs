
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    private List<Transform> pieces;

    public int levelIndex = 4;
    public int difficulty = 4;
    private Vector2Int dimensions;
    public Transform gameHolder;
    public Transform piecePrefab;

    public Texture2D puzzleTexture;

    public float stressReduce = 5;
    public float stressIncrease = 1.5f;
   

    private float width;
    private float height;

    private Transform draggingPiece = null;
    private Vector3 offset;

    private int piecesCorrect;

    private void Start()
    {
        pieces = new List<Transform>();
        dimensions = GetDimensions(puzzleTexture, difficulty);
        CreatePuzzlePieces(puzzleTexture);
        Scatter();
        UpdateBorder();
        piecesCorrect = 0;
    }


    Vector2Int GetDimensions(Texture2D puzzleTexture, int difficulty)
    {
        Vector2Int dimensions = Vector2Int.zero;

        if (puzzleTexture.width < puzzleTexture.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * puzzleTexture.height) / puzzleTexture.width;
        }
        else
        {
            dimensions.x = (difficulty * puzzleTexture.width) / puzzleTexture.height;
            dimensions.y = difficulty;
        }

        return dimensions;
    }

    private void CreatePuzzlePieces(Texture2D puzzleTexture)
    {
        height = 1f / dimensions.y;
        float aspectRatio = (float)puzzleTexture.width / puzzleTexture.height;
        width = aspectRatio / dimensions.x;

        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-height * dimensions.y / 2) + (height * row) + (height / 2),
                    50
                    );
                piece.localScale = new Vector3(width, height, 1f);
                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);

                float width1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;

                Vector2[] UV = new Vector2[4];
                UV[0] = new Vector2(width1 * col, height1 * row);
                UV[1] = new Vector2(width1 * (col + 1), height1 * row);
                UV[2] = new Vector2(width1 * col, height1 * (row + 1));
                UV[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = UV;

                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", puzzleTexture);
            }
        }
    }

    private void Scatter()
    {
        gameHolder.gameObject.SetActive(true);

        float orthoHeight = Camera.main.orthographicSize;
        float screenAspectRatio = (float)Screen.width / Screen.height;
        float orthoWidth = orthoHeight * screenAspectRatio;

        float pieceWidth = width * gameHolder.localScale.x;
        float pieceHeight = height * gameHolder.localScale.y;

        orthoHeight -= pieceHeight;
        orthoWidth -= pieceWidth;

        foreach (Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x, y, 80);

        }
    }

    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();

        float halfWidth = (width * dimensions.x) / 2f;
        float halfHeight = (height * dimensions.y) / 2f;

        float borderZ = 0f;

        lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.enabled = true;
    }

    private void SnapAndCheck()
    {
        int pieceIndex = pieces.IndexOf(draggingPiece);

        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;

        Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
                                      (-height * dimensions.y / 2) + (height * row) + (height / 2));

        if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2))
        {
            draggingPiece.localPosition = targetPosition;

            draggingPiece.GetComponent<BoxCollider2D>().enabled = false;

            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();

            StressBehaviour.currentStress -= stressReduce;

            piecesCorrect++;
            if (piecesCorrect == pieces.Count)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        else
        {
            StressBehaviour.currentStress += stressIncrease;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                draggingPiece = hit.transform;
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
            }
        }

        if (draggingPiece && Input.GetMouseButtonUp(0))
        {
            SnapAndCheck();
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }

        if (draggingPiece)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //newPosition.z=draggingPiece.position.z;
            newPosition += offset;
            draggingPiece.position = newPosition;
        }
    }


}
