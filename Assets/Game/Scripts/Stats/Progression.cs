using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField]
        ProgressionCharacterClass[] progressionCharacterClasses;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            return GetStatValue(characterClass, Stat.Health, level);
        }
        public float GetExperienceReward(CharacterClass characterClass, int level)
        {
            return GetStatValue(characterClass, Stat.ExperienceReward, level);
        }

        public float GetStatValue(CharacterClass characterClass, Stat stat, int level)
        {
            return progressionCharacterClasses.FirstOrDefault(characterProgression => characterProgression.CharacterClass == characterClass).GetStatValue(stat, level - 1);
        }


        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass CharacterClass => characterClass;

            [SerializeField]
            private CharacterClass characterClass;
            [SerializeField]
            private ProgressionStat[] stats;

            public float GetStatValue(Stat stat, int index)
            {
                ProgressionStat progressionStat = stats.FirstOrDefault(s => s.stat == stat);
                if (progressionStat == null) return 0;
                if (index >= progressionStat.levels.Length) return progressionStat.levels.Last();
                if (0 <= index) return progressionStat.levels[index];
                return 0;
            }
        }


        [Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}