using Newtonsoft.Json.Linq;
using RPG.Core.SavingSystem;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, IJsonSaveable
    {
        //public delegate void ExperiencePointsChanged();
        public event UnityAction ExperiencePointsChanged;

        public float XP => experiencePoints;

        [SerializeField]
        private float experiencePoints = 0;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            ExperiencePointsChanged?.Invoke();
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(experiencePoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            experiencePoints = state.Value<float>();
            ExperiencePointsChanged?.Invoke();
        }
    }
}

