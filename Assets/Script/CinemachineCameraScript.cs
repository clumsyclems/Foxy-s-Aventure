using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinemachineCameraScript : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner = null;
    [SerializeField] private BoxCollider2D boxCollider = null;

    private void Start()
    {
        StartCoroutine(AssignConfiner());
        SceneManager.sceneLoaded += OnSceneLoaded; // Écoute le changement de scène
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Nettoyage de l'événement
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(AssignConfiner()); // Réassigner le confiner après le chargement d'une nouvelle scène
    }

    private IEnumerator AssignConfiner()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("SceneBorder") != null);

        GameObject sceneBorder = GameObject.FindGameObjectWithTag("SceneBorder");
        if (sceneBorder == null)
        {
            Debug.LogError("SceneBorder not found!");
            yield break;
        }

        boxCollider = sceneBorder.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError($"GameObject '{sceneBorder.name}' does not have a BoxCollider2D!");
            yield break;
        }

        cinemachineConfiner.BoundingShape2D = boxCollider;
        cinemachineConfiner.InvalidateBoundingShapeCache();

        Debug.Log("BoundingShape2D correctly assigned!");
    }
}
