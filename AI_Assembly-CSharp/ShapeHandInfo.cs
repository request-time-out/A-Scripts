// Decompiled with JetBrains decompiler
// Type: ShapeHandInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.SetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHandInfo : ShapeInfoBase
{
  public const int UPDATE_MASK_HAND_L = 1;
  public const int UPDATE_MASK_HAND_R = 2;
  public int updateMask;

  public override void InitShapeInfo(
    string manifest,
    string assetBundleAnmKey,
    string assetBundleCategory,
    string anmKeyInfoName,
    string cateInfoName,
    Transform trfObj)
  {
    Dictionary<string, int> dictEnumDst = new Dictionary<string, int>();
    foreach (ShapeHandInfo.DstName dstName in (ShapeHandInfo.DstName[]) Enum.GetValues(typeof (ShapeHandInfo.DstName)))
      dictEnumDst[dstName.ToString()] = (int) dstName;
    Dictionary<string, int> dictEnumSrc = new Dictionary<string, int>();
    foreach (ShapeHandInfo.SrcName srcName in (ShapeHandInfo.SrcName[]) Enum.GetValues(typeof (ShapeHandInfo.SrcName)))
      dictEnumSrc[srcName.ToString()] = (int) srcName;
    this.InitShapeInfoBase(manifest, assetBundleAnmKey, assetBundleCategory, anmKeyInfoName, cateInfoName, trfObj, dictEnumDst, dictEnumSrc, new Action<string, string>(Singleton<Character>.Instance.AddLoadAssetBundle));
    this.InitEnd = true;
  }

  public override void ForceUpdate()
  {
    this.Update();
  }

  public override void Update()
  {
  }

  public override void UpdateAlways()
  {
    if (!this.InitEnd || this.dictSrc.Count == 0)
      return;
    ShapeInfoBase.BoneInfo boneInfo = (ShapeInfoBase.BoneInfo) null;
    if ((this.updateMask & 1) != 0)
    {
      int num1 = 0;
      int num2 = 14;
      for (int key = num1; key <= num2; ++key)
      {
        if (this.dictDst.TryGetValue(key, out boneInfo))
          boneInfo.trfBone.SetLocalRotation((float) this.dictSrc[key].vctRot.x, (float) this.dictSrc[key].vctRot.y, (float) this.dictSrc[key].vctRot.z);
      }
    }
    if ((this.updateMask & 2) == 0)
      return;
    int num3 = 15;
    int num4 = 29;
    for (int key = num3; key <= num4; ++key)
    {
      if (this.dictDst.TryGetValue(key, out boneInfo))
        boneInfo.trfBone.SetLocalRotation((float) this.dictSrc[key].vctRot.x, (float) this.dictSrc[key].vctRot.y, (float) this.dictSrc[key].vctRot.z);
    }
  }

  public enum DstName
  {
    cf_J_Hand_Index01_L,
    cf_J_Hand_Index02_L,
    cf_J_Hand_Index03_L,
    cf_J_Hand_Little01_L,
    cf_J_Hand_Little02_L,
    cf_J_Hand_Little03_L,
    cf_J_Hand_Middle01_L,
    cf_J_Hand_Middle02_L,
    cf_J_Hand_Middle03_L,
    cf_J_Hand_Ring01_L,
    cf_J_Hand_Ring02_L,
    cf_J_Hand_Ring03_L,
    cf_J_Hand_Thumb01_L,
    cf_J_Hand_Thumb02_L,
    cf_J_Hand_Thumb03_L,
    cf_J_Hand_Index01_R,
    cf_J_Hand_Index02_R,
    cf_J_Hand_Index03_R,
    cf_J_Hand_Little01_R,
    cf_J_Hand_Little02_R,
    cf_J_Hand_Little03_R,
    cf_J_Hand_Middle01_R,
    cf_J_Hand_Middle02_R,
    cf_J_Hand_Middle03_R,
    cf_J_Hand_Ring01_R,
    cf_J_Hand_Ring02_R,
    cf_J_Hand_Ring03_R,
    cf_J_Hand_Thumb01_R,
    cf_J_Hand_Thumb02_R,
    cf_J_Hand_Thumb03_R,
  }

  public enum SrcName
  {
    cf_J_Hand_Index01_L,
    cf_J_Hand_Index02_L,
    cf_J_Hand_Index03_L,
    cf_J_Hand_Little01_L,
    cf_J_Hand_Little02_L,
    cf_J_Hand_Little03_L,
    cf_J_Hand_Middle01_L,
    cf_J_Hand_Middle02_L,
    cf_J_Hand_Middle03_L,
    cf_J_Hand_Ring01_L,
    cf_J_Hand_Ring02_L,
    cf_J_Hand_Ring03_L,
    cf_J_Hand_Thumb01_L,
    cf_J_Hand_Thumb02_L,
    cf_J_Hand_Thumb03_L,
    cf_J_Hand_Index01_R,
    cf_J_Hand_Index02_R,
    cf_J_Hand_Index03_R,
    cf_J_Hand_Little01_R,
    cf_J_Hand_Little02_R,
    cf_J_Hand_Little03_R,
    cf_J_Hand_Middle01_R,
    cf_J_Hand_Middle02_R,
    cf_J_Hand_Middle03_R,
    cf_J_Hand_Ring01_R,
    cf_J_Hand_Ring02_R,
    cf_J_Hand_Ring03_R,
    cf_J_Hand_Thumb01_R,
    cf_J_Hand_Thumb02_R,
    cf_J_Hand_Thumb03_R,
  }
}
