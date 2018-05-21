using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentCondition : MonoBehaviour
{
    private UISprite[] spriteArray;

    private readonly Color falseCondition = Color.gray;
    private readonly Color trueCondition = Color.cyan;


    private void Awake()
    {
        spriteArray = GetComponentsInChildren<UISprite>();
    }

    private void SetCondition(bool contidion)
    {
        foreach (var item in spriteArray)
        {
            item.color = contidion ? trueCondition : falseCondition;
        }
    }
}
