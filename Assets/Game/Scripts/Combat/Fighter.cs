using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, ICharacterAction
    {
        const string ATTACK_TRIGGER = "AttackTrigger";
        const string STOP_ATTACK_TRIGGER = "StopAttackTrigger";

        [SerializeField]
        float weaponRange = 2f;
        [SerializeField]
        float weaponDamage = 5f;
        [SerializeField]
        [Range(0f, 5f)]
        float timeBetweenAttacks = 1f;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Health target;
        private Animator animator;
        float timeSinceLastAttack = 0;


        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null || target.IsDead) return;

            if (IsTagetInRange())
            {
                mover.StopAction();
                DoAttackBehaviour();
            }
            else
            {
                mover.MoveTo(target.transform.position);
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            actionScheduler.StartAction(this);
        }

        public void CancelAttack()
        {
            target = null;
            animator.SetTrigger(STOP_ATTACK_TRIGGER);
        }

        public void StopAction()
        {
            CancelAttack();
        }

        private void DoAttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the Hit() event
                animator.SetTrigger(ATTACK_TRIGGER);
                timeSinceLastAttack = 0;
            }
        }

        //This is an animation event (called from the attack animations)
        private void Hit()
        {
            if(target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool IsTagetInRange()
        {
            return target != null && Vector3.Distance(target.transform.position, transform.position) <= weaponRange;
        }


    }
}

