using RPG.Controlls;
using RPG.Movement;
using UnityEngine;

public class PatrolBehaviour : AiBehaviour
{
    [SerializeField]
    private PatrolPath patrolPath;
    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private float waypointTolerance = 1f;

    private Mover aiMovement;
    private Vector3 guardPosition;

    private int currentWaypointIndex = 0;

    private void Start()
    {
        guardPosition = transform.position;
        aiMovement = GetComponent<Mover>();
    }

    public override void StartBehaviour()
    {
        aiMovement.SetAgentSpeed(patrolSpeed);
        Vector3 nextPosition = guardPosition;

        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
        }
        aiMovement.StartMoveAction(nextPosition);
    }

    public override void StopBehaviour()
    {
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private bool AtWaypoint()
    {
        return Vector3.Distance(GetCurrentWaypoint(), transform.position) < waypointTolerance;
    }
}
