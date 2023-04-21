using RPG.Attributes;
using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField]
        TMP_Text xpValueText;

        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Start()
        {
            experience.ExperiencePointsChanged += UpdateExperienceDisplay;
            xpValueText.text = String.Format("{0:0}", experience.XP);
        }

        private void UpdateExperienceDisplay()
        {
            xpValueText.text = String.Format("{0:0}", experience.XP);
        }

        private void OnDestroy()
        {
            experience.ExperiencePointsChanged -= UpdateExperienceDisplay;
        }
    }
}