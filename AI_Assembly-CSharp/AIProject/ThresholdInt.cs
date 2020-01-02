// Decompiled with JetBrains decompiler
// Type: AIProject.ThresholdInt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public struct ThresholdInt
  {
    public int min;
    public int max;

    public ThresholdInt(int minValue, int maxValue)
    {
      this.min = minValue;
      this.max = maxValue;
    }

    public int RandomValue
    {
      get
      {
        return Random.Range(this.min, this.max + 1);
      }
    }

    public bool IsRange(int value)
    {
      return value >= this.min && value <= this.max;
    }
  }
}
