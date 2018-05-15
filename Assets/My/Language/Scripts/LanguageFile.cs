using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class LanguageFile 
{
    public static LanguageFile _instance;
    public static LanguageFile Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LanguageFile().OnInit();
            }
            return _instance;
        }
    }

    private const string fileDir = @"Assets/My/Language/Data";
    private const string nowLanguagePath = fileDir + @"/NowLanguage.txt";
    public Dictionary<string, SystemLanguage> LanguageEnumDic { get; private set; }
    public Dictionary<string, string> LanguageTextDic { get; private set; }

    private LanguageFile OnInit()
    {
        OnInitLanguageEnumDic();
        OnInitLanguageTextDic();
        return this;
    }

    private void OnInitLanguageEnumDic()
    {
        LanguageEnumDic = new Dictionary<string, SystemLanguage>
        {
            { "简体中文", SystemLanguage.ChineseSimplified },
            { "繁體中文", SystemLanguage.ChineseTraditional },
            { "English", SystemLanguage.English },
            { "欧系日语", SystemLanguage.Japanese }
        };
    }

    private void OnInitLanguageTextDic()
    {
        string nowLanguage = GetSaveLanguage();
        if (string.IsNullOrEmpty(nowLanguage))
        {
            nowLanguage = Application.systemLanguage.ToString();
            SetNowLanguage(nowLanguage);
        }
        string filepath = string.Format("{0}/{1}.txt", fileDir, nowLanguage);
        string str = File.ReadAllText(filepath);
        LanguageTextDic = JsonConvert.DeserializeObject<Dictionary<string,string>>(str);
    }

    public string GetSaveLanguage()
    {
        string nowLanguage = File.ReadAllText(nowLanguagePath);
        return nowLanguage;
    }

    public void SetNowLanguage(SystemLanguage nowLanguage)
    {
        SetNowLanguage(nowLanguage.ToString());
    }

    public void SetNowLanguage(string nowLanguage)
    {
        string readLanguage = GetSaveLanguage();
        if (readLanguage != nowLanguage)
        {
            SaveNowLanguage(nowLanguage);
        }
    }

    private void SaveNowLanguage(string nowLanguage)
    {
        var file = File.Create(nowLanguagePath);
        var data = Encoding.UTF8.GetBytes(nowLanguage);
        file.Write(data, 0, data.Length);
        file.Close();
        file.Dispose();
    }
}
