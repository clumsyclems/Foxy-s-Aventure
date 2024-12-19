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

    /* Player Input Action Class*/
    private PlayerInput playerInput = null;
   

    private void Awake()
    {
        playerInput.GetComponent<PlayerInput>();
    }

    /* Used to enable the binding Input Action */
    private void OnEnable()
    {
        playerInput.actions["Selection"].performed += Select;
        playerInput.actions["Selection"].Enable();

        playerInput.actions["Pause"].performed += ChangePauseMenuStatus;
        playerInput.actions["Pause"].Enable();

    }

    /* Used to disable the binding Input Action */
    private void OnDisable()
    {
        playerInput.actions["Selection"].performed -= Select;
        playerInput.actions["Selection"].Disable();

        playerInput.actions["Pause"].performed -= ChangePauseMenuStatus;
        playerInput.actions["Pause"].Disable();
    }

    /* Activate/desactivate pause menu */
    private void ChangePauseMenuStatus(InputAction.CallbackContext obj)
    {
        if (isPaused)
        {
            ResumeTheGame();
            playerInput.SwitchCurrentActionMap("Player");
        }
        else
        {
            PauseTheGame();
        }

    }

    private void Select(InputAction.CallbackContext context)
    {
        Debug.Log("Selected");
    }

    private void PauseTheGame()
    {
        isPaused = true;
        pauseMenu.enabled = isPaused;
        /* Change also the sound effect according if the game is paused or not */
        AudioListener.pause = isPaused;
        /* If the pauseMenu is enable the game stop and if it is disable the game start */
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void ResumeTheGame()
    {
        isPaused = false;
        pauseMenu.enabled = isPaused;
        /* Change also the sound effect according if the game is paused or not */
        AudioListener.pause = isPaused;
        /* If the pauseMenu is enable the game stop and if it is disable the game start */
        Time.timeScale = isPaused ? 0f : 1f;
    }

}
