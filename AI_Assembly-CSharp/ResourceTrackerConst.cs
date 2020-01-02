// Decompiled with JetBrains decompiler
// Type: ResourceTrackerConst
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.IO;
using UnityEngine;

public class ResourceTrackerConst
{
  public static string shaderPropertyNameJsonPath = Path.Combine(Path.Combine(Application.get_persistentDataPath(), "TestTools"), "ShaderPropertyNameRecord.json");
  public static char shaderPropertyNameJsonToken = '$';

  public static string FormatBytes(int bytes)
  {
    if (bytes < 0)
      return "error bytes";
    if (bytes < 1024)
      return bytes.ToString() + "b";
    return bytes < 1048576 ? (bytes / 1024).ToString() + "kb" : (bytes / 1024 / 1024).ToString() + "mb";
  }
}
