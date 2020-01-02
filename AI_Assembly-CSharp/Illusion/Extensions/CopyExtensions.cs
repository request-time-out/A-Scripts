// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.CopyExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Illusion.Extensions
{
  public static class CopyExtensions
  {
    public static T DeepCopy<T>(this T self)
    {
      if ((object) self == null)
        return default (T);
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) memoryStream, (object) self);
        memoryStream.Position = 0L;
        return (T) binaryFormatter.Deserialize((Stream) memoryStream);
      }
      finally
      {
        memoryStream.Close();
      }
    }
  }
}
