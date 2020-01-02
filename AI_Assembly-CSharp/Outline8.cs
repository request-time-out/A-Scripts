// Decompiled with JetBrains decompiler
// Type: Outline8
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Outline8 : ModifiedShadow
{
  public override void ModifyVertices(List<UIVertex> verts)
  {
    if (!((UIBehaviour) this).IsActive())
      return;
    int num1 = verts.Count * 9;
    if (verts.Capacity < num1)
      verts.Capacity = num1;
    int count = verts.Count;
    int num2 = 0;
    for (int index1 = -1; index1 <= 1; ++index1)
    {
      for (int index2 = -1; index2 <= 1; ++index2)
      {
        if (index1 != 0 || index2 != 0)
        {
          int num3 = num2 + count;
          this.ApplyShadow(verts, Color32.op_Implicit(this.get_effectColor()), num2, num3, (float) this.get_effectDistance().x * (float) index1, (float) this.get_effectDistance().y * (float) index2);
          num2 = num3;
        }
      }
    }
  }
}
