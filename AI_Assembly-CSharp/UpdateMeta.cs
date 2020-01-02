// Decompiled with JetBrains decompiler
// Type: UpdateMeta
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UpdateMeta : MonoBehaviour
{
  public List<MetaballShoot> lstShoot;
  [Tooltip("StaticMetaballSeedScript")]
  public StaticMetaballSeed metaseed;
  [Tooltip("StaticMesh")]
  public GameObject objStaticMesh;
  [SerializeField]
  private bool isCreate;
  private WaitForSeconds wait;

  public UpdateMeta()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    for (int index = 0; index < this.lstShoot.Count; ++index)
      this.lstShoot[index].ShootMetaBall();
  }

  private void LateUpdate()
  {
    this.isCreate = false;
    for (int index = 0; index < this.lstShoot.Count; ++index)
    {
      if (this.lstShoot[index].IsCreate())
      {
        this.isCreate = true;
        break;
      }
    }
    if (!Object.op_Implicit((Object) this.metaseed) || !this.isCreate)
      return;
    this.metaseed.CreateMesh();
  }

  private void OnValidate()
  {
  }

  public void ConstMetaMesh()
  {
    bool flag = false;
    for (int index = 0; index < this.lstShoot.Count; ++index)
    {
      if (this.lstShoot[index].isConstMetaMesh)
      {
        flag = true;
        break;
      }
    }
    if (flag && !this.isCreate)
    {
      for (int index = 0; index < this.lstShoot.Count; ++index)
      {
        if (this.lstShoot[index].isEnable && this.lstShoot[index].isConstMetaMesh)
        {
          if (!Object.op_Inequality((Object) this.objStaticMesh.get_transform().get_parent(), (Object) this.lstShoot[index].parentTransform))
            break;
          this.objStaticMesh.get_transform().SetParent(this.lstShoot[index].parentTransform);
          break;
        }
      }
    }
    else
      this.StaticMeshParentInit();
  }

  public void MetaBallClear()
  {
    for (int index = 0; index < this.lstShoot.Count; ++index)
      this.lstShoot[index].MetaBallClear();
    this.StaticMeshParentInit();
    this.StartCoroutine(this.CreateMesh());
  }

  private bool StaticMeshParentInit()
  {
    if (!Object.op_Implicit((Object) this.objStaticMesh))
      return false;
    if (Object.op_Equality((Object) this.objStaticMesh.get_transform().get_parent(), (Object) ((Component) this.metaseed).get_transform()))
      return true;
    this.objStaticMesh.get_transform().SetParent(((Component) this.metaseed).get_transform(), false);
    this.objStaticMesh.get_transform().set_localPosition(Vector3.get_zero());
    this.objStaticMesh.get_transform().set_localRotation(Quaternion.get_identity());
    this.objStaticMesh.get_transform().set_localScale(Vector3.get_one());
    return true;
  }

  [DebuggerHidden]
  private IEnumerator CreateMesh()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new UpdateMeta.\u003CCreateMesh\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
