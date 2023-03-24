using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : AiBehaviour
{
    const string PLAYER_TAG = "Player";

    [SerializeField]
    private float chaseDistance = 5f;
    [SerializeField]
    private float suspicionTime = 3f;

    private GameObject playerObject;
    private Fighter fighter;
    private Mover aiMovement;
    private ActionScheduler actionScheduler;

    private float timeSinceLastSawPlayer = Mathf.Infinity;

    private void Start()
    {
        playerObject = GameObject.FindWithTag(PLAYER_TAG);
        fighter = GetComponent<Fighter>();
        aiMovement = GetComponent<Mover>();
        actionScheduler = GetComponent<ActionScheduler>();
    }

    public override void StartBehaviour()
    {
        if (!CanDoAttackBehaviour())
        {
            if (IsSuspicionStateActive())
            {
                DoSuspicionBehaviour();
            }
        }
        else
        {
            timeSinceLastSawPlayer = 0;
            DoAttackBehaviour();
        }
        timeSinceLastSawPlayer += Time.deltaTime;
    }

    public override void StopBehaviour()
    {
        actionScheduler.CancelCurrentAction();
    }

    public override bool CanDoBehaviour()
    {
        return CanDoAttackBehaviour() || IsSuspicionStateActive();
    }

    private bool CanDoAttackBehaviour()
    {
        return IsPlayerInRange() && fighter.CanAttack(playerObject);
    }

    private float DistanceToPlayer()
    {
        if (playerObject == null) return -1;

        return Vector3.Distance(playerObject.transform.position, transform.position);
    }

    private bool IsPlayerInRange()
    {
        return DistanceToPlayer() <= chaseDistance;
    }

    private void DoAttackBehaviour()
    {
        aiMovement.ResetSpeed();
        fighter.Attack(playerObject);
    }

    private void DoSuspicionBehaviour()
    {
        actionScheduler.CancelCurrentAction();
    }

    private bool IsSuspicionStateActive()
    {
        return timeSinceLastSawPlayer < suspicionTime;
    }
}
