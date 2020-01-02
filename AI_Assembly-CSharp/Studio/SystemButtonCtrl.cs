// Decompiled with JetBrains decompiler
// Type: Studio.SystemButtonCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace Studio
{
  public class SystemButtonCtrl : MonoBehaviour
  {
    [SerializeField]
    private SystemButtonCtrl.CommonInfo[] commonInfo;
    private int select;
    [SerializeField]
    private Sprite spriteSave;
    [SerializeField]
    private Sprite spriteInit;
    [SerializeField]
    [Header("一括制御")]
    private PostProcessVolume postProcessVolume;
    [SerializeField]
    [Header("ColorGrading用")]
    private PostProcessVolume postProcessVolumeColor;
    [SerializeField]
    [Header("Reflection Probe制御")]
    private GameObject objReflectionProbe;
    [SerializeField]
    private ReflectionProbe reflectionProbe;
    [SerializeField]
    [Header("個別制御")]
    private DepthOfField depthOfField;
    [SerializeField]
    private GlobalFog globalFog;
    [SerializeField]
    private SunShafts _sunShafts;
    [SerializeField]
    private Sprite[] spriteExpansion;
    [SerializeField]
    private SystemButtonCtrl.ColorGradingInfo colorGradingInfo;
    [SerializeField]
    private SystemButtonCtrl.AmbientOcclusionInfo ambientOcclusionInfo;
    [SerializeField]
    private SystemButtonCtrl.BloomInfo bloomInfo;
    [SerializeField]
    private SystemButtonCtrl.DOFInfo dofInfo;
    [SerializeField]
    private SystemButtonCtrl.VignetteInfo vignetteInfo;
    [SerializeField]
    private SystemButtonCtrl.ScreenSpaceReflectionInfo screenSpaceReflectionInfo;
    [SerializeField]
    private SystemButtonCtrl.ReflectionProbeInfo reflectionProbeInfo;
    [SerializeField]
    private SystemButtonCtrl.FogInfo fogInfo;
    [SerializeField]
    private SystemButtonCtrl.SunShaftsInfo sunShaftsInfo;
    [SerializeField]
    private SystemButtonCtrl.SelfShadowInfo selfShadowInfo;
    [SerializeField]
    private SystemButtonCtrl.EnvironmentLightingInfo environmentLightingInfo;
    private SystemButtonCtrl.EffectInfo[] effectInfos;
    private bool isInit;

    public SystemButtonCtrl()
    {
      base.\u002Ector();
    }

    public SunShafts sunShafts
    {
      get
      {
        return this._sunShafts;
      }
    }

    public bool visible
    {
      set
      {
        if (value)
        {
          this.commonInfo.SafeProc<SystemButtonCtrl.CommonInfo>(this.select, (Action<SystemButtonCtrl.CommonInfo>) (v => v.active = true));
        }
        else
        {
          for (int index = 0; index < this.commonInfo.Length; ++index)
            this.commonInfo[index].active = false;
        }
      }
    }

    public void OnClickSelect(int _idx)
    {
      if (MathfEx.RangeEqualOn<int>(0, this.select, this.commonInfo.Length - 1))
      {
        this.commonInfo[this.select].active = false;
        if (this.select == 2)
          Singleton<Studio.Studio>.Instance.SaveOption();
      }
      this.select = this.select != _idx ? _idx : -1;
      if (MathfEx.RangeEqualOn<int>(0, this.select, this.commonInfo.Length - 1))
        this.commonInfo[this.select].active = true;
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
    }

    public void OnClickSave()
    {
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      Singleton<Studio.Studio>.Instance.SaveScene();
      NotificationScene.spriteMessage = this.spriteSave;
      NotificationScene.waitTime = 1f;
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "StudioNotification",
        isAdd = true
      }, false);
    }

    public void OnClickLoad()
    {
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "StudioSceneLoad",
        isAdd = true
      }, false);
    }

    public void OnClickInit()
    {
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      CheckScene.sprite = this.spriteInit;
      // ISSUE: method pointer
      CheckScene.unityActionYes = new UnityAction((object) this, __methodptr(OnSelectInitYes));
      // ISSUE: method pointer
      CheckScene.unityActionNo = new UnityAction((object) this, __methodptr(OnSelectIniteNo));
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "StudioCheck",
        isAdd = true
      }, false);
    }

    private void OnSelectInitYes()
    {
      Singleton<Scene>.Instance.UnLoad();
      Singleton<Studio.Studio>.Instance.InitScene(true);
    }

    private void OnSelectIniteNo()
    {
      Singleton<Scene>.Instance.UnLoad();
    }

    public void OnClickEnd()
    {
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      Singleton<Scene>.Instance.GameEnd(true);
    }

    public void Init()
    {
      if (this.isInit)
        return;
      GameObject gameObject = ((Component) Camera.get_main())?.get_gameObject();
      if (Object.op_Equality((Object) this.depthOfField, (Object) null))
        this.depthOfField = (DepthOfField) gameObject?.GetComponent<DepthOfField>();
      if (Object.op_Equality((Object) this.globalFog, (Object) null))
        this.globalFog = (GlobalFog) gameObject?.GetComponent<GlobalFog>();
      if (Object.op_Equality((Object) this._sunShafts, (Object) null))
        this._sunShafts = (SunShafts) gameObject?.GetComponent<SunShafts>();
      for (int index = 0; index < this.commonInfo.Length; ++index)
        this.commonInfo[index].active = false;
      this.colorGradingInfo.Init(this.spriteExpansion, (ColorGrading) this.postProcessVolume.get_profile().GetSetting<ColorGrading>(), this.postProcessVolumeColor);
      this.ambientOcclusionInfo.Init(this.spriteExpansion, (AmbientOcclusion) this.postProcessVolume.get_profile().GetSetting<AmbientOcclusion>());
      this.bloomInfo.Init(this.spriteExpansion, (Bloom) this.postProcessVolume.get_profile().GetSetting<Bloom>());
      this.dofInfo.Init(this.spriteExpansion, this.depthOfField);
      this.vignetteInfo.Init(this.spriteExpansion, (Vignette) this.postProcessVolume.get_profile().GetSetting<Vignette>());
      this.screenSpaceReflectionInfo.Init(this.spriteExpansion, (ScreenSpaceReflections) this.postProcessVolume.get_profile().GetSetting<ScreenSpaceReflections>());
      this.reflectionProbeInfo.Init(this.spriteExpansion, this.reflectionProbe, this.objReflectionProbe);
      this.fogInfo.Init(this.spriteExpansion, this.globalFog);
      this.sunShaftsInfo.Init(this.spriteExpansion, this.sunShafts);
      this.selfShadowInfo.Init(this.spriteExpansion);
      this.environmentLightingInfo.Init(this.spriteExpansion);
      this.isInit = true;
      this.effectInfos = new SystemButtonCtrl.EffectInfo[11]
      {
        (SystemButtonCtrl.EffectInfo) this.colorGradingInfo,
        (SystemButtonCtrl.EffectInfo) this.ambientOcclusionInfo,
        (SystemButtonCtrl.EffectInfo) this.bloomInfo,
        (SystemButtonCtrl.EffectInfo) this.dofInfo,
        (SystemButtonCtrl.EffectInfo) this.vignetteInfo,
        (SystemButtonCtrl.EffectInfo) this.screenSpaceReflectionInfo,
        (SystemButtonCtrl.EffectInfo) this.reflectionProbeInfo,
        (SystemButtonCtrl.EffectInfo) this.fogInfo,
        (SystemButtonCtrl.EffectInfo) this.sunShaftsInfo,
        (SystemButtonCtrl.EffectInfo) this.selfShadowInfo,
        (SystemButtonCtrl.EffectInfo) this.environmentLightingInfo
      };
      this.UpdateInfo();
    }

    public void UpdateInfo()
    {
      foreach (SystemButtonCtrl.EffectInfo effectInfo in this.effectInfos)
        effectInfo.UpdateInfo();
    }

    public void Apply()
    {
      foreach (SystemButtonCtrl.EffectInfo effectInfo in this.effectInfos)
        effectInfo.Apply();
    }

    public void SetDepthOfFieldForcus(int _key)
    {
      Transform transform = Singleton<Studio.Studio>.Instance.cameraCtrl.targetObj;
      string str = "注視点";
      ObjectCtrlInfo ctrlInfo = Studio.Studio.GetCtrlInfo(_key);
      if (ctrlInfo == null || ctrlInfo.kind != 1)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.depthForcus = -1;
      }
      else
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.depthForcus = _key;
        transform = (ctrlInfo as OCIItem).objectItem.get_transform();
        str = (ctrlInfo as OCIItem).treeNodeObject.textName;
      }
      this.depthOfField.focalTransform = (__Null) transform;
      this.dofInfo.selectorForcus.text = str;
    }

    public void SetSunCaster(int _key)
    {
      Transform transform = (Transform) null;
      string str = "なし";
      ObjectCtrlInfo ctrlInfo = Studio.Studio.GetCtrlInfo(_key);
      if (ctrlInfo == null || ctrlInfo.kind != 1)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.sunCaster = -1;
      }
      else
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.sunCaster = _key;
        transform = (ctrlInfo as OCIItem).objectItem.get_transform();
        str = (ctrlInfo as OCIItem).treeNodeObject.textName;
      }
      this.sunShafts.sunTransform = (__Null) transform;
      this.sunShaftsInfo.selectorCaster.text = str;
    }

    public void MapDependent()
    {
      this.fogInfo.SetEnable(Singleton<Studio.Studio>.Instance.sceneInfo.enableFog, true);
    }

    [Serializable]
    private class CommonInfo
    {
      public CanvasGroup group;
      public Button button;

      public bool active
      {
        set
        {
          this.group.Enable(value, false);
          ((Graphic) ((Selectable) this.button).get_image()).set_color(!value ? Color.get_white() : Color.get_green());
        }
      }
    }

    [Serializable]
    private class Selector
    {
      public Button _button;
      public TextMeshProUGUI _text;

      public string text
      {
        get
        {
          return ((TMP_Text) this._text).get_text();
        }
        set
        {
          ((TMP_Text) this._text).set_text(value);
        }
      }
    }

    [Serializable]
    private class InputCombination
    {
      public Slider slider;
      public InputField input;
      public Button buttonDefault;

      public bool interactable
      {
        set
        {
          ((Selectable) this.input).set_interactable(value);
          ((Selectable) this.slider).set_interactable(value);
          if (!Object.op_Implicit((Object) this.buttonDefault))
            return;
          ((Selectable) this.buttonDefault).set_interactable(value);
        }
      }

      public string text
      {
        get
        {
          return this.input.get_text();
        }
        set
        {
          this.input.set_text(value);
          this.slider.set_value(Utility.StringToFloat(value));
        }
      }

      public float value
      {
        get
        {
          return this.slider.get_value();
        }
        set
        {
          this.slider.set_value(value);
          this.input.set_text(value.ToString());
        }
      }

      public int IntValue
      {
        set
        {
          this.slider.set_value((float) value);
          this.input.set_text(value.ToString());
        }
      }

      public float Min
      {
        get
        {
          return this.slider.get_minValue();
        }
      }

      public float Max
      {
        get
        {
          return this.slider.get_maxValue();
        }
      }
    }

    [Serializable]
    private class EffectInfo
    {
      public GameObject obj;
      public Button button;
      public Sprite[] sprite;

      public bool active
      {
        get
        {
          return this.obj.get_activeSelf();
        }
        set
        {
          if (!this.obj.SetActiveIfDifferent(value))
            return;
          ((Selectable) this.button).get_image().set_sprite(this.sprite[!value ? 0 : 1]);
        }
      }

      public bool isUpdateInfo { get; set; }

      public virtual void Init(Sprite[] _sprite)
      {
        // ISSUE: method pointer
        ((UnityEvent) this.button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickActive)));
        this.sprite = _sprite;
        this.isUpdateInfo = false;
      }

      public virtual void UpdateInfo()
      {
      }

      public virtual void Apply()
      {
      }

      private void OnClickActive()
      {
        this.active = !this.active;
      }
    }

    [Serializable]
    private class ColorGradingInfo : SystemButtonCtrl.EffectInfo
    {
      public Dropdown dropdownLookupTexture;
      public SystemButtonCtrl.InputCombination icBlend;
      public SystemButtonCtrl.InputCombination icSaturation;
      public SystemButtonCtrl.InputCombination icBrightness;
      public SystemButtonCtrl.InputCombination icContrast;

      private ColorGrading ColorGrading { get; set; }

      private PostProcessVolume PostProcessVolumeBlend { get; set; }

      private ColorGrading ColorGradingBlend { get; set; }

      private float Blend
      {
        set
        {
          this.PostProcessVolumeBlend.weight = (__Null) (double) Mathf.Clamp(value, 0.0f, 1f);
        }
      }

      private float Saturation
      {
        set
        {
          this.ColorGrading.SafeProc<ColorGrading>((Action<ColorGrading>) (_cg => ((ParameterOverride<float>) _cg.saturation).value = (__Null) (double) value));
          this.ColorGradingBlend.SafeProc<ColorGrading>((Action<ColorGrading>) (_cg => ((ParameterOverride<float>) _cg.saturation).value = (__Null) (double) value));
        }
      }

      private float Brightness
      {
        set
        {
          this.ColorGrading.SafeProc<ColorGrading>((Action<ColorGrading>) (_cg => ((ParameterOverride<float>) _cg.brightness).value = (__Null) (double) value));
          this.ColorGradingBlend.SafeProc<ColorGrading>((Action<ColorGrading>) (_cg => ((ParameterOverride<float>) _cg.brightness).value = (__Null) (double) value));
        }
      }

      private float Contrast
      {
        set
        {
          this.ColorGrading.SafeProc<ColorGrading>((Action<ColorGrading>) (_cg => ((ParameterOverride<float>) _cg.contrast).value = (__Null) (double) value));
          this.ColorGradingBlend.SafeProc<ColorGrading>((Action<ColorGrading>) (_cg => ((ParameterOverride<float>) _cg.contrast).value = (__Null) (double) value));
        }
      }

      public void Init(
        Sprite[] _sprite,
        ColorGrading _colorGrading,
        PostProcessVolume _postProcessVolume)
      {
        this.Init(_sprite);
        this.ColorGrading = _colorGrading;
        this.PostProcessVolumeBlend = _postProcessVolume;
        this.ColorGradingBlend = (ColorGrading) this.PostProcessVolumeBlend.get_profile().GetSetting<ColorGrading>();
        this.dropdownLookupTexture.set_options(Singleton<Info>.Instance.dicColorGradingLoadInfo.Select<KeyValuePair<int, Info.LoadCommonInfo>, Dropdown.OptionData>((Func<KeyValuePair<int, Info.LoadCommonInfo>, Dropdown.OptionData>) (v => new Dropdown.OptionData(v.Value.name))).ToList<Dropdown.OptionData>());
        // ISSUE: method pointer
        ((UnityEvent<int>) this.dropdownLookupTexture.get_onValueChanged()).AddListener(new UnityAction<int>((object) this, __methodptr(OnValueChangedLookupTexture)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icBlend.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedBlend)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icBlend.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditBlend)));
        // ISSUE: method pointer
        ((UnityEvent) this.icBlend.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickBlend)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icSaturation.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSaturation)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icSaturation.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditSaturation)));
        // ISSUE: method pointer
        ((UnityEvent) this.icSaturation.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickSaturation)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icBrightness.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedBrightness)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icBrightness.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditBrightness)));
        // ISSUE: method pointer
        ((UnityEvent) this.icBrightness.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickBrightness)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icContrast.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedContrast)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icContrast.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditContrast)));
        // ISSUE: method pointer
        ((UnityEvent) this.icContrast.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickContrast)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.dropdownLookupTexture.set_value(Singleton<Studio.Studio>.Instance.sceneInfo.cgLookupTexture);
        this.icBlend.value = Singleton<Studio.Studio>.Instance.sceneInfo.cgBlend;
        this.icSaturation.IntValue = Singleton<Studio.Studio>.Instance.sceneInfo.cgSaturation;
        this.icBrightness.IntValue = Singleton<Studio.Studio>.Instance.sceneInfo.cgBrightness;
        this.icContrast.IntValue = Singleton<Studio.Studio>.Instance.sceneInfo.cgContrast;
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        this.Blend = Singleton<Studio.Studio>.Instance.sceneInfo.cgBlend;
        this.Saturation = (float) Singleton<Studio.Studio>.Instance.sceneInfo.cgSaturation;
        this.Brightness = (float) Singleton<Studio.Studio>.Instance.sceneInfo.cgBrightness;
        this.Contrast = (float) Singleton<Studio.Studio>.Instance.sceneInfo.cgContrast;
        this.SetLookupTexture(Singleton<Studio.Studio>.Instance.sceneInfo.cgLookupTexture);
      }

      public void SetLookupTexture(int _no)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.cgLookupTexture = _no;
        Info.LoadCommonInfo loadCommonInfo = (Info.LoadCommonInfo) null;
        if (!Singleton<Info>.Instance.dicColorGradingLoadInfo.TryGetValue(_no, out loadCommonInfo))
          return;
        ((ParameterOverride<Texture>) this.ColorGradingBlend.ldrLut).Override(CommonLib.LoadAsset<Texture>(loadCommonInfo.bundlePath, loadCommonInfo.fileName, false, string.Empty));
      }

      private void OnValueChangedLookupTexture(int _value)
      {
        if (this.isUpdateInfo)
          return;
        this.SetLookupTexture(_value);
      }

      private void OnValueChangedBlend(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgBlend = _value;
        this.Blend = _value;
        this.icBlend.value = _value;
      }

      private void OnEndEditBlend(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icBlend.Min, this.icBlend.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.cgBlend = num;
        this.Blend = num;
        this.icBlend.value = num;
      }

      private void OnClickBlend()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgBlend = ScreenEffectDefine.ColorGradingBlend;
        this.Blend = ScreenEffectDefine.ColorGradingBlend;
        this.icBlend.value = ScreenEffectDefine.ColorGradingBlend;
      }

      private void OnValueChangedSaturation(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgSaturation = Mathf.FloorToInt(_value);
        this.Saturation = _value;
        this.icSaturation.IntValue = Singleton<Studio.Studio>.Instance.sceneInfo.cgSaturation;
      }

      private void OnEndEditSaturation(string _text)
      {
        if (this.isUpdateInfo)
          return;
        int num = Mathf.FloorToInt(Mathf.Clamp(Utility.StringToFloat(_text), this.icSaturation.Min, this.icSaturation.Max));
        Singleton<Studio.Studio>.Instance.sceneInfo.cgSaturation = num;
        this.Saturation = (float) num;
        this.icSaturation.IntValue = num;
      }

      private void OnClickSaturation()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgSaturation = ScreenEffectDefine.ColorGradingSaturation;
        this.Saturation = (float) ScreenEffectDefine.ColorGradingSaturation;
        this.icSaturation.IntValue = ScreenEffectDefine.ColorGradingSaturation;
      }

      private void OnValueChangedBrightness(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgBrightness = Mathf.FloorToInt(_value);
        this.Brightness = _value;
        this.icBrightness.IntValue = Singleton<Studio.Studio>.Instance.sceneInfo.cgBrightness;
      }

      private void OnEndEditBrightness(string _text)
      {
        if (this.isUpdateInfo)
          return;
        int num = Mathf.FloorToInt(Mathf.Clamp(Utility.StringToFloat(_text), this.icBrightness.Min, this.icBrightness.Max));
        Singleton<Studio.Studio>.Instance.sceneInfo.cgBrightness = num;
        this.Brightness = (float) num;
        this.icBrightness.IntValue = num;
      }

      private void OnClickBrightness()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgBrightness = ScreenEffectDefine.ColorGradingBrightness;
        this.Brightness = (float) ScreenEffectDefine.ColorGradingBrightness;
        this.icBrightness.IntValue = ScreenEffectDefine.ColorGradingBrightness;
      }

      private void OnValueChangedContrast(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgContrast = Mathf.FloorToInt(_value);
        this.Contrast = _value;
        this.icContrast.IntValue = Singleton<Studio.Studio>.Instance.sceneInfo.cgContrast;
      }

      private void OnEndEditContrast(string _text)
      {
        if (this.isUpdateInfo)
          return;
        int num = Mathf.FloorToInt(Mathf.Clamp(Utility.StringToFloat(_text), this.icContrast.Min, this.icContrast.Max));
        Singleton<Studio.Studio>.Instance.sceneInfo.cgContrast = num;
        this.Contrast = (float) num;
        this.icContrast.IntValue = num;
      }

      private void OnClickContrast()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.cgContrast = ScreenEffectDefine.ColorGradingSaturation;
        this.Contrast = (float) ScreenEffectDefine.ColorGradingSaturation;
        this.icContrast.IntValue = ScreenEffectDefine.ColorGradingSaturation;
      }
    }

    [Serializable]
    private class AmbientOcclusionInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;
      public Button buttonColor;
      public SystemButtonCtrl.InputCombination icIntensity;
      public SystemButtonCtrl.InputCombination icThicknessModeifier;

      private AmbientOcclusion AmbientOcculusion { get; set; }

      public void Init(Sprite[] _sprite, AmbientOcclusion _ambientOcculusion)
      {
        this.Init(_sprite);
        this.AmbientOcculusion = _ambientOcculusion;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icIntensity.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedIntensity)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icIntensity.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditIntensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.icIntensity.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickIntensity)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icThicknessModeifier.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedThicknessModeifier)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icThicknessModeifier.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditThicknessModeifier)));
        // ISSUE: method pointer
        ((UnityEvent) this.icThicknessModeifier.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickThicknessModeifier)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableAmbientOcclusion);
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.aoColor);
        this.icIntensity.value = Singleton<Studio.Studio>.Instance.sceneInfo.aoIntensity;
        this.icThicknessModeifier.value = Singleton<Studio.Studio>.Instance.sceneInfo.aoThicknessModeifier;
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        if (Object.op_Equality((Object) this.AmbientOcculusion, (Object) null))
          return;
        ((PostProcessEffectSettings) this.AmbientOcculusion).active = (__Null) (Singleton<Studio.Studio>.Instance.sceneInfo.enableAmbientOcclusion ? 1 : 0);
        ((ParameterOverride<Color>) this.AmbientOcculusion.color).value = (__Null) Singleton<Studio.Studio>.Instance.sceneInfo.aoColor;
        ((ParameterOverride<float>) this.AmbientOcculusion.intensity).value = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.aoIntensity;
        ((ParameterOverride<float>) this.AmbientOcculusion.thicknessModifier).value = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.aoThicknessModeifier;
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableAmbientOcclusion = _value;
        ((PostProcessEffectSettings) this.AmbientOcculusion).active = (__Null) (_value ? 1 : 0);
      }

      private void OnClickColor()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("アンビエントオクルージョン"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("アンビエントオクルージョン", Singleton<Studio.Studio>.Instance.sceneInfo.aoColor, (Action<Color>) (_c =>
          {
            Singleton<Studio.Studio>.Instance.sceneInfo.aoColor = _c;
            ((ParameterOverride<Color>) this.AmbientOcculusion.color).value = (__Null) _c;
            ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(_c);
          }), false);
      }

      private void OnValueChangedIntensity(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.aoIntensity = _value;
        ((ParameterOverride<float>) this.AmbientOcculusion.intensity).value = (__Null) (double) _value;
        this.icIntensity.value = _value;
      }

      private void OnEndEditIntensity(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icIntensity.Min, this.icIntensity.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.aoIntensity = num;
        ((ParameterOverride<float>) this.AmbientOcculusion.intensity).value = (__Null) (double) num;
        this.icIntensity.value = num;
      }

      private void OnClickIntensity()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.aoIntensity = ScreenEffectDefine.AmbientOcclusionIntensity;
        ((ParameterOverride<float>) this.AmbientOcculusion.intensity).value = (__Null) (double) ScreenEffectDefine.AmbientOcclusionIntensity;
        this.icIntensity.value = ScreenEffectDefine.AmbientOcclusionIntensity;
      }

      private void OnValueChangedThicknessModeifier(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.aoThicknessModeifier = _value;
        ((ParameterOverride<float>) this.AmbientOcculusion.thicknessModifier).value = (__Null) (double) _value;
        this.icThicknessModeifier.value = _value;
      }

      private void OnEndEditThicknessModeifier(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icThicknessModeifier.Min, this.icThicknessModeifier.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.aoThicknessModeifier = num;
        ((ParameterOverride<float>) this.AmbientOcculusion.thicknessModifier).value = (__Null) (double) num;
        this.icThicknessModeifier.value = num;
      }

      private void OnClickThicknessModeifier()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.aoThicknessModeifier = ScreenEffectDefine.AmbientOcclusionThicknessModeifier;
        ((ParameterOverride<float>) this.AmbientOcculusion.thicknessModifier).value = (__Null) (double) ScreenEffectDefine.AmbientOcclusionThicknessModeifier;
        this.icThicknessModeifier.value = ScreenEffectDefine.AmbientOcclusionThicknessModeifier;
      }
    }

    [Serializable]
    private class BloomInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;
      public SystemButtonCtrl.InputCombination icIntensity;
      public SystemButtonCtrl.InputCombination icThreshold;
      public SystemButtonCtrl.InputCombination icSoftKnee;
      public Toggle toggleClamp;
      public SystemButtonCtrl.InputCombination icDiffusion;
      public Button buttonColor;

      private Bloom Bloom { get; set; }

      public void Init(Sprite[] _sprite, Bloom _bloom)
      {
        this.Init(_sprite);
        this.Bloom = _bloom;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icIntensity.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedIntensity)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icIntensity.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditIntensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.icIntensity.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickIntensityDef)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icThreshold.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedThreshold)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icThreshold.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditThreshold)));
        // ISSUE: method pointer
        ((UnityEvent) this.icThreshold.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickThresholdDef)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icSoftKnee.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSoftKnee)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icSoftKnee.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditSoftKnee)));
        // ISSUE: method pointer
        ((UnityEvent) this.icSoftKnee.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickSoftKnee)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleClamp.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedClamp)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icDiffusion.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedDiffusion)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icDiffusion.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditDiffusion)));
        // ISSUE: method pointer
        ((UnityEvent) this.icDiffusion.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDiffusion)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableBloom);
        this.icIntensity.value = Singleton<Studio.Studio>.Instance.sceneInfo.bloomIntensity;
        this.icThreshold.value = Singleton<Studio.Studio>.Instance.sceneInfo.bloomThreshold;
        this.icSoftKnee.value = Singleton<Studio.Studio>.Instance.sceneInfo.bloomSoftKnee;
        this.toggleClamp.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.bloomClamp);
        this.icDiffusion.value = Singleton<Studio.Studio>.Instance.sceneInfo.bloomDiffusion;
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.bloomColor);
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        if (Object.op_Equality((Object) this.Bloom, (Object) null))
          return;
        ((PostProcessEffectSettings) this.Bloom).active = (__Null) (Singleton<Studio.Studio>.Instance.sceneInfo.enableBloom ? 1 : 0);
        ((ParameterOverride<float>) this.Bloom.intensity).value = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.bloomIntensity;
        ((ParameterOverride<float>) this.Bloom.threshold).value = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.bloomThreshold;
        ((ParameterOverride<float>) this.Bloom.softKnee).value = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.bloomSoftKnee;
        ((ParameterOverride) this.Bloom.clamp).overrideState = (__Null) (Singleton<Studio.Studio>.Instance.sceneInfo.bloomClamp ? 1 : 0);
        ((ParameterOverride<float>) this.Bloom.diffusion).value = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.bloomDiffusion;
        ((ParameterOverride<Color>) this.Bloom.color).value = (__Null) Singleton<Studio.Studio>.Instance.sceneInfo.bloomColor;
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableBloom = _value;
        ((PostProcessEffectSettings) this.Bloom).active = (__Null) (_value ? 1 : 0);
      }

      private void OnValueChangedIntensity(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomIntensity = _value;
        ((ParameterOverride<float>) this.Bloom.intensity).value = (__Null) (double) _value;
        this.icIntensity.value = _value;
      }

      private void OnEndEditIntensity(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icIntensity.Min, this.icIntensity.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomIntensity = num;
        ((ParameterOverride<float>) this.Bloom.intensity).value = (__Null) (double) num;
        this.icIntensity.value = num;
      }

      private void OnClickIntensityDef()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomIntensity = ScreenEffectDefine.BloomIntensity;
        ((ParameterOverride<float>) this.Bloom.intensity).value = (__Null) (double) ScreenEffectDefine.BloomIntensity;
        this.icIntensity.value = ScreenEffectDefine.BloomIntensity;
      }

      private void OnValueChangedThreshold(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomThreshold = _value;
        ((ParameterOverride<float>) this.Bloom.threshold).value = (__Null) (double) _value;
        this.icThreshold.value = _value;
      }

      private void OnEndEditThreshold(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icThreshold.Min, this.icThreshold.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomThreshold = num;
        ((ParameterOverride<float>) this.Bloom.threshold).value = (__Null) (double) num;
        this.icThreshold.value = num;
      }

      private void OnClickThresholdDef()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomThreshold = ScreenEffectDefine.BloomThreshold;
        ((ParameterOverride<float>) this.Bloom.threshold).value = (__Null) (double) ScreenEffectDefine.BloomThreshold;
        this.icThreshold.value = ScreenEffectDefine.BloomThreshold;
      }

      private void OnValueChangedSoftKnee(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomSoftKnee = _value;
        ((ParameterOverride<float>) this.Bloom.softKnee).value = (__Null) (double) _value;
        this.icSoftKnee.value = _value;
      }

      private void OnEndEditSoftKnee(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icSoftKnee.Min, this.icSoftKnee.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomSoftKnee = num;
        ((ParameterOverride<float>) this.Bloom.softKnee).value = (__Null) (double) num;
        this.icSoftKnee.value = num;
      }

      private void OnClickSoftKnee()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomSoftKnee = ScreenEffectDefine.BloomSoftKnee;
        ((ParameterOverride<float>) this.Bloom.softKnee).value = (__Null) (double) ScreenEffectDefine.BloomSoftKnee;
        this.icSoftKnee.value = ScreenEffectDefine.BloomSoftKnee;
      }

      private void OnValueChangedClamp(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomClamp = _value;
        ((ParameterOverride) this.Bloom.clamp).overrideState = (__Null) (_value ? 1 : 0);
      }

      private void OnValueChangedDiffusion(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomDiffusion = _value;
        ((ParameterOverride<float>) this.Bloom.diffusion).value = (__Null) (double) _value;
        this.icDiffusion.value = _value;
      }

      private void OnEndEditDiffusion(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icDiffusion.Min, this.icDiffusion.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomDiffusion = num;
        ((ParameterOverride<float>) this.Bloom.diffusion).value = (__Null) (double) num;
        this.icDiffusion.value = num;
      }

      private void OnClickDiffusion()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.bloomDiffusion = ScreenEffectDefine.BloomDiffusion;
        ((ParameterOverride<float>) this.Bloom.diffusion).value = (__Null) (double) ScreenEffectDefine.BloomDiffusion;
        this.icDiffusion.value = ScreenEffectDefine.BloomDiffusion;
      }

      private void OnClickColor()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("ブルーム"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("ブルーム", Singleton<Studio.Studio>.Instance.sceneInfo.bloomColor, (Action<Color>) (_c =>
          {
            Singleton<Studio.Studio>.Instance.sceneInfo.bloomColor = _c;
            ((ParameterOverride<Color>) this.Bloom.color).value = (__Null) _c;
            ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(_c);
          }), false);
      }
    }

    [Serializable]
    private class DOFInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;
      public SystemButtonCtrl.Selector selectorForcus;
      public SystemButtonCtrl.InputCombination icFocalSize;
      public SystemButtonCtrl.InputCombination icAperture;

      private DepthOfField depthOfField { get; set; }

      public void Init(Sprite[] _sprite, DepthOfField _dof)
      {
        this.Init(_sprite);
        this.depthOfField = _dof;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
        // ISSUE: method pointer
        ((UnityEvent) this.selectorForcus._button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickForcus)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icFocalSize.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedFocalSize)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icFocalSize.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditFocalSize)));
        // ISSUE: method pointer
        ((UnityEvent) this.icFocalSize.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickFocalSizeDef)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icAperture.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedAperture)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icAperture.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditAperture)));
        // ISSUE: method pointer
        ((UnityEvent) this.icAperture.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickApertureDef)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableDepth);
        this.icFocalSize.value = Singleton<Studio.Studio>.Instance.sceneInfo.depthFocalSize;
        this.icAperture.value = Singleton<Studio.Studio>.Instance.sceneInfo.depthAperture;
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        if (Object.op_Inequality((Object) this.depthOfField, (Object) null))
        {
          ((Behaviour) this.depthOfField).set_enabled(Singleton<Studio.Studio>.Instance.sceneInfo.enableDepth);
          this.depthOfField.focalSize = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.depthFocalSize;
          this.depthOfField.aperture = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.depthAperture;
        }
        Singleton<Studio.Studio>.Instance.SetDepthOfFieldForcus(Singleton<Studio.Studio>.Instance.sceneInfo.depthForcus);
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableDepth = _value;
        ((Behaviour) this.depthOfField).set_enabled(_value);
      }

      private void OnClickForcus()
      {
        if (this.isUpdateInfo)
          return;
        GuideObject selectObject = Singleton<GuideObjectManager>.Instance.selectObject;
        Singleton<Studio.Studio>.Instance.SetDepthOfFieldForcus(!Object.op_Inequality((Object) selectObject, (Object) null) ? -1 : selectObject.dicKey);
      }

      private void OnValueChangedFocalSize(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.depthFocalSize = _value;
        this.depthOfField.focalSize = (__Null) (double) _value;
        this.icFocalSize.value = _value;
      }

      private void OnEndEditFocalSize(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icFocalSize.Min, this.icFocalSize.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.depthFocalSize = num;
        this.depthOfField.focalSize = (__Null) (double) num;
        this.icFocalSize.value = num;
      }

      private void OnClickFocalSizeDef()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.depthFocalSize = ScreenEffectDefine.DepthOfFieldFocalSize;
        this.depthOfField.focalSize = (__Null) (double) ScreenEffectDefine.DepthOfFieldFocalSize;
        this.icFocalSize.value = ScreenEffectDefine.DepthOfFieldFocalSize;
      }

      private void OnValueChangedAperture(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.depthAperture = _value;
        this.depthOfField.aperture = (__Null) (double) _value;
        this.icAperture.value = _value;
      }

      private void OnEndEditAperture(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icAperture.Min, this.icAperture.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.depthAperture = num;
        this.depthOfField.aperture = (__Null) (double) num;
        this.icAperture.value = num;
      }

      private void OnClickApertureDef()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.depthAperture = ScreenEffectDefine.DepthOfFieldAperture;
        this.depthOfField.aperture = (__Null) (double) ScreenEffectDefine.DepthOfFieldAperture;
        this.icAperture.value = ScreenEffectDefine.DepthOfFieldAperture;
      }
    }

    [Serializable]
    private class VignetteInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;

      private Vignette Vignette { get; set; }

      public void Init(Sprite[] _sprite, Vignette _vignette)
      {
        this.Init(_sprite);
        this.Vignette = _vignette;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableVignette);
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        if (Object.op_Equality((Object) this.Vignette, (Object) null))
          return;
        ((PostProcessEffectSettings) this.Vignette).active = (__Null) (Singleton<Studio.Studio>.Instance.sceneInfo.enableVignette ? 1 : 0);
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableVignette = _value;
        ((PostProcessEffectSettings) this.Vignette).active = (__Null) (_value ? 1 : 0);
      }
    }

    [Serializable]
    private class ScreenSpaceReflectionInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;

      private ScreenSpaceReflections ScreenSpaceReflections { get; set; }

      public void Init(Sprite[] _sprite, ScreenSpaceReflections _screenSpaceReflections)
      {
        this.Init(_sprite);
        this.ScreenSpaceReflections = _screenSpaceReflections;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableSSR);
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        if (Object.op_Equality((Object) this.ScreenSpaceReflections, (Object) null))
          return;
        ((PostProcessEffectSettings) this.ScreenSpaceReflections).active = (__Null) (Singleton<Studio.Studio>.Instance.sceneInfo.enableSSR ? 1 : 0);
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableSSR = _value;
        ((PostProcessEffectSettings) this.ScreenSpaceReflections).active = (__Null) (_value ? 1 : 0);
      }
    }

    [Serializable]
    private class ReflectionProbeInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;
      public Dropdown dropdownCubemap;
      public SystemButtonCtrl.InputCombination icIntensity;

      private ReflectionProbe ReflectionProbe { get; set; }

      private GameObject GameObject { get; set; }

      public void Init(Sprite[] _sprite, ReflectionProbe _reflectionProbe, GameObject _gameObject)
      {
        this.Init(_sprite);
        this.ReflectionProbe = _reflectionProbe;
        this.GameObject = _gameObject;
        this.dropdownCubemap.set_options(Singleton<Info>.Instance.dicReflectionProbeLoadInfo.Select<KeyValuePair<int, Info.LoadCommonInfo>, Dropdown.OptionData>((Func<KeyValuePair<int, Info.LoadCommonInfo>, Dropdown.OptionData>) (v => new Dropdown.OptionData(v.Value.name))).ToList<Dropdown.OptionData>());
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
        // ISSUE: method pointer
        ((UnityEvent<int>) this.dropdownCubemap.get_onValueChanged()).AddListener(new UnityAction<int>((object) this, __methodptr(OnValueChangedCubemap)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icIntensity.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedIntensity)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icIntensity.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditIntensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.icIntensity.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickIntensity)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableReflectionProbe);
        this.dropdownCubemap.set_value(Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeCubemap);
        this.icIntensity.value = Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeIntensity;
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        GameObject gameObject = this.GameObject;
        if (gameObject != null)
          gameObject.SetActiveIfDifferent(Singleton<Studio.Studio>.Instance.sceneInfo.enableReflectionProbe);
        if (!Object.op_Inequality((Object) this.ReflectionProbe, (Object) null))
          return;
        this.SetCubemap(Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeCubemap);
        this.ReflectionProbe.set_intensity(Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeIntensity);
      }

      public void SetCubemap(int _no)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeCubemap = _no;
        Info.LoadCommonInfo loadCommonInfo = (Info.LoadCommonInfo) null;
        if (!Singleton<Info>.Instance.dicReflectionProbeLoadInfo.TryGetValue(_no, out loadCommonInfo))
          return;
        this.ReflectionProbe.set_customBakedTexture(CommonLib.LoadAsset<Texture>(loadCommonInfo.bundlePath, loadCommonInfo.fileName, false, string.Empty));
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableReflectionProbe = _value;
        GameObject gameObject = this.GameObject;
        if (gameObject == null)
          return;
        gameObject.SetActiveIfDifferent(_value);
      }

      private void OnValueChangedCubemap(int _value)
      {
        if (this.isUpdateInfo)
          return;
        this.SetCubemap(_value);
      }

      private void OnValueChangedIntensity(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeIntensity = _value;
        this.ReflectionProbe.set_intensity(_value);
        this.icIntensity.value = _value;
      }

      private void OnEndEditIntensity(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icIntensity.Min, this.icIntensity.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeIntensity = num;
        this.ReflectionProbe.set_intensity(num);
        this.icIntensity.value = num;
      }

      private void OnClickIntensity()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.reflectionProbeIntensity = ScreenEffectDefine.ReflectionProbeIntensity;
        this.ReflectionProbe.set_intensity(ScreenEffectDefine.ReflectionProbeIntensity);
        this.icIntensity.value = ScreenEffectDefine.ReflectionProbeIntensity;
      }
    }

    [Serializable]
    private class FogInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;
      public Toggle toggleExcludeFarPixels;
      public SystemButtonCtrl.InputCombination icHeight;
      public SystemButtonCtrl.InputCombination icHeightDensity;
      public Button buttonColor;
      public SystemButtonCtrl.InputCombination icDensity;

      private GlobalFog GlobalFog { get; set; }

      public void Init(Sprite[] _sprite, GlobalFog _fog)
      {
        this.Init(_sprite);
        this.GlobalFog = _fog;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleExcludeFarPixels.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedExcludeFarPixels)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icHeight.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedHeight)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icHeight.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditHeight)));
        // ISSUE: method pointer
        ((UnityEvent) this.icHeight.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickHeight)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icHeightDensity.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedHeightDensity)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icHeightDensity.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditHeightDensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.icHeightDensity.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickHeightDensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icDensity.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedDensity)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icDensity.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditDensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.icDensity.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDensity)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableFog);
        this.toggleExcludeFarPixels.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.fogExcludeFarPixels);
        this.icHeight.value = Singleton<Studio.Studio>.Instance.sceneInfo.fogHeight;
        this.icHeightDensity.value = Singleton<Studio.Studio>.Instance.sceneInfo.fogHeightDensity;
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.fogColor);
        this.icDensity.value = Singleton<Studio.Studio>.Instance.sceneInfo.fogDensity;
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        if (Object.op_Inequality((Object) this.GlobalFog, (Object) null))
        {
          ((Behaviour) this.GlobalFog).set_enabled(Singleton<Studio.Studio>.Instance.sceneInfo.enableFog);
          this.GlobalFog.excludeFarPixels = (__Null) (Singleton<Studio.Studio>.Instance.sceneInfo.fogExcludeFarPixels ? 1 : 0);
          this.GlobalFog.height = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.fogHeight;
          this.GlobalFog.heightDensity = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.fogHeightDensity;
        }
        RenderSettings.set_fog(Singleton<Studio.Studio>.Instance.sceneInfo.enableFog);
        RenderSettings.set_fogColor(Singleton<Studio.Studio>.Instance.sceneInfo.fogColor);
        RenderSettings.set_fogDensity(Singleton<Studio.Studio>.Instance.sceneInfo.fogDensity);
      }

      public void SetEnable(bool _value, bool _UI = true)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.enableFog = _value;
        ((Behaviour) this.GlobalFog).set_enabled(_value);
        RenderSettings.set_fog(_value);
        if (!_UI)
          return;
        this.toggleEnable.set_isOn(_value);
      }

      public void SetColor(Color _color)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.fogColor = _color;
        RenderSettings.set_fogColor(_color);
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(_color);
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.SetEnable(_value, false);
      }

      private void OnValueChangedExcludeFarPixels(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.fogExcludeFarPixels = _value;
        this.GlobalFog.excludeFarPixels = (__Null) (_value ? 1 : 0);
      }

      private void OnClickColor()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("フォグ"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("フォグ", Singleton<Studio.Studio>.Instance.sceneInfo.fogColor, new Action<Color>(this.SetColor), false);
      }

      private void OnValueChangedHeight(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.fogHeight = _value;
        this.GlobalFog.height = (__Null) (double) _value;
        this.icHeight.value = _value;
      }

      private void OnEndEditHeight(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icHeight.Min, this.icHeight.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.fogHeight = num;
        this.GlobalFog.height = (__Null) (double) num;
        this.icHeight.value = num;
      }

      private void OnClickHeight()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.fogHeight = ScreenEffectDefine.FogHeight;
        this.GlobalFog.height = (__Null) (double) ScreenEffectDefine.FogHeight;
        this.icHeight.value = ScreenEffectDefine.FogHeight;
      }

      private void OnValueChangedHeightDensity(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.fogHeightDensity = _value;
        this.GlobalFog.heightDensity = (__Null) (double) _value;
        this.icHeightDensity.value = _value;
      }

      private void OnEndEditHeightDensity(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icHeightDensity.Min, this.icHeightDensity.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.fogHeightDensity = num;
        this.GlobalFog.heightDensity = (__Null) (double) num;
        this.icHeightDensity.value = num;
      }

      private void OnClickHeightDensity()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.fogHeightDensity = ScreenEffectDefine.FogHeightDensity;
        this.GlobalFog.heightDensity = (__Null) (double) ScreenEffectDefine.FogHeightDensity;
        this.icHeightDensity.value = ScreenEffectDefine.FogHeightDensity;
      }

      private void OnValueChangedDensity(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.fogDensity = _value;
        RenderSettings.set_fogDensity(_value);
        this.icDensity.value = _value;
      }

      private void OnEndEditDensity(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icDensity.Min, this.icDensity.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.fogDensity = num;
        RenderSettings.set_fogDensity(num);
        this.icDensity.value = num;
      }

      private void OnClickDensity()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.fogDensity = ScreenEffectDefine.FogDensity;
        RenderSettings.set_fogDensity(ScreenEffectDefine.FogDensity);
        this.icDensity.value = ScreenEffectDefine.FogDensity;
      }
    }

    [Serializable]
    private class SunShaftsInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;
      public SystemButtonCtrl.Selector selectorCaster;
      public Button buttonThresholdColor;
      public Button buttonShaftsColor;
      public SystemButtonCtrl.InputCombination icDistanceFallOff;
      public SystemButtonCtrl.InputCombination icBlurSize;
      public SystemButtonCtrl.InputCombination icIntensity;

      private SunShafts sunShafts { get; set; }

      public void Init(Sprite[] _sprite, SunShafts _sunShafts)
      {
        this.Init(_sprite);
        this.sunShafts = _sunShafts;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
        // ISSUE: method pointer
        ((UnityEvent) this.selectorCaster._button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickCaster)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonThresholdColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickThresholdColor)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonShaftsColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickShaftsColor)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icDistanceFallOff.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedDistanceFallOff)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icDistanceFallOff.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditDistanceFallOff)));
        // ISSUE: method pointer
        ((UnityEvent) this.icDistanceFallOff.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDistanceFallOff)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icBlurSize.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedBlurSize)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icBlurSize.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditBlurSize)));
        // ISSUE: method pointer
        ((UnityEvent) this.icBlurSize.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickBlurSize)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.icIntensity.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedIntensity)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.icIntensity.input.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditIntensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.icIntensity.buttonDefault.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickIntensity)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableSunShafts);
        ((Graphic) ((Selectable) this.buttonThresholdColor).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.sunThresholdColor);
        ((Graphic) ((Selectable) this.buttonShaftsColor).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.sunColor);
        Singleton<Studio.Studio>.Instance.SetSunCaster(Singleton<Studio.Studio>.Instance.sceneInfo.sunCaster);
        this.icDistanceFallOff.value = Singleton<Studio.Studio>.Instance.sceneInfo.sunDistanceFallOff;
        this.icBlurSize.value = Singleton<Studio.Studio>.Instance.sceneInfo.sunBlurSize;
        this.icIntensity.value = Singleton<Studio.Studio>.Instance.sceneInfo.sunIntensity;
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        if (Object.op_Equality((Object) this.sunShafts, (Object) null))
          return;
        ((Behaviour) this.sunShafts).set_enabled(Singleton<Studio.Studio>.Instance.sceneInfo.enableSunShafts);
        this.sunShafts.sunThreshold = (__Null) Singleton<Studio.Studio>.Instance.sceneInfo.sunThresholdColor;
        this.sunShafts.sunColor = (__Null) Singleton<Studio.Studio>.Instance.sceneInfo.sunColor;
        this.sunShafts.maxRadius = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.sunDistanceFallOff;
        this.sunShafts.sunShaftBlurRadius = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.sunBlurSize;
        this.sunShafts.sunShaftIntensity = (__Null) (double) Singleton<Studio.Studio>.Instance.sceneInfo.sunIntensity;
      }

      public void SetShaftsColor(Color _color)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.sunColor = _color;
        ((Graphic) ((Selectable) this.buttonShaftsColor).get_image()).set_color(_color);
        this.sunShafts.sunColor = (__Null) _color;
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableSunShafts = _value;
        ((Behaviour) this.sunShafts).set_enabled(_value);
      }

      private void OnClickThresholdColor()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("サンシャフト しきい色"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("サンシャフト しきい色", Singleton<Studio.Studio>.Instance.sceneInfo.sunThresholdColor, (Action<Color>) (_c =>
          {
            Singleton<Studio.Studio>.Instance.sceneInfo.sunThresholdColor = _c;
            ((Graphic) ((Selectable) this.buttonThresholdColor).get_image()).set_color(_c);
            this.sunShafts.sunThreshold = (__Null) _c;
          }), false);
      }

      private void OnClickShaftsColor()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("サンシャフト 光の色"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("サンシャフト 光の色", Singleton<Studio.Studio>.Instance.sceneInfo.sunColor, new Action<Color>(this.SetShaftsColor), false);
      }

      private void OnClickCaster()
      {
        if (this.isUpdateInfo)
          return;
        GuideObject selectObject = Singleton<GuideObjectManager>.Instance.selectObject;
        Singleton<Studio.Studio>.Instance.SetSunCaster(!Object.op_Inequality((Object) selectObject, (Object) null) ? -1 : selectObject.dicKey);
      }

      private void OnValueChangedDistanceFallOff(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.sunDistanceFallOff = _value;
        this.sunShafts.maxRadius = (__Null) (double) _value;
        this.icDistanceFallOff.value = _value;
      }

      private void OnEndEditDistanceFallOff(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icDistanceFallOff.Min, this.icDistanceFallOff.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.sunDistanceFallOff = num;
        this.sunShafts.maxRadius = (__Null) (double) num;
        this.icDistanceFallOff.value = num;
      }

      private void OnClickDistanceFallOff()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.sunDistanceFallOff = ScreenEffectDefine.SunShaftDistanceFallOff;
        this.sunShafts.maxRadius = (__Null) (double) ScreenEffectDefine.SunShaftDistanceFallOff;
        this.icDistanceFallOff.value = ScreenEffectDefine.SunShaftDistanceFallOff;
      }

      private void OnValueChangedBlurSize(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.sunBlurSize = _value;
        this.sunShafts.sunShaftBlurRadius = (__Null) (double) _value;
        this.icBlurSize.value = _value;
      }

      private void OnEndEditBlurSize(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icBlurSize.Min, this.icBlurSize.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.sunBlurSize = num;
        this.sunShafts.sunShaftBlurRadius = (__Null) (double) num;
        this.icBlurSize.value = num;
      }

      private void OnClickBlurSize()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.sunBlurSize = ScreenEffectDefine.SunShaftBlurSize;
        this.sunShafts.sunShaftBlurRadius = (__Null) (double) ScreenEffectDefine.SunShaftBlurSize;
        this.icBlurSize.value = ScreenEffectDefine.SunShaftBlurSize;
      }

      private void OnValueChangedIntensity(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.sunIntensity = _value;
        this.sunShafts.sunShaftIntensity = (__Null) (double) _value;
        this.icIntensity.value = _value;
      }

      private void OnEndEditIntensity(string _text)
      {
        if (this.isUpdateInfo)
          return;
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.icIntensity.Min, this.icIntensity.Max);
        Singleton<Studio.Studio>.Instance.sceneInfo.sunIntensity = num;
        this.sunShafts.sunShaftIntensity = (__Null) (double) num;
        this.icIntensity.value = num;
      }

      private void OnClickIntensity()
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.sunIntensity = ScreenEffectDefine.SunShaftIntensity;
        this.sunShafts.sunShaftIntensity = (__Null) (double) ScreenEffectDefine.SunShaftIntensity;
        this.icIntensity.value = ScreenEffectDefine.SunShaftIntensity;
      }
    }

    [Serializable]
    private class SelfShadowInfo : SystemButtonCtrl.EffectInfo
    {
      public Toggle toggleEnable;

      public override void Init(Sprite[] _sprite)
      {
        base.Init(_sprite);
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleEnable.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEnable)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        this.toggleEnable.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.enableShadow);
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        this.Set(Singleton<Studio.Studio>.Instance.sceneInfo.enableShadow);
      }

      private void OnValueChangedEnable(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.enableShadow = _value;
        this.Set(_value);
      }

      private void Set(bool _value)
      {
        QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel() / 2 * 2 + (!_value ? 1 : 0));
      }
    }

    [Serializable]
    private class EnvironmentLightingInfo : SystemButtonCtrl.EffectInfo
    {
      public Button buttonSkyColor;
      public Button buttonEquator;
      public Button buttonGround;

      public override void Init(Sprite[] _sprite)
      {
        base.Init(_sprite);
        // ISSUE: method pointer
        ((UnityEvent) this.buttonSkyColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickSkyColor)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonEquator.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickEquator)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonGround.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickGround)));
      }

      public override void UpdateInfo()
      {
        base.UpdateInfo();
        this.isUpdateInfo = true;
        ((Graphic) ((Selectable) this.buttonSkyColor).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingSkyColor);
        ((Graphic) ((Selectable) this.buttonEquator).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingEquatorColor);
        ((Graphic) ((Selectable) this.buttonGround).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingGroundColor);
        this.Apply();
        this.isUpdateInfo = false;
      }

      public override void Apply()
      {
        RenderSettings.set_ambientSkyColor(Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingSkyColor);
        RenderSettings.set_ambientEquatorColor(Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingEquatorColor);
        RenderSettings.set_ambientGroundColor(Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingGroundColor);
      }

      private void OnClickSkyColor()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("空の環境光"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("空の環境光", Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingSkyColor, (Action<Color>) (_c =>
          {
            Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingSkyColor = _c;
            RenderSettings.set_ambientSkyColor(_c);
            ((Graphic) ((Selectable) this.buttonSkyColor).get_image()).set_color(_c);
          }), false);
      }

      private void OnClickEquator()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("地平線の環境光"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("地平線の環境光", Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingEquatorColor, (Action<Color>) (_c =>
          {
            Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingEquatorColor = _c;
            RenderSettings.set_ambientEquatorColor(_c);
            ((Graphic) ((Selectable) this.buttonEquator).get_image()).set_color(_c);
          }), false);
      }

      private void OnClickGround()
      {
        if (this.isUpdateInfo)
          return;
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("地面の環境光"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("地面の環境光", Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingGroundColor, (Action<Color>) (_c =>
          {
            Singleton<Studio.Studio>.Instance.sceneInfo.environmentLightingGroundColor = _c;
            RenderSettings.set_ambientGroundColor(_c);
            ((Graphic) ((Selectable) this.buttonGround).get_image()).set_color(_c);
          }), false);
      }
    }
  }
}
