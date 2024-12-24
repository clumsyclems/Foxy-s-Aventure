using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    /* The pause menu panel */
    public Image pauseMenu = null;

    /* Static boolean to know if the game is paused or not */
    public static bool isPaused = false;

    /* The Player Input to manage its controls */
    public PlayerInput playerInput = null;

    /* Used to enable the binding Input Action */
    private void OnEnable()
    {
        playerInput.actions["Pause"].performed += ChangePauseMenuStatus;
        playerInput.actions["Pause"].Enable();
    }

    /* Activate/desactivate pause menu */
    public void ChangePauseMenuStatus(InputAction.CallbackContext obj)
    {
        if (isPaused)
        {
            ResumeTheGame();
        }
        else
        {
            PauseTheGame();
        }

    }

    private void PauseTheGame()
    {
        isPaused = true;
        pauseMenu.enabled = isPaused;
        /* Change also the sound effect according if the game is paused or not */
        AudioListener.pause = isPaused;
        /* If the pauseMenu is enable the game stop and if it is disable the game start */
        Time.timeScale = 0f;
    }

    private void ResumeTheGame()
    {
        isPaused = false;
        pauseMenu.enabled = isPaused;
        /* Change also the sound effect according if the game is paused or not */
        AudioListener.pause = isPaused;
        /* If the pauseMenu is enable the game stop and if it is disable the game start */
        Time.timeScale = 1f;
    }

}
