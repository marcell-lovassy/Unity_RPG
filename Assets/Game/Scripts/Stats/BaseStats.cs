using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField]
        private int startingLevel = 1;
        [SerializeField]
        private CharacterClass characterClass;
        [SerializeField] 
        private Progression progression = null;

        private void Start()
        {
            var experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.ExperiencePointsChanged += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            startingLevel = GetLevel();
        }

        public float GetHealth() 
        {
            return progression.GetHealth(characterClass, startingLevel);
        }

        public int GetLevel()
        {
            float currentXP = GetComponent<Experience>().XP;
            return progression.GetPlayerLevel(characterClass, currentXP);
        }

        public float GetExperienceReward()
        {
            return progression.GetExperienceReward(characterClass, startingLevel);
        }
    }
}

