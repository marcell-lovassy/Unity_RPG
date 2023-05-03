using RPG.Combat;
using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class TargetHealthDisplay : MonoBehaviour
    {
        [SerializeField]
        TMP_Text healthValueText;

        Fighter playerFighter;

        private void Awake()
        {
            playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if(playerFighter.GetTarget() != null)
            {
                Health health = playerFighter.GetTarget();
                healthValueText.text = healthValueText.text = string.Format("{0}/{1}", health.HealthPoints, health.MaxHealth);
            }
            else
            {
                healthValueText.text = "N/A";
            }
        }
        
    }
}