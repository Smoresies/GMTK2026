using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shopUI;
    [SerializeField]
    private Button closeShopButton;

    void Awake()
    {
        DisableShop();
        closeShopButton.onClick.AddListener(DisableShop);
    }
    

    public void EnableShop()
    {
        shopUI.SetActive(true);
    }
    public void DisableShop()
    {
        shopUI.SetActive(false);
    }
}
