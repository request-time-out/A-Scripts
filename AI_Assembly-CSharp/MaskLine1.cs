// Decompiled with JetBrains decompiler
// Type: MaskLine1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class MaskLine1 : MonoBehaviour
{
  public int numberOfRects;
  public Color lineColor;
  public GameObject mask;
  public float moveSpeed;
  private VectorLine rectLine;
  private float t;
  private Vector3 startPos;

  public MaskLine1()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.rectLine = new VectorLine("Rects", new List<Vector3>(this.numberOfRects * 8), 2f);
    int num = 0;
    for (int index = 0; index < this.numberOfRects; ++index)
    {
      this.rectLine.MakeRect(new Rect(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(0.25f, 3f), Random.Range(0.25f, 2f)), num);
      num += 8;
    }
    this.rectLine.set_color(Color32.op_Implicit(this.lineColor));
    this.rectLine.set_capLength(1f);
    this.rectLine.set_drawTransform(((Component) this).get_transform());
    this.rectLine.SetMask(this.mask);
    this.startPos = ((Component) this).get_transform().get_position();
  }

  private void Update()
  {
    this.t = Mathf.Repeat(this.t + Time.get_deltaTime() * this.moveSpeed, 360f);
    ((Component) this).get_transform().set_position(Vector2.op_Implicit(new Vector2((float) (this.startPos.x + (double) Mathf.Sin(this.t) * 1.5), (float) (this.startPos.y + (double) Mathf.Cos(this.t) * 1.5))));
    this.rectLine.Draw();
  }
}
