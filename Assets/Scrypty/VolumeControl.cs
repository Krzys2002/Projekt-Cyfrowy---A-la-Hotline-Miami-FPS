using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // Set initial slider value based on current volume
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
