// Decompiled with JetBrains decompiler
// Type: Utility.Xml.Control
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Utility.Xml
{
  public class Control
  {
    private List<Data> dataList = new List<Data>();
    private readonly string savePath;
    private readonly string saveName;
    private readonly string rootName;

    public Control(string savePath, string saveName, string rootName, List<Data> dataList)
    {
      this.savePath = savePath;
      this.saveName = saveName;
      this.rootName = rootName;
      this.dataList = dataList;
      this.Init();
    }

    public List<Data> DataList
    {
      get
      {
        return this.dataList;
      }
    }

    public Data this[int index]
    {
      get
      {
        return this.dataList[index];
      }
    }

    public void Init()
    {
      foreach (Data data in this.dataList)
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
        foreach (Data data in this.dataList)
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
        if (!File.Exists(str))
          return;
        xml.Load(str);
        foreach (Data data in this.dataList)
          data.Read(this.rootName, xml);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
      }
    }
  }
}
