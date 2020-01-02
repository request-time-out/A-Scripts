// Decompiled with JetBrains decompiler
// Type: ShapeInfoSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.SetUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeInfoSample : ShapeInfoBase
{
  private bool initEnd;

  public override void InitShapeInfo(
    string manifest,
    string assetBundleAnmKey,
    string assetBundleCategory,
    string resAnmKeyInfoPath,
    string resCateInfoPath,
    Transform trfObj)
  {
    Dictionary<string, int> dictEnumDst = new Dictionary<string, int>();
    foreach (ShapeInfoSample.DstBoneName dstBoneName in (ShapeInfoSample.DstBoneName[]) Enum.GetValues(typeof (ShapeInfoSample.DstBoneName)))
      dictEnumDst[dstBoneName.ToString()] = (int) dstBoneName;
    Dictionary<string, int> dictEnumSrc = new Dictionary<string, int>();
    foreach (ShapeInfoSample.SrcBoneName srcBoneName in (ShapeInfoSample.SrcBoneName[]) Enum.GetValues(typeof (ShapeInfoSample.SrcBoneName)))
      dictEnumSrc[srcBoneName.ToString()] = (int) srcBoneName;
    this.InitShapeInfoBase(manifest, assetBundleAnmKey, assetBundleCategory, resAnmKeyInfoPath, resCateInfoPath, trfObj, dictEnumDst, dictEnumSrc, (Action<string, string>) null);
    this.initEnd = true;
  }

  public override void ForceUpdate()
  {
    this.Update();
  }

  public override void Update()
  {
    if (!this.initEnd)
      return;
    this.dictDst[0].trfBone.SetLocalScale((float) this.dictSrc[0].vctScl.x, (float) this.dictSrc[0].vctScl.y, (float) this.dictSrc[0].vctScl.z);
    this.dictDst[1].trfBone.SetLocalScale((float) this.dictSrc[1].vctScl.x, (float) this.dictSrc[1].vctScl.y, (float) this.dictSrc[1].vctScl.z);
    this.dictDst[2].trfBone.SetLocalScale((float) (this.dictSrc[1].vctScl.x * this.dictSrc[2].vctScl.x), (float) this.dictSrc[1].vctScl.y, (float) (this.dictSrc[1].vctScl.z * this.dictSrc[3].vctScl.z));
    this.dictDst[3].trfBone.SetLocalScale((float) (this.dictSrc[1].vctScl.x * this.dictSrc[4].vctScl.x), (float) this.dictSrc[1].vctScl.y, (float) (this.dictSrc[1].vctScl.z * this.dictSrc[5].vctScl.z));
    this.dictDst[4].trfBone.SetLocalPositionX((float) this.dictSrc[6].vctPos.x);
    this.dictDst[4].trfBone.SetLocalPositionY((float) this.dictSrc[7].vctPos.y);
    this.dictDst[4].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[8].vctRot.z);
    this.dictDst[4].trfBone.SetLocalScale((float) this.dictSrc[1].vctScl.x, (float) this.dictSrc[1].vctScl.y, (float) this.dictSrc[1].vctScl.z);
    this.dictDst[5].trfBone.SetLocalPositionY((float) this.dictSrc[9].vctPos.y);
    this.dictDst[5].trfBone.SetLocalRotation((float) this.dictSrc[10].vctRot.x, 0.0f, 0.0f);
    this.dictDst[5].trfBone.SetLocalScale(1f, (float) this.dictSrc[10].vctScl.y, 1f);
  }

  public override void UpdateAlways()
  {
  }

  public enum DstBoneName
  {
    cf_J_Root,
    cf_J_Root_s,
    cf_J_Spine01_s,
    cf_J_Spine02_s,
    cf_J_Spine03_s,
    cf_J_Spine02_ss,
  }

  public enum SrcBoneName
  {
    cf_J_Root_height,
    cf_J_Root_1,
    cf_J_Spine01_s_sx,
    cf_J_Spine01_s_sz,
    cf_J_Spine02_s_sx,
    cf_J_Spine02_s_sz,
    cf_J_Spine03_s_tx,
    cf_J_Spine03_s_ty,
    cf_J_Spine03_s_rz,
    cf_J_Spine02_ss_ty,
    cf_J_Spine02_ss_rx,
  }
}
