// Decompiled with JetBrains decompiler
// Type: SelectLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SelectLine : MonoBehaviour
{
  public float lineThickness;
  public int extraThickness;
  public int numberOfLines;
  private VectorLine[] lines;
  private bool[] wasSelected;

  public SelectLine()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.lines = new VectorLine[this.numberOfLines];
    this.wasSelected = new bool[this.numberOfLines];
    for (int i = 0; i < this.numberOfLines; ++i)
    {
      this.lines[i] = new VectorLine(nameof (SelectLine), new List<Vector2>(5), this.lineThickness, (LineType) 0, (Joins) 0);
      this.SetPoints(i);
    }
  }

  private void SetPoints(int i)
  {
    for (int index = 0; index < this.lines[i].get_points2().Count; ++index)
      this.lines[i].get_points2()[index] = new Vector2((float) Random.Range(0, Screen.get_width()), (float) Random.Range(0, Screen.get_height() - 20));
    this.lines[i].Draw();
  }

  private void Update()
  {
    for (int i = 0; i < this.numberOfLines; ++i)
    {
      int num;
      if (this.lines[i].Selected(Vector2.op_Implicit(Input.get_mousePosition()), this.extraThickness, ref num))
      {
        if (!this.wasSelected[i])
        {
          this.lines[i].SetColor(Color32.op_Implicit(Color.get_green()));
          this.wasSelected[i] = true;
        }
        if (Input.GetMouseButtonDown(0))
          this.SetPoints(i);
      }
      else if (this.wasSelected[i])
      {
        this.wasSelected[i] = false;
        this.lines[i].SetColor(Color32.op_Implicit(Color.get_white()));
      }
    }
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10f, 10f, 800f, 30f), "Click a line to make a new line");
  }
}
