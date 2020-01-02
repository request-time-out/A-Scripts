// Decompiled with JetBrains decompiler
// Type: StaticMetaballSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class StaticMetaballSeed : MetaballSeedBase
{
  public MeshFilter meshFilter;
  private MetaballCellCluster _cellCluster;

  private void ConstructCellCluster(
    MetaballCellCluster cluster,
    Transform parentNode,
    Matrix4x4 toLocalMtx,
    Transform meshTrans)
  {
    for (int index = 0; index < parentNode.get_childCount(); ++index)
    {
      Transform child = parentNode.GetChild(index);
      MetaballNode component = (MetaballNode) ((Component) child).GetComponent<MetaballNode>();
      if (Object.op_Inequality((Object) component, (Object) null))
        this._cellCluster.AddCell(meshTrans.InverseTransformPoint(child.get_position()), 0.0f, new float?(component.Radius), ((Object) ((Component) child).get_gameObject()).get_name()).density = component.Density;
      this.ConstructCellCluster(cluster, child, toLocalMtx, meshTrans);
    }
  }

  private void WorldPositionBounds(Transform parentNode, ref Bounds bounds)
  {
    for (int index1 = 0; index1 < parentNode.get_childCount(); ++index1)
    {
      Transform child = parentNode.GetChild(index1);
      MetaballNode component = (MetaballNode) ((Component) child).GetComponent<MetaballNode>();
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          Vector3 position1 = ((Component) child).get_transform().get_position();
          double num1 = (double) ((Vector3) ref position1).get_Item(index2) - (double) component.Radius;
          Vector3 min1 = ((Bounds) ref bounds).get_min();
          double num2 = (double) ((Vector3) ref min1).get_Item(index2);
          if (num1 < num2)
          {
            Vector3 min2 = ((Bounds) ref bounds).get_min();
            ref Vector3 local = ref min2;
            int num3 = index2;
            Vector3 position2 = ((Component) child).get_transform().get_position();
            double num4 = (double) ((Vector3) ref position2).get_Item(index2) - (double) component.Radius;
            ((Vector3) ref local).set_Item(num3, (float) num4);
            ((Bounds) ref bounds).set_min(min2);
          }
          Vector3 position3 = ((Component) child).get_transform().get_position();
          double num5 = (double) ((Vector3) ref position3).get_Item(index2) + (double) component.Radius;
          Vector3 max1 = ((Bounds) ref bounds).get_max();
          double num6 = (double) ((Vector3) ref max1).get_Item(index2);
          if (num5 > num6)
          {
            Vector3 max2 = ((Bounds) ref bounds).get_max();
            ref Vector3 local = ref max2;
            int num3 = index2;
            Vector3 position2 = ((Component) child).get_transform().get_position();
            double num4 = (double) ((Vector3) ref position2).get_Item(index2) + (double) component.Radius;
            ((Vector3) ref local).set_Item(num3, (float) num4);
            ((Bounds) ref bounds).set_max(max2);
          }
        }
      }
      this.WorldPositionBounds(child, ref bounds);
    }
  }

  private bool WorldPositionBoundsFirst(Transform parentNode, ref Bounds bounds)
  {
    for (int index1 = 0; index1 < parentNode.get_childCount(); ++index1)
    {
      Transform child = parentNode.GetChild(index1);
      MetaballNode component = (MetaballNode) ((Component) child).GetComponent<MetaballNode>();
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          Vector3 position = ((Component) child).get_transform().get_position();
          float num = ((Vector3) ref position).get_Item(index2) - component.Radius;
          Vector3 min = ((Bounds) ref bounds).get_min();
          ((Vector3) ref min).set_Item(index2, num);
          ((Bounds) ref bounds).set_min(min);
          Vector3 max = ((Bounds) ref bounds).get_max();
          ((Vector3) ref max).set_Item(index2, num);
          ((Bounds) ref bounds).set_max(max);
        }
        return true;
      }
      if (this.WorldPositionBoundsFirst(child, ref bounds))
        return true;
    }
    return false;
  }

  [ContextMenu("CreateMesh")]
  public override void CreateMesh()
  {
    this.CleanupBoneRoot();
    this._cellCluster = new MetaballCellCluster();
    Bounds bounds;
    ((Bounds) ref bounds).\u002Ector(Vector3.get_zero(), Vector3.get_zero());
    this.WorldPositionBoundsFirst(((Component) this.sourceRoot).get_transform(), ref bounds);
    this.WorldPositionBounds(((Component) this.sourceRoot).get_transform(), ref bounds);
    ((Component) this.meshFilter).get_transform().set_position(((Bounds) ref bounds).get_center());
    this.ConstructCellCluster(this._cellCluster, ((Component) this.sourceRoot).get_transform(), ((Component) this.meshFilter).get_transform().get_worldToLocalMatrix(), ((Component) this.meshFilter).get_transform());
    Vector3 uDir;
    Vector3 vDir;
    Vector3 offset;
    this.GetUVBaseVector(out uDir, out vDir, out offset);
    Bounds? fixedBounds = new Bounds?();
    if (this.bUseFixedBounds)
      fixedBounds = new Bounds?(this.fixedBounds);
    Mesh out_mesh;
    this._errorMsg = MetaballBuilder.Instance.CreateMesh((MetaballCellClusterInterface) this._cellCluster, ((Component) this.boneRoot).get_transform(), this.powerThreshold, this.GridSize, uDir, vDir, offset, out out_mesh, this.cellObjPrefab, this.bReverse, fixedBounds, this.bAutoGridSize, this.autoGridQuarity);
    if (!string.IsNullOrEmpty(this._errorMsg))
    {
      Debug.LogError((object) ("MetaballError : " + this._errorMsg));
    }
    else
    {
      out_mesh.RecalculateBounds();
      this.meshFilter.set_sharedMesh(out_mesh);
      this.EnumBoneNodes();
    }
  }

  public override Mesh Mesh
  {
    get
    {
      return this.meshFilter.get_sharedMesh();
    }
    set
    {
      this.meshFilter.set_sharedMesh(value);
    }
  }

  public override bool IsTreeShape
  {
    get
    {
      return false;
    }
  }
}
