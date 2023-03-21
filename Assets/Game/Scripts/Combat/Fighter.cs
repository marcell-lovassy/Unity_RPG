using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, ICharacterAction
    {
        [SerializeField]
        float weaponRange = 2f;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Transform target;


        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            if (target == null) return;

            if (IsTagetInRange())
            {
                mover.StopAction();
            }
            else
            {
                mover.MoveTo(target.position);
            }
        }

        private bool IsTagetInRange()
        {
            return target != null && Vector3.Distance(target.position, transform.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
            actionScheduler.StartAction(this);
        }

        public void CancelAttack()
        {
            target = null;
        }

        public void StopAction()
        {
            CancelAttack();
        }
    }
}

