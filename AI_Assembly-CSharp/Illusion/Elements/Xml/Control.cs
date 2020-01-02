// Decompiled with JetBrains decompiler
// Type: Illusion.Elements.Xml.Control
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Illusion.Elements.Xml
{
  public class Control
  {
    private readonly string savePath;
    private readonly string saveName;
    private readonly string rootName;
    private Data[] datas;

    public Control(string savePath, string saveName, string rootName, params Data[] datas)
    {
      this.savePath = savePath;
      this.saveName = saveName;
      this.rootName = rootName;
      this.datas = datas;
      this.Init();
    }

    public Data[] Datas
    {
      get
      {
        return this.datas;
      }
    }

    public Data this[int index]
    {
      get
      {
        return this.datas[index];
      }
    }

    public void Init()
    {
      foreach (Data data in this.datas)
        data.Init();
    }

    public void Write()
    {
      string outputFileName = UserData.Create(this.savePath) + this.saveName;
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.Encoding = Encoding.UTF8;
      XmlWriter writer = (XmlWriter) null;
      try
      {
        writer = XmlWriter.Create(outputFileName, settings);
        writer.WriteStartDocument();
        writer.WriteStartElement(this.rootName);
        foreach (Data data in this.datas)
          data.Write(writer);
        writer.WriteEndElement();
        writer.WriteEndDocument();
      }
      finally
      {
        writer?.Close();
      }
    }

    public void Read()
    {
      XmlDocument xml = new XmlDocument();
      try
      {
        string str = UserData.Path + this.savePath + (object) '/' + this.saveName;
        if (!System.IO.File.Exists(str))
          return;
        xml.Load(str);
        foreach (Data data in this.datas)
          data.Read(this.rootName, xml);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
      }
    }
  }
}
