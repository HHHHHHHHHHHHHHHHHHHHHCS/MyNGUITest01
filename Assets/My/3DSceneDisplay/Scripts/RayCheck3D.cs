using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck3D : MonoBehaviour
{
    private const string towerEmpty = "TowerEmpty";
    private const string tower = "Tower";
    private int towerMask;

    private void Awake()
    {
        towerMask = LayerMask.GetMask("Tower");
    }



    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo,1000f, towerMask))
            {
                GameObject gameObj = hitInfo.collider.gameObject;
                //当射线碰撞目标为boot类型的物品，执行拾取操作
                if (gameObj.tag == towerEmpty)
                {
                    Debug.Log("is null");
                }
                else if (gameObj.tag == tower)
                {
                    Debug.Log("have tower");
                }
            }
        }
    }

}