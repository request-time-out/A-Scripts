// Decompiled with JetBrains decompiler
// Type: NormalData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalData : ScriptableObject
{
  public List<NormalData.Param> data;

  public NormalData()
  {
    base.\u002Ector();
  }

  [Serializable]
  public class Param
  {
    public string ObjectName = string.Empty;
    public List<Vector3> NormalMin = new List<Vector3>();
    public List<Vector3> NormalMax = new List<Vector3>();
  }
}
