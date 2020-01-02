// Decompiled with JetBrains decompiler
// Type: ConfigScene.GraphicSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Xml;
using UnityEngine;

namespace ConfigScene
{
  public class GraphicSystem : BaseSystem
  {
    public bool SelfShadow = true;
    public bool Bloom = true;
    public bool SSAO = true;
    public bool SSR = true;
    public bool DepthOfField = true;
    public bool Atmospheric = true;
    public bool Vignette = true;
    public bool Rain = true;
    public bool AmbientLight = true;
    public bool Shield = true;
    public Color SilhouetteColor = Color.get_blue();
    public Color BackColor = Color.get_black();
    public int MaxCharaNum = 4;
    public bool[] CharasEntry = new bool[4];
    public const int MAX_CHARA_NUM = 4;
    public const byte CHARA_GRAPHIC_QUALITY = 0;
    public const byte MAP_GRAPHIC_QUALITY = 0;
    public byte CharaGraphicQuality;
    public byte MapGraphicQuality;
    public bool FaceLight;
    public bool SimpleBody;

    public GraphicSystem(string elementName)
      : base(elementName)
    {
    }

    public override void Init()
    {
      this.SelfShadow = true;
      this.Bloom = true;
      this.SSAO = true;
      this.SSR = true;
      this.DepthOfField = true;
      this.Atmospheric = true;
      this.Vignette = true;
      this.Rain = true;
      this.CharaGraphicQuality = (byte) 0;
      this.MapGraphicQuality = (byte) 0;
      this.FaceLight = false;
      this.AmbientLight = true;
      this.Shield = true;
      this.SimpleBody = false;
      this.SilhouetteColor = Color.get_blue();
      this.BackColor = Color.get_black();
      this.MaxCharaNum = 4;
      for (int index = 0; index < 4; ++index)
        this.CharasEntry[index] = true;
    }

    public override void Read(string rootName, XmlDocument xml)
    {
      try
      {
        string str = rootName + "/" + this.elementName + "/";
        XmlNodeList xmlNodeList1 = xml.SelectNodes(str + "SelfShadow");
        if (xmlNodeList1 != null && xmlNodeList1.Item(0) is XmlElement xmlElement)
          this.SelfShadow = (bool) BaseSystem.Cast(xmlElement.InnerText, this.SelfShadow.GetType());
        XmlNodeList xmlNodeList2 = xml.SelectNodes(str + "Bloom");
        if (xmlNodeList2 != null && xmlNodeList2.Item(0) is XmlElement xmlElement)
          this.Bloom = (bool) BaseSystem.Cast(xmlElement.InnerText, this.Bloom.GetType());
        XmlNodeList xmlNodeList3 = xml.SelectNodes(str + "SSAO");
        if (xmlNodeList3 != null && xmlNodeList3.Item(0) is XmlElement xmlElement)
          this.SSAO = (bool) BaseSystem.Cast(xmlElement.InnerText, this.SSAO.GetType());
        XmlNodeList xmlNodeList4 = xml.SelectNodes(str + "SSR");
        if (xmlNodeList4 != null && xmlNodeList4.Item(0) is XmlElement xmlElement)
          this.SSR = (bool) BaseSystem.Cast(xmlElement.InnerText, this.SSR.GetType());
        XmlNodeList xmlNodeList5 = xml.SelectNodes(str + "DepthOfField");
        if (xmlNodeList5 != null && xmlNodeList5.Item(0) is XmlElement xmlElement)
          this.DepthOfField = (bool) BaseSystem.Cast(xmlElement.InnerText, this.DepthOfField.GetType());
        XmlNodeList xmlNodeList6 = xml.SelectNodes(str + "Atmospheric");
        if (xmlNodeList6 != null && xmlNodeList6.Item(0) is XmlElement xmlElement)
          this.Atmospheric = (bool) BaseSystem.Cast(xmlElement.InnerText, this.Atmospheric.GetType());
        XmlNodeList xmlNodeList7 = xml.SelectNodes(str + "Vignette");
        if (xmlNodeList7 != null && xmlNodeList7.Item(0) is XmlElement xmlElement)
          this.Vignette = (bool) BaseSystem.Cast(xmlElement.InnerText, this.Vignette.GetType());
        XmlNodeList xmlNodeList8 = xml.SelectNodes(str + "Rain");
        if (xmlNodeList8 != null && xmlNodeList8.Item(0) is XmlElement xmlElement)
          this.Rain = (bool) BaseSystem.Cast(xmlElement.InnerText, this.Rain.GetType());
        XmlNodeList xmlNodeList9 = xml.SelectNodes(str + "CharaGraphicQuality");
        if (xmlNodeList9 != null && xmlNodeList9.Item(0) is XmlElement xmlElement)
          this.CharaGraphicQuality = (byte) BaseSystem.Cast(xmlElement.InnerText, this.CharaGraphicQuality.GetType());
        XmlNodeList xmlNodeList10 = xml.SelectNodes(str + "MapGraphicQuality");
        if (xmlNodeList10 != null && xmlNodeList10.Item(0) is XmlElement xmlElement)
          this.MapGraphicQuality = (byte) BaseSystem.Cast(xmlElement.InnerText, this.MapGraphicQuality.GetType());
        XmlNodeList xmlNodeList11 = xml.SelectNodes(str + "FaceLight");
        if (xmlNodeList11 != null && xmlNodeList11.Item(0) is XmlElement xmlElement)
          this.FaceLight = (bool) BaseSystem.Cast(xmlElement.InnerText, this.FaceLight.GetType());
        XmlNodeList xmlNodeList12 = xml.SelectNodes(str + "AmbientLight");
        if (xmlNodeList12 != null && xmlNodeList12.Item(0) is XmlElement xmlElement)
          this.AmbientLight = (bool) BaseSystem.Cast(xmlElement.InnerText, this.AmbientLight.GetType());
        XmlNodeList xmlNodeList13 = xml.SelectNodes(str + "Shield");
        if (xmlNodeList13 != null && xmlNodeList13.Item(0) is XmlElement xmlElement)
          this.Shield = (bool) BaseSystem.Cast(xmlElement.InnerText, this.Shield.GetType());
        XmlNodeList xmlNodeList14 = xml.SelectNodes(str + "SimpleBody");
        if (xmlNodeList14 != null && xmlNodeList14.Item(0) is XmlElement xmlElement)
          this.SimpleBody = (bool) BaseSystem.Cast(xmlElement.InnerText, this.SimpleBody.GetType());
        XmlNodeList xmlNodeList15 = xml.SelectNodes(str + "SilhouetteColor");
        if (xmlNodeList15 != null && xmlNodeList15.Item(0) is XmlElement xmlElement)
          this.SilhouetteColor = (Color) BaseSystem.Cast(xmlElement.InnerText, this.SilhouetteColor.GetType());
        XmlNodeList xmlNodeList16 = xml.SelectNodes(str + "BackColor");
        if (xmlNodeList16 != null && xmlNodeList16.Item(0) is XmlElement xmlElement)
          this.BackColor = (Color) BaseSystem.Cast(xmlElement.InnerText, this.BackColor.GetType());
        XmlNodeList xmlNodeList17 = xml.SelectNodes(str + "MaxCharaNum");
        if (xmlNodeList17 != null && xmlNodeList17.Item(0) is XmlElement xmlElement)
          this.MaxCharaNum = (int) BaseSystem.Cast(xmlElement.InnerText, this.MaxCharaNum.GetType());
        for (int index = 0; index < 4; ++index)
        {
          XmlNodeList xmlNodeList18 = xml.SelectNodes(str + "CharasEntry" + (object) index);
          if (xmlNodeList18 != null && xmlNodeList18.Item(0) is XmlElement xmlElement)
            this.CharasEntry[index] = (bool) BaseSystem.Cast(xmlElement.InnerText, this.CharasEntry[index].GetType());
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
      }
    }

    public override void Write(XmlWriter writer)
    {
      writer.WriteStartElement(this.elementName);
      writer.WriteStartElement("SelfShadow");
      writer.WriteValue(BaseSystem.ConvertString((object) this.SelfShadow));
      writer.WriteEndElement();
      writer.WriteStartElement("Bloom");
      writer.WriteValue(BaseSystem.ConvertString((object) this.Bloom));
      writer.WriteEndElement();
      writer.WriteStartElement("SSAO");
      writer.WriteValue(BaseSystem.ConvertString((object) this.SSAO));
      writer.WriteEndElement();
      writer.WriteStartElement("SSR");
      writer.WriteValue(BaseSystem.ConvertString((object) this.SSR));
      writer.WriteEndElement();
      writer.WriteStartElement("DepthOfField");
      writer.WriteValue(BaseSystem.ConvertString((object) this.DepthOfField));
      writer.WriteEndElement();
      writer.WriteStartElement("Atmospheric");
      writer.WriteValue(BaseSystem.ConvertString((object) this.Atmospheric));
      writer.WriteEndElement();
      writer.WriteStartElement("Vignette");
      writer.WriteValue(BaseSystem.ConvertString((object) this.Vignette));
      writer.WriteEndElement();
      writer.WriteStartElement("Rain");
      writer.WriteValue(BaseSystem.ConvertString((object) this.Rain));
      writer.WriteEndElement();
      writer.WriteStartElement("CharaGraphicQuality");
      writer.WriteValue(BaseSystem.ConvertString((object) this.CharaGraphicQuality));
      writer.WriteEndElement();
      writer.WriteStartElement("MapGraphicQuality");
      writer.WriteValue(BaseSystem.ConvertString((object) this.MapGraphicQuality));
      writer.WriteEndElement();
      writer.WriteStartElement("FaceLight");
      writer.WriteValue(BaseSystem.ConvertString((object) this.FaceLight));
      writer.WriteEndElement();
      writer.WriteStartElement("AmbientLight");
      writer.WriteValue(BaseSystem.ConvertString((object) this.AmbientLight));
      writer.WriteEndElement();
      writer.WriteStartElement("Shield");
      writer.WriteValue(BaseSystem.ConvertString((object) this.Shield));
      writer.WriteEndElement();
      writer.WriteStartElement("SimpleBody");
      writer.WriteValue(BaseSystem.ConvertString((object) this.SimpleBody));
      writer.WriteEndElement();
      writer.WriteStartElement("SilhouetteColor");
      writer.WriteValue(BaseSystem.ConvertString((object) this.SilhouetteColor));
      writer.WriteEndElement();
      writer.WriteStartElement("BackColor");
      writer.WriteValue(BaseSystem.ConvertString((object) this.BackColor));
      writer.WriteEndElement();
      writer.WriteStartElement("MaxCharaNum");
      writer.WriteValue(BaseSystem.ConvertString((object) this.MaxCharaNum));
      writer.WriteEndElement();
      for (int index = 0; index < 4; ++index)
      {
        string localName = "CharasEntry" + (object) index;
        writer.WriteStartElement(localName);
        writer.WriteValue(BaseSystem.ConvertString((object) this.CharasEntry[index]));
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
  }
}
