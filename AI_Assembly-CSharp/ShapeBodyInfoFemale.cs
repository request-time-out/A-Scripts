// Decompiled with JetBrains decompiler
// Type: ShapeBodyInfoFemale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.SetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeBodyInfoFemale : ShapeInfoBase
{
  public int updateMask = 31;
  public const int UPDATE_MASK_BUST_L = 1;
  public const int UPDATE_MASK_BUST_R = 2;
  public const int UPDATE_MASK_NIP_L = 4;
  public const int UPDATE_MASK_NIP_R = 8;
  public const int UPDATE_MASK_ETC = 16;
  public const int UPDATE_MASK_ALL = 31;

  public override void InitShapeInfo(
    string manifest,
    string assetBundleAnmKey,
    string assetBundleCategory,
    string anmKeyInfoName,
    string cateInfoName,
    Transform trfObj)
  {
    Dictionary<string, int> dictEnumDst = new Dictionary<string, int>();
    foreach (ShapeBodyInfoFemale.DstName dstName in (ShapeBodyInfoFemale.DstName[]) Enum.GetValues(typeof (ShapeBodyInfoFemale.DstName)))
      dictEnumDst[dstName.ToString()] = (int) dstName;
    Dictionary<string, int> dictEnumSrc = new Dictionary<string, int>();
    foreach (ShapeBodyInfoFemale.SrcName srcName in (ShapeBodyInfoFemale.SrcName[]) Enum.GetValues(typeof (ShapeBodyInfoFemale.SrcName)))
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
    if (!this.InitEnd || this.dictSrc.Count == 0)
      return;
    float num1 = (float) (this.dictSrc[193].vctPos.y + this.dictSrc[194].vctPos.y);
    float num2 = (float) (this.dictSrc[193].vctScl.y * this.dictSrc[194].vctScl.y);
    float x1 = (float) this.dictSrc[194].vctScl.x;
    float z1 = (float) this.dictSrc[194].vctPos.z;
    float num3 = (float) (this.dictSrc[195].vctPos.y + this.dictSrc[196].vctPos.y);
    float num4 = (float) (this.dictSrc[195].vctPos.z + this.dictSrc[196].vctPos.z);
    float num5 = (float) (this.dictSrc[195].vctScl.y * this.dictSrc[196].vctScl.y);
    float num6 = (float) (this.dictSrc[195].vctScl.x * this.dictSrc[196].vctScl.x);
    float x2 = (float) (this.dictSrc[197].vctPos.x + this.dictSrc[198].vctPos.x);
    float z2 = (float) (this.dictSrc[197].vctRot.z + this.dictSrc[198].vctRot.z);
    float x3 = (float) (this.dictSrc[197].vctScl.x * this.dictSrc[198].vctScl.x);
    float y1 = (float) (this.dictSrc[197].vctScl.y * this.dictSrc[198].vctScl.y);
    float num7 = (float) (this.dictSrc[199].vctScl.x * this.dictSrc[200].vctScl.x);
    float num8 = (float) (this.dictSrc[199].vctScl.y * this.dictSrc[200].vctScl.y);
    float x4 = (float) this.dictSrc[200].vctPos.x;
    float num9 = (float) (this.dictSrc[199].vctPos.z + this.dictSrc[200].vctPos.z);
    float x5 = (float) this.dictSrc[201].vctPos.x;
    float y2 = (float) this.dictSrc[201].vctPos.y;
    float z3 = (float) this.dictSrc[201].vctPos.z;
    float x6 = (float) this.dictSrc[201].vctScl.x;
    float y3 = (float) this.dictSrc[201].vctScl.y;
    float x7 = (float) this.dictSrc[202].vctPos.x;
    float y4 = (float) this.dictSrc[202].vctPos.y;
    float z4 = (float) this.dictSrc[202].vctPos.z;
    float x8 = (float) this.dictSrc[202].vctScl.x;
    float x9 = (float) this.dictSrc[202].vctRot.x;
    float z5 = (float) this.dictSrc[204].vctPos.z;
    float x10 = (float) this.dictSrc[204].vctScl.x;
    if ((this.updateMask & 16) != 0)
    {
      this.dictDst[71].trfBone.SetLocalScale((float) this.dictSrc[32].vctScl.x, (float) this.dictSrc[32].vctScl.y, (float) this.dictSrc[32].vctScl.z);
      this.dictDst[67].trfBone.SetLocalScale((float) this.dictSrc[150].vctScl.x, 1f, (float) this.dictSrc[150].vctScl.z);
      this.dictDst[72].trfBone.SetLocalPositionZ((float) (this.dictSrc[161].vctPos.z + this.dictSrc[162].vctPos.z + this.dictSrc[163].vctPos.z + this.dictSrc[164].vctPos.z));
      this.dictDst[72].trfBone.SetLocalRotation((float) (this.dictSrc[163].vctRot.x + this.dictSrc[164].vctRot.x), 0.0f, 0.0f);
      this.dictDst[73].trfBone.SetLocalPositionX((float) this.dictSrc[165].vctPos.x);
      this.dictDst[73].trfBone.SetLocalPositionZ((float) (this.dictSrc[165].vctPos.z + this.dictSrc[166].vctPos.z + this.dictSrc[167].vctPos.z + this.dictSrc[168].vctPos.z));
      this.dictDst[73].trfBone.SetLocalRotation((float) (this.dictSrc[166].vctRot.x + this.dictSrc[167].vctRot.x + this.dictSrc[168].vctRot.x), 0.0f, 0.0f);
      this.dictDst[74].trfBone.SetLocalPositionX((float) (this.dictSrc[169].vctPos.x + this.dictSrc[170].vctPos.x));
      this.dictDst[74].trfBone.SetLocalPositionZ((float) (this.dictSrc[169].vctPos.z + this.dictSrc[170].vctPos.z + this.dictSrc[171].vctPos.z + this.dictSrc[172].vctPos.z));
      this.dictDst[74].trfBone.SetLocalRotation((float) (this.dictSrc[169].vctRot.x + this.dictSrc[170].vctRot.x), 0.0f, 0.0f);
      this.dictDst[75].trfBone.SetLocalPositionX((float) this.dictSrc[173].vctPos.x);
      this.dictDst[75].trfBone.SetLocalPositionZ((float) (this.dictSrc[173].vctPos.z + this.dictSrc[174].vctPos.z + this.dictSrc[175].vctPos.z + this.dictSrc[176].vctPos.z));
      this.dictDst[75].trfBone.SetLocalRotation((float) (this.dictSrc[174].vctRot.x + this.dictSrc[175].vctRot.x + this.dictSrc[176].vctRot.x), 0.0f, 0.0f);
      this.dictDst[76].trfBone.SetLocalPositionZ((float) (this.dictSrc[177].vctPos.z + this.dictSrc[178].vctPos.z + this.dictSrc[179].vctPos.z + this.dictSrc[180].vctPos.z));
      this.dictDst[76].trfBone.SetLocalRotation((float) (this.dictSrc[179].vctRot.x + this.dictSrc[180].vctRot.x), 0.0f, 0.0f);
      this.dictDst[77].trfBone.SetLocalPositionX((float) (this.dictSrc[181].vctPos.x + this.dictSrc[182].vctPos.x));
      this.dictDst[77].trfBone.SetLocalPositionZ((float) (this.dictSrc[181].vctPos.z + this.dictSrc[182].vctPos.z + this.dictSrc[183].vctPos.z + this.dictSrc[184].vctPos.z));
      this.dictDst[77].trfBone.SetLocalRotation((float) (this.dictSrc[182].vctRot.x + this.dictSrc[183].vctRot.x + this.dictSrc[184].vctRot.x), 0.0f, 0.0f);
      this.dictDst[78].trfBone.SetLocalPositionX((float) (this.dictSrc[185].vctPos.x + this.dictSrc[186].vctPos.x));
      this.dictDst[78].trfBone.SetLocalPositionZ((float) (this.dictSrc[185].vctPos.z + this.dictSrc[186].vctPos.z + this.dictSrc[187].vctPos.z + this.dictSrc[188].vctPos.z));
      this.dictDst[78].trfBone.SetLocalRotation((float) (this.dictSrc[185].vctRot.x + this.dictSrc[186].vctRot.x), 0.0f, 0.0f);
      this.dictDst[79].trfBone.SetLocalPositionX((float) (this.dictSrc[189].vctPos.x + this.dictSrc[190].vctPos.x));
      this.dictDst[79].trfBone.SetLocalPositionZ((float) (this.dictSrc[189].vctPos.z + this.dictSrc[190].vctPos.z + this.dictSrc[191].vctPos.z + this.dictSrc[192].vctPos.z));
      this.dictDst[79].trfBone.SetLocalRotation((float) (this.dictSrc[190].vctRot.x + this.dictSrc[191].vctRot.x + this.dictSrc[192].vctRot.x), 0.0f, 0.0f);
      this.dictDst[66].trfBone.SetLocalPositionZ((float) (this.dictSrc[149].vctPos.z + this.dictSrc[148].vctPos.z));
      this.dictDst[66].trfBone.SetLocalRotation((float) this.dictSrc[149].vctRot.x, 0.0f, 0.0f);
      this.dictDst[62].trfBone.SetLocalPositionX((float) this.dictSrc[136].vctPos.x);
      this.dictDst[62].trfBone.SetLocalScale((float) this.dictSrc[132].vctScl.x, (float) (this.dictSrc[134].vctScl.y * this.dictSrc[132].vctScl.z), (float) (this.dictSrc[134].vctScl.z * this.dictSrc[132].vctScl.y));
      this.dictDst[63].trfBone.SetLocalPositionX((float) this.dictSrc[137].vctPos.x);
      this.dictDst[63].trfBone.SetLocalScale((float) this.dictSrc[133].vctScl.x, (float) (this.dictSrc[135].vctScl.y * this.dictSrc[133].vctScl.z), (float) (this.dictSrc[135].vctScl.z * this.dictSrc[133].vctScl.y));
      this.dictDst[6].trfBone.SetLocalPositionX((float) this.dictSrc[16].vctPos.x);
      this.dictDst[6].trfBone.SetLocalPositionY((float) (this.dictSrc[14].vctPos.y + this.dictSrc[16].vctPos.y));
      this.dictDst[6].trfBone.SetLocalRotation(0.0f, (float) this.dictSrc[14].vctRot.y, (float) (this.dictSrc[14].vctRot.z + this.dictSrc[16].vctRot.z));
      this.dictDst[6].trfBone.SetLocalScale(1f, (float) (this.dictSrc[14].vctScl.y * this.dictSrc[12].vctScl.y), (float) (this.dictSrc[14].vctScl.z * this.dictSrc[12].vctScl.z));
      this.dictDst[7].trfBone.SetLocalPositionX((float) this.dictSrc[17].vctPos.x);
      this.dictDst[7].trfBone.SetLocalPositionY((float) (this.dictSrc[15].vctPos.y + this.dictSrc[17].vctPos.y));
      this.dictDst[7].trfBone.SetLocalRotation(0.0f, (float) this.dictSrc[15].vctRot.y, (float) (this.dictSrc[15].vctRot.z + this.dictSrc[17].vctRot.z));
      this.dictDst[7].trfBone.SetLocalScale(1f, (float) (this.dictSrc[15].vctScl.y * this.dictSrc[13].vctScl.y), (float) (this.dictSrc[15].vctScl.z * this.dictSrc[13].vctScl.z));
      this.dictDst[8].trfBone.SetLocalPositionY((float) this.dictSrc[20].vctPos.y);
      this.dictDst[8].trfBone.SetLocalScale(1f, (float) (this.dictSrc[20].vctScl.y * this.dictSrc[18].vctScl.y), (float) (this.dictSrc[20].vctScl.z * this.dictSrc[18].vctScl.z));
      this.dictDst[9].trfBone.SetLocalPositionY((float) this.dictSrc[21].vctPos.y);
      this.dictDst[9].trfBone.SetLocalScale(1f, (float) (this.dictSrc[21].vctScl.y * this.dictSrc[19].vctScl.y), (float) (this.dictSrc[21].vctScl.z * this.dictSrc[19].vctScl.z));
      this.dictDst[10].trfBone.SetLocalScale(1f, (float) (this.dictSrc[24].vctScl.y * this.dictSrc[22].vctScl.y), (float) (this.dictSrc[24].vctScl.z * this.dictSrc[22].vctScl.z));
      this.dictDst[11].trfBone.SetLocalScale(1f, (float) (this.dictSrc[25].vctScl.y * this.dictSrc[23].vctScl.y), (float) (this.dictSrc[25].vctScl.z * this.dictSrc[23].vctScl.z));
      this.dictDst[0].trfBone.SetLocalScale(1f, (float) (this.dictSrc[2].vctScl.y * this.dictSrc[0].vctScl.y), (float) (this.dictSrc[2].vctScl.z * this.dictSrc[0].vctScl.z));
      this.dictDst[1].trfBone.SetLocalScale(1f, (float) (this.dictSrc[3].vctScl.y * this.dictSrc[1].vctScl.y), (float) (this.dictSrc[3].vctScl.z * this.dictSrc[1].vctScl.z));
      this.dictDst[2].trfBone.SetLocalScale(1f, (float) (this.dictSrc[6].vctScl.y * this.dictSrc[4].vctScl.y), (float) (this.dictSrc[6].vctScl.z * this.dictSrc[4].vctScl.z));
      this.dictDst[3].trfBone.SetLocalScale(1f, (float) (this.dictSrc[7].vctScl.y * this.dictSrc[5].vctScl.y), (float) (this.dictSrc[7].vctScl.z * this.dictSrc[5].vctScl.z));
      this.dictDst[4].trfBone.SetLocalScale(1f, (float) (this.dictSrc[10].vctScl.y * this.dictSrc[8].vctScl.y), (float) (this.dictSrc[10].vctScl.z * this.dictSrc[8].vctScl.z));
      this.dictDst[5].trfBone.SetLocalScale(1f, (float) (this.dictSrc[11].vctScl.y * this.dictSrc[9].vctScl.y), (float) (this.dictSrc[11].vctScl.z * this.dictSrc[9].vctScl.z));
      this.dictDst[12].trfBone.SetLocalScale((float) this.dictSrc[26].vctScl.x, (float) this.dictSrc[26].vctScl.y, (float) this.dictSrc[26].vctScl.z);
      this.dictDst[13].trfBone.SetLocalScale((float) this.dictSrc[27].vctScl.x, (float) this.dictSrc[27].vctScl.y, (float) this.dictSrc[27].vctScl.z);
      this.dictDst[14].trfBone.SetLocalScale(1f, (float) this.dictSrc[28].vctScl.y, (float) this.dictSrc[28].vctScl.z);
      this.dictDst[15].trfBone.SetLocalScale(1f, (float) this.dictSrc[29].vctScl.y, (float) this.dictSrc[29].vctScl.z);
      this.dictDst[17].trfBone.SetLocalScale((float) (this.dictSrc[33].vctScl.x * this.dictSrc[34].vctScl.x), 1f, (float) (this.dictSrc[33].vctScl.z * this.dictSrc[35].vctScl.z));
      this.dictDst[18].trfBone.SetLocalScale((float) (this.dictSrc[36].vctScl.x * this.dictSrc[37].vctScl.x), 1f, (float) (this.dictSrc[36].vctScl.z * this.dictSrc[38].vctScl.z));
      this.dictDst[68].trfBone.SetLocalScale((float) (this.dictSrc[151].vctScl.x * this.dictSrc[152].vctScl.x), 1f, (float) (this.dictSrc[151].vctScl.z * this.dictSrc[153].vctScl.z));
      this.dictDst[68].trfBone.SetLocalPositionY((float) this.dictSrc[154].vctPos.y);
      this.dictDst[68].trfBone.SetLocalPositionZ((float) (this.dictSrc[152].vctPos.z + this.dictSrc[153].vctPos.z));
      this.dictDst[69].trfBone.SetLocalScale((float) (this.dictSrc[155].vctScl.x * this.dictSrc[156].vctScl.x), 1f, (float) (this.dictSrc[155].vctScl.z * this.dictSrc[157].vctScl.z));
      this.dictDst[70].trfBone.SetLocalScale((float) (this.dictSrc[158].vctScl.x * this.dictSrc[159].vctScl.x), 1f, (float) (this.dictSrc[158].vctScl.z * this.dictSrc[160].vctScl.z));
      this.dictDst[61].trfBone.SetLocalScale((float) (this.dictSrc[(int) sbyte.MaxValue].vctScl.x * this.dictSrc[128].vctScl.x), 1f, (float) (this.dictSrc[(int) sbyte.MaxValue].vctScl.z * this.dictSrc[129].vctScl.z));
      this.dictDst[16].trfBone.SetLocalScale((float) (this.dictSrc[30].vctScl.x * this.dictSrc[31].vctScl.x), (float) (this.dictSrc[30].vctScl.y * this.dictSrc[31].vctScl.y), (float) (this.dictSrc[30].vctScl.z * this.dictSrc[31].vctScl.z));
    }
    if ((this.updateMask & 1) != 0)
    {
      float x11 = (float) this.dictSrc[203].vctPos.x;
      float y5 = (float) this.dictSrc[203].vctPos.y;
      float z6 = (float) this.dictSrc[203].vctPos.z;
      float x12 = (float) this.dictSrc[203].vctScl.x;
      this.dictDst[47].trfBone.SetLocalPositionX((float) (this.dictSrc[95].vctPos.x + this.dictSrc[99].vctPos.x));
      this.dictDst[47].trfBone.SetLocalPositionY((float) (this.dictSrc[97].vctPos.y + this.dictSrc[103].vctPos.y));
      this.dictDst[47].trfBone.SetLocalPositionZ((float) (this.dictSrc[97].vctPos.z + this.dictSrc[101].vctPos.z));
      this.dictDst[47].trfBone.SetLocalRotation((float) (this.dictSrc[97].vctRot.x + this.dictSrc[101].vctRot.x + this.dictSrc[103].vctRot.x + this.dictSrc[93].vctRot.x), (float) (this.dictSrc[99].vctRot.y + this.dictSrc[103].vctRot.y), 0.0f);
      this.dictDst[47].trfBone.SetLocalScale((float) this.dictSrc[91].vctScl.x, (float) this.dictSrc[91].vctScl.y, (float) this.dictSrc[91].vctScl.z);
      this.dictDst[45].trfBone.SetLocalPositionX((float) this.dictSrc[93].vctPos.x);
      this.dictDst[45].trfBone.SetLocalPositionY((float) this.dictSrc[93].vctPos.y);
      this.dictDst[45].trfBone.SetLocalPositionZ((float) this.dictSrc[93].vctPos.z);
      this.dictDst[45].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[93].vctRot.z);
      this.dictDst[45].trfBone.SetLocalScale((float) this.dictSrc[93].vctScl.x, (float) this.dictSrc[93].vctScl.y, (float) this.dictSrc[93].vctScl.z);
      this.dictDst[43].trfBone.SetLocalPositionX((float) (this.dictSrc[111].vctPos.x + this.dictSrc[109].vctPos.x));
      this.dictDst[43].trfBone.SetLocalPositionZ((float) (0.649999976158142 + this.dictSrc[107].vctPos.z + this.dictSrc[109].vctPos.z));
      this.dictDst[43].trfBone.SetLocalRotation((float) this.dictSrc[107].vctRot.x, (float) this.dictSrc[109].vctRot.y, 0.0f);
      this.dictDst[49].trfBone.SetLocalPositionY((float) this.dictSrc[105].vctPos.y);
      this.dictDst[49].trfBone.SetLocalPositionZ((float) this.dictSrc[105].vctPos.z);
      this.dictDst[49].trfBone.SetLocalRotation((float) this.dictSrc[105].vctRot.x, (float) this.dictSrc[105].vctRot.y, (float) this.dictSrc[105].vctRot.z);
      this.dictDst[49].trfBone.SetLocalScale((float) this.dictSrc[105].vctScl.x, (float) this.dictSrc[105].vctScl.y, (float) this.dictSrc[105].vctScl.z);
      this.dictDst[51].trfBone.SetLocalPositionX((float) this.dictSrc[105].vctPos.x);
      this.dictDst[51].trfBone.SetLocalPositionZ((float) (0.300000011920929 + this.dictSrc[113].vctPos.z));
      this.dictDst[51].trfBone.SetLocalRotation((float) this.dictSrc[117].vctRot.x, 0.0f, 0.0f);
      this.dictDst[53].trfBone.SetLocalPositionY((float) this.dictSrc[115].vctPos.y);
      this.dictDst[53].trfBone.SetLocalPositionZ((float) this.dictSrc[115].vctPos.z);
      this.dictDst[53].trfBone.SetLocalRotation((float) this.dictSrc[115].vctRot.x, 0.0f, 0.0f);
      this.dictDst[53].trfBone.SetLocalScale((float) this.dictSrc[115].vctScl.x, (float) this.dictSrc[115].vctScl.y, (float) this.dictSrc[115].vctScl.z);
      this.dictDst[55].trfBone.SetLocalPositionZ((float) (0.300000011920929 + this.dictSrc[119].vctPos.z));
      this.dictDst[55].trfBone.SetLocalRotation((float) this.dictSrc[123].vctRot.x, 0.0f, 0.0f);
      this.dictDst[57].trfBone.SetLocalPositionZ((float) this.dictSrc[121].vctPos.z);
      this.dictDst[57].trfBone.SetLocalScale((float) this.dictSrc[121].vctScl.x, (float) this.dictSrc[121].vctScl.y, (float) this.dictSrc[121].vctScl.z);
      this.dictDst[59].trfBone.SetLocalPositionZ((float) this.dictSrc[125].vctPos.z);
      this.dictDst[59].trfBone.SetLocalScale((float) this.dictSrc[125].vctScl.x, (float) this.dictSrc[125].vctScl.y, (float) this.dictSrc[125].vctScl.z);
      this.dictDst[37].trfBone.SetLocalPositionZ((float) (this.dictSrc[83].vctPos.z + this.dictSrc[81].vctPos.z));
      this.dictDst[37].trfBone.SetLocalScale((float) (this.dictSrc[83].vctScl.x * this.dictSrc[81].vctScl.x), (float) (this.dictSrc[83].vctScl.y * this.dictSrc[81].vctScl.y), (float) this.dictSrc[83].vctScl.z);
      this.dictDst[80].trfBone.SetLocalPositionX(x11);
      this.dictDst[80].trfBone.SetLocalPositionY(y5);
      this.dictDst[80].trfBone.SetLocalPositionZ(z6);
      this.dictDst[80].trfBone.SetLocalScale(x12, 1f, x12);
    }
    if ((this.updateMask & 4) != 0)
    {
      this.dictDst[39].trfBone.SetLocalPositionZ((float) this.dictSrc[85].vctPos.z);
      this.dictDst[39].trfBone.SetLocalScale((float) this.dictSrc[85].vctScl.x, (float) this.dictSrc[85].vctScl.y, (float) this.dictSrc[85].vctScl.z);
      this.dictDst[41].trfBone.SetLocalPositionZ((float) (this.dictSrc[87].vctPos.z + this.dictSrc[89].vctPos.z));
      this.dictDst[41].trfBone.SetLocalScale((float) (this.dictSrc[79].vctScl.x * this.dictSrc[89].vctScl.x), (float) (this.dictSrc[79].vctScl.y * this.dictSrc[89].vctScl.y), (float) (this.dictSrc[79].vctScl.z * this.dictSrc[89].vctScl.z));
    }
    if ((this.updateMask & 2) != 0)
    {
      float x11 = (float) this.dictSrc[203].vctPos.x;
      float y5 = (float) this.dictSrc[203].vctPos.y;
      float z6 = (float) this.dictSrc[203].vctPos.z;
      float x12 = (float) this.dictSrc[203].vctScl.x;
      this.dictDst[48].trfBone.SetLocalPositionX((float) (this.dictSrc[96].vctPos.x + this.dictSrc[100].vctPos.x));
      this.dictDst[48].trfBone.SetLocalPositionY((float) (this.dictSrc[98].vctPos.y + this.dictSrc[104].vctPos.y));
      this.dictDst[48].trfBone.SetLocalPositionZ((float) (this.dictSrc[98].vctPos.z + this.dictSrc[102].vctPos.z));
      this.dictDst[48].trfBone.SetLocalRotation((float) (this.dictSrc[98].vctRot.x + this.dictSrc[102].vctRot.x + this.dictSrc[104].vctRot.x + this.dictSrc[94].vctRot.x), (float) (this.dictSrc[100].vctRot.y + this.dictSrc[104].vctRot.y), 0.0f);
      this.dictDst[48].trfBone.SetLocalScale((float) this.dictSrc[92].vctScl.x, (float) this.dictSrc[92].vctScl.y, (float) this.dictSrc[92].vctScl.z);
      this.dictDst[46].trfBone.SetLocalPositionX((float) this.dictSrc[94].vctPos.x);
      this.dictDst[46].trfBone.SetLocalPositionY((float) this.dictSrc[94].vctPos.y);
      this.dictDst[46].trfBone.SetLocalPositionZ((float) this.dictSrc[94].vctPos.z);
      this.dictDst[46].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[94].vctRot.z);
      this.dictDst[46].trfBone.SetLocalScale((float) this.dictSrc[94].vctScl.x, (float) this.dictSrc[94].vctScl.y, (float) this.dictSrc[94].vctScl.z);
      this.dictDst[44].trfBone.SetLocalPositionX((float) (this.dictSrc[112].vctPos.x + this.dictSrc[110].vctPos.x));
      this.dictDst[44].trfBone.SetLocalPositionZ((float) (0.649999976158142 + this.dictSrc[108].vctPos.z + this.dictSrc[110].vctPos.z));
      this.dictDst[44].trfBone.SetLocalRotation((float) this.dictSrc[108].vctRot.x, (float) this.dictSrc[110].vctRot.y, 0.0f);
      this.dictDst[50].trfBone.SetLocalPositionY((float) this.dictSrc[106].vctPos.y);
      this.dictDst[50].trfBone.SetLocalPositionZ((float) this.dictSrc[106].vctPos.z);
      this.dictDst[50].trfBone.SetLocalRotation((float) this.dictSrc[106].vctRot.x, (float) this.dictSrc[106].vctRot.y, (float) this.dictSrc[106].vctRot.z);
      this.dictDst[50].trfBone.SetLocalScale((float) this.dictSrc[106].vctScl.x, (float) this.dictSrc[106].vctScl.y, (float) this.dictSrc[106].vctScl.z);
      this.dictDst[52].trfBone.SetLocalPositionX((float) this.dictSrc[106].vctPos.x);
      this.dictDst[52].trfBone.SetLocalPositionZ((float) (0.300000011920929 + this.dictSrc[114].vctPos.z));
      this.dictDst[52].trfBone.SetLocalRotation((float) this.dictSrc[118].vctRot.x, 0.0f, 0.0f);
      this.dictDst[54].trfBone.SetLocalPositionY((float) this.dictSrc[116].vctPos.y);
      this.dictDst[54].trfBone.SetLocalPositionZ((float) this.dictSrc[116].vctPos.z);
      this.dictDst[54].trfBone.SetLocalRotation((float) this.dictSrc[116].vctRot.x, 0.0f, 0.0f);
      this.dictDst[54].trfBone.SetLocalScale((float) this.dictSrc[116].vctScl.x, (float) this.dictSrc[116].vctScl.y, (float) this.dictSrc[116].vctScl.z);
      this.dictDst[56].trfBone.SetLocalPositionZ((float) (0.300000011920929 + this.dictSrc[120].vctPos.z));
      this.dictDst[56].trfBone.SetLocalRotation((float) this.dictSrc[124].vctRot.x, 0.0f, 0.0f);
      this.dictDst[58].trfBone.SetLocalPositionZ((float) this.dictSrc[122].vctPos.z);
      this.dictDst[58].trfBone.SetLocalScale((float) this.dictSrc[122].vctScl.x, (float) this.dictSrc[122].vctScl.y, (float) this.dictSrc[122].vctScl.z);
      this.dictDst[60].trfBone.SetLocalPositionZ((float) this.dictSrc[126].vctPos.z);
      this.dictDst[60].trfBone.SetLocalScale((float) this.dictSrc[126].vctScl.x, (float) this.dictSrc[126].vctScl.y, (float) this.dictSrc[126].vctScl.z);
      this.dictDst[38].trfBone.SetLocalPositionZ((float) (this.dictSrc[84].vctPos.z + this.dictSrc[82].vctPos.z));
      this.dictDst[38].trfBone.SetLocalScale((float) (this.dictSrc[84].vctScl.x * this.dictSrc[82].vctScl.x), (float) (this.dictSrc[84].vctScl.y * this.dictSrc[82].vctScl.y), (float) this.dictSrc[84].vctScl.z);
      this.dictDst[81].trfBone.SetLocalPositionX(x11);
      this.dictDst[81].trfBone.SetLocalPositionY(y5);
      this.dictDst[81].trfBone.SetLocalPositionZ(z6);
      this.dictDst[81].trfBone.SetLocalScale(x12, 1f, x12);
    }
    if ((this.updateMask & 8) != 0)
    {
      this.dictDst[40].trfBone.SetLocalPositionZ((float) this.dictSrc[86].vctPos.z);
      this.dictDst[40].trfBone.SetLocalScale((float) this.dictSrc[86].vctScl.x, (float) this.dictSrc[86].vctScl.y, (float) this.dictSrc[86].vctScl.z);
      this.dictDst[42].trfBone.SetLocalPositionZ((float) (this.dictSrc[88].vctPos.z + this.dictSrc[90].vctPos.z));
      this.dictDst[42].trfBone.SetLocalScale((float) (this.dictSrc[80].vctScl.x * this.dictSrc[90].vctScl.x), (float) (this.dictSrc[80].vctScl.y * this.dictSrc[90].vctScl.y), (float) (this.dictSrc[80].vctScl.z * this.dictSrc[90].vctScl.z));
    }
    if ((this.updateMask & 16) == 0)
      return;
    this.dictDst[35].trfBone.SetLocalScale((float) this.dictSrc[77].vctScl.x, 1f, (float) this.dictSrc[77].vctScl.z);
    this.dictDst[36].trfBone.SetLocalScale((float) this.dictSrc[78].vctScl.x, 1f, (float) this.dictSrc[78].vctScl.z);
    this.dictDst[29].trfBone.SetLocalPositionX((float) (this.dictSrc[63].vctPos.x + this.dictSrc[57].vctPos.x));
    this.dictDst[29].trfBone.SetLocalPositionZ((float) this.dictSrc[63].vctPos.z);
    this.dictDst[29].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[57].vctRot.z);
    this.dictDst[29].trfBone.SetLocalScale((float) (this.dictSrc[61].vctScl.x * this.dictSrc[63].vctScl.x * this.dictSrc[57].vctScl.x), 1f, (float) (this.dictSrc[61].vctScl.z * this.dictSrc[63].vctScl.z * this.dictSrc[59].vctScl.z));
    this.dictDst[30].trfBone.SetLocalPositionX((float) (this.dictSrc[64].vctPos.x + this.dictSrc[58].vctPos.x));
    this.dictDst[30].trfBone.SetLocalPositionZ((float) this.dictSrc[64].vctPos.z);
    this.dictDst[30].trfBone.SetLocalRotation(0.0f, 0.0f, (float) this.dictSrc[58].vctRot.z);
    this.dictDst[30].trfBone.SetLocalScale((float) (this.dictSrc[62].vctScl.x * this.dictSrc[64].vctScl.x * this.dictSrc[58].vctScl.x), 1f, (float) (this.dictSrc[62].vctScl.z * this.dictSrc[64].vctScl.z * this.dictSrc[60].vctScl.z));
    this.dictDst[31].trfBone.SetLocalScale((float) (this.dictSrc[67].vctScl.x * this.dictSrc[69].vctScl.x * this.dictSrc[65].vctScl.x), 1f, (float) (this.dictSrc[67].vctScl.z * this.dictSrc[69].vctScl.z * this.dictSrc[65].vctScl.z));
    this.dictDst[32].trfBone.SetLocalScale((float) (this.dictSrc[68].vctScl.x * this.dictSrc[70].vctScl.x * this.dictSrc[66].vctScl.x), 1f, (float) (this.dictSrc[68].vctScl.z * this.dictSrc[70].vctScl.z * this.dictSrc[66].vctScl.z));
    this.dictDst[33].trfBone.SetLocalScale((float) (this.dictSrc[73].vctScl.x * this.dictSrc[75].vctScl.x * this.dictSrc[71].vctScl.x), 1f, (float) (this.dictSrc[73].vctScl.z * this.dictSrc[75].vctScl.z * this.dictSrc[71].vctScl.z));
    this.dictDst[34].trfBone.SetLocalScale((float) (this.dictSrc[74].vctScl.x * this.dictSrc[76].vctScl.x * this.dictSrc[72].vctScl.x), 1f, (float) (this.dictSrc[74].vctScl.z * this.dictSrc[76].vctScl.z * this.dictSrc[72].vctScl.z));
    this.dictDst[19].trfBone.SetLocalPositionZ((float) this.dictSrc[39].vctPos.z);
    this.dictDst[19].trfBone.SetLocalScale((float) this.dictSrc[39].vctScl.x, 1f, (float) this.dictSrc[39].vctScl.z);
    this.dictDst[20].trfBone.SetLocalPositionZ((float) this.dictSrc[40].vctPos.z);
    this.dictDst[20].trfBone.SetLocalScale((float) this.dictSrc[40].vctScl.x, 1f, (float) this.dictSrc[40].vctScl.z);
    this.dictDst[21].trfBone.SetLocalPositionZ((float) this.dictSrc[43].vctPos.z);
    this.dictDst[21].trfBone.SetLocalScale((float) (this.dictSrc[45].vctScl.x * this.dictSrc[43].vctScl.x * this.dictSrc[41].vctScl.x), 1f, (float) (this.dictSrc[45].vctScl.z * this.dictSrc[43].vctScl.z * this.dictSrc[41].vctScl.z));
    this.dictDst[22].trfBone.SetLocalPositionZ((float) this.dictSrc[44].vctPos.z);
    this.dictDst[22].trfBone.SetLocalScale((float) (this.dictSrc[46].vctScl.x * this.dictSrc[44].vctScl.x * this.dictSrc[42].vctScl.x), 1f, (float) (this.dictSrc[46].vctScl.z * this.dictSrc[44].vctScl.z * this.dictSrc[42].vctScl.z));
    this.dictDst[23].trfBone.SetLocalRotation((float) this.dictSrc[49].vctRot.x, 0.0f, 0.0f);
    this.dictDst[23].trfBone.SetLocalScale((float) (this.dictSrc[47].vctScl.x * this.dictSrc[49].vctScl.x), 1f, (float) (this.dictSrc[47].vctScl.z * this.dictSrc[49].vctScl.z));
    this.dictDst[24].trfBone.SetLocalRotation((float) this.dictSrc[50].vctRot.x, 0.0f, 0.0f);
    this.dictDst[24].trfBone.SetLocalScale((float) (this.dictSrc[48].vctScl.x * this.dictSrc[50].vctScl.x), 1f, (float) (this.dictSrc[48].vctScl.z * this.dictSrc[50].vctScl.z));
    this.dictDst[25].trfBone.SetLocalScale((float) (this.dictSrc[51].vctScl.x * this.dictSrc[53].vctScl.x), 1f, (float) (this.dictSrc[51].vctScl.z * this.dictSrc[53].vctScl.z));
    this.dictDst[26].trfBone.SetLocalScale((float) (this.dictSrc[52].vctScl.x * this.dictSrc[54].vctScl.x), 1f, (float) (this.dictSrc[52].vctScl.z * this.dictSrc[54].vctScl.z));
    this.dictDst[27].trfBone.SetLocalPositionX((float) this.dictSrc[55].vctPos.x);
    this.dictDst[27].trfBone.SetLocalPositionZ((float) this.dictSrc[55].vctPos.z);
    this.dictDst[27].trfBone.SetLocalRotation((float) this.dictSrc[55].vctRot.x, 0.0f, (float) this.dictSrc[55].vctRot.z);
    this.dictDst[27].trfBone.SetLocalScale((float) this.dictSrc[55].vctScl.x, 1f, (float) this.dictSrc[55].vctScl.z);
    this.dictDst[28].trfBone.SetLocalPositionX((float) this.dictSrc[56].vctPos.x);
    this.dictDst[28].trfBone.SetLocalPositionZ((float) this.dictSrc[55].vctPos.z);
    this.dictDst[28].trfBone.SetLocalRotation((float) this.dictSrc[56].vctRot.x, 0.0f, (float) this.dictSrc[56].vctRot.z);
    this.dictDst[28].trfBone.SetLocalScale((float) this.dictSrc[56].vctScl.x, 1f, (float) this.dictSrc[56].vctScl.z);
    this.dictDst[64].trfBone.SetLocalPosition((float) this.dictSrc[144].vctPos.x, (float) (this.dictSrc[146].vctPos.y + this.dictSrc[144].vctPos.y), (float) (this.dictSrc[142].vctPos.z + this.dictSrc[144].vctPos.z));
    this.dictDst[64].trfBone.SetLocalRotation((float) this.dictSrc[146].vctRot.x, 0.0f, 0.0f);
    this.dictDst[64].trfBone.SetLocalScale((float) (this.dictSrc[140].vctScl.x * this.dictSrc[142].vctScl.x * this.dictSrc[144].vctScl.x), (float) this.dictSrc[144].vctScl.y, (float) (this.dictSrc[138].vctScl.z * this.dictSrc[140].vctScl.z * this.dictSrc[142].vctScl.z * this.dictSrc[144].vctScl.z));
    this.dictDst[65].trfBone.SetLocalPosition((float) this.dictSrc[145].vctPos.x, (float) (this.dictSrc[147].vctPos.y + this.dictSrc[145].vctPos.y), (float) (this.dictSrc[143].vctPos.z + this.dictSrc[145].vctPos.z));
    this.dictDst[65].trfBone.SetLocalRotation((float) this.dictSrc[147].vctRot.x, 0.0f, 0.0f);
    this.dictDst[65].trfBone.SetLocalScale((float) (this.dictSrc[141].vctScl.x * this.dictSrc[143].vctScl.x * this.dictSrc[145].vctScl.x), (float) this.dictSrc[145].vctScl.y, (float) (this.dictSrc[139].vctScl.z * this.dictSrc[141].vctScl.z * this.dictSrc[143].vctScl.z * this.dictSrc[145].vctScl.z));
    this.dictDst[82].trfBone.SetLocalPositionY(num1 + num3);
    this.dictDst[82].trfBone.SetLocalPositionZ(z1 + num4);
    this.dictDst[82].trfBone.SetLocalScale((float) ((double) x1 * (double) num6 * this.dictSrc[205].vctScl.x), num2 * num5, x1 * num6);
    this.dictDst[83].trfBone.SetLocalPositionX(-x2);
    this.dictDst[83].trfBone.SetLocalPositionY((float) this.dictSrc[197].vctPos.y);
    this.dictDst[83].trfBone.SetLocalRotation((float) this.dictSrc[197].vctRot.x, 0.0f, -z2);
    this.dictDst[83].trfBone.SetLocalScale(x3, y1, 1f);
    this.dictDst[84].trfBone.SetLocalPositionX(x2);
    this.dictDst[84].trfBone.SetLocalPositionY((float) this.dictSrc[197].vctPos.y);
    this.dictDst[84].trfBone.SetLocalRotation((float) this.dictSrc[197].vctRot.x, 0.0f, z2);
    this.dictDst[84].trfBone.SetLocalScale(x3, y1, 1f);
    this.dictDst[85].trfBone.SetLocalPositionX((float) -((double) x4 + (double) x5 + (double) x7));
    this.dictDst[85].trfBone.SetLocalPositionY(y2 + y4);
    this.dictDst[85].trfBone.SetLocalPositionZ(num9 + z3 + z4 + z5);
    this.dictDst[85].trfBone.SetLocalRotation(x9, 0.0f, 0.0f);
    this.dictDst[85].trfBone.SetLocalScale((float) ((double) num7 * (double) x6 * (double) x8 * (double) x10 / this.dictSrc[205].vctScl.x), num8 * y3, num7 * x6 * x8);
    this.dictDst[86].trfBone.SetLocalPositionX(x4 + x5 + x7);
    this.dictDst[86].trfBone.SetLocalPositionY(y2 + y4);
    this.dictDst[86].trfBone.SetLocalPositionZ(num9 + z3 + z4 + z5);
    this.dictDst[86].trfBone.SetLocalRotation(x9, 0.0f, 0.0f);
    this.dictDst[86].trfBone.SetLocalScale((float) ((double) num7 * (double) x6 * (double) x8 * (double) x10 / this.dictSrc[205].vctScl.x), num8 * y3, num7 * x6 * x8);
    this.dictDst[87].trfBone.SetLocalPositionX((float) (this.dictSrc[206].vctPos.x + this.dictSrc[209].vctPos.x));
    this.dictDst[87].trfBone.SetLocalScale((float) (this.dictSrc[206].vctScl.x * this.dictSrc[209].vctScl.x), 1f, (float) (this.dictSrc[208].vctScl.z * this.dictSrc[209].vctScl.z));
    this.dictDst[88].trfBone.SetLocalPositionX((float) (this.dictSrc[207].vctPos.x + this.dictSrc[210].vctPos.x));
    this.dictDst[88].trfBone.SetLocalScale((float) (this.dictSrc[207].vctScl.x * this.dictSrc[210].vctScl.x), 1f, (float) (this.dictSrc[208].vctScl.z * this.dictSrc[210].vctScl.z));
  }

  public override void UpdateAlways()
  {
    if (this.InitEnd)
      ;
  }

  public enum DstName
  {
    cf_J_ArmElbo_low_s_L,
    cf_J_ArmElbo_low_s_R,
    cf_J_ArmLow01_s_L,
    cf_J_ArmLow01_s_R,
    cf_J_ArmLow02_s_L,
    cf_J_ArmLow02_s_R,
    cf_J_ArmUp01_s_L,
    cf_J_ArmUp01_s_R,
    cf_J_ArmUp02_s_L,
    cf_J_ArmUp02_s_R,
    cf_J_ArmUp03_s_L,
    cf_J_ArmUp03_s_R,
    cf_J_Hand_s_L,
    cf_J_Hand_s_R,
    cf_J_Hand_Wrist_s_L,
    cf_J_Hand_Wrist_s_R,
    cf_J_Head_s,
    cf_J_Kosi01_s,
    cf_J_Kosi02_s,
    cf_J_LegKnee_back_s_L,
    cf_J_LegKnee_back_s_R,
    cf_J_LegKnee_low_s_L,
    cf_J_LegKnee_low_s_R,
    cf_J_LegLow01_s_L,
    cf_J_LegLow01_s_R,
    cf_J_LegLow02_s_L,
    cf_J_LegLow02_s_R,
    cf_J_LegLow03_s_L,
    cf_J_LegLow03_s_R,
    cf_J_LegUp01_s_L,
    cf_J_LegUp01_s_R,
    cf_J_LegUp02_s_L,
    cf_J_LegUp02_s_R,
    cf_J_LegUp03_s_L,
    cf_J_LegUp03_s_R,
    cf_J_LegUpDam_s_L,
    cf_J_LegUpDam_s_R,
    cf_J_Mune_Nip01_s_L,
    cf_J_Mune_Nip01_s_R,
    cf_J_Mune_Nip02_s_L,
    cf_J_Mune_Nip02_s_R,
    cf_J_Mune_Nipacs01_L,
    cf_J_Mune_Nipacs01_R,
    cf_J_Mune00_d_L,
    cf_J_Mune00_d_R,
    cf_J_Mune00_s_L,
    cf_J_Mune00_s_R,
    cf_J_Mune00_t_L,
    cf_J_Mune00_t_R,
    cf_J_Mune01_s_L,
    cf_J_Mune01_s_R,
    cf_J_Mune01_t_L,
    cf_J_Mune01_t_R,
    cf_J_Mune02_s_L,
    cf_J_Mune02_s_R,
    cf_J_Mune02_t_L,
    cf_J_Mune02_t_R,
    cf_J_Mune03_s_L,
    cf_J_Mune03_s_R,
    cf_J_Mune04_s_L,
    cf_J_Mune04_s_R,
    cf_J_Neck_s,
    cf_J_Shoulder02_s_L,
    cf_J_Shoulder02_s_R,
    cf_J_Siri_s_L,
    cf_J_Siri_s_R,
    cf_J_sk_siri_dam,
    cf_J_sk_top,
    cf_J_Spine01_s,
    cf_J_Spine02_s,
    cf_J_Spine03_s,
    cf_N_height,
    cf_J_sk_00_00_dam,
    cf_J_sk_01_00_dam,
    cf_J_sk_02_00_dam,
    cf_J_sk_03_00_dam,
    cf_J_sk_04_00_dam,
    cf_J_sk_05_00_dam,
    cf_J_sk_06_00_dam,
    cf_J_sk_07_00_dam,
    cf_hit_Mune02_s_L,
    cf_hit_Mune02_s_R,
    cf_hit_Kosi02_s,
    cf_hit_LegUp01_s_L,
    cf_hit_LegUp01_s_R,
    cf_hit_Siri_s_L,
    cf_hit_Siri_s_R,
    cf_J_Legsk_root_L,
    cf_J_Legsk_root_R,
  }

  public enum SrcName
  {
    cf_s_ArmElbo_low_s_L,
    cf_s_ArmElbo_low_s_R,
    cf_s_ArmElbo_up_s_L,
    cf_s_ArmElbo_up_s_R,
    cf_s_ArmLow01_h_L,
    cf_s_ArmLow01_h_R,
    cf_s_ArmLow01_s_L,
    cf_s_ArmLow01_s_R,
    cf_s_ArmLow02_h_L,
    cf_s_ArmLow02_h_R,
    cf_s_ArmLow02_s_L,
    cf_s_ArmLow02_s_R,
    cf_s_ArmUp01_h_L,
    cf_s_ArmUp01_h_R,
    cf_s_ArmUp01_s_L,
    cf_s_ArmUp01_s_R,
    cf_s_ArmUp01_s_tx_L,
    cf_s_ArmUp01_s_tx_R,
    cf_s_ArmUp02_h_L,
    cf_s_ArmUp02_h_R,
    cf_s_ArmUp02_s_L,
    cf_s_ArmUp02_s_R,
    cf_s_ArmUp03_h_L,
    cf_s_ArmUp03_h_R,
    cf_s_ArmUp03_s_L,
    cf_s_ArmUp03_s_R,
    cf_s_Hand_h_L,
    cf_s_Hand_h_R,
    cf_s_Hand_Wrist_s_L,
    cf_s_Hand_Wrist_s_R,
    cf_s_Head_h,
    cf_s_Head_s,
    cf_s_height,
    cf_s_Kosi01_h,
    cf_s_Kosi01_s,
    cf_s_Kosi01_s_sz,
    cf_s_Kosi02_h,
    cf_s_Kosi02_s,
    cf_s_Kosi02_s_sz,
    cf_s_LegKnee_back_s_L,
    cf_s_LegKnee_back_s_R,
    cf_s_LegKnee_h_L,
    cf_s_LegKnee_h_R,
    cf_s_LegKnee_low_s_L,
    cf_s_LegKnee_low_s_R,
    cf_s_LegKnee_up_s_L,
    cf_s_LegKnee_up_s_R,
    cf_s_LegLow01_h_L,
    cf_s_LegLow01_h_R,
    cf_s_LegLow01_s_L,
    cf_s_LegLow01_s_R,
    cf_s_LegLow02_h_L,
    cf_s_LegLow02_h_R,
    cf_s_LegLow02_s_L,
    cf_s_LegLow02_s_R,
    cf_s_LegLow03_s_L,
    cf_s_LegLow03_s_R,
    cf_s_LegUp01_blend_s_L,
    cf_s_LegUp01_blend_s_R,
    cf_s_LegUp01_blend_ss_L,
    cf_s_LegUp01_blend_ss_R,
    cf_s_LegUp01_h_L,
    cf_s_LegUp01_h_R,
    cf_s_LegUp01_s_L,
    cf_s_LegUp01_s_R,
    cf_s_LegUp02_blend_s_L,
    cf_s_LegUp02_blend_s_R,
    cf_s_LegUp02_h_L,
    cf_s_LegUp02_h_R,
    cf_s_LegUp02_s_L,
    cf_s_LegUp02_s_R,
    cf_s_LegUp03_blend_s_L,
    cf_s_LegUp03_blend_s_R,
    cf_s_LegUp03_h_L,
    cf_s_LegUp03_h_R,
    cf_s_LegUp03_s_L,
    cf_s_LegUp03_s_R,
    cf_s_LegUpDam_s_L,
    cf_s_LegUpDam_s_R,
    cf_s_Mune_Nip_dam_L,
    cf_s_Mune_Nip_dam_R,
    cf_s_Mune_Nip01_s_L,
    cf_s_Mune_Nip01_s_R,
    cf_s_Mune_Nip01_ss_L,
    cf_s_Mune_Nip01_ss_R,
    cf_s_Mune_Nip02_s_L,
    cf_s_Mune_Nip02_s_R,
    cf_s_Mune_Nipacs01_L,
    cf_s_Mune_Nipacs01_R,
    cf_s_Mune_Nipacs02_L,
    cf_s_Mune_Nipacs02_R,
    cf_s_Mune00_h_L,
    cf_s_Mune00_h_R,
    cf_s_Mune00_s_L,
    cf_s_Mune00_s_R,
    cf_s_Mune00_ss_02_L,
    cf_s_Mune00_ss_02_R,
    cf_s_Mune00_ss_02sz_L,
    cf_s_Mune00_ss_02sz_R,
    cf_s_Mune00_ss_03_L,
    cf_s_Mune00_ss_03_R,
    cf_s_Mune00_ss_03sz_L,
    cf_s_Mune00_ss_03sz_R,
    cf_s_Mune00_ss_ty_L,
    cf_s_Mune00_ss_ty_R,
    cf_s_Mune01_s_L,
    cf_s_Mune01_s_R,
    cf_s_Mune01_s_rx_L,
    cf_s_Mune01_s_rx_R,
    cf_s_Mune01_s_ry_L,
    cf_s_Mune01_s_ry_R,
    cf_s_Mune01_s_tx_L,
    cf_s_Mune01_s_tx_R,
    cf_s_Mune01_s_tz_L,
    cf_s_Mune01_s_tz_R,
    cf_s_Mune02_s_L,
    cf_s_Mune02_s_R,
    cf_s_Mune02_s_rx_L,
    cf_s_Mune02_s_rx_R,
    cf_s_Mune02_s_tz_L,
    cf_s_Mune02_s_tz_R,
    cf_s_Mune03_s_L,
    cf_s_Mune03_s_R,
    cf_s_Mune03_s_rx_L,
    cf_s_Mune03_s_rx_R,
    cf_s_Mune04_s_L,
    cf_s_Mune04_s_R,
    cf_s_Neck_h,
    cf_s_Neck_s,
    cf_s_Neck_s_sz,
    cf_s_Shoulder_h_L,
    cf_s_Shoulder_h_R,
    cf_s_Shoulder02_h_L,
    cf_s_Shoulder02_h_R,
    cf_s_Shoulder02_s_L,
    cf_s_Shoulder02_s_R,
    cf_s_Shoulder02_s_tx_L,
    cf_s_Shoulder02_s_tx_R,
    cf_s_Siri_kosi01_s_L,
    cf_s_Siri_kosi01_s_R,
    cf_s_Siri_kosi02_s_L,
    cf_s_Siri_kosi02_s_R,
    cf_s_Siri_legup01_s_L,
    cf_s_Siri_legup01_s_R,
    cf_s_Siri_s_L,
    cf_s_Siri_s_R,
    cf_s_Siri_s_ty_L,
    cf_s_Siri_s_ty_R,
    cf_s_sk_siri_dam,
    cf_s_sk_siri_ty_dam,
    cf_s_sk_top_h,
    cf_s_Spine01_h,
    cf_s_Spine01_s,
    cf_s_Spine01_s_sz,
    cf_s_Spine01_s_ty,
    cf_s_Spine02_h,
    cf_s_Spine02_s,
    cf_s_Spine02_s_sz,
    cf_s_Spine03_h,
    cf_s_Spine03_s,
    cf_s_Spine03_s_sz,
    cf_s_sk_00_sx01,
    cf_s_sk_00_sx02,
    cf_s_sk_00_sz01,
    cf_s_sk_00_sz02,
    cf_s_sk_01_sx01,
    cf_s_sk_01_sx02,
    cf_s_sk_01_sz01,
    cf_s_sk_01_sz02,
    cf_s_sk_02_sx01,
    cf_s_sk_02_sx02,
    cf_s_sk_02_sz01,
    cf_s_sk_02_sz02,
    cf_s_sk_03_sx01,
    cf_s_sk_03_sx02,
    cf_s_sk_03_sz01,
    cf_s_sk_03_sz02,
    cf_s_sk_04_sx01,
    cf_s_sk_04_sx02,
    cf_s_sk_04_sz01,
    cf_s_sk_04_sz02,
    cf_s_sk_05_sx01,
    cf_s_sk_05_sx02,
    cf_s_sk_05_sz01,
    cf_s_sk_05_sz02,
    cf_s_sk_06_sx01,
    cf_s_sk_06_sx02,
    cf_s_sk_06_sz01,
    cf_s_sk_06_sz02,
    cf_s_sk_07_sx01,
    cf_s_sk_07_sx02,
    cf_s_sk_07_sz01,
    cf_s_sk_07_sz02,
    cf_hit_Kosi02_Kosi01sx_a,
    cf_hit_Kosi02_Kosi01sz_a,
    cf_hit_Kosi02_Kosi02sx_a,
    cf_hit_Kosi02_Kosi02sz_a,
    cf_hit_LegUp01_Kosi02sz_a,
    cf_hit_LegUp01_Kosi02sx_a,
    cf_hit_Siri_Kosi02sz_a,
    cf_hit_Siri_Kosi02sx_a,
    cf_hit_Siri_size_a,
    cf_hit_Siri_rot_a,
    cf_hit_Mune_size_a,
    cf_hit_Siri_LegUp01,
    cf_hit_height,
    cf_s_legskroot_kosi02_sx_L,
    cf_s_legskroot_kosi02_sx_R,
    cf_s_legskroot_kosi02_sz,
    cf_s_legskroot_leg01_L,
    cf_s_legskroot_leg01_R,
  }
}
