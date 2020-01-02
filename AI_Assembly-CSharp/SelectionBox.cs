// Decompiled with JetBrains decompiler
// Type: SelectionBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Vectrosity;

public class SelectionBox : MonoBehaviour
{
  private VectorLine selectionLine;
  private Vector2 originalPos;
  private List<Color32> lineColors;

  public SelectionBox()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.lineColors = new List<Color32>((IEnumerable<Color32>) new Color32[4]);
    this.selectionLine = new VectorLine("Selection", new List<Vector2>(5), 3f, (LineType) 0);
    this.selectionLine.set_capLength(1.5f);
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10f, 10f, 300f, 25f), "Click & drag to make a selection box");
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      this.StopCoroutine("CycleColor");
      this.selectionLine.SetColor(Color32.op_Implicit(Color.get_white()));
      this.originalPos = Vector2.op_Implicit(Input.get_mousePosition());
    }
    if (Input.GetMouseButton(0))
    {
      this.selectionLine.MakeRect(Vector2.op_Implicit(this.originalPos), Input.get_mousePosition());
      this.selectionLine.Draw();
    }
    if (!Input.GetMouseButtonUp(0))
      return;
    this.StartCoroutine("CycleColor");
  }

  [DebuggerHidden]
  private IEnumerator CycleColor()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new SelectionBox.\u003CCycleColor\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
