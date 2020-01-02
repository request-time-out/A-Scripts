// Decompiled with JetBrains decompiler
// Type: DrawLinesTouch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLinesTouch : MonoBehaviour
{
  public Texture2D lineTex;
  public int maxPoints;
  public float lineWidth;
  public int minPixelMove;
  public bool useEndCap;
  public Texture2D capLineTex;
  public Texture2D capTex;
  public float capLineWidth;
  private VectorLine line;
  private Vector2 previousPosition;
  private int sqrMinPixelMove;
  private bool canDraw;
  private Touch touch;

  public DrawLinesTouch()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Texture2D texture2D;
    float num;
    if (this.useEndCap)
    {
      VectorLine.SetEndCap("RoundCap", (EndCap) 2, new Texture2D[2]
      {
        this.capLineTex,
        this.capTex
      });
      texture2D = this.capLineTex;
      num = this.capLineWidth;
    }
    else
    {
      texture2D = this.lineTex;
      num = this.lineWidth;
    }
    this.line = new VectorLine("DrawnLine", new List<Vector2>(), (Texture) texture2D, num, (LineType) 0, (Joins) 1);
    this.line.set_endPointsUpdate(2);
    if (this.useEndCap)
      this.line.set_endCap("RoundCap");
    this.sqrMinPixelMove = this.minPixelMove * this.minPixelMove;
  }

  private void Update()
  {
    if (Input.get_touchCount() <= 0)
      return;
    this.touch = Input.GetTouch(0);
    if (((Touch) ref this.touch).get_phase() == null)
    {
      this.line.get_points2().Clear();
      this.line.Draw();
      this.previousPosition = ((Touch) ref this.touch).get_position();
      this.line.get_points2().Add(((Touch) ref this.touch).get_position());
      this.canDraw = true;
    }
    else
    {
      if (((Touch) ref this.touch).get_phase() != 1)
        return;
      Vector2 vector2 = Vector2.op_Subtraction(((Touch) ref this.touch).get_position(), this.previousPosition);
      if ((double) ((Vector2) ref vector2).get_sqrMagnitude() <= (double) this.sqrMinPixelMove || !this.canDraw)
        return;
      this.previousPosition = ((Touch) ref this.touch).get_position();
      this.line.get_points2().Add(((Touch) ref this.touch).get_position());
      if (this.line.get_points2().Count >= this.maxPoints)
        this.canDraw = false;
      this.line.Draw();
    }
  }
}
