// Decompiled with JetBrains decompiler
// Type: AIProject.Threshold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public struct Threshold
  {
    public float min;
    public float max;

    public Threshold(float minValue, float maxValue)
    {
      this.min = minValue;
      this.max = maxValue;
    }

    public float RandomValue
    {
      get
      {
        return Random.Range(this.min, this.max);
      }
    }

    public float Lerp(float t)
    {
      return Mathf.Lerp(this.min, this.max, t);
    }

    public bool IsRange(float value)
    {
      return (double) value >= (double) this.min && (double) value <= (double) this.max;
    }
  }
}
