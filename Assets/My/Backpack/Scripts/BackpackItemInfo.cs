using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackItemInfo
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public string ItemSprite { get; set; }
    public BackpackDataManager.BackpackTag ItemType { get; set; }
    public int CrystalPrice { get; set; }
    public int MoenyPrice { get; set; }
    public string ItemDes { get; set; }

    public int ItemNumber { get; set; }


    public BackpackItemInfo(string[] strDetail)
    {
        if (strDetail.Length <= 0)
        {
            return;
        }

        ItemID = int.Parse(strDetail[0]);
        ItemName = strDetail[1];
        ItemSprite = strDetail[2];
        ItemType = (BackpackDataManager.BackpackTag)Enum.Parse(typeof(BackpackDataManager.BackpackTag), strDetail[3]);
        CrystalPrice = int.Parse(strDetail[4]);
        MoenyPrice = int.Parse(strDetail[5]);
        ItemDes = strDetail[6];
    }

    public override string ToString()
    {
        string str = "";
        str += "ItemID:" + ItemID;
        str += "    ItemName:" + ItemName;
        str += "    ItemSprite:" + ItemSprite;
        str += "    ItemType:" + ItemType;
        str += "    CrystalPrice:" + CrystalPrice;
        str += "    MoenyPrice:" + MoenyPrice;
        str += "    ItemDes:" + ItemDes;
        str += "    ItemNumber:" + ItemNumber;
        return str;
    }
}
