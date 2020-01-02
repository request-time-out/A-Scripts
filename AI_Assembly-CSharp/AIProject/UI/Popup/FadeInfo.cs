// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Popup.FadeInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject.UI.Popup
{
  [Serializable]
  public struct FadeInfo
  {
    [SerializeField]
    private float alpha;
    [SerializeField]
    private float scale;
    [SerializeField]
    private float time;

    public FadeInfo(float _alpha, float _scale, float _time)
    {
      this.alpha = _alpha;
      this.scale = _scale;
      this.time = _time;
    }

    public float Alpha
    {
      get
      {
        return this.alpha;
      }
    }

    public float Scale
    {
      get
      {
        return this.scale;
      }
    }

    public float Time
    {
      get
      {
        return this.time;
      }
    }
  }
}
