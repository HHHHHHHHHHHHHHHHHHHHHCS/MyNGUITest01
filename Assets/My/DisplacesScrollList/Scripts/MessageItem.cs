using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageItem : MonoBehaviour
{
    public class MessageInfo
    {
        public string MessageType { get; set; }
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public bool IsSelect { get; set; }
        public bool IsCollection { get; set; }
    }

    private MessageInfo currentInfo;
    private UILabel messageType, messageTitle, messageContent;
    private UIToggle selectToggle;
    private UIButton collectionButton;

    private void Awake()
    {
        messageType = transform.Find("MessageInfo/MessageType").GetComponent<UILabel>();
        messageTitle = transform.Find("MessageInfo/MessageTitle").GetComponent<UILabel>();
        messageContent = transform.Find("MessageInfo/MessageContent").GetComponent<UILabel>();
        selectToggle = transform.Find("SelectToggle").GetComponent<UIToggle>();
        collectionButton = transform.Find("CollectionButton").GetComponent<UIButton>();

        EventDelegate.Add(selectToggle.onChange, ClickSelectToggle);
        EventDelegate.Add(collectionButton.onClick, ClickCollectionButton);
    }

    public void SetInfo(MessageInfo info)
    {
        currentInfo = info;
        messageType.text = "消息类型：" + info.MessageType;
        messageTitle.text = "标题：" + info.MessageTitle;
        messageContent.text = "内容：" + info.MessageContent;
        selectToggle.value = info.IsSelect;
        SetCollectionButtonSprite(info.IsCollection);
    }

    private void ClickSelectToggle()
    {
        if(currentInfo!=null)
        {
            currentInfo.IsSelect = UIToggle.current.value;
        }
    }

    private void ClickCollectionButton()
    {
        if (currentInfo != null)
        {
            currentInfo.IsCollection = !currentInfo.IsCollection;
            SetCollectionButtonSprite(currentInfo.IsCollection);
        }
    }

    private void SetCollectionButtonSprite(bool isCollection)
    {
        collectionButton.normalSprite = isCollection ? "Button Y" : "Button X";
    }
}
