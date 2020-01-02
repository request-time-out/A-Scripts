// Decompiled with JetBrains decompiler
// Type: VoiceInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class VoiceInfo : ScriptableObject
{
  public List<VoiceInfo.Param> param;

  public VoiceInfo()
  {
    base.\u002Ector();
  }

  [Serializable]
  public class Param
  {
    public string Personality;
    public int No;
    public int Sort;
  }
}
