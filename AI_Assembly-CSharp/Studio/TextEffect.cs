// Decompiled with JetBrains decompiler
// Type: Studio.TextEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using GUITree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public abstract class TextEffect : UIBehaviour, IMeshModifier
  {
    private Graphic m_Graphic;

    protected TextEffect()
    {
      base.\u002Ector();
    }

    public Graphic graphic
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Graphic, (Object) null))
          this.m_Graphic = (Graphic) ((Component) this).GetComponent<Graphic>();
        return this.m_Graphic;
      }
    }

    public void ModifyMesh(Mesh mesh)
    {
    }

    public void ModifyMesh(VertexHelper verts)
    {
      List<UIVertex> _stream = ListPool<UIVertex>.Get();
      verts.GetUIVertexStream(_stream);
      this.Modify(ref _stream);
      verts.Clear();
      verts.AddUIVertexTriangleStream(_stream);
      ListPool<UIVertex>.Release(_stream);
    }

    protected abstract void Modify(ref List<UIVertex> _stream);
  }
}
