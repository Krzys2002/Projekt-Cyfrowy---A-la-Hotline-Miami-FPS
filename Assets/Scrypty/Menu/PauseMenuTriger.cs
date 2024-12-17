using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuTriger : MonoBehaviour
{
    public List<GameObject> objectsToDisable;
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    private bool isPaused = false; // Flag to check if the game is paused
    private List<AudioSource> audioSources = new List<AudioSource>();
    
    private void Awake()
    {
        if (InputMenager.inputMenager == null)
        {
            Debug.LogError("InputMenager is missing.");
        }
        
        InputMenager.inputMenager.pauseAction += PauseAction;
        
        Resume();
    }

    private void PauseAction()
    {
        //Debug.Log("Pause action: " + isPaused);
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    
    
    // Method to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        isPaused = false; // Set the pause flag to false
        Time.timeScale = 1f; // Resume the game time
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (GameObject objectToEnable in objectsToDisable)
        {
            objectToEnable.SetActive(true);
        }
        ResumeAllAudio();
    }

    // Method to pause the game
    // ReSharper disable Unity.PerformanceAnalysis
    void Pause()
    {
        Debug.Log("Pause");
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        isPaused = true; // Set the pause flag to true
        Time.timeScale = 0f; // Pause the game time
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (GameObject objectToDisable in objectsToDisable)
        {
            objectToDisable.SetActive(false);
        }
        PauseAllAudio();
    }
    private void PauseAllAudio()
    {
        audioSources.Clear();
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                audioSources.Add(audioSource);
            }
        }
    }

    private void ResumeAllAudio()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.UnPause();
        }
        audioSources.Clear();
    }
}

