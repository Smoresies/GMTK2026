using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    private List<WeightedObject<Relic>> regularRelics;
    [SerializeField]
    private List<WeightedObject<Curse>> regularCurses;
    [SerializeField]
    private List<WeightedObject<RelicCursePair>> legendaryRelicCursePairs;
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
    [SerializeField]
    private TextMeshProUGUI curseText1;
    [SerializeField]
    private TextMeshProUGUI curseText2;
    [SerializeField]
    private TextMeshProUGUI curseText3;
    [SerializeField]
    private TextMeshProUGUI relicDescriptionText1;
    [SerializeField]
    private TextMeshProUGUI relicDescriptionText2;
    [SerializeField]
    private TextMeshProUGUI relicDescriptionText3;
    private WeightedObject<Relic> relic1;
    private WeightedObject<Relic> relic2;
    private WeightedObject<Curse> curse1;
    private WeightedObject<Curse> curse2;
    private WeightedObject<RelicCursePair> legendaryRelicCursePair;

    private bool choseSlot1 = false;
    private bool choseSlot2 = false;
    private bool choseSlot3 = false;

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
        rerollSlot1Button.interactable = true;
        rerollSlot2Button.interactable = true;
        rerollSlot3Button.interactable = true;
        choseSlot1 = false;
        choseSlot2 = false;
        choseSlot3 = false;
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
        relicImage3.sprite = legendaryRelicCursePair.item.relic.relicSprite;
        relicText1.text = relic1.item.relicName;
        relicText2.text = relic2.item.relicName;
        relicText3.text = legendaryRelicCursePair.item.relic.relicName;
        curseText1.text = curse1.item.curseDescription;
        curseText2.text = curse2.item.curseDescription;
        curseText3.text = legendaryRelicCursePair.item.curse.curseDescription;
        relicDescriptionText1.text = relic1.item.relicDescription;
        relicDescriptionText2.text = relic2.item.relicDescription;
        relicDescriptionText3.text = legendaryRelicCursePair.item.relic.relicDescription;
        shopUI.SetActive(true);

    }
    public void DisableShop()
    {
        shopUI.SetActive(false);
    }
    public void ChooseSlot(int slotIndex)
    {
        switch (slotIndex)
        {
            case 1:
                choseSlot1 = !choseSlot1;
                break;
            case 2:
                choseSlot2 = !choseSlot2;
                break;
            case 3:
                choseSlot3 = !choseSlot3;
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

            regularRelics.Add(relic1);
            regularCurses.Add(curse1);

            relic1 = newRelic;
            curse1 = newCurse;
            relicImage1.sprite = newRelic.item.relicSprite;
            relicText1.text = newRelic.item.relicName;
            curseText1.text = newCurse.item.curseDescription;
            relicDescriptionText1.text = newRelic.item.relicDescription;
        }
        else if (slotIndex == 2)
        {
            rerollSlot2Button.interactable = false;
            WeightedObject<Relic> newRelic = Utils.GetRandomWeightedObject(regularRelics);
            regularRelics.Remove(newRelic);
            WeightedObject<Curse> newCurse = Utils.GetRandomWeightedObject(regularCurses);
            regularCurses.Remove(newCurse);

            regularRelics.Add(relic2);
            regularCurses.Add(curse2);

            relic2 = newRelic;
            curse2 = newCurse;
            relicImage2.sprite = newRelic.item.relicSprite;
            relicText2.text = newRelic.item.relicName;
            curseText2.text = newCurse.item.curseDescription;
            relicDescriptionText2.text = newRelic.item.relicDescription;
        } else if (slotIndex == 3)
        {
            rerollSlot3Button.interactable = false;
            WeightedObject<RelicCursePair> newPair = Utils.GetRandomWeightedObject(legendaryRelicCursePairs);
            legendaryRelicCursePairs.Remove(newPair);

            legendaryRelicCursePairs.Add(legendaryRelicCursePair);

            legendaryRelicCursePair = newPair;
            relicImage3.sprite = newPair.item.relic.relicSprite;
            relicText3.text = newPair.item.relic.relicName;
            curseText3.text = newPair.item.curse.curseDescription;
            relicDescriptionText3.text = newPair.item.relic.relicDescription;
        }
    }

    public void EndShop()
    {
        if (choseSlot1)
        {
            playerController.AddRelic(relic1.item);
            playerController.AddCurse(curse1.item);
        } else
        {
            regularRelics.Add(relic1);
            regularCurses.Add(curse1);
        }
        if (choseSlot2)
        {
            playerController.AddRelic(relic2.item);
            playerController.AddCurse(curse2.item);
        } else
        {
            regularRelics.Add(relic2);
            regularCurses.Add(curse2);
        }
        if (choseSlot3)
        {
            playerController.AddRelic(legendaryRelicCursePair.item.relic);
            playerController.AddCurse(legendaryRelicCursePair.item.curse);
        } else
        {
            legendaryRelicCursePairs.Add(legendaryRelicCursePair);
        }
        DisableShop();
    }
}
