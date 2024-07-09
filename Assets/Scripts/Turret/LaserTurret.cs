using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret
{
    [SerializeField] private float damagePerSecond = 60;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject laserEffectPrefab;
    private GameObject laserEffectExample;

    protected override void Start()
    {
        base.Start();
        laserEffectExample = Instantiate(laserEffectPrefab, bulletSpawnPoint.position, Quaternion.identity);
        laserEffectExample.SetActive(false);
    }


    protected override void Attack()
    {
        Transform target = GetTarget();

        if (target == null)
        {
            laserEffectExample.SetActive(false);
            lineRenderer.enabled = false;
            return;
        }
        else
        {
            target.GetComponent<Enemy>().TakeDamage(damagePerSecond * Time.deltaTime);
            laserEffectExample.SetActive(true);
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y = bulletSpawnPoint.position.y;
            laserEffectExample.transform.LookAt(lookAtPosition);
            laserEffectExample.transform.position = target.position + laserEffectPrefab.transform.position;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, bulletSpawnPoint.position);
            lineRenderer.SetPosition(1, target.position);
        }
    }

    public void OnDestroy()
    {
        Destroy(laserEffectExample);
    }
}
