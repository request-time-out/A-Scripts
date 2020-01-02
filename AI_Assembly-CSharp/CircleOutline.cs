// Decompiled with JetBrains decompiler
// Type: CircleOutline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleOutline : ModifiedShadow
{
  [SerializeField]
  private int m_circleCount = 2;
  [SerializeField]
  private int m_firstSample = 4;
  [SerializeField]
  private int m_sampleIncrement = 2;

  public int circleCount
  {
    get
    {
      return this.m_circleCount;
    }
    set
    {
      this.m_circleCount = Mathf.Max(value, 1);
      if (!Object.op_Inequality((Object) ((BaseMeshEffect) this).get_graphic(), (Object) null))
        return;
      ((BaseMeshEffect) this).get_graphic().SetVerticesDirty();
    }
  }

  public int firstSample
  {
    get
    {
      return this.m_firstSample;
    }
    set
    {
      this.m_firstSample = Mathf.Max(value, 2);
      if (!Object.op_Inequality((Object) ((BaseMeshEffect) this).get_graphic(), (Object) null))
        return;
      ((BaseMeshEffect) this).get_graphic().SetVerticesDirty();
    }
  }

  public int sampleIncrement
  {
    get
    {
      return this.m_sampleIncrement;
    }
    set
    {
      this.m_sampleIncrement = Mathf.Max(value, 1);
      if (!Object.op_Inequality((Object) ((BaseMeshEffect) this).get_graphic(), (Object) null))
        return;
      ((BaseMeshEffect) this).get_graphic().SetVerticesDirty();
    }
  }

  public override void ModifyVertices(List<UIVertex> verts)
  {
    if (!((UIBehaviour) this).IsActive())
      return;
    int num1 = (this.m_firstSample * 2 + this.m_sampleIncrement * (this.m_circleCount - 1)) * this.m_circleCount / 2;
    int num2 = verts.Count * (num1 + 1);
    if (verts.Capacity < num2)
      verts.Capacity = num2;
    int count = verts.Count;
    int num3 = 0;
    int firstSample = this.m_firstSample;
    float num4 = (float) this.get_effectDistance().x / (float) this.circleCount;
    float num5 = (float) this.get_effectDistance().y / (float) this.circleCount;
    for (int index1 = 1; index1 <= this.m_circleCount; ++index1)
    {
      float num6 = num4 * (float) index1;
      float num7 = num5 * (float) index1;
      float num8 = 6.283185f / (float) firstSample;
      float num9 = (float) ((double) (index1 % 2) * (double) num8 * 0.5);
      for (int index2 = 0; index2 < firstSample; ++index2)
      {
        int num10 = num3 + count;
        this.ApplyShadow(verts, Color32.op_Implicit(this.get_effectColor()), num3, num10, num6 * Mathf.Cos(num9), num7 * Mathf.Sin(num9));
        num3 = num10;
        num9 += num8;
      }
      firstSample += this.m_sampleIncrement;
    }
  }
}
