using RPG.Core;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float damage = 1f;
    [SerializeField]
    float speed = 1f;

    private Health target;

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void ApplyDamageModifier(float modifier)
    {
        damage += modifier;
    }

    public void SetTarget(Health t)
    {
        target = t;
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
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
