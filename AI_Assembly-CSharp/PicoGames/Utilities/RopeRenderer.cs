// Decompiled with JetBrains decompiler
// Type: PicoGames.Utilities.RopeRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using PicoGames.QuickRopes;
using System;
using UnityEngine;

namespace PicoGames.Utilities
{
  [AddComponentMenu("PicoGames/QuickRopes/Extensions/Rope Renderer")]
  [ExecuteInEditMode]
  [DisallowMultipleComponent]
  [RequireComponent(typeof (QuickRope))]
  public class RopeRenderer : MonoBehaviour
  {
    public bool showBounds;
    public bool showEdges;
    public bool showNormals;
    [SerializeField]
    private int leafs;
    [SerializeField]
    private int detail;
    [SerializeField]
    private float center;
    [SerializeField]
    [Min(1f)]
    private int strandCount;
    [SerializeField]
    private float strandOffset;
    [SerializeField]
    private float twistAngle;
    [SerializeField]
    private float radius;
    [SerializeField]
    private AnimationCurve radiusCurve;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Vector2 uvTile;
    [SerializeField]
    private Vector2 uvOffset;
    [SerializeField]
    [HideInInspector]
    private QuickRope rope;
    [SerializeField]
    [HideInInspector]
    private bool flagShapeUpdate;
    [SerializeField]
    [HideInInspector]
    private Vector3[] shapeLookup;
    [SerializeField]
    [HideInInspector]
    private int[] shapeTriIndices;
    [SerializeField]
    [HideInInspector]
    private int edgeCount;
    [SerializeField]
    [HideInInspector]
    private Vector3 kUp;
    [SerializeField]
    [HideInInspector]
    private Vector3[] positions;
    [SerializeField]
    [HideInInspector]
    private Quaternion[] rotations;
    [SerializeField]
    [HideInInspector]
    private Vector3[] directions;
    [SerializeField]
    [HideInInspector]
    private bool[] isJoints;
    [SerializeField]
    [HideInInspector]
    private Vector3[] vertices;
    [SerializeField]
    [HideInInspector]
    private Vector3[] normals;
    [SerializeField]
    [HideInInspector]
    private int[] triangles;
    [SerializeField]
    [HideInInspector]
    private Vector2[] uvs;
    private GameObject meshObject;
    private Mesh mesh;
    private MeshRenderer mRenderer;
    private MeshFilter mFilter;
    private bool dontRedraw;
    private static int vertIndex;
    private static int triaIndex;

    public RopeRenderer()
    {
      base.\u002Ector();
    }

    public int EdgeCount
    {
      get
      {
        return this.leafs;
      }
      set
      {
        if (this.leafs == value)
          return;
        this.flagShapeUpdate = true;
        this.leafs = value;
      }
    }

    public int EdgeDetail
    {
      get
      {
        return this.detail;
      }
      set
      {
        if (this.detail == value)
          return;
        this.flagShapeUpdate = true;
        this.detail = value;
      }
    }

    public float EdgeIndent
    {
      get
      {
        return this.center;
      }
      set
      {
        if ((double) this.center == (double) value)
          return;
        this.flagShapeUpdate = true;
        this.center = value;
      }
    }

    public int StrandCount
    {
      get
      {
        return this.strandCount;
      }
      set
      {
        if (this.strandCount == value)
          return;
        this.strandCount = value;
      }
    }

    public float StrandOffset
    {
      get
      {
        return this.strandOffset;
      }
      set
      {
        if ((double) this.strandOffset == (double) value)
          return;
        this.strandOffset = value;
      }
    }

    public float StrandTwist
    {
      get
      {
        return this.twistAngle;
      }
      set
      {
        if ((double) this.twistAngle == (double) value)
          return;
        this.twistAngle = value;
      }
    }

    public float Radius
    {
      get
      {
        return this.radius;
      }
      set
      {
        if ((double) this.radius == (double) value)
          return;
        this.radius = value;
      }
    }

    public AnimationCurve RadiusCurve
    {
      get
      {
        return this.radiusCurve;
      }
      set
      {
        this.radiusCurve = value;
      }
    }

    public Material Material
    {
      get
      {
        return this.material;
      }
      set
      {
        this.material = value;
      }
    }

    public Vector2 UVOffset
    {
      get
      {
        return this.uvOffset;
      }
      set
      {
        this.uvOffset = value;
      }
    }

    public Vector2 UVTile
    {
      get
      {
        return this.uvTile;
      }
      set
      {
        this.uvTile = value;
      }
    }

    private void OnDrawGizmos()
    {
      if (Object.op_Inequality((Object) this.mesh, (Object) null) && this.showBounds)
      {
        Gizmos.set_color(Color.get_gray());
        Bounds bounds1 = this.mesh.get_bounds();
        Vector3 center = ((Bounds) ref bounds1).get_center();
        Bounds bounds2 = this.mesh.get_bounds();
        Vector3 size = ((Bounds) ref bounds2).get_size();
        Gizmos.DrawWireCube(center, size);
      }
      if (this.vertices != null && (this.showEdges || this.showNormals))
      {
        for (int index = 0; index < this.vertices.Length; ++index)
        {
          Gizmos.set_color(Color.get_yellow());
          Gizmos.DrawWireCube(this.vertices[index], Vector3.op_Multiply(Vector3.get_one(), 0.01f));
          if (this.showNormals)
          {
            Gizmos.set_color(Color.get_magenta());
            Gizmos.DrawRay(this.vertices[index], this.normals[index]);
          }
        }
      }
      if (!this.showEdges)
        return;
      Gizmos.set_color(Color.get_blue());
      for (int index1 = 0; index1 < this.strandCount; ++index1)
      {
        for (int index2 = 0; index2 < this.positions.Length; ++index2)
        {
          for (int index3 = 0; index3 < this.edgeCount + 1; ++index3)
            Gizmos.DrawLine(this.vertices[index3 + index2 * (this.edgeCount + 1)], this.vertices[(index3 + 1) % this.edgeCount + index2 * (this.edgeCount + 1)]);
        }
      }
    }

    private void OnDestroy()
    {
      if (!Object.op_Inequality((Object) this.meshObject, (Object) null))
        return;
      if (Application.get_isPlaying())
      {
        Object.Destroy((Object) this.mesh);
        Object.Destroy((Object) this.meshObject);
      }
      else
      {
        Object.DestroyImmediate((Object) this.mesh);
        Object.DestroyImmediate((Object) this.meshObject);
      }
    }

    private void OnDisable()
    {
      if (!Object.op_Inequality((Object) this.meshObject, (Object) null))
        return;
      if (Application.get_isPlaying())
      {
        Object.Destroy((Object) this.mesh);
        Object.Destroy((Object) this.meshObject);
      }
      else
      {
        Object.DestroyImmediate((Object) this.mesh);
        Object.DestroyImmediate((Object) this.meshObject);
      }
    }

    private void Start()
    {
      this.rope = (QuickRope) ((Component) this).get_gameObject().GetComponent<QuickRope>();
    }

    public void LateUpdate()
    {
      if (Application.get_isPlaying() && this.dontRedraw)
        return;
      if (this.flagShapeUpdate)
        this.UpdateShape();
      this.UpdatePositions();
      this.UpdateRotations();
      this.RedrawMesh();
      if (!((Component) this).get_gameObject().get_isStatic())
        return;
      this.dontRedraw = true;
    }

    private void VerifyMeshExists()
    {
      if (Object.op_Equality((Object) this.meshObject, (Object) null))
      {
        this.meshObject = GameObject.Find("Rope_Obj_" + (object) ((Object) ((Component) this).get_gameObject()).GetInstanceID());
        if (Object.op_Equality((Object) this.meshObject, (Object) null))
          this.meshObject = new GameObject("Rope_Obj_" + (object) ((Object) ((Component) this).get_gameObject()).GetInstanceID(), new System.Type[2]
          {
            typeof (MeshFilter),
            typeof (MeshRenderer)
          });
        ((Object) this.meshObject).set_hideFlags((HideFlags) 1);
      }
      if (Object.op_Equality((Object) this.mesh, (Object) null))
      {
        this.mesh = new Mesh();
        ((Object) this.mesh).set_hideFlags((HideFlags) 52);
      }
      if (Object.op_Equality((Object) this.mFilter, (Object) null))
      {
        this.mFilter = (MeshFilter) this.meshObject.GetComponent<MeshFilter>();
        if (Object.op_Equality((Object) this.mFilter, (Object) null))
          this.mFilter = (MeshFilter) this.meshObject.AddComponent<MeshFilter>();
      }
      if (Object.op_Equality((Object) this.mRenderer, (Object) null))
      {
        this.mRenderer = (MeshRenderer) this.meshObject.GetComponent<MeshRenderer>();
        if (Object.op_Equality((Object) this.mRenderer, (Object) null))
          this.mRenderer = (MeshRenderer) this.meshObject.AddComponent<MeshRenderer>();
      }
      if (!Object.op_Equality((Object) this.material, (Object) null))
        return;
      this.material = new Material(Shader.Find("Standard"));
    }

    private void RedrawMesh()
    {
      this.strandCount = Mathf.Max(1, this.strandCount);
      this.edgeCount = Mathf.Max(3, this.edgeCount);
      this.detail = Mathf.Max(1, this.detail);
      this.VerifyMeshExists();
      int length1 = (this.edgeCount + 1) * this.positions.Length * this.strandCount + this.shapeLookup.Length * this.strandCount * 2;
      int length2 = 6 * this.edgeCount * this.positions.Length * this.strandCount + this.shapeTriIndices.Length * this.strandCount * 2;
      if (this.vertices == null || this.vertices.Length != length1)
        this.vertices = new Vector3[length1];
      if (this.normals == null || this.normals.Length != length1)
        this.normals = new Vector3[length1];
      if (this.uvs == null || this.uvs.Length != length1)
        this.uvs = new Vector2[length1];
      if (this.triangles == null || this.triangles.Length != length2)
        this.triangles = new int[length2];
      Vector3 vector3_1 = Vector3.op_Multiply(Vector3.get_one(), float.MaxValue);
      Vector3 vector3_2 = Vector3.op_Multiply(Vector3.get_one(), float.MinValue);
      RopeRenderer.vertIndex = RopeRenderer.triaIndex = 0;
      Matrix4x4 matrix4x4 = (Matrix4x4) null;
      for (int index1 = 0; index1 < this.strandCount; ++index1)
      {
        float num1 = (float) (360.0 / (double) this.strandCount * (double) index1 * (Math.PI / 180.0));
        Vector3 vector3_3;
        ((Vector3) ref vector3_3).\u002Ector(Mathf.Cos(num1), Mathf.Sin(num1), 0.0f);
        int vertIndex = RopeRenderer.vertIndex;
        for (int index2 = 0; index2 < this.positions.Length; ++index2)
        {
          float num2 = this.radiusCurve.Evaluate((float) index2 * (1f / (float) this.positions.Length)) * this.radius;
          ((Matrix4x4) ref matrix4x4).SetTRS(Vector3.op_Addition(this.positions[index2], this.strandCount <= 1 ? Vector3.get_zero() : Vector3.op_Multiply(Quaternion.op_Multiply(this.rotations[index2], vector3_3), num2 * this.strandOffset)), this.rotations[index2], Vector3.op_Multiply(Vector3.get_one(), num2));
          for (int index3 = 0; index3 < this.edgeCount + 1; ++index3)
          {
            int index4 = index3 % this.shapeLookup.Length;
            this.vertices[RopeRenderer.vertIndex] = ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(this.shapeLookup[index4]);
            this.normals[RopeRenderer.vertIndex] = Quaternion.op_Multiply(this.rotations[index2], this.shapeLookup[index4]);
            this.uvs[RopeRenderer.vertIndex] = new Vector2((float) ((double) index3 / (double) this.edgeCount * (double) this.edgeCount * this.uvTile.x + this.uvOffset.x), (float) ((double) index2 / (double) (this.positions.Length - 1) * (double) this.positions.Length * this.uvTile.y + this.uvOffset.y));
            vector3_1 = Vector3.Min(vector3_1, this.vertices[RopeRenderer.vertIndex]);
            vector3_2 = Vector3.Max(vector3_2, this.vertices[RopeRenderer.vertIndex]);
            if (index2 < this.positions.Length - 1 && index3 < this.edgeCount && this.isJoints[index2])
            {
              this.triangles[RopeRenderer.triaIndex++] = RopeRenderer.vertIndex;
              this.triangles[RopeRenderer.triaIndex++] = RopeRenderer.vertIndex + 1;
              this.triangles[RopeRenderer.triaIndex++] = RopeRenderer.vertIndex + this.edgeCount + 1;
              this.triangles[RopeRenderer.triaIndex++] = RopeRenderer.vertIndex + this.edgeCount + 1;
              this.triangles[RopeRenderer.triaIndex++] = RopeRenderer.vertIndex + 1;
              this.triangles[RopeRenderer.triaIndex++] = RopeRenderer.vertIndex + 1 + this.edgeCount + 1;
            }
            ++RopeRenderer.vertIndex;
          }
        }
        int num3 = RopeRenderer.vertIndex - 1;
        for (int index2 = 0; index2 < this.shapeLookup.Length; ++index2)
        {
          this.vertices[RopeRenderer.vertIndex] = this.vertices[vertIndex + index2];
          this.vertices[RopeRenderer.vertIndex + this.shapeLookup.Length] = this.vertices[num3 - index2];
          this.normals[RopeRenderer.vertIndex] = Quaternion.op_Multiply(this.rotations[0], Vector3.get_back());
          this.normals[RopeRenderer.vertIndex + this.shapeLookup.Length] = Quaternion.op_Multiply(this.rotations[this.rotations.Length - 1], Vector3.get_forward());
          this.uvs[RopeRenderer.vertIndex] = new Vector2((float) this.shapeLookup[index2].x, (float) this.shapeLookup[index2].y);
          this.uvs[RopeRenderer.vertIndex + this.shapeLookup.Length] = new Vector2((float) this.shapeLookup[index2].x, (float) this.shapeLookup[index2].y);
          ++RopeRenderer.vertIndex;
        }
        RopeRenderer.vertIndex += this.shapeLookup.Length;
        for (int index2 = 0; index2 < this.shapeTriIndices.Length; ++index2)
        {
          this.triangles[RopeRenderer.triaIndex] = num3 + 1 + this.shapeTriIndices[index2];
          this.triangles[RopeRenderer.triaIndex + this.shapeTriIndices.Length] = num3 + this.shapeLookup.Length + 1 + this.shapeTriIndices[index2];
          ++RopeRenderer.triaIndex;
        }
        RopeRenderer.triaIndex += this.shapeTriIndices.Length;
      }
      this.mesh.Clear();
      this.mesh.MarkDynamic();
      this.mesh.set_vertices(this.vertices);
      this.mesh.set_triangles(this.triangles);
      this.mesh.set_normals(this.normals);
      this.mesh.set_uv(this.uvs);
      Vector3 vector3_4;
      ((Vector3) ref vector3_4).\u002Ector((float) (vector3_2.x - vector3_1.x), (float) (vector3_2.y - vector3_1.y), (float) (vector3_2.z - vector3_1.z));
      Vector3 vector3_5;
      ((Vector3) ref vector3_5).\u002Ector((float) (vector3_1.x + vector3_4.x * 0.5), (float) (vector3_1.y + vector3_4.y * 0.5), (float) (vector3_1.z + vector3_4.z * 0.5));
      this.mesh.set_bounds(new Bounds(vector3_5, vector3_4));
      this.mFilter.set_sharedMesh(this.mesh);
      ((Renderer) this.mRenderer).set_sharedMaterial(this.material);
    }

    private void UpdatePositions()
    {
      int newSize = this.rope.ActiveLinkCount + 1;
      if (this.positions == null || this.positions.Length != newSize)
      {
        Array.Resize<Vector3>(ref this.positions, newSize);
        Array.Resize<bool>(ref this.isJoints, newSize);
      }
      for (int index = 0; index <= this.rope.ActiveLinkCount; ++index)
      {
        this.positions[index] = this.rope.Links[index].transform.get_position();
        this.isJoints[index] = Object.op_Implicit((Object) ((Component) this.rope.Links[index].transform).GetComponent<ConfigurableJoint>());
      }
      if (this.rope.Spline.IsLooped)
        this.positions[this.positions.Length - 1] = this.positions[0];
      else
        this.positions[this.positions.Length - 1] = this.rope.Links[this.rope.Links.Length - 1].transform.get_position();
    }

    private void UpdateShape()
    {
      this.shapeLookup = Shape.GetRoseCurve(this.leafs, this.detail, this.center, true);
      this.shapeTriIndices = Triangulate.Edge(this.shapeLookup);
      this.edgeCount = this.shapeLookup.Length;
      this.flagShapeUpdate = false;
    }

    private void UpdateRotations()
    {
      if (this.rotations == null || this.rotations.Length != this.positions.Length)
        Array.Resize<Quaternion>(ref this.rotations, this.positions.Length);
      if (this.directions == null || this.directions.Length != this.positions.Length)
        Array.Resize<Vector3>(ref this.directions, this.positions.Length);
      for (int index = 0; index < this.positions.Length - 1; ++index)
        ((Vector3) ref this.directions[index]).Set((float) (this.positions[index + 1].x - this.positions[index].x), (float) (this.positions[index + 1].y - this.positions[index].y), (float) (this.positions[index + 1].z - this.positions[index].z));
      this.directions[this.directions.Length - 1] = this.directions[this.directions.Length - 2];
      Vector3 zero = Vector3.get_zero();
      Vector3 vector3_1 = !Vector3.op_Equality(this.kUp, Vector3.get_zero()) ? this.kUp : (this.directions[0].x != 0.0 || this.directions[0].z != 0.0 ? Vector3.get_up() : Vector3.get_right());
      for (int index = 0; index < this.positions.Length; ++index)
      {
        if (index != 0 && index != this.positions.Length - 1)
          ((Vector3) ref zero).Set((float) (this.directions[index].x + this.directions[index - 1].x), (float) (this.directions[index].y + this.directions[index - 1].y), (float) (this.directions[index].z + this.directions[index - 1].z));
        else if (Vector3.op_Equality(this.positions[0], this.positions[this.positions.Length - 1]))
          ((Vector3) ref zero).Set((float) (this.directions[this.positions.Length - 1].x + this.directions[0].x), (float) (this.directions[this.positions.Length - 1].y + this.directions[0].y), (float) (this.directions[this.positions.Length - 1].z + this.directions[0].z));
        else
          ((Vector3) ref zero).Set((float) this.directions[index].x, (float) this.directions[index].y, (float) this.directions[index].z);
        if (Vector3.op_Equality(zero, Vector3.get_zero()))
        {
          this.rotations[index] = Quaternion.get_identity();
        }
        else
        {
          ((Vector3) ref zero).Normalize();
          Vector3 vector3_2 = Vector3.Cross(vector3_1, zero);
          vector3_1 = Vector3.Cross(zero, vector3_2);
          if (index == 0)
            this.kUp = vector3_1;
          if ((double) this.twistAngle != 0.0)
            vector3_1 = Quaternion.op_Multiply(Quaternion.AngleAxis(this.twistAngle, zero), vector3_1);
          ((Quaternion) ref this.rotations[index]).SetLookRotation(zero, vector3_1);
        }
      }
      if (!this.rope.Spline.IsLooped)
        return;
      this.rotations[this.rotations.Length - 1] = this.rotations[0];
    }
  }
}
