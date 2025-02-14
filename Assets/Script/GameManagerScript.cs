using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerScript : Singleton<GameManagerScript>
{
    /* Tranparent Status to know the fade system color */
    [SerializeField] private bool isTransparent = true;
    
    /* Definition to the isTransparent Setter and Getter */
    public bool IsTransparent { get => isTransparent; set => isTransparent = value; }

    [SerializeField] private GameObject[] requiredObjects; // Liste des objets à garder en scène

    [SerializeField] private Vector2 cercleEffectPosition = Vector2.zero;

    [SerializeField] private float circleRadiusInUV = 0f;

    [SerializeField] private float playerAnimationDuration = 0f;

    [SerializeField] private float circleExpandDuration = 0f;

    protected override void Awake()
    {
        base.Awake();
        LoadObjectNeeded();
    }

    /* Function to load specific scene (name given in argument) */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    /* Coroutine to laod a specific scene */
    public IEnumerator GetLoadSpecificScene(string sceneName)
    {
        PlayerScript.TriggerToggleControls(false);
        
        IsTransparent = false;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    /* Function add to add some animation at the beginning of the scene */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SceneLoadingSequence());
    }
    private IEnumerator SceneLoadingSequence()
    {
        foreach (GameObject gameObject in requiredObjects)
        {
            yield return StartCoroutine(IsGameObjectReady(gameObject));
        }

        yield return new WaitForSeconds(1f);
        
        if (!isTransparent)
        {
            yield return StartCoroutine(AnimationScript.Instance.StartScene(circleRadiusInUV, playerAnimationDuration, circleExpandDuration));
        }
    }

    private void LoadObjectNeeded()
    {
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
    }

    /* Coroutine to waiting the activation of the all components require for the scene load */
    private IEnumerator IsGameObjectReady(GameObject gameObject)
    {
        if (!Utils.IsGameObjectActive(gameObject))
        {
            yield return null;
        }
    }

}
