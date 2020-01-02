// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ChickenNameChangeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ChickenNameChangeUI : MenuUIBehaviour
  {
    [Header("Infomation Setting")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Button _enterButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private InputField _nameInputField;
    private IDisposable _fadeDisposable;

    public event Action OnSubmit;

    public event Action OnCancel;

    private AIProject.SaveData.Environment.ChickenInfo _info { get; set; }

    public bool isOpen
    {
      get
      {
        return this.IsActiveControl;
      }
    }

    public virtual void Open(AIProject.SaveData.Environment.ChickenInfo info)
    {
      this.IsActiveControl = true;
      this._info = info;
      this._nameInputField.set_text(info.name);
    }

    public virtual void Close()
    {
      if (!this.isOpen)
        return;
      this.IsActiveControl = false;
    }

    public virtual void Refresh()
    {
    }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      if (Object.op_Inequality((Object) this._enterButton, (Object) null))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._enterButton), (Action<M0>) (_ => this.OnInputSubmit()));
      if (Object.op_Inequality((Object) this._closeButton, (Object) null))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Action<M0>) (_ => this.OnInputCancel()));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.SquareX
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
      this._actionCommands.Add(actionIdDownCommand3);
      ActionIDDownCommand actionIdDownCommand4 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand4.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__6)));
      this._actionCommands.Add(actionIdDownCommand4);
      base.Start();
    }

    protected virtual void OnDestroy()
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = (IDisposable) null;
    }

    private void SetActiveControl(bool isActive)
    {
      IEnumerator coroutine = !isActive ? this.DoClose() : this.DoOpen();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChickenNameChangeUI.\u003CDoOpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChickenNameChangeUI.\u003CDoClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void OnInputSubmit()
    {
      if (!this.isOpen)
        return;
      if (this._info != null)
        this._info.name = this._nameInputField.get_text();
      this._info = (AIProject.SaveData.Environment.ChickenInfo) null;
      this._nameInputField.set_text(string.Empty);
      if (this.OnSubmit != null)
        this.OnSubmit();
      this.Close();
    }

    private void OnInputCancel()
    {
      if (!this.isOpen)
        return;
      this.Close();
      if (this.OnCancel == null)
        return;
      this.OnCancel();
    }
  }
}
