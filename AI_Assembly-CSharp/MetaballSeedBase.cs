// Decompiled with JetBrains decompiler
// Type: MetaballSeedBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class MetaballSeedBase : ImplicitSurfaceMeshCreaterBase
{
  public float baseRadius = 1f;
  [SerializeField]
  private GameObject[] _boneNodes = new GameObject[0];
  public Transform boneRoot;
  public MetaballNode sourceRoot;
  public MetaballCellObject cellObjPrefab;
  public bool bUseFixedBounds;
  protected string _errorMsg;

  public abstract bool IsTreeShape { get; }

  private void OnDrawGizmos()
  {
    if (!this.bUseFixedBounds)
      return;
    Gizmos.set_color(Color.get_white());
    Gizmos.DrawWireCube(Vector3.op_Addition(((Bounds) ref this.fixedBounds).get_center(), ((Component) this).get_transform().get_position()), ((Bounds) ref this.fixedBounds).get_size());
  }

  protected void EnumBoneNodes()
  {
    List<GameObject> list = new List<GameObject>();
    this.EnumerateGameObjects(((Component) this.boneRoot).get_gameObject(), list);
    this._boneNodes = list.ToArray();
  }

  private void EnumerateGameObjects(GameObject parent, List<GameObject> list)
  {
    for (int index = 0; index < parent.get_transform().get_childCount(); ++index)
    {
      GameObject gameObject = ((Component) parent.get_transform().GetChild(index)).get_gameObject();
      list.Add(gameObject);
      this.EnumerateGameObjects(gameObject, list);
    }
  }

  protected void CleanupBoneRoot()
  {
    if (this._boneNodes == null)
      this._boneNodes = new GameObject[0];
    int length = this._boneNodes.Length;
    for (int index = 0; index < length; ++index)
    {
      if (!Object.op_Equality((Object) this._boneNodes[index], (Object) null))
      {
        this._boneNodes[index].get_transform().DetachChildren();
        Object.Destroy((Object) this._boneNodes[index]);
      }
    }
  }
}
