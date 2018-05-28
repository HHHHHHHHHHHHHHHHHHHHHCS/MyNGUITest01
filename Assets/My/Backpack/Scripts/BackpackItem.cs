using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackItem : MonoBehaviour
{
    private UISprite itemIcon;
    private UILabel itemName, itemDes, itemNumberText, useNumberText
    , crystalBuyText, moneyBuyText;
    private UIButton numberButton, useButton, crystalUseButton, moneyUseButton;

    private int nowInfoID;

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
        numberButton = root.Find("ItemUseNumberButton").GetComponent<UIButton>();
        useNumberText = numberButton.transform.Find("UseNumberText").GetComponent<UILabel>();
        useButton = root.Find("UseButton").GetComponent<UIButton>();
        crystalUseButton = root.Find("CrystalUseButton").GetComponent<UIButton>();
        crystalBuyText = crystalUseButton.transform.Find("NeedCrystalText").GetComponent<UILabel>();
        moneyUseButton = root.Find("MoneyUseButton").GetComponent<UIButton>();
        moneyBuyText = moneyUseButton.transform.Find("NeedMoenyText").GetComponent<UILabel>();

        EventDelegate.Add(numberButton.onClick, UseItem);
        EventDelegate.Add(useButton.onClick, UseItem);
        EventDelegate.Add(crystalUseButton.onClick, BuyCrystalItem);
        EventDelegate.Add(moneyUseButton.onClick, BuyMoneyItem);
    }


    public void SetInfo(BackpackItemInfo info
        , BackpackDataManager.BackpackColumn column)
    {
        nowInfoID = info.ItemID;


        itemIcon.spriteName = info.ItemSprite;
        itemName.text = info.ItemName;
        itemDes.text = info.ItemDes;



        bool isBackpackShow;
        RefreshItemNumber(info.ItemNumber);
        if (column == BackpackDataManager.BackpackColumn.Backpack)
        {
            isBackpackShow = true;
        }
        else
        {
            isBackpackShow = false;
            crystalBuyText.text = info.CrystalPrice.ToString();
            moneyBuyText.text = info.MoenyPrice.ToString();
        }

        itemNumberText.gameObject.SetActive(isBackpackShow);
        useNumberText.gameObject.SetActive(!isBackpackShow);
        numberButton.gameObject.SetActive(!isBackpackShow);


        useButton.gameObject.SetActive(column == BackpackDataManager.BackpackColumn.Backpack);
        crystalUseButton.gameObject.SetActive(column == BackpackDataManager.BackpackColumn.Crystal);
        moneyUseButton.gameObject.SetActive(column == BackpackDataManager.BackpackColumn.Moeny);

    }

    private void RefreshItemNumber(int result)
    {
        itemNumberText.text = "已拥有:" + result;
        useNumberText.text = "已拥有:" + result + "\n使用";
        numberButton.isEnabled = result > 0;
    }

    public void UseItem()
    {
        var result = BackpackDataManager.Instance.UseItem(nowInfoID);
        RefreshItemNumber(result);
    }

    public void BuyCrystalItem()
    {
        var result =  BackpackDataManager.Instance.BuyCrystalItem(nowInfoID);
        RefreshItemNumber(result);
    }

    public void BuyMoneyItem()
    {
        var result = BackpackDataManager.Instance.BuyMoneyItem(nowInfoID);
        RefreshItemNumber(result);
    }



}
