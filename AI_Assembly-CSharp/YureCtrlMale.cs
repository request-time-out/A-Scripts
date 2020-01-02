// Decompiled with JetBrains decompiler
// Type: YureCtrlMale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class YureCtrlMale : MonoBehaviour
{
  public List<YureCtrlMale.Info> lstInfo;
  public ChaControl chaMale;
  public int MaleID;
  public bool isInit;
  [Tooltip("動いているかの確認用")]
  public bool[] aIsActive;
  [Tooltip("動いているかの確認用")]
  public YureCtrlMale.BreastShapeInfo[] aBreastShape;
  private bool[] aYureEnableActive;
  private YureCtrlMale.BreastShapeInfo[] aBreastShapeEnable;

  public YureCtrlMale()
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
    if (!this.isInit || !Object.op_Implicit((Object) this.chaMale))
      return;
    this.Proc(this.chaMale.getAnimatorStateInfo(0));
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
    if (Object.op_Implicit((Object) this.chaMale))
    {
      for (int index = 0; index < this.aIsActive.Length; ++index)
        this.chaMale.playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, true);
      for (int id = 0; id < ChaFileDefine.cf_BustShapeMaskID.Length; ++id)
        this.chaMale.DisableShapeBodyID(2, id, false);
    }
    Dictionary<int, List<YureCtrlMale.Info>> dictionary = (Dictionary<int, List<YureCtrlMale.Info>>) null;
    List<YureCtrlMale.Info> infoList = (List<YureCtrlMale.Info>) null;
    Singleton<Resources>.Instance.HSceneTable.DicDicYureMale.TryGetValue(category, out dictionary);
    dictionary?.TryGetValue(_motionId, out infoList);
    this.lstInfo = infoList == null ? new List<YureCtrlMale.Info>() : new List<YureCtrlMale.Info>((IEnumerable<YureCtrlMale.Info>) infoList);
    this.isInit = true;
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai)
  {
    if (!this.isInit)
      return false;
    YureCtrlMale.Info info = (YureCtrlMale.Info) null;
    if (this.lstInfo != null)
    {
      for (int index = 0; index < this.lstInfo.Count; ++index)
      {
        if (((AnimatorStateInfo) ref _ai).IsName(this.lstInfo[index].nameAnimation) && this.lstInfo[index].nMale == this.MaleID)
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
        this.chaMale.playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, _aIsActive[index]);
        this.aIsActive[index] = _aIsActive[index];
      }
    }
  }

  private void Shape(YureCtrlMale.BreastShapeInfo[] _shapeInfo)
  {
    for (int index1 = 0; index1 < 2; ++index1)
    {
      int LR = index1;
      YureCtrlMale.BreastShapeInfo breastShapeInfo1 = _shapeInfo[index1];
      YureCtrlMale.BreastShapeInfo breastShapeInfo2 = this.aBreastShape[index1];
      if (breastShapeInfo1.breast != breastShapeInfo2.breast)
      {
        for (int index2 = 0; index2 < ChaFileDefine.cf_BustShapeMaskID.Length - 1; ++index2)
        {
          int id = index2;
          if (breastShapeInfo1.breast[id] != breastShapeInfo2.breast[id])
          {
            if (breastShapeInfo1.breast[id])
              this.chaMale.DisableShapeBodyID(LR, id, false);
            else
              this.chaMale.DisableShapeBodyID(LR, id, true);
          }
        }
        breastShapeInfo2.breast = breastShapeInfo1.breast;
      }
      if (breastShapeInfo1.nip != breastShapeInfo2.nip)
      {
        if (breastShapeInfo1.nip)
          this.chaMale.DisableShapeBodyID(LR, 7, false);
        else
          this.chaMale.DisableShapeBodyID(LR, 7, true);
        breastShapeInfo2.nip = breastShapeInfo1.nip;
      }
      this.aBreastShape[index1] = breastShapeInfo2;
    }
  }

  public void ResetShape()
  {
    if (Object.op_Equality((Object) this.chaMale, (Object) null))
      return;
    for (int id = 0; id < ChaFileDefine.cf_BustShapeMaskID.Length; ++id)
      this.chaMale.DisableShapeBodyID(2, id, false);
    for (int index = 0; index < this.aIsActive.Length; ++index)
    {
      this.aIsActive[index] = true;
      this.chaMale.playDynamicBoneBust((ChaControlDefine.DynamicBoneKind) index, true);
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
    public YureCtrlMale.BreastShapeInfo[] aBreastShape = new YureCtrlMale.BreastShapeInfo[2];
    public int nMale;
  }
}
