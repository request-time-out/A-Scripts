// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CountSettingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using ReMotion;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class CountSettingUI : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Button _addButton;
    [SerializeField]
    private Button _subButton;
    [SerializeField]
    private Button _submitButton;
    [SerializeField]
    private Button _cancelButton;
    [SerializeField]
    private Image _selectedIcon;
    [SerializeField]
    private OptimizedDropdown _dropdown;
    private int _count;
    private IDisposable _subscriber;

    public Button.ButtonClickedEvent OnSubmit
    {
      get
      {
        return this._submitButton.get_onClick();
      }
      set
      {
        this._submitButton.set_onClick(value);
      }
    }

    public Button.ButtonClickedEvent OnCancel
    {
      get
      {
        return this._cancelButton.get_onClick();
      }
      set
      {
        this._cancelButton.set_onClick(value);
      }
    }

    public Action OnClosed { get; set; }

    public Image SelectedIcon
    {
      get
      {
        return this._selectedIcon;
      }
    }

    public StuffItem Item { get; set; }

    public int Count
    {
      get
      {
        return this._count;
      }
      set
      {
        this._count = value;
        if (this._count < 1)
          this._count = 1;
        this.UpdateInteraction();
      }
    }

    private void UpdateInteraction()
    {
      ((Selectable) this._addButton).set_interactable(this._count < this.Item.Count);
      ((Selectable) this._subButton).set_interactable(this._count > 1);
    }

    protected override void Start()
    {
      // ISSUE: method pointer
      ((UnityEvent) this._addButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__0)));
      // ISSUE: method pointer
      ((UnityEvent) this._subButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__1)));
      // ISSUE: method pointer
      this._dropdown.OnValueChanged.AddListener(new UnityAction<int>((object) this, __methodptr(\u003CStart\u003Em__2)));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand2);
      base.Start();
    }

    public void Open()
    {
      this._dropdown.Options = Enumerable.Range(1, this.Item.Count).Select<int, OptimizedDropdown.OptionData>((Func<int, OptimizedDropdown.OptionData>) (x => new OptimizedDropdown.OptionData(x.ToString()))).ToList<OptimizedDropdown.OptionData>();
      this._canvasGroup.set_blocksRaycasts(true);
      if (this._subscriber != null)
        this._subscriber.Dispose();
      this._subscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.3f, true), true), (Action<M0>) (x => this._canvasGroup.set_alpha(((TimeInterval<float>) ref x).get_Value())), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() => this._canvasGroup.set_interactable(true)));
      this._count = 1;
    }

    public void Close()
    {
      this._canvasGroup.set_interactable(false);
      if (this._subscriber != null)
        this._subscriber.Dispose();
      this._subscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseInQuint(0.3f, true), true), (Action<M0>) (x => this._canvasGroup.set_alpha(1f - ((TimeInterval<float>) ref x).get_Value())), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() =>
      {
        this._canvasGroup.set_blocksRaycasts(false);
        Action onClosed = this.OnClosed;
        if (onClosed == null)
          return;
        onClosed();
      }));
      this._count = 0;
    }

    public override void OnInputMoveDirection(MoveDirection moveDir)
    {
      if (moveDir != null)
      {
        if (moveDir != 2 || !Object.op_Inequality((Object) this._addButton, (Object) null) || !((UIBehaviour) this._addButton).IsActive() && !((Selectable) this._addButton).IsInteractable())
          return;
        ((UnityEvent) this._addButton.get_onClick()).Invoke();
      }
      else
      {
        if (!Object.op_Inequality((Object) this._subButton, (Object) null) || !((UIBehaviour) this._subButton).IsActive() && !((Selectable) this._subButton).IsInteractable())
          return;
        ((UnityEvent) this._subButton.get_onClick()).Invoke();
      }
    }

    private void OnInputSubmit()
    {
      if (!Object.op_Inequality((Object) this._submitButton, (Object) null) || !((UIBehaviour) this._submitButton).IsActive() || !((Selectable) this._submitButton).IsInteractable())
        return;
      ((UnityEvent) this._submitButton.get_onClick()).Invoke();
    }

    private void OnInputCancel()
    {
      if (!Object.op_Inequality((Object) this._cancelButton, (Object) null) || !((UIBehaviour) this._cancelButton).IsActive() || !((Selectable) this._cancelButton).IsInteractable())
        return;
      ((UnityEvent) this._cancelButton.get_onClick()).Invoke();
    }
  }
}
