// Decompiled with JetBrains decompiler
// Type: AIProject.ValueInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class ValueInfo
  {
    private float _cycle;

    public ValueInfo(float term)
    {
      this.Term = term;
      this._cycle = 0.0f;
    }

    public float Term { get; set; }

    private int Add()
    {
      return this.Add(1f);
    }

    private int Add(float speed)
    {
      if ((double) this.Term <= 0.0)
        return 0;
      float num = 1f / this.Term;
      if ((double) this._cycle < 1.0)
      {
        this._cycle += Time.get_deltaTime() * num * speed;
        return 0;
      }
      this._cycle = 0.0f;
      return 1;
    }

    public static int operator *(ValueInfo a, float d)
    {
      return a.Add(d);
    }

    public static implicit operator int(ValueInfo a)
    {
      return a.Add();
    }
  }
}
