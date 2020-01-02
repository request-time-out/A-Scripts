﻿// Decompiled with JetBrains decompiler
// Type: VectorObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using Vectrosity;

public class VectorObject : MonoBehaviour
{
  public VectorObject.Shape shape;

  public VectorObject()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    VectorLine vectorLine = new VectorLine("Shape", XrayLineData.use.shapePoints[(int) this.shape], XrayLineData.use.lineTexture, XrayLineData.use.lineWidth);
    vectorLine.set_color(Color32.op_Implicit(Color.get_green()));
    VectorManager.ObjectSetup(((Component) this).get_gameObject(), vectorLine, (Visibility) 2, (Brightness) 1);
  }

  public enum Shape
  {
    Cube,
    Sphere,
  }
}
