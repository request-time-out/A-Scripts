// Decompiled with JetBrains decompiler
// Type: PowerBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class PowerBar : MonoBehaviour
{
  public float speed;
  public int lineWidth;
  public float radius;
  public int segmentCount;
  private VectorLine bar;
  private Vector2 position;
  private float currentPower;
  private float targetPower;

  public PowerBar()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.position = new Vector2(this.radius + 20f, (float) Screen.get_height() - (this.radius + 20f));
    VectorLine vectorLine = new VectorLine("BarBackground", new List<Vector2>(50), (Texture) null, (float) this.lineWidth, (LineType) 0, (Joins) 1);
    vectorLine.MakeCircle(Vector2.op_Implicit(this.position), this.radius);
    vectorLine.Draw();
    this.bar = new VectorLine("TotalBar", new List<Vector2>(this.segmentCount + 1), (Texture) null, (float) (this.lineWidth - 4), (LineType) 0, (Joins) 1);
    this.bar.set_color(Color32.op_Implicit(Color.get_black()));
    this.bar.MakeArc(Vector2.op_Implicit(this.position), this.radius, this.radius, 0.0f, 270f);
    this.bar.Draw();
    this.currentPower = Random.get_value();
    this.SetTargetPower();
    this.bar.SetColor(Color32.op_Implicit(Color.get_red()), 0, (int) Mathf.Lerp(0.0f, (float) this.segmentCount, this.currentPower));
  }

  private void SetTargetPower()
  {
    this.targetPower = Random.get_value();
  }

  private void Update()
  {
    float currentPower = this.currentPower;
    if ((double) this.targetPower < (double) this.currentPower)
    {
      this.currentPower -= this.speed * Time.get_deltaTime();
      if ((double) this.currentPower < (double) this.targetPower)
        this.SetTargetPower();
      this.bar.SetColor(Color32.op_Implicit(Color.get_black()), (int) Mathf.Lerp(0.0f, (float) this.segmentCount, this.currentPower), (int) Mathf.Lerp(0.0f, (float) this.segmentCount, currentPower));
    }
    else
    {
      this.currentPower += this.speed * Time.get_deltaTime();
      if ((double) this.currentPower > (double) this.targetPower)
        this.SetTargetPower();
      this.bar.SetColor(Color32.op_Implicit(Color.get_red()), (int) Mathf.Lerp(0.0f, (float) this.segmentCount, currentPower), (int) Mathf.Lerp(0.0f, (float) this.segmentCount, this.currentPower));
    }
  }

  private void OnGUI()
  {
    GUI.Label(new Rect((float) (Screen.get_width() / 2 - 40), (float) (Screen.get_height() / 2 - 15), 80f, 30f), "Power: " + (this.currentPower * 100f).ToString("f0") + "%");
  }
}
