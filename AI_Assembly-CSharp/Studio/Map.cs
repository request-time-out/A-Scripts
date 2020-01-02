// Decompiled with JetBrains decompiler
// Type: Studio.Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Studio
{
  public class Map : Singleton<Map>
  {
    public GameObject MapRoot { get; private set; }

    public bool isLoading { get; private set; }

    public int no { get; private set; } = -1;

    private MapComponent MapComponent { get; set; }

    public bool IsOption
    {
      get
      {
        return Object.op_Inequality((Object) this.MapComponent, (Object) null) && this.MapComponent.CheckOption;
      }
    }

    public bool VisibleOption
    {
      get
      {
        return Singleton<Studio.Studio>.Instance.sceneInfo.mapOption;
      }
      set
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.mapOption = value;
        this.MapComponent?.SetOptionVisible(value);
      }
    }

    [DebuggerHidden]
    public IEnumerator LoadMapCoroutine(int _no, bool _wait = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadMapCoroutine\u003Ec__Iterator0()
      {
        _no = _no,
        _wait = _wait,
        \u0024this = this
      };
    }

    public void LoadMap(int _no)
    {
      if (!Singleton<Info>.Instance.dicMapLoadInfo.ContainsKey(_no))
      {
        this.ReleaseMap();
      }
      else
      {
        if (this.no == _no)
          return;
        this.MapComponent = (MapComponent) null;
        this.isLoading = true;
        this.no = _no;
        Info.MapLoadInfo data = Singleton<Info>.Instance.dicMapLoadInfo[_no];
        Singleton<Scene>.Instance.LoadBaseScene(new Scene.Data()
        {
          assetBundleName = data.bundlePath,
          levelName = data.fileName,
          fadeType = Scene.Data.FadeType.None,
          onLoad = (Action) (() => this.OnLoadAfter(data.fileName))
        });
      }
    }

    public void ReleaseMap()
    {
      if (!Singleton<Map>.IsInstance())
        return;
      this.MapRoot = (GameObject) null;
      this.no = -1;
      this.MapComponent = (MapComponent) null;
      Singleton<Scene>.Instance.UnloadBaseScene();
    }

    private void OnLoadAfter(string _levelName)
    {
      Scene scene = Scene.GetScene(_levelName);
      this.MapRoot = ((Scene) ref scene).GetRootGameObjects().SafeGet<GameObject>(0);
      this.MapComponent = (MapComponent) this.MapRoot?.GetComponentInChildren<MapComponent>();
      if (Object.op_Inequality((Object) this.MapComponent, (Object) null))
        this.MapComponent.SetupSea();
      if (Singleton<MapCtrl>.IsInstance())
        Singleton<MapCtrl>.Instance.Reflect();
      if (Singleton<Studio.Studio>.IsInstance())
        Singleton<Studio.Studio>.Instance.systemButtonCtrl.Apply();
      this.isLoading = false;
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      this.isLoading = false;
      this.no = -1;
      this.MapRoot = (GameObject) null;
    }
  }
}
