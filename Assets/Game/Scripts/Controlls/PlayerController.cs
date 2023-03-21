using RPG.Combat;
using RPG.Movement;
using System.Linq;
using UnityEngine;

namespace RPG.Controlls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Mover characterMovement;
        [SerializeField]
        private Fighter fighter;

        void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            print("Nothing to do.");
        }

        private bool InteractWithCombat()
        {
            bool attacking = false;
            var hits = Physics.RaycastAll(GetMouseRay());

            var combatTarget = GetFirstCombatTargetIfAny(hits);
            if (combatTarget != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    fighter.Attack(combatTarget);
                }
                attacking = true;
            }

            return attacking;
        }

        private CombatTarget GetFirstCombatTargetIfAny(RaycastHit[] hits)
        {
            var combatTargets = hits
                .Where(h => h.transform.GetComponent<CombatTarget>() != null)
                .Select(h => h.transform.GetComponent<CombatTarget>());

            return combatTargets.FirstOrDefault();
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit))
            {
                if (Input.GetMouseButton(0))
                {
                    characterMovement.MoveTo(hit.point);
                }
                return true;
            }
            return false;
            //Debug.DrawRay(ray.origin, ray.direction * 100);
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
