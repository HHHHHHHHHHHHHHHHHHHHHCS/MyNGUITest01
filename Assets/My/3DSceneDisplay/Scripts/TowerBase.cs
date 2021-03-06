﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTowerInfo = BuildTowerUI.BuildTowerInfo;

public class TowerBase : MonoBehaviour, IClickEvent
{
    #region const string
    private const string fogPath = @"Prefabs/BuildFog";
    private const string towerDir = @"Prefabs/Tower/";

    private const string towerInfoData = @"0|1|Tower_Empty
1|1|Tower_Flamethrower_LV1
1|2|Tower_Flamethrower_LV2
1|3|Tower_Flamethrower_LV3
2|1|Tower_Cannon_LV1
2|2|Tower_Cannon_LV2
2|3|Tower_Cannon_LV3
3|1|Tower_Satellite_LV1
3|2|Tower_Satellite_LV2
3|3|Tower_Satellite_LV3
4|1|Tower_Ice_LV1
4|2|Tower_Ice_LV2
4|3|Tower_Ice_LV3
5|1|Tower_Laser_LV1
5|2|Tower_Laser_LV2
5|3|Tower_Laser_LV3";
    #endregion
    public struct TowerInfo
    {
        public int id;
        public int lv;
        public string modelName;

        public TowerInfo(string _id, string _lv, string _modelName)
        {
            id = int.Parse(_id);
            lv = int.Parse(_lv);
            modelName = _modelName;
        }

        public TowerInfo(int _id, int _lv, string _modelName)
        {
            id = _id;
            lv = _lv;
            modelName = _modelName;
        }

        public static string MakeKey(int _id, int _lv)
        {
            return _id.ToString() + '|' + _lv.ToString();
        }

        public static string MakeKey(string _id, string _lv)
        {
            return _id + '|' + _lv;
        }
    }
    public static Dictionary<string, TowerInfo> towerInfoDic;
    public static ParticleSystem particlePrefab;
    public static TowerBase current;
    

    [SerializeField]
    private int towerID;
    private int cloudID;

    private BoxCollider boxCol;
    private Transform modelTS;
    private TowerInfo towerinfo;
    private Animation buildUp;
    private GameObject lv_Parent;
    private SpriteRenderer lvSpriteUnit, lvSpriteTen;
    private float lvUnitPosX,lvTenPosX,lvMiddlePosX;

    private void Awake()
    {
        OnInitBase();
        OnInitCompent();
        //CreateDeaultTower();
    }

    private void OnInitBase()
    {
        if (towerInfoDic == null)
        {
            towerInfoDic = new Dictionary<string, TowerInfo>();
            string[] towerInfoArray = towerInfoData.Split('\n');
            foreach (var item in towerInfoArray)
            {
                string[] str = item.Split('|');
                TowerInfo info = new TowerInfo
                    (str[0], str[1], str[2].Trim());
                towerInfoDic.Add(TowerInfo.MakeKey(str[0], str[1]), info);
            }
        }

        if(!particlePrefab)
        {
            particlePrefab = Resources.Load<ParticleSystem>(fogPath);
        }

        lv_Parent = transform.Find("Lv_Parent").gameObject;
        lvSpriteUnit = lv_Parent.transform.Find("Lv_Unit").GetComponent<SpriteRenderer>();
        lvSpriteTen = lv_Parent.transform.Find("Lv_Ten").GetComponent<SpriteRenderer>();
        lvUnitPosX = lvSpriteUnit.transform.position.x;
        lvTenPosX = lvSpriteTen.transform.position.x;
        lvMiddlePosX = (lvUnitPosX + lvTenPosX) / 2;
        SetLV(0);
    }

    private TowerInfo? GetTowerInfoByName(string key)
    {
        TowerInfo info;
        towerInfoDic.TryGetValue(key, out info);
        return info;
    }

    public virtual void CreateDeaultTower()
    {
        if (modelTS.childCount <= 0)
        {
            BuildTower(GetTowerInfoByName(TowerInfo.MakeKey(0, 1)).Value);
        }
    }

    protected virtual void OnInitCompent()
    {
        Transform root = transform;
        root.name = root.name + "_" + towerID;
        boxCol = gameObject.GetComponent<BoxCollider>();
        boxCol.enabled = false;
        modelTS = transform.Find("Model");
        buildUp = modelTS.GetComponent<Animation>();
    }

    protected virtual void OnInitTower()
    {
        if (modelTS.childCount > 0)
        {
            var box = modelTS.GetChild(modelTS.childCount-1).GetComponent<BoxCollider>();
            boxCol.center = box.center;
            boxCol.size = box.size;
            boxCol.enabled = true;
            box.enabled = false;
        }
    }

    public virtual void OnClick()
    {
        current = this;
        switch (towerinfo.id)
        {
            case 0:
                UIManager_3DScene.Instance.TowerBuildPanel.OnShow();
                break;
            case 1:
                UIManager_3DScene.Instance.FlamethrowerPanel.OnShow(towerinfo);
                break;
            case 2:
                UIManager_3DScene.Instance.FlamethrowerPanel.OnShow(towerinfo);
                break;
            case 3:
                UIManager_3DScene.Instance.FlamethrowerPanel.OnShow(towerinfo);
                break;
            case 4:
                UIManager_3DScene.Instance.FlamethrowerPanel.OnShow(towerinfo);
                break;
            case 5:
                UIManager_3DScene.Instance.FlamethrowerPanel.OnShow(towerinfo);
                break;
        }
    }

    public virtual void UIEventCallBack(BuildTowerInfo _buildTowerInfo)
    {
        var _towerInfo = GetTowerInfoByName(TowerInfo.MakeKey(_buildTowerInfo.id, 1));
        if (_towerInfo.HasValue)
        {
            UIEventCallBack(_towerInfo.Value);
        }
    }

    public virtual void UIEventCallBack(TowerInfo _towerInfo,bool needUP =false)
    {
            var info = GetTowerInfoByName(TowerInfo.MakeKey(_towerInfo.id, Mathf.Clamp(_towerInfo.lv + (needUP?1:0), 1, 3)));
            BuildTower(info.Value);
    }

    private void BuildTower(TowerInfo _towerInfo)
    {
        if (modelTS.childCount > 0)
        {
            Destroy(modelTS.GetChild(0).gameObject);
            var particle = Instantiate(particlePrefab, transform);
            particle.transform.localPosition = Vector3.zero;
            particle.Play();
            buildUp.Play();
        }
        string towername = _towerInfo.modelName;
        GameObject go = Resources.Load<GameObject>(towerDir + towername);
        if(go!=null)
        {
            towerinfo = _towerInfo;
            Instantiate(go, modelTS);
            OnInitTower();
        }

        SetLV(_towerInfo.id == 0 ? 0:_towerInfo.lv);
    }

    private void SetLV(int i)
    {
        if(i==0)
        {
            lv_Parent.SetActive(false);
        }
        else
        {
            lv_Parent.SetActive(true);

            var spr = LvManager.Instance.GetSprites(i);


            lvSpriteUnit.sprite = spr[0];
            lvSpriteUnit.gameObject.SetActive(true);
            var pos = lvSpriteUnit.transform.position;
            if (spr.Length == 1)
            {
                pos.x = lvMiddlePosX;
                lvSpriteUnit.transform.position = pos;
                lvSpriteTen.gameObject.SetActive(false);
            }
            else
            {
                pos.x = lvUnitPosX;
                lvSpriteTen.sprite = spr[1];
                lvSpriteTen.gameObject.SetActive(true);
            }
            lvSpriteUnit.transform.position = pos;
        }
 
    }
}
