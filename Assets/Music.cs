using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{

    Object[] myMusic; // declare this as Object array
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        myMusic = Resources.LoadAll("Music", typeof(AudioSource));
        audioSource.clip = myMusic[0] as AudioClip;
    }

    void Start()
    {
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
            playRandomMusic();
    }

    void playRandomMusic()
    {
        audioSource.clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
        audioSource.Play();
    }
}