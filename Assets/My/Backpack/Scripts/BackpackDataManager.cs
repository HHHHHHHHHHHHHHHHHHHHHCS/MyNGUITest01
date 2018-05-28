using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackDataManager
{
    public enum BackpackTag
    {
        Special,
        Resources,
        Accelerate,
        Battle,
        TreasureBox,
    }

    public enum BackpackColumn
    {
        Backpack,
        Crystal,
        Moeny
    }

    public event Action<int> RefreshCrystalEvent;
    public event Action<int> RefreshMoneyEvent;
    public event Action<List<BackpackItemInfo>, BackpackColumn> RefreshViewEvent;

    private static BackpackDataManager _instance;
    public static BackpackDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackpackDataManager();
                _instance.OnInit();
            }
            return _instance;
        }
    }

    public BackpackTag NowBackpackTag { get; set; }
    public BackpackColumn NowBackpackColumn { get; set; }

    private Dictionary<int, BackpackItemInfo> itemDic;

    private int nowCrystal, nowMoeny;

    private BackpackDataManager OnInit()
    {
        nowCrystal = 1000;
        nowMoeny = 100000;
        OnInitDic();
        OnInitWeb();
        ChangeColumn(BackpackColumn.Backpack);
        ChangeTag(BackpackTag.Special);

        return this;
    }

    private void OnInitDic()
    {
        itemDic = new Dictionary<int, BackpackItemInfo>();
        string readData = Resources.Load<TextAsset>("BackpackData").text;
        var stringArray = readData.Split('\n');
        foreach (var item in stringArray)
        {
            BackpackItemInfo info = new BackpackItemInfo(item.Split('|'));
            itemDic.Add(info.ItemID, info);
        }
    }

    private void OnInitWeb()
    {
        string readData = Resources.Load<TextAsset>("BackpackWebData").text;

        var stringArray = readData.Split('\n');
        foreach (var item in stringArray)
        {
            string[] strDetail = item.Split('|');
            int itemID = int.Parse(strDetail[0]), itemNumber = int.Parse(strDetail[1]);
            itemDic[itemID].ItemNumber = itemNumber;
        }
    }

    public void ChangeTag(BackpackTag newTag)
    {
        NowBackpackTag = newTag;
        RefreshView();
    }

    public void ChangeColumn(BackpackColumn newColumn)
    {
        NowBackpackColumn = newColumn;
        RefreshView();
    }

    public void RefreshView()
    {
        if (RefreshViewEvent != null)
        {
            RefreshViewEvent(GetNowViewData(), NowBackpackColumn);
        }
    }

    public void RefreshCrystalMoneyView()
    {
        if (RefreshCrystalEvent != null)
        {
            RefreshCrystalEvent(nowCrystal);
        }
        if (RefreshCrystalEvent != null)
        {
            RefreshMoneyEvent(nowMoeny);
        }
    }

    private List<BackpackItemInfo> GetNowViewData()
    {
        List<BackpackItemInfo> list = new List<BackpackItemInfo>();
        foreach (var item in itemDic.Values)
        {
            if (item.ItemType == NowBackpackTag
                && (NowBackpackColumn != BackpackColumn.Backpack
                || item.ItemNumber > 0))
            {
                list.Add(item);
            }
        }
        return list;
    }

    public int UseItem(int itemID)
    {
        itemDic[itemID].ItemNumber -= 1;
        return itemDic[itemID].ItemNumber;
    }

    public int BuyCrystalItem(int itemID)
    {
        itemDic[itemID].ItemNumber += 1;
        nowCrystal -= itemDic[itemID].CrystalPrice;
        RefreshCrystalMoneyView();
        return itemDic[itemID].ItemNumber;
    }

    public int BuyMoneyItem(int itemID)
    {
        itemDic[itemID].ItemNumber += 1;
        nowMoeny -= itemDic[itemID].MoenyPrice;
        RefreshCrystalMoneyView();
        return itemDic[itemID].ItemNumber;
    }
}
