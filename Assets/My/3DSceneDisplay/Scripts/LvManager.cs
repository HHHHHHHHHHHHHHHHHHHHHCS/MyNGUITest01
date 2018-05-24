using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvManager : MonoBehaviour
{
    public static LvManager Instance { get; private set; }

    [SerializeField]
    private Sprite[] spriteArray;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite[] GetSprites(int i)
    {
        Sprite[] newSpriteArray;
        int unit = i % 10, ten = (i / 10) % 10;
        newSpriteArray = new Sprite[ten==0?1:2];
        newSpriteArray[0] = spriteArray[unit];
        if (ten!=0)
        {
            newSpriteArray[1] = spriteArray[ten];
        }
        return newSpriteArray;
    }
}
