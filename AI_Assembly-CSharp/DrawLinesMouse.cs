// Decompiled with JetBrains decompiler
// Type: DrawLinesMouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLinesMouse : MonoBehaviour
{
  public Texture2D lineTex;
  public int maxPoints;
  public float lineWidth;
  public int minPixelMove;
  public bool useEndCap;
  public Texture2D capLineTex;
  public Texture2D capTex;
  public float capLineWidth;
  public bool line3D;
  public float distanceFromCamera;
  private VectorLine line;
  private Vector3 previousPosition;
  private int sqrMinPixelMove;
  private bool canDraw;

  public DrawLinesMouse()
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
    this.line = !this.line3D ? new VectorLine("DrawnLine", new List<Vector2>(), (Texture) texture2D, num, (LineType) 0, (Joins) 1) : new VectorLine("DrawnLine3D", new List<Vector3>(), (Texture) texture2D, num, (LineType) 0, (Joins) 1);
    this.line.set_endPointsUpdate(2);
    if (this.useEndCap)
      this.line.set_endCap("RoundCap");
    this.sqrMinPixelMove = this.minPixelMove * this.minPixelMove;
  }

  private void Update()
  {
    Vector3 mousePos = this.GetMousePos();
    if (Input.GetMouseButtonDown(0))
    {
      if (this.line3D)
      {
        this.line.get_points3().Clear();
        this.line.Draw3D();
      }
      else
      {
        this.line.get_points2().Clear();
        this.line.Draw();
      }
      this.previousPosition = Input.get_mousePosition();
      if (this.line3D)
        this.line.get_points3().Add(mousePos);
      else
        this.line.get_points2().Add(Vector2.op_Implicit(mousePos));
      this.canDraw = true;
    }
    else
    {
      if (!Input.GetMouseButton(0))
        return;
      Vector3 vector3 = Vector3.op_Subtraction(Input.get_mousePosition(), this.previousPosition);
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) this.sqrMinPixelMove || !this.canDraw)
        return;
      this.previousPosition = Input.get_mousePosition();
      int count;
      if (this.line3D)
      {
        this.line.get_points3().Add(mousePos);
        count = this.line.get_points3().Count;
        this.line.Draw3D();
      }
      else
      {
        this.line.get_points2().Add(Vector2.op_Implicit(mousePos));
        count = this.line.get_points2().Count;
        this.line.Draw();
      }
      if (count < this.maxPoints)
        return;
      this.canDraw = false;
    }
  }

  private Vector3 GetMousePos()
  {
    Vector3 mousePosition = Input.get_mousePosition();
    if (!this.line3D)
      return mousePosition;
    mousePosition.z = (__Null) (double) this.distanceFromCamera;
    return Camera.get_main().ScreenToWorldPoint(mousePosition);
  }
}
