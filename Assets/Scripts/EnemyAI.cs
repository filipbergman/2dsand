using UnityEngine;
using Pathfinding;
using System.Collections;

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
    public Path path;

    // The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3f;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    private bool searchingForPlayer = false;

    private void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer() {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null) {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
        }
    }

    IEnumerator UpdatePath() {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        } else {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            yield return new WaitForSeconds(1f / updateRate);
            StartCoroutine(UpdatePath());
        }
    }

    public void OnPathComplete(Path p) {
        //Debug.Log("We got a path. Did it have an error? " + p.error);
        if(!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    // FixedUpdate is good for calculating physics
    void FixedUpdate() {
        if (target == null) {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        // TODO: always look at player?

        if (path == null) {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count) {
            if(pathIsEnded) {
                return;
            }
            StartCoroutine(SearchForPlayer());
            pathIsEnded = true;
            return; 
        }
        pathIsEnded = false;

        // Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        // Move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }
}   

