using Newtonsoft.Json.Linq;
using RPG.Controlls;
using RPG.Core.SavingSystem;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class Health : MonoBehaviour, IJsonSaveable
    {
        const string DEATH_TRIGGER = "DeathTrigger";
        const string REVIVE_TRIGGER = "ReviveTrigger";

        public event UnityAction Died;
        public event UnityAction Revived;

        public bool IsDead => !isAlive;

        [SerializeField]
        private float maxHealth;
        [SerializeField]
        float healthPoints;

        bool isAlive;
        private Animator animator;

        private void Awake()
        {
            healthPoints = maxHealth;
            animator = GetComponent<Animator>();
            isAlive = true;
        }

        public void TakeDamage(float dmg)
        {
            healthPoints = Mathf.Max(healthPoints - dmg, 0);
            if(healthPoints == 0)
            {
                Die();
            }
            print(healthPoints);
        }

        private void Die()
        {
            if (IsDead) return;
            animator.ResetTrigger(REVIVE_TRIGGER);
            animator.SetTrigger(DEATH_TRIGGER);
            isAlive = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            Died?.Invoke();
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(healthPoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            healthPoints = state.ToObject<float>();

            if (healthPoints == 0)
            {
                Die();
            }
            else
            {
                animator.ResetTrigger(DEATH_TRIGGER);
                animator.SetTrigger(REVIVE_TRIGGER);
                Revived?.Invoke();
            }
        }
    }
}
