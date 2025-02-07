using UnityEngine;

public class Abstract_Canvas<T> : Singleton<T> where T : MonoBehaviour 
{
    [SerializeField] private Canvas myCanvas = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        myCanvas.worldCamera = Camera.main;
    }
}
