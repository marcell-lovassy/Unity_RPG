using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField]
        ProgressionCharacterClass[] progressionCharacterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> progressionLookupTable = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            return GetStatValue(characterClass, Stat.Health, level);
        }
        public float GetExperienceReward(CharacterClass characterClass, int level)
        {
            return GetStatValue(characterClass, Stat.ExperienceReward, level);
        }

        public int GetPlayerLevel(CharacterClass characterClass, float currentXP) 
        {
            int level = 1;
            if (characterClass != CharacterClass.Player) return level;

            Array.ForEach(progressionLookupTable[characterClass][Stat.ExperienceToLevelUp], v =>
            {
                if (v > currentXP) return;
                level++;
            });

            return level;
            
        }

        public float GetStatValue(CharacterClass characterClass, Stat stat, int level)
        {
            BuildProgressionLookupTable();

            var levels = progressionLookupTable[characterClass][stat];
            int levelIndex;
            if (levels.Length < level) levelIndex = levels.Length - 1;
            else if (level < 0) levelIndex = 0;
            else levelIndex = level - 1;

            return progressionLookupTable
                .GetValueOrDefault(characterClass)
                .GetValueOrDefault(stat)[levelIndex];
        }

        private void BuildProgressionLookupTable()
        {
            if (progressionLookupTable != null) return;

            progressionLookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (var progressionClass in progressionCharacterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach (var progressionStat in progressionClass.Stats) 
                {
                    statLookupTable.Add(progressionStat.stat, progressionStat.levels);
                }
                progressionLookupTable.Add(progressionClass.CharacterClass, statLookupTable);
            }
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass CharacterClass => characterClass;
            public ProgressionStat[] Stats => stats;

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