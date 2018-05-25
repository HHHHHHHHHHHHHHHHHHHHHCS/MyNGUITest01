using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BackpackTest : MonoBehaviour
{
    [MenuItem("Editor/SetBackpackText")]
    private static void SetText()
    {
        //{2}图片要补全数字  {3}要*100   {4}要*1000
        string[] strData = new string[]
        {
            @"{0}|特殊{1}号|INV_Misc_Food_{2}|Special|{3}|{4}|我是特殊{5}号",
            @"{0}|资源{1}号|INV_Misc_Fish_{2}|Resources|{3}|{4}|我是资源{5}号",
            @"{0}|加速{1}号|INV_Potion_{2}|Accelerate|{3}|{4}|我是加速{5}号",
            @"{0}|战斗{1}号|INV_Sword_{2}|Battle|{3}|{4}|我是战斗{5}号",
            @"{0}|宝箱{1}号|INV_Potion_{2}|TreasureBox|{3}|{4}|我是宝箱{5}号",
        };

        string str = Application.dataPath + @"/My/Backpack/Resources/BackpackData.txt";
        var reader = File.CreateText(str);

        int index=0;
        string nowStr ,formatStr;
        for (int i = 1; i <= 5; i++)
        {
            nowStr = strData[i - 1];
            for (int j = 1; j <= 16; j++)
            {
                index++;
                formatStr = string.Format(nowStr, index, j, j.ToString("D2"), j * 100, j * 1000, j);
                reader.WriteLine(formatStr);
            }
        }

        reader.Close();
        reader.Dispose();
    }
}
