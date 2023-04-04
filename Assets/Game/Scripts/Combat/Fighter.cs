using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Movement;
using System;
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
        [SerializeField]
        GameObject weaponPrefab = null;
        [SerializeField]
        Transform handTransform = null;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Health target;
        private Animator animator;
        float timeSinceLastAttack = 0;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SpawnWeapon();
            timeSinceLastAttack = timeBetweenAttacks;
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

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            actionScheduler.StartAction(this);
        }
        public void StopAction()
        {
            CancelAttack();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead;
        }

        private void CancelAttack()
        {
            target = null;
            TriggerStopAttack();
            mover.StopAction();
        }

        private void DoAttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger(STOP_ATTACK_TRIGGER);
            animator.SetTrigger(ATTACK_TRIGGER);
        }

        private void TriggerStopAttack()
        {
            animator.ResetTrigger(ATTACK_TRIGGER);
            animator.SetTrigger(STOP_ATTACK_TRIGGER);
        }

        private void SpawnWeapon()
        {
            if(weaponPrefab != null)
            {
                Instantiate(weaponPrefab, handTransform);
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

