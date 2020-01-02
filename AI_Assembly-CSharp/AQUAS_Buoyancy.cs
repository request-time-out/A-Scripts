// Decompiled with JetBrains decompiler
// Type: AQUAS_Buoyancy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[AddComponentMenu("AQUAS/Buoyancy")]
[RequireComponent(typeof (Rigidbody))]
public class AQUAS_Buoyancy : MonoBehaviour
{
  public float waterLevel;
  public float waterDensity;
  [Space(5f)]
  public bool useBalanceFactor;
  public Vector3 balanceFactor;
  [Space(20f)]
  [Range(0.0f, 1f)]
  public float dynamicSurface;
  [Range(1f, 10f)]
  public float bounceFrequency;
  [Space(5f)]
  [Header("Debugging can be ver performance heavy!")]
  public AQUAS_Buoyancy.debugModes debug;
  private Vector3[] vertices;
  private int[] triangles;
  private Mesh mesh;
  private Rigidbody rb;
  private float effWaterDensity;
  private float regWaterDensity;
  private float maxWaterDensity;

  public AQUAS_Buoyancy()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.mesh = ((MeshFilter) ((Component) this).GetComponent<MeshFilter>()).get_mesh();
    this.vertices = this.mesh.get_vertices();
    this.triangles = this.mesh.get_triangles();
    this.rb = (Rigidbody) ((Component) this).GetComponent<Rigidbody>();
    this.regWaterDensity = this.waterDensity;
    this.maxWaterDensity = this.regWaterDensity + this.regWaterDensity * 0.5f * this.dynamicSurface;
  }

  private void FixedUpdate()
  {
    if (this.balanceFactor.x < 1.0 / 1000.0)
      this.balanceFactor.x = (__Null) (1.0 / 1000.0);
    if (this.balanceFactor.y < 1.0 / 1000.0)
      this.balanceFactor.y = (__Null) (1.0 / 1000.0);
    if (this.balanceFactor.z < 1.0 / 1000.0)
      this.balanceFactor.z = (__Null) (1.0 / 1000.0);
    this.AddForce();
  }

  private void Update()
  {
    this.regWaterDensity = this.waterDensity;
    this.maxWaterDensity = this.regWaterDensity + this.regWaterDensity * 0.5f * this.dynamicSurface;
    this.effWaterDensity = (float) (((double) this.maxWaterDensity - (double) this.regWaterDensity) / 2.0 + (double) this.regWaterDensity + (double) Mathf.Sin(Time.get_time() * this.bounceFrequency) * ((double) this.maxWaterDensity - (double) this.regWaterDensity) / 2.0);
  }

  private void AddForce()
  {
    for (int index = 0; index < this.triangles.Length; index += 3)
    {
      Vector3 vertex1 = this.vertices[this.triangles[index]];
      Vector3 vertex2 = this.vertices[this.triangles[index + 1]];
      Vector3 vertex3 = this.vertices[this.triangles[index + 2]];
      float num1 = this.waterLevel - (float) this.Center(vertex1, vertex2, vertex3).y;
      if ((double) num1 > 0.0 && this.Center(vertex1, vertex2, vertex3).y > Vector3.op_Addition(this.Center(vertex1, vertex2, vertex3), this.Normal(vertex1, vertex2, vertex3)).y)
      {
        double num2 = (double) this.effWaterDensity * Physics.get_gravity().y * (double) num1 * (double) this.Area(vertex1, vertex2, vertex3);
        Vector3 vector3 = this.Normal(vertex1, vertex2, vertex3);
        // ISSUE: variable of the null type
        __Null y = ((Vector3) ref vector3).get_normalized().y;
        float num3 = (float) (num2 * y);
        if (this.useBalanceFactor)
          this.rb.AddForceAtPosition(new Vector3(0.0f, num3, 0.0f), ((Component) this).get_transform().TransformPoint(new Vector3((float) (((Component) this).get_transform().InverseTransformPoint(this.Center(vertex1, vertex2, vertex3)).x / (this.balanceFactor.x * ((Component) this).get_transform().get_localScale().x * 1000.0)), (float) (((Component) this).get_transform().InverseTransformPoint(this.Center(vertex1, vertex2, vertex3)).y / (this.balanceFactor.y * ((Component) this).get_transform().get_localScale().x * 1000.0)), (float) (((Component) this).get_transform().InverseTransformPoint(this.Center(vertex1, vertex2, vertex3)).z / (this.balanceFactor.z * ((Component) this).get_transform().get_localScale().x * 1000.0)))));
        else
          this.rb.AddForceAtPosition(new Vector3(0.0f, num3, 0.0f), ((Component) this).get_transform().TransformPoint(new Vector3((float) ((Component) this).get_transform().InverseTransformPoint(this.Center(vertex1, vertex2, vertex3)).x, (float) ((Component) this).get_transform().InverseTransformPoint(this.Center(vertex1, vertex2, vertex3)).y, (float) ((Component) this).get_transform().InverseTransformPoint(this.Center(vertex1, vertex2, vertex3)).z)));
        if (this.debug == AQUAS_Buoyancy.debugModes.showAffectedFaces)
          Debug.DrawLine(this.Center(vertex1, vertex2, vertex3), Vector3.op_Addition(this.Center(vertex1, vertex2, vertex3), this.Normal(vertex1, vertex2, vertex3)), Color.get_white());
        if (this.debug == AQUAS_Buoyancy.debugModes.showForceRepresentation)
          Debug.DrawRay(this.Center(vertex1, vertex2, vertex3), new Vector3(0.0f, num3, 0.0f), Color.get_red());
        if (this.debug == AQUAS_Buoyancy.debugModes.showReferenceVolume)
        {
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex1).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex1).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex2).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex2).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex2).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex2).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex3).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex3).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex3).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex3).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex1).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex1).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex1).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex1).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex2).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex2).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex2).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex2).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex3).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex3).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex3).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex3).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex1).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex1).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex1).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex1).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex1).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex1).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex2).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex2).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex2).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex2).z), Color.get_green());
          Debug.DrawLine(new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex3).x, this.waterLevel, (float) ((Component) this).get_transform().TransformPoint(vertex3).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(vertex3).x, (float) this.Center(vertex1, vertex2, vertex3).y, (float) ((Component) this).get_transform().TransformPoint(vertex3).z), Color.get_green());
        }
      }
    }
  }

  private Vector3 Center(Vector3 p1, Vector3 p2, Vector3 p3)
  {
    return ((Component) this).get_transform().TransformPoint(Vector3.op_Division(Vector3.op_Addition(Vector3.op_Addition(p1, p2), p3), 3f));
  }

  private Vector3 Normal(Vector3 p1, Vector3 p2, Vector3 p3)
  {
    Vector3 vector3 = Vector3.Cross(Vector3.op_Subtraction(((Component) this).get_transform().TransformPoint(p2), ((Component) this).get_transform().TransformPoint(p1)), Vector3.op_Subtraction(((Component) this).get_transform().TransformPoint(p3), ((Component) this).get_transform().TransformPoint(p1)));
    return ((Vector3) ref vector3).get_normalized();
  }

  private float Area(Vector3 p1, Vector3 p2, Vector3 p3)
  {
    return (float) ((double) Vector3.Distance(new Vector3((float) ((Component) this).get_transform().TransformPoint(p1).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p1).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(p2).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p2).z)) * (double) Vector3.Distance(new Vector3((float) ((Component) this).get_transform().TransformPoint(p3).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p3).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(p1).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p1).z)) * (double) Mathf.Sin(Vector3.Angle(Vector3.op_Subtraction(new Vector3((float) ((Component) this).get_transform().TransformPoint(p2).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p2).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(p1).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p1).z)), Vector3.op_Subtraction(new Vector3((float) ((Component) this).get_transform().TransformPoint(p3).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p3).z), new Vector3((float) ((Component) this).get_transform().TransformPoint(p1).x, (float) this.Center(p1, p2, p3).y, (float) ((Component) this).get_transform().TransformPoint(p1).z))) * ((float) Math.PI / 180f)) / 2.0);
  }

  public enum debugModes
  {
    none,
    showAffectedFaces,
    showForceRepresentation,
    showReferenceVolume,
  }
}
