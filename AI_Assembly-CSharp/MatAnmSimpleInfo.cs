// Decompiled with JetBrains decompiler
// Type: MatAnmSimpleInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class MatAnmSimpleInfo
{
  public bool change_RGB = true;
  public bool change_A = true;
  public YS_Timer yst;
  public MeshRenderer mr;
  public int materialNo;
  public float time;
  public Gradient Value;
  private float cnt;

  public void Update(int colorPropertyId)
  {
    if (Object.op_Equality((Object) null, (Object) this.mr))
      return;
    float num;
    if (Object.op_Equality((Object) null, (Object) this.yst))
    {
      this.cnt += Time.get_deltaTime();
      while ((double) this.time < (double) this.cnt)
        this.cnt -= this.time;
      num = this.cnt / this.time;
    }
    else
      num = this.yst.rate;
    Color color = ((Renderer) this.mr).get_material().GetColor(colorPropertyId);
    if (this.change_RGB)
    {
      color.r = this.Value.Evaluate(num).r;
      color.g = this.Value.Evaluate(num).g;
      color.b = this.Value.Evaluate(num).b;
    }
    if (this.change_A)
      color.a = this.Value.Evaluate(num).a;
    ((Renderer) this.mr).get_material().SetColor(colorPropertyId, color);
  }
}
