using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioRandom : MonoBehaviour

{
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source.time = Random.Range(0f, source.clip.length);
        source.Play();
    }
}
