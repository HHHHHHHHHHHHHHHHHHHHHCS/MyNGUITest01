using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        return;

        for(int i =1;i<=32;i++)
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


}
