// Decompiled with JetBrains decompiler
// Type: LoadingScreenPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using PlayfulSystems.LoadingScreen;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenPro : LoadingScreenProBase
{
  [Header("Fade Settings")]
  public bool doFade = true;
  public float fadeInDuration = 1f;
  public float fadeOutDuration = 1f;
  public Color fadeFromColor = Color.get_black();
  public Color fadeToColor = Color.get_black();
  [Tooltip("#progress# will be replaced with the loading progress from 0 to 100.")]
  public string loadingString = "#progress#%";
  [Header("Scene Info")]
  public Text sceneInfoHeader;
  public Text sceneInfoDescription;
  public Image sceneInfoImage;
  private const string scenePreviewPath = "ScenePreviews/";
  [Header("Game Tips")]
  public Text tipHeader;
  public Text tipDescription;
  private CameraFade fade;
  [Header("Loading Visuals")]
  [Tooltip("A canvas group and parent for all graphics to show during loading. Leave empty if you want no fading.")]
  public CanvasGroup loadingCanvasGroup;
  [Tooltip("Progress Bar Pro that will filled as the target scene loads.")]
  public ProgressBarPro progressBar;
  [Tooltip("Fillable image that will be filled as the target scene loads.")]
  public Image loadingBar;
  public Text loadingText;
  [Header("Confirmation Visuals")]
  [Tooltip("A canvas group and parent for all graphics to show once loading is done. Leave empty if you want no fading.")]
  public CanvasGroup confirmationCanvasGroup;

  protected override void Init()
  {
    if (!this.doFade)
      return;
    this.fade = (CameraFade) ((Component) this).get_gameObject().AddComponent<CameraFade>();
    this.fade.Init();
  }

  protected override void DisplaySceneInfo(SceneInfo info)
  {
    this.SetTextIfStringIsNotNull(this.sceneInfoHeader, info == null ? (string) null : info.header);
    this.SetTextIfStringIsNotNull(this.sceneInfoDescription, info == null ? (string) null : info.description);
    if (!Object.op_Inequality((Object) this.sceneInfoImage, (Object) null) || info == null || string.IsNullOrEmpty(info.imageName))
      return;
    this.sceneInfoImage.set_sprite((Sprite) Resources.Load<Sprite>("ScenePreviews/" + info.imageName));
    AspectRatioFitter component = (AspectRatioFitter) ((Component) this.sceneInfoImage).GetComponent<AspectRatioFitter>();
    if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) this.sceneInfoImage.get_sprite(), (Object) null))
      return;
    component.set_aspectRatio((float) ((Texture) this.sceneInfoImage.get_sprite().get_texture()).get_width() / (float) ((Texture) this.sceneInfoImage.get_sprite().get_texture()).get_height());
  }

  protected override void DisplayGameTip(LoadingTip info)
  {
    this.SetTextIfStringIsNotNull(this.tipHeader, info == null ? (string) null : info.header);
    this.SetTextIfStringIsNotNull(this.tipDescription, info == null ? (string) null : info.description);
  }

  protected override void ShowStartingVisuals()
  {
    if (this.doFade)
      this.fade.StartFadeFrom(this.fadeFromColor, this.fadeInDuration, (Action) null);
    this.SetLoadingVisuals(0.0f);
    this.ShowGroup(this.loadingCanvasGroup, true, 0.0f);
    this.ShowGroup(this.confirmationCanvasGroup, false, 0.0f);
  }

  protected override void ShowProgressVisuals(float progress)
  {
    this.SetLoadingVisuals(progress);
  }

  protected override void ShowLoadingDoneVisuals()
  {
    this.ShowGroup(this.loadingCanvasGroup, false, 0.25f);
    this.ShowGroup(this.confirmationCanvasGroup, true, 0.25f);
  }

  protected override void ShowEndingVisuals()
  {
    if (!this.doFade)
      return;
    this.fade.StartFadeTo(this.fadeToColor, this.fadeOutDuration, (Action) null);
  }

  protected override bool CanShowConfirmation()
  {
    return !Object.op_Inequality((Object) this.progressBar, (Object) null) || !this.progressBar.IsAnimating();
  }

  protected override bool CanActivateTargetScene()
  {
    return !this.doFade || !Object.op_Inequality((Object) this.fade, (Object) null) || !this.fade.IsFading();
  }

  private void SetTextIfStringIsNotNull(Text text, string s)
  {
    if (Object.op_Equality((Object) text, (Object) null))
      return;
    if (string.IsNullOrEmpty(s))
    {
      ((Component) text).get_gameObject().SetActive(false);
    }
    else
    {
      ((Component) text).get_gameObject().SetActive(true);
      text.set_text(s);
    }
  }

  private void ShowGroup(CanvasGroup group, bool show, float fadeDuration)
  {
    if (Object.op_Equality((Object) group, (Object) null))
      return;
    CanvasGroupFade canvasGroupFade = (CanvasGroupFade) ((Component) group).GetComponent<CanvasGroupFade>();
    if (Object.op_Equality((Object) canvasGroupFade, (Object) null))
      canvasGroupFade = (CanvasGroupFade) ((Component) group).get_gameObject().AddComponent<CanvasGroupFade>();
    if (!Object.op_Inequality((Object) canvasGroupFade, (Object) null))
      return;
    canvasGroupFade.FadeAlpha(!show ? 1f : 0.0f, !show ? 0.0f : 1f, fadeDuration);
  }

  private void SetLoadingVisuals(float progress)
  {
    if (Object.op_Inequality((Object) this.progressBar, (Object) null))
      this.progressBar.Value = progress;
    if (Object.op_Inequality((Object) this.loadingBar, (Object) null))
      this.loadingBar.set_fillAmount(progress);
    if (!Object.op_Inequality((Object) this.loadingText, (Object) null))
      return;
    this.loadingText.set_text(this.loadingString.Replace("#progress#", Mathf.RoundToInt(progress * 100f).ToString()));
  }
}
