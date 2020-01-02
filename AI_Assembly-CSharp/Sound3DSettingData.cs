// Decompiled with JetBrains decompiler
// Type: Sound3DSettingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class Sound3DSettingData : ScriptableObject
{
  public List<Sound3DSettingData.Param> param;

  public Sound3DSettingData()
  {
    base.\u002Ector();
  }

  [Serializable]
  public class Param
  {
    public int No;
    public float DopplerLevel;
    public float Spread;
    public float MinDistance;
    public float MaxDistance;
    public int AudioRolloffMode;
  }
}
