// Decompiled with JetBrains decompiler
// Type: bl_ApplySettingsPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class bl_ApplySettingsPro : MonoBehaviour
{
  [SerializeField]
  private CanvasScaler HUDCanvas;
  [SerializeField]
  private GameObject[] FPSObject;
  private int[] ShadowCascadeOptions;
  private bl_BrightnessImage BrightnessImage;

  public bl_ApplySettingsPro()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) Object.FindObjectOfType<bl_BrightnessImage>(), (Object) null))
      this.BrightnessImage = (bl_BrightnessImage) Object.FindObjectOfType<bl_BrightnessImage>();
    this.LoadAndApply();
  }

  public void ShadowProjectionType(bool b)
  {
    if (b)
      QualitySettings.set_shadowProjection((ShadowProjection) 1);
    else
      QualitySettings.set_shadowProjection((ShadowProjection) 0);
  }

  private void LoadAndApply()
  {
    int num1 = PlayerPrefs.GetInt("GameName.AntiAliasing");
    int num2 = PlayerPrefs.GetInt("GameName.AnisoTropic");
    int num3 = PlayerPrefs.GetInt("GameName.BlendWeight");
    int num4 = PlayerPrefs.GetInt("GameName.QualityLevel");
    int index1 = PlayerPrefs.GetInt("GameName.ResolutionScreen");
    int num5 = PlayerPrefs.GetInt("GameName.VSyncCount");
    int num6 = PlayerPrefs.GetInt("GameName.TextureLimit", 0);
    int index2 = PlayerPrefs.GetInt("GameName.ShadowCascade", 0);
    bool flag1 = PlayerPrefs.GetInt("GameName.ShowFPS", 0) == 1;
    float num7 = PlayerPrefs.GetFloat("GameName.Volumen", 1f);
    float num8 = PlayerPrefs.GetFloat("GameName.ShadowDistance");
    bool b = PlayerPrefs.GetInt("GameName.ShadowProjection", 0) == 1;
    bool flag2 = AllOptionsKeyPro.IntToBool(PlayerPrefs.GetInt("GameName.ShadowEnable"));
    float val = PlayerPrefs.GetFloat("GameName.Brightness", 1f);
    bool flag3 = AllOptionsKeyPro.IntToBool(PlayerPrefs.GetInt("GameName.RealtimeReflection", 1));
    float num9 = PlayerPrefs.GetFloat("GameName.LoadBias", 1f);
    float num10 = PlayerPrefs.GetFloat("GameName.HudScale", 0.0f);
    QualitySettings.set_shadowDistance(num8);
    AudioListener.set_volume(num7);
    AudioListener.set_pause(PlayerPrefs.GetInt("GameName.PauseAudio", 0) == 1);
    this.ShadowProjectionType(b);
    QualitySettings.set_masterTextureLimit(num6);
    QualitySettings.set_shadowCascades(this.ShadowCascadeOptions[index2]);
    QualitySettings.SetQualityLevel(num4);
    QualitySettings.set_realtimeReflectionProbes(flag3);
    QualitySettings.set_shadowDistance(!flag2 ? 0.0f : num8);
    QualitySettings.set_lodBias(num9);
    if (Object.op_Inequality((Object) this.BrightnessImage, (Object) null))
      this.BrightnessImage.SetValue(val);
    else
      Debug.LogWarning((object) "You have not the brightness prefab in this scene, brightness will not work");
    if (Object.op_Inequality((Object) this.HUDCanvas, (Object) null))
      this.HUDCanvas.set_matchWidthOrHeight(1f - num10);
    if (this.FPSObject != null)
    {
      foreach (GameObject gameObject in this.FPSObject)
        gameObject.SetActive(flag1);
    }
    switch (num2)
    {
      case 0:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 0);
        break;
      case 1:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 1);
        break;
      case 2:
        QualitySettings.set_anisotropicFiltering((AnisotropicFiltering) 2);
        break;
    }
    switch (num1)
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
    switch (num5)
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
    switch (num3)
    {
      case 0:
        QualitySettings.set_blendWeights((BlendWeights) 1);
        break;
      case 1:
        QualitySettings.set_blendWeights((BlendWeights) 2);
        break;
      case 2:
        QualitySettings.set_blendWeights((BlendWeights) 4);
        break;
    }
    Screen.SetResolution(((Resolution) ref Screen.get_resolutions()[index1]).get_width(), ((Resolution) ref Screen.get_resolutions()[index1]).get_height(), false);
  }
}
