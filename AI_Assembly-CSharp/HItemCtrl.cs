// Decompiled with JetBrains decompiler
// Type: HItemCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

public class HItemCtrl
{
  private List<HItemCtrl.Item> lstItem = new List<HItemCtrl.Item>();
  private List<Dictionary<int, List<HItemCtrl.ListItem>>>[] lstParent = new List<Dictionary<int, List<HItemCtrl.ListItem>>>[6]
  {
    new List<Dictionary<int, List<HItemCtrl.ListItem>>>(),
    new List<Dictionary<int, List<HItemCtrl.ListItem>>>(),
    new List<Dictionary<int, List<HItemCtrl.ListItem>>>(),
    new List<Dictionary<int, List<HItemCtrl.ListItem>>>(),
    new List<Dictionary<int, List<HItemCtrl.ListItem>>>(),
    new List<Dictionary<int, List<HItemCtrl.ListItem>>>()
  };
  private List<ValueTuple<Transform, Transform, bool>> itemObj = new List<ValueTuple<Transform, Transform, bool>>();
  private List<ValueTuple<string, RuntimeAnimatorController>> BaseRacs = new List<ValueTuple<string, RuntimeAnimatorController>>();
  private RuntimeAnimatorController[] rac = new RuntimeAnimatorController[2];
  private Transform hitemPlace;

  public void HItemInit(Transform _hitemPlace)
  {
    this.lstParent = Singleton<Resources>.Instance.HSceneTable.lstHItemObjInfo;
    this.BaseRacs = Singleton<Resources>.Instance.HSceneTable.lstHItemBase;
    this.hitemPlace = _hitemPlace;
  }

  public bool LoadItem(
    int _mode,
    int _id,
    GameObject _boneMale,
    GameObject _boneFemale,
    GameObject _boneMale1,
    GameObject _boneFemale1)
  {
    this.ReleaseItem();
    List<HItemCtrl.ListItem> listItemList = (List<HItemCtrl.ListItem>) null;
    this.itemObj.Clear();
    for (int index = 0; index < this.lstParent[_mode].Count; ++index)
    {
      if (this.lstParent[_mode][index].ContainsKey(_id))
      {
        listItemList = this.lstParent[_mode][index][_id];
        foreach (HItemCtrl.ListItem _info in listItemList)
        {
          HItemCtrl.Item obj = new HItemCtrl.Item();
          if (GlobalMethod.AssetFileExist(_info.pathAssetObject, _info.nameObject, _info.nameManifest))
          {
            obj.itemName = _info.Name;
            obj.objItem = CommonLib.LoadAsset<GameObject>(_info.pathAssetObject, _info.nameObject, true, _info.nameManifest);
            obj.transItem = obj.objItem.get_transform();
            Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(_info.pathAssetObject);
            this.LoadAnimation(obj, _info);
            this.lstItem.Add(obj);
          }
        }
      }
    }
    if (listItemList == null)
      return false;
    for (int index1 = 0; index1 < listItemList.Count && this.lstItem.Count > index1; ++index1)
    {
      if (!Object.op_Equality((Object) this.lstItem[index1].objItem, (Object) null))
      {
        foreach (HItemCtrl.ParentInfo parentInfo in listItemList[index1].lstParent)
        {
          GameObject gameObject1 = (GameObject) null;
          if (parentInfo.numToWhomParent == 0)
          {
            if (Object.op_Inequality((Object) _boneMale, (Object) null))
              gameObject1 = _boneMale.get_transform().FindLoop(parentInfo.nameParent);
          }
          else if (parentInfo.numToWhomParent == 1)
          {
            if (Object.op_Inequality((Object) _boneFemale, (Object) null))
              gameObject1 = _boneFemale.get_transform().FindLoop(parentInfo.nameParent);
          }
          else if (parentInfo.numToWhomParent == 2)
          {
            if (Object.op_Inequality((Object) _boneMale1, (Object) null))
              gameObject1 = _boneMale1.get_transform().FindLoop(parentInfo.nameParent);
          }
          else if (parentInfo.numToWhomParent == 3)
          {
            if (Object.op_Inequality((Object) _boneFemale1, (Object) null))
              gameObject1 = _boneFemale1.get_transform().FindLoop(parentInfo.nameParent);
          }
          else
          {
            int index2 = parentInfo.numToWhomParent - 4;
            if (this.lstItem.Count > index2 && Object.op_Implicit((Object) this.lstItem[index2].objItem))
              gameObject1 = this.lstItem[index2].transItem.FindLoop(parentInfo.nameParent);
          }
          GameObject gameObject2 = !(parentInfo.nameSelf != string.Empty) ? this.lstItem[index1].objItem : this.lstItem[index1].transItem.FindLoop(parentInfo.nameSelf);
          if (!Object.op_Equality((Object) gameObject1, (Object) null) && !Object.op_Equality((Object) gameObject2, (Object) null))
          {
            HItemCtrl.ChildInfo childInfo = new HItemCtrl.ChildInfo();
            childInfo.objChild = gameObject2;
            childInfo.transChild = gameObject2.get_transform();
            childInfo.oldParent = gameObject2.get_transform().get_parent();
            this.lstItem[index1].lstChild.Add(childInfo);
            if (parentInfo.isParentMode)
            {
              childInfo.transChild.SetParent(gameObject1.get_transform(), false);
              childInfo.transChild.set_localPosition(Vector3.get_zero());
              childInfo.transChild.set_localRotation(Quaternion.get_identity());
            }
            else
            {
              childInfo.transChild.SetParent(this.hitemPlace, false);
              childInfo.transChild.set_position(gameObject1.get_transform().get_position());
              childInfo.transChild.set_rotation(gameObject1.get_transform().get_rotation());
            }
            if (!parentInfo.isParentScale)
              this.itemObj.Add(new ValueTuple<Transform, Transform, bool>(childInfo.transChild, gameObject1.get_transform(), parentInfo.isParentMode));
          }
        }
      }
    }
    GC.Collect();
    return true;
  }

  public void ParentScaleReject()
  {
    if (this.itemObj.Count <= 0)
      return;
    for (int index = 0; index < this.itemObj.Count; ++index)
    {
      if (this.itemObj[index].Item3 != null)
      {
        Vector3 lossyScale = ((Transform) this.itemObj[index].Item1).get_lossyScale();
        Vector3 localScale = ((Transform) this.itemObj[index].Item1).get_localScale();
        ((Transform) this.itemObj[index].Item1).set_localScale(new Vector3((float) (localScale.x / lossyScale.x), (float) (localScale.y / lossyScale.y), (float) (localScale.z / lossyScale.z)));
      }
      else
      {
        ((Transform) this.itemObj[index].Item1).set_position(((Transform) this.itemObj[index].Item2).get_position());
        ((Transform) this.itemObj[index].Item1).set_rotation(((Transform) this.itemObj[index].Item2).get_rotation());
      }
    }
  }

  public bool ReleaseItem()
  {
    for (int index1 = 0; index1 < this.lstItem.Count; ++index1)
    {
      if (!Object.op_Equality((Object) this.lstItem[index1].objItem, (Object) null))
      {
        for (int index2 = 0; index2 < this.lstItem[index1].lstChild.Count; ++index2)
        {
          HItemCtrl.ChildInfo childInfo = this.lstItem[index1].lstChild[index2];
          if (Object.op_Implicit((Object) childInfo.objChild) && Object.op_Implicit((Object) childInfo.oldParent))
            childInfo.transChild.SetParent(childInfo.oldParent, false);
        }
        Object.Destroy((Object) this.lstItem[index1].objItem);
        this.lstItem[index1].objItem = (GameObject) null;
        this.lstItem[index1].animItem = (Animator) null;
      }
    }
    this.lstItem.Clear();
    this.itemObj.Clear();
    return true;
  }

  public bool setTransform(Transform _transform)
  {
    if (Object.op_Equality((Object) _transform, (Object) null))
      return false;
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.objItem, (Object) null) && !Object.op_Inequality((Object) obj.transItem.get_parent(), (Object) null))
      {
        obj.transItem.set_position(_transform.get_position());
        obj.transItem.set_rotation(_transform.get_rotation());
      }
    }
    return true;
  }

  public bool setTransform(Vector3 pos, Vector3 rot)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.objItem, (Object) null) && !Object.op_Inequality((Object) obj.transItem.get_parent(), (Object) null))
      {
        obj.transItem.set_position(pos);
        obj.transItem.set_rotation(Quaternion.Euler(rot));
      }
    }
    return true;
  }

  public void syncPlay(AnimatorStateInfo _ai)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.Play(((AnimatorStateInfo) ref _ai).get_shortNameHash(), 0, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    }
  }

  public void syncPlay(int _nameHash, float _fnormalizedTime)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.Play(_nameHash, 0, _fnormalizedTime);
    }
  }

  public void Update()
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.Update(0.0f);
    }
  }

  public void setPlay(string _strAnmName)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.Play(_strAnmName, 0);
    }
  }

  public void setPlay(string _strAnmName, float normalizeTime)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.Play(_strAnmName, 0, normalizeTime);
    }
  }

  public bool setPlay(string _strAnmName, int _nObj)
  {
    if (this.lstItem.Count <= _nObj || Object.op_Equality((Object) this.lstItem[_nObj].animItem, (Object) null))
      return false;
    this.lstItem[_nObj].animItem.Play(_strAnmName, 0);
    return true;
  }

  public void setAnimatorParamTrigger(string _strAnmName)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.SetTrigger(_strAnmName);
    }
  }

  public void setAnimatorParamResetTrigger(string _strAnmName)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.ResetTrigger(_strAnmName);
    }
  }

  public void setAnimatorParamBool(string _strAnmName, bool _bFlag)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.SetBool(_strAnmName, _bFlag);
    }
  }

  public void setAnimatorParamFloat(string _strAnmName, float _fValue)
  {
    foreach (HItemCtrl.Item obj in this.lstItem)
    {
      if (!Object.op_Equality((Object) obj.animItem, (Object) null))
        obj.animItem.SetFloat(_strAnmName, _fValue);
    }
  }

  public GameObject GetItem()
  {
    return this.lstItem.Count < 1 ? (GameObject) null : this.lstItem[0].objItem;
  }

  public List<HItemCtrl.Item> GetItems()
  {
    return this.lstItem;
  }

  public List<Dictionary<int, List<HItemCtrl.ListItem>>>[] GetListItemInfos()
  {
    return this.lstParent;
  }

  private bool LoadAnimation(HItemCtrl.Item _item, HItemCtrl.ListItem _info)
  {
    if (Object.op_Equality((Object) _item.objItem, (Object) null))
      return false;
    _item.animItem = (Animator) _item.objItem.GetComponent<Animator>();
    if (Object.op_Equality((Object) _item.animItem, (Object) null))
    {
      _item.animItem = (Animator) _item.objItem.GetComponentInChildren<Animator>();
      if (Object.op_Equality((Object) _item.animItem, (Object) null))
        return false;
    }
    if (_info.pathAssetAnimator.IsNullOrEmpty() || _info.nameAnimator.IsNullOrEmpty())
    {
      _item.animItem = (Animator) null;
      return false;
    }
    using (List<ValueTuple<string, RuntimeAnimatorController>>.Enumerator enumerator = this.BaseRacs.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        ValueTuple<string, RuntimeAnimatorController> current = enumerator.Current;
        if (!((string) current.Item1 != _info.nameAnimatorBase))
          this.rac[0] = (RuntimeAnimatorController) current.Item2;
      }
    }
    _item.animItem.set_runtimeAnimatorController(this.rac[0]);
    if (Object.op_Equality((Object) _item.animItem.get_runtimeAnimatorController(), (Object) null))
      _item.animItem = (Animator) null;
    this.rac[1] = CommonLib.LoadAsset<RuntimeAnimatorController>(_info.pathAssetAnimator, _info.nameAnimator, false, string.Empty);
    _item.animItem.set_runtimeAnimatorController((RuntimeAnimatorController) Illusion.Utils.Animator.SetupAnimatorOverrideController(_item.animItem.get_runtimeAnimatorController(), this.rac[1]));
    return true;
  }

  public class ChildInfo
  {
    public GameObject objChild;
    public Transform transChild;
    public Transform oldParent;
  }

  public class Item
  {
    public List<HItemCtrl.ChildInfo> lstChild = new List<HItemCtrl.ChildInfo>();
    public string itemName;
    public GameObject objItem;
    public Transform transItem;
    public Animator animItem;
  }

  public class ParentInfo
  {
    public bool isParentMode;
    public int numToWhomParent;
    public string nameParent;
    public string nameSelf;
    public bool isParentScale;
  }

  public class ListItem
  {
    public List<HItemCtrl.ParentInfo> lstParent = new List<HItemCtrl.ParentInfo>();
    public string Name;
    public int itemkind;
    public int itemID;
    public string nameManifest;
    public string pathAssetObject;
    public string nameObject;
    public string pathAssetAnimatorBase;
    public string nameAnimatorBase;
    public string pathAssetAnimator;
    public string nameAnimator;
  }
}
