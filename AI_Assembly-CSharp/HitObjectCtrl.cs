// Decompiled with JetBrains decompiler
// Type: HitObjectCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HitObjectCtrl
{
  public bool isActive = true;
  public int id = -1;
  private List<string> atariName = new List<string>();
  private List<GameObject> lstObject = new List<GameObject>();
  private List<HitObjectCtrl.CollisionInfo> lstInfo = new List<HitObjectCtrl.CollisionInfo>();
  private Dictionary<int, Dictionary<string, GameObject>> tmpDic = new Dictionary<int, Dictionary<string, GameObject>>();
  private Dictionary<string, GameObject> tmpLst = new Dictionary<string, GameObject>();
  public bool isInit;
  public Transform Place;
  private AnimatorStateInfo ai;
  private string pathAsset;
  private static List<string> lstHitObject;
  private Transform[] getChild;

  [DebuggerHidden]
  public IEnumerator HitObjInit(int Sex, GameObject _objBody, ChaControl _custom)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HitObjectCtrl.\u003CHitObjInit\u003Ec__Iterator0()
    {
      _objBody = _objBody,
      Sex = Sex,
      _custom = _custom,
      \u0024this = this
    };
  }

  private GameObject GetObjParent(Transform objTop, string name)
  {
    this.getChild = (Transform[]) ((Component) objTop).GetComponentsInChildren<Transform>();
    for (int index = 0; index < this.getChild.Length; ++index)
    {
      if (!(((Object) this.getChild[index]).get_name() != name))
        return ((Component) this.getChild[index]).get_gameObject();
    }
    return (GameObject) null;
  }

  public void HitObjLoadExcel(string _file)
  {
    this.lstInfo = new List<HitObjectCtrl.CollisionInfo>();
    this.atariName = new List<string>();
    if (_file == string.Empty)
      return;
    if (!Singleton<Resources>.Instance.HSceneTable.DicLstHitObjInfo.TryGetValue(_file, out this.lstInfo))
      this.lstInfo = new List<HitObjectCtrl.CollisionInfo>();
    if (Singleton<Resources>.Instance.HSceneTable.HitObjAtariName.TryGetValue(_file, out this.atariName))
      return;
    this.atariName = new List<string>();
  }

  public bool setActiveObject(bool val)
  {
    for (int index = 0; index < this.lstObject.Count; ++index)
    {
      if (!Object.op_Equality((Object) this.lstObject[index], (Object) null) && this.lstObject[index].get_activeSelf() != val)
        this.lstObject[index].SetActive(val);
    }
    this.isActive = val;
    return true;
  }

  public bool ReleaseObject()
  {
    for (int index = 0; index < this.lstObject.Count; ++index)
    {
      if (!Object.op_Equality((Object) this.lstObject[index], (Object) null))
      {
        this.lstObject[index].SetActive(false);
        this.lstObject[index].get_transform().SetParent(this.Place, false);
      }
    }
    this.isInit = false;
    this.isActive = true;
    return true;
  }

  public bool Proc(Animator _anim)
  {
    if (Object.op_Equality((Object) _anim, (Object) null) || Object.op_Equality((Object) _anim.get_runtimeAnimatorController(), (Object) null))
    {
      this.Visible(false);
      return false;
    }
    this.ai = _anim.GetCurrentAnimatorStateInfo(0);
    for (int index1 = 0; index1 < this.lstInfo.Count; ++index1)
    {
      if (((AnimatorStateInfo) ref this.ai).IsName(this.lstInfo[index1].nameAnimation.ToString()))
      {
        this.isActive = true;
        using (List<GameObject>.Enumerator enumerator = this.lstObject.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            for (int index2 = 0; index2 < this.atariName.Count; ++index2)
            {
              if (!(((Object) current).get_name() != this.atariName[index2]) && current.get_activeSelf() != this.lstInfo[index1].lstIsActive[index2])
                current.SetActive(this.lstInfo[index1].lstIsActive[index2]);
            }
          }
        }
        return true;
      }
    }
    this.Visible(false);
    return false;
  }

  private bool Visible(bool _visible)
  {
    if (this.isActive == _visible)
      return false;
    for (int index = 0; index < this.lstObject.Count; ++index)
      this.lstObject[index].SetActive(_visible);
    this.isActive = _visible;
    return false;
  }

  public struct CollisionInfo
  {
    public string nameAnimation;
    public List<bool> lstIsActive;
  }
}
