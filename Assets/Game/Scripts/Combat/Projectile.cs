using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float damage = 1f;
        [SerializeField]
        private float speed = 1f;
        [SerializeField]
        private bool followTarget = false;
        [SerializeField]
        private GameObject hitEffect = null;
        [SerializeField]
        private float maxLifeTime = 10f;
        [SerializeField]
        private float lifetimeAfterImpact = 1f;
        [SerializeField]
        private GameObject[] destroyOnHit;

        private Health target;
        private GameObject instigator = null;
        private GameObject hitEffectInstance;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            //StartCoroutine(CleanUpAfter(10f));
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            if (followTarget && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void ApplyDamageModifier(float modifier)
        {
            damage += modifier;
        }

        public void SetTarget(Health t, GameObject instigator)
        {
            target = t;
            transform.LookAt(GetAimLocation());
            this.instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCapsule.height / 1.5f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            if (other.GetComponent<Health>() == target)
            {
                if (target.IsDead)
                {
                    Destroy(gameObject, maxLifeTime);
                }
                else
                {
                    speed = 0;
                    if (hitEffect != null)
                    {
                        hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                    }
                    target.TakeDamage(instigator, damage);

                    foreach (GameObject toDestroy in destroyOnHit)
                    {
                        Destroy(toDestroy);
                    }

                    Destroy(gameObject, lifetimeAfterImpact);
                }
            }
        }
    }
}