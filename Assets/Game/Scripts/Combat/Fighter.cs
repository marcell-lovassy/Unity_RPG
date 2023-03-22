using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, ICharacterAction
    {
        const string ATTACK_TRIGGER = "AttackTrigger";

        [SerializeField]
        float weaponRange = 2f;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Transform target;
        private Animator animator;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (target == null) return;

            if (IsTagetInRange())
            {
                mover.StopAction();
                DoAttackBehaviour();
            }
            else
            {
                mover.MoveTo(target.position);
            }
        }

        private void DoAttackBehaviour()
        {
            animator.SetTrigger(ATTACK_TRIGGER);
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

        //This is an animation event (called from the attack animations)
        private void Hit()
        {

        }
    }
}

