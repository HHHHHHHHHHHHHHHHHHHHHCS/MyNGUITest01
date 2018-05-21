using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentInfo
{
    public int SkillID { get; set; }
    public int SkillName { get; set; }
    public int SkillSprite { get; set; }
    public int SkillConditionLevel { get; set; }
    public int SkillSkillLevel { get; set; }
    public int SkillMaxLevel { get; set; }
    public KeyValuePair<int, int>[] SkillCondition { get; set; }
}
