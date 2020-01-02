// Decompiled with JetBrains decompiler
// Type: YureCtrlEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class YureCtrlEx
{
  private YureCtrl.BreastShapeInfo[] _shapeInfo = new YureCtrl.BreastShapeInfo[2];
  public bool isInit;
  private ChaControl chara;
  private const bool _defaultNip = true;

  public Dictionary<string, YureCtrl.Info> dicInfo { get; } = new Dictionary<string, YureCtrl.Info>();

  private static bool[] _defaultYure { get; } = new bool[7]
  {
    true,
    true,
    true,
    true,
    true,
    true,
    true
  };

  public bool Init(ChaControl _female)
  {
    this.chara = _female;
    for (int index = 0; index < this._shapeInfo.Length; ++index)
      this._shapeInfo[index].MemberInit();
    this.isInit = false;
    return true;
  }

  public bool Release()
  {
    this.isInit = false;
    this.dicInfo.Clear();
    return true;
  }

  private static int[] _bustIndexes { get; } = new int[7]
  {
    2,
    3,
    4,
    5,
    6,
    7,
    8
  };

  public bool Load(string bundle, string asset)
  {
    this.isInit = false;
    this.dicInfo.Clear();
    if (Object.op_Inequality((Object) this.chara, (Object) null))
      this.ResetShape();
    if (asset.IsNullOrEmpty())
      return false;
    this.LoadParam(AssetBundleManager.LoadAsset(bundle, asset, typeof (MotionBustCtrlData), (string) null).GetAsset<MotionBustCtrlData>().param);
    AssetBundleManager.UnloadAssetBundle(bundle, true, (string) null, false);
    this.isInit = true;
    return true;
  }

  public bool LoadParam(List<MotionBustCtrlData.Param> _param)
  {
    foreach (MotionBustCtrlData.Param obj in _param)
    {
      if (!obj.State.IsNullOrEmpty())
      {
        string state = obj.State;
        YureCtrl.Info info;
        if (!this.dicInfo.TryGetValue(state, out info))
          this.dicInfo[state] = info = new YureCtrl.Info();
        int num1 = 0;
        bool[] aIsActive1 = info.aIsActive;
        List<string> parameters1 = obj.Parameters;
        int index1 = num1;
        int num2 = index1 + 1;
        int num3 = parameters1.GetElement<string>(index1) == "1" ? 1 : 0;
        aIsActive1[0] = num3 != 0;
        info.aBreastShape[0].MemberInit();
        for (int index2 = 0; index2 < YureCtrlEx._bustIndexes.Length; ++index2)
          info.aBreastShape[0].breast[index2] = obj.Parameters.GetElement<string>(num2++) == "1";
        ref YureCtrl.BreastShapeInfo local1 = ref info.aBreastShape[0];
        List<string> parameters2 = obj.Parameters;
        int index3 = num2;
        int num4 = index3 + 1;
        int num5 = parameters2.GetElement<string>(index3) == "1" ? 1 : 0;
        local1.nip = num5 != 0;
        bool[] aIsActive2 = info.aIsActive;
        List<string> parameters3 = obj.Parameters;
        int index4 = num4;
        int num6 = index4 + 1;
        int num7 = parameters3.GetElement<string>(index4) == "1" ? 1 : 0;
        aIsActive2[1] = num7 != 0;
        info.aBreastShape[1].MemberInit();
        for (int index2 = 0; index2 < YureCtrlEx._bustIndexes.Length; ++index2)
          info.aBreastShape[1].breast[index2] = obj.Parameters.GetElement<string>(num6++) == "1";
        ref YureCtrl.BreastShapeInfo local2 = ref info.aBreastShape[1];
        List<string> parameters4 = obj.Parameters;
        int index5 = num6;
        int num8 = index5 + 1;
        int num9 = parameters4.GetElement<string>(index5) == "1" ? 1 : 0;
        local2.nip = num9 != 0;
        bool[] aIsActive3 = info.aIsActive;
        List<string> parameters5 = obj.Parameters;
        int index6 = num8;
        int num10 = index6 + 1;
        int num11 = parameters5.GetElement<string>(index6) == "1" ? 1 : 0;
        aIsActive3[2] = num11 != 0;
        bool[] aIsActive4 = info.aIsActive;
        List<string> parameters6 = obj.Parameters;
        int index7 = num10;
        int num12 = index7 + 1;
        int num13 = parameters6.GetElement<string>(index7) == "1" ? 1 : 0;
        aIsActive4[3] = num13 != 0;
      }
    }
    return true;
  }

  public bool Proc(string _animation)
  {
    YureCtrl.Info info;
    if (!this.isInit || !this.dicInfo.TryGetValue(_animation, out info))
      return false;
    this.Active(info);
    this.Shape(info);
    return true;
  }

  private void Active(YureCtrl.Info info)
  {
    bool[] aIsActive = info.aIsActive;
    for (int area = 0; area < aIsActive.Length; ++area)
      this.chara.playDynamicBoneBust(area, aIsActive[area]);
  }

  private void Shape(YureCtrl.Info info)
  {
    YureCtrl.BreastShapeInfo[] aBreastShape = info.aBreastShape;
    for (int index = 0; index < 2; ++index)
    {
      YureCtrl.BreastShapeInfo breastShapeInfo1 = aBreastShape[index];
      YureCtrl.BreastShapeInfo breastShapeInfo2 = this._shapeInfo[index];
      for (int id = 0; id < breastShapeInfo1.breast.Length; ++id)
      {
        bool flag1 = breastShapeInfo1.breast[id];
        bool flag2 = breastShapeInfo2.breast[id];
        if (flag1 != flag2)
        {
          if (flag1)
            this.chara.DisableShapeBodyID(index != 0 ? 1 : 0, id, false);
          else
            this.chara.DisableShapeBodyID(index != 0 ? 1 : 0, id, true);
        }
        this._shapeInfo[index].breast[id] = flag1;
      }
      if (breastShapeInfo1.nip != breastShapeInfo2.nip)
      {
        if (breastShapeInfo1.nip)
          this.chara.DisableShapeBodyID(index != 0 ? 1 : 0, 7, false);
        else
          this.chara.DisableShapeBodyID(index != 0 ? 1 : 0, 7, false);
        this._shapeInfo[index].nip = breastShapeInfo1.nip;
      }
    }
  }

  public void ResetShape()
  {
    for (int area = 0; area < 4; ++area)
      this.chara.playDynamicBoneBust(area, true);
    for (int index = 0; index < 2; ++index)
    {
      YureCtrl.BreastShapeInfo breastShapeInfo = this._shapeInfo[index];
      for (int id = 0; id < YureCtrlEx._defaultYure.Length; ++id)
      {
        bool flag1 = YureCtrlEx._defaultYure[id];
        bool flag2 = breastShapeInfo.breast[id];
        if (flag1 != flag2)
        {
          if (flag1)
            this.chara.DisableShapeBodyID(index != 0 ? 1 : 0, id, false);
          else
            this.chara.DisableShapeBodyID(index != 0 ? 1 : 0, id, true);
        }
        this._shapeInfo[index].breast[id] = flag1;
      }
      if (!breastShapeInfo.nip)
      {
        this.chara.DisableShapeBodyID(index != 0 ? 1 : 0, 7, false);
        this._shapeInfo[index].nip = true;
      }
    }
  }
}
