using UnityEngine;

public class Utils
{
    /* Function to verify if the gameobject give in argument is active */
    public static bool IsGameObjectActive(GameObject gameObject)
    {
        return gameObject != null && gameObject.activeInHierarchy;
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
