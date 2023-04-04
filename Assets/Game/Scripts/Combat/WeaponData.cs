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
        float range = 5;
        [SerializeField]
        float damage = 15f;
        [SerializeField]
        bool rightHanded = true;
        [SerializeField]
        AnimatorOverrideController weaponAnimatorOverride = null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if(equippedPrefab != null)
            {
                Instantiate(equippedPrefab, rightHanded ? rightHand : leftHand);
            }

            if(weaponAnimatorOverride != null)
            {
                animator.runtimeAnimatorController = weaponAnimatorOverride;
            }
        }
    }
}
