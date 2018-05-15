using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSetting : MonoBehaviour
{
    private UIPopupList mList;

    private void Awake()
    {
        mList = GetComponent<UIPopupList>();
        AddLanguage();
        SetLanguage(LanguageFile.Instance.GetSaveLanguage());
        EventDelegate.Add(mList.onChange, SetLanguageEnum);
    }

    private void AddLanguage()
    {
        foreach (var item in LanguageFile.Instance.LanguageEnumDic.Keys)
        {
            mList.items.Add(item);
        }
    }

    private void SetLanguage(string language)
    {
        foreach (var item in LanguageFile.Instance.LanguageEnumDic)
        {
            if (item.Value.ToString() == language)
            {
                mList.value = item.Key;
            }
        }
    }

    private void SetLanguageEnum()
    {
        var values = LanguageFile.Instance.LanguageEnumDic[UIPopupList.current.value];
        LanguageFile.Instance.SetNowLanguage(values);
    }
}
