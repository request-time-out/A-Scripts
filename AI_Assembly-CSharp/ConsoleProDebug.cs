// Decompiled with JetBrains decompiler
// Type: ConsoleProDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public static class ConsoleProDebug
{
  public static void Clear()
  {
  }

  public static void LogToFilter(string inLog, string inFilterName)
  {
    Debug.Log((object) (inLog + "\nCPAPI:{\"cmd\":\"Filter\" \"name\":\"" + inFilterName + "\"}"));
  }

  public static void Watch(string inName, string inValue)
  {
    Debug.Log((object) (inName + " : " + inValue + "\nCPAPI:{\"cmd\":\"Watch\" \"name\":\"" + inName + "\"}"));
  }
}
