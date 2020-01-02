// Decompiled with JetBrains decompiler
// Type: Studio.TextSlideEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Studio
{
  public class TextSlideEffect : TextEffect
  {
    private float m_SubPos;

    public float subPos
    {
      get
      {
        return this.m_SubPos;
      }
      set
      {
        this.m_SubPos = value;
        if (!Object.op_Inequality((Object) this.graphic, (Object) null))
          return;
        this.graphic.SetVerticesDirty();
      }
    }

    protected override void Modify(ref List<UIVertex> _stream)
    {
      int num = 0;
      for (int count = _stream.Count; num < count; num += 6)
      {
        for (int index = 0; index < 6; ++index)
        {
          UIVertex uiVertex = _stream[num + index];
          Vector3 position = (Vector3) uiVertex.position;
          ref Vector3 local = ref position;
          local.x = (__Null) (local.x - (double) this.subPos);
          uiVertex.position = (__Null) position;
          _stream[num + index] = uiVertex;
        }
      }
    }
  }
}
