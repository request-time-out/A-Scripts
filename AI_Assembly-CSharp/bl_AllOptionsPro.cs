// Decompiled with JetBrains decompiler
// Type: bl_AllOptionsPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class bl_AllOptionsPro : MonoBehaviour
{
  [Header("Panels")]
  [SerializeField]
  private GameObject[] Panels;
  [SerializeField]
  private Button[] PanelButtons;
  [SerializeField]
  private Animator PanelAnimator;
  [Header("Settings")]
  public bool ApplyOnStart;
  public bool AutoApplyResolution;
  public bool SaveOnDisable;
  public bool AnimateHidePanel;
  public int StartWindow;
  [SerializeField]
  [Range(0.0f, 8f)]
  private int DefaultQuality;
  [SerializeField]
  [Range(0.0f, 15f)]
  private int DefaultResolution;
  [SerializeField]
  [Range(0.0f, 3f)]
  private int DefaultAntiAliasing;
  [SerializeField]
  [Range(0.0f, 2f)]
  private int DefaultAnisoTropic;
  [SerializeField]
  [Range(0.0f, 2f)]
  private int DefaultVSync;
  [SerializeField]
  [Range(0.0f, 2f)]
  private int DefaultBlendWeight;
  [SerializeField]
  [Range(0.0f, 100f)]
  private int DefaultShadowDistance;
  [SerializeField]
  [Range(0.0f, 1f)]
  private int DefaultBrightness;
  [SerializeField]
  [Range(0.01f, 3f)]
  private int DefaultLoadBias;
  [Header("Options Name")]
  [SerializeField]
  private string[] AntiAliasingNames;
  [SerializeField]
  private string[] VSyncNames;
  [SerializeField]
  private string[] TextureQualityNames;
  [SerializeField]
  private string[] ShadowCascadeNames;
  [Header("References")]
  [SerializeField]
  private GameObject SettingsPanel;
  [SerializeField]
  private Animator ContentAnim;
  public Text QualityText;
  private int CurrentQuality;
  public Text AnisotropicText;
  private int CurrentAS;
  public Text AntiAliasingText;
  private int CurrentAA;
  public Text vSyncText;
  private int CurrentVSC;
  public Text blendWeightsText;
  private int CurrentBW;
  public Text ResolutionText;
  private int CurrentRS;
  [SerializeField]
  private Text FullScreenOnText;
  private bool useFullScreen;
  [SerializeField]
  private Text TextureLimitText;
  private int CurrentTL;
  [SerializeField]
  private Text RealtimeReflectionText;
  private bool _realtimeReflection;
  [SerializeField]
  private Text LoadBiasText;
  private float _lodBias;
  [SerializeField]
  private Text ShadowCascadeText;
  private int CurrentSC;
  private int[] ShadowCascadeOptions;
  [SerializeField]
  private Text ShowFPSText;
  private bool _showFPS;
  [SerializeField]
  private Text ShadowDistanceText;
  [SerializeField]
  private Slider ShadowDistanceSlider;
  private float cacheShadowDistance;
  [SerializeField]
  private Slider BrightnessSlider;
  [SerializeField]
  private Slider LoadBiasSlider;
  [SerializeField]
  private Slider HUDScaleFactor;
  [SerializeField]
  private Text HudScaleText;
  [SerializeField]
  private Text BrightnessText;
  private float _brightness;
  [SerializeField]
  private Text ShadowProjectionText;
  private bool shadowProjection;
  [SerializeField]
  private Text ShadowEnebleText;
  private bool _shadowEnable;
  [SerializeField]
  private Text PauseText;
  private bool _isPauseSound;
  [SerializeField]
  private Text VolumenText;
  [SerializeField]
  private Slider VolumenSlider;
  [SerializeField]
  private Text TitlePanelText;
  [SerializeField]
  private CanvasScaler HUDCanvas;
  [SerializeField]
  private GameObject[] FPSObject;
  private bl_BrightnessImage BrightnessImage;
  private float _hudScale;
  private bool Show;
  private float _volumen;

  public bl_AllOptionsPro()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    if (Object.op_Inequality((Object) Object.FindObjectOfType<bl_BrightnessImage>(), (Object) null))
      this.BrightnessImage = (bl_BrightnessImage) Object.FindObjectOfType<bl_BrightnessImage>();
    if (!Object.op_Implicit((Object) this.HUDCanvas))
      return;
    this._hudScale = 1f - this.HUDCanvas.get_matchWidthOrHeight();
  }

  private void Start()
  {
    if (this.ApplyOnStart)
      this.LoadAndApply();
    this.ChangeWindow(this.StartWindow, false);
    this.ChangeSelectionButton(this.PanelButtons[this.StartWindow]);
    this.SettingsPanel.SetActive(false);
  }

  private void OnDisable()
  {
    if (!this.SaveOnDisable)
      return;
    this.SaveOptions();
  }

  private void OnApplicationQuit()
  {
    if (!this.SaveOnDisable)
      return;
    this.SaveOptions();
  }

  public void ChangeWindow(int _id)
  {
    this.PanelAnimator.Play("Change", 0, 0.0f);
    this.StartCoroutine(this.WaitForSwichet(_id));
  }

  public void ChangeWindow(int _id, bool anim)
  {
    if (anim)
      this.PanelAnimator.Play("Change", 0, 0.0f);
    this.StartCoroutine(this.WaitForSwichet(_id));
  }

  public void ChangeSelectionButton(Button b)
  {
    for (int index = 0; index < this.PanelButtons.Length; ++index)
      ((Selectable) this.PanelButtons[index]).set_interactable(true);
    ((Selectable) b).set_interactable(false);
  }

  public void ShowMenu()
  {
    this.Show = !this.Show;
    if (this.Show)
    {
      this.StopCoroutine("HideAnimate");
      this.SettingsPanel.SetActive(true);
      this.ContentAnim.SetBool("Show", true);
    }
    else if (this.AnimateHidePanel)
      this.StartCoroutine("HideAnimate");
    else
      this.SettingsPanel.SetActive(false);
  }

  public void GameQuality(bool mas)
  {
    this.CurrentQuality = !mas ? (this.CurrentQuality == 0 ? QualitySettings.get_names().Length - 1 : (this.CurrentQuality - 1) % QualitySettings.get_names().Length) : (this.CurrentQuality + 1) % QualitySettings.get_names().Length;
    this.QualityText.set_text(QualitySettings.get_names()[this.CurrentQuality].ToUpper());
    QualitySettings.SetQualityLevel(this.CurrentQuality);
  }

  public void AntiStropic(bool b)
  {
    this.CurrentAS = !b ? (this.CurrentAS == 0 ? 2 : (this.CurrentAS - 1) % 3) : (this.CurrentAS + 1) % 3;
    switch (this.CurrentAS)
    {
      case 0:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 0);
        this.AnisotropicText.set_text(((AnisotropicFiltering) 0).ToString().ToUpper());
        break;
      case 1:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 1);
        this.AnisotropicText.set_text(((AnisotropicFiltering) 1).ToString().ToUpper());
        break;
      case 2:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 2);
        this.AnisotropicText.set_text(((AnisotropicFiltering) 2).ToString().ToUpper());
        break;
    }
  }

  public void FullScreenMode(bool use)
  {
    this.useFullScreen = use;
    this.FullScreenOnText.set_text(!this.useFullScreen ? "OFF" : "ON");
  }

  public void AntiAliasing(bool b)
  {
    this.CurrentAA = !b ? (this.CurrentAA == 0 ? (this.CurrentAA = 3) : (this.CurrentAA - 1) % 4) : (this.CurrentAA + 1) % 4;
    this.AntiAliasingText.set_text(this.AntiAliasingNames[this.CurrentAA].ToUpper());
    switch (this.CurrentAA)
    {
      case 0:
        QualitySettings.set_antiAliasing(0);
        break;
      case 1:
        QualitySettings.set_antiAliasing(2);
        break;
      case 2:
        QualitySettings.set_antiAliasing(4);
        break;
      case 3:
        QualitySettings.set_antiAliasing(8);
        break;
    }
  }

  public void ShowFPS()
  {
    this._showFPS = !this._showFPS;
    this.ShowFPSText.set_text(!this._showFPS ? "OFF" : "ON");
    if (this.FPSObject == null)
      return;
    foreach (GameObject gameObject in this.FPSObject)
      gameObject.SetActive(this._showFPS);
  }

  public void PauseSound(bool b)
  {
    this._isPauseSound = b;
    this.PauseText.set_text(!this._isPauseSound ? "OFF" : "ON");
  }

  public void VSyncCount(bool b)
  {
    this.CurrentVSC = !b ? (this.CurrentVSC == 0 ? (this.CurrentVSC = 2) : (this.CurrentVSC - 1) % 3) : (this.CurrentVSC + 1) % 3;
    this.vSyncText.set_text(this.VSyncNames[this.CurrentVSC].ToUpper());
    switch (this.CurrentVSC)
    {
      case 0:
        QualitySettings.set_vSyncCount(0);
        break;
      case 1:
        QualitySettings.set_vSyncCount(1);
        break;
      case 2:
        QualitySettings.set_vSyncCount(2);
        break;
    }
  }

  public void TextureQuality(bool b)
  {
    this.CurrentTL = !b ? (this.CurrentTL == 0 ? (this.CurrentTL = 3) : (this.CurrentTL - 1) % 3) : (this.CurrentTL + 1) % 3;
    QualitySettings.set_masterTextureLimit(this.CurrentTL);
    this.TextureLimitText.set_text(this.TextureQualityNames[this.CurrentTL]);
  }

  public void ShadowCascades(bool b)
  {
    this.CurrentSC = !b ? (this.CurrentSC == 0 ? (this.CurrentSC = 3) : (this.CurrentSC - 1) % 3) : (this.CurrentSC + 1) % 3;
    QualitySettings.set_shadowCascades(this.ShadowCascadeOptions[this.CurrentSC]);
    this.ShadowCascadeText.set_text(this.ShadowCascadeNames[this.CurrentSC]);
  }

  public void blendWeights(bool b)
  {
    this.CurrentBW = !b ? (this.CurrentBW == 0 ? (this.CurrentBW = 2) : (this.CurrentBW - 1) % 3) : (this.CurrentBW + 1) % 3;
    switch (this.CurrentBW)
    {
      case 0:
        QualitySettings.set_blendWeights((BlendWeights) 1);
        this.blendWeightsText.set_text(((BlendWeights) 1).ToString().ToUpper());
        break;
      case 1:
        QualitySettings.set_blendWeights((BlendWeights) 2);
        this.blendWeightsText.set_text(((BlendWeights) 2).ToString().ToUpper());
        break;
      case 2:
        QualitySettings.set_blendWeights((BlendWeights) 4);
        this.blendWeightsText.set_text(((BlendWeights) 4).ToString().ToUpper());
        break;
    }
  }

  public void SetBrightness(float v)
  {
    if (Object.op_Equality((Object) this.BrightnessImage, (Object) null))
      return;
    this._brightness = v;
    this.BrightnessImage.SetValue(v);
    this.BrightnessSlider.set_value(v);
    this.BrightnessText.set_text(string.Format("{0}%", (object) (v * 100f).ToString("F0")));
  }

  public void SetLodBias(float value)
  {
    QualitySettings.set_lodBias(value);
    this._lodBias = value;
    this.LoadBiasText.set_text(string.Format("{0}", (object) value.ToString("F2")));
  }

  public void ShadowDistance(float value)
  {
    if (this._shadowEnable)
      QualitySettings.set_shadowDistance(value);
    this.ShadowDistanceText.set_text(string.Format("{0}m", (object) value.ToString("F0")));
    this.cacheShadowDistance = value;
  }

  public void SetShadowEnable(bool enable)
  {
    QualitySettings.set_shadowDistance(!enable ? 0.0f : this.cacheShadowDistance);
    this._shadowEnable = enable;
    this.ShadowEnebleText.set_text(!enable ? "DISABLE" : "ENABLE");
  }

  public void SetRealTimeReflection(bool b)
  {
    QualitySettings.set_realtimeReflectionProbes(b);
    this._realtimeReflection = b;
    this.RealtimeReflectionText.set_text(!this._realtimeReflection ? "DISABLE" : "ENABLE");
  }

  public void SetHUDScale(float value)
  {
    if (Object.op_Equality((Object) this.HUDCanvas, (Object) null))
      return;
    this.HUDCanvas.set_matchWidthOrHeight(1f - value);
    this._hudScale = value;
    this.HudScaleText.set_text(string.Format("{0}", (object) value.ToString("F2")));
  }

  public void Resolution(bool b)
  {
    this.CurrentRS = !b ? (this.CurrentRS == 0 ? (this.CurrentRS = Screen.get_resolutions().Length - 1) : (this.CurrentRS - 1) % Screen.get_resolutions().Length) : (this.CurrentRS + 1) % Screen.get_resolutions().Length;
    this.ResolutionText.set_text(((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_width().ToString() + " X " + (object) ((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_height());
  }

  public void Volumen(float v)
  {
    AudioListener.set_volume(v);
    this._volumen = v;
    this.VolumenText.set_text((this._volumen * 100f).ToString("00") + "%");
  }

  public void ShadowProjectionType(bool b)
  {
    if (b)
    {
      QualitySettings.set_shadowProjection((ShadowProjection) 1);
      this.ShadowProjectionText.set_text(((ShadowProjection) 1).ToString().ToUpper());
    }
    else
    {
      QualitySettings.set_shadowProjection((ShadowProjection) 0);
      this.ShadowProjectionText.set_text(((ShadowProjection) 0).ToString().ToUpper());
    }
  }

  public void ApplyResolution()
  {
    Screen.SetResolution(((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_width(), ((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_height(), this.AutoApplyResolution && this.useFullScreen);
  }

  private void LoadAndApply()
  {
    bl_Input.Instance.InitInput();
    this.CurrentAA = PlayerPrefs.GetInt("GameName.AntiAliasing", this.DefaultAntiAliasing);
    this.CurrentAS = PlayerPrefs.GetInt("GameName.AnisoTropic", this.DefaultAnisoTropic);
    this.CurrentBW = PlayerPrefs.GetInt("GameName.BlendWeight", this.DefaultBlendWeight);
    this.CurrentQuality = PlayerPrefs.GetInt("GameName.QualityLevel", this.DefaultQuality);
    this.CurrentRS = PlayerPrefs.GetInt("GameName.ResolutionScreen", this.DefaultResolution);
    this.CurrentVSC = PlayerPrefs.GetInt("GameName.VSyncCount", this.DefaultVSync);
    this.CurrentTL = PlayerPrefs.GetInt("GameName.TextureLimit", 0);
    this.CurrentSC = PlayerPrefs.GetInt("GameName.ShadowCascade", 0);
    this._showFPS = PlayerPrefs.GetInt("GameName.ShowFPS", 0) == 1;
    this._volumen = PlayerPrefs.GetFloat("GameName.Volumen", 1f);
    float num = PlayerPrefs.GetFloat("GameName.ShadowDistance", (float) this.DefaultShadowDistance);
    this.shadowProjection = PlayerPrefs.GetInt("GameName.ShadowProjection", 0) == 1;
    this.PauseSound(PlayerPrefs.GetInt("GameName.PauseAudio", 0) == 1);
    this.useFullScreen = PlayerPrefs.GetInt("GameName.ResolutionMode", 0) == 1;
    this._shadowEnable = AllOptionsKeyPro.IntToBool(PlayerPrefs.GetInt("GameName.ShadowEnable"));
    this._brightness = PlayerPrefs.GetFloat("GameName.Brightness", (float) this.DefaultBrightness);
    this._realtimeReflection = AllOptionsKeyPro.IntToBool(PlayerPrefs.GetInt("GameName.RealtimeReflection", 1));
    this._lodBias = PlayerPrefs.GetFloat("GameName.LoadBias", (float) this.DefaultLoadBias);
    this._hudScale = PlayerPrefs.GetFloat("GameName.HudScale", this._hudScale);
    this.SetBrightness(this._brightness);
    this.ShadowDistance(num);
    this.ShadowDistanceSlider.set_value(num);
    this.Volumen(this._volumen);
    this.VolumenSlider.set_value(this._volumen);
    this.ShadowProjectionType(this.shadowProjection);
    this.SetShadowEnable(this._shadowEnable);
    this.SetRealTimeReflection(this._realtimeReflection);
    this.SetLodBias(this._lodBias);
    this.SetHUDScale(this._hudScale);
    this.ApplyResolution();
    QualitySettings.set_shadowCascades(this.ShadowCascadeOptions[this.CurrentSC]);
    this.ShadowCascadeText.set_text(this.ShadowCascadeNames[this.CurrentSC].ToUpper());
    this.QualityText.set_text(QualitySettings.get_names()[this.CurrentQuality].ToUpper());
    QualitySettings.SetQualityLevel(this.CurrentQuality);
    this.FullScreenOnText.set_text(!this.useFullScreen ? "OFF" : "ON");
    this.ShowFPSText.set_text(!this._showFPS ? "OFF" : "ON");
    if (this.FPSObject != null)
    {
      foreach (GameObject gameObject in this.FPSObject)
        gameObject.SetActive(this._showFPS);
    }
    this.BrightnessSlider.set_value(this._brightness);
    this.LoadBiasSlider.set_value(this._lodBias);
    this.HUDScaleFactor.set_value(this._hudScale);
    switch (this.CurrentAS)
    {
      case 0:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 0);
        this.AnisotropicText.set_text(((AnisotropicFiltering) 0).ToString().ToUpper());
        break;
      case 1:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 1);
        this.AnisotropicText.set_text(((AnisotropicFiltering) 1).ToString().ToUpper());
        break;
      case 2:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 2);
        this.AnisotropicText.set_text(((AnisotropicFiltering) 2).ToString().ToUpper());
        break;
    }
    switch (this.CurrentAA)
    {
      case 0:
        QualitySettings.set_antiAliasing(0);
        break;
      case 1:
        QualitySettings.set_antiAliasing(2);
        break;
      case 2:
        QualitySettings.set_antiAliasing(4);
        break;
      case 3:
        QualitySettings.set_antiAliasing(8);
        break;
    }
    this.AntiAliasingText.set_text(this.AntiAliasingNames[this.CurrentAA].ToUpper());
    switch (this.CurrentVSC)
    {
      case 0:
        QualitySettings.set_vSyncCount(0);
        break;
      case 1:
        QualitySettings.set_vSyncCount(1);
        break;
      case 2:
        QualitySettings.set_vSyncCount(2);
        break;
    }
    this.vSyncText.set_text(this.VSyncNames[this.CurrentVSC].ToUpper());
    switch (this.CurrentBW)
    {
      case 0:
        QualitySettings.set_blendWeights((BlendWeights) 1);
        this.blendWeightsText.set_text(((BlendWeights) 1).ToString().ToUpper());
        break;
      case 1:
        QualitySettings.set_blendWeights((BlendWeights) 2);
        this.blendWeightsText.set_text(((BlendWeights) 2).ToString().ToUpper());
        break;
      case 2:
        QualitySettings.set_blendWeights((BlendWeights) 4);
        this.blendWeightsText.set_text(((BlendWeights) 4).ToString().ToUpper());
        break;
    }
    QualitySettings.set_masterTextureLimit(this.CurrentTL);
    this.TextureLimitText.set_text(this.TextureQualityNames[this.CurrentTL]);
    this.ResolutionText.set_text(((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_width().ToString() + " X " + (object) ((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_height());
    Screen.SetResolution(((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_width(), ((Resolution) ref Screen.get_resolutions()[this.CurrentRS]).get_height(), this.AutoApplyResolution && this.useFullScreen);
  }

  [DebuggerHidden]
  private IEnumerator WaitForSwichet(int _id)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new bl_AllOptionsPro.\u003CWaitForSwichet\u003Ec__Iterator0()
    {
      _id = _id,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  public static IEnumerator WaitForRealSeconds(float time)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new bl_AllOptionsPro.\u003CWaitForRealSeconds\u003Ec__Iterator1()
    {
      time = time
    };
  }

  [DebuggerHidden]
  private IEnumerator HideAnimate()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new bl_AllOptionsPro.\u003CHideAnimate\u003Ec__Iterator2()
    {
      \u0024this = this
    };
  }

  public void SaveOptions()
  {
    PlayerPrefs.SetInt("GameName.AnisoTropic", this.CurrentAS);
    PlayerPrefs.SetInt("GameName.AntiAliasing", this.CurrentAA);
    PlayerPrefs.SetInt("GameName.BlendWeight", this.CurrentBW);
    PlayerPrefs.SetInt("GameName.QualityLevel", this.CurrentQuality);
    PlayerPrefs.SetInt("GameName.ResolutionScreen", this.CurrentRS);
    PlayerPrefs.SetInt("GameName.VSyncCount", this.CurrentVSC);
    PlayerPrefs.SetInt("GameName.AnisoTropic", this.CurrentAS);
    PlayerPrefs.SetInt("GameName.TextureLimit", this.CurrentTL);
    PlayerPrefs.SetInt("GameName.ShadowCascade", this.CurrentSC);
    PlayerPrefs.SetFloat("GameName.Volumen", this._volumen);
    PlayerPrefs.SetFloat("GameName.ShadowDistance", this.cacheShadowDistance);
    PlayerPrefs.SetInt("GameName.ShadowProjection", !this.shadowProjection ? 0 : 1);
    PlayerPrefs.SetInt("GameName.ShowFPS", !this._showFPS ? 0 : 1);
    PlayerPrefs.SetInt("GameName.PauseAudio", !this._isPauseSound ? 0 : 1);
    PlayerPrefs.SetInt("GameName.ResolutionMode", !this.useFullScreen ? 0 : 1);
    PlayerPrefs.SetInt("GameName.ShadowEnable", AllOptionsKeyPro.BoolToInt(this._shadowEnable));
    PlayerPrefs.SetFloat("GameName.Brightness", this._brightness);
    PlayerPrefs.SetInt("GameName.RealtimeReflection", AllOptionsKeyPro.BoolToInt(this._realtimeReflection));
    PlayerPrefs.SetFloat("GameName.LoadBias", this._lodBias);
    PlayerPrefs.SetFloat("GameName.HudScale", this._hudScale);
  }
}
