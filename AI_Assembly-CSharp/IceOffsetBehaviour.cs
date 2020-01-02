// Decompiled with JetBrains decompiler
// Type: IceOffsetBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class IceOffsetBehaviour : MonoBehaviour
{
  public IceOffsetBehaviour()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    FadeInOutShaderFloat component1 = (FadeInOutShaderFloat) ((Component) this).GetComponent<FadeInOutShaderFloat>();
    if (Object.op_Equality((Object) component1, (Object) null))
      return;
    Transform parent = ((Component) this).get_transform().get_parent();
    SkinnedMeshRenderer component2 = (SkinnedMeshRenderer) ((Component) parent).GetComponent<SkinnedMeshRenderer>();
    Mesh sharedMesh;
    if (Object.op_Inequality((Object) component2, (Object) null))
    {
      sharedMesh = component2.get_sharedMesh();
    }
    else
    {
      MeshFilter component3 = (MeshFilter) ((Component) parent).GetComponent<MeshFilter>();
      if (Object.op_Equality((Object) component3, (Object) null))
        return;
      sharedMesh = component3.get_sharedMesh();
    }
    if (!sharedMesh.get_isReadable())
    {
      component1.MaxFloat = 0.0f;
    }
    else
    {
      int length = sharedMesh.get_triangles().Length;
      if (length >= 1000)
        return;
      if (length > 500)
        component1.MaxFloat = (float) ((double) length / 1000.0 - 0.5);
      else
        component1.MaxFloat = 0.0f;
    }
  }

  private void Update()
  {
  }
}
