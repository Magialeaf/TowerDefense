using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float attackRange = 1f;
    private float nextAttackTime = 0f;

    private SphereCollider sphereCollider;

    [SerializeField] protected Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform head;
    private void Update()
    {
        Attack();
        DirectionControl();
    }

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    protected virtual void Start()
    {
        sphereCollider.radius = attackRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyList.Add(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyList.Remove(other.gameObject);
        }
    }

    protected virtual void Attack()
    {
        if (enemyList == null || enemyList.Count == 0) return;

        if (Time.time > nextAttackTime)
        {
            Transform target = GetTarget();
            if (target != null)
            {
                GameObject tempBullet = GameObject.Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
                tempBullet.GetComponent<Bullet>().SetTarget(target);
                nextAttackTime = Time.time + attackRate;
            }

        }
    }

    private void DirectionControl()
    {
        if (enemyList == null || enemyList.Count == 0) return;

        Transform target = GetTarget();
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            head.rotation = Quaternion.Slerp(head.rotation, lookRotation, 0.1f);
        }
    }

    public Transform GetTarget()
    {
        if (enemyList != null && enemyList.Count > 0 && enemyList[0] != null) return enemyList[0].transform;
        if (enemyList == null || enemyList.Count == 0) return null;

        List<int> indexList = new List<int>();
        for (int i = 0; i < enemyList.Count; i++)
        {
            // Unity重写了Equals，为了防止对象滞留可以用null.Equals(null)进行一次严谨的判断
            // if (enemyList[i] == null || enemyList[i].Equals(null))
            if (enemyList[i] == null)
            {
                indexList.Add(i);
            }
        }
        for (int i = indexList.Count - 1; i >= 0; i--)
        {
            enemyList.RemoveAt(indexList[i]);
        }

        if (enemyList != null && enemyList.Count > 0)
        {
            return enemyList[0].transform;
        }
        return null;
    }
}
