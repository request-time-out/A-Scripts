// Decompiled with JetBrains decompiler
// Type: Utility.Xml.Seializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.IO;
using System.Xml.Serialization;

namespace Utility.Xml
{
  public class Seializer
  {
    public static T Seialize<T>(string filename, T data)
    {
      using (FileStream fileStream = new FileStream(filename, FileMode.Create))
        new XmlSerializer(typeof (T)).Serialize((Stream) fileStream, (object) data);
      return data;
    }

    public static T Deserialize<T>(string filename)
    {
      using (FileStream fileStream = new FileStream(filename, FileMode.Open))
        return (T) new XmlSerializer(typeof (T)).Deserialize((Stream) fileStream);
    }
  }
}
