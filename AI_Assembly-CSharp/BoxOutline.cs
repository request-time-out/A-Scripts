// Decompiled with JetBrains decompiler
// Type: BoxOutline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxOutline : ModifiedShadow
{
  [SerializeField]
  [Range(1f, 20f)]
  private int m_halfSampleCountX = 1;
  [SerializeField]
  [Range(1f, 20f)]
  private int m_halfSampleCountY = 1;
  private const int maxHalfSampleCount = 20;

  public int halfSampleCountX
  {
    get
    {
      return this.m_halfSampleCountX;
    }
    set
    {
      this.m_halfSampleCountX = Mathf.Clamp(value, 1, 20);
      if (!Object.op_Inequality((Object) ((BaseMeshEffect) this).get_graphic(), (Object) null))
        return;
      ((BaseMeshEffect) this).get_graphic().SetVerticesDirty();
    }
  }

  public int halfSampleCountY
  {
    get
    {
      return this.m_halfSampleCountY;
    }
    set
    {
      this.m_halfSampleCountY = Mathf.Clamp(value, 1, 20);
      if (!Object.op_Inequality((Object) ((BaseMeshEffect) this).get_graphic(), (Object) null))
        return;
      ((BaseMeshEffect) this).get_graphic().SetVerticesDirty();
    }
  }

  public override void ModifyVertices(List<UIVertex> verts)
  {
    if (!((UIBehaviour) this).IsActive())
      return;
    int num1 = verts.Count * (this.m_halfSampleCountX * 2 + 1) * (this.m_halfSampleCountY * 2 + 1);
    if (verts.Capacity < num1)
      verts.Capacity = num1;
    int count = verts.Count;
    int num2 = 0;
    float num3 = (float) this.get_effectDistance().x / (float) this.m_halfSampleCountX;
    float num4 = (float) this.get_effectDistance().y / (float) this.m_halfSampleCountY;
    for (int index1 = -this.m_halfSampleCountX; index1 <= this.m_halfSampleCountX; ++index1)
    {
      for (int index2 = -this.m_halfSampleCountY; index2 <= this.m_halfSampleCountY; ++index2)
      {
        if (index1 != 0 || index2 != 0)
        {
          int num5 = num2 + count;
          this.ApplyShadow(verts, Color32.op_Implicit(this.get_effectColor()), num2, num5, num3 * (float) index1, num4 * (float) index2);
          num2 = num5;
        }
      }
    }
  }
}
