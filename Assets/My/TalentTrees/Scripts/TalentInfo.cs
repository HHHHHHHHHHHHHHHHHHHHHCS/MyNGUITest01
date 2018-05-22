using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentInfo
{
    public int SkillID { get; set; }
    public string SkillName { get; set; }
    public string SkillSprite { get; set; }
    public int SkillSkillLevel { get; set; }
    public int SkillMaxLevel { get; set; }
    public KeyValuePair<int, int>[] SkillCondition { get; set; }
}
