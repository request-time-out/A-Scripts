// Decompiled with JetBrains decompiler
// Type: TextDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class TextDemo : MonoBehaviour
{
  public string text;
  public int textSize;
  private VectorLine textLine;

  public TextDemo()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.textLine = new VectorLine("Text", new List<Vector2>(), 1f);
    this.textLine.set_color(Color32.op_Implicit(Color.get_yellow()));
    this.textLine.set_drawTransform(((Component) this).get_transform());
    this.textLine.MakeText(this.text, Vector2.op_Implicit(new Vector2((float) (Screen.get_width() / 2 - this.text.Length * this.textSize / 2), (float) (Screen.get_height() / 2 + this.textSize / 2))), (float) this.textSize);
  }

  private void Update()
  {
    ((Component) this).get_transform().RotateAround(Vector2.op_Implicit(new Vector2((float) (Screen.get_width() / 2), (float) (Screen.get_height() / 2))), Vector3.get_forward(), Time.get_deltaTime() * 45f);
    Vector3 localScale = ((Component) this).get_transform().get_localScale();
    localScale.x = (__Null) (1.0 + (double) Mathf.Sin(Time.get_time() * 3f) * 0.300000011920929);
    localScale.y = (__Null) (1.0 + (double) Mathf.Cos(Time.get_time() * 3f) * 0.300000011920929);
    ((Component) this).get_transform().set_localScale(localScale);
    this.textLine.Draw();
  }
}
