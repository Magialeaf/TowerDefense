using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private int pointIndex = 0;
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float maxHealth = 100f;
    private float health;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private int getMoney = 0;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject enemyDieEffect;

    private void Start()
    {
        targetPosition = WayPoints.Instance.GetWayPoint(pointIndex);
        health = maxHealth * (1 + (EnemySpawner.Instance.getLevel() / 4f));
        hpSlider.maxValue = health;
        hpSlider.value = health;

    }

    private void Update()
    {
        transform.Translate((targetPosition - transform.position).normalized * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            MoveNextPoint();
        }
    }

    private void MoveNextPoint()
    {
        pointIndex++;
        if (pointIndex > WayPoints.Instance.GetLength() - 1)
        {
            Die();
            GameManager.Instance.Fail();
            return;
        }

        targetPosition = WayPoints.Instance.GetWayPoint(pointIndex);
    }

    private void Die()
    {
        Destroy(gameObject);
        GameObject tempEffect = Instantiate(enemyDieEffect, transform.position, Quaternion.identity);
        Destroy(tempEffect, 1f);

        EnemySpawner.Instance.DecreaseEnemyCount();
        BuildManager.Instance.ChangeMoney(getMoney);
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        hpSlider.value = health;
    }
}
