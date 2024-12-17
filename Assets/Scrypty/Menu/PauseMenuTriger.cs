using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuTriger : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    private bool isPaused = false; // Flag to check if the game is paused
    
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
        Debug.Log("Pause action: " + isPaused);
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
    }

    // Method to pause the game
    void Pause()
    {
        Debug.Log("Pause");
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        isPaused = true; // Set the pause flag to true
        Time.timeScale = 0f; // Pause the game time
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
