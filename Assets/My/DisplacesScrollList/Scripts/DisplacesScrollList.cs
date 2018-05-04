using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageInfo = MessageItem.MessageInfo;

public class DisplacesScrollList : MonoBehaviour
{
    public MessageItem messageItemPrefab;

    private string[] messgageArray = new string[100];

    private MessageItem[] messageItemArray;
    private MessageInfo[] messageInfoArray;

    private void Awake()
    {
        InitInfoAndMessage();
        CreateDefaultMessageItem();
        InitEvent();
    }

    private void InitInfoAndMessage()
    {
        messageInfoArray = new MessageInfo[messgageArray.Length];
        for (int i = 0; i < messgageArray.Length; i++)
        {
            messgageArray[i] = i.ToString();
            messageInfoArray[i] = new MessageInfo()
            {
                MessageType = i.ToString(),
                MessageTitle = i.ToString(),
                MessageContent = i.ToString(),
                IsSelect = false,
                IsCollection = false,
            };
        }
    }

    private void InitEvent()
    {
        if (messgageArray.Length > 1)
        {
            var content = transform.GetComponent<UIWrapContent>();
            content.itemSize = (int)messageItemPrefab.GetComponent<UISprite>().localSize.y;
            content.onInitializeItem += DoEvent;
            content.minIndex = -messgageArray.Length + 1;
            content.maxIndex = 0;
            content.enabled = true;
        }
        else if (messgageArray.Length > 0)
        {
            messageItemArray[0].SetInfo(messageInfoArray[0]);
        }
    }

    private void CreateDefaultMessageItem()
    {
        Vector3 vec3 = new Vector3(0, -150, 0);
        int length = messgageArray.Length < 10 ? messgageArray.Length : 10;
        messageItemArray = new MessageItem[length];
        for (int i = 0; i < length; i++)
        {
            messageItemArray[i] = Instantiate(messageItemPrefab, Vector3.zero
                , Quaternion.identity, transform);
            messageItemArray[i].transform.localPosition = vec3 * i;
        }
    }

    private void DoEvent(GameObject go, int wrapIndex, int realIndex)
    {
        int index = -realIndex;
        if (index >= 0 && index < messgageArray.Length)
        {
            messageItemArray[wrapIndex].SetInfo(messageInfoArray[index]);
        }
    }
}
