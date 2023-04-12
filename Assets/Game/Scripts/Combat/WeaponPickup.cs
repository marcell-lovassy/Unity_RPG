using RPG.Core;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        private float runningTime = 0f;
        private float startingY;

        [Header("Weapon settings")]
        [SerializeField]
        private ItemTier tier;
        [SerializeField]
        private WeaponData weaponData;

        [Header("Hover Settings")]
        [SerializeField]
        private float amplitude = 1f;
        [SerializeField]
        private float frequency = 1f;

        [SerializeField]
        private float hideTime = 1f;

        private Light[] lights;

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
                StartCoroutine(HideForSeconds(hideTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool show)
        {
            GetComponent<Collider>().enabled = show;
            foreach (Transform child in transform) 
            {
                child.gameObject.SetActive(show);
            }
        }
    }
}