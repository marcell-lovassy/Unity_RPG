using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Controlls
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    public class AIController : MonoBehaviour
    {
        const string PLAYER_TAG = "Player";

        [SerializeField]
        private float chaseDistance = 5f;
        [SerializeField]
        private float suspicionTime = 3f;
        [SerializeField]
        private PatrolPath patrolPath;
        [SerializeField]
        private float patrolSpeed;
        [SerializeField]
        private float waypointTolerance = 1f;
        [SerializeField]
        private float minWaitingTimeAtWaypoint;
        [SerializeField]
        private float maxWaitingTimeAtWaypoint;

        private Mover aiMovement;
        private Fighter fighter;
        private ActionScheduler actionScheduler;
        private GameObject playerObject;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float waitingTime;

        private bool shouldUpdate = true;
        private int currentWaypointIndex = 0;

        private void Start()
        {
            aiMovement = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            actionScheduler = GetComponent<ActionScheduler>();
            playerObject = GameObject.FindWithTag(PLAYER_TAG);
            guardPosition = transform.position;
            GetComponent<Health>().Died += Died;
        }

      
        private void Update()
        {
            if (!shouldUpdate) return;

            if (IsPlayerInRange() && fighter.CanAttack(playerObject))
            {
                aiMovement.ResetSpeed();
                AttackBehaviour();
            }
            else if (IsSuspicionStateActive())
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(playerObject);
        }

        private void PatrolBehaviour()
        {
            aiMovement.SetAgentSpeed(patrolSpeed);
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                    waitingTime = UnityEngine.Random.Range(minWaitingTimeAtWaypoint, maxWaitingTimeAtWaypoint);
                    timeSinceArrivedAtWaypoint = 0;
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waitingTime)
            {
                aiMovement.StartMoveAction(nextPosition);
            }
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

        private void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        private void Died()
        {
            shouldUpdate = false;
            aiMovement.DisableAgent();
        }

        private bool IsSuspicionStateActive()
        {
            return timeSinceLastSawPlayer < suspicionTime;
        }

        private float DistanceToPlayer()
        {
            if(playerObject == null) return -1;

            return Vector3.Distance(playerObject.transform.position, transform.position);
        }

        private bool IsPlayerInRange()
        {
            return DistanceToPlayer() <= chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
