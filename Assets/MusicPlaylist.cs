using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioClip[] songs;
    int currentSong;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartPlaylist());
    }

    IEnumerator StartPlaylist()
    {
        currentSong = 0;
        for (; ; )
        {
            if (!audioSource.isPlaying)
            {

                if (currentSong < songs.Length)
                    audioSource.clip = songs[currentSong];
                else
                {
                    currentSong = 0;
                    audioSource.clip = songs[currentSong];
                }

                audioSource.Play();

                currentSong++;
            }

            yield return new WaitForSeconds(2F);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
