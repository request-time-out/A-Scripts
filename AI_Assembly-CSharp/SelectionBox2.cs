// Decompiled with JetBrains decompiler
// Type: SelectionBox2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SelectionBox2 : MonoBehaviour
{
  public Texture lineTexture;
  public float textureScale;
  private VectorLine selectionLine;
  private Vector2 originalPos;

  public SelectionBox2()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.selectionLine = new VectorLine("Selection", new List<Vector2>(5), this.lineTexture, 4f, (LineType) 0);
    this.selectionLine.set_textureScale(this.textureScale);
    this.selectionLine.set_alignOddWidthToPixels(true);
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10f, 10f, 300f, 25f), "Click & drag to make a selection box");
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
      this.originalPos = Vector2.op_Implicit(Input.get_mousePosition());
    if (Input.GetMouseButton(0))
    {
      this.selectionLine.MakeRect(Vector2.op_Implicit(this.originalPos), Input.get_mousePosition());
      this.selectionLine.Draw();
    }
    this.selectionLine.set_textureOffset((float) (-(double) Time.get_time() * 2.0 % 1.0));
  }
}
