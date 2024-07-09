using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    Animator animator;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Vector3 showTransform;

    private string IS_SHOW = "IsShow";
    private bool isShow = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Show(Vector3 position, bool isDisableUpgrade)
    {
        if (isShow)
        {
            Hide();
        }
        else
        {
            ShowUI(position, isDisableUpgrade);
        }
    }

    private IEnumerator WaitAndContinue(float waitTime, Vector3 position, bool isDisableUpgrade)
    {
        yield return new WaitForSeconds(waitTime);
        if (transform.position != position)
        {
            ShowUI(position, isDisableUpgrade);
        }
    }

    private void ShowUI(Vector3 position, bool isDisableUpgrade)
    {
        transform.position = position + showTransform;
        upgradeButton.interactable = !isDisableUpgrade;
        isShow = true;
        animator.SetBool(IS_SHOW, true);
    }

    public void Hide()
    {
        isShow = false;
        animator.SetBool(IS_SHOW, false);
    }

    public void OnUpgradeButtonClick() => BuildManager.Instance.OnTurretUpgrade();
    public void OnDestroyButtonClick() => BuildManager.Instance.OnTurretDestroy();
}
