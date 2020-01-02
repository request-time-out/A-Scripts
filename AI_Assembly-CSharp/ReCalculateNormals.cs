// Decompiled with JetBrains decompiler
// Type: ReCalculateNormals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class ReCalculateNormals : MonoBehaviour
{
  public ReCalculateNormals()
  {
    base.\u002Ector();
  }

  public void Update()
  {
    MeshFilter meshFilter = new MeshFilter();
    MeshFilter component = ((Component) this).get_gameObject().GetComponent(typeof (MeshFilter)) as MeshFilter;
    Mesh sharedMesh;
    if (Object.op_Implicit((Object) component))
    {
      sharedMesh = component.get_sharedMesh();
    }
    else
    {
      SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
      sharedMesh = (((Component) this).get_gameObject().GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer).get_sharedMesh();
    }
    if (!Object.op_Inequality((Object) null, (Object) sharedMesh))
      return;
    sharedMesh.RecalculateNormals();
  }
}
