using RPG.Combat;
using RPG.Core;
using RPG.Movement;
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

        private Mover aiMovement;
        private Fighter fighter;
        private GameObject playerObject;

        private bool shouldUpdate = true;

        private void Start()
        {
            aiMovement = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            playerObject = GameObject.FindWithTag(PLAYER_TAG);
            GetComponent<Health>().Died += Died;
        }

        private void Died()
        {
            shouldUpdate = false;
            aiMovement.DisableAgent();
        }

        private void Update()
        {
            if (!shouldUpdate) return;

            if (IsPlayerInRange() && fighter.CanAttack(playerObject))
            {
                fighter.Attack(playerObject);
            }
            else
            {
                fighter.StopAction();
            }
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
