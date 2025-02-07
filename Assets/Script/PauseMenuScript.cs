using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuScript : Abstract_Canvas<PauseMenuScript>
{
    /* The pause menu panel */
    [SerializeField] private Image pauseMenu = null;

    /* Static boolean to know if the game is paused or not */
    [SerializeField] private static bool isPaused = false;

    /* The Player Input to manage its controls */
    [SerializeField] private PlayerInput playerInput = null;

    protected override void Awake()
    {
        base.Awake();
        FindEventManager(); // Recherche de l'EventManager
    }

    protected void FindEventManager()
    {
        GameObject player = GameObject.Find("Player");

        if (player == null)
        {
            Debug.LogError("EventManager inexistant inside the scene !");
        }

        playerInput = player.GetComponent<PlayerInput>();

        if(playerInput == null)
        {
            Debug.LogError("EventManager inexistant inside the scene !");
        }
    }

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
