using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEditor;

public static class AutoUICodeEditor
{
    [MenuItem("Editor/AutoUICode")]
    public static void AutoUICode()
    {
        CreateCSharp();
    }

    private static void CreateCSharp()
    {
        if(Selection.activeGameObject.name.IndexOf("Panel")<0)
        {
            return;
        }
        string filePath = AssetDatabase.GetAssetOrScenePath(Selection.activeObject)
            + Selection.activeGameObject.name + ".cs";
        using (FileStream fs = File.Create(filePath))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                WriteHead(sw);
                WriteValue(sw);
                sw.Flush();
            }
        }
        AssetDatabase.Refresh();
    }

    private static void WriteHead(StreamWriter sw)
    {
        string selectName = Selection.activeGameObject.name;
        string head = @"using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.UI;

        public class "+ selectName + " : MonoBehaviour{";

        sw.WriteLine(head);
    }

    private static void WriteValue(StreamWriter sw)
    {
        StringBuilder sb = new StringBuilder();
        GameObject root = Selection.activeGameObject;

        var labels = _WriteProperty<UILabel>(sb, root, "Text");
        var buttons = _WriteProperty<UIButton>(sb, root, "Button");
        var sliders = _WriteProperty<UISlider>(sb, root, "Slider");
        sw.WriteLine(sb.ToString());

        sw.WriteLine(@"private void Awake (){");
        sw.WriteLine(@"Transform root = transform;");

        sb.Length = 0;
        _WriteAwake(sb, labels);
        _WriteAwake(sb, buttons);
        _WriteAwake(sb, sliders);
        sw.WriteLine(sb.ToString());

        sw.WriteLine(@"}}");
    }

    private static List<T> _WriteProperty<T>(StringBuilder _sb, GameObject _root
        , string _indexOf) where T : MonoBehaviour
    {
        T[] array = _root.GetComponentsInChildren<T>();
        List<T> list = new List<T>();
        foreach (var item in array)
        {
            if (item.name.IndexOf(_indexOf) >= 0)
            {
                _sb.AppendFormat("private {0} {1};", typeof(T).ToString()
                    , GetName(item.name)).AppendLine();
                list.Add(item);
            }
        }
        return list;
    }

    private static void _WriteAwake<T>(StringBuilder _sb, List<T> _list)
        where T : MonoBehaviour
    {
        string formatStr = "{0} {1} = root.Find(\"{2}\").GetComponent<{0}>();";
        foreach (var item in _list)
        {
            _sb.AppendFormat(formatStr, typeof(T), GetName(item.name), FindParent(item.transform))
                .AppendLine();
        }
    }

    private static string GetName(string _name)
    {
        string itemName = _name;
        char ch = itemName[0];
        itemName = itemName.Remove(0,1);
        itemName = itemName.Insert(0, char.ToLower(ch).ToString());
        return itemName;
    }


    private static string FindParent(Transform _item)
    {
        string path = null;
        for (int i = 0; i < 1000; i++)//这里不用while 防止死循环
        {
            if (_item != Selection.activeGameObject.transform)
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = _item.name;
                }
                else
                {
                    path = _item.name + "/" + path;
                }
                _item = _item.parent;
            }
            else
            {
                break;
            }
        }
        return path;
    }
}
