using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Aduio Source")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip click;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip ballBounce;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sfxSource.clip = click;
            sfxSource.Play();
        }
    }

    public void playWinSound()
    {
        sfxSource.clip = win;
        sfxSource.Play();
    }

    public void playLoseSound()
    {
        sfxSource.clip = lose;
        sfxSource.Play();
    }

    public void playBallBounceSound()
    {
        sfxSource.clip = ballBounce;
        sfxSource.Play();
    }

    private void Start()
    {
        audioSource.clip = background;
        audioSource.Play();
    }
}
