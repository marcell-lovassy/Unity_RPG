using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "RPG/Weapons/Make new WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        public float WeaponDamage => weaponDamage;
        public float WeaponRange => weaponRange;

        [SerializeField]
        GameObject equippedPrefab = null;
        [SerializeField]
        float weaponRange = 5;
        [SerializeField]
        float weaponDamage = 15f;
        [SerializeField]
        AnimatorOverrideController weaponAnimatorOverride = null;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if(equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);
            }

            if(weaponAnimatorOverride != null)
            {
                animator.runtimeAnimatorController = weaponAnimatorOverride;
            }
        }
    }
}
