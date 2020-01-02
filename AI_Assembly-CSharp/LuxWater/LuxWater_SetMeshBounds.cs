// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_SetMeshBounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace LuxWater
{
  public class LuxWater_SetMeshBounds : MonoBehaviour
  {
    [Space(6f)]
    [LuxWater_HelpBtn("h.s0d0kaaphhix")]
    public float Expand_XZ;
    public float Expand_Y;
    private Renderer rend;

    public LuxWater_SetMeshBounds()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      Mesh sharedMesh = ((MeshFilter) ((Component) this).GetComponent<MeshFilter>()).get_sharedMesh();
      sharedMesh.RecalculateBounds();
      Bounds bounds = sharedMesh.get_bounds();
      ((Bounds) ref bounds).Expand(new Vector3(this.Expand_XZ, this.Expand_Y, this.Expand_XZ));
      sharedMesh.set_bounds(bounds);
    }

    private void OnDisable()
    {
      ((MeshFilter) ((Component) this).GetComponent<MeshFilter>()).get_sharedMesh().RecalculateBounds();
    }

    private void OnValidate()
    {
      Mesh sharedMesh = ((MeshFilter) ((Component) this).GetComponent<MeshFilter>()).get_sharedMesh();
      sharedMesh.RecalculateBounds();
      Bounds bounds = sharedMesh.get_bounds();
      ((Bounds) ref bounds).Expand(new Vector3(this.Expand_XZ, this.Expand_Y, this.Expand_XZ));
      sharedMesh.set_bounds(bounds);
    }

    private void OnDrawGizmosSelected()
    {
      this.rend = (Renderer) ((Component) this).GetComponent<Renderer>();
      Bounds bounds1 = this.rend.get_bounds();
      Vector3 center = ((Bounds) ref bounds1).get_center();
      Bounds bounds2 = this.rend.get_bounds();
      ((Bounds) ref bounds2).get_extents();
      Gizmos.set_color(Color.get_red());
      Vector3 vector3 = center;
      Bounds bounds3 = this.rend.get_bounds();
      Vector3 size = ((Bounds) ref bounds3).get_size();
      Gizmos.DrawWireCube(vector3, size);
    }
  }
}
