using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event UnityAction LevelChanged;
        public event UnityAction OnLevelUp;

        [Range(1, 99)]
        [SerializeField]
        private int startingLevel = 1;
        [SerializeField]
        private CharacterClass characterClass;
        [SerializeField] 
        private Progression progression = null;
        [SerializeField]
        GameObject levelUpVFX;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.ExperiencePointsChanged += UpdateLevel;
                UpdateLevel();
                //force a level changed to show the correct level at start
                LevelChanged?.Invoke();
            }
        }

        public float GetHealth()
        {
            return progression.GetHealth(characterClass, GetCurrentLevel());
        }

        public int GetCurrentLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStatValue(characterClass, stat, GetCurrentLevel());
        }
        public float GetExperienceReward()
        {
            return progression.GetExperienceReward(characterClass, GetCurrentLevel());
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpVFX, transform);
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel) 
            {
                currentLevel = newLevel;
                OnLevelUp?.Invoke();
                LevelChanged?.Invoke();
                LevelUpEffect();
            }
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = experience.XP;
            return progression.GetPlayerLevel(characterClass, currentXP);
        }

    }
}

