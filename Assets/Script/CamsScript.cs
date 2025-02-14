using UnityEngine;

public class CamsScript : Singleton<CamsScript>
{
    [SerializeField] Transform backgroundFollowingCamTransform = null;

    protected override void Awake()
    {
        base.Awake();
        backgroundFollowingCamTransform = GameObject.FindGameObjectWithTag("Background").transform;
        if( backgroundFollowingCamTransform == null )
        {
            Debug.Log("No background transform found");
        }
    }

    private void Update()
    {
        if (backgroundFollowingCamTransform != null)
        {
            
            backgroundFollowingCamTransform.transform.position = new Vector3 (Camera.main.transform.position.x, 
                                                                     backgroundFollowingCamTransform.position.y, 
                                                                     backgroundFollowingCamTransform.position.z
            );
        }
    }


}
