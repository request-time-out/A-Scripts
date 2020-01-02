// Decompiled with JetBrains decompiler
// Type: DynamicBoneReferenceCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DynamicBoneReferenceCtrl
{
  private DynamicBone_Ver02.BonePtn[] bonePtns = new DynamicBone_Ver02.BonePtn[2];
  private List<Transform>[] lstsTrans = new List<Transform>[2];
  private List<string> row = new List<string>();
  private StringBuilder sbAssetName = new StringBuilder();
  private List<DynamicBoneReferenceCtrl.Reference>[] lstsRef = new List<DynamicBoneReferenceCtrl.Reference>[2];
  private bool isInit;
  private ChaControl chaFemale;

  public bool Init(ChaControl _female)
  {
    this.chaFemale = _female;
    if (Object.op_Equality((Object) this.chaFemale, (Object) null))
      return false;
    DynamicBone_Ver02[] dynamicBoneVer02Array = new DynamicBone_Ver02[2]
    {
      this.chaFemale.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastL),
      this.chaFemale.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastR)
    };
    for (int index1 = 0; index1 < 2; ++index1)
    {
      this.lstsRef[index1] = new List<DynamicBoneReferenceCtrl.Reference>();
      this.bonePtns[index1] = new DynamicBone_Ver02.BonePtn();
      this.lstsTrans[index1] = new List<Transform>();
      if (Object.op_Inequality((Object) dynamicBoneVer02Array[index1], (Object) null) && dynamicBoneVer02Array[index1].Patterns.Count > 0)
      {
        this.bonePtns[index1] = dynamicBoneVer02Array[index1].Patterns[0];
        for (int index2 = 1; index2 < this.bonePtns[index1].Params.Count; ++index2)
          this.lstsTrans[index1].Add(this.bonePtns[index1].Params[index2].RefTransform);
      }
    }
    return true;
  }

  public bool Load(string _assetpath, string _file)
  {
    List<bool>[] boolListArray1 = new List<bool>[2];
    List<bool>[] boolListArray2 = new List<bool>[2];
    this.isInit = false;
    for (int index = 0; index < 2; ++index)
    {
      this.InitDynamicBoneReferenceBone(this.bonePtns[index], this.lstsTrans[index]);
      if (this.lstsRef[index] != null)
        this.lstsRef[index].Clear();
      boolListArray1[index] = new List<bool>();
      boolListArray1[index].Add(false);
      boolListArray2[index] = new List<bool>();
      boolListArray2[index].Add(false);
    }
    if (_file == string.Empty)
      return false;
    this.sbAssetName.Clear();
    this.sbAssetName.Append(_file.Remove(3, 2));
    List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_assetpath, false);
    for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
    {
      if (!GlobalMethod.AssetFileExist(nameListFromPath[index1], this.sbAssetName.ToString(), string.Empty))
        return false;
      ExcelData excelData = CommonLib.LoadAsset<ExcelData>(nameListFromPath[index1], this.sbAssetName.ToString(), false, string.Empty);
      Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(nameListFromPath[index1]);
      if (Object.op_Equality((Object) excelData, (Object) null))
        return false;
      int num1 = 3;
      while (num1 < excelData.MaxCell)
      {
        int num2 = num1 - 3;
        this.row = excelData.list[num1++].list;
        int num3 = 2;
        DynamicBoneReferenceCtrl.Reference reference;
        ref DynamicBoneReferenceCtrl.RateInfo local1 = ref reference.position;
        List<string> row1 = this.row;
        int index2 = num3;
        int num4 = index2 + 1;
        double num5 = (double) float.Parse(row1.GetElement<string>(index2));
        List<string> row2 = this.row;
        int index3 = num4;
        int num6 = index3 + 1;
        double num7 = (double) float.Parse(row2.GetElement<string>(index3));
        List<string> row3 = this.row;
        int index4 = num6;
        int num8 = index4 + 1;
        double num9 = (double) float.Parse(row3.GetElement<string>(index4));
        Vector3 vector3_1 = new Vector3((float) num5, (float) num7, (float) num9);
        local1.sizeS = vector3_1;
        ref DynamicBoneReferenceCtrl.RateInfo local2 = ref reference.position;
        List<string> row4 = this.row;
        int index5 = num8;
        int num10 = index5 + 1;
        double num11 = (double) float.Parse(row4.GetElement<string>(index5));
        List<string> row5 = this.row;
        int index6 = num10;
        int num12 = index6 + 1;
        double num13 = (double) float.Parse(row5.GetElement<string>(index6));
        List<string> row6 = this.row;
        int index7 = num12;
        int num14 = index7 + 1;
        double num15 = (double) float.Parse(row6.GetElement<string>(index7));
        Vector3 vector3_2 = new Vector3((float) num11, (float) num13, (float) num15);
        local2.sizeM = vector3_2;
        ref DynamicBoneReferenceCtrl.RateInfo local3 = ref reference.position;
        List<string> row7 = this.row;
        int index8 = num14;
        int num16 = index8 + 1;
        double num17 = (double) float.Parse(row7.GetElement<string>(index8));
        List<string> row8 = this.row;
        int index9 = num16;
        int num18 = index9 + 1;
        double num19 = (double) float.Parse(row8.GetElement<string>(index9));
        List<string> row9 = this.row;
        int index10 = num18;
        int num20 = index10 + 1;
        double num21 = (double) float.Parse(row9.GetElement<string>(index10));
        Vector3 vector3_3 = new Vector3((float) num17, (float) num19, (float) num21);
        local3.sizeL = vector3_3;
        ref DynamicBoneReferenceCtrl.RateInfo local4 = ref reference.rotation;
        List<string> row10 = this.row;
        int index11 = num20;
        int num22 = index11 + 1;
        double num23 = (double) float.Parse(row10.GetElement<string>(index11));
        List<string> row11 = this.row;
        int index12 = num22;
        int num24 = index12 + 1;
        double num25 = (double) float.Parse(row11.GetElement<string>(index12));
        List<string> row12 = this.row;
        int index13 = num24;
        int num26 = index13 + 1;
        double num27 = (double) float.Parse(row12.GetElement<string>(index13));
        Vector3 vector3_4 = new Vector3((float) num23, (float) num25, (float) num27);
        local4.sizeS = vector3_4;
        ref DynamicBoneReferenceCtrl.RateInfo local5 = ref reference.rotation;
        List<string> row13 = this.row;
        int index14 = num26;
        int num28 = index14 + 1;
        double num29 = (double) float.Parse(row13.GetElement<string>(index14));
        List<string> row14 = this.row;
        int index15 = num28;
        int num30 = index15 + 1;
        double num31 = (double) float.Parse(row14.GetElement<string>(index15));
        List<string> row15 = this.row;
        int index16 = num30;
        int num32 = index16 + 1;
        double num33 = (double) float.Parse(row15.GetElement<string>(index16));
        Vector3 vector3_5 = new Vector3((float) num29, (float) num31, (float) num33);
        local5.sizeM = vector3_5;
        ref DynamicBoneReferenceCtrl.RateInfo local6 = ref reference.rotation;
        List<string> row16 = this.row;
        int index17 = num32;
        int num34 = index17 + 1;
        double num35 = (double) float.Parse(row16.GetElement<string>(index17));
        List<string> row17 = this.row;
        int index18 = num34;
        int num36 = index18 + 1;
        double num37 = (double) float.Parse(row17.GetElement<string>(index18));
        List<string> row18 = this.row;
        int index19 = num36;
        int num38 = index19 + 1;
        double num39 = (double) float.Parse(row18.GetElement<string>(index19));
        Vector3 vector3_6 = new Vector3((float) num35, (float) num37, (float) num39);
        local6.sizeL = vector3_6;
        ref DynamicBoneReferenceCtrl.RateInfo local7 = ref reference.scale;
        List<string> row19 = this.row;
        int index20 = num38;
        int num40 = index20 + 1;
        double num41 = (double) float.Parse(row19.GetElement<string>(index20));
        List<string> row20 = this.row;
        int index21 = num40;
        int num42 = index21 + 1;
        double num43 = (double) float.Parse(row20.GetElement<string>(index21));
        List<string> row21 = this.row;
        int index22 = num42;
        int num44 = index22 + 1;
        double num45 = (double) float.Parse(row21.GetElement<string>(index22));
        Vector3 vector3_7 = new Vector3((float) num41, (float) num43, (float) num45);
        local7.sizeS = vector3_7;
        ref DynamicBoneReferenceCtrl.RateInfo local8 = ref reference.scale;
        List<string> row22 = this.row;
        int index23 = num44;
        int num46 = index23 + 1;
        double num47 = (double) float.Parse(row22.GetElement<string>(index23));
        List<string> row23 = this.row;
        int index24 = num46;
        int num48 = index24 + 1;
        double num49 = (double) float.Parse(row23.GetElement<string>(index24));
        List<string> row24 = this.row;
        int index25 = num48;
        int num50 = index25 + 1;
        double num51 = (double) float.Parse(row24.GetElement<string>(index25));
        Vector3 vector3_8 = new Vector3((float) num47, (float) num49, (float) num51);
        local8.sizeM = vector3_8;
        ref DynamicBoneReferenceCtrl.RateInfo local9 = ref reference.scale;
        List<string> row25 = this.row;
        int index26 = num50;
        int num52 = index26 + 1;
        double num53 = (double) float.Parse(row25.GetElement<string>(index26));
        List<string> row26 = this.row;
        int index27 = num52;
        int num54 = index27 + 1;
        double num55 = (double) float.Parse(row26.GetElement<string>(index27));
        List<string> row27 = this.row;
        int index28 = num54;
        int num56 = index28 + 1;
        double num57 = (double) float.Parse(row27.GetElement<string>(index28));
        Vector3 vector3_9 = new Vector3((float) num53, (float) num55, (float) num57);
        local9.sizeL = vector3_9;
        List<bool> boolList1 = boolListArray1[num2 / 4];
        List<string> row28 = this.row;
        int index29 = num56;
        int num58 = index29 + 1;
        int num59 = row28.GetElement<string>(index29) == "1" ? 1 : 0;
        boolList1.Add(num59 != 0);
        List<bool> boolList2 = boolListArray2[num2 / 4];
        List<string> row29 = this.row;
        int index30 = num58;
        int num60 = index30 + 1;
        int num61 = row29.GetElement<string>(index30) == "1" ? 1 : 0;
        boolList2.Add(num61 != 0);
        this.lstsRef[num2 / 4].Add(reference);
      }
    }
    for (int index = 0; index < 2; ++index)
    {
      this.SetDynamicBoneRotationCalc(this.bonePtns[index], boolListArray1[index]);
      this.SetDynamicBoneMoveLimitFlag(this.bonePtns[index], boolListArray2[index]);
    }
    this.isInit = true;
    return true;
  }

  public bool Proc()
  {
    if (!this.isInit)
      return false;
    float shapeBodyValue = this.chaFemale.GetShapeBodyValue(1);
    for (int index1 = 0; index1 < 2; ++index1)
    {
      if (this.lstsTrans[index1].Count == this.lstsRef[index1].Count)
      {
        for (int index2 = 0; index2 < this.lstsRef[index1].Count; ++index2)
        {
          if (!Object.op_Equality((Object) this.lstsTrans[index1][index2], (Object) null))
          {
            this.lstsTrans[index1][index2].set_localPosition(this.lstsRef[index1][index2].position.Calc(shapeBodyValue));
            this.lstsTrans[index1][index2].set_localRotation(Quaternion.Euler(this.lstsRef[index1][index2].rotation.Calc(shapeBodyValue)));
            this.lstsTrans[index1][index2].set_localScale(this.lstsRef[index1][index2].scale.Calc(shapeBodyValue));
          }
        }
      }
    }
    return true;
  }

  public bool InitDynamicBoneReferenceBone()
  {
    if (!this.isInit)
      return false;
    for (int index = 0; index < 2; ++index)
      this.InitDynamicBoneReferenceBone(this.bonePtns[index], this.lstsTrans[index]);
    return true;
  }

  private bool InitDynamicBoneReferenceBone(
    DynamicBone_Ver02.BonePtn _ptn,
    List<Transform> _lstTrans)
  {
    if (_ptn == null || _lstTrans == null)
      return false;
    List<bool> _lstBool1 = new List<bool>()
    {
      false,
      true,
      false,
      true,
      false
    };
    this.SetDynamicBoneRotationCalc(_ptn, _lstBool1);
    List<bool> _lstBool2 = new List<bool>()
    {
      false,
      false,
      false,
      false,
      false
    };
    this.SetDynamicBoneMoveLimitFlag(_ptn, _lstBool2);
    using (List<Transform>.Enumerator enumerator = _lstTrans.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        Transform current = enumerator.Current;
        if (!Object.op_Equality((Object) current, (Object) null))
        {
          current.set_localPosition(Vector3.get_zero());
          current.set_localRotation(Quaternion.get_identity());
          current.set_localScale(Vector3.get_one());
        }
      }
    }
    return true;
  }

  private bool SetDynamicBoneRotationCalc(DynamicBone_Ver02.BonePtn _ptn, List<bool> _lstBool)
  {
    if (_ptn == null || _lstBool == null || _lstBool.Count != _ptn.Params.Count)
      return false;
    for (int index = 0; index < _ptn.Params.Count; ++index)
      _ptn.Params[index].IsRotationCalc = _lstBool[index];
    for (int index = 0; index < _ptn.ParticlePtns.Count && index < _lstBool.Count; ++index)
      _ptn.ParticlePtns[index].IsRotationCalc = _lstBool[index];
    return true;
  }

  private bool SetDynamicBoneMoveLimitFlag(DynamicBone_Ver02.BonePtn _ptn, List<bool> _lstBool)
  {
    if (_ptn == null || _lstBool == null || _lstBool.Count != _ptn.Params.Count)
      return false;
    for (int index = 0; index < _ptn.Params.Count; ++index)
      _ptn.Params[index].IsMoveLimit = _lstBool[index];
    for (int index = 0; index < _ptn.ParticlePtns.Count && index < _lstBool.Count; ++index)
      _ptn.ParticlePtns[index].IsMoveLimit = _lstBool[index];
    return true;
  }

  private struct RateInfo
  {
    public Vector3 sizeS;
    public Vector3 sizeM;
    public Vector3 sizeL;

    public Vector3 Calc(float _rate)
    {
      return (double) _rate >= 0.5 ? Vector3.Lerp(this.sizeM, this.sizeL, Mathf.InverseLerp(0.5f, 1f, _rate)) : Vector3.Lerp(this.sizeS, this.sizeM, Mathf.InverseLerp(0.0f, 0.5f, _rate));
    }
  }

  private struct Reference
  {
    public DynamicBoneReferenceCtrl.RateInfo position;
    public DynamicBoneReferenceCtrl.RateInfo rotation;
    public DynamicBoneReferenceCtrl.RateInfo scale;
  }
}
