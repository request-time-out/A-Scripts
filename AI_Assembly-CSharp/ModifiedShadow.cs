// Decompiled with JetBrains decompiler
// Type: ModifiedShadow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModifiedShadow : Shadow
{
  public ModifiedShadow()
  {
    base.\u002Ector();
  }

  public virtual void ModifyMesh(VertexHelper vh)
  {
    if (!((UIBehaviour) this).IsActive())
      return;
    List<UIVertex> uiVertexList = ListPool<UIVertex>.Get();
    vh.GetUIVertexStream(uiVertexList);
    this.ModifyVertices(uiVertexList);
    vh.Clear();
    vh.AddUIVertexTriangleStream(uiVertexList);
    ListPool<UIVertex>.Release(uiVertexList);
  }

  public virtual void ModifyVertices(List<UIVertex> verts)
  {
  }
}
