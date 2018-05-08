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
    private float startPos = 370;
    private float stepPixed = 10;
    private float bottomPos;

    private List<MessagesInfo> messagesInfoList;


    private void Awake()
    {
        scrollView = GetComponent<UIPanel>();
        var messagesStruct = OnInitMessageArray();
        OnInitScrollView(messagesStruct);
    }

    private MessagesStruct[] OnInitMessageArray()
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
            };
            bottomPos -= (80 + ProcessText(messgaeDetailInfo[4]) + stepPixed);
            messagesInfoList.Add(info);
        };

        return messageStructInfos;
    }

    private void OnInitScrollView(MessagesStruct[] messagesStruct)
    {
        float lastPosY = startPos;
        for (int i = 0; i < messagesStruct.Length; i++)
        {
            var messagesItem = Instantiate(chatMessagesItemPrefab, transform);
            var height = messagesItem.OnInit(messagesStruct[i]);
            messagesItem.transform.localPosition =
                new Vector3(0, lastPosY, 0);

            lastPosY -= height + stepPixed;
        }
        var endPosY = lastPosY + startPos;
        var pos = scrollView.transform.localPosition;
        pos.y = -endPosY;
        scrollView.transform.localPosition= pos;
        var offest = scrollView.clipOffset;
        offest.y = endPosY;
        scrollView.clipOffset= offest;
    }

    private int ProcessText(string mText)
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


}
