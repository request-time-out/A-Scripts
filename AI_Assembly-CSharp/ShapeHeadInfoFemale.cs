// Decompiled with JetBrains decompiler
// Type: ShapeHeadInfoFemale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.SetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHeadInfoFemale : ShapeInfoBase
{
  public override void InitShapeInfo(
    string manifest,
    string assetBundleAnmKey,
    string assetBundleCategory,
    string anmKeyInfoPath,
    string cateInfoPath,
    Transform trfObj)
  {
    Dictionary<string, int> dictEnumDst = new Dictionary<string, int>();
    foreach (ShapeHeadInfoFemale.DstName dstName in (ShapeHeadInfoFemale.DstName[]) Enum.GetValues(typeof (ShapeHeadInfoFemale.DstName)))
      dictEnumDst[dstName.ToString()] = (int) dstName;
    Dictionary<string, int> dictEnumSrc = new Dictionary<string, int>();
    foreach (ShapeHeadInfoFemale.SrcName srcName in (ShapeHeadInfoFemale.SrcName[]) Enum.GetValues(typeof (ShapeHeadInfoFemale.SrcName)))
      dictEnumSrc[srcName.ToString()] = (int) srcName;
    this.InitShapeInfoBase(manifest, assetBundleAnmKey, assetBundleCategory, anmKeyInfoPath, cateInfoPath, trfObj, dictEnumDst, dictEnumSrc, new Action<string, string>(Singleton<Character>.Instance.AddLoadAssetBundle));
    this.InitEnd = true;
  }

  public override void ForceUpdate()
  {
    this.Update();
  }

  public override void Update()
  {
    if (!this.InitEnd || this.dictSrc.Count == 0)
      return;
    this.dictDst[51].trfBone.SetLocalPositionY((float) this.dictSrc[100].vctPos.y);
    this.dictDst[51].trfBone.SetLocalPositionZ((float) (this.dictSrc[101].vctPos.z + this.dictSrc[100].vctPos.z));
    this.dictDst[19].trfBone.SetLocalPositionX((float) this.dictSrc[41].vctPos.x);
    this.dictDst[19].trfBone.SetLocalPositionY((float) this.dictSrc[43].vctPos.y);
    this.dictDst[19].trfBone.SetLocalPositionZ((float) this.dictSrc[44].vctPos.z);
    this.dictDst[19].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[35].vctRot.z);
    this.dictDst[20].trfBone.SetLocalPositionX((float) this.dictSrc[42].vctPos.x);
    this.dictDst[20].trfBone.SetLocalPositionY((float) this.dictSrc[43].vctPos.y);
    this.dictDst[20].trfBone.SetLocalPositionZ((float) this.dictSrc[44].vctPos.z);
    this.dictDst[20].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[36].vctRot.z);
    this.dictDst[17].trfBone.SetLocalScale((float) this.dictSrc[37].vctScl.x, (float) this.dictSrc[39].vctScl.y, 1f);
    this.dictDst[15].trfBone.SetLocalRotation(0.0f, (float) this.dictSrc[33].vctRot.y, 0.0f);
    this.dictDst[18].trfBone.SetLocalScale((float) this.dictSrc[38].vctScl.x, (float) this.dictSrc[40].vctScl.y, 1f);
    this.dictDst[16].trfBone.SetLocalRotation(0.0f, (float) this.dictSrc[34].vctRot.y, 0.0f);
    this.dictDst[21].trfBone.SetLocalRotation((float) this.dictSrc[47].vctRot.x, (float) (this.dictSrc[45].vctRot.y + this.dictSrc[47].vctRot.y), 0.0f);
    this.dictDst[23].trfBone.SetLocalRotation((float) this.dictSrc[49].vctRot.x, (float) this.dictSrc[51].vctRot.y, (float) this.dictSrc[51].vctRot.z);
    this.dictDst[25].trfBone.SetLocalPositionX((float) this.dictSrc[53].vctPos.x);
    this.dictDst[25].trfBone.SetLocalRotation((float) this.dictSrc[55].vctRot.x, (float) this.dictSrc[53].vctRot.y, 0.0f);
    this.dictDst[27].trfBone.SetLocalRotation((float) this.dictSrc[57].vctRot.x, (float) this.dictSrc[59].vctRot.y, (float) this.dictSrc[59].vctRot.z);
    this.dictDst[22].trfBone.SetLocalRotation((float) this.dictSrc[48].vctRot.x, (float) (this.dictSrc[46].vctRot.y + this.dictSrc[48].vctRot.y), 0.0f);
    this.dictDst[24].trfBone.SetLocalRotation((float) this.dictSrc[50].vctRot.x, (float) this.dictSrc[52].vctRot.y, (float) this.dictSrc[52].vctRot.z);
    this.dictDst[26].trfBone.SetLocalPositionX((float) this.dictSrc[54].vctPos.x);
    this.dictDst[26].trfBone.SetLocalRotation((float) this.dictSrc[56].vctRot.x, (float) this.dictSrc[54].vctRot.y, 0.0f);
    this.dictDst[28].trfBone.SetLocalRotation((float) this.dictSrc[58].vctRot.x, (float) this.dictSrc[60].vctRot.y, (float) this.dictSrc[60].vctRot.z);
    this.dictDst[29].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[61].vctRot.z);
    this.dictDst[30].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[62].vctRot.z);
    this.dictDst[31].trfBone.SetLocalScale((float) this.dictSrc[63].vctScl.x, 1f, 1f);
    this.dictDst[34].trfBone.SetLocalPositionY((float) this.dictSrc[66].vctPos.y);
    this.dictDst[35].trfBone.SetLocalPositionZ((float) this.dictSrc[67].vctPos.z);
    this.dictDst[2].trfBone.SetLocalPositionX((float) this.dictSrc[4].vctPos.x);
    this.dictDst[2].trfBone.SetLocalPositionY((float) this.dictSrc[6].vctPos.y);
    this.dictDst[2].trfBone.SetLocalPositionZ((float) (this.dictSrc[7].vctPos.z + this.dictSrc[8].vctPos.z));
    this.dictDst[3].trfBone.SetLocalPositionX((float) this.dictSrc[5].vctPos.x);
    this.dictDst[3].trfBone.SetLocalPositionY((float) this.dictSrc[6].vctPos.y);
    this.dictDst[3].trfBone.SetLocalPositionZ((float) (this.dictSrc[7].vctPos.z + this.dictSrc[8].vctPos.z));
    this.dictDst[0].trfBone.SetLocalPositionX((float) this.dictSrc[0].vctPos.x);
    this.dictDst[0].trfBone.SetLocalPositionY((float) this.dictSrc[2].vctPos.y);
    this.dictDst[0].trfBone.SetLocalPositionZ((float) this.dictSrc[3].vctPos.z);
    this.dictDst[1].trfBone.SetLocalPositionX((float) this.dictSrc[1].vctPos.x);
    this.dictDst[1].trfBone.SetLocalPositionY((float) this.dictSrc[2].vctPos.y);
    this.dictDst[1].trfBone.SetLocalPositionZ((float) this.dictSrc[3].vctPos.z);
    this.dictDst[33].trfBone.SetLocalPositionZ((float) this.dictSrc[65].vctPos.z);
    this.dictDst[32].trfBone.SetLocalScale((float) this.dictSrc[64].vctScl.x, 1f, 1f);
    this.dictDst[42].trfBone.SetLocalPositionY((float) this.dictSrc[78].vctPos.y);
    this.dictDst[42].trfBone.SetLocalPositionZ((float) (this.dictSrc[78].vctPos.z + this.dictSrc[72].vctPos.z));
    this.dictDst[41].trfBone.SetLocalScale((float) this.dictSrc[76].vctScl.x, (float) this.dictSrc[77].vctScl.y, 1f);
    this.dictDst[40].trfBone.SetLocalPositionY((float) this.dictSrc[73].vctPos.y);
    this.dictDst[39].trfBone.SetLocalPositionY((float) this.dictSrc[79].vctPos.y);
    this.dictDst[39].trfBone.SetLocalPositionZ((float) this.dictSrc[79].vctPos.z);
    this.dictDst[39].trfBone.SetLocalScale((float) this.dictSrc[79].vctScl.x, 1f, 1f);
    this.dictDst[37].trfBone.SetLocalPositionY((float) this.dictSrc[74].vctPos.y);
    this.dictDst[37].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[74].vctRot.z);
    this.dictDst[38].trfBone.SetLocalPositionY((float) this.dictSrc[75].vctPos.y);
    this.dictDst[38].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[75].vctRot.z);
    this.dictDst[5].trfBone.SetLocalPositionY((float) this.dictSrc[13].vctPos.y);
    this.dictDst[4].trfBone.SetLocalPositionY((float) (this.dictSrc[11].vctPos.y + this.dictSrc[9].vctPos.y));
    this.dictDst[4].trfBone.SetLocalPositionZ((float) (this.dictSrc[12].vctPos.z + this.dictSrc[9].vctPos.z));
    this.dictDst[4].trfBone.SetLocalRotation((float) this.dictSrc[9].vctRot.x, 0.0f, 0.0f);
    this.dictDst[4].trfBone.SetLocalScale((float) this.dictSrc[10].vctScl.x, 1f, 1f);
    this.dictDst[6].trfBone.SetLocalPositionY((float) this.dictSrc[15].vctPos.y);
    this.dictDst[6].trfBone.SetLocalPositionZ((float) this.dictSrc[16].vctPos.z);
    this.dictDst[6].trfBone.SetLocalScale((float) this.dictSrc[14].vctScl.x, (float) this.dictSrc[14].vctScl.y, (float) this.dictSrc[14].vctScl.z);
    this.dictDst[48].trfBone.SetLocalPositionY((float) this.dictSrc[90].vctPos.y);
    this.dictDst[48].trfBone.SetLocalPositionZ((float) (this.dictSrc[91].vctPos.z + this.dictSrc[92].vctPos.z + this.dictSrc[90].vctPos.z + this.dictSrc[88].vctPos.z));
    this.dictDst[48].trfBone.SetLocalRotation((float) this.dictSrc[88].vctRot.x, 0.0f, 0.0f);
    this.dictDst[47].trfBone.SetLocalScale((float) this.dictSrc[89].vctScl.x, 1f, 1f);
    this.dictDst[46].trfBone.SetLocalPositionY((float) (this.dictSrc[84].vctPos.y + this.dictSrc[86].vctPos.y + this.dictSrc[83].vctPos.y));
    this.dictDst[46].trfBone.SetLocalPositionZ((float) (this.dictSrc[84].vctPos.z + this.dictSrc[87].vctPos.z + this.dictSrc[83].vctPos.z));
    this.dictDst[45].trfBone.SetLocalRotation((float) (this.dictSrc[84].vctRot.x + this.dictSrc[83].vctRot.x), 0.0f, 0.0f);
    this.dictDst[45].trfBone.SetLocalScale((float) this.dictSrc[85].vctScl.x, (float) this.dictSrc[85].vctScl.y, (float) this.dictSrc[85].vctScl.z);
    this.dictDst[49].trfBone.SetLocalPositionX((float) this.dictSrc[96].vctPos.x);
    this.dictDst[49].trfBone.SetLocalPositionY((float) this.dictSrc[98].vctPos.y);
    this.dictDst[49].trfBone.SetLocalPositionZ((float) this.dictSrc[99].vctPos.z);
    this.dictDst[49].trfBone.SetLocalRotation((float) this.dictSrc[93].vctRot.x, 0.0f, (float) this.dictSrc[94].vctRot.z);
    this.dictDst[50].trfBone.SetLocalPositionX((float) this.dictSrc[97].vctPos.x);
    this.dictDst[50].trfBone.SetLocalPositionY((float) this.dictSrc[98].vctPos.y);
    this.dictDst[50].trfBone.SetLocalPositionZ((float) this.dictSrc[99].vctPos.z);
    this.dictDst[50].trfBone.SetLocalRotation((float) this.dictSrc[93].vctRot.x, 0.0f, (float) this.dictSrc[95].vctRot.z);
    this.dictDst[44].trfBone.SetLocalPositionY((float) this.dictSrc[81].vctPos.y);
    this.dictDst[44].trfBone.SetLocalPositionZ((float) this.dictSrc[81].vctPos.z);
    this.dictDst[44].trfBone.SetLocalScale((float) this.dictSrc[81].vctScl.x, (float) this.dictSrc[81].vctScl.y, (float) this.dictSrc[81].vctScl.z);
    this.dictDst[43].trfBone.SetLocalPositionY((float) this.dictSrc[80].vctPos.y);
    this.dictDst[43].trfBone.SetLocalPositionZ((float) this.dictSrc[82].vctPos.z);
    this.dictDst[43].trfBone.SetLocalRotation((float) this.dictSrc[80].vctRot.x, 0.0f, 0.0f);
    this.dictDst[36].trfBone.SetLocalPositionY((float) (this.dictSrc[70].vctPos.y + this.dictSrc[68].vctPos.y + this.dictSrc[69].vctPos.y));
    this.dictDst[36].trfBone.SetLocalPositionZ((float) (this.dictSrc[70].vctPos.z + this.dictSrc[71].vctPos.z + this.dictSrc[69].vctPos.z));
    this.dictDst[36].trfBone.SetLocalRotation((float) (this.dictSrc[70].vctRot.x + this.dictSrc[68].vctRot.x + this.dictSrc[69].vctRot.x), 0.0f, 0.0f);
    this.dictDst[7].trfBone.SetLocalRotation(0.0f, (float) this.dictSrc[17].vctRot.y, (float) this.dictSrc[19].vctRot.z);
    this.dictDst[7].trfBone.SetLocalScale((float) this.dictSrc[21].vctScl.x, (float) this.dictSrc[21].vctScl.y, (float) this.dictSrc[21].vctScl.z);
    this.dictDst[13].trfBone.SetLocalPositionX((float) this.dictSrc[31].vctPos.x);
    this.dictDst[13].trfBone.SetLocalPositionY((float) this.dictSrc[31].vctPos.y);
    this.dictDst[13].trfBone.SetLocalPositionZ((float) this.dictSrc[31].vctPos.z);
    this.dictDst[13].trfBone.SetLocalRotation((float) this.dictSrc[31].vctRot.x, (float) this.dictSrc[31].vctRot.y, 0.0f);
    this.dictDst[13].trfBone.SetLocalScale((float) this.dictSrc[31].vctScl.x, (float) this.dictSrc[31].vctScl.y, (float) this.dictSrc[31].vctScl.z);
    this.dictDst[9].trfBone.SetLocalPositionY((float) this.dictSrc[23].vctPos.y);
    this.dictDst[9].trfBone.SetLocalPositionZ((float) this.dictSrc[23].vctPos.z);
    this.dictDst[9].trfBone.SetLocalScale((float) this.dictSrc[23].vctScl.x, (float) this.dictSrc[23].vctScl.y, (float) this.dictSrc[23].vctScl.z);
    this.dictDst[8].trfBone.SetLocalRotation(0.0f, (float) this.dictSrc[18].vctRot.y, (float) this.dictSrc[20].vctRot.z);
    this.dictDst[8].trfBone.SetLocalScale((float) this.dictSrc[22].vctScl.x, (float) this.dictSrc[22].vctScl.y, (float) this.dictSrc[22].vctScl.z);
    this.dictDst[14].trfBone.SetLocalPositionX((float) this.dictSrc[32].vctPos.x);
    this.dictDst[14].trfBone.SetLocalPositionY((float) this.dictSrc[32].vctPos.y);
    this.dictDst[14].trfBone.SetLocalPositionZ((float) this.dictSrc[32].vctPos.z);
    this.dictDst[14].trfBone.SetLocalRotation((float) this.dictSrc[32].vctRot.x, (float) this.dictSrc[32].vctRot.y, 0.0f);
    this.dictDst[14].trfBone.SetLocalScale((float) this.dictSrc[32].vctScl.x, (float) this.dictSrc[32].vctScl.y, (float) this.dictSrc[32].vctScl.z);
    this.dictDst[10].trfBone.SetLocalPositionY((float) this.dictSrc[24].vctPos.y);
    this.dictDst[10].trfBone.SetLocalPositionZ((float) this.dictSrc[24].vctPos.z);
    this.dictDst[10].trfBone.SetLocalScale((float) this.dictSrc[24].vctScl.x, (float) this.dictSrc[24].vctScl.y, (float) this.dictSrc[24].vctScl.z);
    this.dictDst[11].trfBone.SetLocalPositionY((float) this.dictSrc[25].vctPos.y);
    this.dictDst[11].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[27].vctRot.z);
    this.dictDst[11].trfBone.SetLocalScale((float) this.dictSrc[29].vctScl.x, (float) this.dictSrc[29].vctScl.y, (float) this.dictSrc[29].vctScl.z);
    this.dictDst[12].trfBone.SetLocalPositionY((float) this.dictSrc[26].vctPos.y);
    this.dictDst[12].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[28].vctRot.z);
    this.dictDst[12].trfBone.SetLocalScale((float) this.dictSrc[30].vctScl.x, (float) this.dictSrc[30].vctScl.y, (float) this.dictSrc[30].vctScl.z);
  }

  public override void UpdateAlways()
  {
    if (this.InitEnd)
      ;
  }

  public enum DstName
  {
    cf_J_CheekLow_L,
    cf_J_CheekLow_R,
    cf_J_CheekUp_L,
    cf_J_CheekUp_R,
    cf_J_Chin_rs,
    cf_J_ChinLow,
    cf_J_ChinTip_s,
    cf_J_EarBase_s_L,
    cf_J_EarBase_s_R,
    cf_J_EarLow_L,
    cf_J_EarLow_R,
    cf_J_EarRing_L,
    cf_J_EarRing_R,
    cf_J_EarUp_L,
    cf_J_EarUp_R,
    cf_J_Eye_r_L,
    cf_J_Eye_r_R,
    cf_J_Eye_s_L,
    cf_J_Eye_s_R,
    cf_J_Eye_t_L,
    cf_J_Eye_t_R,
    cf_J_Eye01_L,
    cf_J_Eye01_R,
    cf_J_Eye02_L,
    cf_J_Eye02_R,
    cf_J_Eye03_L,
    cf_J_Eye03_R,
    cf_J_Eye04_L,
    cf_J_Eye04_R,
    cf_J_EyePos_rz_L,
    cf_J_EyePos_rz_R,
    cf_J_FaceBase,
    cf_J_FaceLow_s,
    cf_J_FaceLowBase,
    cf_J_FaceUp_ty,
    cf_J_FaceUp_tz,
    cf_J_megane,
    cf_J_Mouth_L,
    cf_J_Mouth_R,
    cf_J_MouthLow,
    cf_J_Mouthup,
    cf_J_MouthBase_s,
    cf_J_MouthBase_tr,
    cf_J_Nose_t,
    cf_J_Nose_tip,
    cf_J_NoseBase_s,
    cf_J_NoseBase_trs,
    cf_J_NoseBridge_s,
    cf_J_NoseBridge_t,
    cf_J_NoseWing_tx_L,
    cf_J_NoseWing_tx_R,
    cf_J_MouthCavity,
  }

  public enum SrcName
  {
    cf_s_CheekLow_tx_L,
    cf_s_CheekLow_tx_R,
    cf_s_CheekLow_ty,
    cf_s_CheekLow_tz,
    cf_s_CheekUp_tx_L,
    cf_s_CheekUp_tx_R,
    cf_s_CheekUp_ty,
    cf_s_CheekUp_tz_00,
    cf_s_CheekUp_tz_01,
    cf_s_Chin_rx,
    cf_s_Chin_sx,
    cf_s_Chin_ty,
    cf_s_Chin_tz,
    cf_s_ChinLow,
    cf_s_ChinTip_sx,
    cf_s_ChinTip_ty,
    cf_s_ChinTip_tz,
    cf_s_EarBase_ry_L,
    cf_s_EarBase_ry_R,
    cf_s_EarBase_rz_L,
    cf_s_EarBase_rz_R,
    cf_s_EarBase_s_L,
    cf_s_EarBase_s_R,
    cf_s_EarLow_L,
    cf_s_EarLow_R,
    cf_s_EarRing_L,
    cf_s_EarRing_R,
    cf_s_EarRing_rz_L,
    cf_s_EarRing_rz_R,
    cf_s_EarRing_s_L,
    cf_s_EarRing_s_R,
    cf_s_EarUp_L,
    cf_s_EarUp_R,
    cf_s_Eye_ry_L,
    cf_s_Eye_ry_R,
    cf_s_Eye_rz_L,
    cf_s_Eye_rz_R,
    cf_s_Eye_sx_L,
    cf_s_Eye_sx_R,
    cf_s_Eye_sy_L,
    cf_s_Eye_sy_R,
    cf_s_Eye_tx_L,
    cf_s_Eye_tx_R,
    cf_s_Eye_ty,
    cf_s_Eye_tz,
    cf_s_Eye01_L,
    cf_s_Eye01_R,
    cf_s_Eye01_rx_L,
    cf_s_Eye01_rx_R,
    cf_s_Eye02_L,
    cf_s_Eye02_R,
    cf_s_Eye02_ry_L,
    cf_s_Eye02_ry_R,
    cf_s_Eye03_L,
    cf_s_Eye03_R,
    cf_s_Eye03_rx_L,
    cf_s_Eye03_rx_R,
    cf_s_Eye04_L,
    cf_s_Eye04_R,
    cf_s_Eye04_ry_L,
    cf_s_Eye04_ry_R,
    cf_s_EyePos_rz_L,
    cf_s_EyePos_rz_R,
    cf_s_FaceBase_sx,
    cf_s_FaceLow_sx,
    cf_s_FaceLow_tz,
    cf_s_FaceUp_ty,
    cf_s_FaceUp_tz,
    cf_s_megane_rx_nosebridge,
    cf_s_megane_ty_eye,
    cf_s_megane_ty_nose,
    cf_s_megane_tz_nosebridge,
    cf_s_MouthBase_tz,
    cf_s_Mouthup,
    cf_s_Mouth_L,
    cf_s_Mouth_R,
    cf_s_MouthBase_sx,
    cf_s_MouthBase_sy,
    cf_s_MouthBase_ty,
    cf_s_MouthLow,
    cf_s_Nose_rx,
    cf_s_Nose_tip,
    cf_s_Nose_tz,
    cf_s_NoseBase,
    cf_s_NoseBase_rx,
    cf_s_NoseBase_sx,
    cf_s_NoseBase_ty,
    cf_s_NoseBase_tz,
    cf_s_NoseBridge_rx,
    cf_s_NoseBridge_sx,
    cf_s_NoseBridge_ty,
    cf_s_NoseBridge_tz_00,
    cf_s_NoseBridge_tz_01,
    cf_s_NoseWing_rx,
    cf_s_NoseWing_rz_L,
    cf_s_NoseWing_rz_R,
    cf_s_NoseWing_tx_L,
    cf_s_NoseWing_tx_R,
    cf_s_NoseWing_ty,
    cf_s_NoseWing_tz,
    cf_s_MouthC_ty,
    cf_s_MouthC_tz,
  }
}
