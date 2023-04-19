using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField]
        TMP_Text healthValueText;

        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Start()
        {
            health.HealthChanged += UpdateHealthDisplay;
            healthValueText.text = String.Format("{0:0}%", health.HealthPercentage);
        }

        private void UpdateHealthDisplay()
        {
            healthValueText.text = String.Format("{0:0}%" , health.HealthPercentage);
        }
    }
}