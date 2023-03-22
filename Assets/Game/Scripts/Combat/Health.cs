using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        const string DEATH_TRIGGER = "DeathTrigger";

        public bool IsDead => !isAlive;

        [SerializeField]
        private float maxHealth;

        float healthPoints;
        bool isAlive;
        private Animator animator;


        // Start is called before the first frame update
        void Start()
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
            if (!isAlive) return;
            animator.SetTrigger(DEATH_TRIGGER);
            isAlive = false;
        }
    }
}
