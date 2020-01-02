// Decompiled with JetBrains decompiler
// Type: DrawCurve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawCurve : MonoBehaviour
{
  public Texture lineTexture;
  public Color lineColor;
  public Texture dottedLineTexture;
  public Color dottedLineColor;
  public int segments;
  public GameObject anchorPoint;
  public GameObject controlPoint;
  private int numberOfCurves;
  private VectorLine line;
  private VectorLine controlLine;
  private int pointIndex;
  private GameObject anchorObject;
  private int oldWidth;
  private bool useDottedLine;
  private bool oldDottedLineSetting;
  private int oldSegments;
  private bool listPoints;
  public static DrawCurve use;
  public static Camera cam;

  public DrawCurve()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    DrawCurve.use = this;
    DrawCurve.cam = Camera.get_main();
    this.oldWidth = Screen.get_width();
    this.oldSegments = this.segments;
    List<Vector2> vector2List = new List<Vector2>();
    vector2List.Add(new Vector2((float) Screen.get_width() * 0.25f, (float) Screen.get_height() * 0.25f));
    vector2List.Add(new Vector2((float) Screen.get_width() * 0.125f, (float) Screen.get_height() * 0.5f));
    vector2List.Add(new Vector2((float) Screen.get_width() - (float) Screen.get_width() * 0.25f, (float) Screen.get_height() - (float) Screen.get_height() * 0.25f));
    vector2List.Add(new Vector2((float) Screen.get_width() - (float) Screen.get_width() * 0.125f, (float) Screen.get_height() * 0.5f));
    this.controlLine = new VectorLine("Control Line", vector2List, 2f);
    this.controlLine.set_color(Color32.op_Implicit(new Color(0.0f, 0.75f, 0.1f, 0.6f)));
    this.controlLine.Draw();
    this.line = new VectorLine("Curve", new List<Vector2>(this.segments + 1), this.lineTexture, 5f, (LineType) 0, (Joins) 1);
    this.line.MakeCurve(Vector2.op_Implicit(vector2List[0]), Vector2.op_Implicit(vector2List[1]), Vector2.op_Implicit(vector2List[2]), Vector2.op_Implicit(vector2List[3]), this.segments);
    this.line.Draw();
    this.AddControlObjects();
    this.AddControlObjects();
  }

  private void SetLine()
  {
    if (this.useDottedLine)
    {
      this.line.set_texture(this.dottedLineTexture);
      this.line.set_color(Color32.op_Implicit(this.dottedLineColor));
      this.line.set_lineWidth(8f);
      this.line.set_textureScale(1f);
    }
    else
    {
      this.line.set_texture(this.lineTexture);
      this.line.set_color(Color32.op_Implicit(this.lineColor));
      this.line.set_lineWidth(5f);
      this.line.set_textureScale(0.0f);
    }
  }

  private void AddControlObjects()
  {
    this.anchorObject = (GameObject) Object.Instantiate<GameObject>((M0) this.anchorPoint, DrawCurve.cam.ScreenToViewportPoint(Vector2.op_Implicit(this.controlLine.get_points2()[this.pointIndex])), Quaternion.get_identity());
    ((CurvePointControl) this.anchorObject.GetComponent<CurvePointControl>()).objectNumber = this.pointIndex++;
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.controlPoint, DrawCurve.cam.ScreenToViewportPoint(Vector2.op_Implicit(this.controlLine.get_points2()[this.pointIndex])), Quaternion.get_identity());
    ((CurvePointControl) gameObject.GetComponent<CurvePointControl>()).objectNumber = this.pointIndex++;
    ((CurvePointControl) this.anchorObject.GetComponent<CurvePointControl>()).controlObject = gameObject;
  }

  public void UpdateLine(int objectNumber, Vector2 pos, GameObject go)
  {
    Vector2 vector2 = this.controlLine.get_points2()[objectNumber];
    this.controlLine.get_points2()[objectNumber] = pos;
    int num = objectNumber / 4;
    int index1 = num * 4;
    this.line.MakeCurve(Vector2.op_Implicit(this.controlLine.get_points2()[index1]), Vector2.op_Implicit(this.controlLine.get_points2()[index1 + 1]), Vector2.op_Implicit(this.controlLine.get_points2()[index1 + 2]), Vector2.op_Implicit(this.controlLine.get_points2()[index1 + 3]), this.segments, num * (this.segments + 1));
    if (objectNumber % 2 == 0)
    {
      List<Vector2> points2_1;
      int index2;
      (points2_1 = this.controlLine.get_points2())[index2 = objectNumber + 1] = Vector2.op_Addition(points2_1[index2], Vector2.op_Subtraction(pos, vector2));
      ((CurvePointControl) go.GetComponent<CurvePointControl>()).controlObject.get_transform().set_position(DrawCurve.cam.ScreenToViewportPoint(Vector2.op_Implicit(this.controlLine.get_points2()[objectNumber + 1])));
      if (objectNumber > 0 && objectNumber < this.controlLine.get_points2().Count - 2)
      {
        this.controlLine.get_points2()[objectNumber + 2] = pos;
        List<Vector2> points2_2;
        int index3;
        (points2_2 = this.controlLine.get_points2())[index3 = objectNumber + 3] = Vector2.op_Addition(points2_2[index3], Vector2.op_Subtraction(pos, vector2));
        ((CurvePointControl) go.GetComponent<CurvePointControl>()).controlObject2.get_transform().set_position(DrawCurve.cam.ScreenToViewportPoint(Vector2.op_Implicit(this.controlLine.get_points2()[objectNumber + 3])));
        this.line.MakeCurve(Vector2.op_Implicit(this.controlLine.get_points2()[index1 + 4]), Vector2.op_Implicit(this.controlLine.get_points2()[index1 + 5]), Vector2.op_Implicit(this.controlLine.get_points2()[index1 + 6]), Vector2.op_Implicit(this.controlLine.get_points2()[index1 + 7]), this.segments, (num + 1) * (this.segments + 1));
      }
    }
    this.line.Draw();
    this.controlLine.Draw();
  }

  private void OnGUI()
  {
    if (GUI.Button(new Rect(20f, 20f, 100f, 30f), "Add Point"))
      this.AddPoint();
    GUI.Label(new Rect(20f, 59f, 200f, 30f), "Curve resolution: " + (object) this.segments);
    this.segments = (int) GUI.HorizontalSlider(new Rect(20f, 80f, 150f, 30f), (float) this.segments, 3f, 60f);
    if (this.oldSegments != this.segments)
    {
      this.oldSegments = this.segments;
      this.ChangeSegments();
    }
    this.useDottedLine = GUI.Toggle(new Rect(20f, 105f, 80f, 20f), this.useDottedLine, " Dotted line");
    if (this.oldDottedLineSetting != this.useDottedLine)
    {
      this.oldDottedLineSetting = this.useDottedLine;
      this.SetLine();
      this.line.Draw();
    }
    GUILayout.BeginArea(new Rect(20f, 150f, 150f, 800f));
    if (GUILayout.Button(!this.listPoints ? "List points" : "Hide points", new GUILayoutOption[1]
    {
      GUILayout.Width(100f)
    }))
      this.listPoints = !this.listPoints;
    if (this.listPoints)
    {
      int num = 0;
      for (int index = 0; index < this.controlLine.get_points2().Count; index += 2)
      {
        GUILayout.Label("Anchor " + (object) num + ": (" + (object) (int) this.controlLine.get_points2()[index].x + ", " + (object) (int) this.controlLine.get_points2()[index].y + ")", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.Label("Control " + (object) num++ + ": (" + (object) (int) this.controlLine.get_points2()[index + 1].x + ", " + (object) (int) this.controlLine.get_points2()[index + 1].y + ")", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      }
    }
    GUILayout.EndArea();
  }

  private void AddPoint()
  {
    if (this.line.get_points2().Count + this.controlLine.get_points2().Count + this.segments + 4 > 16383)
      return;
    this.controlLine.get_points2().Add(this.controlLine.get_points2()[this.pointIndex - 2]);
    this.controlLine.get_points2().Add(this.controlLine.get_points2()[this.pointIndex - 1]);
    Vector2 vector2_1 = Vector2.op_Multiply(Vector2.op_Subtraction(this.controlLine.get_points2()[this.pointIndex - 2], this.controlLine.get_points2()[this.pointIndex - 4]), 0.25f);
    this.controlLine.get_points2().Add(Vector2.op_Addition(this.controlLine.get_points2()[this.pointIndex - 2], vector2_1));
    this.controlLine.get_points2().Add(Vector2.op_Addition(this.controlLine.get_points2()[this.pointIndex - 1], vector2_1));
    if (this.controlLine.get_points2()[this.pointIndex + 2].x > (double) Screen.get_width() || this.controlLine.get_points2()[this.pointIndex + 2].y > (double) Screen.get_height() || (this.controlLine.get_points2()[this.pointIndex + 2].x < 0.0 || this.controlLine.get_points2()[this.pointIndex + 2].y < 0.0))
    {
      this.controlLine.get_points2()[this.pointIndex + 2] = Vector2.op_Subtraction(this.controlLine.get_points2()[this.pointIndex - 2], vector2_1);
      this.controlLine.get_points2()[this.pointIndex + 3] = Vector2.op_Subtraction(this.controlLine.get_points2()[this.pointIndex - 1], vector2_1);
    }
    Vector2 vector2_2 = Vector2.op_Addition(this.controlLine.get_points2()[this.pointIndex - 1], Vector2.op_Multiply(Vector2.op_Subtraction(this.controlLine.get_points2()[this.pointIndex], this.controlLine.get_points2()[this.pointIndex - 1]), 2f));
    ++this.pointIndex;
    this.controlLine.get_points2()[this.pointIndex] = vector2_2;
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.controlPoint, DrawCurve.cam.ScreenToViewportPoint(Vector2.op_Implicit(vector2_2)), Quaternion.get_identity());
    ((CurvePointControl) gameObject.GetComponent<CurvePointControl>()).objectNumber = this.pointIndex++;
    ((CurvePointControl) this.anchorObject.GetComponent<CurvePointControl>()).controlObject2 = gameObject;
    this.AddControlObjects();
    this.controlLine.Draw();
    this.line.Resize((this.segments + 1) * ++this.numberOfCurves);
    this.line.MakeCurve(Vector2.op_Implicit(this.controlLine.get_points2()[this.pointIndex - 4]), Vector2.op_Implicit(this.controlLine.get_points2()[this.pointIndex - 3]), Vector2.op_Implicit(this.controlLine.get_points2()[this.pointIndex - 2]), Vector2.op_Implicit(this.controlLine.get_points2()[this.pointIndex - 1]), this.segments, (this.segments + 1) * (this.numberOfCurves - 1));
    this.line.Draw();
  }

  private void ChangeSegments()
  {
    if (this.segments * 4 * this.numberOfCurves > 65534)
      return;
    this.line.Resize((this.segments + 1) * this.numberOfCurves);
    for (int index = 0; index < this.numberOfCurves; ++index)
      this.line.MakeCurve(Vector2.op_Implicit(this.controlLine.get_points2()[index * 4]), Vector2.op_Implicit(this.controlLine.get_points2()[index * 4 + 1]), Vector2.op_Implicit(this.controlLine.get_points2()[index * 4 + 2]), Vector2.op_Implicit(this.controlLine.get_points2()[index * 4 + 3]), this.segments, (this.segments + 1) * index);
    this.line.Draw();
  }

  private void Update()
  {
    if (Screen.get_width() == this.oldWidth)
      return;
    this.oldWidth = Screen.get_width();
    this.ChangeResolution();
  }

  private void ChangeResolution()
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("GameController"))
      gameObject.get_transform().set_position(DrawCurve.cam.ScreenToViewportPoint(Vector2.op_Implicit(this.controlLine.get_points2()[((CurvePointControl) gameObject.GetComponent<CurvePointControl>()).objectNumber])));
  }
}
