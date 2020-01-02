// Decompiled with JetBrains decompiler
// Type: DrawPoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawPoints : MonoBehaviour
{
  public float dotSize;
  public int numberOfDots;
  public int numberOfRings;
  public Color dotColor;

  public DrawPoints()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    int length = this.numberOfDots * this.numberOfRings;
    Vector2[] vector2Array = new Vector2[length];
    Color32[] color32Array = new Color32[length];
    float num = (float) (1.0 - 0.75 / (double) length);
    for (int index = 0; index < color32Array.Length; ++index)
    {
      color32Array[index] = Color32.op_Implicit(this.dotColor);
      DrawPoints drawPoints = this;
      drawPoints.dotColor = Color.op_Multiply(drawPoints.dotColor, num);
    }
    VectorLine vectorLine = new VectorLine("Dots", new List<Vector2>((IEnumerable<Vector2>) vector2Array), this.dotSize, (LineType) 2);
    vectorLine.SetColors(new List<Color32>((IEnumerable<Color32>) color32Array));
    for (int index = 0; index < this.numberOfRings; ++index)
      vectorLine.MakeCircle(Vector2.op_Implicit(new Vector2((float) (Screen.get_width() / 2), (float) (Screen.get_height() / 2))), (float) (Screen.get_height() / (index + 2)), this.numberOfDots, this.numberOfDots * index);
    vectorLine.Draw();
  }
}
