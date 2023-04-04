using RPG.Core;
using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float damage = 1f;
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    bool followTarget = false;
    [SerializeField]
    GameObject hitEffect = null;
    [SerializeField]
    GameObject mesh;
    [SerializeField]
    GameObject trail;

    private Health target;
    GameObject hitEffectInstance;

    private void Start()
    {
        StartCoroutine(CleanUpAfter(10f));
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

    public void SetTarget(Health t)
    {
        target = t;
        transform.LookAt(GetAimLocation());
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if(targetCapsule == null)
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

        if(other.GetComponent<Health>() == target)
        {
            if (target.IsDead)
            {
                StartCoroutine(DestroyProjectileInSeconds(5f));
            }
            else
            {
                if(hitEffect != null)
                {
                    hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                }
                target.TakeDamage(damage);
                StartCoroutine(DestroyProjectileInSeconds(0.25f));
            }
        }
    }

    private IEnumerator DestroyProjectileInSeconds(float time)
    {
        if (!target.IsDead)
        {
            Destroy(mesh);
            Destroy(trail);
        }
        yield return new WaitForSeconds(time);
        if(hitEffectInstance != null)
        {
            Destroy(hitEffectInstance);
        }
        Destroy(gameObject);
    }

    private IEnumerator CleanUpAfter(float time)
    {
        yield return new WaitForSeconds(time);
        if (hitEffectInstance != null)
        {
            Destroy(hitEffectInstance);
        }
        Destroy(gameObject);
    }
}
