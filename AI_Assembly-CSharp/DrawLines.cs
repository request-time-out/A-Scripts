// Decompiled with JetBrains decompiler
// Type: DrawLines
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLines : MonoBehaviour
{
  public float rotateSpeed;
  public float maxPoints;
  private VectorLine line;
  private bool endReached;
  private bool continuous;
  private bool oldContinuous;
  private bool fillJoins;
  private bool oldFillJoins;
  private bool weldJoins;
  private bool oldWeldJoins;
  private bool thickLine;
  private bool canClick;

  public DrawLines()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.SetLine();
  }

  private void SetLine()
  {
    VectorLine.Destroy(ref this.line);
    if (!this.continuous)
      this.fillJoins = false;
    LineType lineType = !this.continuous ? (LineType) 1 : (LineType) 0;
    Joins joins = !this.fillJoins ? (Joins) 2 : (Joins) 0;
    this.line = new VectorLine("Line", new List<Vector2>(), !this.thickLine ? 2f : 24f, lineType, joins);
    this.line.set_drawTransform(((Component) this).get_transform());
    this.endReached = false;
  }

  private void Update()
  {
    Vector3 vector3 = ((Component) this).get_transform().InverseTransformPoint(Input.get_mousePosition());
    if (Input.GetMouseButtonDown(0) && this.canClick && !this.endReached)
    {
      this.line.get_points2().Add(Vector2.op_Implicit(vector3));
      if (this.line.get_points2().Count == 1)
        this.line.get_points2().Add(Vector2.get_zero());
      if ((double) this.line.get_points2().Count == (double) this.maxPoints)
        this.endReached = true;
    }
    if (this.line.get_points2().Count >= 2)
    {
      this.line.get_points2()[this.line.get_points2().Count - 1] = Vector2.op_Implicit(vector3);
      this.line.Draw();
    }
    ((Component) this).get_transform().RotateAround(Vector2.op_Implicit(new Vector2((float) (Screen.get_width() / 2), (float) (Screen.get_height() / 2))), Vector3.get_forward(), Time.get_deltaTime() * this.rotateSpeed * Input.GetAxis("Horizontal"));
  }

  private void OnGUI()
  {
    Rect rect;
    ((Rect) ref rect).\u002Ector(20f, 20f, 265f, 220f);
    this.canClick = !((Rect) ref rect).Contains(Event.get_current().get_mousePosition());
    GUILayout.BeginArea(rect);
    GUI.set_contentColor(Color.get_black());
    GUILayout.Label("Click to add points to the line\nRotate with the right/left arrow keys", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(5f);
    this.continuous = GUILayout.Toggle(this.continuous, "Continuous line", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    this.thickLine = GUILayout.Toggle(this.thickLine, "Thick line", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    this.line.set_lineWidth(!this.thickLine ? 2f : 24f);
    this.fillJoins = GUILayout.Toggle(this.fillJoins, "Fill joins (only works with continuous line)", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    if (this.line.get_lineType() != null)
      this.fillJoins = false;
    this.weldJoins = GUILayout.Toggle(this.weldJoins, "Weld joins", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    if (this.oldContinuous != this.continuous)
    {
      this.oldContinuous = this.continuous;
      this.line.set_lineType(!this.continuous ? (LineType) 1 : (LineType) 0);
    }
    if (this.oldFillJoins != this.fillJoins)
    {
      if (this.fillJoins)
        this.weldJoins = false;
      this.oldFillJoins = this.fillJoins;
    }
    else if (this.oldWeldJoins != this.weldJoins)
    {
      if (this.weldJoins)
        this.fillJoins = false;
      this.oldWeldJoins = this.weldJoins;
    }
    if (this.fillJoins)
      this.line.set_joins((Joins) 0);
    else if (this.weldJoins)
      this.line.set_joins((Joins) 1);
    else
      this.line.set_joins((Joins) 2);
    GUILayout.Space(10f);
    GUI.set_contentColor(Color.get_white());
    if (GUILayout.Button("Randomize Color", new GUILayoutOption[1]
    {
      GUILayout.Width(150f)
    }))
      this.RandomizeColor();
    if (GUILayout.Button("Randomize All Colors", new GUILayoutOption[1]
    {
      GUILayout.Width(150f)
    }))
      this.RandomizeAllColors();
    if (GUILayout.Button("Reset line", new GUILayoutOption[1]
    {
      GUILayout.Width(150f)
    }))
      this.SetLine();
    if (this.endReached)
    {
      GUI.set_contentColor(Color.get_black());
      GUILayout.Label("No more points available. You must be bored!", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    }
    GUILayout.EndArea();
  }

  private void RandomizeColor()
  {
    this.line.set_color(Color32.op_Implicit(new Color(Random.get_value(), Random.get_value(), Random.get_value())));
  }

  private void RandomizeAllColors()
  {
    int segmentNumber = this.line.GetSegmentNumber();
    for (int index = 0; index < segmentNumber; ++index)
      this.line.SetColor(Color32.op_Implicit(new Color(Random.get_value(), Random.get_value(), Random.get_value())), index);
  }
}
