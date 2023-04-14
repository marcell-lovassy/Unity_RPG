using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "RPG/Weapons/Make new WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        public float Damage => damage;
        public float Range => range;

        [SerializeField]
        GameObject equippedPrefab = null;
        [SerializeField]
        Projectile projectile = null;
        [SerializeField]
        float range = 2;
        [SerializeField]
        float damage = 5f;
        [SerializeField]
        bool rightHanded = true;
        [SerializeField]
        AnimatorOverrideController weaponAnimatorOverride = null;

        public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            GameObject weapon = null;
            if(equippedPrefab != null)
            {
                weapon = Instantiate(equippedPrefab, GetActiveHand(rightHand, leftHand));
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if(weaponAnimatorOverride != null)
            {
                animator.runtimeAnimatorController = weaponAnimatorOverride;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Health target, Transform rightHand, Transform leftHand)
        {
            Projectile projectileInstance = SpawnProjectile(GetActiveHand(rightHand, leftHand));
            projectileInstance.SetTarget(target);
            projectileInstance.ApplyDamageModifier(damage);
        }

        private Transform GetActiveHand(Transform rightHand, Transform leftHand)
        {
            return rightHanded ? rightHand : leftHand;
        }

        private Projectile SpawnProjectile(Transform spawnPosition)
        {
            return Instantiate(projectile, spawnPosition.position, Quaternion.identity);
        }
    }
}
