using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public event Action OnShopClosed;

    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    private List<WeightedObject<Relic>> regularRelics;
    [SerializeField]
    private List<WeightedObject<Curse>> regularCurses;
    [SerializeField]
    private List<WeightedObject<Relic>> legendaryRelicCursePairs;
    [SerializeField]
    private GameObject shopUI;
    [SerializeField]
    private Button closeShopButton;
    [SerializeField]
    private Button slot1Button;
    [SerializeField]
    private Button slot2Button;
    [SerializeField]
    private Button slot3Button;
    [SerializeField]
    private Button rerollSlot1Button;
    [SerializeField]
    private Button rerollSlot2Button;
    [SerializeField]
    private Button rerollSlot3Button;
    [SerializeField]
    private Image relicImage1;
    [SerializeField]
    private Image relicImage2;
    [SerializeField]
    private Image relicImage3;
    [SerializeField]
    private TextMeshProUGUI relicText1;
    [SerializeField]
    private TextMeshProUGUI relicText2;
    [SerializeField]
    private TextMeshProUGUI relicText3;
    private WeightedObject<Relic> relic1;
    private WeightedObject<Relic> relic2;
    private WeightedObject<Curse> curse1;
    private WeightedObject<Curse> curse2;
    private WeightedObject<Relic> legendaryRelicCursePair;

    private bool choseSlot1 = false;
    private bool choseSlot2 = false;
    private bool choseSlot3 = false;

    private int slotCost1 = 15;
    private int slotCost2 = 30;
    private int slotCost3 = 45;

    private bool didBuy = false;

    void Awake()
    {
        DisableShop();
        closeShopButton.onClick.AddListener(EndShop);
        slot1Button.onClick.AddListener(() => ChooseSlot(1));
        slot2Button.onClick.AddListener(() => ChooseSlot(2));
        slot3Button.onClick.AddListener(() => ChooseSlot(3));
        rerollSlot1Button.onClick.AddListener(() => RerollSlot(1));
        rerollSlot2Button.onClick.AddListener(() => RerollSlot(2));
        rerollSlot3Button.onClick.AddListener(() => RerollSlot(3));
    }
    

    public void EnableShop()
    {
        Time.timeScale = 0;
        rerollSlot1Button.interactable = true;
        rerollSlot2Button.interactable = true;
        rerollSlot3Button.interactable = true;
        choseSlot1 = false;
        choseSlot2 = false;
        choseSlot3 = false;
        didBuy = false;
        slotCost1 = 15 + (playerController.curse3 ? 15 : 0);
        slotCost2 = 30 + (playerController.curse3 ? 15 : 0);
        slotCost3 = 45 + (playerController.curse3 ? 15 : 0);
        relic1 = Utils.GetRandomWeightedObject(regularRelics);
        regularRelics.Remove(relic1);
        relic2 = Utils.GetRandomWeightedObject(regularRelics);
        regularRelics.Remove(relic2);
        curse1 = Utils.GetRandomWeightedObject(regularCurses);
        regularCurses.Remove(curse1);
        curse2 = Utils.GetRandomWeightedObject(regularCurses);
        regularCurses.Remove(curse2);
        legendaryRelicCursePair = Utils.GetRandomWeightedObject(legendaryRelicCursePairs);
        legendaryRelicCursePairs.Remove(legendaryRelicCursePair);
        relicImage1.sprite = relic1.item.relicSprite;
        relicImage2.sprite = relic2.item.relicSprite;
        relicImage3.sprite = legendaryRelicCursePair.item.relicSprite;
        relicText1.text = slotCost1 + " Health\n" + relic1.item.relicName + "\n" + relic1.item.relicDescription + "\n<color=purple>" + (playerController.curse16 ? "???" : curse1.item.curseDescription) + "</color>";
        relicText2.text = slotCost2 + " Health\n" + relic2.item.relicName + "\n" + relic2.item.relicDescription + "\n<color=purple>" + (playerController.curse16 ? "???" : curse2.item.curseDescription) + "</color>";
        relicText3.text = slotCost3 + " Health\n" + legendaryRelicCursePair.item.relicName + "\n"  + legendaryRelicCursePair.item.relicDescription;
        shopUI.SetActive(true);

    }
    public void DisableShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void ChooseSlot(int slotIndex)
    {
        didBuy = true;
        switch (slotIndex)
        {
            case 1:
                playerController.AddRelic(relic1.item);
                playerController.AddCurse(curse1.item);
                playerController.Capitalism(slotCost1);
                slot1Button.interactable = false;
                choseSlot1 = true;
                break;
            case 2:
                playerController.AddRelic(relic2.item);
                playerController.AddCurse(curse2.item);
                playerController.Capitalism(slotCost2);
                slot2Button.interactable = false;
                choseSlot2 = true;
                break;
            case 3:
                playerController.AddRelic(legendaryRelicCursePair.item);
                playerController.Capitalism(slotCost3);
                slot3Button.interactable = false;
                choseSlot3 = true;
                break;
            default:
                Debug.LogError("Invalid slot index: " + slotIndex);
                break;
        }
    }
    public void RerollSlot(int slotIndex)
    {
        if (slotIndex == 1)
        {
            rerollSlot1Button.interactable = false;
            WeightedObject<Relic> newRelic = Utils.GetRandomWeightedObject(regularRelics);
            regularRelics.Remove(newRelic);
            WeightedObject<Curse> newCurse = Utils.GetRandomWeightedObject(regularCurses);
            regularCurses.Remove(newCurse);

            slotCost1 += 15;
            
            if (choseSlot1)
            {
                slot1Button.interactable = true;
                choseSlot1 = false;
                
            }
            else
            {
                regularRelics.Add(relic1);
                regularCurses.Add(curse1);
            }
            
        
            relic1 = newRelic;
            curse1 = newCurse;
            relicImage1.sprite = newRelic.item.relicSprite;
            relicText1.text = slotCost1 + " Health\n" + relic1.item.relicName + "\n" + relic1.item.relicDescription + "\n<color=purple>" + (playerController.curse16 ? "???" : curse1.item.curseDescription) + "</color>";
        }
        else if (slotIndex == 2)
        {
            rerollSlot2Button.interactable = false;
            WeightedObject<Relic> newRelic = Utils.GetRandomWeightedObject(regularRelics);
            regularRelics.Remove(newRelic);
            WeightedObject<Curse> newCurse = Utils.GetRandomWeightedObject(regularCurses);
            regularCurses.Remove(newCurse);
            
            slotCost2 += 15;
            
            if (choseSlot2)
            {
                slot2Button.interactable = true;
                choseSlot2 = false;
            }
            else
            {
                regularRelics.Add(relic2);
                regularCurses.Add(curse2);
            }

            relic2 = newRelic;
            curse2 = newCurse;
            relicImage2.sprite = newRelic.item.relicSprite;
            relicText2.text = slotCost2 + " Health\n" + relic2.item.relicName + "\n" + relic2.item.relicDescription + "\n<color=purple>" + (playerController.curse16 ? "???" : curse2.item.curseDescription) + "</color>"; 
        } else if (slotIndex == 3)
        {
            rerollSlot3Button.interactable = false;
            WeightedObject<Relic> newPair = Utils.GetRandomWeightedObject(legendaryRelicCursePairs);
            legendaryRelicCursePairs.Remove(newPair);
            
            slotCost3 += 15;
            
            if (choseSlot3)
            {
                slot3Button.interactable = true;
                choseSlot3 = false;
            }
            else
                legendaryRelicCursePairs.Add(legendaryRelicCursePair);

            legendaryRelicCursePair = newPair;
            relicImage3.sprite = newPair.item.relicSprite;
            relicText3.text = slotCost3 + " Health\n" + legendaryRelicCursePair.item.relicName + "\n"  + legendaryRelicCursePair.item.relicDescription;
        }
    }

    public void EndShop()
    {
        if(!didBuy && playerController.curse12)
            playerController.Capitalism(30);
        if (choseSlot1)
            slot1Button.interactable = true;
        else
        {
            regularRelics.Add(relic1);
            regularCurses.Add(curse1);
        }
        if (choseSlot2)
            slot2Button.interactable = true;
        else
        {
            regularRelics.Add(relic2);
            regularCurses.Add(curse2);
        }
        if (choseSlot3)
            slot3Button.interactable = true;
        else
            legendaryRelicCursePairs.Add(legendaryRelicCursePair);
        DisableShop();
        OnShopClosed?.Invoke();
    }
}
