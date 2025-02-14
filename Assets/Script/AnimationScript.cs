using System.Collections;
using UnityEngine;

public class AnimationScript : Singleton<AnimationScript>
{
    public IEnumerator StartScene(float circleRadiusInUV, float playerAnimationDuration, float circleExpandDuration)
    {
        Vector2 cercleEffectPosition = Utils.WorldToUV(PlayerScript.Instance.transform.position, Camera.main);
        cercleEffectPosition.x = (cercleEffectPosition.x > circleRadiusInUV) ? cercleEffectPosition.x : circleRadiusInUV / 3;
        Vector2 cercleEffectPositionXY = Utils.UVToWorld(cercleEffectPosition, Camera.main);

        if (Mathf.Abs(PlayerScript.Instance.transform.position.x - cercleEffectPositionXY.x) > 0.1)
        {
            yield return StartCoroutine(CircleRevealEffect.ExpandRadius(cercleEffectPosition, 0f, circleRadiusInUV, circleExpandDuration));

            yield return StartCoroutine(PlayerScript.PlayerMovementAnimation(cercleEffectPositionXY, playerAnimationDuration));

            yield return StartCoroutine(CircleRevealEffect.ExpandRadius(cercleEffectPosition, circleRadiusInUV, 2.5f, circleExpandDuration));
        }
        else
        {
            yield return StartCoroutine(CircleRevealEffect.ExpandRadius(cercleEffectPosition, 0f, 2.5f, circleExpandDuration));
        }

            GameManagerScript.Instance.IsTransparent = true;
    }
}
