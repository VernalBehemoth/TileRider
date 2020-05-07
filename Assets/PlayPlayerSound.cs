using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlayerSound : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioSources;
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(string attackName)
    {
        if (attackName == "Attack01")
        {
            audioSource.clip = audioSources[1];
            audioSource.Play();
        }
        else if (attackName == "Attack02")
        {
            audioSource.clip = audioSources[0];
            audioSource.Play();
        }

        if (attackName == "AttackEnded")
            Player.isHitting = false;
        else
            Player.isHitting = true;

    }

}
