// Decompiled with JetBrains decompiler
// Type: HMotionEyeNeckFemale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HMotionEyeNeckFemale : MonoBehaviour
{
  [Label("相手男顔オブジェクト名")]
  public string strMaleHead;
  [Label("相手男性器オブジェクト名")]
  public string strMaleGenital;
  [Label("相手女顔オブジェクト名")]
  public string strFemaleHead;
  [Label("相手女性器オブジェクト名")]
  public string strFemaleGenital;
  [SerializeField]
  private List<HMotionEyeNeckFemale.EyeNeck> lstEyeNeck;
  [DisabledGroup("女クラス")]
  [SerializeField]
  private ChaControl chaFemale;
  [DisabledGroup("自分性器オブジェクト")]
  [SerializeField]
  private GameObject objGenitalSelf;
  [DisabledGroup("男1顔オブジェクト")]
  [SerializeField]
  private GameObject objMale1Head;
  [DisabledGroup("男1性器オブジェクト")]
  [SerializeField]
  private GameObject objMale1Genital;
  [DisabledGroup("男2顔オブジェクト")]
  [SerializeField]
  private GameObject objMale2Head;
  [DisabledGroup("男2性器オブジェクト")]
  [SerializeField]
  private GameObject objMale2Genital;
  [DisabledGroup("女相手顔オブジェクト")]
  [SerializeField]
  private GameObject objFemale1Head;
  [DisabledGroup("女相手性器オブジェクト")]
  [SerializeField]
  private GameObject objFemale1Genital;
  private Transform LoopParent;
  private HMotionEyeNeckFemale.EyeNeck en;
  private HMotionEyeNeckFemale.EyeNeckFace enFace;
  private int ID;
  private AnimatorStateInfo ai;
  private Transform NeckTrs;
  private Transform HeadTrs;
  private Transform[] EyeTrs;
  private Quaternion BackUpNeck;
  private Quaternion BackUpHead;
  private Quaternion[] BackUpEye;
  private HVoiceCtrl.FaceInfo faceInfo;
  private float ChangeTimeNeck;
  private int NeckType;
  private float ChangeTimeEye;
  private int EyeType;
  private bool[] bFaceInfo;
  private int nYuragiType;
  private string OldAnimName;
  private HScene hscene;
  private ExcelData excelData;
  private HMotionEyeNeckFemale.EyeNeck info;
  private List<string> row;
  private string abName;
  private string assetName;
  public bool NowEndADV;
  private Transform[] getChild;
  private float[] nowleapSpeed;

  public HMotionEyeNeckFemale()
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
            this.info = new HMotionEyeNeckFemale.EyeNeck();
            this.info.Init();
            if (this.row.Count != 0)
            {
              List<string> row1 = this.row;
              int index2 = num2;
              int num3 = index2 + 1;
              string element1 = row1.GetElement<string>(index2);
              if (!element1.IsNullOrEmpty())
              {
                this.info.anim = element1;
                ref HMotionEyeNeckFemale.EyeNeck local1 = ref this.info;
                List<string> row2 = this.row;
                int index3 = num3;
                int num4 = index3 + 1;
                int num5 = row2.GetElement<string>(index3) == "1" ? 1 : 0;
                local1.isConfigDisregardNeck = num5 != 0;
                ref HMotionEyeNeckFemale.EyeNeck local2 = ref this.info;
                List<string> row3 = this.row;
                int index4 = num4;
                int num6 = index4 + 1;
                int num7 = row3.GetElement<string>(index4) == "1" ? 1 : 0;
                local2.isConfigDisregardEye = num7 != 0;
                List<string> row4 = this.row;
                int index5 = num6;
                int num8 = index5 + 1;
                string element2 = row4.GetElement<string>(index5);
                this.info.faceinfo[0].isDisregardVoiceNeck = !element2.IsNullOrEmpty() && element2 == "1";
                List<string> row5 = this.row;
                int index6 = num8;
                int num9 = index6 + 1;
                string element3 = row5.GetElement<string>(index6);
                this.info.faceinfo[0].isDisregardVoiceEye = !element3.IsNullOrEmpty() && element3 == "1";
                ref HMotionEyeNeckFemale.EyeNeckFace local3 = ref this.info.faceinfo[0];
                List<string> row6 = this.row;
                int index7 = num9;
                int num10 = index7 + 1;
                int num11 = int.Parse(row6.GetElement<string>(index7));
                local3.Neckbehaviour = num11;
                ref HMotionEyeNeckFemale.EyeNeckFace local4 = ref this.info.faceinfo[0];
                List<string> row7 = this.row;
                int index8 = num10;
                int num12 = index8 + 1;
                int num13 = int.Parse(row7.GetElement<string>(index8));
                local4.Eyebehaviour = num13;
                ref HMotionEyeNeckFemale.EyeNeckFace local5 = ref this.info.faceinfo[0];
                List<string> row8 = this.row;
                int index9 = num12;
                int num14 = index9 + 1;
                int num15 = int.Parse(row8.GetElement<string>(index9));
                local5.targetNeck = num15;
                List<string> row9 = this.row;
                int index10 = num14;
                int num16 = index10 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row9.GetElement<string>(index10), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row10 = this.row;
                int index11 = num16;
                int num17 = index11 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row10.GetElement<string>(index11), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row11 = this.row;
                int index12 = num17;
                int num18 = index12 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row11.GetElement<string>(index12), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo[0].NeckRot[0] = zero;
                List<string> row12 = this.row;
                int index13 = num18;
                int num19 = index13 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row12.GetElement<string>(index13), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row13 = this.row;
                int index14 = num19;
                int num20 = index14 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row13.GetElement<string>(index14), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row14 = this.row;
                int index15 = num20;
                int num21 = index15 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row14.GetElement<string>(index15), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo[0].NeckRot[1] = zero;
                List<string> row15 = this.row;
                int index16 = num21;
                int num22 = index16 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row15.GetElement<string>(index16), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row16 = this.row;
                int index17 = num22;
                int num23 = index17 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row16.GetElement<string>(index17), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row17 = this.row;
                int index18 = num23;
                int num24 = index18 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row17.GetElement<string>(index18), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo[0].HeadRot[0] = zero;
                List<string> row18 = this.row;
                int index19 = num24;
                int num25 = index19 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row18.GetElement<string>(index19), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row19 = this.row;
                int index20 = num25;
                int num26 = index20 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row19.GetElement<string>(index20), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row20 = this.row;
                int index21 = num26;
                int num27 = index21 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row20.GetElement<string>(index21), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo[0].HeadRot[1] = zero;
                ref HMotionEyeNeckFemale.EyeNeckFace local6 = ref this.info.faceinfo[0];
                List<string> row21 = this.row;
                int index22 = num27;
                int num28 = index22 + 1;
                int num29 = int.Parse(row21.GetElement<string>(index22));
                local6.targetEye = num29;
                List<string> row22 = this.row;
                int index23 = num28;
                int num30 = index23 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row22.GetElement<string>(index23), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row23 = this.row;
                int index24 = num30;
                int num31 = index24 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row23.GetElement<string>(index24), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row24 = this.row;
                int index25 = num31;
                int num32 = index25 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row24.GetElement<string>(index25), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo[0].EyeRot[0] = zero;
                List<string> row25 = this.row;
                int index26 = num32;
                int num33 = index26 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row25.GetElement<string>(index26), (float&) ref zero.x))
                  zero.x = (__Null) 0.0;
                List<string> row26 = this.row;
                int index27 = num33;
                int num34 = index27 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row26.GetElement<string>(index27), (float&) ref zero.y))
                  zero.y = (__Null) 0.0;
                List<string> row27 = this.row;
                int index28 = num34;
                int num35 = index28 + 1;
                // ISSUE: cast to a reference type
                if (!float.TryParse(row27.GetElement<string>(index28), (float&) ref zero.z))
                  zero.z = (__Null) 0.0;
                this.info.faceinfo[0].EyeRot[1] = zero;
                if (num35 >= this.row.Count)
                {
                  this.lstEyeNeck.Add(this.info);
                }
                else
                {
                  List<string> row28 = this.row;
                  int index29 = num35;
                  int num36 = index29 + 1;
                  string element4 = row28.GetElement<string>(index29);
                  if (element4.IsNullOrEmpty())
                  {
                    this.lstEyeNeck.Add(this.info);
                  }
                  else
                  {
                    this.info.ExistFaceInfoIya = true;
                    this.info.faceinfo[1].isDisregardVoiceNeck = !element4.IsNullOrEmpty() && element4 == "1";
                    List<string> row29 = this.row;
                    int index30 = num36;
                    int num37 = index30 + 1;
                    string element5 = row29.GetElement<string>(index30);
                    this.info.faceinfo[1].isDisregardVoiceEye = !element5.IsNullOrEmpty() && element5 == "1";
                    ref HMotionEyeNeckFemale.EyeNeckFace local7 = ref this.info.faceinfo[1];
                    List<string> row30 = this.row;
                    int index31 = num37;
                    int num38 = index31 + 1;
                    int num39 = int.Parse(row30.GetElement<string>(index31));
                    local7.Neckbehaviour = num39;
                    ref HMotionEyeNeckFemale.EyeNeckFace local8 = ref this.info.faceinfo[1];
                    List<string> row31 = this.row;
                    int index32 = num38;
                    int num40 = index32 + 1;
                    int num41 = int.Parse(row31.GetElement<string>(index32));
                    local8.Eyebehaviour = num41;
                    ref HMotionEyeNeckFemale.EyeNeckFace local9 = ref this.info.faceinfo[1];
                    List<string> row32 = this.row;
                    int index33 = num40;
                    int num42 = index33 + 1;
                    int num43 = int.Parse(row32.GetElement<string>(index33));
                    local9.targetNeck = num43;
                    List<string> row33 = this.row;
                    int index34 = num42;
                    int num44 = index34 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row33.GetElement<string>(index34), (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    List<string> row34 = this.row;
                    int index35 = num44;
                    int num45 = index35 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row34.GetElement<string>(index35), (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    List<string> row35 = this.row;
                    int index36 = num45;
                    int num46 = index36 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row35.GetElement<string>(index36), (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    this.info.faceinfo[1].NeckRot[0] = zero;
                    List<string> row36 = this.row;
                    int index37 = num46;
                    int num47 = index37 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row36.GetElement<string>(index37), (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    List<string> row37 = this.row;
                    int index38 = num47;
                    int num48 = index38 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row37.GetElement<string>(index38), (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    List<string> row38 = this.row;
                    int index39 = num48;
                    int num49 = index39 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row38.GetElement<string>(index39), (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    this.info.faceinfo[1].NeckRot[1] = zero;
                    List<string> row39 = this.row;
                    int index40 = num49;
                    int num50 = index40 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row39.GetElement<string>(index40), (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    List<string> row40 = this.row;
                    int index41 = num50;
                    int num51 = index41 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row40.GetElement<string>(index41), (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    List<string> row41 = this.row;
                    int index42 = num51;
                    int num52 = index42 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row41.GetElement<string>(index42), (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    this.info.faceinfo[1].HeadRot[0] = zero;
                    List<string> row42 = this.row;
                    int index43 = num52;
                    int num53 = index43 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row42.GetElement<string>(index43), (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    List<string> row43 = this.row;
                    int index44 = num53;
                    int num54 = index44 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row43.GetElement<string>(index44), (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    List<string> row44 = this.row;
                    int index45 = num54;
                    int num55 = index45 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row44.GetElement<string>(index45), (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    this.info.faceinfo[1].HeadRot[1] = zero;
                    ref HMotionEyeNeckFemale.EyeNeckFace local10 = ref this.info.faceinfo[1];
                    List<string> row45 = this.row;
                    int index46 = num55;
                    int num56 = index46 + 1;
                    int num57 = int.Parse(row45.GetElement<string>(index46));
                    local10.targetEye = num57;
                    List<string> row46 = this.row;
                    int index47 = num56;
                    int num58 = index47 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row46.GetElement<string>(index47), (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    List<string> row47 = this.row;
                    int index48 = num58;
                    int num59 = index48 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row47.GetElement<string>(index48), (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    List<string> row48 = this.row;
                    int index49 = num59;
                    int num60 = index49 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row48.GetElement<string>(index49), (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    this.info.faceinfo[1].EyeRot[0] = zero;
                    List<string> row49 = this.row;
                    int index50 = num60;
                    int num61 = index50 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row49.GetElement<string>(index50), (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    List<string> row50 = this.row;
                    int index51 = num61;
                    int num62 = index51 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row50.GetElement<string>(index51), (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    List<string> row51 = this.row;
                    int index52 = num62;
                    int num63 = index52 + 1;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(row51.GetElement<string>(index52), (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    this.info.faceinfo[1].EyeRot[1] = zero;
                    this.lstEyeNeck.Add(this.info);
                  }
                }
              }
            }
          }
        }
      }
    }
    return true;
  }

  public bool SetPartner(
    GameObject _objMale1Bone,
    GameObject _objMale2Bone,
    GameObject _objFemale1Bone)
  {
    this.objMale1Head = (GameObject) null;
    this.objMale2Head = (GameObject) null;
    this.objFemale1Head = (GameObject) null;
    this.objMale1Genital = (GameObject) null;
    this.objMale2Genital = (GameObject) null;
    this.objFemale1Genital = (GameObject) null;
    if (Object.op_Implicit((Object) _objMale1Bone))
    {
      this.LoopParent = _objMale1Bone.get_transform();
      if (this.strMaleHead != string.Empty)
        this.objMale1Head = this.GetObjectName(this.LoopParent, this.strMaleHead);
      if (this.strMaleGenital != string.Empty)
        this.objMale1Genital = this.GetObjectName(this.LoopParent, this.strMaleGenital);
    }
    if (Object.op_Implicit((Object) _objMale2Bone))
    {
      this.LoopParent = _objMale2Bone.get_transform();
      if (this.strMaleHead != string.Empty)
        this.objMale2Head = this.GetObjectName(this.LoopParent, this.strMaleHead);
      if (this.strMaleGenital != string.Empty)
        this.objMale2Genital = this.GetObjectName(this.LoopParent, this.strMaleGenital);
    }
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

  public bool Proc(AnimatorStateInfo _ai, HVoiceCtrl.FaceInfo _faceVoice, int _main)
  {
    bool flag1 = _main != 0 ? Manager.Config.HData.EyeDir1 : Manager.Config.HData.EyeDir0;
    bool flag2 = _main != 0 ? Manager.Config.HData.NeckDir1 : Manager.Config.HData.NeckDir0;
    this.ai = _ai;
    this.faceInfo = _faceVoice;
    for (int index = 0; index < this.lstEyeNeck.Count; ++index)
    {
      this.en = this.lstEyeNeck[index];
      if (((AnimatorStateInfo) ref _ai).IsName(this.en.anim))
        this.enFace = !this.en.ExistFaceInfoIya ? this.en.faceinfo[0] : this.en.faceinfo[Singleton<HSceneManager>.Instance.isForce ? 1 : 0];
    }
    if (this.hscene.ctrlVoice.nowVoices[this.ID].state == HVoiceCtrl.VoiceKind.startVoice || this.hscene.ctrlVoice.nowVoices[this.ID].state == HVoiceCtrl.VoiceKind.voice)
    {
      if (!this.enFace.isDisregardVoiceEye)
      {
        this.bFaceInfo[0] = true;
        this.SetEyesTarget(!flag1 ? (_faceVoice == null ? 0 : _faceVoice.targetEyeLine) : 0);
        this.SetBehaviourEyes(!flag1 ? (_faceVoice == null ? 0 : _faceVoice.behaviorEyeLine) : 1);
      }
      else
      {
        this.SetEyesTarget(!this.en.isConfigDisregardEye ? (!flag1 ? this.enFace.targetEye : 0) : this.enFace.targetEye);
        this.SetBehaviourEyes(!this.en.isConfigDisregardEye ? (!flag1 ? this.enFace.Eyebehaviour : 1) : this.enFace.Eyebehaviour);
      }
      if (!this.enFace.isDisregardVoiceNeck)
      {
        this.bFaceInfo[1] = true;
        this.SetNeckTarget(!flag2 ? (_faceVoice == null ? 0 : _faceVoice.targetNeckLine) : 0);
        this.SetBehaviourNeck(!flag2 ? (_faceVoice == null ? 0 : _faceVoice.behaviorNeckLine) : 2);
      }
      else
      {
        this.SetNeckTarget(!this.en.isConfigDisregardNeck ? (!flag2 ? this.enFace.targetNeck : 0) : this.enFace.targetNeck);
        this.SetBehaviourNeck(!this.en.isConfigDisregardNeck ? (!flag2 ? this.enFace.Neckbehaviour : 1) : this.enFace.Neckbehaviour);
      }
    }
    else
    {
      this.SetNeckTarget(!this.en.isConfigDisregardNeck ? (!flag2 ? this.enFace.targetNeck : 0) : this.enFace.targetNeck);
      this.SetEyesTarget(!this.en.isConfigDisregardEye ? (!flag1 ? this.enFace.targetEye : 0) : this.enFace.targetEye);
      this.SetBehaviourEyes(!this.en.isConfigDisregardEye ? (!flag1 ? this.enFace.Eyebehaviour : 1) : this.enFace.Eyebehaviour);
      this.SetBehaviourNeck(!this.en.isConfigDisregardNeck ? (!flag2 ? this.enFace.Neckbehaviour : 1) : this.enFace.Neckbehaviour);
      this.bFaceInfo[0] = false;
      this.bFaceInfo[1] = false;
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
    bool flag1 = this.ID != 0 ? Manager.Config.HData.NeckDir1 : Manager.Config.HData.NeckDir0;
    bool flag2 = this.ID != 0 ? Manager.Config.HData.EyeDir1 : Manager.Config.HData.EyeDir0;
    if (!this.bFaceInfo[0] || !this.bFaceInfo[1])
    {
      for (int index = 0; index < this.lstEyeNeck.Count; ++index)
      {
        this.en = this.lstEyeNeck[index];
        if (((AnimatorStateInfo) ref this.ai).IsName(this.en.anim))
        {
          this.enFace = !this.en.ExistFaceInfoIya ? this.en.faceinfo[0] : this.en.faceinfo[Singleton<HSceneManager>.Instance.isForce ? 1 : 0];
          if (!flag1)
          {
            if (!this.bFaceInfo[1])
            {
              if (this.enFace.targetNeck == 7)
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
            }
            else if (this.faceInfo != null && this.faceInfo.targetNeckLine == 7)
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
              this.NeckCalc(this.faceInfo.NeckRot[this.nYuragiType], this.faceInfo.HeadRot[this.nYuragiType]);
            }
          }
          if (flag2)
            break;
          if (!this.bFaceInfo[0])
          {
            if (this.enFace.targetEye != 7)
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
          if (this.faceInfo == null || this.faceInfo.targetEyeLine != 7)
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
          this.EyeCalc(this.faceInfo.EyeRot[this.nYuragiType]);
          break;
        }
      }
    }
    else
    {
      if (this.faceInfo == null)
        return;
      if (!flag1 && this.faceInfo.targetNeckLine == 7)
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
        this.NeckCalc(this.faceInfo.NeckRot[this.nYuragiType], this.faceInfo.HeadRot[this.nYuragiType]);
      }
      if (flag2 || this.faceInfo.targetEyeLine != 7)
        return;
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
      this.EyeCalc(this.faceInfo.EyeRot[this.nYuragiType]);
    }
  }

  private bool SetEyesTarget(int _tag)
  {
    switch (_tag)
    {
      case 1:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objMale1Head) ? (Transform) null : this.objMale1Head.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 2:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objMale1Genital) ? (Transform) null : this.objMale1Genital.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 3:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objMale2Head) ? (Transform) null : this.objMale2Head.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 4:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objMale2Genital) ? (Transform) null : this.objMale2Genital.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 5:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale1Head) ? (Transform) null : this.objFemale1Head.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      case 6:
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objFemale1Genital) ? (Transform) null : this.objFemale1Genital.get_transform(), 0.5f, 0.0f, 1f, 2f);
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
        this.chaFemale.ChangeLookEyesTarget(1, !Object.op_Implicit((Object) this.objGenitalSelf) ? (Transform) null : this.objGenitalSelf.get_transform(), 0.5f, 0.0f, 1f, 2f);
        goto case 7;
      default:
        this.chaFemale.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
        goto case 7;
    }
  }

  private bool SetNeckTarget(int _tag)
  {
    switch (_tag)
    {
      case 1:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objMale1Head) ? (Transform) null : this.objMale1Head.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 2:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objMale1Genital) ? (Transform) null : this.objMale1Genital.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 3:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objMale2Head) ? (Transform) null : this.objMale2Head.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 4:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objMale2Genital) ? (Transform) null : this.objMale2Genital.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 5:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale1Head) ? (Transform) null : this.objFemale1Head.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      case 6:
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objFemale1Genital) ? (Transform) null : this.objFemale1Genital.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
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
        this.chaFemale.ChangeLookNeckTarget(1, !Object.op_Implicit((Object) this.objGenitalSelf) ? (Transform) null : this.objGenitalSelf.get_transform(), 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
      default:
        this.chaFemale.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
        goto case 7;
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
    float num1 = Mathf.InverseLerp(0.0f, 1f, this.ChangeTimeEye);
    this.nowleapSpeed[1] = Mathf.Clamp01(this.chaFemale.eyeLookCtrl.eyeLookScript.eyeTypeStates[0].leapSpeed * deltaTime);
    Quaternion quaternion1 = Quaternion.Slerp(this.BackUpEye[0], Quaternion.Slerp(this.EyeTrs[0].get_localRotation(), Quaternion.Slerp(this.BackUpEye[0], Quaternion.Euler(targetEyeRot), this.nowleapSpeed[1]), 1f), num1);
    ref Quaternion local1 = ref this.BackUpEye[0];
    Quaternion quaternion2 = quaternion1;
    this.EyeTrs[0].set_localRotation(quaternion2);
    Quaternion quaternion3 = quaternion2;
    local1 = quaternion3;
    Quaternion quaternion4 = Quaternion.Slerp(this.BackUpEye[1], Quaternion.Slerp(this.EyeTrs[1].get_localRotation(), Quaternion.Slerp(this.BackUpEye[1], Quaternion.Euler(targetEyeRot), this.nowleapSpeed[1]), 1f), num1);
    ref Quaternion local2 = ref this.BackUpEye[1];
    Quaternion quaternion5 = quaternion4;
    this.EyeTrs[1].set_localRotation(quaternion5);
    Quaternion quaternion6 = quaternion5;
    local2 = quaternion6;
    Quaternion localRotation1 = this.EyeTrs[1].get_localRotation();
    float y = (float) ((Quaternion) ref localRotation1).get_eulerAngles().y;
    Quaternion localRotation2 = this.EyeTrs[1].get_localRotation();
    float x = (float) ((Quaternion) ref localRotation2).get_eulerAngles().x;
    float num2 = (double) y <= 180.0 ? y : y - 360f;
    float num3 = (double) x <= 180.0 ? x : x - 360f;
    this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[0].angleH = num2;
    this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[0].angleV = num3;
    this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[1].angleH = num2;
    this.chaFemale.eyeLookCtrl.eyeLookScript.eyeObjs[1].angleV = num3;
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
    public bool ExistFaceInfoIya;
    public HMotionEyeNeckFemale.EyeNeckFace[] faceinfo;

    public void Init()
    {
      this.anim = string.Empty;
      this.isConfigDisregardNeck = false;
      this.isConfigDisregardEye = false;
      this.ExistFaceInfoIya = false;
      this.faceinfo = new HMotionEyeNeckFemale.EyeNeckFace[2];
      this.faceinfo[0].Init();
      this.faceinfo[1].Init();
    }
  }

  [Serializable]
  public struct EyeNeckFace
  {
    [Label("セリフ無視(首)")]
    public bool isDisregardVoiceNeck;
    [Label("セリフ無視(目)")]
    public bool isDisregardVoiceEye;
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
      this.isDisregardVoiceNeck = false;
      this.isDisregardVoiceEye = false;
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
