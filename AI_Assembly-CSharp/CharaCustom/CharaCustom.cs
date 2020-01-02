// Decompiled with JetBrains decompiler
// Type: CharaCustom.CharaCustom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace CharaCustom
{
  [DefaultExecutionOrder(-1)]
  public class CharaCustom : BaseLoader
  {
    public static bool modeNew = true;
    public static byte modeSex = 1;
    public static string nextScene = string.Empty;
    public static string editCharaFileName = string.Empty;
    private float shadowDistance = 400f;
    [Button("ChangeInert", "惰性通常", new object[] {false})]
    public int inert00;
    [Button("ChangeInert", "惰性強", new object[] {true})]
    public int inert01;
    public static Action actEixt;
    [SerializeField]
    private CustomControl customCtrl;
    [SerializeField]
    private CanvasGroup cgScene;
    private int backLimit;

    private void ChangeInert(bool h)
    {
      if (!Object.op_Inequality((Object) null, (Object) Singleton<CustomBase>.Instance.chaCtrl))
        return;
      Singleton<CustomBase>.Instance.chaCtrl.ChangeBustInert(h);
    }

    protected override void Awake()
    {
      base.Awake();
    }

    private void Start()
    {
      if (Singleton<Manager.Map>.IsInstance())
        Singleton<Manager.Map>.Instance.UpdateTexSetting = false;
      this.shadowDistance = QualitySettings.get_shadowDistance();
      this.backLimit = QualitySettings.get_masterTextureLimit();
      if (QualitySettings.GetQualityLevel() / 2 == 0)
        QualitySettings.set_masterTextureLimit(1);
      else
        QualitySettings.set_masterTextureLimit(0);
      Singleton<Character>.Instance.customLoadGCClear = true;
      this.customCtrl.Initialize(CharaCustom.CharaCustom.modeSex, CharaCustom.CharaCustom.modeNew, CharaCustom.CharaCustom.nextScene, CharaCustom.CharaCustom.editCharaFileName);
      Illusion.Game.Utils.Sound.SettingBGM settingBgm = new Illusion.Game.Utils.Sound.SettingBGM();
      settingBgm.assetBundleName = "sound/data/bgm/00.unity3d";
      settingBgm.assetName = "ai_bgm_10";
      Illusion.Game.Utils.Sound.Play((Illusion.Game.Utils.Sound.Setting) settingBgm);
      this.cgScene.set_blocksRaycasts(true);
    }

    private void Update()
    {
      if ((double) QualitySettings.get_shadowDistance() == 120.0)
        return;
      QualitySettings.set_shadowDistance(120f);
    }

    private void OnDestroy()
    {
      if (Singleton<CustomBase>.IsInstance())
        Singleton<CustomBase>.Instance.customSettingSave.Save();
      if (Singleton<Character>.IsInstance())
      {
        Singleton<Character>.Instance.chaListCtrl.SaveItemID();
        Singleton<Character>.Instance.DeleteCharaAll();
        Singleton<Character>.Instance.EndLoadAssetBundle(false);
        Singleton<Character>.Instance.customLoadGCClear = false;
      }
      QualitySettings.set_shadowDistance(this.shadowDistance);
      QualitySettings.set_masterTextureLimit(this.backLimit);
      if (Singleton<Manager.Map>.IsInstance())
        Singleton<Manager.Map>.Instance.UpdateTexSetting = true;
      CharaCustom.CharaCustom.nextScene = string.Empty;
      CharaCustom.CharaCustom.editCharaFileName = string.Empty;
      if (CharaCustom.CharaCustom.actEixt != null)
        CharaCustom.CharaCustom.actEixt();
      CharaCustom.CharaCustom.actEixt = (Action) null;
    }
  }
}
