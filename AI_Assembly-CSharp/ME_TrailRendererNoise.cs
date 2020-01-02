// Decompiled with JetBrains decompiler
// Type: ME_TrailRendererNoise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ME_TrailRendererNoise : MonoBehaviour
{
  [Range(0.01f, 10f)]
  public float MinVertexDistance;
  public float VertexTime;
  public float TotalLifeTime;
  public bool SmoothCurves;
  public bool IsRibbon;
  public bool IsActive;
  [Range(0.001f, 10f)]
  public float Frequency;
  [Range(0.001f, 10f)]
  public float TimeScale;
  [Range(0.001f, 10f)]
  public float Amplitude;
  public float Gravity;
  public float TurbulenceStrength;
  public bool AutodestructWhenNotActive;
  private LineRenderer lineRenderer;
  private Transform t;
  private Vector3 prevPos;
  private List<Vector3> points;
  private List<float> lifeTimes;
  private List<Vector3> velocities;
  private float randomOffset;
  private List<Vector3> controlPoints;
  private int curveCount;
  private const float MinimumSqrDistance = 0.01f;
  private const float DivisionThreshold = -0.99f;
  private const float SmoothCurvesScale = 0.5f;

  public ME_TrailRendererNoise()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.lineRenderer = (LineRenderer) ((Component) this).GetComponent<LineRenderer>();
    this.lineRenderer.set_useWorldSpace(true);
    this.t = ((Component) this).get_transform();
    this.prevPos = this.t.get_position();
    this.points.Insert(0, this.t.get_position());
    this.lifeTimes.Insert(0, this.VertexTime);
    this.velocities.Insert(0, Vector3.get_zero());
    this.randomOffset = (float) Random.Range(0, 10000000) / 1000000f;
  }

  private void OnEnable()
  {
    this.points.Clear();
    this.lifeTimes.Clear();
    this.velocities.Clear();
  }

  private void Update()
  {
    if (this.IsActive)
      this.AddNewPoints();
    this.UpdatetPoints();
    if (this.SmoothCurves && this.points.Count > 2)
      this.UpdateLineRendererBezier();
    else
      this.UpdateLineRenderer();
    if (!this.AutodestructWhenNotActive || this.IsActive || this.points.Count > 1)
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject(), this.TotalLifeTime);
  }

  private void AddNewPoints()
  {
    Vector3 vector3_1 = Vector3.op_Subtraction(this.t.get_position(), this.prevPos);
    if ((double) ((Vector3) ref vector3_1).get_magnitude() <= (double) this.MinVertexDistance && (!this.IsRibbon || this.points.Count != 0))
    {
      if (!this.IsRibbon || this.points.Count <= 0)
        return;
      Vector3 vector3_2 = Vector3.op_Subtraction(this.t.get_position(), this.points[0]);
      if ((double) ((Vector3) ref vector3_2).get_magnitude() <= (double) this.MinVertexDistance)
        return;
    }
    this.prevPos = this.t.get_position();
    this.points.Insert(0, this.t.get_position());
    this.lifeTimes.Insert(0, this.VertexTime);
    this.velocities.Insert(0, Vector3.get_zero());
  }

  private void UpdatetPoints()
  {
    for (int index1 = 0; index1 < this.lifeTimes.Count; ++index1)
    {
      List<float> lifeTimes;
      int index2;
      (lifeTimes = this.lifeTimes)[index2 = index1] = lifeTimes[index2] - Time.get_deltaTime();
      if ((double) this.lifeTimes[index1] <= 0.0)
      {
        int count = this.lifeTimes.Count - index1;
        this.lifeTimes.RemoveRange(index1, count);
        this.points.RemoveRange(index1, count);
        this.velocities.RemoveRange(index1, count);
        break;
      }
      this.CalculateTurbuelence(this.points[index1], this.TimeScale, this.Frequency, this.Amplitude, this.Gravity, index1);
    }
  }

  private void UpdateLineRendererBezier()
  {
    if (!this.SmoothCurves || this.points.Count <= 2)
      return;
    this.InterpolateBezier(this.points, 0.5f);
    List<Vector3> drawingPoints = this.GetDrawingPoints();
    this.lineRenderer.set_positionCount(drawingPoints.Count - 1);
    this.lineRenderer.SetPositions(drawingPoints.ToArray());
  }

  private void UpdateLineRenderer()
  {
    this.lineRenderer.set_positionCount(Mathf.Clamp(this.points.Count - 1, 0, int.MaxValue));
    this.lineRenderer.SetPositions(this.points.ToArray());
  }

  private void CalculateTurbuelence(
    Vector3 position,
    float speed,
    float scale,
    float height,
    float gravity,
    int index)
  {
    float num1 = Time.get_timeSinceLevelLoad() * speed + this.randomOffset;
    float num2 = (float) position.x * scale + num1;
    float num3 = (float) (position.y * (double) scale + (double) num1 + 10.0);
    float num4 = (float) (position.z * (double) scale + (double) num1 + 25.0);
    position.x = (__Null) (((double) Mathf.PerlinNoise(num3, num4) - 0.5) * (double) height * (double) Time.get_deltaTime());
    position.y = (__Null) (((double) Mathf.PerlinNoise(num2, num4) - 0.5) * (double) height * (double) Time.get_deltaTime() - (double) gravity * (double) Time.get_deltaTime());
    position.z = (__Null) (((double) Mathf.PerlinNoise(num2, num3) - 0.5) * (double) height * (double) Time.get_deltaTime());
    List<Vector3> points;
    int index1;
    (points = this.points)[index1 = index] = Vector3.op_Addition(points[index1], Vector3.op_Multiply(position, this.TurbulenceStrength));
  }

  public void InterpolateBezier(List<Vector3> segmentPoints, float scale)
  {
    this.controlPoints.Clear();
    if (segmentPoints.Count < 2)
      return;
    for (int index = 0; index < segmentPoints.Count; ++index)
    {
      if (index == 0)
      {
        Vector3 segmentPoint = segmentPoints[index];
        Vector3 vector3_1 = Vector3.op_Subtraction(segmentPoints[index + 1], segmentPoint);
        Vector3 vector3_2 = Vector3.op_Addition(segmentPoint, Vector3.op_Multiply(scale, vector3_1));
        this.controlPoints.Add(segmentPoint);
        this.controlPoints.Add(vector3_2);
      }
      else if (index == segmentPoints.Count - 1)
      {
        Vector3 segmentPoint1 = segmentPoints[index - 1];
        Vector3 segmentPoint2 = segmentPoints[index];
        Vector3 vector3 = Vector3.op_Subtraction(segmentPoint2, segmentPoint1);
        this.controlPoints.Add(Vector3.op_Subtraction(segmentPoint2, Vector3.op_Multiply(scale, vector3)));
        this.controlPoints.Add(segmentPoint2);
      }
      else
      {
        Vector3 segmentPoint1 = segmentPoints[index - 1];
        Vector3 segmentPoint2 = segmentPoints[index];
        Vector3 segmentPoint3 = segmentPoints[index + 1];
        Vector3 vector3_1 = Vector3.op_Subtraction(segmentPoint3, segmentPoint1);
        Vector3 normalized = ((Vector3) ref vector3_1).get_normalized();
        Vector3 vector3_2 = segmentPoint2;
        Vector3 vector3_3 = Vector3.op_Multiply(scale, normalized);
        Vector3 vector3_4 = Vector3.op_Subtraction(segmentPoint2, segmentPoint1);
        double magnitude1 = (double) ((Vector3) ref vector3_4).get_magnitude();
        Vector3 vector3_5 = Vector3.op_Multiply(vector3_3, (float) magnitude1);
        Vector3 vector3_6 = Vector3.op_Subtraction(vector3_2, vector3_5);
        Vector3 vector3_7 = segmentPoint2;
        Vector3 vector3_8 = Vector3.op_Multiply(scale, normalized);
        Vector3 vector3_9 = Vector3.op_Subtraction(segmentPoint3, segmentPoint2);
        double magnitude2 = (double) ((Vector3) ref vector3_9).get_magnitude();
        Vector3 vector3_10 = Vector3.op_Multiply(vector3_8, (float) magnitude2);
        Vector3 vector3_11 = Vector3.op_Addition(vector3_7, vector3_10);
        this.controlPoints.Add(vector3_6);
        this.controlPoints.Add(segmentPoint2);
        this.controlPoints.Add(vector3_11);
      }
    }
    this.curveCount = (this.controlPoints.Count - 1) / 3;
  }

  public List<Vector3> GetDrawingPoints()
  {
    List<Vector3> vector3List = new List<Vector3>();
    for (int curveIndex = 0; curveIndex < this.curveCount; ++curveIndex)
    {
      List<Vector3> drawingPoints = this.FindDrawingPoints(curveIndex);
      if (curveIndex != 0)
        drawingPoints.RemoveAt(0);
      vector3List.AddRange((IEnumerable<Vector3>) drawingPoints);
    }
    return vector3List;
  }

  private List<Vector3> FindDrawingPoints(int curveIndex)
  {
    List<Vector3> pointList = new List<Vector3>();
    Vector3 bezierPoint1 = this.CalculateBezierPoint(curveIndex, 0.0f);
    Vector3 bezierPoint2 = this.CalculateBezierPoint(curveIndex, 1f);
    pointList.Add(bezierPoint1);
    pointList.Add(bezierPoint2);
    this.FindDrawingPoints(curveIndex, 0.0f, 1f, pointList, 1);
    return pointList;
  }

  private int FindDrawingPoints(
    int curveIndex,
    float t0,
    float t1,
    List<Vector3> pointList,
    int insertionIndex)
  {
    Vector3 bezierPoint1 = this.CalculateBezierPoint(curveIndex, t0);
    Vector3 bezierPoint2 = this.CalculateBezierPoint(curveIndex, t1);
    Vector3 vector3_1 = Vector3.op_Subtraction(bezierPoint1, bezierPoint2);
    if ((double) ((Vector3) ref vector3_1).get_sqrMagnitude() < 0.00999999977648258)
      return 0;
    float num1 = (float) (((double) t0 + (double) t1) / 2.0);
    Vector3 bezierPoint3 = this.CalculateBezierPoint(curveIndex, num1);
    Vector3 vector3_2 = Vector3.op_Subtraction(bezierPoint1, bezierPoint3);
    Vector3 normalized1 = ((Vector3) ref vector3_2).get_normalized();
    Vector3 vector3_3 = Vector3.op_Subtraction(bezierPoint2, bezierPoint3);
    Vector3 normalized2 = ((Vector3) ref vector3_3).get_normalized();
    if ((double) Vector3.Dot(normalized1, normalized2) <= -0.990000009536743 && (double) Mathf.Abs(num1 - 0.5f) >= 9.99999974737875E-05)
      return 0;
    int num2 = 0 + this.FindDrawingPoints(curveIndex, t0, num1, pointList, insertionIndex);
    pointList.Insert(insertionIndex + num2, bezierPoint3);
    int num3 = num2 + 1;
    return num3 + this.FindDrawingPoints(curveIndex, num1, t1, pointList, insertionIndex + num3);
  }

  public Vector3 CalculateBezierPoint(int curveIndex, float t)
  {
    int index = curveIndex * 3;
    Vector3 controlPoint1 = this.controlPoints[index];
    Vector3 controlPoint2 = this.controlPoints[index + 1];
    Vector3 controlPoint3 = this.controlPoints[index + 2];
    Vector3 controlPoint4 = this.controlPoints[index + 3];
    return this.CalculateBezierPoint(t, controlPoint1, controlPoint2, controlPoint3, controlPoint4);
  }

  private Vector3 CalculateBezierPoint(
    float t,
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3)
  {
    float num1 = 1f - t;
    float num2 = t * t;
    float num3 = num1 * num1;
    float num4 = num3 * num1;
    float num5 = num2 * t;
    return Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(num4, p0), Vector3.op_Multiply(3f * num3 * t, p1)), Vector3.op_Multiply(3f * num1 * num2, p2)), Vector3.op_Multiply(num5, p3));
  }
}
