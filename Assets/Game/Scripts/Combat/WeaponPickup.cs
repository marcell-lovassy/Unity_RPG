using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        float runningTime = 0f;
        float startingY;

        [Header("Weapon settings")]
        [SerializeField]
        ItemTier tier;
        [SerializeField]
        WeaponData weaponData;

        [Header("Hover Settings")]
        [SerializeField]
        float amplitude = 1f;
        [SerializeField]
        float frequency = 1f;

        Light[] lights;

        private void Start()
        {
            startingY = transform.position.y;
            lights = GetComponentsInChildren<Light>();

            foreach (Light light in lights)
            {
                light.color = tier.Color;
            }
        }

        void Update()
        {
            float y = startingY + amplitude * Mathf.Sin(2 * Mathf.PI * runningTime * frequency);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            runningTime += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weaponData);
                Destroy(gameObject);
            }
        }
    }
}