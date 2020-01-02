// Decompiled with JetBrains decompiler
// Type: AIProject.UI.AudioSortUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (CanvasGroup))]
  [RequireComponent(typeof (RectTransform))]
  public class AudioSortUI : MenuUIBehaviour
  {
    private Toggle[] _toggles = new Toggle[0];
    private CompositeDisposable _allDisposable = new CompositeDisposable();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private ToggleGroup _toggleGroup;
    [SerializeField]
    private JukeBoxUI _mainUI;
    [SerializeField]
    private JukeBoxAudioListUI _listUI;

    public Toggle[] Toggles
    {
      get
      {
        return this._toggles;
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    public bool InputEnabled
    {
      get
      {
        return this.EnabledInput && this._focusLevel == Singleton<Input>.Instance.FocusLevel;
      }
    }

    public Action<int> ToggleIndexChanged { get; set; }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        this._canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (!Object.op_Equality((Object) this._rectTransform, (Object) null))
        return;
      this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
    }

    protected override void OnBeforeStart()
    {
      base.OnBeforeStart();
      ObservableExtensions.Subscribe<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Component) this), (Action<M0>) (x => this.SetActiveControl(x)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.DoClose()));
      this._toggles = this._toggleGroup != null ? (Toggle[]) ((Component) this._toggleGroup).GetComponentsInChildren<Toggle>(true) : (Toggle[]) null;
      if (((IReadOnlyList<Toggle>) this._toggles).IsNullOrEmpty<Toggle>())
        return;
      for (int i = 0; i < this._toggles.Length; ++i)
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this._toggles[i]), (Func<M0, bool>) (flag => flag)), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.ChangeIndex(i)));
    }

    public int SortIndex()
    {
      if (!((IReadOnlyList<Toggle>) this._toggles).IsNullOrEmpty<Toggle>())
      {
        for (int index = 0; index < this._toggles.Length; ++index)
        {
          Toggle toggle = this._toggles[index];
          if (Object.op_Inequality((Object) toggle, (Object) null) && toggle.get_isOn())
            return index;
        }
      }
      return 0;
    }

    protected override void OnAfterStart()
    {
      base.OnAfterStart();
      this.Hide();
      ((Component) this).get_gameObject().SetActiveSelf(false);
    }

    public void Hide()
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        return;
      this._canvasGroup.SetBlocksRaycasts(false);
      this._canvasGroup.SetInteractable(false);
      this._canvasGroup.set_alpha(0.0f);
    }

    private void DoClose()
    {
      this.IsActiveControl = false;
    }

    private void SetActiveControl(bool active)
    {
      this._allDisposable.Clear();
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Component) this)), (ICollection<IDisposable>) this._allDisposable);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AudioSortUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AudioSortUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void ChangeIndex(int index)
    {
      Action<int> toggleIndexChanged = this.ToggleIndexChanged;
      if (toggleIndexChanged == null)
        return;
      toggleIndexChanged(index);
    }

    private void PlaySE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }

    private delegate void ChangedFunk(int index);
  }
}
