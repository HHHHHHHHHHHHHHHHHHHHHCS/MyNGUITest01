using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentInfoManager
{
    private static TalentInfoManager _instance;
    public static TalentInfoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TalentInfoManager().OnInit();
            }
            return _instance;
        }
    }

    private Dictionary<int, TalentInfo> skillInfoDic;
    private LinkedList<TalentMono> TalentMonoList;

    private TalentInfoManager OnInit()
    {
        TalentMonoList = new LinkedList<TalentMono>();
        skillInfoDic = new Dictionary<int, TalentInfo>();
        string str = Resources.Load<TextAsset>("TalentData").text;
        foreach (var item in str.Split('\n'))
        {
            string[] infoDetail = item.Split('|');

            int skillID = int.Parse(infoDetail[0]);

            int conditionLength = (infoDetail.Length - 5) >> 1;
            KeyValuePair<int, int>[] keyValuePairs = conditionLength <= 0 ? null : new KeyValuePair<int, int>[conditionLength];
            for (int i = 0; i < conditionLength; i++)
            {
                keyValuePairs[i] = new KeyValuePair<int, int>(int.Parse(infoDetail[5 + 2 * i]), int.Parse(infoDetail[6 + 2 * i]));
            }

            TalentInfo talentInfo = new TalentInfo()
            {
                SkillID = skillID,
                SkillName = infoDetail[1],
                SkillSprite = infoDetail[2],
                SkillSkillLevel = int.Parse(infoDetail[3]),
                SkillMaxLevel = int.Parse(infoDetail[4]),
                SkillCondition = keyValuePairs,
            };
            skillInfoDic.Add(skillID, talentInfo);
        }
        return this;
    }

    public TalentInfo GetTalentInfoByID(int id)
    {
        TalentInfo outValue = null;
        skillInfoDic.TryGetValue(id, out outValue);
        return outValue;
    }

    public void AddTalentMono(TalentMono mono)
    {
        TalentMonoList.AddLast(mono);
    }

    public void RegisterLvChangeEvent(int id, Action act)
    {
        foreach (var mono in TalentMonoList)
        {
            if (mono.GetTalentID() == id)
            {
                mono.LvUpEvent += act;
                break;
            }
        }

    }
}
