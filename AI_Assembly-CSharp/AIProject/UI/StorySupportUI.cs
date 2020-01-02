// Decompiled with JetBrains decompiler
// Type: AIProject.UI.StorySupportUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI.Popup;
using ConfigScene;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (CanvasGroup))]
  public class StorySupportUI : UIBehaviour
  {
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private RectTransform childRoot;
    [SerializeField]
    private GameObject childObject;
    [SerializeField]
    private CanvasGroup childCanvasGroup;
    [SerializeField]
    private Text messageText;
    [SerializeField]
    private float popupWidth;
    [SerializeField]
    private FadeInfo openInfo;
    [SerializeField]
    private FadeInfo closeInfo;
    private bool isActive;
    private CompositeDisposable openDisposable;
    private CompositeDisposable closeDisposable;
    private CompositeDisposable changeDisposable;

    public StorySupportUI()
    {
      base.\u002Ector();
    }

    public string ReceivedMessageText { get; set; }

    public int Index { get; private set; }

    private int LangIdx
    {
      get
      {
        return Singleton<GameSystem>.IsInstance() ? Singleton<GameSystem>.Instance.languageInt : 0;
      }
    }

    public bool IsActiveControl
    {
      get
      {
        return this.isActive;
      }
      set
      {
        if (!this.isActive && !value || this.isActive && value && this.messageText.get_text() == this.ReceivedMessageText)
          return;
        if (!value)
          this.StartClose();
        else
          this.StartChange();
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this.canvasGroup, (Object) null) ? this.canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this.canvasGroup, (Object) null))
          return;
        this.canvasGroup.set_alpha(value);
      }
    }

    public float ChildCanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this.childCanvasGroup, (Object) null) ? this.childCanvasGroup.get_alpha() : 0.0f;
      }
      private set
      {
        if (!Object.op_Inequality((Object) this.childCanvasGroup, (Object) null))
          return;
        this.childCanvasGroup.set_alpha(value);
      }
    }

    private Vector3 ChildLocalPosition
    {
      get
      {
        return ((Transform) this.childRoot).get_localPosition();
      }
      set
      {
        ((Transform) this.childRoot).set_localPosition(value);
      }
    }

    private void Dispose()
    {
      if (this.openDisposable != null)
        this.openDisposable.Dispose();
      if (this.closeDisposable != null)
        this.closeDisposable.Dispose();
      if (this.changeDisposable != null)
        this.changeDisposable.Dispose();
      this.openDisposable = (CompositeDisposable) null;
      this.closeDisposable = (CompositeDisposable) null;
      this.changeDisposable = (CompositeDisposable) null;
    }

    protected virtual void Start()
    {
      base.Start();
      if (Object.op_Equality((Object) this.childObject, (Object) null) && Object.op_Inequality((Object) this.childRoot, (Object) null) && Object.op_Inequality((Object) ((Component) this.childRoot).get_gameObject(), (Object) null))
        this.childObject = ((Component) this.childRoot).get_gameObject();
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), this.childObject), (System.Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnUpdate()
    {
      GameConfigSystem gameData = Manager.Config.GameData;
      if (gameData == null)
        return;
      this.SetActive(this.childObject, gameData.StoryHelp);
    }

    public void RefreshState()
    {
      GameConfigSystem gameData = Manager.Config.GameData;
      if (gameData == null || !Object.op_Inequality((Object) this.childObject, (Object) null))
        return;
      this.childObject.SetActive(gameData.StoryHelp);
    }

    public System.Action OpenedAction { get; set; }

    public System.Action ClosedAction { get; set; }

    public bool PlayingOpen
    {
      get
      {
        return this.openDisposable != null;
      }
    }

    public bool PlayingClose
    {
      get
      {
        return this.closeDisposable != null;
      }
    }

    public bool PlayingChange
    {
      get
      {
        return this.changeDisposable != null;
      }
    }

    public bool IsPlaying
    {
      get
      {
        return this.PlayingOpen || this.PlayingClose || this.PlayingChange;
      }
    }

    public void Open(AIProject.Definitions.Popup.StorySupport.Type _type)
    {
      ReadOnlyDictionary<int, string[]> storySupportTable = Singleton<Resources>.Instance.PopupInfo.StorySupportTable;
      this.Index = (int) _type;
      string[] source;
      if (!storySupportTable.TryGetValue(this.Index, ref source))
      {
        if (!this.IsActiveControl)
          return;
        this.IsActiveControl = false;
      }
      else
      {
        string self = source != null ? source.GetElement<string>(this.LangIdx) : (string) null;
        if (self.IsNullOrEmpty())
        {
          if (!this.IsActiveControl)
            return;
          this.IsActiveControl = false;
        }
        else
        {
          this.ReceivedMessageText = self;
          this.IsActiveControl = true;
        }
      }
    }

    public void Open()
    {
      this.Open(this.Index);
    }

    public void Open(int _id)
    {
      ReadOnlyDictionary<int, string[]> storySupportTable = Singleton<Resources>.Instance.PopupInfo.StorySupportTable;
      this.Index = _id;
      string[] source;
      if (!storySupportTable.TryGetValue(this.Index, ref source))
      {
        if (!this.IsActiveControl)
          return;
        this.IsActiveControl = false;
      }
      else
      {
        string self = source != null ? source.GetElement<string>(this.LangIdx) : (string) null;
        if (self.IsNullOrEmpty())
        {
          if (!this.IsActiveControl)
            return;
          this.IsActiveControl = false;
        }
        else
        {
          this.ReceivedMessageText = self;
          this.IsActiveControl = true;
        }
      }
    }

    public void SetIndexClose(int idx)
    {
      this.Index = idx;
      this.IsActiveControl = false;
    }

    public void Close()
    {
      this.IsActiveControl = false;
    }

    private void StartOpen()
    {
      this.Dispose();
      this.openDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.OpenCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this)), (ICollection<IDisposable>) this.openDisposable);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StorySupportUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void StartClose()
    {
      this.Dispose();
      this.closeDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.CloseCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this)), (ICollection<IDisposable>) this.closeDisposable);
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StorySupportUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void StartChange()
    {
      this.Dispose();
      this.changeDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.ChangeCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this)), (ICollection<IDisposable>) this.changeDisposable);
    }

    [DebuggerHidden]
    private IEnumerator ChangeCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StorySupportUI.\u003CChangeCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void PlaySE()
    {
      if (Mathf.Approximately(this.CanvasAlpha, 0.0f) || Object.op_Inequality((Object) this.childRoot, (Object) null) && Object.op_Inequality((Object) ((Component) this.childRoot).get_gameObject(), (Object) null) && !((Component) this.childRoot).get_gameObject().get_activeSelf() || !Singleton<Resources>.IsInstance())
        return;
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Popup);
    }

    private void SetActive(GameObject obj, bool active)
    {
      if (Object.op_Equality((Object) obj, (Object) null) || obj.get_activeSelf() == active)
        return;
      obj.SetActive(active);
    }
  }
}
