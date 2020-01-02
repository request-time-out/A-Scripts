// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CommCommandList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx.Misc;

namespace AIProject.UI
{
  public class CommCommandList : MenuUIBehaviour
  {
    private bool _visibled = true;
    [SerializeField]
    private IntReactiveProperty _selectedID = new IntReactiveProperty(0);
    [SerializeField]
    private Color _normalTimerColor = Color.get_white();
    [SerializeField]
    private Color _redZoneTimerColor = Color.get_white();
    private List<CommCommandList.CommandInfo> _commandList = new List<CommCommandList.CommandInfo>();
    private List<ScrollCylinderNode> _nodeList = new List<ScrollCylinderNode>();
    private List<CommCommandOption> _optionList = new List<CommCommandOption>();
    private IDisposable _fadeSubscriber;
    private MenuUIBehaviour[] _menuUIElements;
    private IDisposable _refreshDisposable;
    private IConnectableObservable<int> _selectIDChange;
    [SerializeField]
    private ScrollCylinder _scrollCylinder;
    [SerializeField]
    private Text _label;
    [SerializeField]
    private RectTransform _guideRoot;
    [SerializeField]
    private RectTransform _poolRoot;
    [SerializeField]
    private ScrollCylinderNode _node;
    [SerializeField]
    private GuideOption _guideNode;
    [SerializeField]
    private Sprite _normalSprite;
    [SerializeField]
    private Sprite _selectedSprite;
    [SerializeField]
    private Sprite _specialSelectedSprite;
    private CommCommandList.CommandInfo _selectedCommand;
    private CommCommandOption _selectedOption;
    private Action _cancelEvent;
    private CommCommandList.CommandInfo[] _options;
    private CommCommandList.NodePool _nodePool;
    [SerializeField]
    private CanvasGroup _panelCanvasGroup;
    private GuideOption _okGuide;
    private GuideOption _cancelGuide;
    private GuideOption _scrollGuide;
    private Subject<Unit> _completeDoStop;

    private void SetActiveControl(bool isActive)
    {
      IEnumerator coroutine = !isActive ? this.Close() : this.Open();
      if (this._fadeSubscriber != null)
        this._fadeSubscriber.Dispose();
      this._fadeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    public Action OnOpened { get; set; }

    private MenuUIBehaviour[] MenuUIElements
    {
      get
      {
        MenuUIBehaviour[] menuUiElements = this._menuUIElements;
        if (menuUiElements != null)
          return menuUiElements;
        return this._menuUIElements = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this
        };
      }
    }

    public bool Visibled
    {
      get
      {
        return this._visibled;
      }
      set
      {
        if (this._visibled == value)
          return;
        this._visibled = value;
        if (this._fadeSubscriber != null)
          this._fadeSubscriber.Dispose();
        IEnumerator coroutine = !value ? this.Vanish(this.CanvasGroup) : this.Display(this.CanvasGroup);
        this._fadeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
      }
    }

    public IObservable<int> OnSelectIDChagnedAsObservable()
    {
      if (this._selectIDChange == null)
      {
        this._selectIDChange = (IConnectableObservable<int>) Observable.Publish<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._selectedID, ((Component) this).get_gameObject()));
        this._selectIDChange.Connect();
      }
      return (IObservable<int>) this._selectIDChange;
    }

    public Text Label
    {
      get
      {
        return this._label;
      }
    }

    public Action CancelEvent
    {
      get
      {
        return this._cancelEvent;
      }
      set
      {
        if (this._cancelEvent == value)
          return;
        this._cancelEvent = value;
        UnityExtensions.SetActive((Component) this._cancelGuide, value != null);
      }
    }

    public CommCommandList.CommandInfo[] Options
    {
      get
      {
        return this._options;
      }
      set
      {
        this._options = ((IEnumerable<CommCommandList.CommandInfo>) value).ToArray<CommCommandList.CommandInfo>();
        this._commandList.Clear();
        foreach (CommCommandList.CommandInfo option in this._options)
        {
          Func<bool> condition = option.Condition;
          bool? nullable1;
          bool? nullable2;
          if (condition == null)
          {
            nullable1 = new bool?();
            nullable2 = nullable1;
          }
          else
            nullable2 = new bool?(condition());
          nullable1 = nullable2;
          if ((!nullable1.HasValue ? 1 : (nullable1.Value ? 1 : 0)) != 0)
            this._commandList.Add(option);
        }
      }
    }

    public int ID { get; set; }

    public CanvasGroup CanvasGroup { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      this.CanvasGroup = (CanvasGroup) ((Component) this).GetComponentInChildren<CanvasGroup>(true);
    }

    protected override void OnBeforeStart()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CommCommandList.\u003COnBeforeStart\u003Ec__AnonStorey9 startCAnonStorey9 = new CommCommandList.\u003COnBeforeStart\u003Ec__AnonStorey9();
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey9.\u0024this = this;
      if (!Application.get_isPlaying())
        return;
      this._scrollCylinder.enableInternalScroll = false;
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) new Action<bool>(startCAnonStorey9.\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.OnSelectIDChagnedAsObservable(), (Action<M0>) new Action<int>(startCAnonStorey9.\u003C\u003Em__1));
      IConnectableObservable<long> iconnectableObservable = (IConnectableObservable<long>) Observable.Publish<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()));
      iconnectableObservable.Connect();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<long>(Observable.Where<long>((IObservable<M0>) iconnectableObservable, (Func<M0, bool>) new Func<long, bool>(startCAnonStorey9.\u003C\u003Em__2)), (Action<M0>) new Action<long>(startCAnonStorey9.\u003C\u003Em__3));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) startCAnonStorey9, __methodptr(\u003C\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) startCAnonStorey9, __methodptr(\u003C\u003Em__5)));
      this._actionCommands.Add(actionIdDownCommand2);
      this._nodePool = new CommCommandList.NodePool()
      {
        Source = ((Component) this._node).get_gameObject()
      };
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey9.iconTable = Singleton<Resources>.Instance.itemIconTables;
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey9.commonDefine = Singleton<Resources>.Instance.CommonDefine;
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Singleton<Resources>.Instance.LoadMapResourceStream, (Action<M0>) new Action<Unit>(startCAnonStorey9.\u003C\u003Em__6));
    }

    private void OnUpdate()
    {
      if (Object.op_Inequality((Object) this._guideRoot, (Object) null))
        ((Component) this._guideRoot).get_gameObject().SetActiveIfDifferent(Manager.Config.GameData.ActionGuide);
      if (!Singleton<Game>.IsInstance() || !Singleton<Input>.IsInstance())
        return;
      Input instance1 = Singleton<Input>.Instance;
      Game instance2 = Singleton<Game>.Instance;
      if (Object.op_Inequality((Object) instance2.ExitScene, (Object) null) || Object.op_Inequality((Object) instance2.Dialog, (Object) null) || (Object.op_Inequality((Object) instance2.Config, (Object) null) || Object.op_Inequality((Object) instance2.MapShortcutUI, (Object) null)) || (!this.EnabledInput || this.FocusLevel != Singleton<Input>.Instance.FocusLevel))
        return;
      float num1 = Singleton<Input>.Instance.ScrollValue();
      if ((double) num1 < 0.0)
      {
        int num2 = ((ReactiveProperty<int>) this._selectedID).get_Value() + 1;
        if (num2 < this._nodeList.Count)
        {
          ((ReactiveProperty<int>) this._selectedID).set_Value(num2);
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
        }
      }
      else if ((double) num1 > 0.0)
      {
        int num2 = ((ReactiveProperty<int>) this._selectedID).get_Value() - 1;
        if (num2 >= 0)
        {
          ((ReactiveProperty<int>) this._selectedID).set_Value(num2);
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
        }
      }
      if (!Object.op_Inequality((Object) this._selectedOption, (Object) null) || this._selectedCommand == null)
        return;
      foreach (CommCommandOption option in this._optionList)
      {
        if (Object.op_Equality((Object) option, (Object) this._selectedOption))
        {
          option.SetActiveTimer(this._selectedCommand.Timer != null);
          if (option.ActiveTimer)
          {
            float time = this._selectedCommand.Timer();
            Color color = (double) time <= (double) this._selectedCommand.TimerRedZoneBorder ? this._redZoneTimerColor : this._normalTimerColor;
            option.SetTimeColor(color);
            option.SetTime(time);
          }
        }
        else
          option.SetActiveTimer(false);
      }
    }

    [DebuggerHidden]
    private IEnumerator Open()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommCommandList.\u003COpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DisplayElement(CanvasGroup canvasGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommCommandList.\u003CDisplayElement\u003Ec__Iterator1()
      {
        canvasGroup = canvasGroup,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator Close()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommCommandList.\u003CClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator VanishElement(CanvasGroup canvasGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommCommandList.\u003CVanishElement\u003Ec__Iterator3()
      {
        canvasGroup = canvasGroup
      };
    }

    [DebuggerHidden]
    private IEnumerator Display(CanvasGroup canvasGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommCommandList.\u003CDisplay\u003Ec__Iterator4()
      {
        canvasGroup = canvasGroup,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator Vanish(CanvasGroup canvasGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommCommandList.\u003CVanish\u003Ec__Iterator5()
      {
        canvasGroup = canvasGroup,
        \u0024this = this
      };
    }

    public IObservable<Unit> OnCompletedStopAsObservable()
    {
      return (IObservable<Unit>) this._completeDoStop ?? (IObservable<Unit>) (this._completeDoStop = new Subject<Unit>());
    }

    public override void OnInputMoveDirection(MoveDirection moveDir)
    {
      if (moveDir != 3)
      {
        if (moveDir != 1)
          return;
        int num = ((ReactiveProperty<int>) this._selectedID).get_Value() - 1;
        if (num < 0)
          return;
        ((ReactiveProperty<int>) this._selectedID).set_Value(num);
      }
      else
      {
        int num = ((ReactiveProperty<int>) this._selectedID).get_Value() + 1;
        if (num >= this._nodeList.Count)
          return;
        ((ReactiveProperty<int>) this._selectedID).set_Value(num);
      }
    }

    private void OnInputSubmit()
    {
      if (this._selectedCommand == null)
        return;
      Action<int> action = this._selectedCommand.Event;
      if (action == null)
        return;
      action(this.ID);
    }

    private void OnInputSubmitFromButton(CommCommandOption option, int optionID)
    {
      if (Object.op_Equality((Object) option, (Object) this._selectedOption))
      {
        if (this._selectedCommand == null)
          return;
        Action<int> action = this._selectedCommand.Event;
        if (action == null)
          return;
        action(this.ID);
      }
      else
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
        ((ReactiveProperty<int>) this._selectedID).set_Value(optionID);
      }
    }

    private void OnInputCancel()
    {
      Action cancelEvent = this.CancelEvent;
      if (cancelEvent == null)
        return;
      cancelEvent();
    }

    public void Refresh(CommCommandList.CommandInfo[] options, Action onCompleted = null)
    {
      if (this._refreshDisposable != null)
        this._refreshDisposable.Dispose();
      this._refreshDisposable = ObservableExtensions.Subscribe<Unit>(Observable.DoOnCompleted<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.Reopen(options, this._panelCanvasGroup)), false), (Action) (() =>
      {
        Action action = onCompleted;
        if (action == null)
          return;
        action();
      })));
    }

    public void Refresh(CommCommandList.CommandInfo[] options, CanvasGroup cg, Action onCompleted = null)
    {
      if (this._refreshDisposable != null)
        this._refreshDisposable.Dispose();
      this._refreshDisposable = ObservableExtensions.Subscribe<Unit>(Observable.DoOnCompleted<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.Reopen(options, cg)), false), (Action) (() =>
      {
        Action action = onCompleted;
        if (action == null)
          return;
        action();
      })));
    }

    [DebuggerHidden]
    private IEnumerator Reopen(
      CommCommandList.CommandInfo[] options,
      CanvasGroup canvasGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommCommandList.\u003CReopen\u003Ec__Iterator6()
      {
        canvasGroup = canvasGroup,
        options = options,
        \u0024this = this
      };
    }

    public class NodePool : ObjectPool<ScrollCylinderNode>
    {
      public NodePool()
      {
        base.\u002Ector();
      }

      public GameObject Source { get; set; }

      protected virtual ScrollCylinderNode CreateInstance()
      {
        return (ScrollCylinderNode) ((GameObject) Object.Instantiate<GameObject>((M0) this.Source)).GetComponent<ScrollCylinderNode>();
      }
    }

    [Serializable]
    public class CommandInfo
    {
      public CommandInfo()
      {
      }

      public CommandInfo(string name)
      {
        this.Name = name;
      }

      public CommandInfo(string name, Func<bool> cond, Action<int> action)
      {
        this.Name = name;
        this.Condition = cond;
        this.Event = action;
      }

      public string Name { get; set; }

      public Func<bool> Condition { get; set; }

      public Action<int> Event { get; set; }

      public bool IsSpecial { get; set; }

      public Func<float> Timer { get; set; }

      public int TimerRedZoneBorder { get; set; }
    }
  }
}
