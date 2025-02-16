using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public Transform goal;
    public Transform start;
    public float speed = 1;

    NavMeshAgent agent;
    PathCreation.Examples.PathFollower path;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        gameController = GameObject.FindObjectOfType<GameController>();
        path = this.GetComponent<PathCreation.Examples.PathFollower>();
        goal = gameController.endPoint;
        start = gameController.startPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, goal.position) <= 0.2f )
        {
            path.hasStarted = false;
        }
        else
        {
            path.hasStarted = true;
        }
    }

    public void Play()
    {
        StartCoroutine(move());
    }

    IEnumerator move()
    {

        yield return new WaitForSeconds(1.5f);

        //After we have waited 5 seconds print the time again.
        path.hasStarted = true;
        path.speed = speed;
        path.speedAnim = speed;
        path.endOfPathInstruction = PathCreation.EndOfPathInstruction.Stop;
        path.pathCreator = gameController.currentCurve;
        //agent.destination = goal.position;
        //agent.speed = speed;

    }
}
