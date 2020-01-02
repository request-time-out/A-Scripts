// Decompiled with JetBrains decompiler
// Type: HMotionEyeNeckLesPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HMotionEyeNeckLesPlayer : MonoBehaviour
{
  [Label("相手女顔オブジェクト名")]
  public string strFemaleHead;
  [Label("相手女性器オブジェクト名")]
  public string strFemaleGenital;
  [SerializeField]
  private List<HMotionEyeNeckLesPlayer.EyeNeck> lstEyeNeck;
  [DisabledGroup("女クラス")]
  [SerializeField]
  private ChaControl chaFemale;
  [DisabledGroup("自分性器オブジェクト")]
  [SerializeField]
  private GameObject objGenitalSelf;
  [DisabledGroup("女相手顔オブジェクト")]
  [SerializeField]
  private GameObject objFemale1Head;
  [DisabledGroup("女相手性器オブジェクト")]
  [SerializeField]
  private GameObject objFemale1Genital;
  private Transform LoopParent;
  private HMotionEyeNeckLesPlayer.EyeNeck en;
  private HMotionEyeNeckLesPlayer.EyeNeck eyeNeck;
  private HMotionEyeNeckLesPlayer.EyeNeckFace enFace;
  private int ID;
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
  private bool[] bFaceInfo;
  private int nYuragiType;
  private string OldAnimName;
  private HScene hscene;
  private ExcelData excelData;
  private HMotionEyeNeckLesPlayer.EyeNeck info;
  private List<string> row;
  private string abName;
  private string assetName;
  public bool NowEndADV;
  private Transform[] getChild;
  private float[] nowleapSpeed;

  public HMotionEyeNeckLesPlayer()
  {
    base.\u002Ector();
  }

  public bool Init(ChaControl _female, int id)
  {
    this.Release();
    this.ID = id;
    this.chaFemale = _female;
    this.NeckTrs = this.chaFemale.neckLookCtrl.neckLookScript.aBones[0].neckBone;
    this.HeadTrs = this.chaFemale.neckLookCtrl.neckLookScript.aBones[1].neckBone;
    this.EyeTrs = new Transform[2]
    {
      this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[0].eyeTransform,
      this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[1].eyeTransform
    };
    this.BackUpNeck = this.NeckTrs.get_localRotation();
    this.BackUpHead = this.HeadTrs.get_localRotation();
    this.BackUpEye = new Quaternion[2]
    {
      this.EyeTrs[0].get_localRotation(),
      this.EyeTrs[1].get_localRotation()
    };
    this.objGenitalSelf = (GameObject) null;
    if (Object.op_Implicit((Object) this.chaFemale) && Object.op_Implicit((Object) this.chaFemale.objBodyBone))
    {
      this.LoopParent = this.chaFemale.objBodyBone.get_transform();
      if (this.strFemaleGenital != string.Empty)
        this.objGenitalSelf = this.GetObjectName(this.LoopParent, this.strFemaleGenital);
    }
    if (Object.op_Equality((Object) this.hscene, (Object) null))
      this.hscene = (HScene) Singleton<HSceneManager>.Instance.HSceneSet.GetComponentInChildren<HScene>();
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
    this.ChangeTimeEye = 0.0f;
    this.nowleapSpeed[1] = 0.0f;
    this.BackUpEye[0] = this.EyeTrs[0].get_localRotation();
    this.BackUpEye[1] = this.EyeTrs[1].get_localRotation();
    this.ChangeTimeNeck = 0.0f;
    this.nowleapSpeed[0] = 0.0f;
    this.BackUpNeck = this.NeckTrs.get_localRotation();
    this.BackUpHead = this.HeadTrs.get_localRotation();
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
            this.info = new HMotionEyeNeckLesPlayer.EyeNeck();
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
                ref HMotionEyeNeckLesPlayer.EyeNeck local1 = ref this.info;
                List<string> row2 = this.row;
                int index3 = num3;
                int num4 = index3 + 1;
                int num5 = row2.GetElement<string>(index3) == "1" ? 1 : 0;
                local1.isConfigDisregardNeck = num5 != 0;
                ref HMotionEyeNeckLesPlayer.EyeNeck local2 = ref this.info;
                List<string> row3 = this.row;
                int index4 = num4;
                int num6 = index4 + 1;
                int num7 = row3.GetElement<string>(index4) == "1" ? 1 : 0;
                local2.isConfigDisregardEye = num7 != 0;
                float result1 = 0.0f;
                List<string> row4 = this.row;
                int index5 = num6;
                int num8 = index5 + 1;
                if (!float.TryParse(row4.GetElement<string>(index5), out result1))
                  result1 = 0.0f;
                this.info.openEye = result1;
                float result2 = 0.0f;
                List<string> row5 = this.row;
                int index6 = num8;
                int num9 = index6 + 1;
                if (!float.TryParse(row5.GetElement<string>(index6), out result2))
                  result2 = 0.0f;
                this.info.openMouth = result2;
                int result3 = 0;
                List<string> row6 = this.row;
                int index7 = num9;
                int num10 = index7 + 1;
                if (!int.TryParse(row6.GetElement<string>(index7), out result3))
                  result3 = 0;
                this.info.eyeBlow = result3;
                int result4 = 0;
                List<string> row7 = this.row;
                int index8 = num10;
                int num11 = index8 + 1;
                if (!int.TryParse(row7.GetElement<string>(index8), out result4))
                  result4 = 0;
                this.info.eye = result4;
                int result5 = 0;
                List<string> row8 = this.row;
                int index9 = num11;
                int num12 = index9 + 1;
                if (!int.TryParse(row8.GetElement<string>(index9), out result5))
                  result5 = 0;
                this.info.mouth = result5;
                float result6 = 0.0f;
                List<string> row9 = this.row;
                int index10 = num12;
                int num13 = index10 + 1;
                if (!float.TryParse(row9.GetElement<string>(index10), out result6))
                  result6 = 0.0f;
                this.info.tear = result6;
                float result7 = 0.0f;
                List<string> row10 = this.row;
                int index11 = num13;
                int num14 = index11 + 1;
                if (!float.TryParse(row10.GetElement<string>(index11), out result7))
                  result7 = 0.0f;
                this.info.cheek = result7;
                ref HMotionEyeNeckLesPlayer.EyeNeck local3 = ref this.info;
                List<string> row11 = this.row;
                int index12 = num14;
                int num15 = index12 + 1;
                int num16 = row11.GetElement<string>(index12) == "1" ? 1 : 0;
                local3.blink = num16 != 0;
                ref HMotionEyeNeckLesPlayer.EyeNeckFace local4 = ref this.info.faceinfo;
                List<string> row12 = this.row;
                int index13 = num15;
                int num17 = index13 + 1;
                int num18 = int.Parse(row12.GetElement<string>(index13));
                local4.Neckbehaviour = num18;
                ref HMotionEyeNeckLesPlayer.EyeNeckFace local5 = ref this.info.faceinfo;
                List<string> row13 = this.row;
                int index14 = num17;
                int num19 = index14 + 1;
                int num20 = int.Parse(row13.GetElement<string>(index14));
                local5.Eyebehaviour = num20;
                ref HMotionEyeNeckLesPlayer.EyeNeckFace local6 = ref this.info.faceinfo;
                List<string> row14 = this.row;
                int index15 = num19;
                int num21 = index15 + 1;
                int num22 = int.Parse(row14.GetElement<string>(index15));
                local6.targetNeck = num22;
                List<string> row15 = this.row;
                int index16 = num21;
                int num23 = index16 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row15.GetElement<string>(index16), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row16 = this.row;
                int index17 = num23;
                int num24 = index17 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row16.GetElement<string>(index17), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row17 = this.row;
                int index18 = num24;
                int num25 = index18 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row17.GetElement<string>(index18), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo.NeckRot[0] = zero;
                List<string> row18 = this.row;
                int index19 = num25;
                int num26 = index19 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row18.GetElement<string>(index19), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row19 = this.row;
                int index20 = num26;
                int num27 = index20 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row19.GetElement<string>(index20), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row20 = this.row;
                int index21 = num27;
                int num28 = index21 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row20.GetElement<string>(index21), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo.NeckRot[1] = zero;
                List<string> row21 = this.row;
                int index22 = num28;
                int num29 = index22 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row21.GetElement<string>(index22), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row22 = this.row;
                int index23 = num29;
                int num30 = index23 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row22.GetElement<string>(index23), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row23 = this.row;
                int index24 = num30;
                int num31 = index24 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row23.GetElement<string>(index24), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo.HeadRot[0] = zero;
                List<string> row24 = this.row;
                int index25 = num31;
                int num32 = index25 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row24.GetElement<string>(index25), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row25 = this.row;
                int index26 = num32;
                int num33 = index26 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row25.GetElement<string>(index26), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row26 = this.row;
                int index27 = num33;
                int num34 = index27 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row26.GetElement<string>(index27), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo.HeadRot[1] = zero;
                ref HMotionEyeNeckLesPlayer.EyeNeckFace local7 = ref this.info.faceinfo;
                List<string> row27 = this.row;
                int index28 = num34;
                int num35 = index28 + 1;
                int num36 = int.Parse(row27.GetElement<string>(index28));
                local7.targetEye = num36;
                List<string> row28 = this.row;
                int index29 = num35;
                int num37 = index29 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row28.GetElement<string>(index29), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row29 = this.row;
                int index30 = num37;
                int num38 = index30 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row29.GetElement<string>(index30), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row30 = this.row;
                int index31 = num38;
                int num39 = index31 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row30.GetElement<string>(index31), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo.EyeRot[0] = zero;
                List<string> row31 = this.row;
                int index32 = num39;
                int num40 = index32 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row31.GetElement<string>(index32), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row32 = this.row;
                int index33 = num40;
                int num41 = index33 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row32.GetElement<string>(index33), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row33 = this.row;
                int index34 = num41;
                int num42 = index34 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row33.GetElement<string>(index34), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo.EyeRot[1] = zero;
                this.lstEyeNeck.Add(this.info);
              }
            }
          }
        }
      }
    }
    return true;
  }

  public bool SetPartner(GameObject _objFemale1Bone)
  {
    this.objFemale1Head = (GameObject) null;
    this.objFemale1Genital = (GameObject) null;
    if (Object.op_Implicit((Object) _objFemale1Bone))
    {
      this.LoopParent = _objFemale1Bone.get_transform();
      if (this.strFemaleHead != string.Empty)
        this.objFemale1Head = this.GetObjectName(this.LoopParent, this.strFemaleHead);
      if (this.strFemaleGenital != string.Empty)
        this.objFemale1Genital = this.GetObjectName(this.LoopParent, this.strFemaleGenital);
    }
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai, int _main)
  {
    this.ai = _ai;
    for (int index = 0; index < this.lstEyeNeck.Count; ++index)
    {
      this.en = this.lstEyeNeck[index];
      if (((AnimatorStateInfo) ref _ai).IsName(this.en.anim))
      {
        this.enFace = this.en.faceinfo;
        this.eyeNeck = this.en;
      }
    }
    this.chaFemale.ChangeEyesOpenMax(this.eyeNeck.openEye);
    FBSCtrlMouth mouthCtrl = this.chaFemale.mouthCtrl;
    if (mouthCtrl != null)
      mouthCtrl.OpenMin = this.en.openMouth;
    this.chaFemale.ChangeEyebrowPtn(this.en.eyeBlow, true);
    this.chaFemale.ChangeEyesPtn(this.en.eye, true);
    this.chaFemale.ChangeMouthPtn(this.eyeNeck.mouth, true);
    if (this.eyeNeck.mouth == 10 || this.eyeNeck.mouth == 13)
      this.chaFemale.DisableShapeMouth(true);
    else
      this.chaFemale.DisableShapeMouth(false);
    this.chaFemale.ChangeTearsRate(this.eyeNeck.tear);
    this.chaFemale.ChangeHohoAkaRate(this.eyeNeck.cheek);
    this.chaFemale.ChangeEyesBlinkFlag(this.eyeNeck.blink);
    this.SetNeckTarget(!this.en.isConfigDisregardNeck ? this.enFace.targetNeck : this.enFace.targetNeck);
    this.SetEyesTarget(!this.en.isConfigDisregardEye ? this.enFace.targetEye : this.enFace.targetEye);
    this.SetBehaviourEyes(!this.en.isConfigDisregardEye ? this.enFace.Eyebehaviour : this.enFace.Eyebehaviour);
    this.SetBehaviourNeck(!this.en.isConfigDisregardNeck ? this.enFace.Neckbehaviour : this.enFace.Neckbehaviour);
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
      this.OldAnimName = this.en.anim;
    }
    return true;
  }

  private void LateUpdate()
  {
    this.EyeNeckCalc();
  }

  public void EyeNeckCalc()
  {
    if (Object.op_Equality((Object) this.chaFemale, (Object) null) || this.NowEndADV)
      return;
    for (int index = 0; index < this.lstEyeNeck.Count; ++index)
    {
      this.en = this.lstEyeNeck[index];
      if (((AnimatorStateInfo) ref this.ai).IsName(this.en.anim))
      {
        this.enFace = this.en.faceinfo;
        if (this.enFace.targetNeck == 3)
        {
          if ((double) Singleton<HSceneFlagCtrl>.Instance.motions[this.ID] < 0.5)
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
          this.NeckCalc(this.enFace.NeckRot[this.nYuragiType], this.enFace.HeadRot[this.nYuragiType]);
        }
        if (this.enFace.targetEye != 3)
          break;
        if ((double) Singleton<HSceneFlagCtrl>.Instance.motions[this.ID] < 0.5)
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
        this.EyeCalc(this.enFace.EyeRot[this.nYuragiType]);
        break;
      }
    }
  }

  private bool SetEyesTarget(int _tag)
  {
    switch (_tag)
    {
      case 1:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale1Head) ? (Transform) null : this.objFemale1Head.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 3;
      case 2:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale1Genital) ? (Transform) null : this.objFemale1Genital.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 3;
      case 3:
        if (this.EyeType != _tag)
        {
          this.ChangeTimeEye = 0.0f;
          this.nowleapSpeed[1] = 0.0f;
          this.BackUpEye[0] = this.EyeTrs[0].get_localRotation();
          this.BackUpEye[1] = this.EyeTrs[1].get_localRotation();
        }
        this.EyeType = _tag;
        return true;
      case 4:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objGenitalSelf) ? (Transform) null : this.objGenitalSelf.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 3;
      default:
        this.chaFemale.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
        goto case 3;
    }
  }

  private bool SetNeckTarget(int _tag)
  {
    switch (_tag)
    {
      case 1:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale1Head) ? (Transform) null : this.objFemale1Head.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 3;
      case 2:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale1Genital) ? (Transform) null : this.objFemale1Genital.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 3;
      case 3:
        if (this.NeckType != _tag)
        {
          this.ChangeTimeNeck = 0.0f;
          this.nowleapSpeed[0] = 0.0f;
          this.BackUpNeck = this.NeckTrs.get_localRotation();
          this.BackUpHead = this.HeadTrs.get_localRotation();
        }
        this.NeckType = _tag;
        return true;
      case 4:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objGenitalSelf) ? (Transform) null : this.objGenitalSelf.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 3;
      default:
        this.chaFemale.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
        goto case 3;
    }
  }

  private bool SetBehaviourEyes(int _behaviour)
  {
    switch (_behaviour)
    {
      case 1:
        this.chaFemale.ChangeLookEyesPtn(1);
        break;
      case 2:
        this.chaFemale.ChangeLookEyesPtn(2);
        break;
      case 3:
        this.chaFemale.ChangeLookEyesPtn(1);
        break;
      default:
        this.chaFemale.ChangeLookEyesPtn(0);
        break;
    }
    if (_behaviour == 3)
      ((Behaviour) this.chaFemale.eyeLookCtrl).set_enabled(false);
    else
      ((Behaviour) this.chaFemale.eyeLookCtrl).set_enabled(true);
    return true;
  }

  private bool SetBehaviourNeck(int _behaviour)
  {
    if (!((Behaviour) this.chaFemale.neckLookCtrl).get_enabled() && _behaviour != 3)
      this.chaFemale.neckLookCtrl.neckLookScript.UpdateCall(0);
    switch (_behaviour)
    {
      case 1:
        this.chaFemale.ChangeLookNeckPtn(1, 1f);
        break;
      case 2:
        this.chaFemale.ChangeLookNeckPtn(2, 1f);
        break;
      case 3:
        this.chaFemale.ChangeLookNeckPtn(1, 1f);
        break;
      default:
        this.chaFemale.ChangeLookNeckPtn(3, 1f);
        break;
    }
    if (_behaviour == 3)
      ((Behaviour) this.chaFemale.neckLookCtrl).set_enabled(false);
    else
      ((Behaviour) this.chaFemale.neckLookCtrl).set_enabled(true);
    return true;
  }

  private void NeckCalc(Vector3 targetNeckRot, Vector3 targetHeadRot)
  {
    float deltaTime = Time.get_deltaTime();
    this.ChangeTimeNeck = Mathf.Clamp(this.ChangeTimeNeck + deltaTime, 0.0f, this.chaFemale.neckLookCtrl.neckLookScript.changeTypeLeapTime);
    float num = Mathf.InverseLerp(0.0f, this.chaFemale.neckLookCtrl.neckLookScript.changeTypeLeapTime, this.ChangeTimeNeck);
    if (this.chaFemale.neckLookCtrl.neckLookScript.changeTypeLerpCurve != null)
      num = this.chaFemale.neckLookCtrl.neckLookScript.changeTypeLerpCurve.Evaluate(num);
    this.nowleapSpeed[0] = Mathf.Clamp01(this.chaFemale.neckLookCtrl.neckLookScript.neckTypeStates[3].leapSpeed * deltaTime);
    Quaternion quaternion1 = Quaternion.Slerp(this.BackUpNeck, Quaternion.Slerp(this.NeckTrs.get_localRotation(), Quaternion.Slerp(this.BackUpNeck, Quaternion.Euler(targetNeckRot), this.nowleapSpeed[0]), this.chaFemale.neckLookCtrl.neckLookScript.calcLerp), num);
    this.NeckTrs.set_localRotation(quaternion1);
    this.BackUpNeck = quaternion1;
    Quaternion quaternion2 = Quaternion.Slerp(this.BackUpHead, Quaternion.Slerp(this.HeadTrs.get_localRotation(), Quaternion.Slerp(this.BackUpHead, Quaternion.Euler(targetHeadRot), this.nowleapSpeed[0]), this.chaFemale.neckLookCtrl.neckLookScript.calcLerp), num);
    this.HeadTrs.set_localRotation(quaternion2);
    this.BackUpHead = quaternion2;
    this.chaFemale.neckLookCtrl.neckLookScript.aBones[0].fixAngle = this.NeckTrs.get_localRotation();
    this.chaFemale.neckLookCtrl.neckLookScript.aBones[1].fixAngle = this.HeadTrs.get_localRotation();
  }

  private void EyeCalc(Vector3 targetEyeRot)
  {
    float deltaTime = Time.get_deltaTime();
    this.ChangeTimeEye = Mathf.Clamp(this.ChangeTimeEye + deltaTime, 0.0f, 1f);
    float num = Mathf.InverseLerp(0.0f, 1f, this.ChangeTimeEye);
    this.nowleapSpeed[1] = Mathf.Clamp01(this.chaFemale.eyeLookCtrl.eyeLookScript.eyeTypeStates[0].leapSpeed * deltaTime);
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
    EyeObject eyeObj1 = this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[0];
    Quaternion localRotation1 = this.EyeTrs[1].get_localRotation();
    // ISSUE: variable of the null type
    __Null y1 = ((Quaternion) ref localRotation1).get_eulerAngles().y;
    eyeObj1.angleH = (float) y1;
    EyeObject eyeObj2 = this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[0];
    Quaternion localRotation2 = this.EyeTrs[1].get_localRotation();
    // ISSUE: variable of the null type
    __Null x1 = ((Quaternion) ref localRotation2).get_eulerAngles().x;
    eyeObj2.angleV = (float) x1;
    EyeObject eyeObj3 = this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[1];
    Quaternion localRotation3 = this.EyeTrs[1].get_localRotation();
    // ISSUE: variable of the null type
    __Null y2 = ((Quaternion) ref localRotation3).get_eulerAngles().y;
    eyeObj3.angleH = (float) y2;
    EyeObject eyeObj4 = this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[1];
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
    [Label("首コンフィグ無視")]
    public bool isConfigDisregardNeck;
    [Label("目コンフィグ無視")]
    public bool isConfigDisregardEye;
    [Label("目の開き")]
    public float openEye;
    [Label("口の開き")]
    public float openMouth;
    [Label("眉の形")]
    public int eyeBlow;
    [Label("目の形")]
    public int eye;
    [Label("口の形")]
    public int mouth;
    [RangeLabel("涙", 0.0f, 1f)]
    public float tear;
    [RangeLabel("頬赤", 0.0f, 1f)]
    public float cheek;
    [Label("瞬き")]
    public bool blink;
    public HMotionEyeNeckLesPlayer.EyeNeckFace faceinfo;

    public void Init()
    {
      this.anim = string.Empty;
      this.isConfigDisregardNeck = false;
      this.isConfigDisregardEye = false;
      this.faceinfo.Init();
    }
  }

  [Serializable]
  public struct EyeNeckFace
  {
    [Label("首挙動")]
    public int Neckbehaviour;
    [Label("目挙動")]
    public int Eyebehaviour;
    [Label("首ターゲット")]
    public int targetNeck;
    [Label("首角度")]
    public Vector3[] NeckRot;
    [Label("頭角度")]
    public Vector3[] HeadRot;
    [Label("目ターゲット")]
    public int targetEye;
    [Label("視線角度")]
    public Vector3[] EyeRot;

    public void Init()
    {
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
