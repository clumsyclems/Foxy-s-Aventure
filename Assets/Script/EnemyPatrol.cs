using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 1f;
    public Transform[] waypoints = null;

    public Transform target = null;
    public int waypoints_index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = waypoints[waypoints_index];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        
        if(dir.normalized.x > 0.3f)
        {
            GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
        }
        else if (dir.normalized.x < 0.3f)
        {
            GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        }
        float distance = Vector3.Distance(transform.position, target.position);
        if ( distance < 0.3f)
        { 
            waypoints_index = (waypoints_index + 1) % waypoints.Length;
            target = waypoints[waypoints_index];

        }
        
    }
}
