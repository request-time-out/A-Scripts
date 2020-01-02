// Decompiled with JetBrains decompiler
// Type: SimpleCurve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SimpleCurve : MonoBehaviour
{
  public Vector2[] curvePoints;
  public int segments;

  public SimpleCurve()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (this.curvePoints.Length != 4)
    {
      Debug.Log((object) "Curve points array must have 4 elements only");
    }
    else
    {
      VectorLine vectorLine = new VectorLine("Curve", new List<Vector2>(this.segments + 1), 2f, (LineType) 0, (Joins) 1);
      vectorLine.MakeCurve(this.curvePoints, this.segments);
      vectorLine.Draw();
    }
  }
}
