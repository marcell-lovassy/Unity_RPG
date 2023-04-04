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
        [Range(0f, 5f)]
        float timeBetweenAttacks = 1f;
        [SerializeField]
        Transform handTransform = null;
        [SerializeField]
        WeaponData defaultWeaponData = null;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Health target;
        private WeaponData currentWeapon;
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
            EquipWeapon(defaultWeaponData);
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

        public void EquipWeapon(WeaponData weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(handTransform, animator);
        }

        //This is an animation event (called from the attack animations)
        private void Hit()
        {
            if(target == null) return;
            target.TakeDamage(currentWeapon.Damage);
        }

        private bool IsTagetInRange()
        {
            return target != null && Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.Range;
        }
    }
}

