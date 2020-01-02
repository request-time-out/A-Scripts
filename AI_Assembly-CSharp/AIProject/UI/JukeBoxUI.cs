// Decompiled with JetBrains decompiler
// Type: AIProject.UI.JukeBoxUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class JukeBoxUI : MenuUIBehaviour
  {
    private static bool First = true;
    private int _prevFocusLevel = -1;
    private float _prevTimeScale = 1f;
    private CompositeDisposable _allDisposable = new CompositeDisposable();
    [SerializeField]
    private RectTransform _myTransform;
    [SerializeField]
    private CanvasGroup _myCanvas;
    [SerializeField]
    private CanvasGroup _backCanvasGroup;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Text _audioNameText;
    [SerializeField]
    private Button _changeButton;
    [SerializeField]
    private JukeBoxAudioListUI _listUI;
    [SerializeField]
    private AudioSortUI _sortUI;
    private MenuUIBehaviour[] _menuUIBehaviors;
    private IDisposable _fadeDisposable;

    public RectTransform MyTransform
    {
      get
      {
        return this._myTransform;
      }
    }

    public CanvasGroup MyCanvas
    {
      get
      {
        return this._myCanvas;
      }
    }

    public CanvasGroup BackCanvasGroup
    {
      get
      {
        return this._backCanvasGroup;
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    public float MyCanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._myCanvas, (Object) null) ? this._myCanvas.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this._myCanvas, (Object) null))
          return;
        this._myCanvas.set_alpha(value);
      }
    }

    public bool InputEnabled
    {
      get
      {
        return this.EnabledInput && this._focusLevel == Singleton<Input>.Instance.FocusLevel;
      }
    }

    public Action ClosedAction { get; set; }

    public JukePoint CurrentJukePoint { get; private set; }

    public MenuUIBehaviour[] MenuUIBehaviors
    {
      get
      {
        if (this._menuUIBehaviors == null)
          this._menuUIBehaviors = new MenuUIBehaviour[3]
          {
            (MenuUIBehaviour) this,
            (MenuUIBehaviour) this._listUI,
            (MenuUIBehaviour) this._sortUI
          };
        return this._menuUIBehaviors;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._myCanvas, (Object) null))
        this._myCanvas = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (Object.op_Equality((Object) this._myTransform, (Object) null))
        this._myTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      if (!JukeBoxUI.First)
        return;
      if (!System.IO.Directory.Exists(SoundPlayer.Directory.AudioFile))
        System.IO.Directory.CreateDirectory(SoundPlayer.Directory.AudioFile);
      JukeBoxUI.First = false;
    }

    protected override void OnBeforeStart()
    {
      base.OnBeforeStart();
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      this._actionCommands.Add(actionIdDownCommand);
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      this._keyCommands.Add(keyCodeDownCommand);
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.DoClose()));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._changeButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this._listUI.IsActiveControl = !this._listUI.IsActiveControl));
    }

    protected override void OnAfterStart()
    {
      base.OnAfterStart();
      this.Off();
    }

    private void SetActiveControl(bool active)
    {
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      if (this._allDisposable != null)
        this._allDisposable.Clear();
      this._fadeDisposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (ICollection<IDisposable>) this._allDisposable);
    }

    private void DoClose()
    {
      this.PlaySystemSE(SoundPack.SystemSE.Cancel);
      this.IsActiveControl = false;
    }

    private void Off()
    {
      this.SetBlockRaycast(false);
      this.SetInteractable(false);
      this.SetAllEnableInput(false);
      this.SetAllFocusLevel(99);
      ((Component) this).get_gameObject().SetActiveSelf(false);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new JukeBoxUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new JukeBoxUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void SetAudioName(string fileName, string fileNameWithExtension)
    {
      this._audioNameText.set_text(fileName ?? this._listUI.NoneStr);
      int index1 = !Singleton<Manager.Map>.IsInstance() ? 0 : Singleton<Manager.Map>.Instance.MapID;
      int? areaId = this.CurrentJukePoint?.AreaID;
      int index2 = !areaId.HasValue ? -1 : areaId.Value;
      Dictionary<int, string> dictionary1 = (Dictionary<int, string>) null;
      if (Singleton<Game>.IsInstance())
      {
        if (index1 == 0)
        {
          dictionary1 = Singleton<Game>.Instance.Environment?.JukeBoxAudioNameTable;
        }
        else
        {
          Dictionary<int, Dictionary<int, string>> boxAudioNameTable = Singleton<Game>.Instance.Environment?.AnotherJukeBoxAudioNameTable;
          if (boxAudioNameTable != null && (!boxAudioNameTable.TryGetValue(index1, out dictionary1) || dictionary1 == null))
          {
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            boxAudioNameTable[index1] = dictionary2;
            dictionary1 = dictionary2;
          }
        }
      }
      if (dictionary1 == null)
        return;
      bool flag = false;
      if (fileNameWithExtension.IsNullOrEmpty())
      {
        if (dictionary1.ContainsKey(index2))
        {
          dictionary1.Remove(index2);
          flag = true;
        }
      }
      else
      {
        flag = !dictionary1.ContainsKey(index2) || dictionary1[index2] != fileNameWithExtension;
        dictionary1[index2] = fileNameWithExtension;
      }
      if (!flag || !Singleton<SoundPlayer>.IsInstance())
        return;
      Singleton<SoundPlayer>.Instance.RemoveAreaAudioClip(index1, index2);
    }

    private void SetBlockRaycast(bool active)
    {
      this._myCanvas.SetBlocksRaycasts(active);
      this._backCanvasGroup.SetBlocksRaycasts(active);
    }

    private void SetInteractable(bool active)
    {
      this._myCanvas.SetInteractable(active);
      this._backCanvasGroup.SetInteractable(active);
    }

    private void SetAllEnableInput(bool active)
    {
      foreach (MenuUIBehaviour menuUiBehavior in this.MenuUIBehaviors)
        menuUiBehavior.EnabledInput = active;
    }

    private void SetAllFocusLevel(int level)
    {
      foreach (MenuUIBehaviour menuUiBehavior in this.MenuUIBehaviors)
        menuUiBehavior.SetFocusLevel(level);
    }

    private void PlaySystemSE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }
  }
}
