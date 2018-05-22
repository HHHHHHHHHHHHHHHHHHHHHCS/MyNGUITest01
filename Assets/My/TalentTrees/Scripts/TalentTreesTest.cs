using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

public class TalentTreesTest : MonoBehaviour
{
    private void Awake()
    {
        return;

        for (int i = 1; i <= 32; i++)
        {
            string str = string.Format(@"F:\MyUnity\MyNGUITest01\Assets\My\TalentTrees\Textures\Ability_{0}.png", i.ToString("D2"));
            string newStr = string.Format(@"F:\MyUnity\MyNGUITest01\Assets\My\TalentTrees\Textures\TalentIcon_{0}.png", i.ToString("D2"));
            File.Move(str, newStr);
        }


        using (var fileStream = File.Open(@"F:\MyUnity\MyNGUITest01\Assets\My\TalentTrees\Resources\TalentData.txt"
            , FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fileStream))
            {
                for (int i = 1; i <= 32; i++)
                {
                    sw.WriteLine(string.Format("{0}|技能{1}|TalentIcon_{2}|0|5|{3}|1", i, i.ToString("D2"), i.ToString("D2"), (i - 1).ToString()));
                }
            }
        }
    }

    [MenuItem("Editor/SetItemName")]
    private static void SetItemName()
    {
        var activeTransform = Selection.activeTransform;
        if (activeTransform.name != "TalentScrollView")
            return;

        string nameBase = "TalentItem";
        Vector2 startPos = new Vector2(-600, 235);
        Vector2 offestStep = new Vector2(400, 360);

        List<Transform> talentItemList = new List<Transform>();

        //GameObject mask = activeTransform.parent.Find("TalentMask").gameObject;

        foreach (Transform item in activeTransform)
        {
            talentItemList.Add(item);
        }

        talentItemList.Sort((p1, p2) =>
        {
            if (p1.position.y > p2.position.y)
            {
                return -1;
            }
            else if (p1.position.x < p2.position.x)
            {
                return -1;
            }
            return 1;
        });

        int xIndex = 1, yIndex = 1;
        for (int i = 0; i < talentItemList.Count; i++)
        {
            if (i > 0 && talentItemList[i - 1].position.y != talentItemList[i].position.y)
            {
                xIndex = 1;
                yIndex++;
            }
            talentItemList[i].name = string.Format("{0}_{1}_{2}", nameBase, yIndex, xIndex++);
            talentItemList[i].SetSiblingIndex(i);
            talentItemList[i].GetComponent<TalentMono>().SetTalentID(i+1);


            //NGUITools.AddChild(talentItemList[i].Find("TalentIcon").gameObject, mask).name= "TalentMask";
        }
    }
}
