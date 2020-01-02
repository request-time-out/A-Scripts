// Decompiled with JetBrains decompiler
// Type: AIProject.CommandLabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI;
using Illusion.Extensions;
using Manager;
using ReMotion;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject
{
  public class CommandLabel : MonoBehaviour, IActionCommand
  {
    [SerializeField]
    private Transform _labelRoot;
    [SerializeField]
    private Transform _subjectRoot;
    [SerializeField]
    private CanvasGroup _optionsCanvasGroup;
    [SerializeField]
    private float _holdDuration;
    [SerializeField]
    private GameObject _labelNode;
    [SerializeField]
    private GameObject _subjectLabelNode;
    private List<CommandLabel.CommandInfo> _commands;
    private CommandLabel.CommandInfo[] _objectCommands;
    private List<CommandLabelOption> _commandLabels;
    private CommandLabel.LabelPool _objectPool;
    private CommandLabel.LabelPool _subjectPool;
    private List<ActionIDCommand> _actionCommands;
    private List<ActionIDDownCommand> _actionTriggerCommands;
    private CommandLabel.CommandInfo _cancelCommand;
    private int _commandID;
    private CommandLabel.AcceptionState _acception;
    private IDisposable _disposable;
    private bool _isPressedAction;
    private float _holdElapsedTime;

    public CommandLabel()
    {
      base.\u002Ector();
    }

    public CommandLabel.CommandInfo SubjectCommand { get; set; }

    public CommandLabel.CommandInfo[] ObjectCommands
    {
      get
      {
        return this._objectCommands;
      }
      set
      {
        this._objectCommands = value;
        this.RefreshCommands();
      }
    }

    public void RefreshCommands()
    {
      foreach (CommandLabelOption commandLabel in this._commandLabels)
        commandLabel.Visibility = 0;
      this._commandLabels.Clear();
      this._commands.Clear();
      if (this._acception == CommandLabel.AcceptionState.InvokeAcception)
      {
        if (this._objectCommands.IsNullOrEmpty<CommandLabel.CommandInfo>() && this.SubjectCommand != null)
          this._commands.Add(this.SubjectCommand);
        for (int index = 0; index < this._objectCommands.Length; ++index)
          this._commands.Add(this._objectCommands[index]);
        List<CommandLabel.CommandInfo> commands = this._commands;
        // ISSUE: reference to a compiler-generated field
        if (CommandLabel.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CommandLabel.\u003C\u003Ef__mg\u0024cache0 = new Comparison<CommandLabel.CommandInfo>(CommandLabel.CompareDistance);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<CommandLabel.CommandInfo> fMgCache0 = CommandLabel.\u003C\u003Ef__mg\u0024cache0;
        commands.Sort(fMgCache0);
        for (int index = 0; index < this._commands.Count; ++index)
        {
          CommandLabel.CommandInfo command = this._commands[index];
          if (Object.op_Inequality((Object) command.Transform, (Object) null))
          {
            CommandLabelOption commandLabelOption = this._objectPool.Rent();
            ((Component) commandLabelOption).get_transform().SetParent(this._labelRoot, false);
            ((Component) commandLabelOption).get_transform().set_localPosition(Vector3.get_zero());
            commandLabelOption.CommandInfo = command;
            this._commandLabels.Add(commandLabelOption);
          }
          else
          {
            CommandLabelOption commandLabelOption = this._subjectPool.Rent();
            if (Object.op_Inequality((Object) ((Component) commandLabelOption).get_transform().get_parent(), (Object) this._subjectRoot))
              ((Component) commandLabelOption).get_transform().SetParent(this._subjectRoot, false);
            commandLabelOption.CommandInfo = command;
            this._commandLabels.Add(commandLabelOption);
          }
        }
        this.CommandID = 0;
      }
      else
      {
        if (this._acception != CommandLabel.AcceptionState.CancelAcception)
          return;
        if (this._cancelCommand != null)
        {
          CommandLabelOption commandLabelOption = this._subjectPool.Rent();
          if (Object.op_Inequality((Object) ((Component) commandLabelOption).get_transform().get_parent(), (Object) this._subjectRoot))
            ((Component) commandLabelOption).get_transform().SetParent(this._subjectRoot, false);
          commandLabelOption.CommandInfo = this._cancelCommand;
          this._commandLabels.Add(commandLabelOption);
        }
        this.CommandID = 0;
      }
    }

    private static int CompareDistance(CommandLabel.CommandInfo a, CommandLabel.CommandInfo b)
    {
      if (Singleton<Manager.Map>.IsInstance())
      {
        Vector3 position = Singleton<Manager.Map>.Instance.Player.Position;
        float num1 = Vector3.Distance(a.Position, position);
        float num2 = Vector3.Distance(b.Position, position);
        if ((double) num1 > (double) num2)
          return 1;
        if ((double) num2 < (double) num1)
          return -1;
      }
      return 0;
    }

    public CommandLabel.CommandInfo CancelCommand
    {
      get
      {
        return this._cancelCommand;
      }
      set
      {
        this._cancelCommand = value;
        this.RefreshCommands();
      }
    }

    private int CommandID
    {
      get
      {
        return this._commandID;
      }
      set
      {
        this._commandID = value;
        if (this._commandID >= this._commands.Count)
          this._commandID = 0;
        else if (this._commandID < 0)
          this._commandID = this._commands.Count - 1;
        if (this._commandLabels.Count == 0)
          return;
        CommandLabelOption commandLabelOption = this._commandLabels != null ? this._commandLabels.GetElement<CommandLabelOption>(this._commandID) : (CommandLabelOption) null;
        commandLabelOption.Visibility = 2;
        foreach (CommandLabelOption commandLabel in this._commandLabels)
        {
          if (!Object.op_Equality((Object) commandLabel, (Object) commandLabelOption))
            commandLabel.Visibility = 1;
        }
      }
    }

    public Tuple<CommandType, int> EventTypeTuple { get; set; }

    public CommandLabel.AcceptionState Acception
    {
      get
      {
        return this._acception;
      }
      set
      {
        this._acception = value;
        if (this._disposable != null)
          this._disposable.Dispose();
        float startValueA = this._optionsCanvasGroup.get_alpha();
        switch (value)
        {
          case CommandLabel.AcceptionState.InvokeAcception:
            this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.5f, true), true), (Action<M0>) (x => this._optionsCanvasGroup.set_alpha(Mathf.Lerp(startValueA, 1f, ((TimeInterval<float>) ref x).get_Value()))), (Action) (() => this._disposable = (IDisposable) null));
            break;
          case CommandLabel.AcceptionState.CancelAcception:
            this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.5f, true), true), (Action<M0>) (x => this._optionsCanvasGroup.set_alpha(Mathf.Lerp(startValueA, 0.0f, ((TimeInterval<float>) ref x).get_Value()))), (Action) (() => this._disposable = (IDisposable) null));
            break;
          case CommandLabel.AcceptionState.None:
            this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.5f, true), true), (Action<M0>) (x => this._optionsCanvasGroup.set_alpha(Mathf.Lerp(startValueA, 0.0f, ((TimeInterval<float>) ref x).get_Value()))), (Action) (() => this._disposable = (IDisposable) null));
            break;
        }
      }
    }

    public bool EnabledInput { get; set; }

    private void Start()
    {
      this._objectPool.Source = this._labelNode;
      this._subjectPool.Source = this._subjectLabelNode;
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
      ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
      {
        ActionID = ActionID.Action
      };
      // ISSUE: method pointer
      actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      this._actionTriggerCommands.Add(actionIdDownCommand);
      ActionIDCommand actionIdCommand = new ActionIDCommand()
      {
        ActionID = ActionID.Action
      };
      // ISSUE: method pointer
      actionIdCommand.TriggerEvent.AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdCommand);
    }

    private void OnUpdate()
    {
      ((Component) this._subjectRoot).get_gameObject().SetActiveIfDifferent(Manager.Config.GameData.ActionGuide);
      if (this._commands.IsNullOrEmpty<CommandLabel.CommandInfo>())
        return;
      this._commands[this._commandID].FillRate = Mathf.Clamp01(!Mathf.Approximately(this._holdDuration, 0.0f) ? this._holdElapsedTime / this._holdDuration : 0.0f);
    }

    public void NextCommandID()
    {
      ++this.CommandID;
    }

    public void PrevCommandID()
    {
      --this.CommandID;
    }

    public void OnUpdateInput()
    {
      switch (this._acception)
      {
        case CommandLabel.AcceptionState.InvokeAcception:
          if (this._commands.IsNullOrEmpty<CommandLabel.CommandInfo>())
            return;
          break;
        case CommandLabel.AcceptionState.CancelAcception:
          if (this._cancelCommand == null)
            return;
          break;
        case CommandLabel.AcceptionState.None:
          return;
      }
      if (!Singleton<Input>.IsInstance())
        return;
      Input instance = Singleton<Input>.Instance;
      foreach (CommandDataBase actionTriggerCommand in this._actionTriggerCommands)
        actionTriggerCommand.Invoke(instance);
      foreach (CommandDataBase actionCommand in this._actionCommands)
        actionCommand.Invoke(instance);
      if (this._acception != CommandLabel.AcceptionState.InvokeAcception)
        return;
      float num = instance.ScrollValue();
      if ((double) num > 0.0)
      {
        this.PrevCommandID();
      }
      else
      {
        if ((double) num >= 0.0)
          return;
        this.NextCommandID();
      }
    }

    private void OnInputAction(bool isDown)
    {
      if (this._acception == CommandLabel.AcceptionState.None || this._disposable != null)
        return;
      if (isDown)
      {
        switch (this._acception)
        {
          case CommandLabel.AcceptionState.InvokeAcception:
            CommandLabel.CommandInfo element = this._commands.GetElement<CommandLabel.CommandInfo>(this._commandID);
            if (element == null)
              break;
            if (element.IsHold)
            {
              if ((double) this._holdElapsedTime < (double) this._holdDuration && this._isPressedAction && (Singleton<Manager.Map>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null)) && this._acception == CommandLabel.AcceptionState.InvokeAcception)
                this._holdElapsedTime += Time.get_deltaTime();
              if ((double) this._holdElapsedTime < (double) this._holdDuration)
                break;
              PlayerActor player = Singleton<Manager.Map>.Instance.Player;
              Func<PlayerActor, bool> condition = element.Condition;
              bool? nullable = condition != null ? new bool?(condition(player)) : new bool?();
              if ((!nullable.HasValue ? 1 : (nullable.Value ? 1 : 0)) != 0)
              {
                Action action = element.Event;
                if (action != null)
                  action();
              }
              else if (element.ErrorText != null)
                MapUIContainer.PushMessageUI(element.ErrorText(player), 2, 0, (Action) null);
              this._holdElapsedTime = 0.0f;
              this._isPressedAction = false;
              break;
            }
            if (!this._isPressedAction || !Singleton<Manager.Map>.IsInstance() || !Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null))
              break;
            PlayerActor player1 = Singleton<Manager.Map>.Instance.Player;
            Func<PlayerActor, bool> condition1 = element.Condition;
            bool? nullable1 = condition1 != null ? new bool?(condition1(player1)) : new bool?();
            if ((!nullable1.HasValue ? 1 : (nullable1.Value ? 1 : 0)) != 0)
            {
              Action action = element.Event;
              if (action != null)
                action();
            }
            else if (element.ErrorText != null)
              MapUIContainer.PushMessageUI(element.ErrorText(player1), 2, 0, (Action) null);
            this._isPressedAction = false;
            break;
          case CommandLabel.AcceptionState.CancelAcception:
            if (this._cancelCommand == null)
              break;
            Debug.Log((object) "Run CancelCommand");
            Action action1 = this._cancelCommand.Event;
            if (action1 != null)
              action1();
            this._cancelCommand = (CommandLabel.CommandInfo) null;
            break;
        }
      }
      else
      {
        this._isPressedAction = false;
        this._holdElapsedTime = 0.0f;
      }
    }

    private void OnInputLeftShoulder()
    {
      this.PrevCommandID();
    }

    private void OnInputRightShoulder()
    {
      this.NextCommandID();
    }

    public enum AcceptionState
    {
      InvokeAcception,
      CancelAcception,
      None,
    }

    public class CommandInfo
    {
      public EventType EventType { get; set; }

      public Func<string> OnText { get; set; }

      public string Text { get; set; }

      public Sprite Icon { get; set; }

      public CommandTargetSpriteInfo TargetSpriteInfo { get; set; }

      public bool IsHold { get; set; }

      public float FillRate { get; set; }

      public Func<float> CoolTimeFillRate { get; set; }

      public Vector3 Position { get; set; }

      public Transform Transform { get; set; }

      public bool ReferenceCanvasPosition { get; set; }

      public Func<PlayerActor, bool> Condition { get; set; }

      public Func<PlayerActor, string> ErrorText { get; set; }

      public Func<bool> DisplayCondition { get; set; }

      public Action Event { get; set; }
    }

    public class LabelPool : ObjectPool<CommandLabelOption>
    {
      public LabelPool()
      {
        base.\u002Ector();
      }

      public GameObject Source { get; set; }

      protected virtual CommandLabelOption CreateInstance()
      {
        CommandLabelOption component = (CommandLabelOption) ((GameObject) Object.Instantiate<GameObject>((M0) this.Source)).GetComponent<CommandLabelOption>();
        component.PoolSource = this;
        return component;
      }
    }
  }
}
