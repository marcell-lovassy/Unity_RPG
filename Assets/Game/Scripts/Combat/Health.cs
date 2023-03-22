using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth;

        float health;


        // Start is called before the first frame update
        void Start()
        {
            health = maxHealth;
        }

        public void TakeDamage(float dmg)
        {
            health = Mathf.Max(health - dmg, 0);
            print(health);
        }
    }
}
