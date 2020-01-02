// Decompiled with JetBrains decompiler
// Type: YureCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class YureCtrl : MonoBehaviour
{
  public List<YureCtrl.Info> lstInfo;
  public ChaControl chaFemale;
  public int femaleID;
  public bool isInit;
  [Tooltip("動いているかの確認用")]
  public bool[] aIsActive;
  [Tooltip("動いているかの確認用")]
  public YureCtrl.BreastShapeInfo[] aBreastShape;
  private bool[] aYureEnableActive;
  private YureCtrl.BreastShapeInfo[] aBreastShapeEnable;
  public YureCtrl.YureCtrlMapH CtrlMapH;

  public YureCtrl()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    for (int index = 0; index < 2; ++index)
    {
      this.aBreastShape[index].MemberInit();
      this.aBreastShapeEnable[index].MemberInit();
    }
  }

  private void LateUpdate()
  {
    if (!this.isInit || !Object.op_Implicit((Object) this.chaFemale))
      return;
    this.Proc(this.chaFemale.getAnimatorStateInfo(0));
  }

  public bool Release()
  {
    this.isInit = false;
    if (this.lstInfo != null)
      this.lstInfo.Clear();
    return true;
  }

  public bool Load(int _motionId, int category)
  {
    this.isInit = false;
    for (int index = 0; index < this.aIsActive.Length; ++index)
      this.aIsActive[index] = true;
    for (int index = 0; index < 2; ++index)
      this.aBreastShape[index].MemberInit();
    if (Object.op_Implicit((Object) this.chaFemale))
    {
      for (int index = 0; index < this.aIsActive.Length; ++index)
        this.chaFemale.playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, true);
      ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryEndOfFrame(), 1), (Action<M0>) (_ =>
      {
        ((Behaviour) this.chaFemale.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastL)).set_enabled(false);
        ((Behaviour) this.chaFemale.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastR)).set_enabled(false);
        ((Behaviour) this.chaFemale.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.HipL)).set_enabled(false);
        ((Behaviour) this.chaFemale.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.HipR)).set_enabled(false);
      }));
      for (int id = 0; id < ChaFileDefine.cf_BustShapeMaskID.Length; ++id)
        this.chaFemale.DisableShapeBodyID(2, id, false);
    }
    Dictionary<int, List<YureCtrl.Info>> dictionary = (Dictionary<int, List<YureCtrl.Info>>) null;
    List<YureCtrl.Info> infoList = (List<YureCtrl.Info>) null;
    if (Singleton<Resources>.Instance.HSceneTable.DicDicYure.TryGetValue(category, out dictionary))
      dictionary.TryGetValue(_motionId, out infoList);
    this.lstInfo = infoList == null ? new List<YureCtrl.Info>() : new List<YureCtrl.Info>((IEnumerable<YureCtrl.Info>) infoList);
    this.isInit = true;
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai)
  {
    if (!this.isInit)
      return false;
    YureCtrl.Info info = (YureCtrl.Info) null;
    if (this.lstInfo != null)
    {
      for (int index = 0; index < this.lstInfo.Count; ++index)
      {
        if (((AnimatorStateInfo) ref _ai).IsName(this.lstInfo[index].nameAnimation) && this.lstInfo[index].nFemale == this.femaleID)
        {
          info = this.lstInfo[index];
          break;
        }
      }
    }
    if (info != null)
    {
      this.Active(info.aIsActive);
      this.Shape(info.aBreastShape);
      return true;
    }
    this.Active(this.aYureEnableActive);
    this.Shape(this.aBreastShapeEnable);
    return false;
  }

  private void Active(bool[] _aIsActive)
  {
    for (int index = 0; index < this.aIsActive.Length; ++index)
    {
      if (this.aIsActive[index] != _aIsActive[index])
      {
        this.chaFemale.playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, _aIsActive[index]);
        this.aIsActive[index] = _aIsActive[index];
      }
    }
  }

  private void Shape(YureCtrl.BreastShapeInfo[] _shapeInfo)
  {
    for (int index1 = 0; index1 < 2; ++index1)
    {
      int LR = index1;
      YureCtrl.BreastShapeInfo breastShapeInfo1 = _shapeInfo[index1];
      YureCtrl.BreastShapeInfo breastShapeInfo2 = this.aBreastShape[index1];
      if (breastShapeInfo1.breast != breastShapeInfo2.breast)
      {
        for (int index2 = 0; index2 < ChaFileDefine.cf_BustShapeMaskID.Length - 1; ++index2)
        {
          int id = index2;
          if (breastShapeInfo1.breast[id] != breastShapeInfo2.breast[id])
          {
            if (breastShapeInfo1.breast[id])
              this.chaFemale.DisableShapeBodyID(LR, id, false);
            else
              this.chaFemale.DisableShapeBodyID(LR, id, true);
          }
        }
        breastShapeInfo2.breast = breastShapeInfo1.breast;
      }
      if (breastShapeInfo1.nip != breastShapeInfo2.nip)
      {
        if (breastShapeInfo1.nip)
          this.chaFemale.DisableShapeBodyID(LR, 7, false);
        else
          this.chaFemale.DisableShapeBodyID(LR, 7, true);
        breastShapeInfo2.nip = breastShapeInfo1.nip;
      }
      this.aBreastShape[index1] = breastShapeInfo2;
    }
  }

  public void ResetShape()
  {
    if (Object.op_Equality((Object) this.chaFemale, (Object) null))
      return;
    for (int id = 0; id < ChaFileDefine.cf_BustShapeMaskID.Length; ++id)
      this.chaFemale.DisableShapeBodyID(2, id, false);
    for (int index = 0; index < this.aIsActive.Length; ++index)
    {
      this.aIsActive[index] = true;
      this.chaFemale.playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, true);
    }
  }

  [Serializable]
  public struct BreastShapeInfo
  {
    public bool[] breast;
    public bool nip;

    public void MemberInit()
    {
      this.breast = new bool[7]
      {
        true,
        true,
        true,
        true,
        true,
        true,
        true
      };
      this.nip = true;
    }
  }

  [Serializable]
  public class Info
  {
    public string nameAnimation = string.Empty;
    public bool[] aIsActive = new bool[4];
    public YureCtrl.BreastShapeInfo[] aBreastShape = new YureCtrl.BreastShapeInfo[2];
    public int nFemale;
  }

  [Serializable]
  public class YureCtrlMapH
  {
    public Dictionary<int, List<YureCtrl.Info>> lstInfo = new Dictionary<int, List<YureCtrl.Info>>();
    public Dictionary<int, ChaControl> chaFemale = new Dictionary<int, ChaControl>();
    public Dictionary<int, bool> isInit = new Dictionary<int, bool>();
    public Dictionary<int, bool[]> aIsActive = new Dictionary<int, bool[]>();
    public Dictionary<int, YureCtrl.BreastShapeInfo[]> aBreastShape = new Dictionary<int, YureCtrl.BreastShapeInfo[]>();
    private Dictionary<int, bool[]> aYureEnableActive = new Dictionary<int, bool[]>();
    private Dictionary<int, YureCtrl.BreastShapeInfo[]> aBreastShapeEnable = new Dictionary<int, YureCtrl.BreastShapeInfo[]>();
    [HideInInspector]
    public int femaleID;

    public int AddChaInit(ChaControl female)
    {
      int key1 = this.aBreastShapeEnable.Count;
      if (this.chaFemale.ContainsKey(key1))
      {
        int key2 = 0;
        while (this.chaFemale.ContainsKey(key2))
          ++key2;
        key1 = key2;
      }
      this.chaFemale.Add(key1, female);
      this.aBreastShapeEnable.Add(key1, new YureCtrl.BreastShapeInfo[2]);
      this.aBreastShape.Add(key1, new YureCtrl.BreastShapeInfo[2]);
      for (int index = 0; index < 2; ++index)
      {
        this.aBreastShape[key1][index].MemberInit();
        this.aBreastShapeEnable[key1][index].MemberInit();
      }
      this.aYureEnableActive.Add(key1, new bool[4]
      {
        true,
        true,
        true,
        true
      });
      return key1;
    }

    public void RemoveChaInit(int ID)
    {
      if (this.lstInfo.ContainsKey(ID))
      {
        this.lstInfo[ID] = (List<YureCtrl.Info>) null;
        this.lstInfo.Remove(ID);
      }
      if (this.chaFemale.ContainsKey(ID))
      {
        this.chaFemale[ID] = (ChaControl) null;
        this.chaFemale.Remove(ID);
      }
      if (this.isInit.ContainsKey(ID))
        this.isInit.Remove(ID);
      if (this.aIsActive.ContainsKey(ID))
      {
        this.aIsActive[ID] = (bool[]) null;
        this.aIsActive.Remove(ID);
      }
      if (this.aBreastShape.ContainsKey(ID))
      {
        this.aBreastShape[ID] = (YureCtrl.BreastShapeInfo[]) null;
        this.aBreastShape.Remove(ID);
      }
      if (this.aYureEnableActive.ContainsKey(ID))
      {
        this.aYureEnableActive[ID] = (bool[]) null;
        this.aYureEnableActive.Remove(ID);
      }
      if (!this.aBreastShapeEnable.ContainsKey(ID))
        return;
      this.aBreastShapeEnable[ID] = (YureCtrl.BreastShapeInfo[]) null;
      this.aBreastShapeEnable.Remove(ID);
    }

    public bool AddLoadInfo(int _motionId, int category, int _addID)
    {
      if (!this.isInit.ContainsKey(_addID))
        this.isInit.Add(_addID, false);
      else
        this.isInit[_addID] = false;
      if (!this.lstInfo.ContainsKey(_addID))
        this.lstInfo.Add(_addID, new List<YureCtrl.Info>());
      else
        this.lstInfo[_addID] = new List<YureCtrl.Info>();
      if (!this.aIsActive.ContainsKey(_addID))
        this.aIsActive.Add(_addID, new bool[4]);
      for (int index = 0; index < this.aIsActive[_addID].Length; ++index)
        this.aIsActive[_addID][index] = true;
      if (!this.aBreastShape.ContainsKey(_addID))
        this.aBreastShape.Add(_addID, new YureCtrl.BreastShapeInfo[2]);
      for (int index = 0; index < 2; ++index)
        this.aBreastShape[_addID][index].MemberInit();
      if (Object.op_Implicit((Object) this.chaFemale[_addID]))
      {
        this.chaFemale[_addID].playDynamicBoneBust(0, true);
        this.chaFemale[_addID].playDynamicBoneBust(1, true);
        ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryEndOfFrame(), 1), (Action<M0>) (_ =>
        {
          ((Behaviour) this.chaFemale[_addID].GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastL)).set_enabled(false);
          ((Behaviour) this.chaFemale[_addID].GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastR)).set_enabled(false);
        }));
        for (int index = 0; index < ChaFileDefine.cf_BustShapeMaskID.Length; ++index)
        {
          int id = index;
          this.chaFemale[_addID].DisableShapeBodyID(2, id, false);
        }
      }
      Dictionary<int, List<YureCtrl.Info>> dictionary = (Dictionary<int, List<YureCtrl.Info>>) null;
      Singleton<Resources>.Instance.HSceneTable.DicDicYure.TryGetValue(category, out dictionary);
      if (dictionary != null)
      {
        List<YureCtrl.Info> infoList = new List<YureCtrl.Info>();
        dictionary.TryGetValue(_motionId, out infoList);
        this.lstInfo[_addID] = infoList;
      }
      this.isInit[_addID] = true;
      return true;
    }

    public bool Proc(AnimatorStateInfo _ai, int _id)
    {
      if (!this.isInit[_id])
        return false;
      YureCtrl.Info info = (YureCtrl.Info) null;
      if (this.lstInfo != null && this.lstInfo[_id] != null)
      {
        for (int index = 0; index < this.lstInfo[_id].Count; ++index)
        {
          if (((AnimatorStateInfo) ref _ai).IsName(this.lstInfo[_id][index].nameAnimation) && this.lstInfo[_id][index].nFemale == 0)
          {
            info = this.lstInfo[_id][index];
            break;
          }
        }
      }
      if (info != null)
      {
        this.Active(info.aIsActive, _id);
        this.Shape(info.aBreastShape, _id);
        return true;
      }
      this.Active(this.aYureEnableActive[_id], _id);
      this.Shape(this.aBreastShapeEnable[_id], _id);
      return false;
    }

    private void Active(bool[] _aIsActive, int _id)
    {
      for (int index = 0; index < this.aIsActive[_id].Length; ++index)
      {
        if (this.aIsActive[_id][index] != _aIsActive[index])
        {
          this.chaFemale[_id].playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, _aIsActive[index]);
          this.aIsActive[_id][index] = _aIsActive[index];
        }
      }
    }

    private void Shape(YureCtrl.BreastShapeInfo[] _shapeInfo, int _id)
    {
      for (int index1 = 0; index1 < 2; ++index1)
      {
        int LR = index1;
        YureCtrl.BreastShapeInfo breastShapeInfo1 = _shapeInfo[index1];
        YureCtrl.BreastShapeInfo breastShapeInfo2 = this.aBreastShape[_id][index1];
        if (breastShapeInfo1.breast != breastShapeInfo2.breast)
        {
          for (int index2 = 0; index2 < ChaFileDefine.cf_BustShapeMaskID.Length - 1; ++index2)
          {
            int id = index2;
            if (breastShapeInfo1.breast[id] != breastShapeInfo2.breast[id])
            {
              if (breastShapeInfo1.breast[id])
                this.chaFemale[_id].DisableShapeBodyID(LR, id, false);
              else
                this.chaFemale[_id].DisableShapeBodyID(LR, id, true);
            }
          }
          breastShapeInfo2.breast = breastShapeInfo1.breast;
        }
        if (breastShapeInfo1.nip != breastShapeInfo2.nip)
        {
          if (breastShapeInfo1.nip)
            this.chaFemale[_id].DisableShapeBodyID(LR, 7, false);
          else
            this.chaFemale[_id].DisableShapeBodyID(LR, 7, true);
          breastShapeInfo2.nip = breastShapeInfo1.nip;
        }
        this.aBreastShape[_id][index1] = breastShapeInfo2;
      }
    }

    public void ResetShape(int _id)
    {
      if (this.chaFemale == null)
        return;
      for (int index = 0; index < ChaFileDefine.cf_BustShapeMaskID.Length; ++index)
      {
        int id = index;
        if (this.chaFemale.ContainsKey(_id))
          this.chaFemale[_id].DisableShapeBodyID(2, id, false);
      }
      if (!this.aIsActive.ContainsKey(_id))
        return;
      for (int index = 0; index < this.aIsActive[_id].Length; ++index)
      {
        this.aIsActive[_id][index] = true;
        if (this.chaFemale.ContainsKey(_id))
          this.chaFemale[_id].playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, true);
      }
    }
  }
}
