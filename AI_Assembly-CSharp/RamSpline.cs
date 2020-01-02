// Decompiled with JetBrains decompiler
// Type: RamSpline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof (MeshFilter))]
public class RamSpline : MonoBehaviour
{
  public SplineProfile currentProfile;
  public SplineProfile oldProfile;
  public List<RamSpline> beginnigChildSplines;
  public List<RamSpline> endingChildSplines;
  public RamSpline beginningSpline;
  public RamSpline endingSpline;
  public int beginningConnectionID;
  public int endingConnectionID;
  public float beginningMinWidth;
  public float beginningMaxWidth;
  public float endingMinWidth;
  public float endingMaxWidth;
  public int toolbarInt;
  public bool invertUVDirection;
  public bool uvRotation;
  public MeshFilter meshfilter;
  public List<Vector4> controlPoints;
  public List<Quaternion> controlPointsRotations;
  public List<Quaternion> controlPointsOrientation;
  public List<Vector3> controlPointsUp;
  public List<Vector3> controlPointsDown;
  public List<float> controlPointsSnap;
  public AnimationCurve meshCurve;
  public List<AnimationCurve> controlPointsMeshCurves;
  public bool normalFromRaycast;
  public bool snapToTerrain;
  public LayerMask snapMask;
  public List<Vector3> points;
  public List<Vector3> pointsUp;
  public List<Vector3> pointsDown;
  public List<Vector3> points2;
  public List<Vector3> verticesBeginning;
  public List<Vector3> verticesEnding;
  public List<Vector3> normalsBeginning;
  public List<Vector3> normalsEnding;
  public List<float> widths;
  public List<float> snaps;
  public List<float> lerpValues;
  public List<Quaternion> orientations;
  public List<Vector3> tangents;
  public List<Vector3> normalsList;
  public Color[] colors;
  public List<Vector2> colorsFlowMap;
  public float minVal;
  public float maxVal;
  public float width;
  public int vertsInShape;
  public float traingleDensity;
  public float uvScale;
  public Material oldMaterial;
  public bool showVertexColors;
  public bool showFlowMap;
  public bool overrideFlowMap;
  public bool drawOnMesh;
  public bool drawOnMeshFlowMap;
  public bool uvScaleOverride;
  public bool debug;
  public Color drawColor;
  public bool drawOnMultiple;
  public float flowSpeed;
  public float flowDirection;
  public AnimationCurve flowFlat;
  public AnimationCurve flowWaterfall;
  public float opacity;
  public float drawSize;
  public float length;
  public float fulllength;
  public float minMaxWidth;
  public float uvWidth;
  public float uvBeginning;
  public bool generateMeshParts;
  public int meshPartsCount;
  public List<Transform> meshesPartTransforms;
  public float simulatedRiverLength;
  public int simulatedRiverPoints;
  public float simulationMinStepSize;
  public AnimationCurve terrainCurve;
  public int detailTerrain;
  public int detailTerrainForward;
  public float terrainDepthHeight;
  public float terrainDepthMultiplier;
  public float terrainAdditionalWidth;
  public float terrainSmoothMultiplier;

  public RamSpline()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    this.GenerateSpline((List<RamSpline>) null);
  }

  public static RamSpline CreateSpline(Material splineMaterial = null, List<Vector4> positions = null)
  {
    GameObject gameObject = new GameObject(nameof (RamSpline));
    RamSpline ramSpline = (RamSpline) gameObject.AddComponent<RamSpline>();
    MeshRenderer meshRenderer = (MeshRenderer) gameObject.AddComponent<MeshRenderer>();
    ((Renderer) meshRenderer).set_receiveShadows(false);
    ((Renderer) meshRenderer).set_shadowCastingMode((ShadowCastingMode) 0);
    if (Object.op_Inequality((Object) splineMaterial, (Object) null))
      ((Renderer) meshRenderer).set_sharedMaterial(splineMaterial);
    if (positions != null)
    {
      for (int index = 0; index < positions.Count; ++index)
        ramSpline.AddPoint(positions[index]);
    }
    return ramSpline;
  }

  public void AddPoint(Vector4 position)
  {
    if (position.w == 0.0)
      position.w = this.controlPoints.Count <= 0 ? (__Null) (double) this.width : this.controlPoints[this.controlPoints.Count - 1].w;
    this.controlPointsRotations.Add(Quaternion.get_identity());
    this.controlPoints.Add(position);
    this.controlPointsSnap.Add(0.0f);
    this.controlPointsMeshCurves.Add(new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(1f, 0.0f)
    }));
  }

  public void AddPointAfter(int i)
  {
    Vector4 vector4 = this.controlPoints[i];
    if (i < this.controlPoints.Count - 1 && this.controlPoints.Count > i + 1)
    {
      Vector4 controlPoint = this.controlPoints[i + 1];
      if ((double) Vector3.Distance(Vector4.op_Implicit(controlPoint), Vector4.op_Implicit(vector4)) > 0.0)
      {
        vector4 = Vector4.op_Multiply(Vector4.op_Addition(vector4, controlPoint), 0.5f);
      }
      else
      {
        ref Vector4 local = ref vector4;
        local.x = (__Null) (local.x + 1.0);
      }
    }
    else if (this.controlPoints.Count > 1 && i == this.controlPoints.Count - 1)
    {
      Vector4 controlPoint = this.controlPoints[i - 1];
      if ((double) Vector3.Distance(Vector4.op_Implicit(controlPoint), Vector4.op_Implicit(vector4)) > 0.0)
      {
        vector4 = Vector4.op_Addition(vector4, Vector4.op_Subtraction(vector4, controlPoint));
      }
      else
      {
        ref Vector4 local = ref vector4;
        local.x = (__Null) (local.x + 1.0);
      }
    }
    else
    {
      ref Vector4 local = ref vector4;
      local.x = (__Null) (local.x + 1.0);
    }
    this.controlPoints.Insert(i + 1, vector4);
    this.controlPointsRotations.Insert(i + 1, Quaternion.get_identity());
    this.controlPointsSnap.Insert(i + 1, 0.0f);
    this.controlPointsMeshCurves.Insert(i + 1, new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(1f, 0.0f)
    }));
  }

  public void ChangePointPosition(int i, Vector3 position)
  {
    this.ChangePointPosition(i, new Vector4((float) position.x, (float) position.y, (float) position.z, 0.0f));
  }

  public void ChangePointPosition(int i, Vector4 position)
  {
    Vector4 controlPoint = this.controlPoints[i];
    if (position.w == 0.0)
      position.w = controlPoint.w;
    this.controlPoints[i] = position;
  }

  public void RemovePoint(int i)
  {
    if (i >= this.controlPoints.Count)
      return;
    this.controlPoints.RemoveAt(i);
    this.controlPointsRotations.RemoveAt(i);
    this.controlPointsMeshCurves.RemoveAt(i);
    this.controlPointsSnap.RemoveAt(i);
  }

  public void RemovePoints(int fromID = -1)
  {
    for (int i = this.controlPoints.Count - 1; i > fromID; --i)
      this.RemovePoint(i);
  }

  public void GenerateBeginningParentBased()
  {
    this.vertsInShape = (int) Mathf.Round((float) ((double) (this.beginningSpline.vertsInShape - 1) * ((double) this.beginningMaxWidth - (double) this.beginningMinWidth) + 1.0));
    if (this.vertsInShape < 1)
      this.vertsInShape = 1;
    this.beginningConnectionID = this.beginningSpline.points.Count - 1;
    float num = (float) this.beginningSpline.controlPoints[this.beginningSpline.controlPoints.Count - 1].w * (this.beginningMaxWidth - this.beginningMinWidth);
    Vector4 vector4 = Vector4.op_Implicit(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.Lerp(this.beginningSpline.pointsDown[this.beginningConnectionID], this.beginningSpline.pointsUp[this.beginningConnectionID], this.beginningMinWidth + (float) (((double) this.beginningMaxWidth - (double) this.beginningMinWidth) * 0.5)), ((Component) this.beginningSpline).get_transform().get_position()), ((Component) this).get_transform().get_position()));
    vector4.w = (__Null) (double) num;
    this.controlPoints[0] = vector4;
    if (this.uvScaleOverride)
      return;
    this.uvScale = this.beginningSpline.uvScale;
  }

  public void GenerateEndingParentBased()
  {
    if (Object.op_Equality((Object) this.beginningSpline, (Object) null))
    {
      this.vertsInShape = (int) Mathf.Round((float) ((double) (this.endingSpline.vertsInShape - 1) * ((double) this.endingMaxWidth - (double) this.endingMinWidth) + 1.0));
      if (this.vertsInShape < 1)
        this.vertsInShape = 1;
    }
    this.endingConnectionID = 0;
    float num = (float) this.endingSpline.controlPoints[0].w * (this.endingMaxWidth - this.endingMinWidth);
    Vector4 vector4 = Vector4.op_Implicit(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.Lerp(this.endingSpline.pointsDown[this.endingConnectionID], this.endingSpline.pointsUp[this.endingConnectionID], this.endingMinWidth + (float) (((double) this.endingMaxWidth - (double) this.endingMinWidth) * 0.5)), ((Component) this.endingSpline).get_transform().get_position()), ((Component) this).get_transform().get_position()));
    vector4.w = (__Null) (double) num;
    this.controlPoints[this.controlPoints.Count - 1] = vector4;
  }

  public void GenerateSpline(List<RamSpline> generatedSplines = null)
  {
    generatedSplines = new List<RamSpline>();
    if (Object.op_Implicit((Object) this.beginningSpline))
      this.GenerateBeginningParentBased();
    if (Object.op_Implicit((Object) this.endingSpline))
      this.GenerateEndingParentBased();
    List<Vector4> controlPoints = new List<Vector4>();
    for (int index = 0; index < this.controlPoints.Count; ++index)
    {
      if (index > 0)
      {
        if ((double) Vector3.Distance(Vector4.op_Implicit(this.controlPoints[index]), Vector4.op_Implicit(this.controlPoints[index - 1])) > 0.0)
          controlPoints.Add(this.controlPoints[index]);
      }
      else
        controlPoints.Add(this.controlPoints[index]);
    }
    Mesh mesh = new Mesh();
    this.meshfilter = (MeshFilter) ((Component) this).GetComponent<MeshFilter>();
    if (controlPoints.Count < 2)
    {
      mesh.Clear();
      this.meshfilter.set_mesh(mesh);
    }
    else
    {
      this.controlPointsOrientation = new List<Quaternion>();
      this.lerpValues.Clear();
      this.snaps.Clear();
      this.points.Clear();
      this.pointsUp.Clear();
      this.pointsDown.Clear();
      this.orientations.Clear();
      this.tangents.Clear();
      this.normalsList.Clear();
      this.widths.Clear();
      this.controlPointsUp.Clear();
      this.controlPointsDown.Clear();
      this.verticesBeginning.Clear();
      this.verticesEnding.Clear();
      this.normalsBeginning.Clear();
      this.normalsEnding.Clear();
      if (Object.op_Inequality((Object) this.beginningSpline, (Object) null) && this.beginningSpline.controlPointsRotations.Count > 0)
        this.controlPointsRotations[0] = Quaternion.get_identity();
      if (Object.op_Inequality((Object) this.endingSpline, (Object) null) && this.endingSpline.controlPointsRotations.Count > 0)
        this.controlPointsRotations[this.controlPointsRotations.Count - 1] = Quaternion.get_identity();
      for (int pos = 0; pos < controlPoints.Count; ++pos)
      {
        if (pos <= controlPoints.Count - 2)
          this.CalculateCatmullRomSideSplines(controlPoints, pos);
      }
      if (Object.op_Inequality((Object) this.beginningSpline, (Object) null) && this.beginningSpline.controlPointsRotations.Count > 0)
        this.controlPointsRotations[0] = Quaternion.op_Multiply(Quaternion.Inverse(this.controlPointsOrientation[0]), this.beginningSpline.controlPointsOrientation[this.beginningSpline.controlPointsOrientation.Count - 1]);
      if (Object.op_Inequality((Object) this.endingSpline, (Object) null) && this.endingSpline.controlPointsRotations.Count > 0)
        this.controlPointsRotations[this.controlPointsRotations.Count - 1] = Quaternion.op_Multiply(Quaternion.Inverse(this.controlPointsOrientation[this.controlPointsOrientation.Count - 1]), this.endingSpline.controlPointsOrientation[0]);
      this.controlPointsOrientation = new List<Quaternion>();
      this.controlPointsUp.Clear();
      this.controlPointsDown.Clear();
      for (int pos = 0; pos < controlPoints.Count; ++pos)
      {
        if (pos <= controlPoints.Count - 2)
          this.CalculateCatmullRomSideSplines(controlPoints, pos);
      }
      for (int pos = 0; pos < controlPoints.Count; ++pos)
      {
        if (pos <= controlPoints.Count - 2)
          this.CalculateCatmullRomSplineParameters(controlPoints, pos, false);
      }
      for (int pos = 0; pos < this.controlPointsUp.Count; ++pos)
      {
        if (pos <= this.controlPointsUp.Count - 2)
          this.CalculateCatmullRomSpline(this.controlPointsUp, pos, ref this.pointsUp);
      }
      for (int pos = 0; pos < this.controlPointsDown.Count; ++pos)
      {
        if (pos <= this.controlPointsDown.Count - 2)
          this.CalculateCatmullRomSpline(this.controlPointsDown, pos, ref this.pointsDown);
      }
      this.GenerateMesh(ref mesh);
      if (generatedSplines == null)
        return;
      generatedSplines.Add(this);
      foreach (RamSpline beginnigChildSpline in this.beginnigChildSplines)
      {
        if (Object.op_Inequality((Object) beginnigChildSpline, (Object) null) && !generatedSplines.Contains(beginnigChildSpline) && (Object.op_Equality((Object) beginnigChildSpline.beginningSpline, (Object) this) || Object.op_Equality((Object) beginnigChildSpline.endingSpline, (Object) this)))
          beginnigChildSpline.GenerateSpline(generatedSplines);
      }
      foreach (RamSpline endingChildSpline in this.endingChildSplines)
      {
        if (Object.op_Inequality((Object) endingChildSpline, (Object) null) && !generatedSplines.Contains(endingChildSpline) && (Object.op_Equality((Object) endingChildSpline.beginningSpline, (Object) this) || Object.op_Equality((Object) endingChildSpline.endingSpline, (Object) this)))
          endingChildSpline.GenerateSpline(generatedSplines);
      }
    }
  }

  private void CalculateCatmullRomSideSplines(List<Vector4> controlPoints, int pos)
  {
    Vector3 p0 = Vector4.op_Implicit(controlPoints[pos]);
    Vector3 p1 = Vector4.op_Implicit(controlPoints[pos]);
    Vector3 p2 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos + 1)]);
    Vector3 p3 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos + 1)]);
    if (pos > 0)
      p0 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos - 1)]);
    if (pos < controlPoints.Count - 2)
      p3 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos + 2)]);
    int num = 0;
    if (pos == controlPoints.Count - 2)
      num = 1;
    for (int index = 0; index <= num; ++index)
    {
      Vector3 catmullRomPosition = this.GetCatmullRomPosition((float) index, p0, p1, p2, p3);
      Vector3 catmullRomTangent = this.GetCatmullRomTangent((float) index, p0, p1, p2, p3);
      Vector3 normalized1 = ((Vector3) ref catmullRomTangent).get_normalized();
      Vector3 normal = this.CalculateNormal(normalized1, Vector3.get_up());
      Vector3 normalized2 = ((Vector3) ref normal).get_normalized();
      Quaternion quaternion = Quaternion.op_Multiply(!Vector3.op_Equality(normalized2, normalized1) || !Vector3.op_Equality(normalized2, Vector3.get_zero()) ? Quaternion.LookRotation(normalized1, normalized2) : Quaternion.get_identity(), Quaternion.Lerp(this.controlPointsRotations[pos], this.controlPointsRotations[this.ClampListPos(pos + 1)], (float) index));
      this.controlPointsOrientation.Add(quaternion);
      Vector3 vector3_1 = Vector3.op_Addition(catmullRomPosition, Quaternion.op_Multiply(quaternion, Vector3.op_Multiply((float) (0.5 * controlPoints[pos + index].w), Vector3.get_right())));
      Vector3 vector3_2 = Vector3.op_Addition(catmullRomPosition, Quaternion.op_Multiply(quaternion, Vector3.op_Multiply((float) (0.5 * controlPoints[pos + index].w), Vector3.get_left())));
      this.controlPointsUp.Add(vector3_1);
      this.controlPointsDown.Add(vector3_2);
    }
  }

  private void CalculateCatmullRomSplineParameters(
    List<Vector4> controlPoints,
    int pos,
    bool initialPoints = false)
  {
    Vector3 p0 = Vector4.op_Implicit(controlPoints[pos]);
    Vector3 p1 = Vector4.op_Implicit(controlPoints[pos]);
    Vector3 p2 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos + 1)]);
    Vector3 p3 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos + 1)]);
    if (pos > 0)
      p0 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos - 1)]);
    if (pos < controlPoints.Count - 2)
      p3 = Vector4.op_Implicit(controlPoints[this.ClampListPos(pos + 2)]);
    int num1 = Mathf.FloorToInt(1f / this.traingleDensity);
    float num2 = 0.0f;
    if (pos > 0)
      num2 = 1f;
    float num3;
    for (num3 = num2; (double) num3 <= (double) num1; ++num3)
    {
      float t = num3 * this.traingleDensity;
      this.CalculatePointParameters(controlPoints, pos, p0, p1, p2, p3, t);
    }
    if ((double) num3 >= (double) num1)
      return;
    float t1 = (float) num1 * this.traingleDensity;
    this.CalculatePointParameters(controlPoints, pos, p0, p1, p2, p3, t1);
  }

  private void CalculateCatmullRomSpline(
    List<Vector3> controlPoints,
    int pos,
    ref List<Vector3> points)
  {
    Vector3 controlPoint1 = controlPoints[pos];
    Vector3 controlPoint2 = controlPoints[pos];
    Vector3 controlPoint3 = controlPoints[this.ClampListPos(pos + 1)];
    Vector3 controlPoint4 = controlPoints[this.ClampListPos(pos + 1)];
    if (pos > 0)
      controlPoint1 = controlPoints[this.ClampListPos(pos - 1)];
    if (pos < controlPoints.Count - 2)
      controlPoint4 = controlPoints[this.ClampListPos(pos + 2)];
    int num1 = Mathf.FloorToInt(1f / this.traingleDensity);
    float num2 = 0.0f;
    if (pos > 0)
      num2 = 1f;
    float num3;
    for (num3 = num2; (double) num3 <= (double) num1; ++num3)
    {
      float t = num3 * this.traingleDensity;
      this.CalculatePointPosition(controlPoints, pos, controlPoint1, controlPoint2, controlPoint3, controlPoint4, t, ref points);
    }
    if ((double) num3 >= (double) num1)
      return;
    float t1 = (float) num1 * this.traingleDensity;
    this.CalculatePointPosition(controlPoints, pos, controlPoint1, controlPoint2, controlPoint3, controlPoint4, t1, ref points);
  }

  private void CalculatePointPosition(
    List<Vector3> controlPoints,
    int pos,
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3,
    float t,
    ref List<Vector3> points)
  {
    Vector3 catmullRomPosition = this.GetCatmullRomPosition(t, p0, p1, p2, p3);
    points.Add(catmullRomPosition);
    Vector3 catmullRomTangent = this.GetCatmullRomTangent(t, p0, p1, p2, p3);
    Vector3 normal = this.CalculateNormal(((Vector3) ref catmullRomTangent).get_normalized(), Vector3.get_up());
    ((Vector3) ref normal).get_normalized();
  }

  private void CalculatePointParameters(
    List<Vector4> controlPoints,
    int pos,
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3,
    float t)
  {
    Vector3 catmullRomPosition = this.GetCatmullRomPosition(t, p0, p1, p2, p3);
    this.widths.Add(Mathf.Lerp((float) controlPoints[pos].w, (float) controlPoints[this.ClampListPos(pos + 1)].w, t));
    if (this.controlPointsSnap.Count > pos + 1)
      this.snaps.Add(Mathf.Lerp(this.controlPointsSnap[pos], this.controlPointsSnap[this.ClampListPos(pos + 1)], t));
    else
      this.snaps.Add(0.0f);
    this.lerpValues.Add((float) pos + t);
    this.points.Add(catmullRomPosition);
    Vector3 catmullRomTangent = this.GetCatmullRomTangent(t, p0, p1, p2, p3);
    Vector3 normalized = ((Vector3) ref catmullRomTangent).get_normalized();
    Vector3 normal = this.CalculateNormal(normalized, Vector3.get_up());
    Vector3 vector3 = ((Vector3) ref normal).get_normalized();
    this.orientations.Add(Quaternion.op_Multiply(!Vector3.op_Equality(vector3, normalized) || !Vector3.op_Equality(vector3, Vector3.get_zero()) ? Quaternion.LookRotation(normalized, vector3) : Quaternion.get_identity(), Quaternion.Lerp(this.controlPointsRotations[pos], this.controlPointsRotations[this.ClampListPos(pos + 1)], t)));
    this.tangents.Add(normalized);
    if (this.normalsList.Count > 0 && (double) Vector3.Angle(this.normalsList[this.normalsList.Count - 1], vector3) > 90.0)
      vector3 = Vector3.op_Multiply(vector3, -1f);
    this.normalsList.Add(vector3);
  }

  private int ClampListPos(int pos)
  {
    if (pos < 0)
      pos = this.controlPoints.Count - 1;
    if (pos > this.controlPoints.Count)
      pos = 1;
    else if (pos > this.controlPoints.Count - 1)
      pos = 0;
    return pos;
  }

  private Vector3 GetCatmullRomPosition(
    float t,
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3)
  {
    Vector3 vector3_1 = Vector3.op_Multiply(2f, p1);
    Vector3 vector3_2 = Vector3.op_Subtraction(p2, p0);
    Vector3 vector3_3 = Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Multiply(2f, p0), Vector3.op_Multiply(5f, p1)), Vector3.op_Multiply(4f, p2)), p3);
    Vector3 vector3_4 = Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_UnaryNegation(p0), Vector3.op_Multiply(3f, p1)), Vector3.op_Multiply(3f, p2)), p3);
    return Vector3.op_Multiply(0.5f, Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(vector3_1, Vector3.op_Multiply(vector3_2, t)), Vector3.op_Multiply(Vector3.op_Multiply(vector3_3, t), t)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(vector3_4, t), t), t)));
  }

  private Vector3 GetCatmullRomTangent(
    float t,
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3)
  {
    return Vector3.op_Multiply(0.5f, Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_UnaryNegation(p0), p2), Vector3.op_Multiply(Vector3.op_Multiply(2f, Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Multiply(2f, p0), Vector3.op_Multiply(5f, p1)), Vector3.op_Multiply(4f, p2)), p3)), t)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(3f, Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_UnaryNegation(p0), Vector3.op_Multiply(3f, p1)), Vector3.op_Multiply(3f, p2)), p3)), t), t)));
  }

  private Vector3 CalculateNormal(Vector3 tangent, Vector3 up)
  {
    Vector3 vector3 = Vector3.Cross(up, tangent);
    return Vector3.Cross(tangent, vector3);
  }

  private void GenerateMesh(ref Mesh mesh)
  {
    using (List<Transform>.Enumerator enumerator = this.meshesPartTransforms.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        Transform current = enumerator.Current;
        if (Object.op_Inequality((Object) current, (Object) null))
          Object.DestroyImmediate((Object) ((Component) current).get_gameObject());
      }
    }
    int num1 = this.points.Count - 1;
    int length = this.vertsInShape * this.points.Count;
    List<int> intList = new List<int>();
    Vector3[] vector3Array1 = new Vector3[length];
    Vector3[] vector3Array2 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    Vector2[] vector2Array2 = new Vector2[length];
    Vector2[] vector2Array3 = new Vector2[length];
    if (this.colors == null || this.colors.Length != length)
    {
      this.colors = new Color[length];
      for (int index = 0; index < this.colors.Length; ++index)
        this.colors[index] = Color.get_black();
    }
    if (this.colorsFlowMap.Count != length)
      this.colorsFlowMap.Clear();
    this.length = 0.0f;
    this.fulllength = 0.0f;
    if (Object.op_Inequality((Object) this.beginningSpline, (Object) null))
      this.length = this.beginningSpline.length;
    this.minMaxWidth = 1f;
    this.uvWidth = 1f;
    this.uvBeginning = 0.0f;
    if (Object.op_Inequality((Object) this.beginningSpline, (Object) null))
    {
      this.minMaxWidth = this.beginningMaxWidth - this.beginningMinWidth;
      this.uvWidth = this.minMaxWidth * this.beginningSpline.uvWidth;
      this.uvBeginning = this.beginningSpline.uvWidth * this.beginningMinWidth + this.beginningSpline.uvBeginning;
    }
    else if (Object.op_Inequality((Object) this.endingSpline, (Object) null))
    {
      this.minMaxWidth = this.endingMaxWidth - this.endingMinWidth;
      this.uvWidth = this.minMaxWidth * this.endingSpline.uvWidth;
      this.uvBeginning = this.endingSpline.uvWidth * this.endingMinWidth + this.endingSpline.uvBeginning;
    }
    for (int index = 0; index < this.pointsDown.Count; ++index)
    {
      float width = this.widths[index];
      if (index > 0)
        this.fulllength += this.uvWidth * Vector3.Distance(this.pointsDown[index], this.pointsDown[index - 1]) / (this.uvScale * width);
    }
    float num2 = Mathf.Round(this.fulllength);
    for (int index1 = 0; index1 < this.pointsDown.Count; ++index1)
    {
      float width = this.widths[index1];
      int num3 = index1 * this.vertsInShape;
      if (index1 > 0)
        this.length += this.uvWidth * Vector3.Distance(this.pointsDown[index1], this.pointsDown[index1 - 1]) / (this.uvScale * width) / this.fulllength * num2;
      float num4 = 0.0f;
      float u1 = 0.0f;
      for (int index2 = 0; index2 < this.vertsInShape; ++index2)
      {
        int index3 = num3 + index2;
        float num5 = (float) index2 / (float) (this.vertsInShape - 1);
        float num6 = (double) num5 >= 0.5 ? (float) ((((double) num5 - 0.5) * (1.0 - (double) this.maxVal) + 0.5 * (double) this.maxVal) * 2.0) : num5 * (this.minVal * 2f);
        if (index1 == 0 && Object.op_Inequality((Object) this.beginningSpline, (Object) null) && (this.beginningSpline.verticesEnding != null && this.beginningSpline.normalsEnding != null))
        {
          int num7 = (int) ((double) this.beginningSpline.vertsInShape * (double) this.beginningMinWidth);
          vector3Array1[index3] = Vector3.op_Subtraction(Vector3.op_Addition(this.beginningSpline.verticesEnding[Mathf.Clamp(index2 + num7, 0, this.beginningSpline.verticesEnding.Count - 1)], ((Component) this.beginningSpline).get_transform().get_position()), ((Component) this).get_transform().get_position());
        }
        else if (index1 == this.pointsDown.Count - 1 && Object.op_Inequality((Object) this.endingSpline, (Object) null) && (this.endingSpline.verticesBeginning != null && this.endingSpline.normalsBeginning != null))
        {
          int num7 = (int) ((double) this.endingSpline.vertsInShape * (double) this.endingMinWidth);
          vector3Array1[index3] = Vector3.op_Subtraction(Vector3.op_Addition(this.endingSpline.verticesBeginning[Mathf.Clamp(index2 + num7, 0, this.endingSpline.verticesBeginning.Count - 1)], ((Component) this.endingSpline).get_transform().get_position()), ((Component) this).get_transform().get_position());
        }
        else
        {
          vector3Array1[index3] = Vector3.Lerp(this.pointsDown[index1], this.pointsUp[index1], num6);
          RaycastHit raycastHit1;
          if (Physics.Raycast(Vector3.op_Addition(Vector3.op_Addition(vector3Array1[index3], ((Component) this).get_transform().get_position()), Vector3.op_Multiply(Vector3.get_up(), 5f)), Vector3.get_down(), ref raycastHit1, 1000f, ((LayerMask) ref this.snapMask).get_value()))
            vector3Array1[index3] = Vector3.Lerp(vector3Array1[index3], Vector3.op_Addition(Vector3.op_Subtraction(((RaycastHit) ref raycastHit1).get_point(), ((Component) this).get_transform().get_position()), new Vector3(0.0f, 0.1f, 0.0f)), (float) (((double) Mathf.Sin((float) (3.14159274101257 * (double) this.snaps[index1] - 1.57079637050629)) + 1.0) * 0.5));
          RaycastHit raycastHit2;
          if (this.normalFromRaycast && Physics.Raycast(Vector3.op_Addition(Vector3.op_Addition(this.points[index1], ((Component) this).get_transform().get_position()), Vector3.op_Multiply(Vector3.get_up(), 5f)), Vector3.get_down(), ref raycastHit2, 1000f, ((LayerMask) ref this.snapMask).get_value()))
            vector3Array2[index3] = ((RaycastHit) ref raycastHit2).get_normal();
          ref Vector3 local = ref vector3Array1[index3];
          local.y = (__Null) (local.y + (double) Mathf.Lerp(this.controlPointsMeshCurves[Mathf.FloorToInt(this.lerpValues[index1])].Evaluate(num6), this.controlPointsMeshCurves[Mathf.CeilToInt(this.lerpValues[index1])].Evaluate(num6), this.lerpValues[index1] - Mathf.Floor(this.lerpValues[index1])));
        }
        if (index1 > 0 && index1 < 5 && (Object.op_Inequality((Object) this.beginningSpline, (Object) null) && this.beginningSpline.verticesEnding != null))
          vector3Array1[index3].y = (__Null) ((vector3Array1[index3].y + vector3Array1[index3 - this.vertsInShape].y) * 0.5);
        if (index1 == this.pointsDown.Count - 1 && Object.op_Inequality((Object) this.endingSpline, (Object) null) && this.endingSpline.verticesBeginning != null)
        {
          for (int index4 = 1; index4 < 5; ++index4)
            vector3Array1[index3 - this.vertsInShape * index4].y = (__Null) ((vector3Array1[index3 - this.vertsInShape * (index4 - 1)].y + vector3Array1[index3 - this.vertsInShape * index4].y) * 0.5);
        }
        if (index1 == 0)
          this.verticesBeginning.Add(vector3Array1[index3]);
        if (index1 == this.pointsDown.Count - 1)
          this.verticesEnding.Add(vector3Array1[index3]);
        if (!this.normalFromRaycast)
          vector3Array2[index3] = Quaternion.op_Multiply(this.orientations[index1], Vector3.get_up());
        if (index1 == 0)
          this.normalsBeginning.Add(vector3Array2[index3]);
        if (index1 == this.pointsDown.Count - 1)
          this.normalsEnding.Add(vector3Array2[index3]);
        if (index2 > 0)
        {
          num4 = num6 * this.uvWidth;
          u1 = num6;
        }
        if (Object.op_Inequality((Object) this.beginningSpline, (Object) null) || Object.op_Inequality((Object) this.endingSpline, (Object) null))
          num4 += this.uvBeginning;
        num4 /= this.uvScale;
        float num8 = this.FlowCalculate(u1, (float) vector3Array2[index3].y);
        int num9 = 10;
        if (this.beginnigChildSplines.Count > 0 && index1 <= num9)
        {
          float u2 = 0.0f;
          foreach (RamSpline beginnigChildSpline in this.beginnigChildSplines)
          {
            if (Mathf.CeilToInt(beginnigChildSpline.endingMaxWidth * (float) (this.vertsInShape - 1)) >= index2 && index2 >= Mathf.CeilToInt(beginnigChildSpline.endingMinWidth * (float) (this.vertsInShape - 1)))
            {
              u2 = (float) (index2 - Mathf.CeilToInt(beginnigChildSpline.endingMinWidth * (float) (this.vertsInShape - 1))) / (float) (Mathf.CeilToInt(beginnigChildSpline.endingMaxWidth * (float) (this.vertsInShape - 1)) - Mathf.CeilToInt(beginnigChildSpline.endingMinWidth * (float) (this.vertsInShape - 1)));
              u2 = this.FlowCalculate(u2, (float) vector3Array2[index3].y);
            }
          }
          num8 = index1 <= 0 ? u2 : Mathf.Lerp(num8, u2, (float) (1.0 - (double) index1 / (double) num9));
        }
        if (index1 >= this.pointsDown.Count - num9 - 1 && this.endingChildSplines.Count > 0)
        {
          float u2 = 0.0f;
          foreach (RamSpline endingChildSpline in this.endingChildSplines)
          {
            if (Mathf.CeilToInt(endingChildSpline.beginningMaxWidth * (float) (this.vertsInShape - 1)) >= index2 && index2 >= Mathf.CeilToInt(endingChildSpline.beginningMinWidth * (float) (this.vertsInShape - 1)))
            {
              u2 = (float) (index2 - Mathf.CeilToInt(endingChildSpline.beginningMinWidth * (float) (this.vertsInShape - 1))) / (float) (Mathf.CeilToInt(endingChildSpline.beginningMaxWidth * (float) (this.vertsInShape - 1)) - Mathf.CeilToInt(endingChildSpline.beginningMinWidth * (float) (this.vertsInShape - 1)));
              u2 = this.FlowCalculate(u2, (float) vector3Array2[index3].y);
            }
          }
          num8 = index1 >= this.pointsDown.Count - 1 ? u2 : Mathf.Lerp(num8, u2, (float) (index1 - (this.pointsDown.Count - num9 - 1)) / (float) num9);
        }
        float num10 = (float) (-((double) u1 - 0.5) * 0.00999999977648258);
        if (this.uvRotation)
        {
          if (!this.invertUVDirection)
          {
            vector2Array1[index3] = new Vector2(1f - this.length, num4);
            vector2Array2[index3] = new Vector2((float) (1.0 - (double) this.length / (double) this.fulllength), u1);
            vector2Array3[index3] = new Vector2(num8, num10);
          }
          else
          {
            vector2Array1[index3] = new Vector2(1f + this.length, num4);
            vector2Array2[index3] = new Vector2((float) (1.0 + (double) this.length / (double) this.fulllength), u1);
            vector2Array3[index3] = new Vector2(num8, num10);
          }
        }
        else if (!this.invertUVDirection)
        {
          vector2Array1[index3] = new Vector2(num4, 1f - this.length);
          vector2Array2[index3] = new Vector2(u1, (float) (1.0 - (double) this.length / (double) this.fulllength));
          vector2Array3[index3] = new Vector2(num10, num8);
        }
        else
        {
          vector2Array1[index3] = new Vector2(num4, 1f + this.length);
          vector2Array2[index3] = new Vector2(u1, (float) (1.0 + (double) this.length / (double) this.fulllength));
          vector2Array3[index3] = new Vector2(num10, num8);
        }
        if (this.colorsFlowMap.Count <= index3)
          this.colorsFlowMap.Add(vector2Array3[index3]);
        else if (!this.overrideFlowMap)
          this.colorsFlowMap[index3] = vector2Array3[index3];
      }
    }
    for (int index1 = 0; index1 < num1; ++index1)
    {
      int num3 = index1 * this.vertsInShape;
      for (int index2 = 0; index2 < this.vertsInShape - 1; ++index2)
      {
        int num4 = num3 + index2;
        int num5 = num3 + index2 + this.vertsInShape;
        int num6 = num3 + index2 + 1 + this.vertsInShape;
        int num7 = num3 + index2 + 1;
        intList.Add(num4);
        intList.Add(num5);
        intList.Add(num6);
        intList.Add(num6);
        intList.Add(num7);
        intList.Add(num4);
      }
    }
    mesh = new Mesh();
    mesh.Clear();
    mesh.set_vertices(vector3Array1);
    mesh.set_normals(vector3Array2);
    mesh.set_uv(vector2Array1);
    mesh.set_uv3(vector2Array2);
    mesh.set_uv4(this.colorsFlowMap.ToArray());
    mesh.set_triangles(intList.ToArray());
    mesh.set_colors(this.colors);
    mesh.RecalculateTangents();
    this.meshfilter.set_mesh(mesh);
    ((Renderer) ((Component) this).GetComponent<MeshRenderer>()).set_enabled(true);
    if (!this.generateMeshParts)
      return;
    this.GenerateMeshParts(mesh);
  }

  public void GenerateMeshParts(Mesh baseMesh)
  {
    using (List<Transform>.Enumerator enumerator = this.meshesPartTransforms.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        Transform current = enumerator.Current;
        if (Object.op_Inequality((Object) current, (Object) null))
          Object.DestroyImmediate((Object) ((Component) current).get_gameObject());
      }
    }
    Vector3[] vertices = baseMesh.get_vertices();
    Vector3[] normals = baseMesh.get_normals();
    Vector2[] uv = baseMesh.get_uv();
    Vector2[] uv3 = baseMesh.get_uv3();
    ((Renderer) ((Component) this).GetComponent<MeshRenderer>()).set_enabled(false);
    int num1 = Mathf.RoundToInt((float) (vertices.Length / this.vertsInShape) / (float) this.meshPartsCount) * this.vertsInShape;
    for (int index1 = 0; index1 < this.meshPartsCount; ++index1)
    {
      GameObject gameObject = new GameObject(((Object) ((Component) this).get_gameObject()).get_name() + "- Mesh part " + (object) index1);
      gameObject.get_transform().SetParent(((Component) this).get_gameObject().get_transform(), false);
      gameObject.get_transform().set_localPosition(Vector3.get_zero());
      gameObject.get_transform().set_localEulerAngles(Vector3.get_zero());
      gameObject.get_transform().set_localScale(Vector3.get_one());
      this.meshesPartTransforms.Add(gameObject.get_transform());
      MeshRenderer meshRenderer = (MeshRenderer) gameObject.AddComponent<MeshRenderer>();
      ((Renderer) meshRenderer).set_sharedMaterial(((Renderer) ((Component) this).GetComponent<MeshRenderer>()).get_sharedMaterial());
      ((Renderer) meshRenderer).set_receiveShadows(false);
      ((Renderer) meshRenderer).set_shadowCastingMode((ShadowCastingMode) 0);
      MeshFilter meshFilter = (MeshFilter) gameObject.AddComponent<MeshFilter>();
      Mesh mesh = new Mesh();
      mesh.Clear();
      List<Vector3> vector3List1 = new List<Vector3>();
      List<Vector3> vector3List2 = new List<Vector3>();
      List<Vector2> vector2List1 = new List<Vector2>();
      List<Vector2> vector2List2 = new List<Vector2>();
      List<Vector2> vector2List3 = new List<Vector2>();
      List<Color> colorList = new List<Color>();
      List<int> intList = new List<int>();
      for (int index2 = num1 * index1 + (index1 <= 0 ? 0 : -this.vertsInShape); index2 < num1 * (index1 + 1) && index2 < vertices.Length || index1 == this.meshPartsCount - 1 && index2 < vertices.Length; ++index2)
      {
        vector3List1.Add(vertices[index2]);
        vector3List2.Add(normals[index2]);
        vector2List1.Add(uv[index2]);
        vector2List2.Add(uv3[index2]);
        vector2List3.Add(this.colorsFlowMap[index2]);
        colorList.Add(this.colors[index2]);
      }
      if (vector3List1.Count > 0)
      {
        Vector3 vector3 = vector3List1[0];
        for (int index2 = 0; index2 < vector3List1.Count; ++index2)
          vector3List1[index2] = Vector3.op_Subtraction(vector3List1[index2], vector3);
        for (int index2 = 0; index2 < vector3List1.Count / this.vertsInShape - 1; ++index2)
        {
          int num2 = index2 * this.vertsInShape;
          for (int index3 = 0; index3 < this.vertsInShape - 1; ++index3)
          {
            int num3 = num2 + index3;
            int num4 = num2 + index3 + this.vertsInShape;
            int num5 = num2 + index3 + 1 + this.vertsInShape;
            int num6 = num2 + index3 + 1;
            intList.Add(num3);
            intList.Add(num4);
            intList.Add(num5);
            intList.Add(num5);
            intList.Add(num6);
            intList.Add(num3);
          }
        }
        Transform transform = gameObject.get_transform();
        transform.set_position(Vector3.op_Addition(transform.get_position(), vector3));
        mesh.set_vertices(vector3List1.ToArray());
        mesh.set_triangles(intList.ToArray());
        mesh.set_normals(vector3List2.ToArray());
        mesh.set_uv(vector2List1.ToArray());
        mesh.set_uv3(vector2List2.ToArray());
        mesh.set_uv4(vector2List3.ToArray());
        mesh.set_colors(colorList.ToArray());
        mesh.RecalculateTangents();
        meshFilter.set_mesh(mesh);
      }
    }
  }

  private float FlowCalculate(float u, float normalY)
  {
    return Mathf.Lerp(this.flowWaterfall.Evaluate(u), this.flowFlat.Evaluate(u), Mathf.Clamp(normalY, 0.0f, 1f));
  }
}
