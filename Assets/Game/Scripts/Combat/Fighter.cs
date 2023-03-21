using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField]
        float weaponRange = 2f;

        private Mover mover;
        private Transform target;


        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            if (target == null) return;

            if (IsTagetInRange())
            {
                mover.Stop();
            }
            else
            {
                mover.MoveTo(target.position);
            }
        }

        private bool IsTagetInRange()
        {
            return target != null && Vector3.Distance(target.position, transform.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
            Debug.Log("attack");
        }

        public void CancelAttack()
        {
            target = null;
        }
    }
}

