using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text.RegularExpressions;

[CustomEditor(typeof(BoardData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    private BoardData GameDataInstance => target as BoardData;
    private ReorderableList dataList;

    private void OnEnable()
    {
        InitializeReordableList(ref dataList, "SearchWords", "Searching Words");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawColumnsRowFields();
        EditorGUILayout.Space();
        ConvertToUpperButton();

        if(GameDataInstance.Board != null && GameDataInstance.Columns > 0 && GameDataInstance.Rows > 0)
        {
            DrawBoardTable();
        }

        GUILayout.BeginHorizontal();

        ClearBoardButton();
        FillUpWithRandomLetterButton();

        GUILayout.EndHorizontal();  

        EditorGUILayout.Space();
        dataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if(GUI.changed)
        {
            EditorUtility.SetDirty(GameDataInstance);
        }
    }

    private void DrawColumnsRowFields()
    {
        var columnsTemp = GameDataInstance.Columns;
        var rowsTemp = GameDataInstance.Rows;

        GameDataInstance.Columns = EditorGUILayout.IntField("Columns", GameDataInstance.Columns);
        GameDataInstance.Rows = EditorGUILayout.IntField("Rows", GameDataInstance.Rows);

        if((GameDataInstance.Columns != columnsTemp || GameDataInstance.Rows != rowsTemp) && GameDataInstance.Columns > 0 && GameDataInstance.Rows > 0)
        {
            GameDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("Box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnsStyle = new GUIStyle();
        headerColumnsStyle.fixedWidth = 35;

        var columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 50;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.fixedWidth = 40;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var textFieldStye = new GUIStyle();
        textFieldStye.normal.background = Texture2D.grayTexture;
        textFieldStye.normal.textColor = Color.white;
        textFieldStye.fontStyle = FontStyle.Bold;
        textFieldStye.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for(var x=0; x<GameDataInstance.Columns;x++)
        {
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnsStyle : columnStyle);
            for(var y=0; y < GameDataInstance.Rows; y++)
            {
                if(x>=0 && y >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(GameDataInstance.Board[x].row[y], textFieldStye);
                    if (GameDataInstance.Board[x].row[y].Length > 1)
                    {
                        character = GameDataInstance.Board[x].row[y].Substring(0, 1);
                    }
                    GameDataInstance.Board[x].row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal ();
    }

    private void InitializeReordableList(ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };

        var l = list;

        list.drawElementCallback = (Rect rect, int Index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(Index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Word"), GUIContent.none);
        };

    }

    private void ConvertToUpperButton()
    {
        if(GUILayout.Button("To Upper"))
        {
            for(var i = 0; i < GameDataInstance.Columns; i++)
            {
                for (var j = 0; j < GameDataInstance.Rows; j++)
                {
                    var errorCounter = Regex.Matches(GameDataInstance.Board[i].row[j], @"[a-z]").Count;

                    if(errorCounter > 0)
                    {
                        GameDataInstance.Board[i].row[j] = GameDataInstance.Board[i].row[j].ToUpper();
                    }
                }
                foreach(var searchWord in GameDataInstance.SearchWords)
                {
                    var errorCounter = Regex.Matches(searchWord.Word, @"[a-z]").Count;
                    if( errorCounter > 0)
                    {
                        searchWord.Word = searchWord.Word.ToUpper();
                    }
                }
            }
        }
    }

    private void ClearBoardButton()
    {
        if(GUILayout.Button("Clear Board"))
        {
            for (int i = 0;i < GameDataInstance.Columns; i++)
            {
                for (int j = 0;j < GameDataInstance.Rows; j++)
                {
                    GameDataInstance.Board[i].row[j] = " ";
                }
            }
        }
    }

    private void FillUpWithRandomLetterButton()
    {
        if(GUILayout.Button("Fill Up With Random"))
        {
            for (int i = 0; i < GameDataInstance.Columns; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    int errorCounter = Regex.Matches(GameDataInstance.Board[i].row[j], @"[a-zA-Z]").Count;
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int index = UnityEngine.Random.Range(0, letters.Length);

                    if( errorCounter == 0)
                    {
                        GameDataInstance.Board[i].row[j] = letters[index].ToString();
                    }
                }
            }
        }
    }

}
