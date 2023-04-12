using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Linq;
using UnityEngine;

namespace RPG.Controlls
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
        private Mover playerMovement;
        private bool shouldUpdate = true;

        private void Awake()
        {
            playerMovement = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            
            GetComponent<Health>().Died += Died;
        }

        private void Died()
        {
            shouldUpdate = false;
            playerMovement.DisableAgent();
            GetComponent<Collider>().enabled = false;
        }

        void Update()
        {
            if (!shouldUpdate) return;

            if (InteractWithCombat())
            {
                return;
            }
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            bool attacking = false;
            var hits = Physics.RaycastAll(GetMouseRay());

            var combatTarget = GetFirstCombatTargetIfAny(hits);
            if (combatTarget != null)
            {
                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(combatTarget);
                }
                attacking = true;
            }

            return attacking;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit))
            {
                if (Input.GetMouseButton(0))
                {
                    playerMovement.StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private GameObject GetFirstCombatTargetIfAny(RaycastHit[] hits)
        {
            var combatTargets = hits
                .Where(h => h.transform.GetComponent<CombatTarget>() != null && fighter.CanAttack(h.transform.gameObject))
                .Select(h => h.transform.gameObject);

            return combatTargets.
                OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).
                FirstOrDefault();
        }

        

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
