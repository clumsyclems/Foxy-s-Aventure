using System.Collections;
using UnityEngine;

public class Utils
{
    /* Function to verify if the gameobject give in argument is active */
    public static bool IsGameObjectActive(GameObject gameObject)
    {
        return gameObject != null && gameObject.activeInHierarchy;
    }

    public static Vector2 WorldToUV(Vector3 worldPos, Camera mainCamera)
    {
        // Convertit la position monde en position écran (pixels)
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        // Convertit en UV (0-1)
        float uvX = screenPos.x / Screen.width;
        float uvY = screenPos.y / Screen.height;

        return new Vector2(uvX, uvY);
    }

    public static Vector3 UVToWorld(Vector2 uv, Camera mainCamera, float worldZ = 0f)
    {
        // Convertit les coordonnées UV en coordonnées écran (pixels)
        float screenX = uv.x * Screen.width;
        float screenY = uv.y * Screen.height;

        // Convertit la position écran en position monde
        Vector3 screenPos = new Vector3(screenX, screenY, mainCamera.WorldToScreenPoint(new Vector3(0, 0, worldZ)).z);
        return mainCamera.ScreenToWorldPoint(screenPos);
    }


    public static IEnumerator DelayedAction(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}

public struct GameObjectState
{
    public GameObject gameObject;
    public bool isReady;

    public GameObjectState(GameObject gameObject, bool isReady)
    {
        this.gameObject = gameObject;
        this.isReady = isReady;
    }
}
