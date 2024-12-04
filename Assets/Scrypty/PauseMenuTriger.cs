using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuTriger : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    private bool isPaused = false; // Flag to check if the game is paused


    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    // Method to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        Time.timeScale = 1f; // Resume the game time
        isPaused = false; // Set the pause flag to false
    }

    // Method to pause the game
    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        Time.timeScale = 0f; // Pause the game time
        isPaused = true; // Set the pause flag to true
    }
}
