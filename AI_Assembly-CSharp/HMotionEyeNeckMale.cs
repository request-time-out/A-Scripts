// Decompiled with JetBrains decompiler
// Type: HMotionEyeNeckMale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HMotionEyeNeckMale : MonoBehaviour
{
  [Label("相手女顔オブジェクト名")]
  public string strFemaleHead;
  [Label("相手女性器オブジェクト名")]
  public string strFemaleGenital;
  [Label("相手男顔オブジェクト名")]
  public string strMaleHead;
  [Label("相手男性器オブジェクト名")]
  public string strMaleGenital;
  [SerializeField]
  private List<HMotionEyeNeckMale.EyeNeck> lstEyeNeck;
  [DisabledGroup("男クラス")]
  [SerializeField]
  private ChaControl chaMale;
  [DisabledGroup("女1顔オブジェクト")]
  [SerializeField]
  private GameObject objFemale1Head;
  [DisabledGroup("女1性器オブジェクト")]
  [SerializeField]
  private GameObject objFemale1Genital;
  [DisabledGroup("女2顔オブジェクト")]
  [SerializeField]
  private GameObject objFemale2Head;
  [DisabledGroup("女2性器オブジェクト")]
  [SerializeField]
  private GameObject objFemale2Genital;
  [DisabledGroup("男顔オブジェクト")]
  [SerializeField]
  private GameObject objMaleHead;
  [DisabledGroup("男性器オブジェクト")]
  [SerializeField]
  private GameObject objMaleGenital;
  [DisabledGroup("自分性器オブジェクト")]
  [SerializeField]
  private GameObject objGenitalSelf;
  private Transform LoopParent;
  private HMotionEyeNeckMale.EyeNeck en;
  private FBSCtrlMouth mouthCtrl;
  private AnimatorStateInfo ai;
  private Transform NeckTrs;
  private Transform HeadTrs;
  private Transform[] EyeTrs;
  private Quaternion BackUpNeck;
  private Quaternion BackUpHead;
  private Quaternion[] BackUpEye;
  private float ChangeTimeNeck;
  private int NeckType;
  private float ChangeTimeEye;
  private int EyeType;
  private bool bFaceInfo;
  private int nYuragiType;
  private string OldAnimName;
  private ExcelData excelData;
  private HMotionEyeNeckMale.EyeNeck info;
  private List<string> row;
  private string abName;
  private string assetName;
  public bool NowEndADV;
  private Transform[] getChild;
  private float[] nowleapSpeed;

  public HMotionEyeNeckMale()
  {
    base.\u002Ector();
  }

  public bool Init(ChaControl _male, int id)
  {
    this.Release();
    this.chaMale = _male;
    this.NeckTrs = this.chaMale.neckLookCtrl.neckLookScript.aBones[0].neckBone;
    this.HeadTrs = this.chaMale.neckLookCtrl.neckLookScript.aBones[1].neckBone;
    this.EyeTrs = new Transform[2]
    {
      this.chaMale.eyeLookCtrl.eyeLookScript.eyeObjs[0].eyeTransform,
      this.chaMale.eyeLookCtrl.eyeLookScript.eyeObjs[1].eyeTransform
    };
    this.BackUpNeck = this.NeckTrs.get_localRotation();
    this.BackUpHead = this.HeadTrs.get_localRotation();
    this.BackUpEye = new Quaternion[2]
    {
      this.EyeTrs[0].get_localRotation(),
      this.EyeTrs[1].get_localRotation()
    };
    this.objGenitalSelf = (GameObject) null;
    if (Object.op_Implicit((Object) this.chaMale) && Object.op_Implicit((Object) this.chaMale.objBodyBone))
    {
      this.LoopParent = this.chaMale.objBodyBone.get_transform();
      if (this.strFemaleGenital != string.Empty)
        this.objGenitalSelf = this.GetObjectName(this.LoopParent, this.strFemaleGenital);
    }
    return true;
  }

  public void Release()
  {
    this.lstEyeNeck.Clear();
  }

  private GameObject GetObjectName(Transform top, string name)
  {
    this.getChild = (Transform[]) ((Component) top).GetComponentsInChildren<Transform>();
    for (int index = 0; index < this.getChild.Length; ++index)
    {
      if (!(((Object) this.getChild[index]).get_name() != name))
        return ((Component) this.getChild[index]).get_gameObject();
    }
    return (GameObject) null;
  }

  public bool Load(string _assetpath, string _file)
  {
    this.lstEyeNeck.Clear();
    if (_file == string.Empty)
      return false;
    List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_assetpath, false);
    nameListFromPath.Sort();
    this.abName = string.Empty;
    this.assetName = string.Empty;
    this.excelData = (ExcelData) null;
    this.info.Init();
    Vector3 zero = Vector3.get_zero();
    for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
    {
      this.abName = nameListFromPath[index1];
      this.assetName = _file;
      if (GlobalMethod.AssetFileExist(this.abName, this.assetName, string.Empty))
      {
        this.excelData = CommonLib.LoadAsset<ExcelData>(this.abName, this.assetName, false, string.Empty);
        Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(this.abName);
        if (!Object.op_Equality((Object) this.excelData, (Object) null))
        {
          int num1 = 3;
          while (num1 < this.excelData.MaxCell)
          {
            this.row = this.excelData.list[num1++].list;
            int num2 = 0;
            this.info = new HMotionEyeNeckMale.EyeNeck();
            this.info.Init();
            if (this.row.Count != 0)
            {
              List<string> row1 = this.row;
              int index2 = num2;
              int num3 = index2 + 1;
              string element = row1.GetElement<string>(index2);
              if (!element.IsNullOrEmpty())
              {
                this.info.anim = element;
                ref HMotionEyeNeckMale.EyeNeck local1 = ref this.info;
                List<string> row2 = this.row;
                int index3 = num3;
                int num4 = index3 + 1;
                int num5 = int.Parse(row2.GetElement<string>(index3));
                local1.openEye = num5;
                ref HMotionEyeNeckMale.EyeNeck local2 = ref this.info;
                List<string> row3 = this.row;
                int index4 = num4;
                int num6 = index4 + 1;
                int num7 = int.Parse(row3.GetElement<string>(index4));
                local2.openMouth = num7;
                ref HMotionEyeNeckMale.EyeNeck local3 = ref this.info;
                List<string> row4 = this.row;
                int index5 = num6;
                int num8 = index5 + 1;
                int num9 = int.Parse(row4.GetElement<string>(index5));
                local3.eyebrow = num9;
                ref HMotionEyeNeckMale.EyeNeck local4 = ref this.info;
                List<string> row5 = this.row;
                int index6 = num8;
                int num10 = index6 + 1;
                int num11 = int.Parse(row5.GetElement<string>(index6));
                local4.eye = num11;
                ref HMotionEyeNeckMale.EyeNeck local5 = ref this.info;
                List<string> row6 = this.row;
                int index7 = num10;
                int num12 = index7 + 1;
                int num13 = int.Parse(row6.GetElement<string>(index7));
                local5.mouth = num13;
                ref HMotionEyeNeckMale.EyeNeck local6 = ref this.info;
                List<string> row7 = this.row;
                int index8 = num12;
                int num14 = index8 + 1;
                int num15 = int.Parse(row7.GetElement<string>(index8));
                local6.Neckbehaviour = num15;
                ref HMotionEyeNeckMale.EyeNeck local7 = ref this.info;
                List<string> row8 = this.row;
                int index9 = num14;
                int num16 = index9 + 1;
                int num17 = int.Parse(row8.GetElement<string>(index9));
                local7.Eyebehaviour = num17;
                ref HMotionEyeNeckMale.EyeNeck local8 = ref this.info;
                List<string> row9 = this.row;
                int index10 = num16;
                int num18 = index10 + 1;
                int num19 = int.Parse(row9.GetElement<string>(index10));
                local8.targetNeck = num19;
                List<string> row10 = this.row;
                int index11 = num18;
                int num20 = index11 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row10.GetElement<string>(index11), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row11 = this.row;
                int index12 = num20;
                int num21 = index12 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row11.GetElement<string>(index12), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row12 = this.row;
                int index13 = num21;
                int num22 = index13 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row12.GetElement<string>(index13), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.NeckRot[0] = zero;
                List<string> row13 = this.row;
                int index14 = num22;
                int num23 = index14 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row13.GetElement<string>(index14), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row14 = this.row;
                int index15 = num23;
                int num24 = index15 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row14.GetElement<string>(index15), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row15 = this.row;
                int index16 = num24;
                int num25 = index16 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row15.GetElement<string>(index16), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.NeckRot[1] = zero;
                List<string> row16 = this.row;
                int index17 = num25;
                int num26 = index17 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row16.GetElement<string>(index17), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row17 = this.row;
                int index18 = num26;
                int num27 = index18 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row17.GetElement<string>(index18), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row18 = this.row;
                int index19 = num27;
                int num28 = index19 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row18.GetElement<string>(index19), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.HeadRot[0] = zero;
                List<string> row19 = this.row;
                int index20 = num28;
                int num29 = index20 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row19.GetElement<string>(index20), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row20 = this.row;
                int index21 = num29;
                int num30 = index21 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row20.GetElement<string>(index21), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row21 = this.row;
                int index22 = num30;
                int num31 = index22 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row21.GetElement<string>(index22), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.HeadRot[1] = zero;
                ref HMotionEyeNeckMale.EyeNeck local9 = ref this.info;
                List<string> row22 = this.row;
                int index23 = num31;
                int num32 = index23 + 1;
                int num33 = int.Parse(row22.GetElement<string>(index23));
                local9.targetEye = num33;
                List<string> row23 = this.row;
                int index24 = num32;
                int num34 = index24 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row23.GetElement<string>(index24), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row24 = this.row;
                int index25 = num34;
                int num35 = index25 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row24.GetElement<string>(index25), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row25 = this.row;
                int index26 = num35;
                int num36 = index26 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row25.GetElement<string>(index26), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.EyeRot[0] = zero;
                List<string> row26 = this.row;
                int index27 = num36;
                int num37 = index27 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row26.GetElement<string>(index27), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row27 = this.row;
                int index28 = num37;
                int num38 = index28 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row27.GetElement<string>(index28), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row28 = this.row;
                int index29 = num38;
                int num39 = index29 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row28.GetElement<string>(index29), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.EyeRot[1] = zero;
                this.lstEyeNeck.Add(this.info);
              }
            }
          }
        }
      }
    }
    return true;
  }

  public bool SetPartner(
    GameObject _objFemale1Bone,
    GameObject _objFemale2Bone,
    GameObject _objMaleBone)
  {
    this.objFemale1Head = (GameObject) null;
    this.objFemale2Head = (GameObject) null;
    this.objMaleHead = (GameObject) null;
    this.objFemale1Genital = (GameObject) null;
    this.objFemale2Genital = (GameObject) null;
    this.objMaleGenital = (GameObject) null;
    if (Object.op_Implicit((Object) _objFemale1Bone))
    {
      this.LoopParent = _objFemale1Bone.get_transform();
      if (this.strFemaleHead != string.Empty)
        this.objFemale1Head = this.GetObjectName(this.LoopParent, this.strFemaleHead);
      if (this.strFemaleGenital != string.Empty)
        this.objFemale1Genital = this.LoopParent.FindLoop(this.strFemaleGenital);
    }
    if (Object.op_Implicit((Object) _objFemale2Bone))
    {
      this.LoopParent = _objFemale2Bone.get_transform();
      if (this.strFemaleHead != string.Empty)
        this.objFemale2Head = this.GetObjectName(this.LoopParent, this.strFemaleHead);
      if (this.strFemaleGenital != string.Empty)
        this.objFemale2Genital = this.GetObjectName(this.LoopParent, this.strFemaleGenital);
    }
    if (Object.op_Implicit((Object) _objMaleBone))
    {
      this.LoopParent = _objMaleBone.get_transform();
      if (this.strMaleHead != string.Empty)
        this.objMaleHead = this.GetObjectName(this.LoopParent, this.strMaleHead);
      if (this.strMaleGenital != string.Empty)
        this.objMaleGenital = this.GetObjectName(this.LoopParent, this.strMaleGenital);
    }
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai)
  {
    this.ai = _ai;
    for (int index = 0; index < this.lstEyeNeck.Count; ++index)
    {
      this.en = this.lstEyeNeck[index];
      if (((AnimatorStateInfo) ref _ai).IsName(this.en.anim))
      {
        this.chaMale.ChangeEyesOpenMax((float) this.en.openEye * 0.1f);
        this.mouthCtrl = this.chaMale.mouthCtrl;
        if (this.mouthCtrl != null)
          this.mouthCtrl.OpenMin = (float) this.en.openMouth * 0.1f;
        this.chaMale.ChangeEyebrowPtn(this.en.eyebrow, true);
        this.chaMale.ChangeEyesPtn(this.en.eye, true);
        this.chaMale.ChangeMouthPtn(this.en.mouth, true);
        this.SetEyesTarget(this.en.targetEye);
        this.SetNeckTarget(this.en.targetNeck);
        this.SetEyeBehaviour(this.en.Eyebehaviour);
        this.SetNeckBehaviour(this.en.Neckbehaviour);
        this.bFaceInfo = false;
        if (this.OldAnimName != this.en.anim)
        {
          this.ChangeTimeEye = 0.0f;
          this.nowleapSpeed[1] = 0.0f;
          this.BackUpEye[0] = this.EyeTrs[0].get_localRotation();
          this.BackUpEye[1] = this.EyeTrs[1].get_localRotation();
          this.ChangeTimeNeck = 0.0f;
          this.nowleapSpeed[0] = 0.0f;
          this.BackUpNeck = this.NeckTrs.get_localRotation();
          this.BackUpHead = this.HeadTrs.get_localRotation();
        }
        this.OldAnimName = this.en.anim;
        return true;
      }
    }
    this.bFaceInfo = true;
    return true;
  }

  private void LateUpdate()
  {
    this.EyeNeckCalc();
  }

  public void EyeNeckCalc()
  {
    if (Object.op_Equality((Object) this.chaMale, (Object) null) || this.NowEndADV || this.bFaceInfo)
      return;
    for (int index = 0; index < this.lstEyeNeck.Count; ++index)
    {
      this.en = this.lstEyeNeck[index];
      if (((AnimatorStateInfo) ref this.ai).IsName(this.en.anim))
      {
        if (this.en.targetNeck == 7)
        {
          if ((double) Singleton<HSceneFlagCtrl>.Instance.motions[0] < 0.5)
          {
            if (this.nYuragiType != 0)
            {
              this.nYuragiType = 0;
              this.ChangeTimeNeck = 0.0f;
              this.nowleapSpeed[0] = 0.0f;
            }
          }
          else if (this.nYuragiType != 1)
          {
            this.nYuragiType = 1;
            this.ChangeTimeNeck = 0.0f;
            this.nowleapSpeed[0] = 0.0f;
          }
          this.NeckCalc(this.en.NeckRot[this.nYuragiType], this.en.HeadRot[this.nYuragiType]);
        }
        if (this.en.targetEye != 7)
          break;
        if ((double) Singleton<HSceneFlagCtrl>.Instance.motions[0] < 0.5)
        {
          if (this.nYuragiType != 0)
          {
            this.nYuragiType = 0;
            this.ChangeTimeEye = 0.0f;
            this.nowleapSpeed[1] = 0.0f;
          }
        }
        else if (this.nYuragiType != 1)
        {
          this.nYuragiType = 1;
          this.ChangeTimeEye = 0.0f;
          this.nowleapSpeed[1] = 0.0f;
        }
        this.EyeCalc(this.en.EyeRot[this.nYuragiType]);
        break;
      }
    }
  }

  private bool SetEyesTarget(int _tag)
  {
    switch (_tag)
    {
      case 1:
        this.chaMale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale1Head) ? (Transform) null : this.objFemale1Head.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 2:
        this.chaMale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale1Genital) ? (Transform) null : this.objFemale1Genital.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 3:
        this.chaMale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale2Head) ? (Transform) null : this.objFemale2Head.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 4:
        this.chaMale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale2Genital) ? (Transform) null : this.objFemale2Genital.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 5:
        this.chaMale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objMaleHead) ? (Transform) null : this.objMaleHead.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 6:
        this.chaMale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objMaleGenital) ? (Transform) null : this.objMaleGenital.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 7:
        if (this.EyeType != _tag)
        {
          this.ChangeTimeEye = 0.0f;
          this.nowleapSpeed[1] = 0.0f;
          this.BackUpEye[0] = this.EyeTrs[0].get_localRotation();
          this.BackUpEye[1] = this.EyeTrs[1].get_localRotation();
        }
        this.EyeType = _tag;
        return true;
      case 8:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objGenitalSelf) ? (Transform) null : this.objGenitalSelf.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      default:
        this.chaMale.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
        goto case 7;
    }
  }

  private bool SetNeckTarget(int _tag)
  {
    switch (_tag)
    {
      case 1:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale1Head) ? (Transform) null : this.objFemale1Head.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 2:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale1Genital) ? (Transform) null : this.objFemale1Genital.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 3:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale2Head) ? (Transform) null : this.objFemale2Head.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 4:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale2Genital) ? (Transform) null : this.objFemale2Genital.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 5:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objMaleHead) ? (Transform) null : this.objMaleHead.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 6:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objMaleGenital) ? (Transform) null : this.objMaleGenital.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 7:
        if (this.NeckType != _tag)
        {
          this.ChangeTimeNeck = 0.0f;
          this.nowleapSpeed[0] = 0.0f;
          this.BackUpNeck = this.NeckTrs.get_localRotation();
          this.BackUpHead = this.HeadTrs.get_localRotation();
        }
        this.NeckType = _tag;
        return true;
      case 8:
        this.chaMale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objGenitalSelf) ? (Transform) null : this.objGenitalSelf.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      default:
        this.chaMale.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
    }
  }

  private bool SetNeckBehaviour(int _behaviour)
  {
    if (!((Behaviour) this.chaMale.neckLookCtrl).get_enabled() && _behaviour != 3)
      this.chaMale.neckLookCtrl.neckLookScript.UpdateCall(0);
    switch (_behaviour)
    {
      case 1:
        this.chaMale.ChangeLookNeckPtn(1, 1f);
        break;
      case 2:
        this.chaMale.ChangeLookNeckPtn(2, 1f);
        break;
      case 3:
        this.chaMale.ChangeLookNeckPtn(1, 1f);
        break;
      default:
        this.chaMale.ChangeLookNeckPtn(3, 1f);
        break;
    }
    if (_behaviour == 3)
      ((Behaviour) this.chaMale.neckLookCtrl).set_enabled(false);
    else
      ((Behaviour) this.chaMale.neckLookCtrl).set_enabled(true);
    return true;
  }

  private bool SetEyeBehaviour(int _behaviour)
  {
    switch (_behaviour)
    {
      case 1:
        this.chaMale.ChangeLookEyesPtn(1);
        break;
      case 2:
        this.chaMale.ChangeLookEyesPtn(2);
        break;
      case 3:
        this.chaMale.ChangeLookEyesPtn(1);
        break;
      default:
        this.chaMale.ChangeLookEyesPtn(0);
        break;
    }
    if (_behaviour == 3)
      ((Behaviour) this.chaMale.eyeLookCtrl).set_enabled(false);
    else
      ((Behaviour) this.chaMale.eyeLookCtrl).set_enabled(true);
    return true;
  }

  private void NeckCalc(Vector3 targetNeckRot, Vector3 targetHeadRot)
  {
    float deltaTime = Time.get_deltaTime();
    this.ChangeTimeNeck = Mathf.Clamp(this.ChangeTimeNeck + deltaTime, 0.0f, this.chaMale.neckLookCtrl.neckLookScript.changeTypeLeapTime);
    float num = Mathf.InverseLerp(0.0f, this.chaMale.neckLookCtrl.neckLookScript.changeTypeLeapTime, this.ChangeTimeNeck);
    if (this.chaMale.neckLookCtrl.neckLookScript.changeTypeLerpCurve != null)
      num = this.chaMale.neckLookCtrl.neckLookScript.changeTypeLerpCurve.Evaluate(num);
    this.nowleapSpeed[0] = Mathf.Clamp01(this.chaMale.neckLookCtrl.neckLookScript.neckTypeStates[3].leapSpeed * deltaTime);
    Quaternion quaternion1 = Quaternion.Slerp(this.BackUpNeck, Quaternion.Slerp(this.NeckTrs.get_localRotation(), Quaternion.Slerp(this.BackUpNeck, Quaternion.Euler(targetNeckRot), this.nowleapSpeed[0]), this.chaMale.neckLookCtrl.neckLookScript.calcLerp), num);
    this.NeckTrs.set_localRotation(quaternion1);
    this.BackUpNeck = quaternion1;
    Quaternion quaternion2 = Quaternion.Slerp(this.BackUpHead, Quaternion.Slerp(this.HeadTrs.get_localRotation(), Quaternion.Slerp(this.BackUpHead, Quaternion.Euler(targetHeadRot), this.nowleapSpeed[0]), this.chaMale.neckLookCtrl.neckLookScript.calcLerp), num);
    this.HeadTrs.set_localRotation(quaternion2);
    this.BackUpHead = quaternion2;
    this.chaMale.neckLookCtrl.neckLookScript.aBones[0].fixAngle = this.NeckTrs.get_localRotation();
    this.chaMale.neckLookCtrl.neckLookScript.aBones[1].fixAngle = this.HeadTrs.get_localRotation();
  }

  private void EyeCalc(Vector3 targetEyeRot)
  {
    float deltaTime = Time.get_deltaTime();
    this.ChangeTimeEye = Mathf.Clamp(this.ChangeTimeEye + deltaTime, 0.0f, 1f);
    float num = Mathf.InverseLerp(0.0f, 1f, this.ChangeTimeEye);
    this.nowleapSpeed[1] = Mathf.Clamp01(this.chaMale.eyeLookCtrl.eyeLookScript.eyeTypeStates[0].leapSpeed * deltaTime);
    Quaternion quaternion1 = Quaternion.Slerp(this.BackUpEye[0], Quaternion.Slerp(this.EyeTrs[0].get_localRotation(), Quaternion.Slerp(this.BackUpEye[0], Quaternion.Euler(targetEyeRot), this.nowleapSpeed[1]), 1f), num);
    ref Quaternion local1 = ref this.BackUpEye[0];
    Quaternion quaternion2 = quaternion1;
    this.EyeTrs[0].set_localRotation(quaternion2);
    Quaternion quaternion3 = quaternion2;
    local1 = quaternion3;
    Quaternion quaternion4 = Quaternion.Slerp(this.BackUpEye[1], Quaternion.Slerp(this.EyeTrs[1].get_localRotation(), Quaternion.Slerp(this.BackUpEye[1], Quaternion.Euler(targetEyeRot), this.nowleapSpeed[1]), 1f), num);
    ref Quaternion local2 = ref this.BackUpEye[1];
    Quaternion quaternion5 = quaternion4;
    this.EyeTrs[1].set_localRotation(quaternion5);
    Quaternion quaternion6 = quaternion5;
    local2 = quaternion6;
    EyeObject eyeObj1 = this.chaMale.eyeLookCtrl.eyeLookScript.eyeObjs[0];
    Quaternion localRotation1 = this.EyeTrs[1].get_localRotation();
    // ISSUE: variable of the null type
    __Null y1 = ((Quaternion) ref localRotation1).get_eulerAngles().y;
    eyeObj1.angleH = (float) y1;
    EyeObject eyeObj2 = this.chaMale.eyeLookCtrl.eyeLookScript.eyeObjs[0];
    Quaternion localRotation2 = this.EyeTrs[1].get_localRotation();
    // ISSUE: variable of the null type
    __Null x1 = ((Quaternion) ref localRotation2).get_eulerAngles().x;
    eyeObj2.angleV = (float) x1;
    EyeObject eyeObj3 = this.chaMale.eyeLookCtrl.eyeLookScript.eyeObjs[1];
    Quaternion localRotation3 = this.EyeTrs[1].get_localRotation();
    // ISSUE: variable of the null type
    __Null y2 = ((Quaternion) ref localRotation3).get_eulerAngles().y;
    eyeObj3.angleH = (float) y2;
    EyeObject eyeObj4 = this.chaMale.eyeLookCtrl.eyeLookScript.eyeObjs[1];
    Quaternion localRotation4 = this.EyeTrs[1].get_localRotation();
    // ISSUE: variable of the null type
    __Null x2 = ((Quaternion) ref localRotation4).get_eulerAngles().x;
    eyeObj4.angleV = (float) x2;
  }

  [Serializable]
  public struct EyeNeck
  {
    [Label("アニメーション名")]
    public string anim;
    [Label("目の開き")]
    public int openEye;
    [Label("口の開き")]
    public int openMouth;
    [Label("眉")]
    public int eyebrow;
    [Label("目")]
    public int eye;
    [Label("口")]
    public int mouth;
    [Label("首挙動")]
    public int Neckbehaviour;
    [Label("目挙動")]
    public int Eyebehaviour;
    [Label("目ターゲット")]
    public int targetEye;
    [Label("視線角度")]
    public Vector3[] EyeRot;
    [Label("首ターゲット")]
    public int targetNeck;
    [Label("首角度")]
    public Vector3[] NeckRot;
    [Label("頭角度")]
    public Vector3[] HeadRot;

    public void Init()
    {
      this.anim = string.Empty;
      this.openEye = 0;
      this.openMouth = 0;
      this.eyebrow = 0;
      this.eye = 0;
      this.mouth = 0;
      this.Neckbehaviour = 0;
      this.Eyebehaviour = 0;
      this.targetEye = 0;
      this.EyeRot = new Vector3[2]
      {
        Vector3.get_zero(),
        Vector3.get_zero()
      };
      this.targetNeck = 0;
      this.NeckRot = new Vector3[2]
      {
        Vector3.get_zero(),
        Vector3.get_zero()
      };
      this.HeadRot = new Vector3[2]
      {
        Vector3.get_zero(),
        Vector3.get_zero()
      };
    }
  }
}
