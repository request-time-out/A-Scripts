// Decompiled with JetBrains decompiler
// Type: SkinnedMetaballSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class SkinnedMetaballSeed : MetaballSeedBase
{
  public SkinnedMeshRenderer skinnedMesh;
  private SkinnedMetaballCell _rootCell;

  [ContextMenu("CreateMesh")]
  public override void CreateMesh()
  {
    this.CleanupBoneRoot();
    this._rootCell = new SkinnedMetaballCell();
    this._rootCell.radius = this.sourceRoot.Radius;
    this._rootCell.tag = ((Object) ((Component) this.sourceRoot).get_gameObject()).get_name();
    this._rootCell.density = this.sourceRoot.Density;
    this._rootCell.modelPosition = Vector3.op_Subtraction(((Component) this.sourceRoot).get_transform().get_position(), ((Component) this).get_transform().get_position());
    this.ConstructTree(((Component) this.sourceRoot).get_transform(), this._rootCell, ((Component) this.skinnedMesh).get_transform().get_worldToLocalMatrix());
    Vector3 uDir;
    Vector3 vDir;
    Vector3 offset;
    this.GetUVBaseVector(out uDir, out vDir, out offset);
    Bounds? fixedBounds = new Bounds?();
    if (this.bUseFixedBounds)
      fixedBounds = new Bounds?(this.fixedBounds);
    Mesh out_mesh;
    Transform[] out_bones;
    this._errorMsg = MetaballBuilder.Instance.CreateMeshWithSkeleton(this._rootCell, ((Component) this.boneRoot).get_transform(), this.powerThreshold, this.GridSize, uDir, vDir, offset, out out_mesh, out out_bones, this.cellObjPrefab, this.bReverse, fixedBounds, this.bAutoGridSize, this.autoGridQuarity);
    if (!string.IsNullOrEmpty(this._errorMsg))
    {
      Debug.LogError((object) ("MetaballError : " + this._errorMsg));
    }
    else
    {
      out_mesh.set_bounds(new Bounds(Vector3.get_zero(), Vector3.op_Multiply(Vector3.get_one(), 500f)));
      this.skinnedMesh.set_bones(out_bones);
      this.skinnedMesh.set_sharedMesh(out_mesh);
      this.skinnedMesh.set_localBounds(new Bounds(Vector3.get_zero(), Vector3.op_Multiply(Vector3.get_one(), 500f)));
      this.skinnedMesh.set_rootBone(this.boneRoot);
      this.EnumBoneNodes();
    }
  }

  private void ConstructTree(Transform node, SkinnedMetaballCell cell, Matrix4x4 toLocalMtx)
  {
    for (int index = 0; index < node.get_childCount(); ++index)
    {
      Transform child = node.GetChild(index);
      MetaballNode component = (MetaballNode) ((Component) child).GetComponent<MetaballNode>();
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        SkinnedMetaballCell cell1 = cell.AddChild(Vector4.op_Implicit(Matrix4x4.op_Multiply(toLocalMtx, Vector4.op_Implicit(Vector3.op_Subtraction(((Component) child).get_transform().get_position(), ((Component) this).get_transform().get_position())))), component.Radius, 0.0f);
        cell1.tag = ((Object) ((Component) child).get_gameObject()).get_name();
        cell1.density = component.Density;
        this.ConstructTree(child, cell1, toLocalMtx);
      }
    }
  }

  public override Mesh Mesh
  {
    get
    {
      return this.skinnedMesh.get_sharedMesh();
    }
    set
    {
      this.skinnedMesh.set_sharedMesh(value);
    }
  }

  public override bool IsTreeShape
  {
    get
    {
      return true;
    }
  }
}
