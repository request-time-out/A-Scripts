// Decompiled with JetBrains decompiler
// Type: DrawGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawGrid : MonoBehaviour
{
  public int gridPixels;
  private VectorLine gridLine;

  public DrawGrid()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.gridLine = new VectorLine("Grid", new List<Vector2>(), 1f);
    this.gridLine.set_alignOddWidthToPixels(true);
    this.MakeGrid();
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10f, 10f, 30f, 20f), this.gridPixels.ToString());
    this.gridPixels = (int) GUI.HorizontalSlider(new Rect(40f, 15f, 590f, 20f), (float) this.gridPixels, 5f, 200f);
    if (!GUI.get_changed())
      return;
    this.MakeGrid();
  }

  private void MakeGrid()
  {
    this.gridLine.Resize((Screen.get_width() / this.gridPixels + 1 + (Screen.get_height() / this.gridPixels + 1)) * 2);
    int num1 = 0;
    for (int index1 = 0; index1 < Screen.get_width(); index1 += this.gridPixels)
    {
      List<Vector2> points2_1 = this.gridLine.get_points2();
      int index2 = num1;
      int num2 = index2 + 1;
      Vector2 vector2_1 = new Vector2((float) index1, 0.0f);
      points2_1[index2] = vector2_1;
      List<Vector2> points2_2 = this.gridLine.get_points2();
      int index3 = num2;
      num1 = index3 + 1;
      Vector2 vector2_2 = new Vector2((float) index1, (float) (Screen.get_height() - 1));
      points2_2[index3] = vector2_2;
    }
    for (int index1 = 0; index1 < Screen.get_height(); index1 += this.gridPixels)
    {
      List<Vector2> points2_1 = this.gridLine.get_points2();
      int index2 = num1;
      int num2 = index2 + 1;
      Vector2 vector2_1 = new Vector2(0.0f, (float) index1);
      points2_1[index2] = vector2_1;
      List<Vector2> points2_2 = this.gridLine.get_points2();
      int index3 = num2;
      num1 = index3 + 1;
      Vector2 vector2_2 = new Vector2((float) (Screen.get_width() - 1), (float) index1);
      points2_2[index3] = vector2_2;
    }
    this.gridLine.Draw();
  }
}
