// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.ParamRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public struct ParamRange
  {
    public float min;
    public float max;
    public float limit;

    public ParamRange(float _min, float _max, float _limit)
    {
      this.min = _min;
      this.max = _max;
      this.limit = _limit;
      this.min = Mathf.Max(Mathf.Min(_min, _max), 0.0f);
      this.max = Mathf.Min(Mathf.Max(_min, this.max), this.limit);
    }

    public float RandomValue
    {
      get
      {
        return Random.Range(this.min, this.max);
      }
    }
  }
}
