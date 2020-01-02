// Decompiled with JetBrains decompiler
// Type: ImplicitSurface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ImplicitSurface : ImplicitSurfaceMeshCreaterBase
{
  private bool _bMapsDirty = true;
  public MeshFilter meshFilter;
  public MeshCollider meshCollider;
  protected Vector3[] _positionMap;
  protected float[] _powerMap;
  protected float[] _powerMapMask;
  protected int _countX;
  protected int _countY;
  protected int _countZ;

  public MeshFilter MeshFilter
  {
    get
    {
      if (Object.op_Equality((Object) this.meshFilter, (Object) null))
        this.meshFilter = (MeshFilter) ((Component) this).GetComponent<MeshFilter>();
      return this.meshFilter;
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

  protected void ResetMaps()
  {
    int maxGridCellCount = MetaballBuilder.MaxGridCellCount;
    float num1 = !this.bAutoGridSize ? this.gridSize : Mathf.Pow((float) (((Bounds) ref this.fixedBounds).get_size().x * ((Bounds) ref this.fixedBounds).get_size().y * ((Bounds) ref this.fixedBounds).get_size().z) / (float) (int) ((double) maxGridCellCount * (double) Mathf.Clamp01(this.autoGridQuarity)), 0.3333333f);
    int num2 = Mathf.CeilToInt((float) ((Bounds) ref this.fixedBounds).get_extents().x / num1) + 1;
    int num3 = Mathf.CeilToInt((float) ((Bounds) ref this.fixedBounds).get_extents().y / num1) + 1;
    int num4 = Mathf.CeilToInt((float) ((Bounds) ref this.fixedBounds).get_extents().z / num1) + 1;
    this._countX = num2 * 2;
    this._countY = num3 * 2;
    this._countZ = num4 * 2;
    Vector3 vector3_1;
    ((Vector3) ref vector3_1).\u002Ector((float) num2 * num1, (float) num3 * num1, (float) num4 * num1);
    Vector3 vector3_2 = Vector3.op_Subtraction(((Bounds) ref this.fixedBounds).get_center(), vector3_1);
    int countX = this._countX;
    int num5 = this._countX * this._countY;
    int length = this._countX * this._countY * this._countZ;
    this._positionMap = new Vector3[length];
    this._powerMap = new float[length];
    this._powerMapMask = new float[length];
    for (int index = 0; index < length; ++index)
      this._powerMap[index] = 0.0f;
    for (int index1 = 0; index1 < this._countZ; ++index1)
    {
      for (int index2 = 0; index2 < this._countY; ++index2)
      {
        for (int index3 = 0; index3 < this._countX; ++index3)
        {
          int index4 = index3 + index2 * countX + index1 * num5;
          this._positionMap[index4] = Vector3.op_Addition(vector3_2, new Vector3(num1 * (float) index3, num1 * (float) index2, num1 * (float) index1));
          this._powerMapMask[index4] = index1 == 0 || index1 == this._countZ - 1 || (index2 == 0 || index2 == this._countY - 1) || (index3 == 0 || index3 == this._countX - 1) ? 0.0f : 1f;
        }
      }
    }
    this.InitializePowerMap();
    this._bMapsDirty = false;
  }

  protected virtual void InitializePowerMap()
  {
    int num = this._countX * this._countY * this._countZ;
    for (int index = 0; index < num; ++index)
      this._powerMap[index] = 0.0f;
  }

  public override void CreateMesh()
  {
    if (this._bMapsDirty)
      this.ResetMaps();
    Vector3 uDir;
    Vector3 vDir;
    Vector3 offset;
    this.GetUVBaseVector(out uDir, out vDir, out offset);
    Mesh implicitSurfaceMesh = MetaballBuilder.Instance.CreateImplicitSurfaceMesh(this._countX, this._countY, this._countZ, this._positionMap, this._powerMap, this.bReverse, this.powerThreshold, uDir, vDir, offset);
    implicitSurfaceMesh.RecalculateBounds();
    this.Mesh = implicitSurfaceMesh;
    if (!Object.op_Inequality((Object) this.meshCollider, (Object) null))
      return;
    this.meshCollider.set_sharedMesh(implicitSurfaceMesh);
  }

  private void OnDrawGizmos()
  {
    Gizmos.set_color(Color.get_white());
    Gizmos.DrawWireCube(Vector3.op_Addition(((Bounds) ref this.fixedBounds).get_center(), ((Component) this).get_transform().get_position()), ((Bounds) ref this.fixedBounds).get_size());
  }
}
