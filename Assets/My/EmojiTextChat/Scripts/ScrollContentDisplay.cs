using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagesStruct = ChatMessagesItem.ChatMessagesStruct;
using MessagesInfo = ChatMessagesItem.ChatMessagesInfo;

public class ScrollContentDisplay : MonoBehaviour
{

    [SerializeField]
    private ChatMessagesItem chatMessagesItemPrefab;

    private UIPanel scrollPanel;
    private UIScrollView scrollView;
    private const float itemBaseWidth = 80;
    private const float startPos = 370;
    private const float stepPixed = 10;
    private const float instantiateNumber = 14;
    private const float offestDistance = 300;
    private float bottomPos;

    private List<MessagesInfo> messagesInfoList;
    private List<ChatMessagesItem> showItemList;
    private Queue<ChatMessagesItem> hideItemQueue;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="chatMessage"></param>
    public void OnInit(string chatMessage)
    {
        scrollPanel = GetComponent<UIPanel>();
        OnInitMessageArray(chatMessage);
        OnInitScrollView();
    }

    /// <summary>
    /// 初始化聊天消息
    /// </summary>
    private void OnInitMessageArray(string chatMessage)
    {
        messagesInfoList = new List<MessagesInfo>();
        bottomPos = startPos;

        if (!string.IsNullOrEmpty(chatMessage))
        {
            var messageInfo = chatMessage.Split('\n');

            SetFont();
            for (int i = 0; i < messageInfo.Length; i++)
            {
                AddMessage(messageInfo[i], true);
            };
            ScrollContentBottm(scrollPanel);
        }
    }

    private void SetFont()
    {
        chatMessagesItemPrefab.UpdateNGUIFont();
        NGUIText.fontSize = chatMessagesItemPrefab.GetFontSize();
        NGUIText.rectHeight = 1000000;
        NGUIText.regionHeight = 1000000;
        NGUIText.Update();
    }

    /// <summary>
    /// 有新消息的添加
    /// </summary>
    /// <param name="messgaeDetailInfo"></param>
    /// <param name="isRange"></param>
    public void AddMessage(string messageInfo, bool isRange = false)
    {
        if (!isRange)
        {
            SetFont();
        }
        string[] messgaeDetailInfo = messageInfo.Split('|');
        var messageStructInfo = new MessagesStruct()
        {
            headSpriteName = messgaeDetailInfo[0],
            isSelf = messgaeDetailInfo[1] == "1",
            vipLevel = int.Parse(messgaeDetailInfo[2]),
            playerName = messgaeDetailInfo[3],
            chatContentMessage = messgaeDetailInfo[4],
        };
        MessagesInfo info = new MessagesInfo()
        {
            chatMessagesStruct = messageStructInfo,
            posY = bottomPos,
            isShow = false,
        };
        var width = (itemBaseWidth + CalculateTextHeight(messgaeDetailInfo[4]) + stepPixed);
        bottomPos -= width;
        messagesInfoList.Add(info);

        if (!isRange)
        {
            ScrollContentBottm(scrollPanel);
        }
    }

    /// <summary>
    /// 初始化聊天说话的prefab
    /// </summary>
    private void OnInitScrollView()
    {
        showItemList = new List<ChatMessagesItem>();
        hideItemQueue = new Queue<ChatMessagesItem>();
        GetComponent<UIPanel>().onClipMove = SetScrollView;

        for (int i = 0; i < instantiateNumber; i++)
        {
            var messagesItem = Instantiate(chatMessagesItemPrefab, transform);
            messagesItem.OnCreate();
            hideItemQueue.Enqueue(messagesItem);
        }

        SetScrollView(scrollPanel);
    }

    /// <summary>
    /// 设置显示跟隐藏
    /// </summary>
    /// <param name="panel">滑动面板</param>
    private void SetScrollView(UIPanel panel)
    {
        ClampPanel(panel);

        //计算出 最高和最低的边界
        float clipStartY = panel.clipOffset.y + startPos + offestDistance;
        float clipEndY = panel.clipOffset.y - panel.width
            + startPos - offestDistance;

        //先隐藏显示的
        for (int i = showItemList.Count - 1; i >= 0; i--)
        {
            var item = showItemList[i];
            if (item.CurrentInfo.posY > clipStartY
                || item.CurrentInfo.posY < clipEndY)
            {
                item.OnHide();
                hideItemQueue.Enqueue(item);
                showItemList.Remove(item);
            }
        }

        //在显示要显示的
        for (int i = 0; i < messagesInfoList.Count; i++)
        {
            var item = messagesInfoList[i];
            if (item.posY <= clipStartY
                && item.posY >= clipEndY)
            {
                if (!item.isShow)
                {
                    var hideItem = hideItemQueue.Dequeue();
                    hideItem.OnInit(item);
                    showItemList.Add(hideItem);
                }
            }
            else if (item.posY < clipEndY)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 限制面板上限和下限
    /// </summary>
    /// <param name="panel"></param>
    private void ClampPanel(UIPanel panel)
    {
        var startPosY = offestDistance;
        var endPosY = bottomPos + startPos - offestDistance;
        var nowPos = panel.clipOffset.y;

        var clampPos = Mathf.Clamp(nowPos, endPosY, startPosY);
        if (clampPos != nowPos && startPosY - endPosY > panel.width)
        {
            SetScrollContent(panel, clampPos);
        }
    }

    /// <summary>
    /// 计算文字宽度为了实现不同prefab 的制作
    /// </summary>
    /// <param name="mText">文字</param>
    /// <returns>文字宽度</returns>
    private int CalculateTextHeight(string mText)
    {
        float regionY = 1;

        string mProcessedText;
        bool fits = NGUIText.WrapText(mText, out mProcessedText, true, false);
        Vector2 mCalculatedSize = NGUIText.CalculatePrintedSize(mProcessedText);
        int mHeight = Mathf.Max(0, Mathf.RoundToInt(mCalculatedSize.y));
        if (regionY != 1f) mHeight = Mathf.RoundToInt(mHeight / regionY);
        if ((mHeight & 1) == 1) ++mHeight;

        return mHeight;
    }

    /// <summary>
    /// 滑动到底部
    /// </summary>
    public void ScrollContentBottm(UIPanel panel)
    {
        var endPosY = bottomPos + startPos;

        SetScrollContent(panel, endPosY);
    }

    /// <summary>
    /// 设置滑动的位置
    /// </summary>
    public void SetScrollContent(UIPanel panel, float y)
    {
       if (!scrollView)
       {
           scrollView = GetComponent<UIScrollView>();
       }
       if (scrollView)
       {
            scrollView.StopScroll();
       }

        var pos = panel.transform.localPosition;
        pos.y = -y;
        panel.transform.localPosition = pos;
        var offest = panel.clipOffset;
        offest.y = y;
        panel.clipOffset = offest;
    }
}
