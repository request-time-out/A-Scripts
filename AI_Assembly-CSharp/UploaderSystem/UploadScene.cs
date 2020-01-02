// Decompiled with JetBrains decompiler
// Type: UploaderSystem.UploadScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UniRx;

namespace UploaderSystem
{
  public class UploadScene : BaseLoader
  {
    public UpPhpControl phpCtrl;
    public UpUIControl uiCtrl;

    private NetworkInfo netInfo
    {
      get
      {
        return Singleton<NetworkInfo>.Instance;
      }
    }

    private NetCacheControl cacheCtrl
    {
      get
      {
        return Singleton<NetworkInfo>.IsInstance() ? this.netInfo.cacheCtrl : (NetCacheControl) null;
      }
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.phpCtrl.GetBaseInfo(true)), false), (Action<M0>) (__ => {}), (Action<Exception>) (err => {}), (Action) (() => this.uiCtrl.UpdateProfile()));
      Illusion.Game.Utils.Sound.SettingBGM settingBgm = new Illusion.Game.Utils.Sound.SettingBGM();
      settingBgm.assetBundleName = "sound/data/bgm/00.unity3d";
      settingBgm.assetName = "ai_bgm_10";
      Illusion.Game.Utils.Sound.Play((Illusion.Game.Utils.Sound.Setting) settingBgm);
    }

    private void Update()
    {
    }
  }
}
