// Decompiled with JetBrains decompiler
// Type: Ellipse1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Ellipse1 : MonoBehaviour
{
  public Texture lineTexture;
  public float xRadius;
  public float yRadius;
  public int segments;
  public float pointRotation;

  public Ellipse1()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    VectorLine vectorLine = new VectorLine("Line", new List<Vector2>(this.segments + 1), this.lineTexture, 3f, (LineType) 0);
    vectorLine.MakeEllipse(Vector2.op_Implicit(new Vector2((float) (Screen.get_width() / 2), (float) (Screen.get_height() / 2))), this.xRadius, this.yRadius, this.segments, this.pointRotation);
    vectorLine.Draw();
  }
}
