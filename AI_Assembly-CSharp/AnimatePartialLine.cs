// Decompiled with JetBrains decompiler
// Type: AnimatePartialLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class AnimatePartialLine : MonoBehaviour
{
  public Texture lineTexture;
  public int segments;
  public int visibleLineSegments;
  public float speed;
  private float startIndex;
  private float endIndex;
  private VectorLine line;

  public AnimatePartialLine()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.startIndex = (float) -this.visibleLineSegments;
    this.endIndex = 0.0f;
    this.line = new VectorLine("Spline", new List<Vector2>(this.segments + 1), this.lineTexture, 30f, (LineType) 0, (Joins) 1);
    int num1 = Screen.get_width() / 5;
    int num2 = Screen.get_height() / 3;
    this.line.MakeSpline(new Vector2[4]
    {
      new Vector2((float) num1, (float) num2),
      new Vector2((float) (num1 * 2), (float) (num2 * 2)),
      new Vector2((float) (num1 * 3), (float) (num2 * 2)),
      new Vector2((float) (num1 * 4), (float) num2)
    });
  }

  private void Update()
  {
    this.startIndex += Time.get_deltaTime() * this.speed;
    this.endIndex += Time.get_deltaTime() * this.speed;
    if ((double) this.startIndex >= (double) (this.segments + 1))
    {
      this.startIndex = (float) -this.visibleLineSegments;
      this.endIndex = 0.0f;
    }
    else if ((double) this.startIndex < (double) -this.visibleLineSegments)
    {
      this.startIndex = (float) this.segments;
      this.endIndex = (float) (this.segments + this.visibleLineSegments);
    }
    this.line.set_drawStart((int) this.startIndex);
    this.line.set_drawEnd((int) this.endIndex);
    this.line.Draw();
  }
}
