// Decompiled with JetBrains decompiler
// Type: Utility.Xml.Data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Xml;

namespace Utility.Xml
{
  public class Data
  {
    protected readonly string elementName;

    public Data(string elementName)
    {
      this.elementName = elementName;
    }

    public virtual void Init()
    {
    }

    public virtual void Read(string rootName, XmlDocument xml)
    {
    }

    public virtual void Write(XmlWriter writer)
    {
    }
  }
}
