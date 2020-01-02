// Decompiled with JetBrains decompiler
// Type: SoundSettingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettingData : ScriptableObject
{
  public List<SoundSettingData.Param> param;

  public SoundSettingData()
  {
    base.\u002Ector();
  }

  [Serializable]
  public class Param
  {
    public int No;
    public float Volume;
    public float Pitch;
    public float Pan;
    public float Level3D;
    public int Priority;
    public bool PlayAwake;
    public bool Loop;
    public float DelayTime;
    public int Setting3DNo;
  }
}
