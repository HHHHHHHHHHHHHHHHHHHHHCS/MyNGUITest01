using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagesStruct = ChatMessagesItem.ChatMessagesStruct;
using MessagesInfo = ChatMessagesItem.ChatMessagesInfo;

public class ScrollContentDisplay : MonoBehaviour
{
    #region ChatMessage
    private string chatMessage = @"Head1|1|1|吃饭|的确我爸爸就开始防盗防核扩散自行车请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head2|0|2|睡觉|的请问爸就盛大请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head3|0|3|打豆豆|的玩儿玩儿始防盗防核扩散自行车请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head1|0|4|啊呜|的确
Head2|0|5|开挂的|的区委区为企鹅撒
Head3|0|6|旗鼓相当|的自行车在去办法炉石爱死9发
Head1|0|7|你爸爸|的区委区为自行车asdaasdasdaklkwekrwgqekqwjegkqwej爱死9发
Head2|0|8|他爸爸|的不那么发
Head3|0|9|渣渣|齐尔铁塔
Head1|0|10|坤|自行车vbbnm恢复搞活
Head2|0|11|昆|离开脚后跟范德萨
Head3|0|12|鲲|阿萨德
Head1|0|13|yoooo|请问
Head2|0|14|time|自行车
Head3|0|15|我青蛙去|法阿萨德请问去国恢复空间
Head1|0|16|爱死|的as大肆宣传大请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head2|0|17|才|的9发
Head3|0|18|地方|的确
Head1|1|1|请问额|的自行车vv 部分避风港青蛙
Head2|0|20|赶回家|请问请问try让他
Head3|0|21|不那么|iuouiou丈热天
Head1|1|1|吃饭|的确我爸爸就开始防盗防核扩散自行车请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head2|0|2|睡觉|的请问爸就盛大请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head3|0|3|打豆豆|的玩儿玩儿始防盗防核扩散自行车请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head1|0|4|啊呜|的确
Head2|0|5|开挂的|的区委区为企鹅撒
Head3|0|6|旗鼓相当|的自行车在去办法炉石爱死9发
Head1|0|7|你爸爸|的区委区为自行车asdaasdasdaklkwekrwgqekqwjegkqwej爱死9发
Head2|0|8|他爸爸|的不那么发
Head3|0|9|渣渣|齐尔铁塔
Head1|0|10|坤|自行车vbbnm恢复搞活
Head2|0|11|昆|离开脚后跟范德萨
Head3|0|12|鲲|阿萨德
Head1|0|13|yoooo|请问
Head2|0|14|time|自行车
Head3|0|15|我青蛙去|法阿萨德请问去国恢复空间
Head1|0|16|爱死|的as大肆宣传大请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head2|0|17|才|的9发
Head3|0|18|地方|的确
Head1|0|19|请问额|的自行车vv 部分避风港青蛙
Head2|0|20|赶回家|请问请问try让他
Head3|0|21|不那么|iuouiou丈热天
Head1|0|10|坤|自行车vbbnm恢复搞活
Head2|1|1|吃饭|离开脚后跟范德萨
Head3|0|12|鲲|阿萨德
Head1|0|13|yoooo|请问
Head2|0|14|time|自行车
Head3|0|15|我青蛙去|法阿萨德请问去国恢复空间
Head1|0|16|爱死|的as大肆宣传大请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head2|0|17|才|的9发
Head3|0|18|地方|的确
Head1|0|19|请问额|的自行车vv 部分避风港青蛙
Head2|0|20|赶回家|请问请问try让他
Head3|0|21|不那么|iuouiou丈热天
Head1|1|1|吃饭|的确我爸爸就开始防盗防核扩散自行车请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head2|0|2|睡觉|的请问爸就盛大请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head3|0|3|打豆豆|的玩儿玩儿始防盗防核扩散自行车请问哦i许昌开机速度不vxo我去办法炉石爱死9发
Head1|0|4|啊呜|的确";
    #endregion
    [SerializeField]
    private ChatMessagesItem chatMessagesItemPrefab;

    private UIPanel scrollView;
    private const float itemBaseWidth = 80;
    private const float startPos = 370;
    private const float stepPixed = 10;
    private const float instantiateNumber = 14;
    private const float offestDistance = 200;
    private float bottomPos;

    private List<MessagesInfo> messagesInfoList;
    private List<ChatMessagesItem> showItemList;
    private Queue<ChatMessagesItem> hideItemQueue;


    private void Awake()
    {
        scrollView = GetComponent<UIPanel>();
        OnInitMessageArray();
        OnInitScrollView();
        ScrollContentBottm();
    }

    /// <summary>
    /// 初始化聊天消息
    /// </summary>
    private void OnInitMessageArray()
    {
        messagesInfoList = new List<MessagesInfo>();
        bottomPos = startPos;

        var messageInfo = chatMessage.Split('\n');
        MessagesStruct[] messageStructInfos =
            new MessagesStruct[messageInfo.Length];
        string[] messgaeDetailInfo;

        chatMessagesItemPrefab.UpdateNGUIFont();
        NGUIText.fontSize = chatMessagesItemPrefab.GetFontSize();
        NGUIText.rectHeight = 1000000;
        NGUIText.regionHeight = 1000000;
        NGUIText.Update();

        for (int i = 0; i < messageInfo.Length; i++)
        {
            messgaeDetailInfo = messageInfo[i].Split('|');
            messageStructInfos[i] = new MessagesStruct()
            {
                headSpriteName = messgaeDetailInfo[0],
                isSelf = messgaeDetailInfo[1] == "1",
                vipLevel = int.Parse(messgaeDetailInfo[2]),
                playerName = messgaeDetailInfo[3],
                chatContentMessage = messgaeDetailInfo[4],
            };
            MessagesInfo info = new MessagesInfo()
            {
                chatMessagesStruct = messageStructInfos[i],
                posY = bottomPos,
                isShow = false,
            };
            bottomPos -= (itemBaseWidth + CalculateTextHeight(messgaeDetailInfo[4]) + stepPixed);
            messagesInfoList.Add(info);
        };
    }


    /// <summary>
    /// 初始化聊天每一句话的prefab
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

        SetScrollView(scrollView);
    }

    /// <summary>
    /// 设置显示跟隐藏
    /// </summary>
    /// <param name="panel">滑动面板</param>
    private void SetScrollView(UIPanel panel )
    {
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
            else if(item.posY < clipEndY)
            {
                break;
            }
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
    public void ScrollContentBottm()
    {
        var endPosY = bottomPos + startPos;

        var pos = scrollView.transform.localPosition;
        pos.y = -endPosY;
        scrollView.transform.localPosition = pos;
        var offest = scrollView.clipOffset;
        offest.y = endPosY;
        scrollView.clipOffset = offest;
    }
}
