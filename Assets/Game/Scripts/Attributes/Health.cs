using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Core.SavingSystem;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IJsonSaveable
    {
        const string DEATH_TRIGGER = "DeathTrigger";
        const string REVIVE_TRIGGER = "ReviveTrigger";

        public event UnityAction Died;
        public event UnityAction Revived;
        public event UnityAction HealthChanged;

        public bool IsDead => !isAlive;
        public float HealthPoints => healthPoints;
        public float HealthPercentage => GetHealthPercentage();

        [SerializeField]
        private float regenerationPercentageOnLevelUp = 100f;
        
        float healthPoints = -1f;



        bool isAlive;
        private Animator animator;
        BaseStats stats;

        private void Awake()
        {
            stats = GetComponent<BaseStats>();
            if(healthPoints < 0)
            {
                healthPoints = stats.GetHealth();
            }
            stats.LevelChanged += RefreshHealth;
            stats.OnLevelUp += RegenerateHealthOnLevelUp;
            animator = GetComponent<Animator>();
            isAlive = true;
        }

        private void RegenerateHealthOnLevelUp()
        {
            float regenHealthPoints = stats.GetHealth() * (regenerationPercentageOnLevelUp / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
            HealthChanged?.Invoke();
        }

        private void RefreshHealth()
        {
            HealthChanged?.Invoke();
        }

        public float GetHealthPercentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetHealth()) * 100f;
        }

        public void TakeDamage(GameObject instigator, float dmg)
        {
            healthPoints = Mathf.Max(healthPoints - dmg, 0);
            if(healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }

            HealthChanged?.Invoke();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        private void Die()
        {
            //if (IsDead) return;
            animator.ResetTrigger(REVIVE_TRIGGER);
            animator.SetTrigger(DEATH_TRIGGER);
            isAlive = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            Died?.Invoke();
        }

        public JToken CaptureAsJToken()
        {
            //return JToken.FromObject(healthPoints);
            return JToken.FromObject(new HealthData(isAlive, healthPoints));
        }

        public void RestoreFromJToken(JToken state)
        {
            var healthData = state.ToObject<HealthData>();
            isAlive = healthData.IsAlive;
            healthPoints = healthData.HealthPoints;
            HealthChanged?.Invoke();

            if (healthPoints == 0)
            {
                Die();
            }
            else
            {
                isAlive = true;
                animator.ResetTrigger(DEATH_TRIGGER);
                animator.SetTrigger(REVIVE_TRIGGER);
                Revived?.Invoke();
            }
        }
    }

    struct HealthData
    {
        public bool IsAlive { get; set; }
        public float HealthPoints { get; set; }

        public HealthData(bool isAlive, float health)
        {
            IsAlive = isAlive;
            HealthPoints = health;
        }
    }
}
