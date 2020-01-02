// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using ConfigScene;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomControl : MonoBehaviour
  {
    [Header("デバッグ ------------------------------")]
    [Button("InitializeScneUI", "シーンUI初期設定", new object[] {})]
    public int initializescneui;
    [Header("メンバ -------------------------------")]
    public CameraControl_Ver2 camCtrl;
    [SerializeField]
    private CustomCapture _customCap;
    [SerializeField]
    private CvsA_Slot cvsA_Slot;
    [SerializeField]
    private CvsH_Hair cvsH_Hair;
    public Canvas cvsChangeScene;
    [SerializeField]
    private Text textFullName;
    [SerializeField]
    private GameObject objMainCanvas;
    [SerializeField]
    private GameObject objSubCanvas;
    [SerializeField]
    private CanvasGroup cvgDrawCanvas;
    [SerializeField]
    private CanvasGroup cvgFusionCanvas;
    [SerializeField]
    private GameObject objCapCanvas;
    [SerializeField]
    private CanvasGroup cvgColorPanel;
    [SerializeField]
    private CanvasGroup cvgPattern;
    [SerializeField]
    private CanvasGroup cvgShortcut;
    [SerializeField]
    private CanvasGroup cvsInputCoordinate;
    private bool firstUpdate;
    private BoolReactiveProperty _saveMode;
    private BoolReactiveProperty _updatePng;
    private List<bool> lstShow;
    [Header("スクロールのRaycast -------------------")]
    public CustomControl.SliderScrollRaycast sliderScrollRaycast;
    [Header("条件による表示 ------------------------")]
    [SerializeField]
    private CustomControl.HideTrial hideTrial;
    [Header("条件による表示 ------------------------")]
    [SerializeField]
    private CustomControl.HideTrialOnly hideTrialOnly;
    [Header("条件による表示 ------------------------")]
    [SerializeField]
    private CustomControl.DisplayByCondition hideByCondition;
    [Header("背景関連 ------------------------------")]
    [SerializeField]
    private BackgroundCtrl bgCtrl;
    [SerializeField]
    private BoolReactiveProperty _draw3D;
    [SerializeField]
    private GameObject obj2DTop;
    [SerializeField]
    private GameObject obj3DTop;
    [SerializeField]
    private Renderer rendBG;

    public CustomControl()
    {
      base.\u002Ector();
    }

    public CustomCapture customCap
    {
      get
      {
        return this._customCap;
      }
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
      set
      {
        this.customBase.chaCtrl = value;
      }
    }

    private ChaFileControl chaFile
    {
      get
      {
        return this.chaCtrl.chaFile;
      }
    }

    private ChaFileBody body
    {
      get
      {
        return this.chaFile.custom.body;
      }
    }

    private ChaFileFace face
    {
      get
      {
        return this.chaFile.custom.face;
      }
    }

    private ChaFileHair hair
    {
      get
      {
        return this.chaFile.custom.hair;
      }
    }

    private bool modeNew
    {
      get
      {
        return this.customBase.modeNew;
      }
      set
      {
        this.customBase.modeNew = value;
      }
    }

    private byte modeSex
    {
      get
      {
        return this.customBase.modeSex;
      }
      set
      {
        this.customBase.modeSex = value;
      }
    }

    public bool saveMode
    {
      get
      {
        return ((ReactiveProperty<bool>) this._saveMode).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._saveMode).set_Value(value);
      }
    }

    public string overwriteSavePath { get; set; }

    public bool updatePng
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updatePng).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updatePng).set_Value(value);
      }
    }

    public bool showMainCvs { get; set; }

    public bool showFusionCvs { get; set; }

    public bool showDrawMenu { get; set; }

    public bool showColorCvs { get; set; }

    public bool showFileList { get; set; }

    public bool showPattern { get; set; }

    public bool showShortcut { get; set; }

    public bool showInputCoordinate { get; set; }

    public void Initialize(byte _sex, bool _new, string _nextScene, string _editCharaFileName = "")
    {
      this.modeSex = _sex;
      this.modeNew = _new;
      this.customBase.nextSceneName = _nextScene;
      if (!this.modeNew)
        this.customBase.editSaveFileName = _editCharaFileName;
      this.customBase.customCtrl = this;
      this.customBase.saveFrameAssist.Initialize();
      this.customBase.drawSaveFrameTop = false;
      this.customBase.drawSaveFrameBack = true;
      this.customBase.drawSaveFrameFront = true;
      if (this.modeNew)
        this.customBase.defChaCtrl.LoadFromAssetBundle(this.modeSex != (byte) 0 ? "custom/00/presets_f_00.unity3d" : "custom/00/presets_m_00.unity3d", this.modeSex != (byte) 0 ? "ill_Default_Female" : "ill_Default_Male", false, true);
      else
        this.customBase.defChaCtrl.LoadCharaFile(this.customBase.editSaveFileName, this.modeSex, false, true);
      foreach (VoiceInfo.Param obj in Singleton<Manager.Voice>.Instance.voiceInfoDic.Values.Where<VoiceInfo.Param>((Func<VoiceInfo.Param, bool>) (x => 0 <= x.No)).ToArray<VoiceInfo.Param>())
        this.customBase.dictPersonality[obj.No] = obj.Personality;
      this.InitializeMapControl();
      this.LoadChara();
      this.customBase.poseNo = 1;
      this.customBase.customMotionIK = new MotionIK(this.chaCtrl, false, (MotionIKData) null);
      this.customBase.customMotionIK.MapIK = false;
      this.customBase.customMotionIK.SetPartners(this.customBase.customMotionIK);
      this.customBase.customMotionIK.Reset();
      if (this.modeSex == (byte) 0)
      {
        foreach (GameObject self in this.hideByCondition.objMale)
        {
          if (Object.op_Implicit((Object) self))
            self.SetActiveIfDifferent(false);
        }
      }
      else
      {
        foreach (GameObject self in this.hideByCondition.objFemale)
        {
          if (Object.op_Implicit((Object) self))
            self.SetActiveIfDifferent(false);
        }
      }
      if (this.modeNew)
      {
        foreach (GameObject self in this.hideByCondition.objNew)
        {
          if (Object.op_Implicit((Object) self))
            self.SetActiveIfDifferent(false);
        }
      }
      else
      {
        foreach (GameObject self in this.hideByCondition.objEdit)
        {
          if (Object.op_Implicit((Object) self))
            self.SetActiveIfDifferent(false);
        }
      }
      if (this.hideTrialOnly != null && this.hideTrialOnly.objHide != null)
      {
        foreach (GameObject self in this.hideTrialOnly.objHide)
          self.SetActiveIfDifferent(false);
      }
      this.customBase.forceUpdateAcsList = true;
      this.customBase.updateCustomUI = true;
    }

    private void LoadChara()
    {
      Singleton<Character>.Instance.BeginLoadAssetBundle();
      if (this.modeNew)
      {
        this.chaCtrl = Singleton<Character>.Instance.CreateChara(this.modeSex, ((Component) this).get_gameObject(), 0, (ChaFileControl) null);
        this.chaCtrl.chaFile.pngData = (byte[]) null;
        this.chaCtrl.chaFile.userID = Singleton<GameSystem>.Instance.UserUUID;
        this.chaCtrl.chaFile.dataID = YS_Assist.CreateUUID();
      }
      else
      {
        this.chaCtrl = Singleton<Character>.Instance.CreateChara(this.modeSex, ((Component) this).get_gameObject(), 0, (ChaFileControl) null);
        this.chaCtrl.chaFile.LoadCharaFile(this.customBase.editSaveFileName, this.modeSex, false, true);
        this.chaCtrl.ChangeNowCoordinate(false, true);
      }
      this.chaCtrl.releaseCustomInputTexture = false;
      this.chaCtrl.Load(false);
      this.chaCtrl.ChangeEyebrowPtn(0, true);
      this.chaCtrl.ChangeEyesPtn(0, true);
      this.chaCtrl.ChangeMouthPtn(0, true);
      this.chaCtrl.ChangeLookEyesPtn(1);
      this.chaCtrl.ChangeLookNeckPtn(0, 1f);
      this.chaCtrl.hideMoz = true;
      this.chaCtrl.fileStatus.visibleSon = false;
    }

    public void InitializeScneUI()
    {
      string[] strArray1 = new string[16]
      {
        nameof (CustomControl),
        "MainMenu",
        "SubMenuFace",
        "SettingWindow",
        "WinFace",
        "DefaultWin",
        "F_FaceType",
        "B_ShapeWhole",
        "H_Hair",
        "C_Clothes",
        "A_Slot",
        "O_Chara",
        "dwChara",
        "Setting01",
        "menuPicker",
        "DrawWindow"
      };
      CanvasGroup[] componentsInChildren1 = (CanvasGroup[]) ((Component) this).GetComponentsInChildren<CanvasGroup>(true);
      if (componentsInChildren1 != null && componentsInChildren1.Length != 0)
      {
        foreach (CanvasGroup canvasGroup in componentsInChildren1)
          canvasGroup.Enable(((IEnumerable<string>) strArray1).Contains<string>(((Object) canvasGroup).get_name()), false);
      }
      string[] strArray2 = new string[23]
      {
        "tglFace",
        "SameSettingEyes",
        "AutoHairColor",
        "SameHairColor",
        "ControlTogether",
        "imgRbCol00",
        "imgRB00",
        "tgl01",
        "tglControl",
        "tglCtrlMove",
        "tglDay",
        "tglChangeParentLR",
        "TglType01",
        "tglPlay",
        "TglLoadType01",
        "TglLoadType02",
        "TglLoadType03",
        "TglLoadType04",
        "TglLoadType05",
        "RbHSV",
        "rbPicker",
        "rbSample",
        "ToggleH"
      };
      Toggle[] componentsInChildren2 = (Toggle[]) ((Component) this).GetComponentsInChildren<Toggle>(true);
      if (componentsInChildren2 == null || componentsInChildren2.Length == 0)
        return;
      foreach (Toggle toggle in componentsInChildren2)
        toggle.set_isOn(((IEnumerable<string>) strArray2).Contains<string>(((Object) toggle).get_name()));
    }

    public void UpdateCharaNameText()
    {
      this.textFullName.set_text(this.chaCtrl.chaFile.parameter.fullname);
    }

    private void Start()
    {
      if (Object.op_Equality((Object) null, (Object) this.camCtrl))
      {
        GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("MainCamera");
        if (Object.op_Implicit((Object) gameObjectWithTag))
          this.camCtrl = (CameraControl_Ver2) gameObjectWithTag.GetComponent<CameraControl_Ver2>();
      }
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._saveMode, (Action<M0>) (m =>
      {
        this.showMainCvs = !m;
        if (Object.op_Implicit((Object) this.objCapCanvas))
          this.objCapCanvas.SetActiveIfDifferent(m);
        if (!m)
          return;
        this.customBase.cvsCapMenu.BeginCapture();
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._updatePng, (Action<M0>) (m =>
      {
        this.showMainCvs = !m;
        if (Object.op_Implicit((Object) this.objCapCanvas))
          this.objCapCanvas.SetActiveIfDifferent(m);
        if (!m)
          return;
        this.customBase.cvsCapMenu.BeginCapture();
      }));
    }

    private void Update()
    {
      bool flag = Object.op_Inequality((Object) null, (Object) Singleton<Game>.Instance.Config);
      this.lstShow.Clear();
      this.lstShow.Add(this.showMainCvs);
      this.lstShow.Add(!flag);
      this.lstShow.Add(!this.showFusionCvs);
      this.lstShow.Add(!this.showInputCoordinate);
      bool active = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.objMainCanvas))
        this.objMainCanvas.SetActiveIfDifferent(active);
      if (Object.op_Implicit((Object) this.objSubCanvas))
        this.objSubCanvas.SetActiveIfDifferent(active);
      this.lstShow.Clear();
      this.lstShow.Add(this.showFusionCvs);
      this.lstShow.Add(!flag);
      bool enable1 = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.cvgFusionCanvas))
        this.cvgFusionCanvas.Enable(enable1, false);
      this.lstShow.Clear();
      this.lstShow.Add(this.showDrawMenu);
      this.lstShow.Add(!this.showFusionCvs);
      this.lstShow.Add(!this.showFileList);
      this.lstShow.Add(!flag);
      bool enable2 = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.cvgDrawCanvas))
        this.cvgDrawCanvas.Enable(enable2, false);
      this.lstShow.Clear();
      this.lstShow.Add(this.showColorCvs);
      this.lstShow.Add(!this.showFusionCvs);
      this.lstShow.Add(this.saveMode || this.updatePng || !this.showFileList);
      this.lstShow.Add(!flag);
      bool enable3 = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.cvgColorPanel))
        this.cvgColorPanel.Enable(enable3, false);
      this.lstShow.Clear();
      this.lstShow.Add(this.showPattern);
      this.lstShow.Add(!flag);
      bool enable4 = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.cvgPattern))
        this.cvgPattern.Enable(enable4, false);
      this.lstShow.Clear();
      this.lstShow.Add(this.showInputCoordinate);
      bool enable5 = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.cvsInputCoordinate))
        this.cvsInputCoordinate.Enable(enable5, false);
      if (Object.op_Implicit((Object) this.cvgShortcut))
        this.cvgShortcut.Enable(this.showShortcut, false);
      if (this.saveMode || this.updatePng)
      {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
          if (Object.op_Implicit((Object) this.camCtrl))
            this.camCtrl.NoCtrlCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => false);
        }
        else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && (Illusion.Utils.uGUI.isMouseHit && Object.op_Implicit((Object) this.camCtrl)))
          this.camCtrl.NoCtrlCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => true);
      }
      else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
      {
        if (Object.op_Implicit((Object) this.camCtrl))
          this.camCtrl.NoCtrlCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => false);
      }
      else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && (Illusion.Utils.uGUI.isMouseHit && Object.op_Implicit((Object) this.camCtrl)))
        this.camCtrl.NoCtrlCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => true);
      this.customBase.UpdateIKCalc();
      if (this.customBase.playVoiceBackup.playSampleVoice && !Singleton<Sound>.Instance.IsPlay(Sound.Type.SystemSE, (string) null))
      {
        this.chaCtrl.ChangeEyebrowPtn(this.customBase.playVoiceBackup.backEyebrowPtn, true);
        this.chaCtrl.ChangeEyesPtn(this.customBase.playVoiceBackup.backEyesPtn, true);
        this.chaCtrl.HideEyeHighlight(false);
        this.chaCtrl.ChangeEyesBlinkFlag(this.customBase.playVoiceBackup.backBlink);
        this.chaCtrl.ChangeEyesOpenMax(this.customBase.playVoiceBackup.backEyesOpen);
        this.chaCtrl.ChangeMouthPtn(this.customBase.playVoiceBackup.backMouthPtn, true);
        this.chaCtrl.ChangeMouthFixed(this.customBase.playVoiceBackup.backMouthFix);
        this.chaCtrl.ChangeMouthOpenMax(this.customBase.playVoiceBackup.backMouthOpen);
        this.customBase.playVoiceBackup.playSampleVoice = false;
      }
      if (this.showShortcut)
      {
        if (Input.GetKeyDown((KeyCode) 283) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
          this.showShortcut = false;
          return;
        }
      }
      else if (Object.op_Inequality((Object) null, (Object) Singleton<Game>.Instance.Config) && Input.GetKeyDown((KeyCode) 284))
        Singleton<Game>.Instance.Config.OnBack();
      bool isInputFocused = this.customBase.IsInputFocused();
      if (!isInputFocused && "CharaCustom" == Singleton<Scene>.Instance.NowSceneNames[0] && (!this.showShortcut && Object.op_Equality((Object) null, (Object) Singleton<Game>.Instance.Config)) && Object.op_Equality((Object) null, (Object) Singleton<Game>.Instance.ExitScene))
      {
        if (Input.GetKeyDown((KeyCode) 283))
          this.showShortcut = true;
        else if (Input.GetKeyDown((KeyCode) 27))
        {
          if (Object.op_Equality((Object) null, (Object) Singleton<Game>.Instance.ExitScene))
            AIProject.GameUtil.GameEnd(true);
        }
        else if (Input.GetKeyDown((KeyCode) 122))
        {
          Manager.Config.ActData.Look = !Manager.Config.ActData.Look;
          this.customBase.centerDraw = Manager.Config.ActData.Look;
        }
        else if (Input.GetKeyDown((KeyCode) 119))
        {
          if (this.customBase.objAcs01ControllerTop.get_activeSelf() || this.customBase.objAcs02ControllerTop.get_activeSelf())
            this.cvsA_Slot.ShortcutChangeGuidType(0);
          else if (this.customBase.objHairControllerTop.get_activeSelf())
            this.cvsH_Hair.ShortcutChangeGuidType(0);
        }
        else if (Input.GetKeyDown((KeyCode) 101))
        {
          if (this.customBase.objAcs01ControllerTop.get_activeSelf() || this.customBase.objAcs02ControllerTop.get_activeSelf())
            this.cvsA_Slot.ShortcutChangeGuidType(1);
          else if (this.customBase.objHairControllerTop.get_activeSelf())
            this.cvsH_Hair.ShortcutChangeGuidType(1);
        }
        else if (Input.GetKeyDown((KeyCode) 284) && Object.op_Equality((Object) null, (Object) Singleton<Game>.Instance.Config))
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          ConfigWindow.UnLoadAction = (Action) (() => this.customBase.centerDraw = Manager.Config.ActData.Look);
          Singleton<Game>.Instance.LoadConfig();
        }
      }
      if (!this.firstUpdate && Singleton<CustomBase>.IsInstance() && this.customBase.updateCustomUI)
      {
        this.customBase.changeCharaName = true;
        this.customBase.updateCvsFaceType = true;
        this.customBase.updateCvsFaceShapeWhole = true;
        this.customBase.updateCvsFaceShapeChin = true;
        this.customBase.updateCvsFaceShapeCheek = true;
        this.customBase.updateCvsFaceShapeEyebrow = true;
        this.customBase.updateCvsFaceShapeEyes = true;
        this.customBase.updateCvsFaceShapeNose = true;
        this.customBase.updateCvsFaceShapeMouth = true;
        this.customBase.updateCvsFaceShapeEar = true;
        this.customBase.updateCvsMole = true;
        this.customBase.updateCvsEyeLR = true;
        this.customBase.updateCvsEyeEtc = true;
        this.customBase.updateCvsEyeHL = true;
        this.customBase.updateCvsEyebrow = true;
        this.customBase.updateCvsEyelashes = true;
        this.customBase.updateCvsEyeshadow = true;
        this.customBase.updateCvsCheek = true;
        this.customBase.updateCvsLip = true;
        this.customBase.updateCvsFacePaint = true;
        this.customBase.updateCvsBeard = true;
        this.customBase.updateCvsBodyShapeWhole = true;
        this.customBase.updateCvsBodyShapeBreast = true;
        this.customBase.updateCvsBodyShapeUpper = true;
        this.customBase.updateCvsBodyShapeLower = true;
        this.customBase.updateCvsBodyShapeArm = true;
        this.customBase.updateCvsBodyShapeLeg = true;
        this.customBase.updateCvsBodySkinType = true;
        this.customBase.updateCvsSunburn = true;
        this.customBase.updateCvsNip = true;
        this.customBase.updateCvsUnderhair = true;
        this.customBase.updateCvsNail = true;
        this.customBase.updateCvsBodyPaint = true;
        this.customBase.updateCvsFutanari = true;
        this.customBase.updateCvsHair = true;
        this.customBase.updateCvsClothes = true;
        this.customBase.updateCvsClothesSaveDelete = true;
        this.customBase.updateCvsClothesLoad = true;
        this.customBase.updateCvsAccessory = true;
        this.customBase.updateCvsAcsCopy = true;
        this.customBase.updateCvsChara = true;
        this.customBase.updateCvsType = true;
        this.customBase.updateCvsStatus = true;
        this.customBase.updateCvsCharaSaveDelete = true;
        this.customBase.updateCvsCharaLoad = true;
        this.customBase.updateCustomUI = false;
      }
      if (Object.op_Implicit((Object) this.camCtrl))
        this.camCtrl.KeyCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => !isInputFocused);
      this.firstUpdate = false;
    }

    public bool draw3D
    {
      get
      {
        return ((ReactiveProperty<bool>) this._draw3D).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._draw3D).set_Value(value);
      }
    }

    public void ChangeBGImage(int bf)
    {
      if (!Object.op_Implicit((Object) this.bgCtrl))
        return;
      this.bgCtrl.ChangeBGImage((byte) bf, true);
    }

    public void ChangeBGColor(Color color)
    {
      if (Object.op_Equality((Object) null, (Object) this.rendBG))
        return;
      this.rendBG.get_material().SetColor(ChaShader.Color2, color);
    }

    public Color GetBGColor()
    {
      return Object.op_Equality((Object) null, (Object) this.rendBG) ? Color.get_white() : this.rendBG.get_material().GetColor(ChaShader.Color2);
    }

    public void InitializeMapControl()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._draw3D, (Action<M0>) (isOn =>
      {
        if (Object.op_Implicit((Object) this.obj2DTop))
          this.obj2DTop.SetActiveIfDifferent(!isOn);
        if (!Object.op_Implicit((Object) this.obj3DTop))
          return;
        this.obj3DTop.SetActiveIfDifferent(isOn);
      }));
    }

    [Serializable]
    public class SliderScrollRaycast
    {
      public Image[] imgScrollRaycast;

      public void ChangeActiveRaycast(bool enable)
      {
        if (this.imgScrollRaycast == null)
          return;
        foreach (Image image in this.imgScrollRaycast)
        {
          if (!Object.op_Equality((Object) null, (Object) image))
            ((Graphic) image).set_raycastTarget(enable);
        }
      }
    }

    [Serializable]
    public class HideTrial
    {
      public GameObject[] objHide;
    }

    [Serializable]
    public class HideTrialOnly
    {
      public GameObject[] objHide;
    }

    [Serializable]
    public class DisplayByCondition
    {
      public GameObject[] objFemale;
      public GameObject[] objMale;
      public GameObject[] objNew;
      public GameObject[] objEdit;
    }
  }
}
