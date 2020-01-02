// Decompiled with JetBrains decompiler
// Type: MeshTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MeshTool : MonoBehaviour
{
  public List<MeshFilter> m_Filters;
  public float m_Radius;
  public float m_Power;
  public MeshTool.ExtrudeMethod m_Method;
  private RaycastHit m_HitInfo;

  public MeshTool()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Cursor.set_lockState((CursorLockMode) 1);
    Cursor.set_visible(false);
  }

  private void Update()
  {
    Ray ray;
    ((Ray) ref ray).\u002Ector(((Component) Camera.get_main()).get_transform().get_position(), ((Component) Camera.get_main()).get_transform().get_forward());
    if (!Physics.Raycast(((Ray) ref ray).get_origin(), ((Ray) ref ray).get_direction(), ref this.m_HitInfo))
      return;
    Debug.DrawRay(((RaycastHit) ref this.m_HitInfo).get_point(), ((RaycastHit) ref this.m_HitInfo).get_normal(), Color.get_red());
    Vector3 vector3 = this.m_Method != MeshTool.ExtrudeMethod.Vertical ? ((RaycastHit) ref this.m_HitInfo).get_normal() : Vector3.get_up();
    if (Input.GetMouseButton(0) || Input.GetKey((KeyCode) 32) && !Input.GetKey((KeyCode) 304))
      this.ModifyMesh(Vector3.op_Multiply(this.m_Power, vector3), ((RaycastHit) ref this.m_HitInfo).get_point());
    if (!Input.GetMouseButton(1) && (!Input.GetKey((KeyCode) 32) || !Input.GetKey((KeyCode) 304)))
      return;
    this.ModifyMesh(Vector3.op_Multiply(-this.m_Power, vector3), ((RaycastHit) ref this.m_HitInfo).get_point());
  }

  private void ModifyMesh(Vector3 displacement, Vector3 center)
  {
    using (List<MeshFilter>.Enumerator enumerator = this.m_Filters.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        MeshFilter current = enumerator.Current;
        Mesh mesh1 = current.get_mesh();
        Vector3[] vertices = mesh1.get_vertices();
        for (int index = 0; index < vertices.Length; ++index)
        {
          Vector3 pos = ((Component) current).get_transform().TransformPoint(vertices[index]);
          vertices[index] = Vector3.op_Addition(vertices[index], Vector3.op_Multiply(displacement, MeshTool.Gaussian(pos, center, this.m_Radius)));
        }
        mesh1.set_vertices(vertices);
        mesh1.RecalculateBounds();
        MeshCollider component = (MeshCollider) ((Component) current).GetComponent<MeshCollider>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          Mesh mesh2 = new Mesh();
          mesh2.set_vertices(mesh1.get_vertices());
          mesh2.set_triangles(mesh1.get_triangles());
          component.set_sharedMesh(mesh2);
        }
      }
    }
  }

  private static float Gaussian(Vector3 pos, Vector3 mean, float dev)
  {
    float num1 = (float) (pos.x - mean.x);
    float num2 = (float) (pos.y - mean.y);
    float num3 = (float) (pos.z - mean.z);
    return (float) (1.0 / (6.28318548202515 * (double) dev * (double) dev)) * Mathf.Pow(2.718282f, (float) (-((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3) / (2.0 * (double) dev * (double) dev)));
  }

  public enum ExtrudeMethod
  {
    Vertical,
    MeshNormal,
  }
}
