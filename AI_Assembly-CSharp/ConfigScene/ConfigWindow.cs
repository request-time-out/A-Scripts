// Decompiled with JetBrains decompiler
// Type: ConfigScene.ConfigWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using AIProject.Scene;
using Illusion.Extensions;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ConfigScene
{
  [DefaultExecutionOrder(100)]
  public class ConfigWindow : BaseLoader
  {
    public static Action UnLoadAction = (Action) null;
    public static Action TitleChangeAction = (Action) null;
    public static Color backGroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    public static bool IsConfig = false;
    private float timeScale = 1f;
    private readonly string[] localizeIsInit = new string[5]
    {
      "設定を初期化しますか？",
      string.Empty,
      string.Empty,
      string.Empty,
      string.Empty
    };
    private readonly string[] localizeIsTitle = new string[5]
    {
      "タイトルに戻りますか？",
      string.Empty,
      string.Empty,
      string.Empty,
      string.Empty
    };
    [SerializeField]
    private CanvasGroup canvasGroup;
    [Label("コンフィグ項目を写す場所")]
    [SerializeField]
    private RectTransform mainWindow;
    [Label("ショートカットボタンの場所")]
    [SerializeField]
    private RectTransform shortCutButtonBackGround;
    [Label("ショートカットボタンのプレファブ")]
    [SerializeField]
    private Button shortCutButtonPrefab;
    [Tooltip("ショートカットのリンク")]
    [SerializeField]
    private ConfigWindow.ShortCutGroup[] shortCuts;
    [Tooltip("初めに設定されているボタン")]
    [SerializeField]
    private Button[] buttons;
    [Tooltip("背景")]
    [SerializeField]
    private Image imgBackGroud;
    private BaseSetting[] settings;
    private Input.ValidType _validType;
    private GraphicSetting graphicSetting;

    public float timeScaleChange
    {
      set
      {
        this.timeScale = value;
      }
    }

    public void Unload()
    {
      if (!ConfigWindow.IsConfig)
        return;
      this.Save();
      if (ConfigWindow.UnLoadAction != null)
        ConfigWindow.UnLoadAction();
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }

    private void PlaySE(SoundPack.SystemSE se = SoundPack.SystemSE.OK_S)
    {
      Singleton<Resources>.Instance.SoundPack.Play(se);
    }

    protected override void Awake()
    {
      base.Awake();
      if (Singleton<Game>.IsInstance())
      {
        if (Object.op_Inequality((Object) Singleton<Game>.Instance.Config, (Object) null))
        {
          Object.Destroy((Object) ((Component) Singleton<Game>.Instance.Config).get_gameObject());
          Singleton<Game>.Instance.Config = (ConfigWindow) null;
          GC.Collect();
          Resources.UnloadUnusedAssets();
        }
        Singleton<Game>.Instance.Config = this;
      }
      this.timeScale = Time.get_timeScale();
      Time.set_timeScale(0.0f);
      if (!Singleton<Input>.IsInstance())
        return;
      this._validType = Singleton<Input>.Instance.State;
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
    }

    protected void OnDisable()
    {
      if (Singleton<Game>.IsInstance())
        Singleton<Game>.Instance.Config = (ConfigWindow) null;
      Time.set_timeScale(this.timeScale);
      if (!Singleton<Input>.IsInstance())
        return;
      Singleton<Input>.Instance.ReserveState(this._validType);
      Singleton<Input>.Instance.SetupState();
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ConfigWindow.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
      ConfigWindow.IsConfig = false;
      ConfigWindow.UnLoadAction = (Action) null;
      ConfigWindow.TitleChangeAction = (Action) null;
    }

    public void OnDefault()
    {
      this.PlaySE(SoundPack.SystemSE.OK_S);
      ConfirmScene.Sentence = this.localizeIsInit[Singleton<GameSystem>.Instance.languageInt];
      ConfirmScene.OnClickedYes = (Action) (() =>
      {
        this.PlaySE(SoundPack.SystemSE.OK_L);
        Singleton<Manager.Voice>.Instance.Reset();
        Singleton<Manager.Config>.Instance.Reset();
        foreach (BaseSetting setting in this.settings)
          setting.UIPresenter();
      });
      ConfirmScene.OnClickedNo = (Action) (() => this.PlaySE(SoundPack.SystemSE.Cancel));
      Singleton<Game>.Instance.LoadDialog();
    }

    public void OnTitle()
    {
      this.PlaySE(SoundPack.SystemSE.OK_S);
      bool flag = false;
      if (Singleton<Manager.Scene>.Instance.NowSceneNames[0] == "Title")
        flag = true;
      if (!flag)
      {
        ConfirmScene.Sentence = this.localizeIsTitle[Singleton<GameSystem>.Instance.languageInt];
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          this.Save();
          if (ConfigWindow.TitleChangeAction != null)
            ConfigWindow.TitleChangeAction();
          if (ConfigWindow.UnLoadAction != null)
            ConfigWindow.UnLoadAction();
          Singleton<Game>.Instance.Dialog.TimeScale = 1f;
          this.PlaySE(SoundPack.SystemSE.OK_L);
          Singleton<Manager.Scene>.Instance.LoadReserve(new Manager.Scene.Data()
          {
            levelName = "Title",
            isFade = true,
            onLoad = (Action) (() =>
            {
              if (!ConfigWindow.IsConfig)
                return;
              Object.Destroy((Object) ((Component) this).get_gameObject());
              Singleton<Game>.Instance.WorldData = (WorldData) null;
            })
          }, true);
        });
        ConfirmScene.OnClickedNo = (Action) (() => this.PlaySE(SoundPack.SystemSE.Cancel));
        Singleton<Game>.Instance.LoadDialog();
      }
      else
        this.Close((Action) (() => this.Unload()));
    }

    public void OnGameEnd()
    {
      this.PlaySE(SoundPack.SystemSE.OK_S);
    }

    public void OnBack()
    {
      if (Object.op_Inequality((Object) Singleton<Game>.Instance.ExitScene, (Object) null) || Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null))
        return;
      this.PlaySE(SoundPack.SystemSE.Cancel);
      this.Close((Action) (() => this.Unload()));
    }

    private void Save()
    {
      Singleton<Manager.Voice>.Instance.Save();
      Singleton<Manager.Config>.Instance.Save();
    }

    private void Open()
    {
      this.canvasGroup.set_blocksRaycasts(false);
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, true), true), (Action<M0>) (x => this.canvasGroup.set_alpha(((TimeInterval<float>) ref x).get_Value())), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() => this.canvasGroup.set_blocksRaycasts(true)));
    }

    private void Close(Action onCompleted)
    {
      this.canvasGroup.set_blocksRaycasts(false);
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, true), true), (Action<M0>) (x => this.canvasGroup.set_alpha(1f - ((TimeInterval<float>) ref x).get_Value())), (Action<Exception>) (ex => Debug.LogException(ex)), onCompleted);
    }

    public void CharaEntryInteractable(bool _interactable)
    {
      if (!Object.op_Implicit((Object) this.graphicSetting))
        return;
      this.graphicSetting.EntryInteractable(_interactable);
    }

    [Serializable]
    public class ShortCutGroup
    {
      public string title;
      public string name;
      public int visibleNo;
      [SerializeField]
      private RectTransform[] _trans;

      public RectTransform trans
      {
        get
        {
          return this._trans[this.visibleNo];
        }
      }

      public void VisibleUpdate()
      {
        foreach (RectTransform tran in this._trans)
          ((Component) tran).get_gameObject().SetActiveIfDifferent(Object.op_Equality((Object) tran, (Object) this.trans));
      }
    }
  }
}
