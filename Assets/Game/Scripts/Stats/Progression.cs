using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField]
        ProgressionCharacterClass[] progressionCharacterClass;

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass CharacterClass => characterClass;

            [SerializeField]
            CharacterClass characterClass;
            [SerializeField]
            float[] healthByLevel;

            public float GetHealth(int index)
            {
                if (index >= healthByLevel.Length) return healthByLevel.Last();
                if(0 <= index) return healthByLevel[index];
                return 0;
            }
        }

        public float GetHealth(CharacterClass characterClass, int level)
        {
            return progressionCharacterClass.FirstOrDefault(characterProgression => characterProgression.CharacterClass == characterClass).GetHealth(level - 1);
        }
    }
}