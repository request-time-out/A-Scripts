// Decompiled with JetBrains decompiler
// Type: Ellipse2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Ellipse2 : MonoBehaviour
{
  public Texture lineTexture;
  public int segments;
  public int numberOfEllipses;

  public Ellipse2()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    VectorLine vectorLine = new VectorLine("Line", new List<Vector2>(this.segments * 2 * this.numberOfEllipses), this.lineTexture, 3f);
    for (int index = 0; index < this.numberOfEllipses; ++index)
    {
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector((float) Random.Range(0, Screen.get_width()), (float) Random.Range(0, Screen.get_height()));
      vectorLine.MakeEllipse(Vector2.op_Implicit(vector2), (float) Random.Range(10, Screen.get_width() / 2), (float) Random.Range(10, Screen.get_height() / 2), this.segments, index * (this.segments * 2));
    }
    vectorLine.Draw();
  }
}
