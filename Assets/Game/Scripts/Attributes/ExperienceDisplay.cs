using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
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
    }
}