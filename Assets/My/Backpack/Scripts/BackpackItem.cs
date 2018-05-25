using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackItem : MonoBehaviour
{
    private UISprite itemIcon;
    private UILabel itemName;
    private UILabel itemDes;
    private UILabel itemNumberText;
    private UILabel useNumberText;
    private UILabel crystalBuyText;
    private UILabel moneyBuyText;

    private void Awake()
    {
        OnInitCompent();
    }

    private void OnInitCompent()
    {
        Transform root = transform;
        itemIcon = root.Find("ItemIcon").GetComponent<UISprite>();
        itemName = root.Find("ItemName").GetComponent<UILabel>();
        itemDes = root.Find("ItemDes").GetComponent<UILabel>();
        itemNumberText = root.Find("ItemNumberText").GetComponent<UILabel>();
        var numberButton = root.Find("ItemUseNumberButton").GetComponent<UIButton>();
        useNumberText = numberButton.transform.Find("UseNumberText").GetComponent<UILabel>();
        var useButton = root.Find("UseButton").GetComponent<UIButton>();
        var crystalUseButton = root.Find("CrystalUseButton").GetComponent<UIButton>();
        crystalBuyText = crystalUseButton.transform.Find("CrystalBuyText").GetComponent<UILabel>();
        var moneyUseButton = root.Find("MoneyUseButton").GetComponent<UIButton>();
        moneyBuyText = moneyUseButton.transform.Find("MoneyBuyText").GetComponent<UILabel>();
    }


    public void SetInfo(BackpackItemInfo info)
    {
        itemIcon.spriteName = info.ItemSprite;
    }

}
