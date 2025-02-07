using UnityEngine;

public class LoadSpecificScene : MonoBehaviour
{
    public string sceneName = "";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GameManagerScript.Instance.GetLoadSpecificScene(sceneName));
        }

    }
}
