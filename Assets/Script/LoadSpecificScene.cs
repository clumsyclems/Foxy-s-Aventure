using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSpecificScene : MonoBehaviour
{
    public string sceneName = "";
    public Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GetLoadSpecificScene());
        }

    }

    private IEnumerator GetLoadSpecificScene()
    {
        animator.SetTrigger("FadeIn");
        GameManagerScript.instance.isTransparent = false;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(sceneName);
    }
}
