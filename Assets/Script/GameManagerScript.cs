using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] gameObjectsToNotDestroy = null;

    public Animator fadeSystemAnimator = null;

    public static GameManagerScript instance;

    public bool isTransparent = true;

    public bool IsTransparent { get => isTransparent; set => isTransparent = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("They have more than inventory inside the project");
            return;
        }
        instance = this;

        foreach (GameObject go in gameObjectsToNotDestroy)
        {
            DontDestroyOnLoad(go);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isTransparent)
        {
            StartCoroutine(StartScene());
        }
    }

    private IEnumerator StartScene()
    {
        yield return new WaitForSeconds(2f);

        fadeSystemAnimator.SetTrigger("FadeOut");
        isTransparent = true;
    }
}
