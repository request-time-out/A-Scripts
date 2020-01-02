// Decompiled with JetBrains decompiler
// Type: CollisionCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCtrl : MonoBehaviour
{
  public List<CollisionCtrl.CollisionInfo> lstInfo;
  public List<GameObject> lstObj;
  public ChaControl chaFemale;
  [DisabledGroup("表示")]
  public bool isActive;

  public CollisionCtrl()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.chaFemale))
      return;
    this.Proc(this.chaFemale.getAnimatorStateInfo(0));
  }

  public bool Init(ChaControl _female, GameObject _objHitHead, GameObject _objHitBody)
  {
    this.Release();
    this.chaFemale = _female;
    List<GameObject> collisionComponent = this.GetHeadCollisionComponent(_objHitHead);
    if (collisionComponent != null)
      this.lstObj.AddRange((IEnumerable<GameObject>) collisionComponent);
    else
      this.lstObj.Add((GameObject) null);
    if (Object.op_Implicit((Object) _objHitBody))
    {
      HitCollision componentInChildren = (HitCollision) _objHitBody.GetComponentInChildren<HitCollision>();
      if (Object.op_Implicit((Object) componentInChildren))
        this.lstObj.AddRange((IEnumerable<GameObject>) componentInChildren.lstObj);
    }
    return true;
  }

  private List<GameObject> GetHeadCollisionComponent(GameObject _objHitHead)
  {
    if (Object.op_Equality((Object) _objHitHead, (Object) null))
      return (List<GameObject>) null;
    HitCollision componentInChildren = (HitCollision) _objHitHead.GetComponentInChildren<HitCollision>();
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return (List<GameObject>) null;
    return componentInChildren.lstObj.Count == 0 ? (List<GameObject>) null : componentInChildren.lstObj;
  }

  public void Release()
  {
    this.lstObj.Clear();
    this.lstInfo = new List<CollisionCtrl.CollisionInfo>();
    this.isActive = true;
  }

  public void LoadExcel(string _file)
  {
    if (_file == string.Empty || Singleton<Resources>.Instance.HSceneTable.DicLstCollisionInfo.TryGetValue(_file, out this.lstInfo))
      return;
    this.lstInfo = new List<CollisionCtrl.CollisionInfo>();
  }

  private bool Proc(AnimatorStateInfo _ai)
  {
    for (int index1 = 0; index1 < this.lstInfo.Count; ++index1)
    {
      if (((AnimatorStateInfo) ref _ai).IsName(this.lstInfo[index1].nameAnimation.ToString()))
      {
        this.Visible(true);
        for (int index2 = 0; index2 < this.lstObj.Count; ++index2)
        {
          if (!Object.op_Equality((Object) this.lstObj[index2], (Object) null) && this.lstObj[index2].get_activeSelf() != this.lstInfo[index1].lstIsActive[index2])
            this.lstObj[index2].SetActive(this.lstInfo[index1].lstIsActive[index2]);
        }
        return true;
      }
    }
    this.Visible(false);
    return false;
  }

  private void Visible(bool _visible)
  {
    if (this.isActive == _visible)
      return;
    for (int index = 0; index < this.lstObj.Count; ++index)
    {
      if (Object.op_Implicit((Object) this.lstObj[index]))
        this.lstObj[index].SetActive(_visible);
    }
    this.isActive = _visible;
  }

  [Serializable]
  public struct CollisionInfo
  {
    public string nameAnimation;
    public List<bool> lstIsActive;
  }
}
