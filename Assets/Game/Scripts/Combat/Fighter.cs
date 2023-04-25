using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Core.SavingSystem;
using RPG.Movement;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, ICharacterAction, IJsonSaveable
    {
        const string ATTACK_TRIGGER = "AttackTrigger";
        const string STOP_ATTACK_TRIGGER = "StopAttackTrigger";

        [SerializeField]
        [Range(0f, 5f)]
        private float timeBetweenAttacks = 1f;
        [SerializeField]
        private Transform rightHandTransform = null;
        [SerializeField]
        private Transform leftHandTransform = null;
        [SerializeField]
        private WeaponData defaultWeaponData = null;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Health target;
        private WeaponData currentWeapon;
        private GameObject currentWeaponObject;
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
            EquipWeapon(currentWeapon != null ? currentWeapon : defaultWeaponData);
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

        public void EquipWeapon(WeaponData weapon)
        {
            Destroy(currentWeaponObject);
            currentWeapon = weapon;
            currentWeaponObject = weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
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

        //This is an animation event (called from the attack animations)
        private void Hit()
        {
            if(target == null) return;

            //this damage is the base damage of the Player level (it increases at every level up)
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(target, rightHandTransform, leftHandTransform, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        //This is an animation event (called from the shoot animations)
        private void Shoot()
        {
            Hit();
        }

        private bool IsTagetInRange()
        {
            return target != null && Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.Range;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(currentWeapon.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            currentWeapon = Resources.Load<WeaponData>(state.ToString());
            EquipWeapon(currentWeapon);
        }
    }
}

