using Unity.VisualScripting;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject lvl1;
    [SerializeField] private GameObject lvl2;
    [SerializeField] private GameObject lvl3;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private VoidChannel upgradeRelay;
    [SerializeField] private VoidChannel sellRelay;

    public void Setup(int level = 1)
    {
        DisableAll();
        upgradeButton.SetActive(true);

        switch (level)
        {
            case 1:
                lvl1.SetActive(true);
                break;
            case 2:
                lvl1.SetActive(true);
                lvl2.SetActive(true);
                break;
            case 3:
                lvl1.SetActive(true);
                lvl2.SetActive(true);
                lvl3.SetActive(true);
                upgradeButton.SetActive(false);
                break;
        }
    }
    private void DisableAll()
    {
        lvl1.SetActive(false);
        lvl2.SetActive(false);
        lvl3.SetActive(false);
    }
    public void OnUpgrade()
    {
        upgradeRelay.RaiseEvent();
    }
    public void OnSell()
    {
        Destroy(this.gameObject);
        sellRelay.RaiseEvent();
    }
}
