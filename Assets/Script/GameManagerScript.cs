using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerScript : Singleton<GameManagerScript>
{    
    /* The animator to manage the fade system */
    [SerializeField] private Animator fadeSystemAnimator = null;

    /* Tranparent Status to know the fade system color */
    [SerializeField] private bool isTransparent = true;
    
    /* Definition to the isTransparent Setter and Getter */
    public bool IsTransparent { get => isTransparent; set => isTransparent = value; }

    [SerializeField] private GameObject[] requiredObjects; // Liste des objets à garder en scène

    protected override void Awake()
    {
        base.Awake();
        foreach (GameObject obj in requiredObjects)
        {
            if (obj != null)
            {
                // Vérifie si un objet de même nom existe déjà dans la scène
                GameObject existingObject = GameObject.Find(obj.name);

                if (existingObject == null)
                {
                    // Si l'objet n'existe pas, on le recrée
                    GameObject newObj = Instantiate(obj);
                    newObj.name = obj.name; // Assure que le nom reste le même
                }
            }
        }

        GameObject fadeSystem = GameObject.Find("FadeSystem");
        if (fadeSystem == null)
        {
            Debug.LogError("FadeSystem is inexistant inside the scene");
        }

        fadeSystemAnimator = fadeSystem.GetComponent<Animator>();

        if (fadeSystemAnimator == null)
        {
            Debug.LogError("FadeSystemAnimator is inexistant inside the fadeSystem component");
        }

    }

    /* Function to load specific scene (name given in argument) */
    public IEnumerator GetLoadSpecificScene(string sceneName)
    {
        PlayerScript.TriggerToggleControls(false);
        fadeSystemAnimator.SetTrigger("FadeIn");
        //Debug.Log("FadeIn activate");
        isTransparent = false;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /* Function add to add some animation at the beginning of the scene */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isTransparent)
        {
            foreach(GameObject gameObject in requiredObjects)
            {
                StartCoroutine(IsGameObjectReady(gameObject));
            }
            StartCoroutine(StartScene());
        }
    }

    /* Coroutine to launch some animation at the beginning of the scene */
    private IEnumerator StartScene()
    {

        fadeSystemAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        isTransparent = true;
        PlayerScript.TriggerToggleControls(true);

    }

    /* Coroutine to waiting the activation of the all components require for the scene load */
    public IEnumerator IsGameObjectReady(GameObject gameObject)
    {
        if (!Utils.IsGameObjectActive(gameObject))
        {
            yield return null;
        }
    }
}
