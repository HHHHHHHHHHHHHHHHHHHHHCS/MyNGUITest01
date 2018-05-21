using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentInfoManager : MonoBehaviour
{
    private Dictionary<int, TalentInfo> skillInfoDic;

    private void Awake()
    {
        skillInfoDic = new Dictionary<int, TalentInfo>();
    }
}
