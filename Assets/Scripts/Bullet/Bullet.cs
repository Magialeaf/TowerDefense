using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] float speed = 10f;

    private Transform target;
    [SerializeField] private GameObject bulletExplosion;

    private void Update()
    {
        Attack();
    }

    public virtual void Attack()
    {
        if (target == null)
        {
            Dead();
            return;
        }

        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            Dead();
            target.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public void SetTarget(Transform _target) => target = _target;

    private void Dead()
    {
        Destroy(gameObject);

        if (target != null)
        {
            GameObject tempEffect = Instantiate(bulletExplosion, transform.position, Quaternion.identity);
            tempEffect.transform.parent = target.transform;
            Destroy(tempEffect, 1f);
        }
    }
}
