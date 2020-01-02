// Decompiled with JetBrains decompiler
// Type: ADV.ADVScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV.Backup;
using Cinemachine;
using ConfigScene;
using Manager;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ADV
{
  public class ADVScene : BaseLoader
  {
    private FadeData bkFadeDat;
    [SerializeField]
    private TextScenario scenario;
    [SerializeField]
    private Transform stand;
    [SerializeField]
    private ADVFade advFade;
    private bool isReleased;
    private const string CreateCameraName = "FrontCamera";
    private const string CameraAssetName = "ActionCamera";
    private IDisposable updateDisposable;

    public TextScenario Scenario
    {
      get
      {
        return this.scenario;
      }
    }

    public Transform Stand
    {
      get
      {
        return this.stand;
      }
    }

    public ADVFade AdvFade
    {
      get
      {
        return this.advFade;
      }
    }

    public string startAddSceneName { get; private set; }

    public Camera advCamera
    {
      get
      {
        return this._advCamera;
      }
    }

    private Camera _advCamera { get; set; }

    public float? fadeTime { get; set; }

    private void Init()
    {
      this.isReleased = false;
      this.fadeTime = new float?();
      if (Object.op_Inequality((Object) this.stand, (Object) null))
        this.stand.SetPositionAndRotation(Vector3.get_zero(), Quaternion.get_identity());
      this.bkFadeDat = new FadeData((SimpleFade) Singleton<Scene>.Instance.sceneFade);
      TextScenario scenario = this.scenario;
      Camera main = Camera.get_main();
      this._advCamera = main;
      Camera camera = main;
      scenario.AdvCamera = camera;
      if (Singleton<Manager.Map>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null))
        this.scenario.virtualCamera = Singleton<Manager.Map>.Instance.Player.CameraControl.ADVCamera as CinemachineVirtualCamera;
      ParameterList.Init();
      if ((this.scenario.isWait || this.fadeTime.HasValue) && Object.op_Inequality((Object) this.advFade, (Object) null))
      {
        ADVFade advFade = this.advFade;
        float? fadeTime = this.fadeTime;
        double num = !fadeTime.HasValue ? 0.0 : (double) fadeTime.Value;
        advFade.CrossFadeAlpha(true, 1f, (float) num, true);
        this.scenario.isWait = true;
        IObservable<M0> observable = Observable.Take<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, bool>) (_ => this.advFade.IsFadeInEnd)), 1);
        Action<Unit> action = (Action<Unit>) (_ => this.scenario.isWait = false);
        // ISSUE: reference to a compiler-generated field
        if (ADVScene.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ADVScene.\u003C\u003Ef__mg\u0024cache0 = new Action(ParameterList.WaitEndProc);
        }
        // ISSUE: reference to a compiler-generated field
        Action fMgCache0 = ADVScene.\u003C\u003Ef__mg\u0024cache0;
        this.updateDisposable = ObservableExtensions.Subscribe<Unit>(observable, (Action<M0>) action, fMgCache0);
      }
      this.scenario.ConfigProc();
    }

    public void Release()
    {
      if (Scene.isGameEnd || this.isReleased)
        return;
      this.isReleased = true;
      if (this.updateDisposable != null)
        this.updateDisposable.Dispose();
      this.updateDisposable = (IDisposable) null;
      this.scenario.Release();
      if (!Singleton<Scene>.IsInstance())
        return;
      if (this.bkFadeDat != null)
        this.bkFadeDat.Load((SimpleFade) Singleton<Scene>.Instance.sceneFade);
      ParameterList.Release();
    }

    private void OnEnable()
    {
      SceneParameter.advScene = this;
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this.scenario.OnInitializedAsync, (Component) this), (Action<M0>) (_ => this.Init()));
      this.startAddSceneName = Singleton<Scene>.IsInstance() ? Singleton<Scene>.Instance.AddSceneNameOverlapRemoved : string.Empty;
    }

    private void OnDisable()
    {
    }

    private void Update()
    {
      if (!Singleton<Game>.IsInstance())
        return;
      ConfigWindow config = Singleton<Game>.Instance.Config;
      if (!Object.op_Inequality((Object) config, (Object) null))
        return;
      config.CharaEntryInteractable(false);
      this.scenario.ConfigProc();
    }
  }
}
