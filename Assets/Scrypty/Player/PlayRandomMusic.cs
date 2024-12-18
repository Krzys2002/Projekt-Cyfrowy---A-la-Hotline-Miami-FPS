using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomMusic : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip[] musicClipArray;

    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicClipArray[Random.Range(0, musicClipArray.Length)];
        musicSource.PlayOneShot(musicSource.clip);
    }

}
