using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentMono : MonoBehaviour
{
    public event Action LvUpEvent;

    [SerializeField]
    private int talentID;

    private TalentInfo talentInfo;
    private TalentCondition[] talentConditionArray;

    private UISprite icon;
    private UIButton iconButton;
    private UISprite maskIcon;
    private UILabel nameText;
    private UISlider lvSlider;
    private UILabel lvText;

    private bool isUnlock;

    public int GetTalentID()
    {
        return talentID;
    }
#if UNITY_EDITOR
    public void SetTalentID(int i)
    {
        talentID = i;
    }
#endif

    private void Awake()
    {
        Transform root = transform;
        talentConditionArray = root.Find("Conditions").GetComponentsInChildren<TalentCondition>();
        icon = root.Find("TalentIcon").GetComponent<UISprite>();
        iconButton = icon.GetComponent<UIButton>();
        maskIcon = root.Find("TalentIcon/TalentMask").GetComponent<UISprite>();
        nameText = root.Find("TalentNameBg/TalentNameText").GetComponent<UILabel>();
        lvSlider = root.Find("TalentLevelBg").GetComponent<UISlider>();
        lvText = root.Find("TalentLevelBg/TaletnLevelText").GetComponent<UILabel>();

        EventDelegate.Add(iconButton.onClick, ClickIconEvent);
        TalentInfoManager.Instance.AddTalentMono(this);
    }

    private void Start()
    {
        talentInfo = TalentInfoManager.Instance.GetTalentInfoByID(talentID);
        if (talentInfo != null)
        {
            icon.spriteName = talentInfo.SkillSprite;
            iconButton.normalSprite = talentInfo.SkillSprite;
            nameText.text = talentInfo.SkillName;
            SetLV();
            CheckCondition();

            if(talentInfo.SkillCondition!=null)
            {
                foreach (var item in talentInfo.SkillCondition)
                {
                    TalentInfoManager.Instance.RegisterLvChangeEvent(item.Key, CheckCondition);
                }
            }

        }
    }

    private void SetLV()
    {
        lvSlider.value = (float)talentInfo.SkillSkillLevel / talentInfo.SkillMaxLevel;
        lvText.text = talentInfo.SkillSkillLevel + "/" + talentInfo.SkillMaxLevel;
    }

    public void CheckCondition()
    {
        if (talentInfo != null)
        {
            var conditions = talentInfo.SkillCondition;
            if (conditions != null)
            {
                int trueCondition = 0;
                for (int i = 0; i < conditions.Length; i++)
                {
                    var condition = TalentInfoManager.Instance.GetTalentInfoByID(conditions[i].Key);
                    if (condition != null && condition.SkillSkillLevel >= conditions[i].Value
                        && i < talentConditionArray.Length)
                    {
                        talentConditionArray[i].SetCondition();
                        trueCondition++;
                    }
                }

                if (trueCondition >= conditions.Length)
                {
                    isUnlock = true;
                    maskIcon.gameObject.SetActive(false);
                    if (talentConditionArray.Length > conditions.Length)
                    {
                        talentConditionArray[talentConditionArray.Length - 1].SetCondition();
                    }
                }
            }
            else
            {
                isUnlock = true;
                maskIcon.gameObject.SetActive(false);
            }
        }
    }

    private void ClickIconEvent()
    {
        if (isUnlock )
        {
            talentInfo.SkillSkillLevel = Mathf.Clamp(talentInfo.SkillSkillLevel + 1, 0, talentInfo.SkillMaxLevel);
            SetLV();
            if (LvUpEvent != null)
            {
                LvUpEvent();
            }
        }

    }

}
