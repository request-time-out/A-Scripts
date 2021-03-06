﻿// Decompiled with JetBrains decompiler
// Type: DeepCopy.DeepCopyUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DeepCopy
{
  internal static class DeepCopyUtils
  {
    public static object DeepCopy(this object target)
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        binaryFormatter.Serialize((Stream) memoryStream, target);
        memoryStream.Position = 0L;
        return binaryFormatter.Deserialize((Stream) memoryStream);
      }
      finally
      {
        memoryStream.Close();
      }
    }
  }
}
