using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ObjectMatchGame : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public int matchID;

    public float stressReduce = 5;

    private bool isDragging;

    private Vector3 endPoint;

    private ObjectMatchForm objectMatchForm;

    public static int collectedPuzzles = 0;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        objectMatchForm = GetComponent<ObjectMatchForm>();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                lineRenderer.SetPosition(0, mousePosition);

            }
        }

        if(isDragging) 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint  (Input.mousePosition);
            mousePosition.z = 0;
            lineRenderer.SetPosition(1, mousePosition);
            endPoint = mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {

            isDragging = false;
            RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
            if(hit.collider != null && hit.collider.TryGetComponent(out objectMatchForm) && matchID == objectMatchForm.getID())
            {

                collectedPuzzles ++;

                print(collectedPuzzles);
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();
                print("Correct");

                
                StressBehaviour.currentStress -= stressReduce;
                

                if(collectedPuzzles >= 6)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {

                }

                this.enabled = false;

            }
            else if(hit.collider != null && hit.collider.TryGetComponent(out objectMatchForm) && matchID != objectMatchForm.getID())
            {
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playLoseSound();
                lineRenderer.positionCount = 0;
                StressBehaviour.currentStress += stressReduce;
            }
            else
            {
                lineRenderer.positionCount = 0;
                
            }
            lineRenderer.positionCount = 2;
        }
    }
}
