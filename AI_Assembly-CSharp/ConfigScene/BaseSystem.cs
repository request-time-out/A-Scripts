// Decompiled with JetBrains decompiler
// Type: ConfigScene.BaseSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Elements.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ConfigScene
{
  public abstract class BaseSystem : Data
  {
    public BaseSystem(string elementName)
      : base(elementName)
    {
    }

    public FieldInfo[] FieldInfos
    {
      get
      {
        return this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
      }
    }

    public override void Read(string rootName, XmlDocument xml)
    {
      string str = rootName + "/" + this.elementName + "/";
      foreach (FieldInfo fieldInfo in this.FieldInfos)
      {
        XmlNodeList xmlNodeList = xml.SelectNodes(str + fieldInfo.Name);
        if (xmlNodeList != null && xmlNodeList.Item(0) is XmlElement xmlElement)
          fieldInfo.SetValue((object) this, BaseSystem.Cast(xmlElement.InnerText, fieldInfo.FieldType));
      }
    }

    public override void Write(XmlWriter writer)
    {
      writer.WriteStartElement(this.elementName);
      foreach (FieldInfo fieldInfo in this.FieldInfos)
      {
        writer.WriteStartElement(fieldInfo.Name);
        writer.WriteValue(BaseSystem.ConvertString(fieldInfo.GetValue((object) this)));
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    public static object Cast(string str, System.Type type)
    {
      if (type == typeof (Color))
      {
        string[] strArray1 = str.Split(',');
        if (strArray1.Length != 4)
          return (object) Color.get_white();
        int num1 = 0;
        string[] strArray2 = strArray1;
        int index1 = num1;
        int num2 = index1 + 1;
        double num3 = (double) float.Parse(strArray2[index1]);
        string[] strArray3 = strArray1;
        int index2 = num2;
        int num4 = index2 + 1;
        double num5 = (double) float.Parse(strArray3[index2]);
        string[] strArray4 = strArray1;
        int index3 = num4;
        int num6 = index3 + 1;
        double num7 = (double) float.Parse(strArray4[index3]);
        string[] strArray5 = strArray1;
        int index4 = num6;
        int num8 = index4 + 1;
        double num9 = (double) float.Parse(strArray5[index4]);
        return (object) new Color((float) num3, (float) num5, (float) num7, (float) num9);
      }
      if (!type.IsArray)
        return Convert.ChangeType((object) str, type);
      string[] strArray = str.Split(',');
      System.Type elementType = type.GetElementType();
      Array instance = Array.CreateInstance(elementType, strArray.Length);
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType5<string, int> anonType5 in ((IEnumerable<string>) strArray).Select<string, \u003C\u003E__AnonType5<string, int>>((Func<string, int, \u003C\u003E__AnonType5<string, int>>) ((v, i) => new \u003C\u003E__AnonType5<string, int>(v, i))))
        instance.SetValue(Convert.ChangeType((object) anonType5.v, elementType), anonType5.i);
      return (object) instance;
    }

    public static string ConvertString(object o)
    {
      if (o is Color)
      {
        Color color = (Color) o;
        return string.Format("{0},{1},{2},{3}", (object) (float) color.r, (object) (float) color.g, (object) (float) color.b, (object) (float) color.a);
      }
      if (!o.GetType().IsArray)
        return o.ToString();
      Array array = (Array) o;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < array.Length; ++index)
      {
        stringBuilder.Append(array.GetValue(index));
        if (index + 1 < array.Length)
          stringBuilder.Append(",");
      }
      return stringBuilder.ToString();
    }
  }
}
