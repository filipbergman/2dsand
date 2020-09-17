using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {
    
    // What to chase
    public Transform target;

    // How many times a second we will update our path
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    // The calculated path
    public Pathfinding path;

    // The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWayPoint = 0;

    void start () {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null) {
            Debug.LogError("TODO: No player found, panicc");
            return;
        }
    
        // Start a new path to the target position and return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        startCoroutine(UpdatePath);



    }

    public void OnPathComplete(Path p) {
        Debug.Log("We got a path, did it have an error?" + p.error)
        if(!p.error) {
            path = p;
            currentWayPoint = 0;
        }
    
    
    }








}
