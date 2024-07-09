using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }
    [SerializeField] private GameObject turretDataManager;
    private List<TurretData> turretData;

    public TurretData selectedTurretData;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int money = 100;
    [SerializeField] private UpgradeUI upgradeUI;
    private Animator animator;

    private MapCube upgradeCube;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        InitTurretData();
        animator = moneyText.GetComponent<Animator>();
        ChangeMoney(0);
    }

    private void InitTurretData()
    {
        TurretData[] temp = turretDataManager.GetComponentsInChildren<TurretData>();
        turretData = new List<TurretData>(temp);
    }

    public void OnStandardSelected(bool isOn) { if (isOn) SelectTurret(TurretType.Standard); }
    public void OnMissileSelected(bool isOn) { if (isOn) SelectTurret(TurretType.Missile); }
    public void OnLaserSelected(bool isOn) { if (isOn) SelectTurret(TurretType.Laser); }


    private void SelectTurret(TurretType type)
    {
        selectedTurretData = turretData.Find(x => x.type == type);
    }

    public bool IsEnoughMoney(int need)
    {
        if (money >= need)
        {
            return true;
        }
        else
        {
            LackOfMoney();
            return false;
        }
    }

    public void LackOfMoney()
    {
        animator.SetTrigger("LackOfMoney");
    }

    public void ChangeMoney(int value)
    {
        money += value;
        moneyText.text = "￥" + money.ToString();
    }

    public void ShowUpgradeUI(MapCube cube, Vector3 position, bool isDisableUpgrade)
    {
        upgradeCube = cube;
        upgradeUI.Show(position, isDisableUpgrade);
    }
    public void HideUpgradeUI() => upgradeUI.Hide();

    public void OnTurretUpgrade()
    {
        upgradeCube?.OnTurretUpgrade();
        HideUpgradeUI();
    }

    public void OnTurretDestroy()
    {
        upgradeCube?.OnTurretDestroy();
        HideUpgradeUI();
    }
}


