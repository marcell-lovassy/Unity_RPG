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
                healthValueText.text = healthValueText.text = String.Format("{0:0}%", playerFighter.GetTarget().HealthPercentage);
            }
            else
            {
                healthValueText.text = "N/A";
            }
        }
        
    }
}