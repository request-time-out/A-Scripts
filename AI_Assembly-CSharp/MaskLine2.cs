// Decompiled with JetBrains decompiler
// Type: MaskLine2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class MaskLine2 : MonoBehaviour
{
  public int numberOfPoints;
  public Color lineColor;
  public GameObject mask;
  public float lineWidth;
  public float lineHeight;
  private VectorLine spikeLine;
  private float t;
  private Vector3 startPos;

  public MaskLine2()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.spikeLine = new VectorLine("SpikeLine", new List<Vector3>(this.numberOfPoints), 2f, (LineType) 0);
    float num = this.lineHeight / 2f;
    for (int index = 0; index < this.numberOfPoints; ++index)
    {
      this.spikeLine.get_points3()[index] = Vector2.op_Implicit(new Vector2(Random.Range((float) (-(double) this.lineWidth / 2.0), this.lineWidth / 2f), num));
      num -= this.lineHeight / (float) this.numberOfPoints;
    }
    this.spikeLine.set_color(Color32.op_Implicit(this.lineColor));
    this.spikeLine.set_drawTransform(((Component) this).get_transform());
    this.spikeLine.SetMask(this.mask);
    this.startPos = ((Component) this).get_transform().get_position();
  }

  private void Update()
  {
    this.t = Mathf.Repeat(this.t + Time.get_deltaTime(), 360f);
    ((Component) this).get_transform().set_position(Vector2.op_Implicit(new Vector2((float) this.startPos.x, (float) (this.startPos.y + (double) Mathf.Cos(this.t) * 4.0))));
    this.spikeLine.Draw();
  }
}
