// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomBase : Singleton<CustomBase>
  {
    [Header("-----------------------------------------")]
    private BoolReactiveProperty _sliderControlWheel = new BoolReactiveProperty(true);
    public CustomBase.PlayVoiceBackup playVoiceBackup = new CustomBase.PlayVoiceBackup();
    public CustomBase.CustomSettingSave customSettingSave = new CustomBase.CustomSettingSave();
    public ChaFileControl defChaCtrl = new ChaFileControl();
    private string animeStateName = string.Empty;
    private List<bool> lstShow = new List<bool>();
    public bool[] showAcsController = new bool[2];
    public Dictionary<int, string> dictPersonality = new Dictionary<int, string>();
    public List<InputField> lstInputField = new List<InputField>();
    private BoolReactiveProperty _centerDraw = new BoolReactiveProperty(true);
    private FloatReactiveProperty _bgmVol = new FloatReactiveProperty(0.3f);
    private FloatReactiveProperty _seVol = new FloatReactiveProperty(0.5f);
    private BoolReactiveProperty _drawSaveFrameTop = new BoolReactiveProperty(false);
    private BoolReactiveProperty _forceBackFrameHide = new BoolReactiveProperty(false);
    private BoolReactiveProperty _drawSaveFrameBack = new BoolReactiveProperty(false);
    private BoolReactiveProperty _drawSaveFrameFront = new BoolReactiveProperty(false);
    private BoolReactiveProperty _changeCharaName = new BoolReactiveProperty(false);
    private BoolReactiveProperty _drawTopHairColor = new BoolReactiveProperty(false);
    private BoolReactiveProperty _drawUnderHairColor = new BoolReactiveProperty(false);
    private BoolReactiveProperty _autoHairColor = new BoolReactiveProperty(false);
    private BoolReactiveProperty _playPoseAnime = new BoolReactiveProperty(true);
    private BoolReactiveProperty _cursorDraw = new BoolReactiveProperty(true);
    private BoolReactiveProperty _accessoryDraw = new BoolReactiveProperty(true);
    public int backPoseNo = -1;
    private IntReactiveProperty _poseNo = new IntReactiveProperty(-1);
    public float animationPos = -1f;
    private IntReactiveProperty _eyelook = new IntReactiveProperty(0);
    private IntReactiveProperty _necklook = new IntReactiveProperty(1);
    private BoolReactiveProperty _updateCvsFaceType = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeWhole = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeChin = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeCheek = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeEyebrow = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeEyes = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeNose = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeMouth = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFaceShapeEar = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsMole = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsEyeLR = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsEyeEtc = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsEyeHL = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsEyebrow = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsEyelashes = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsEyeshadow = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsCheek = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsLip = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFacePaint = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBeard = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodyShapeWhole = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodyShapeBreast = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodyShapeUpper = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodyShapeLower = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodyShapeArm = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodyShapeLeg = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodySkinType = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsSunburn = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsNip = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsUnderhair = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsNail = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsBodyPaint = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFutanari = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsHair = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsClothes = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsClothesSaveDelete = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsClothesLoad = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsAccessory = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsAcsCopy = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsChara = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsType = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsStatus = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsCharaSaveDelete = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsCharaLoad = new BoolReactiveProperty(false);
    private BoolReactiveProperty _updateCvsFusion = new BoolReactiveProperty(false);
    public CustomColorCtrl customColorCtrl;
    public CvsCaptureMenu cvsCapMenu;
    public CustomDrawMenu drawMenu;
    public SaveFrameAssist saveFrameAssist;
    public Light lightCustom;
    public GameObject objAcs01ControllerTop;
    public GameObject objAcs02ControllerTop;
    public GameObject objHairControllerTop;
    [SerializeField]
    private Toggle tglEyesSameSetting;
    [SerializeField]
    private Toggle tglHairSameSetting;
    [SerializeField]
    private Toggle tglHairAutoSetting;
    [SerializeField]
    private Toggle tglHairControlTogether;
    [SerializeField]
    private Toggle tglSliderWheel;
    [SerializeField]
    private Toggle tglCenterDraw;
    [SerializeField]
    private Slider sldBGMVol;
    [SerializeField]
    private Slider sldSEVol;
    [SerializeField]
    private UI_ButtonEx subMenuBot;
    [SerializeField]
    private UI_ButtonEx subMenuInnerDown;
    [Header("髪の毛関連の表示 ------------------------")]
    [SerializeField]
    private CustomBase.HairUICondition hairUICondition;
    [Header("アクセサリのスロット名 ------------------")]
    [SerializeField]
    private Text[] acsSlotText;
    public CustomControl customCtrl;
    public bool forceUpdateAcsList;

    public bool sliderControlWheel
    {
      get
      {
        return ((ReactiveProperty<bool>) this._sliderControlWheel).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._sliderControlWheel).set_Value(value);
      }
    }

    public string nextSceneName { get; set; } = string.Empty;

    public string editSaveFileName { get; set; } = string.Empty;

    public bool modeNew { get; set; } = true;

    public byte modeSex { get; set; } = 1;

    public ChaControl chaCtrl { get; set; }

    public MotionIK customMotionIK { get; set; }

    public bool autoClothesState { get; set; } = true;

    public int autoClothesStateNo { get; set; }

    public int clothesStateNo { get; set; }

    public bool showAcsControllerAll { get; set; }

    public bool showHairController { get; set; }

    public int eyebrowPtn { get; set; }

    public int eyePtn { get; set; }

    public int mouthPtn { get; set; }

    public bool centerDraw
    {
      get
      {
        return ((ReactiveProperty<bool>) this._centerDraw).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._centerDraw).set_Value(value);
      }
    }

    public float bgmVol
    {
      get
      {
        return ((ReactiveProperty<float>) this._bgmVol).get_Value();
      }
      set
      {
        ((ReactiveProperty<float>) this._bgmVol).set_Value(value);
      }
    }

    public float seVol
    {
      get
      {
        return ((ReactiveProperty<float>) this._seVol).get_Value();
      }
      set
      {
        ((ReactiveProperty<float>) this._seVol).set_Value(value);
      }
    }

    public bool drawSaveFrameTop
    {
      get
      {
        return ((ReactiveProperty<bool>) this._drawSaveFrameTop).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._drawSaveFrameTop).set_Value(value);
      }
    }

    public bool forceBackFrameHide
    {
      get
      {
        return ((ReactiveProperty<bool>) this._forceBackFrameHide).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._forceBackFrameHide).set_Value(value);
      }
    }

    public bool drawSaveFrameBack
    {
      get
      {
        return ((ReactiveProperty<bool>) this._drawSaveFrameBack).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._drawSaveFrameBack).set_Value(value);
      }
    }

    public bool drawSaveFrameFront
    {
      get
      {
        return ((ReactiveProperty<bool>) this._drawSaveFrameFront).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._drawSaveFrameFront).set_Value(value);
      }
    }

    public bool changeCharaName
    {
      get
      {
        return ((ReactiveProperty<bool>) this._changeCharaName).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._changeCharaName).set_Value(value);
      }
    }

    public bool drawTopHairColor
    {
      get
      {
        return ((ReactiveProperty<bool>) this._drawTopHairColor).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._drawTopHairColor).set_Value(value);
      }
    }

    public bool drawUnderHairColor
    {
      get
      {
        return ((ReactiveProperty<bool>) this._drawUnderHairColor).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._drawUnderHairColor).set_Value(value);
      }
    }

    public bool autoHairColor
    {
      get
      {
        return ((ReactiveProperty<bool>) this._autoHairColor).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._autoHairColor).set_Value(value);
      }
    }

    public bool playPoseAnime
    {
      get
      {
        return ((ReactiveProperty<bool>) this._playPoseAnime).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._playPoseAnime).set_Value(value);
      }
    }

    public bool cursorDraw
    {
      get
      {
        return ((ReactiveProperty<bool>) this._cursorDraw).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._cursorDraw).set_Value(value);
      }
    }

    public bool updateCustomUI { get; set; }

    public bool accessoryDraw
    {
      get
      {
        return ((ReactiveProperty<bool>) this._accessoryDraw).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._accessoryDraw).set_Value(value);
      }
    }

    public int poseNo
    {
      get
      {
        return ((ReactiveProperty<int>) this._poseNo).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._poseNo).set_Value(value);
      }
    }

    public int eyelook
    {
      get
      {
        return ((ReactiveProperty<int>) this._eyelook).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._eyelook).set_Value(value);
      }
    }

    public int necklook
    {
      get
      {
        return ((ReactiveProperty<int>) this._necklook).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._necklook).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceType;

    public bool updateCvsFaceType
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceType).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceType).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeWhole;

    public bool updateCvsFaceShapeWhole
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeWhole).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeWhole).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeChin;

    public bool updateCvsFaceShapeChin
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeChin).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeChin).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeCheek;

    public bool updateCvsFaceShapeCheek
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeCheek).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeCheek).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeEyebrow;

    public bool updateCvsFaceShapeEyebrow
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeEyebrow).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeEyebrow).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeEyes;

    public bool updateCvsFaceShapeEyes
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeEyes).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeEyes).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeNose;

    public bool updateCvsFaceShapeNose
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeNose).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeNose).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeMouth;

    public bool updateCvsFaceShapeMouth
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeMouth).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeMouth).set_Value(value);
      }
    }

    public event Action actUpdateCvsFaceShapeEar;

    public bool updateCvsFaceShapeEar
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFaceShapeEar).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFaceShapeEar).set_Value(value);
      }
    }

    public event Action actUpdateCvsMole;

    public bool updateCvsMole
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsMole).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsMole).set_Value(value);
      }
    }

    public event Action actUpdateCvsEyeLR;

    public bool updateCvsEyeLR
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsEyeLR).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsEyeLR).set_Value(value);
      }
    }

    public event Action actUpdateCvsEyeEtc;

    public bool updateCvsEyeEtc
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsEyeEtc).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsEyeEtc).set_Value(value);
      }
    }

    public event Action actUpdateCvsEyeHL;

    public bool updateCvsEyeHL
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsEyeHL).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsEyeHL).set_Value(value);
      }
    }

    public event Action actUpdateCvsEyebrow;

    public bool updateCvsEyebrow
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsEyebrow).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsEyebrow).set_Value(value);
      }
    }

    public event Action actUpdateCvsEyelashes;

    public bool updateCvsEyelashes
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsEyelashes).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsEyelashes).set_Value(value);
      }
    }

    public event Action actUpdateCvsEyeshadow;

    public bool updateCvsEyeshadow
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsEyeshadow).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsEyeshadow).set_Value(value);
      }
    }

    public event Action actUpdateCvsCheek;

    public bool updateCvsCheek
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsCheek).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsCheek).set_Value(value);
      }
    }

    public event Action actUpdateCvsLip;

    public bool updateCvsLip
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsLip).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsLip).set_Value(value);
      }
    }

    public event Action actUpdateCvsFacePaint;

    public bool updateCvsFacePaint
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFacePaint).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFacePaint).set_Value(value);
      }
    }

    public event Action actUpdateCvsBeard;

    public bool updateCvsBeard
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBeard).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBeard).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodyShapeWhole;

    public bool updateCvsBodyShapeWhole
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodyShapeWhole).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodyShapeWhole).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodyShapeBreast;

    public bool updateCvsBodyShapeBreast
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodyShapeBreast).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodyShapeBreast).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodyShapeUpper;

    public bool updateCvsBodyShapeUpper
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodyShapeUpper).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodyShapeUpper).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodyShapeLower;

    public bool updateCvsBodyShapeLower
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodyShapeLower).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodyShapeLower).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodyShapeArm;

    public bool updateCvsBodyShapeArm
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodyShapeArm).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodyShapeArm).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodyShapeLeg;

    public bool updateCvsBodyShapeLeg
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodyShapeLeg).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodyShapeLeg).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodySkinType;

    public bool updateCvsBodySkinType
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodySkinType).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodySkinType).set_Value(value);
      }
    }

    public event Action actUpdateCvsSunburn;

    public bool updateCvsSunburn
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsSunburn).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsSunburn).set_Value(value);
      }
    }

    public event Action actUpdateCvsNip;

    public bool updateCvsNip
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsNip).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsNip).set_Value(value);
      }
    }

    public event Action actUpdateCvsUnderhair;

    public bool updateCvsUnderhair
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsUnderhair).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsUnderhair).set_Value(value);
      }
    }

    public event Action actUpdateCvsNail;

    public bool updateCvsNail
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsNail).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsNail).set_Value(value);
      }
    }

    public event Action actUpdateCvsBodyPaint;

    public bool updateCvsBodyPaint
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsBodyPaint).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsBodyPaint).set_Value(value);
      }
    }

    public event Action actUpdateCvsFutanari;

    public bool updateCvsFutanari
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFutanari).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFutanari).set_Value(value);
      }
    }

    public event Action actUpdateCvsHair;

    public bool updateCvsHair
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsHair).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsHair).set_Value(value);
      }
    }

    public event Action actUpdateCvsClothes;

    public bool updateCvsClothes
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsClothes).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsClothes).set_Value(value);
      }
    }

    public event Action actUpdateCvsClothesSaveDelete;

    public bool updateCvsClothesSaveDelete
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsClothesSaveDelete).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsClothesSaveDelete).set_Value(value);
      }
    }

    public event Action actUpdateCvsClothesLoad;

    public bool updateCvsClothesLoad
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsClothesLoad).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsClothesLoad).set_Value(value);
      }
    }

    public event Action actUpdateCvsAccessory;

    public bool updateCvsAccessory
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsAccessory).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsAccessory).set_Value(value);
      }
    }

    public event Action actUpdateCvsAcsCopy;

    public bool updateCvsAcsCopy
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsAcsCopy).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsAcsCopy).set_Value(value);
      }
    }

    public event Action actUpdateCvsChara;

    public bool updateCvsChara
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsChara).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsChara).set_Value(value);
      }
    }

    public event Action actUpdateCvsType;

    public bool updateCvsType
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsType).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsType).set_Value(value);
      }
    }

    public event Action actUpdateCvsStatus;

    public bool updateCvsStatus
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsStatus).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsStatus).set_Value(value);
      }
    }

    public event Action actUpdateCvsCharaSaveDelete;

    public bool updateCvsCharaSaveDelete
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsCharaSaveDelete).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsCharaSaveDelete).set_Value(value);
      }
    }

    public event Action actUpdateCvsCharaLoad;

    public bool updateCvsCharaLoad
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsCharaLoad).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsCharaLoad).set_Value(value);
      }
    }

    public event Action actUpdateCvsFusion;

    public bool updateCvsFusion
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCvsFusion).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCvsFusion).set_Value(value);
      }
    }

    public void ChangeCharaData()
    {
      this.RestrictSubMenu();
      this.ChangeAcsSlotName(-1);
    }

    public void RestrictSubMenu()
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl) || this.chaCtrl.cmpClothes == null)
        return;
      bool flag1 = true;
      bool flag2 = true;
      ListInfoBase infoClothe1 = this.chaCtrl.infoClothes[0];
      if (infoClothe1 != null)
        flag1 = "0" == infoClothe1.GetInfo(ChaListDefine.KeyType.Coordinate);
      ListInfoBase infoClothe2 = this.chaCtrl.infoClothes[2];
      if (infoClothe2 != null)
        flag2 = "0" == infoClothe2.GetInfo(ChaListDefine.KeyType.Coordinate);
      if (Object.op_Implicit((Object) this.subMenuBot))
        ((Selectable) this.subMenuBot).set_interactable(flag1);
      if (!Object.op_Implicit((Object) this.subMenuInnerDown))
        return;
      ((Selectable) this.subMenuInnerDown).set_interactable(flag2);
    }

    public void ChangeAcsSlotName(int slotNo = -1)
    {
      for (int index = 0; index < this.acsSlotText.Length; ++index)
      {
        if ((slotNo == -1 || index == slotNo) && !Object.op_Equality((Object) null, (Object) this.acsSlotText[index]))
        {
          int type = this.chaCtrl.nowCoordinate.accessory.parts[index].type;
          if (type == 350)
          {
            this.acsSlotText[index].set_text((index + 1).ToString("00"));
          }
          else
          {
            ListInfoBase listInfo = this.chaCtrl.lstCtrl.GetListInfo((ChaListDefine.CategoryNo) type, this.chaCtrl.nowCoordinate.accessory.parts[index].id);
            this.acsSlotText[index].set_text(string.Format("{0:00} {1}", (object) (index + 1), (object) listInfo.Name));
          }
        }
      }
    }

    public void ChangeAcsSlotColor(int slotNo)
    {
      for (int index = 0; index < this.acsSlotText.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.acsSlotText[index]))
        {
          if (index != slotNo)
            ((Graphic) this.acsSlotText[index]).set_color(Color32.op_Implicit(new Color32((byte) 235, (byte) 226, (byte) 215, byte.MaxValue)));
          else
            ((Graphic) this.acsSlotText[index]).set_color(Color32.op_Implicit(new Color32((byte) 204, (byte) 197, (byte) 59, byte.MaxValue)));
        }
      }
    }

    public void ChangeClothesStateAuto(int stateNo)
    {
      this.autoClothesStateNo = (int) (byte) stateNo;
      if (!this.autoClothesState)
        return;
      this.ChangeClothesState(0);
    }

    public void ChangeClothesState(int stateNo)
    {
      byte[,] numArray = new byte[3, 8]
      {
        {
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0
        },
        {
          (byte) 2,
          (byte) 2,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 2
        },
        {
          (byte) 2,
          (byte) 2,
          (byte) 2,
          (byte) 2,
          (byte) 2,
          (byte) 2,
          (byte) 2,
          (byte) 2
        }
      };
      switch (stateNo)
      {
        case -1:
          if (!Object.op_Implicit((Object) this.chaCtrl))
            break;
          int length = Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length;
          for (int clothesKind = 0; clothesKind < length; ++clothesKind)
            this.chaCtrl.SetClothesState(clothesKind, numArray[this.clothesStateNo, clothesKind], true);
          break;
        case 0:
          this.autoClothesState = true;
          this.clothesStateNo = this.autoClothesStateNo;
          goto case -1;
        default:
          this.autoClothesState = false;
          this.clothesStateNo = stateNo - 1;
          goto case -1;
      }
    }

    public void ChangeAnimationNext(int next)
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl))
        return;
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_pose_f : ChaListDefine.CategoryNo.custom_pose_m).Keys.ToArray<int>();
      if (next == 0)
      {
        int num = (this.poseNo + array.Length - 1) % array.Length;
        if (num == 0)
          num = array.Length - 1;
        this.poseNo = num;
      }
      else
      {
        int num = (this.poseNo + 1) % array.Length;
        if (num == 0)
          ++num;
        this.poseNo = num;
      }
    }

    public void ChangeAnimationNo(int no, bool mannequin = false)
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl))
        return;
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_pose_f : ChaListDefine.CategoryNo.custom_pose_m).Keys.ToArray<int>();
      if (!mannequin && no < 1)
        no = 1;
      if (no >= array.Length)
        no = array.Length - 1;
      this.poseNo = no;
    }

    public bool ChangeAnimation()
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl))
        return false;
      Dictionary<int, ListInfoBase> categoryInfo = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_pose_f : ChaListDefine.CategoryNo.custom_pose_m);
      int[] array = categoryInfo.Keys.ToArray<int>();
      if (this.poseNo >= array.Length || this.poseNo < 0)
        return false;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      ListInfoBase listInfoBase1;
      if (!categoryInfo.TryGetValue(array[this.poseNo], out listInfoBase1))
        return false;
      string info1 = listInfoBase1.GetInfo(ChaListDefine.KeyType.MainManifest);
      string info2 = listInfoBase1.GetInfo(ChaListDefine.KeyType.MainAB);
      string info3 = listInfoBase1.GetInfo(ChaListDefine.KeyType.MainData);
      string info4 = listInfoBase1.GetInfo(ChaListDefine.KeyType.Clip);
      string info5 = listInfoBase1.GetInfo(ChaListDefine.KeyType.IKAB);
      string info6 = listInfoBase1.GetInfo(ChaListDefine.KeyType.IKData);
      bool flag = true;
      ListInfoBase listInfoBase2;
      if (0 <= this.backPoseNo && categoryInfo.TryGetValue(array[this.poseNo], out listInfoBase2) && (listInfoBase2.GetInfo(ChaListDefine.KeyType.MainManifest) == info1 && listInfoBase2.GetInfo(ChaListDefine.KeyType.MainAB) == info2) && listInfoBase2.GetInfo(ChaListDefine.KeyType.MainData) == info3)
        flag = false;
      if (flag)
        this.chaCtrl.LoadAnimation(info2, info3, info1);
      if (this.customMotionIK != null)
        this.customMotionIK.LoadData(info5, info6, false);
      if (0.0 > (double) this.animationPos)
        this.chaCtrl.AnimPlay(info4);
      else
        this.chaCtrl.syncPlay(info4, 0, this.animationPos);
      this.animationPos = -1f;
      if (this.customMotionIK != null)
        this.customMotionIK.Calc(info4);
      this.chaCtrl.resetDynamicBoneAll = true;
      this.animeStateName = info4;
      this.backPoseNo = this.poseNo;
      return true;
    }

    public void ChangeEyebrowPtnNext(int next)
    {
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_eyebrow_f : ChaListDefine.CategoryNo.custom_eyebrow_m).Keys.ToArray<int>();
      switch (next)
      {
        case -1:
          this.eyebrowPtn = 0;
          break;
        case 0:
          this.eyebrowPtn = (this.eyebrowPtn + array.Length - 1) % array.Length;
          break;
        default:
          this.eyebrowPtn = (this.eyebrowPtn + 1) % array.Length;
          break;
      }
      this.chaCtrl.ChangeEyebrowPtn(array[this.eyebrowPtn], true);
    }

    public void ChangeEyebrowPtnNo(int no)
    {
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_eyebrow_f : ChaListDefine.CategoryNo.custom_eyebrow_m).Keys.ToArray<int>();
      this.eyebrowPtn = Mathf.Clamp(no - 1, 0, array.Length - 1);
      this.chaCtrl.ChangeEyebrowPtn(array[this.eyebrowPtn], true);
    }

    public void ChangeEyePtnNext(int next)
    {
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_eye_f : ChaListDefine.CategoryNo.custom_eye_m).Keys.ToArray<int>();
      switch (next)
      {
        case -1:
          this.eyePtn = 0;
          break;
        case 0:
          this.eyePtn = (this.eyePtn + array.Length - 1) % array.Length;
          break;
        default:
          this.eyePtn = (this.eyePtn + 1) % array.Length;
          break;
      }
      this.chaCtrl.ChangeEyesPtn(array[this.eyePtn], true);
    }

    public void ChangeEyePtnNo(int no)
    {
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_eye_f : ChaListDefine.CategoryNo.custom_eye_m).Keys.ToArray<int>();
      this.eyePtn = Mathf.Clamp(no - 1, 0, array.Length - 1);
      this.chaCtrl.ChangeEyesPtn(array[this.eyePtn], true);
    }

    public void ChangeMouthPtnNext(int next)
    {
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_mouth_f : ChaListDefine.CategoryNo.custom_mouth_m).Keys.ToArray<int>();
      switch (next)
      {
        case -1:
          this.mouthPtn = 0;
          break;
        case 0:
          this.mouthPtn = (this.mouthPtn + array.Length - 1) % array.Length;
          break;
        default:
          this.mouthPtn = (this.mouthPtn + 1) % array.Length;
          break;
      }
      this.chaCtrl.ChangeMouthPtn(array[this.mouthPtn], true);
    }

    public void ChangeMouthPtnNo(int no)
    {
      int[] array = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_mouth_f : ChaListDefine.CategoryNo.custom_mouth_m).Keys.ToArray<int>();
      this.mouthPtn = Mathf.Clamp(no - 1, 0, array.Length - 1);
      this.chaCtrl.ChangeMouthPtn(array[this.mouthPtn], true);
    }

    public void UpdateIKCalc()
    {
      if (this.customMotionIK == null)
        return;
      this.customMotionIK.Calc(this.animeStateName);
    }

    public bool IsInputFocused()
    {
      using (List<InputField>.Enumerator enumerator = this.lstInputField.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          InputField current = enumerator.Current;
          if (!Object.op_Equality((Object) null, (Object) current) && current.get_isFocused())
            return true;
        }
      }
      return false;
    }

    public static string ConvertTextFromRate(int min, int max, float value)
    {
      return Mathf.RoundToInt(Mathf.Lerp((float) min, (float) max, value)).ToString();
    }

    public static float ConvertRateFromText(int min, int max, string buf)
    {
      int result;
      return buf.IsNullOrEmpty() || !int.TryParse(buf, out result) ? 0.0f : Mathf.InverseLerp((float) min, (float) max, (float) result);
    }

    public static float ConvertValueFromTextLimit(float min, float max, int digit, string buf)
    {
      if (buf.IsNullOrEmpty() || !MathfEx.RangeEqualOn<int>(0, digit, 4))
        return 0.0f;
      float result = 0.0f;
      float.TryParse(buf, out result);
      string[] strArray = new string[5]
      {
        "f0",
        "f1",
        "f2",
        "f3",
        "f4"
      };
      return Mathf.Clamp(float.Parse(result.ToString(strArray[digit])), min, max);
    }

    public void SetUpdateToggleSetting()
    {
      if (Object.op_Implicit((Object) this.tglEyesSameSetting))
        this.tglEyesSameSetting.SetIsOnWithoutCallback(this.chaCtrl.fileFace.pupilSameSetting);
      if (Object.op_Implicit((Object) this.tglHairSameSetting))
        this.tglHairSameSetting.SetIsOnWithoutCallback(this.chaCtrl.fileHair.sameSetting);
      if (Object.op_Implicit((Object) this.tglHairAutoSetting))
        this.tglHairAutoSetting.SetIsOnWithoutCallback(this.chaCtrl.fileHair.autoSetting);
      this.autoHairColor = this.chaCtrl.fileHair.autoSetting;
      if (!Object.op_Implicit((Object) this.tglHairControlTogether))
        return;
      this.tglHairControlTogether.SetIsOnWithoutCallback(this.chaCtrl.fileHair.ctrlTogether);
    }

    public void ResetLightSetting()
    {
      ((Component) this.lightCustom).get_transform().set_localEulerAngles(new Vector3(8f, -20f, 0.0f));
      this.lightCustom.set_color(new Color(0.951f, 0.906f, 0.876f));
      this.lightCustom.set_intensity(1f);
    }

    protected override void Awake()
    {
      this.lstInputField.Clear();
      this.customSettingSave.Load();
    }

    private void Start()
    {
      this.sliderControlWheel = this.customSettingSave.sliderWheel;
      this.tglSliderWheel.SetIsOnWithoutCallback(this.customSettingSave.sliderWheel);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._drawSaveFrameTop, (Action<M0>) (draw =>
      {
        if (!Object.op_Inequality((Object) null, (Object) this.saveFrameAssist))
          return;
        this.saveFrameAssist.SetActiveSaveFrameTop(draw);
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._forceBackFrameHide, (Action<M0>) (hide =>
      {
        if (!Object.op_Inequality((Object) null, (Object) this.saveFrameAssist))
          return;
        this.saveFrameAssist.forceBackFrameHide = hide;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._drawSaveFrameBack, (Action<M0>) (draw =>
      {
        if (!Object.op_Inequality((Object) null, (Object) this.saveFrameAssist))
          return;
        this.saveFrameAssist.backFrameDraw = draw;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._drawSaveFrameFront, (Action<M0>) (draw =>
      {
        if (!Object.op_Inequality((Object) null, (Object) this.saveFrameAssist))
          return;
        this.saveFrameAssist.frontFrameDraw = draw;
      }));
      this.SetUpdateToggleSetting();
      if (Object.op_Implicit((Object) this.tglEyesSameSetting))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglEyesSameSetting), (Action<M0>) (isOn =>
        {
          if (!Object.op_Implicit((Object) this.chaCtrl))
            return;
          this.chaCtrl.fileFace.pupilSameSetting = isOn;
        }));
      if (Object.op_Implicit((Object) this.tglHairSameSetting))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglHairSameSetting), (Action<M0>) (isOn =>
        {
          if (!Object.op_Implicit((Object) this.chaCtrl))
            return;
          this.chaCtrl.fileHair.sameSetting = isOn;
        }));
      if (Object.op_Implicit((Object) this.tglHairAutoSetting))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglHairAutoSetting), (Action<M0>) (isOn =>
        {
          if (Object.op_Implicit((Object) this.chaCtrl))
            this.chaCtrl.fileHair.autoSetting = isOn;
          this.autoHairColor = isOn;
        }));
      if (Object.op_Implicit((Object) this.tglHairControlTogether))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglHairControlTogether), (Action<M0>) (isOn =>
        {
          if (!Object.op_Implicit((Object) this.chaCtrl))
            return;
          this.chaCtrl.fileHair.ctrlTogether = isOn;
        }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._changeCharaName, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.customCtrl.UpdateCharaNameText();
        this.changeCharaName = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceType, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceType.Call();
        this.updateCvsFaceType = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeWhole, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeWhole.Call();
        this.updateCvsFaceShapeWhole = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeChin, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeChin.Call();
        this.updateCvsFaceShapeChin = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeCheek, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeCheek.Call();
        this.updateCvsFaceShapeCheek = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeEyebrow, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeEyebrow.Call();
        this.updateCvsFaceShapeEyebrow = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeEyes, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeEyes.Call();
        this.updateCvsFaceShapeEyes = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeNose, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeNose.Call();
        this.updateCvsFaceShapeNose = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeMouth, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeMouth.Call();
        this.updateCvsFaceShapeMouth = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFaceShapeEar, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFaceShapeEar.Call();
        this.updateCvsFaceShapeEar = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsMole, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsMole.Call();
        this.updateCvsMole = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsEyeLR, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsEyeLR.Call();
        this.updateCvsEyeLR = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsEyeEtc, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsEyeEtc.Call();
        this.updateCvsEyeEtc = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsEyeHL, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsEyeHL.Call();
        this.updateCvsEyeHL = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsEyebrow, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsEyebrow.Call();
        this.updateCvsEyebrow = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsEyelashes, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsEyelashes.Call();
        this.updateCvsEyelashes = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsEyeshadow, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsEyeshadow.Call();
        this.updateCvsEyeshadow = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsCheek, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsCheek.Call();
        this.updateCvsCheek = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsLip, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsLip.Call();
        this.updateCvsLip = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFacePaint, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFacePaint.Call();
        this.updateCvsFacePaint = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBeard, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBeard.Call();
        this.updateCvsBeard = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodyShapeWhole, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodyShapeWhole.Call();
        this.updateCvsBodyShapeWhole = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodyShapeBreast, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodyShapeBreast.Call();
        this.updateCvsBodyShapeBreast = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodyShapeUpper, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodyShapeUpper.Call();
        this.updateCvsBodyShapeUpper = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodyShapeLower, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodyShapeLower.Call();
        this.updateCvsBodyShapeLower = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodyShapeArm, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodyShapeArm.Call();
        this.updateCvsBodyShapeArm = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodyShapeLeg, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodyShapeLeg.Call();
        this.updateCvsBodyShapeLeg = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodySkinType, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodySkinType.Call();
        this.updateCvsBodySkinType = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsSunburn, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsSunburn.Call();
        this.updateCvsSunburn = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsNip, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsNip.Call();
        this.updateCvsNip = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsUnderhair, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsUnderhair.Call();
        this.updateCvsUnderhair = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsNail, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsNail.Call();
        this.updateCvsNail = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsBodyPaint, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsBodyPaint.Call();
        this.updateCvsBodyPaint = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsFutanari, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsFutanari.Call();
        this.updateCvsFutanari = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsHair, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsHair.Call();
        this.updateCvsHair = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsClothes, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsClothes.Call();
        this.updateCvsClothes = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsClothesSaveDelete, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsClothesSaveDelete.Call();
        this.updateCvsClothesSaveDelete = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsClothesLoad, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsClothesLoad.Call();
        this.updateCvsClothesLoad = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsAccessory, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsAccessory.Call();
        this.updateCvsAccessory = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsAcsCopy, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsAcsCopy.Call();
        this.updateCvsAcsCopy = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsChara, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsChara.Call();
        this.updateCvsChara = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsType, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsType.Call();
        this.updateCvsType = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsStatus, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsStatus.Call();
        this.updateCvsStatus = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsCharaSaveDelete, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsCharaSaveDelete.Call();
        this.updateCvsCharaSaveDelete = false;
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._updateCvsCharaLoad, (Func<M0, bool>) (f => f)), (Action<M0>) (f =>
      {
        this.actUpdateCvsCharaLoad.Call();
        this.updateCvsCharaLoad = false;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._accessoryDraw, (Action<M0>) (f => this.chaCtrl.SetAccessoryStateAll(f)));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._poseNo, (Action<M0>) (_ => this.ChangeAnimation()));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._eyelook, (Action<M0>) (v => this.chaCtrl.ChangeLookEyesPtn(v != 0 ? 0 : 1)));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._necklook, (Action<M0>) (v => this.chaCtrl.ChangeLookNeckPtn(v != 0 ? 3 : 1, 1f)));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._sliderControlWheel, (Action<M0>) (f =>
      {
        if (this.customCtrl.sliderScrollRaycast == null)
          return;
        this.customCtrl.sliderScrollRaycast.ChangeActiveRaycast(!f);
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglSliderWheel), (Action<M0>) (isOn =>
      {
        this.customSettingSave.sliderWheel = isOn;
        this.sliderControlWheel = isOn;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._centerDraw, (Action<M0>) (f => this.customCtrl.camCtrl.isOutsideTargetTex = f));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._autoHairColor, (Action<M0>) (f =>
      {
        if (Object.op_Implicit((Object) this.hairUICondition.objTopColorSet))
          this.hairUICondition.objTopColorSet.SetActiveIfDifferent(!f && this.drawTopHairColor);
        if (Object.op_Implicit((Object) this.hairUICondition.objUnderColorSet))
          this.hairUICondition.objUnderColorSet.SetActiveIfDifferent(!f && this.drawUnderHairColor);
        if (!Object.op_Implicit((Object) this.hairUICondition.objGlossColorSet))
          return;
        this.hairUICondition.objGlossColorSet.SetActiveIfDifferent(!f);
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._drawTopHairColor, (Action<M0>) (f =>
      {
        if (!Object.op_Implicit((Object) this.hairUICondition.objTopColorSet))
          return;
        this.hairUICondition.objTopColorSet.SetActiveIfDifferent(!this.autoHairColor && f);
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._drawUnderHairColor, (Action<M0>) (f =>
      {
        if (!Object.op_Implicit((Object) this.hairUICondition.objUnderColorSet))
          return;
        this.hairUICondition.objUnderColorSet.SetActiveIfDifferent(!this.autoHairColor && f);
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._cursorDraw, (Action<M0>) (f => Cursor.set_visible(f)));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._playPoseAnime, (Action<M0>) (f => this.chaCtrl.animBody.set_speed(!f ? 0.0f : 1f)));
    }

    public void Update()
    {
      this.lstShow.Clear();
      this.lstShow.Add(this.showAcsControllerAll);
      this.lstShow.Add(this.showAcsController[0]);
      this.lstShow.Add(this.customSettingSave.acsCtrlSetting.correctSetting[0].draw);
      bool active1 = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.objAcs01ControllerTop))
        this.objAcs01ControllerTop.SetActiveIfDifferent(active1);
      this.lstShow.Clear();
      this.lstShow.Add(this.showAcsControllerAll);
      this.lstShow.Add(this.showAcsController[1]);
      this.lstShow.Add(this.customSettingSave.acsCtrlSetting.correctSetting[1].draw);
      bool active2 = YS_Assist.CheckFlagsList(this.lstShow);
      if (Object.op_Implicit((Object) this.objAcs02ControllerTop))
        this.objAcs02ControllerTop.SetActiveIfDifferent(active2);
      this.lstShow.Clear();
      this.lstShow.Add(this.showHairController);
      this.lstShow.Add(this.customSettingSave.hairCtrlSetting.drawController);
      bool active3 = YS_Assist.CheckFlagsList(this.lstShow);
      if (!Object.op_Implicit((Object) this.objHairControllerTop))
        return;
      this.objHairControllerTop.SetActiveIfDifferent(active3);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.customSettingSave.Save();
    }

    public class CustomSettingSave
    {
      public Version version = CharaCustomDefine.CustomSettingVersion;
      public Color backColor = Color.get_gray();
      public bool bgmOn = true;
      public bool sliderWheel = true;
      public bool centerDraw = true;
      public float bgmVol = 0.3f;
      public float seVol = 0.5f;
      public CustomBase.CustomSettingSave.HairCtrlSetting hairCtrlSetting = new CustomBase.CustomSettingSave.HairCtrlSetting();
      public CustomBase.CustomSettingSave.AcsCtrlSetting acsCtrlSetting = new CustomBase.CustomSettingSave.AcsCtrlSetting();
      public Vector2 winSubLayout = new Vector2(1444f, -8f);
      public Vector2 winDrawLayout = new Vector2(1536f, -568f);
      public Vector2 winColorLayout = new Vector2(1536f, -768f);
      public Vector2 winPatternLayout = new Vector2(1176f, -8f);

      public void ResetWinLayout()
      {
        this.winSubLayout = new Vector2(1444f, -8f);
        this.winDrawLayout = new Vector2(1536f, -568f);
        this.winColorLayout = new Vector2(1536f, -768f);
        this.winPatternLayout = new Vector2(1176f, -8f);
      }

      public void Save()
      {
        string path = UserData.Path + "custom/customscene.dat";
        string directoryName = Path.GetDirectoryName(path);
        if (!System.IO.Directory.Exists(directoryName))
          System.IO.Directory.CreateDirectory(directoryName);
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
          using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
          {
            binaryWriter.Write(CharaCustomDefine.CustomSettingVersion.ToString());
            binaryWriter.Write((float) this.backColor.r);
            binaryWriter.Write((float) this.backColor.g);
            binaryWriter.Write((float) this.backColor.b);
            binaryWriter.Write(this.bgmOn);
            binaryWriter.Write(this.hairCtrlSetting.drawController);
            binaryWriter.Write(this.hairCtrlSetting.controllerType);
            for (int index = 0; index < 2; ++index)
            {
              binaryWriter.Write(this.acsCtrlSetting.correctSetting[index].posRate);
              binaryWriter.Write(this.acsCtrlSetting.correctSetting[index].rotRate);
              binaryWriter.Write(this.acsCtrlSetting.correctSetting[index].sclRate);
              binaryWriter.Write(this.acsCtrlSetting.correctSetting[index].draw);
              binaryWriter.Write(this.acsCtrlSetting.correctSetting[index].type);
              binaryWriter.Write(this.acsCtrlSetting.correctSetting[index].speed);
              binaryWriter.Write(this.acsCtrlSetting.correctSetting[index].scale);
            }
            binaryWriter.Write(this.sliderWheel);
            binaryWriter.Write(this.centerDraw);
            binaryWriter.Write(this.bgmVol);
            binaryWriter.Write(this.seVol);
            binaryWriter.Write((float) this.winSubLayout.x);
            binaryWriter.Write((float) this.winSubLayout.y);
            binaryWriter.Write((float) this.winDrawLayout.x);
            binaryWriter.Write((float) this.winDrawLayout.y);
            binaryWriter.Write((float) this.winColorLayout.x);
            binaryWriter.Write((float) this.winColorLayout.y);
            binaryWriter.Write((float) this.winPatternLayout.x);
            binaryWriter.Write((float) this.winPatternLayout.y);
            binaryWriter.Write(this.hairCtrlSetting.controllerSpeed);
            binaryWriter.Write(this.hairCtrlSetting.controllerScale);
          }
        }
      }

      public void Load()
      {
        string path = UserData.Path + "custom/customscene.dat";
        if (!File.Exists(path))
          return;
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
          using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
          {
            this.version = new Version(binaryReader.ReadString());
            this.backColor.r = (__Null) (double) binaryReader.ReadSingle();
            this.backColor.g = (__Null) (double) binaryReader.ReadSingle();
            this.backColor.b = (__Null) (double) binaryReader.ReadSingle();
            this.bgmOn = binaryReader.ReadBoolean();
            this.hairCtrlSetting.drawController = binaryReader.ReadBoolean();
            this.hairCtrlSetting.controllerType = binaryReader.ReadInt32();
            for (int index = 0; index < 2; ++index)
            {
              this.acsCtrlSetting.correctSetting[index].posRate = binaryReader.ReadInt32();
              this.acsCtrlSetting.correctSetting[index].rotRate = binaryReader.ReadInt32();
              this.acsCtrlSetting.correctSetting[index].sclRate = binaryReader.ReadInt32();
              this.acsCtrlSetting.correctSetting[index].draw = binaryReader.ReadBoolean();
              this.acsCtrlSetting.correctSetting[index].type = binaryReader.ReadInt32();
              this.acsCtrlSetting.correctSetting[index].speed = binaryReader.ReadSingle();
              this.acsCtrlSetting.correctSetting[index].scale = binaryReader.ReadSingle();
            }
            if (this.version < new Version("0.0.1"))
              return;
            this.sliderWheel = binaryReader.ReadBoolean();
            this.centerDraw = binaryReader.ReadBoolean();
            this.bgmVol = binaryReader.ReadSingle();
            this.seVol = binaryReader.ReadSingle();
            if (this.version < new Version("0.0.2"))
              return;
            this.winSubLayout.x = (__Null) (double) binaryReader.ReadSingle();
            this.winSubLayout.y = (__Null) (double) binaryReader.ReadSingle();
            this.winDrawLayout.x = (__Null) (double) binaryReader.ReadSingle();
            this.winDrawLayout.y = (__Null) (double) binaryReader.ReadSingle();
            this.winColorLayout.x = (__Null) (double) binaryReader.ReadSingle();
            this.winColorLayout.y = (__Null) (double) binaryReader.ReadSingle();
            this.winPatternLayout.x = (__Null) (double) binaryReader.ReadSingle();
            this.winPatternLayout.y = (__Null) (double) binaryReader.ReadSingle();
            if (this.version < new Version("0.0.3"))
              return;
            this.hairCtrlSetting.controllerSpeed = binaryReader.ReadSingle();
            this.hairCtrlSetting.controllerScale = binaryReader.ReadSingle();
          }
        }
      }

      public class HairCtrlSetting
      {
        public float controllerSpeed = 0.3f;
        public float controllerScale = 0.4f;
        public bool drawController;
        public int controllerType;
      }

      public class AcsCtrlSetting
      {
        public CustomBase.CustomSettingSave.AcsCtrlSetting.CorrectSetting[] correctSetting = new CustomBase.CustomSettingSave.AcsCtrlSetting.CorrectSetting[2];

        public AcsCtrlSetting()
        {
          for (int index = 0; index < this.correctSetting.Length; ++index)
            this.correctSetting[index] = new CustomBase.CustomSettingSave.AcsCtrlSetting.CorrectSetting();
        }

        public class CorrectSetting
        {
          public float speed = 0.3f;
          public float scale = 1.5f;
          public int posRate;
          public int rotRate;
          public int sclRate;
          public bool draw;
          public int type;
        }
      }
    }

    public class PlayVoiceBackup
    {
      public bool backBlink = true;
      public float backEyesOpen = 1f;
      public bool backMouthFix = true;
      public bool playSampleVoice;
      public int backEyebrowPtn;
      public int backEyesPtn;
      public int backMouthPtn;
      public float backMouthOpen;
    }

    [Serializable]
    public class HairUICondition
    {
      public GameObject objTopColorSet;
      public GameObject objUnderColorSet;
      public GameObject objGlossColorSet;
    }
  }
}
