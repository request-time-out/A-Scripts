// Decompiled with JetBrains decompiler
// Type: SceneLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using UnityEngine;

public class SceneLoader : BaseLoader
{
  public bool isLoad = true;
  public bool isAsync = true;
  public bool isFade = true;
  public bool isLoadingImageDraw = true;
  [SerializeField]
  protected bool isStartAfterErase = true;
  public string assetBundleName;
  public string levelName;
  public bool isOverlap;
  public string manifestFileName;
  public Action onLoad;

  public Func<IEnumerator> onFadeIn { get; set; }

  public Func<IEnumerator> onFadeOut { get; set; }

  public Scene.Data.FadeType fadeType { get; set; }

  protected virtual void Start()
  {
    Scene.Data data = new Scene.Data()
    {
      assetBundleName = this.assetBundleName,
      levelName = this.levelName,
      isAdd = !this.isLoad,
      isAsync = this.isAsync,
      isOverlap = this.isOverlap,
      manifestFileName = this.manifestFileName,
      onLoad = this.onLoad,
      onFadeIn = this.onFadeIn,
      onFadeOut = this.onFadeOut
    };
    if (this.isFade)
      data.isFade = this.isFade;
    else
      data.fadeType = this.fadeType;
    Singleton<Scene>.Instance.LoadReserve(data, this.isLoadingImageDraw);
    if (!this.isStartAfterErase)
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }
}
