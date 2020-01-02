// Decompiled with JetBrains decompiler
// Type: AIProject.TriThreshold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public struct TriThreshold
  {
    public float SThreshold;
    public float MThreshold;
    public float LThreshold;

    public TriThreshold(int s, int m, int l)
    {
      this.SThreshold = (float) s;
      this.MThreshold = (float) m;
      this.LThreshold = (float) l;
    }

    public TriThreshold(float s, float m, float l)
    {
      this.SThreshold = s;
      this.MThreshold = m;
      this.LThreshold = l;
    }

    public int Evaluate(float t)
    {
      if (Mathf.Approximately(this.SThreshold, 0.0f) && Mathf.Approximately(this.MThreshold, 0.0f) && Mathf.Approximately(this.LThreshold, 0.0f))
        return 0;
      if (Mathf.Approximately(this.MThreshold, 0.0f))
        return Mathf.RoundToInt(Mathf.Lerp(this.SThreshold, this.LThreshold, t));
      return (double) t < 0.5 ? (int) Mathf.Lerp(this.SThreshold, this.MThreshold, Mathf.InverseLerp(0.0f, 0.5f, t)) : (int) Mathf.Lerp(this.MThreshold, this.LThreshold, Mathf.InverseLerp(0.5f, 1f, t));
    }

    public float EvaluateFloat(float t)
    {
      if (Mathf.Approximately(this.MThreshold, 0.0f) && Mathf.Approximately(this.LThreshold, 0.0f))
        return this.SThreshold;
      if (Mathf.Approximately(this.LThreshold, 0.0f))
        return Mathf.Lerp(this.SThreshold, this.LThreshold, t);
      return (double) t < 0.5 ? Mathf.Lerp(this.SThreshold, this.MThreshold, Mathf.InverseLerp(0.0f, 0.5f, t)) : Mathf.Lerp(this.MThreshold, this.LThreshold, Mathf.InverseLerp(0.5f, 1f, t));
    }
  }
}
