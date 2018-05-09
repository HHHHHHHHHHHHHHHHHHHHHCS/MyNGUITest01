using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessageCtrl : MonoBehaviour
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

    private ScrollContentDisplay display;

    private void Awake()
    {
        display=GetComponent<ScrollContentDisplay>();
        display.OnInit(null);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            string s= "";
            for(int i=0;i<Random.Range(1,5);i++)
            {
                s+=Random.Range(0, 100000000).ToString()+"吃饭";
            }
            display.AddMessage(
                string.Format("{0}|{1}|{2}|{3}|{4}", "Head"+Random.Range(1,4).ToString(), Random.Range(0, 2), Random.Range(1, 99), "我是" + Random.Range(0, 30), s));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            display.AddMessage(
                string.Format("{0}|{1}|{2}|{3}|{4}|[#{5}]", "Head" + Random.Range(1, 4).ToString(), Random.Range(0, 2), Random.Range(1, 99)
                , "我是" + Random.Range(0, 30), null,Random.Range(1,6)));
        }
    }
}
