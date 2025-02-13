using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircleRevealEffect : Singleton<CircleRevealEffect>
{
    [SerializeField] private Image image = null;

    protected override void Awake()
    {
        base.Awake();
        Instance.image.material.SetFloat("_Radius", 0f);
    }

    public static IEnumerator ExpandRadius(Vector2 startPosition, float startRadius, float finalRadius, float duration)
    {
        Instance.image.material.SetVector("_Center", startPosition);
        Instance.image.material.SetFloat("_Radius", startRadius);

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float radius = Mathf.Lerp(startRadius, finalRadius, time / duration);
            Instance.image.material.SetFloat("_Radius", radius);
            yield return null;
        }
        Instance.image.material.SetFloat("_Radius", finalRadius);
    }

}
