using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float jumpForce = 10f;

    Rigidbody2D rb;
    SpriteRenderer sr;

    public string currentColor;

    public Color colorCyan;
    public Color colorYellow;
    public Color colorMagenta;
    public Color colorPurple;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        SetRandomColor();
    }


    // Update is called once per frame
    void Update()
    {
        StressBehaviour.currentStress -= 1 * Time.deltaTime;

        print(currentColor);

        if (Input.GetButton("Jump") || Input.GetMouseButtonDown(0))
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "ColorChanger")
        {
            SetRandomColor();
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();
            Destroy(col.gameObject);
            return;
        }

        

        if (col.gameObject.tag != currentColor || col.tag == "Ground")
        {
            StressBehaviour.currentStress += 10;

            print("GAME OVER");
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playLoseSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(col.tag == "Finish")
        {
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().playWinSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void SetRandomColor ()
    {
        int index = Random.Range(0, 4);


        switch (index)
        {
            case 0:
                if (sr.color == colorCyan)
                {
                    SetRandomColor();
                }
                else
                {
                    currentColor = "Cyan";
                    sr.color = colorCyan;
                }
                break;
            case 1:
                if (sr.color == colorYellow)
                {
                    SetRandomColor();
                }
                else
                {
                    currentColor = "Yellow";
                    sr.color = colorYellow;
                }
                break;  
            case 2:
                if (sr.color == colorMagenta)
                {
                    SetRandomColor();
                }
                else
                {
                    currentColor = "Magenta";
                    sr.color = colorMagenta;
                }
                break;
            case 3:
                if (sr.color == colorPurple)
                {
                    SetRandomColor();
                }
                else
                {
                    currentColor = "Purple";
                    sr.color = colorPurple;
                }
                break;
                    
        }
    }
}
