// Decompiled with JetBrains decompiler
// Type: ImplicitSurfaceMeshCreaterBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public abstract class ImplicitSurfaceMeshCreaterBase : MonoBehaviour
{
  public float gridSize;
  [Tooltip("Ignore gridSize and use automatically determined value by autoGridQuarity")]
  public bool bAutoGridSize;
  [Range(0.005f, 1f)]
  public float autoGridQuarity;
  public MetaballUVGuide uvProjectNode;
  public float powerThreshold;
  public bool bReverse;
  public Bounds fixedBounds;

  protected ImplicitSurfaceMeshCreaterBase()
  {
    base.\u002Ector();
  }

  public float GridSize
  {
    get
    {
      return Mathf.Max(float.Epsilon, this.gridSize);
    }
  }

  public abstract void CreateMesh();

  public abstract Mesh Mesh { get; set; }

  protected virtual void Update()
  {
  }

  protected void GetUVBaseVector(out Vector3 uDir, out Vector3 vDir, out Vector3 offset)
  {
    if (Object.op_Inequality((Object) this.uvProjectNode, (Object) null))
    {
      float num1 = Mathf.Max(this.uvProjectNode.uScale, 1f / 1000f);
      float num2 = Mathf.Max(this.uvProjectNode.vScale, 1f / 1000f);
      uDir = Vector3.op_Division(((Component) this.uvProjectNode).get_transform().get_right(), num1);
      vDir = Vector3.op_Division(((Component) this.uvProjectNode).get_transform().get_up(), num2);
      offset = Vector3.op_UnaryNegation(((Component) this.uvProjectNode).get_transform().get_localPosition());
    }
    else
    {
      uDir = Vector3.get_right();
      vDir = Vector3.get_up();
      offset = Vector3.get_zero();
    }
  }
}
