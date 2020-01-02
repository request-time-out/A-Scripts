// Decompiled with JetBrains decompiler
// Type: ScribbleCube
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class ScribbleCube : MonoBehaviour
{
  public Texture lineTexture;
  public Material lineMaterial;
  public int lineWidth;
  private Color color1;
  private Color color2;
  private VectorLine line;
  private List<Color32> lineColors;
  private int numberOfPoints;

  public ScribbleCube()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.line = new VectorLine("Line", new List<Vector3>(this.numberOfPoints), this.lineTexture, (float) this.lineWidth, (LineType) 0);
    this.line.set_material(this.lineMaterial);
    this.line.set_drawTransform(((Component) this).get_transform());
    this.LineSetup(false);
  }

  private void LineSetup(bool resize)
  {
    if (resize)
    {
      this.lineColors = (List<Color32>) null;
      this.line.Resize(this.numberOfPoints);
    }
    for (int index = 0; index < this.line.get_points3().Count; ++index)
      this.line.get_points3()[index] = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    this.SetLineColors();
  }

  private void SetLineColors()
  {
    if (this.lineColors == null)
      this.lineColors = new List<Color32>((IEnumerable<Color32>) new Color32[this.numberOfPoints - 1]);
    for (int index = 0; index < this.lineColors.Count; ++index)
      this.lineColors[index] = Color32.op_Implicit(Color.Lerp(this.color1, this.color2, (float) index / (float) this.lineColors.Count));
    this.line.SetColors(this.lineColors);
  }

  private void LateUpdate()
  {
    this.line.Draw();
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(20f, 10f, 250f, 30f), "Zoom with scrollwheel or arrow keys");
    if (GUI.Button(new Rect(20f, 50f, 100f, 30f), "Change colors"))
    {
      int component1 = Random.Range(0, 3);
      int component2;
      do
      {
        component2 = Random.Range(0, 3);
      }
      while (component2 == component1);
      this.color1 = this.RandomColor(this.color1, component1);
      this.color2 = this.RandomColor(this.color2, component2);
      this.SetLineColors();
    }
    GUI.Label(new Rect(20f, 100f, 150f, 30f), "Number of points: " + (object) this.numberOfPoints);
    this.numberOfPoints = (int) GUI.HorizontalSlider(new Rect(20f, 130f, 120f, 30f), (float) this.numberOfPoints, 50f, 1000f);
    if (!GUI.Button(new Rect(160f, 120f, 40f, 30f), "Set"))
      return;
    this.LineSetup(true);
  }

  private Color RandomColor(Color color, int component)
  {
    for (int index = 0; index < 3; ++index)
    {
      if (index == component)
        ((Color) ref color).set_Item(index, Random.get_value() * 0.25f);
      else
        ((Color) ref color).set_Item(index, (float) ((double) Random.get_value() * 0.5 + 0.5));
    }
    return color;
  }
}
