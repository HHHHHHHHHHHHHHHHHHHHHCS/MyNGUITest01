using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageItem : MonoBehaviour
{
    public void SetInfo(string i)
    {
        transform.Find("MessageInfo/MessageType").GetComponent<UILabel>().text = "消息类型：" + i;
        transform.Find("MessageInfo/MessageTitle").GetComponent<UILabel>().text = "标题：+" + i;
        transform.Find("MessageInfo/MessageContent").GetComponent<UILabel>().text = "内容：" + i;
    }
}
