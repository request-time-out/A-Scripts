// Decompiled with JetBrains decompiler
// Type: UVData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UVData : ScriptableObject
{
  public List<UVData.Param> data;
  public int[] rangeIndex;

  public UVData()
  {
    base.\u002Ector();
  }

  [Serializable]
  public class Param
  {
    public string ObjectName = string.Empty;
    public List<Vector2> UV = new List<Vector2>();
  }
}
