using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCube : MonoBehaviour
{
    private GameObject turretGo;
    private TurretData turretData;
    private bool isUpgrade = false;

    [SerializeField] private GameObject buildEffect;

    public void Build(object sender, EventArgs e)
    {
        TurretData selectedTD = BuildManager.Instance.selectedTurretData;

        if (selectedTD == null || selectedTD.turretPrefab == null) return;
        if (turretGo != null) return;

        BuildTurret(selectedTD);
    }

    public bool IsBuilt() => turretGo != null;
    public bool IsUpgrade() => isUpgrade;

    private void BuildTurret(TurretData _turretData)
    {

        if (BuildManager.Instance.IsEnoughMoney(_turretData.cost) == false) return;
        BuildManager.Instance.ChangeMoney(-_turretData.cost);

        turretData = _turretData;
        turretGo = InstantiateTurret(_turretData.turretPrefab);
    }

    public void OnTurretUpgrade()
    {
        if (BuildManager.Instance.IsEnoughMoney(turretData.costUpgraded))
        {
            BuildManager.Instance.ChangeMoney(-turretData.costUpgraded);
            Destroy(turretGo);
            turretGo = InstantiateTurret(turretData.turretUpgradedPrefab);
            isUpgrade = true;
        }
    }

    public void OnTurretDestroy()
    {
        Destroy(turretGo);
        if (!isUpgrade)
        {
            BuildManager.Instance.ChangeMoney((int)(turretData.cost * 0.4f));
        }
        else
        {
            BuildManager.Instance.ChangeMoney((int)((turretData.cost + turretData.costUpgraded) * 0.4f));
        }

        turretGo = null;
        turretData = null;
        isUpgrade = false;
        InstanitateEffect();
    }

    private GameObject InstantiateTurret(GameObject prefab)
    {
        GameObject turretGo = GameObject.Instantiate(prefab, transform.position + turretData.turretPrefab.transform.position, Quaternion.identity);
        InstanitateEffect();
        return turretGo;
    }
    private void InstanitateEffect()
    {
        GameObject tempEffect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(tempEffect, 2f);
    }

    //private bool IsClickedObjectSelf()
    //{
    //    // Get mouse position from Input System
    //    Vector2 mousePosition = Mouse.current.position.ReadValue();

    //    // Perform a raycast into the scene from the screen point
    //    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    //    RaycastHit hitInfo;

    //    // Check if the ray hits this object
    //    if (Physics.Raycast(ray, out hitInfo))
    //    {
    //        return hitInfo.collider.gameObject == gameObject;
    //    }

    //    return false;
    //}
}