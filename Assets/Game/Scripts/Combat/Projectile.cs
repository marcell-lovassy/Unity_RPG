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

    private Health target;

    private void Start()
    {
        StartCoroutine(DestroyProjectileInSeconds(10f));
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
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator DestroyProjectileInSeconds(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
