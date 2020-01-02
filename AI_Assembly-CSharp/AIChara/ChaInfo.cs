// Decompiled with JetBrains decompiler
// Type: AIChara.ChaInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using CharaUtils;
using Correct.Process;
using Illusion.Extensions;
using OutputLogControl;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIChara
{
  public class ChaInfo : ChaReference
  {
    private GameObject[] _objHair;
    private GameObject[] _objClothes;
    private GameObject[] _objAccessory;
    private Transform[,] _trfAcsMove;
    public Dictionary<int, Transform> dictAccessoryParent;
    private GameObject[] _objExtraAccessory;
    private Transform[,] _trfExtraAcsMove;
    private ListInfoBase[] _infoHair;
    private ListInfoBase[] _infoClothes;
    private ListInfoBase[] _infoAccessory;

    public ChaFileControl chaFile { get; protected set; }

    public ChaFileCustom fileCustom
    {
      get
      {
        return this.chaFile.custom;
      }
    }

    public ChaFileBody fileBody
    {
      get
      {
        return this.chaFile.custom.body;
      }
    }

    public ChaFileFace fileFace
    {
      get
      {
        return this.chaFile.custom.face;
      }
    }

    public ChaFileHair fileHair
    {
      get
      {
        return this.chaFile.custom.hair;
      }
    }

    public ChaFileParameter fileParam
    {
      get
      {
        return this.chaFile.parameter;
      }
    }

    public ChaFileGameInfo fileGameInfo
    {
      get
      {
        return this.chaFile.gameinfo;
      }
    }

    public ChaFileStatus fileStatus
    {
      get
      {
        return this.chaFile.status;
      }
    }

    public ChaListControl lstCtrl { get; protected set; }

    public EyeLookController eyeLookCtrl { get; protected set; }

    public NeckLookControllerVer2 neckLookCtrl { get; protected set; }

    public FaceBlendShape fbsCtrl { get; protected set; }

    public FBSCtrlEyebrow eyebrowCtrl { get; protected set; }

    public FBSCtrlEyes eyesCtrl { get; protected set; }

    public FBSCtrlMouth mouthCtrl { get; protected set; }

    public Expression expression { get; protected set; }

    public CmpBoneHead cmpBoneHead { get; protected set; }

    public CmpBoneBody cmpBoneBody { get; protected set; }

    public CmpFace cmpFace { get; protected set; }

    public CmpBody cmpBody { get; protected set; }

    public CmpBody cmpSimpleBody { get; protected set; }

    public CmpHair[] cmpHair { get; protected set; }

    public CmpHair GetCustomHairComponent(int parts)
    {
      if (this.cmpHair == null)
        return (CmpHair) null;
      if (parts >= this.cmpHair.Length)
        return (CmpHair) null;
      CmpHair cmpHair = this.cmpHair[parts];
      return Object.op_Equality((Object) null, (Object) cmpHair) ? (CmpHair) null : cmpHair;
    }

    public CmpClothes[] cmpClothes { get; protected set; }

    public CmpClothes GetCustomClothesComponent(int parts)
    {
      if (this.cmpClothes == null)
        return (CmpClothes) null;
      if (parts >= this.cmpClothes.Length)
        return (CmpClothes) null;
      CmpClothes cmpClothe = this.cmpClothes[parts];
      return Object.op_Equality((Object) null, (Object) cmpClothe) ? (CmpClothes) null : cmpClothe;
    }

    public CmpAccessory[] cmpAccessory { get; protected set; }

    public CmpAccessory GetAccessoryComponent(int parts)
    {
      if (this.cmpAccessory == null)
        return (CmpAccessory) null;
      if (parts >= this.cmpAccessory.Length)
        return (CmpAccessory) null;
      CmpAccessory cmpAccessory = this.cmpAccessory[parts];
      return Object.op_Equality((Object) null, (Object) cmpAccessory) ? (CmpAccessory) null : cmpAccessory;
    }

    public CmpAccessory[] cmpExtraAccessory { get; protected set; }

    public CmpAccessory GetExtraAccessoryComponent(int parts)
    {
      if (this.cmpExtraAccessory == null)
        return (CmpAccessory) null;
      if (parts >= this.cmpExtraAccessory.Length)
        return (CmpAccessory) null;
      CmpAccessory cmpAccessory = this.cmpExtraAccessory[parts];
      return Object.op_Equality((Object) null, (Object) cmpAccessory) ? (CmpAccessory) null : cmpAccessory;
    }

    public FullBodyBipedIK fullBodyIK { get; protected set; }

    public int chaID { get; protected set; }

    public int loadNo { get; protected set; }

    public byte sex
    {
      get
      {
        return this.chaFile.parameter.sex;
      }
    }

    public bool isPlayer { get; set; }

    public bool hideMoz { get; set; }

    public bool loadEnd { get; protected set; }

    public bool visibleAll { get; set; }

    public bool visibleBody
    {
      get
      {
        return this.fileStatus.visibleBodyAlways;
      }
      set
      {
        this.fileStatus.visibleBodyAlways = value;
      }
    }

    public bool visibleSon
    {
      get
      {
        if (this.sex == (byte) 0)
          return this.fileStatus.visibleSon;
        return this.fileParam.futanari && this.fileStatus.visibleSonAlways;
      }
      set
      {
        if (this.sex == (byte) 0)
        {
          this.fileStatus.visibleSonAlways = true;
          this.fileStatus.visibleSon = value;
        }
        else
        {
          this.fileStatus.visibleSonAlways = this.fileParam.futanari && value;
          this.fileStatus.visibleSon = true;
        }
      }
    }

    public bool updateShapeFace { get; set; }

    public bool updateShapeBody { get; set; }

    public bool updateShape
    {
      set
      {
        this.updateShapeFace = value;
        this.updateShapeBody = value;
      }
    }

    public bool updateWet { get; set; }

    public bool resetDynamicBoneAll { get; set; }

    public bool reSetupDynamicBoneBust { get; set; }

    protected bool[] enableDynamicBoneBustAndHip { get; set; }

    public bool updateBustSize { get; set; }

    public bool releaseCustomInputTexture { get; set; }

    public bool loadWithDefaultColorAndPtn { get; set; }

    protected bool[] showExtraAccessory { get; set; }

    public bool hideHairForThumbnailCapture { get; set; }

    public Renderer[] rendBra { get; protected set; }

    public Renderer rendInnerTB { get; protected set; }

    public Renderer rendInnerB { get; protected set; }

    public Renderer rendPanst { get; protected set; }

    public CustomTextureControl customTexCtrlFace { get; protected set; }

    public CustomTextureControl customTexCtrlBody { get; protected set; }

    public Material customMatFace
    {
      get
      {
        return this.customTexCtrlFace == null ? (Material) null : this.customTexCtrlFace.matDraw;
      }
    }

    public Material customMatBody
    {
      get
      {
        return this.customTexCtrlBody == null ? (Material) null : this.customTexCtrlBody.matDraw;
      }
    }

    public CustomTextureCreate[,] ctCreateClothes { get; protected set; }

    public CustomTextureCreate[,] ctCreateClothesGloss { get; protected set; }

    public GameObject objRoot { get; protected set; }

    public GameObject objTop { get; protected set; }

    public GameObject objAnim { get; protected set; }

    public GameObject objBodyBone { get; protected set; }

    public GameObject objBody { get; protected set; }

    public GameObject objSimpleBody { get; protected set; }

    public GameObject objHeadBone { get; protected set; }

    public GameObject objHead { get; protected set; }

    public GameObject[] objHair
    {
      get
      {
        return this._objHair;
      }
      protected set
      {
        this._objHair = value;
      }
    }

    public GameObject[] objClothes
    {
      get
      {
        return this._objClothes;
      }
      protected set
      {
        this._objClothes = value;
      }
    }

    public GameObject[] objAccessory
    {
      get
      {
        return this._objAccessory;
      }
      protected set
      {
        this._objAccessory = value;
      }
    }

    public Transform[,] trfAcsMove
    {
      get
      {
        return this._trfAcsMove;
      }
      protected set
      {
        this._trfAcsMove = value;
      }
    }

    public GameObject objHitBody { get; protected set; }

    public GameObject objHitHead { get; protected set; }

    public Animator animBody { get; protected set; }

    public GameObject objEyesLookTargetP { get; protected set; }

    public GameObject objEyesLookTarget { get; protected set; }

    public GameObject objNeckLookTargetP { get; protected set; }

    public GameObject objNeckLookTarget { get; protected set; }

    public GameObject[] objExtraAccessory
    {
      get
      {
        return this._objExtraAccessory;
      }
      protected set
      {
        this._objExtraAccessory = value;
      }
    }

    public ListInfoBase infoHead { get; protected set; }

    public ListInfoBase[] infoHair
    {
      get
      {
        return this._infoHair;
      }
      protected set
      {
        this._infoHair = value;
      }
    }

    public ListInfoBase[] infoClothes
    {
      get
      {
        return this._infoClothes;
      }
      protected set
      {
        this._infoClothes = value;
      }
    }

    public ListInfoBase[] infoAccessory
    {
      get
      {
        return this._infoAccessory;
      }
      protected set
      {
        this._infoAccessory = value;
      }
    }

    public bool enableExpression
    {
      get
      {
        return !Object.op_Equality((Object) null, (Object) this.expression) && this.expression.enable;
      }
      set
      {
        if (!Object.op_Inequality((Object) null, (Object) this.expression))
          return;
        this.expression.enable = value;
      }
    }

    public void EnableExpressionIndex(int indexNo, bool enable)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.expression))
        return;
      this.expression.EnableIndex(indexNo, enable);
    }

    public void EnableExpressionCategory(int categoryNo, bool enable)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.expression))
        return;
      this.expression.EnableCategory(categoryNo, enable);
    }

    public int GetBustSizeKind()
    {
      return this.fileCustom.GetBustSizeKind();
    }

    public int GetHeightKind()
    {
      return this.fileCustom.GetHeightKind();
    }

    public int GetHairType()
    {
      return this.fileHair.kind;
    }

    protected void MemberInitializeAll()
    {
      this.chaFile = (ChaFileControl) null;
      this.lstCtrl = (ChaListControl) null;
      this.chaID = 0;
      this.loadNo = -1;
      this.hideMoz = false;
      this.releaseCustomInputTexture = true;
      this.loadWithDefaultColorAndPtn = false;
      this.hideHairForThumbnailCapture = false;
      this.objRoot = (GameObject) null;
      this.customTexCtrlBody = (CustomTextureControl) null;
      this.MemberInitializeObject();
    }

    protected void MemberInitializeObject()
    {
      this.eyeLookCtrl = (EyeLookController) null;
      this.neckLookCtrl = (NeckLookControllerVer2) null;
      this.fbsCtrl = (FaceBlendShape) null;
      this.eyebrowCtrl = (FBSCtrlEyebrow) null;
      this.eyesCtrl = (FBSCtrlEyes) null;
      this.mouthCtrl = (FBSCtrlMouth) null;
      this.expression = (Expression) null;
      this.cmpFace = (CmpFace) null;
      this.cmpBody = (CmpBody) null;
      this.cmpHair = new CmpHair[Enum.GetNames(typeof (ChaFileDefine.HairKind)).Length];
      this.cmpClothes = new CmpClothes[Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length];
      this.cmpAccessory = new CmpAccessory[20];
      this.cmpExtraAccessory = new CmpAccessory[Enum.GetNames(typeof (ChaControlDefine.ExtraAccessoryParts)).Length];
      this.customTexCtrlFace = (CustomTextureControl) null;
      this.ctCreateClothes = new CustomTextureCreate[Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length, 3];
      this.ctCreateClothesGloss = new CustomTextureCreate[Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length, 3];
      this.loadEnd = false;
      this.visibleAll = true;
      this.updateShapeFace = false;
      this.updateShapeBody = false;
      this.resetDynamicBoneAll = false;
      this.reSetupDynamicBoneBust = false;
      this.enableDynamicBoneBustAndHip = new bool[4]
      {
        true,
        true,
        true,
        true
      };
      this.updateBustSize = false;
      this.showExtraAccessory = new bool[Enum.GetNames(typeof (ChaControlDefine.ExtraAccessoryParts)).Length];
      for (int index = 0; index < this.showExtraAccessory.Length; ++index)
        this.showExtraAccessory[index] = false;
      this.rendBra = new Renderer[2];
      this.rendInnerTB = (Renderer) null;
      this.rendInnerB = (Renderer) null;
      this.rendPanst = (Renderer) null;
      this.objTop = (GameObject) null;
      this.objAnim = (GameObject) null;
      this.objBodyBone = (GameObject) null;
      this.objBody = (GameObject) null;
      this.objSimpleBody = (GameObject) null;
      this.objHeadBone = (GameObject) null;
      this.objHead = (GameObject) null;
      this.objHair = new GameObject[Enum.GetNames(typeof (ChaFileDefine.HairKind)).Length];
      this.objClothes = new GameObject[Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length];
      this.objAccessory = new GameObject[20];
      this.trfAcsMove = new Transform[20, 2];
      this.objHitHead = (GameObject) null;
      this.objHitBody = (GameObject) null;
      this.animBody = (Animator) null;
      this.objEyesLookTargetP = (GameObject) null;
      this.objEyesLookTarget = (GameObject) null;
      this.objNeckLookTargetP = (GameObject) null;
      this.objNeckLookTarget = (GameObject) null;
      this.dictAccessoryParent = (Dictionary<int, Transform>) null;
      this.objExtraAccessory = new GameObject[Enum.GetNames(typeof (ChaControlDefine.ExtraAccessoryParts)).Length];
      this.infoHead = (ListInfoBase) null;
      this.infoHair = new ListInfoBase[Enum.GetNames(typeof (ChaFileDefine.HairKind)).Length];
      this.infoClothes = new ListInfoBase[Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length];
      this.infoAccessory = new ListInfoBase[20];
    }

    protected void ReleaseInfoAll()
    {
      this.ReleaseInfoObject(false);
      if (this.customTexCtrlBody != null)
        this.customTexCtrlBody.Release();
      OutputLog.Log(nameof (ReleaseInfoAll), false, "UnloadUnusedAssets");
      Resources.UnloadUnusedAssets();
    }

    protected void ReleaseInfoObject(bool init = true)
    {
      if (this.customTexCtrlFace != null)
        this.customTexCtrlFace.Release();
      if (this.ctCreateClothes != null)
      {
        for (int index1 = 0; index1 < this.ctCreateClothes.GetLength(0); ++index1)
        {
          for (int index2 = 0; index2 < 3; ++index2)
          {
            if (this.ctCreateClothes[index1, index2] != null)
            {
              this.ctCreateClothes[index1, index2].Release();
              this.ctCreateClothes[index1, index2] = (CustomTextureCreate) null;
            }
          }
        }
      }
      if (this.ctCreateClothesGloss != null)
      {
        for (int index1 = 0; index1 < this.ctCreateClothesGloss.GetLength(0); ++index1)
        {
          for (int index2 = 0; index2 < 3; ++index2)
          {
            if (this.ctCreateClothesGloss[index1, index2] != null)
            {
              this.ctCreateClothesGloss[index1, index2].Release();
              this.ctCreateClothesGloss[index1, index2] = (CustomTextureCreate) null;
            }
          }
        }
      }
      if (false)
      {
        if (Object.op_Inequality((Object) null, (Object) this.objTop))
        {
          IKAfterProcess[] componentsInChildren1 = (IKAfterProcess[]) this.objTop.GetComponentsInChildren<IKAfterProcess>(true);
          IKBeforeOfDankonProcess[] componentsInChildren2 = (IKBeforeOfDankonProcess[]) this.objTop.GetComponentsInChildren<IKBeforeOfDankonProcess>(true);
          IKBeforeProcess[] componentsInChildren3 = (IKBeforeProcess[]) this.objTop.GetComponentsInChildren<IKBeforeProcess>(true);
          for (int index = 0; index < componentsInChildren1.Length; ++index)
            Object.DestroyImmediate((Object) componentsInChildren1[index]);
          for (int index = 0; index < componentsInChildren2.Length; ++index)
            Object.DestroyImmediate((Object) componentsInChildren2[index]);
          for (int index = 0; index < componentsInChildren3.Length; ++index)
            Object.DestroyImmediate((Object) componentsInChildren3[index]);
          this.objTop.SetActiveIfDifferent(false);
          ((Object) this.objTop).set_name("Delete_Reserve");
          Object.Destroy((Object) this.objTop);
        }
      }
      else
        this.SafeDestroy(this.objTop);
      this.objTop = (GameObject) null;
      this.ReleaseRefAll();
      if (!init)
        return;
      this.MemberInitializeObject();
    }

    public void SafeDestroy(GameObject obj)
    {
      if (!Object.op_Inequality((Object) null, (Object) obj))
        return;
      IKAfterProcess[] componentsInChildren1 = (IKAfterProcess[]) obj.GetComponentsInChildren<IKAfterProcess>(true);
      IKBeforeOfDankonProcess[] componentsInChildren2 = (IKBeforeOfDankonProcess[]) obj.GetComponentsInChildren<IKBeforeOfDankonProcess>(true);
      IKBeforeProcess[] componentsInChildren3 = (IKBeforeProcess[]) obj.GetComponentsInChildren<IKBeforeProcess>(true);
      for (int index = 0; index < componentsInChildren1.Length; ++index)
        Object.DestroyImmediate((Object) componentsInChildren1[index]);
      for (int index = 0; index < componentsInChildren2.Length; ++index)
        Object.DestroyImmediate((Object) componentsInChildren2[index]);
      for (int index = 0; index < componentsInChildren3.Length; ++index)
        Object.DestroyImmediate((Object) componentsInChildren3[index]);
      obj.SetActiveIfDifferent(false);
      obj.get_transform().SetParent((Transform) null);
      ((Object) obj).set_name("Delete_Reserve");
      Object.Destroy((Object) obj);
    }

    public DynamicBone[] GetDynamicBoneHairAll()
    {
      if (this.cmpHair == null)
        return (DynamicBone[]) null;
      List<DynamicBone> dynamicBoneList = new List<DynamicBone>();
      for (int index = 0; index < this.cmpHair.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpHair[index]) && this.cmpHair[index].boneInfo != null)
        {
          foreach (DynamicBone[] dynamicBoneArray in ((IEnumerable<CmpHair.BoneInfo>) this.cmpHair[index].boneInfo).Where<CmpHair.BoneInfo>((Func<CmpHair.BoneInfo, bool>) (x => x != null && null != x.dynamicBone)).Select<CmpHair.BoneInfo, DynamicBone[]>((Func<CmpHair.BoneInfo, DynamicBone[]>) (x => x.dynamicBone)))
            dynamicBoneList.AddRange((IEnumerable<DynamicBone>) dynamicBoneArray);
        }
      }
      return dynamicBoneList.ToArray();
    }

    public DynamicBone[] GetDynamicBoneHair(int parts)
    {
      if (this.cmpHair == null)
        return (DynamicBone[]) null;
      if (parts >= this.cmpHair.Length)
        return (DynamicBone[]) null;
      if (this.cmpHair[parts].boneInfo == null)
        return (DynamicBone[]) null;
      List<DynamicBone> dynamicBoneList = new List<DynamicBone>();
      foreach (DynamicBone[] dynamicBoneArray in ((IEnumerable<CmpHair.BoneInfo>) this.cmpHair[parts].boneInfo).Where<CmpHair.BoneInfo>((Func<CmpHair.BoneInfo, bool>) (x => x != null && null != x.dynamicBone)).Select<CmpHair.BoneInfo, DynamicBone[]>((Func<CmpHair.BoneInfo, DynamicBone[]>) (x => x.dynamicBone)))
        dynamicBoneList.AddRange((IEnumerable<DynamicBone>) dynamicBoneArray);
      return dynamicBoneList.ToArray();
    }

    public void InitializeAccessoryParent()
    {
      this.dictAccessoryParent = new Dictionary<int, Transform>();
      if (Object.op_Inequality((Object) null, (Object) this.cmpBoneHead))
      {
        string[] strArray = new string[14]
        {
          "N_Hair_pony",
          "N_Hair_twin_L",
          "N_Hair_twin_R",
          "N_Hair_pin_L",
          "N_Hair_pin_R",
          "N_Head_top",
          "N_Head",
          "N_Hitai",
          "N_Face",
          "N_Megane",
          "N_Earring_L",
          "N_Earring_R",
          "N_Nose",
          "N_Mouth"
        };
        Transform[] transformArray = new Transform[14]
        {
          this.cmpBoneHead.targetAccessory.acs_Hair_pony,
          this.cmpBoneHead.targetAccessory.acs_Hair_twin_L,
          this.cmpBoneHead.targetAccessory.acs_Hair_twin_R,
          this.cmpBoneHead.targetAccessory.acs_Hair_pin_L,
          this.cmpBoneHead.targetAccessory.acs_Hair_pin_R,
          this.cmpBoneHead.targetAccessory.acs_Head_top,
          this.cmpBoneHead.targetAccessory.acs_Head,
          this.cmpBoneHead.targetAccessory.acs_Hitai,
          this.cmpBoneHead.targetAccessory.acs_Face,
          this.cmpBoneHead.targetAccessory.acs_Megane,
          this.cmpBoneHead.targetAccessory.acs_Earring_L,
          this.cmpBoneHead.targetAccessory.acs_Earring_R,
          this.cmpBoneHead.targetAccessory.acs_Nose,
          this.cmpBoneHead.targetAccessory.acs_Mouth
        };
        for (int index = 0; index < strArray.Length; ++index)
          this.dictAccessoryParent[ChaAccessoryDefine.GetAccessoryParentInt(strArray[index])] = transformArray[index];
      }
      if (!Object.op_Inequality((Object) null, (Object) this.cmpBoneBody))
        return;
      string[] strArray1 = new string[40]
      {
        "N_Neck",
        "N_Chest_f",
        "N_Chest",
        "N_Tikubi_L",
        "N_Tikubi_R",
        "N_Back",
        "N_Back_L",
        "N_Back_R",
        "N_Waist",
        "N_Waist_f",
        "N_Waist_b",
        "N_Waist_L",
        "N_Waist_R",
        "N_Leg_L",
        "N_Leg_R",
        "N_Knee_L",
        "N_Knee_R",
        "N_Ankle_L",
        "N_Ankle_R",
        "N_Foot_L",
        "N_Foot_R",
        "N_Shoulder_L",
        "N_Shoulder_R",
        "N_Elbo_L",
        "N_Elbo_R",
        "N_Arm_L",
        "N_Arm_R",
        "N_Wrist_L",
        "N_Wrist_R",
        "N_Hand_L",
        "N_Hand_R",
        "N_Index_L",
        "N_Index_R",
        "N_Middle_L",
        "N_Middle_R",
        "N_Ring_L",
        "N_Ring_R",
        "N_Dan",
        "N_Kokan",
        "N_Ana"
      };
      Transform[] transformArray1 = new Transform[40]
      {
        this.cmpBoneBody.targetAccessory.acs_Neck,
        this.cmpBoneBody.targetAccessory.acs_Chest_f,
        this.cmpBoneBody.targetAccessory.acs_Chest,
        this.cmpBoneBody.targetAccessory.acs_Tikubi_L,
        this.cmpBoneBody.targetAccessory.acs_Tikubi_R,
        this.cmpBoneBody.targetAccessory.acs_Back,
        this.cmpBoneBody.targetAccessory.acs_Back_L,
        this.cmpBoneBody.targetAccessory.acs_Back_R,
        this.cmpBoneBody.targetAccessory.acs_Waist,
        this.cmpBoneBody.targetAccessory.acs_Waist_f,
        this.cmpBoneBody.targetAccessory.acs_Waist_b,
        this.cmpBoneBody.targetAccessory.acs_Waist_L,
        this.cmpBoneBody.targetAccessory.acs_Waist_R,
        this.cmpBoneBody.targetAccessory.acs_Leg_L,
        this.cmpBoneBody.targetAccessory.acs_Leg_R,
        this.cmpBoneBody.targetAccessory.acs_Knee_L,
        this.cmpBoneBody.targetAccessory.acs_Knee_R,
        this.cmpBoneBody.targetAccessory.acs_Ankle_L,
        this.cmpBoneBody.targetAccessory.acs_Ankle_R,
        this.cmpBoneBody.targetAccessory.acs_Foot_L,
        this.cmpBoneBody.targetAccessory.acs_Foot_R,
        this.cmpBoneBody.targetAccessory.acs_Shoulder_L,
        this.cmpBoneBody.targetAccessory.acs_Shoulder_R,
        this.cmpBoneBody.targetAccessory.acs_Elbo_L,
        this.cmpBoneBody.targetAccessory.acs_Elbo_R,
        this.cmpBoneBody.targetAccessory.acs_Arm_L,
        this.cmpBoneBody.targetAccessory.acs_Arm_R,
        this.cmpBoneBody.targetAccessory.acs_Wrist_L,
        this.cmpBoneBody.targetAccessory.acs_Wrist_R,
        this.cmpBoneBody.targetAccessory.acs_Hand_L,
        this.cmpBoneBody.targetAccessory.acs_Hand_R,
        this.cmpBoneBody.targetAccessory.acs_Index_L,
        this.cmpBoneBody.targetAccessory.acs_Index_R,
        this.cmpBoneBody.targetAccessory.acs_Middle_L,
        this.cmpBoneBody.targetAccessory.acs_Middle_R,
        this.cmpBoneBody.targetAccessory.acs_Ring_L,
        this.cmpBoneBody.targetAccessory.acs_Ring_R,
        this.cmpBoneBody.targetAccessory.acs_Dan,
        this.cmpBoneBody.targetAccessory.acs_Kokan,
        this.cmpBoneBody.targetAccessory.acs_Ana
      };
      for (int index = 0; index < strArray1.Length; ++index)
        this.dictAccessoryParent[ChaAccessoryDefine.GetAccessoryParentInt(strArray1[index])] = transformArray1[index];
    }

    public Transform GetAccessoryParentTransform(string key)
    {
      Transform transform;
      return this.dictAccessoryParent.TryGetValue(ChaAccessoryDefine.GetAccessoryParentInt(key), out transform) ? transform : (Transform) null;
    }

    public Transform GetAccessoryParentTransform(int index)
    {
      Transform transform;
      return this.dictAccessoryParent.TryGetValue(index, out transform) ? transform : (Transform) null;
    }
  }
}
