// Decompiled with JetBrains decompiler
// Type: AIProject.UI.MenuUIBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AIProject.UI
{
  public abstract class MenuUIBehaviour : SerializedMonoBehaviour
  {
    protected BoolReactiveProperty _enabledInput;
    [SerializeField]
    [MinValue(0.0)]
    protected int _focusLevel;
    [SerializeField]
    protected float _alphaAccelerationTime;
    [SerializeField]
    protected float _followAccelerationTime;
    protected List<ICommandData> _commands;
    [SerializeField]
    protected List<ActionIDDownCommand> _actionCommands;
    [SerializeField]
    protected List<KeyCodeDownCommand> _keyCommands;
    protected BoolReactiveProperty _isActive;
    private IConnectableObservable<bool> _activeChanged;

    protected MenuUIBehaviour()
    {
      base.\u002Ector();
    }

    public bool EnabledInput
    {
      get
      {
        Game game = !Singleton<Game>.IsInstance() ? (Game) null : Singleton<Game>.Instance;
        return ((ReactiveProperty<bool>) this._enabledInput).get_Value() && (Object.op_Equality((Object) game?.MapShortcutUI, (Object) null) || Object.op_Equality((Object) game?.MapShortcutUI, (Object) this)) && (Object.op_Equality((Object) game?.Config, (Object) null) && Object.op_Equality((Object) game?.Dialog, (Object) null)) && Object.op_Equality((Object) game?.ExitScene, (Object) null);
      }
      set
      {
        ((ReactiveProperty<bool>) this._enabledInput).set_Value(value);
      }
    }

    public int FocusLevel
    {
      get
      {
        return this._focusLevel = Mathf.Max(0, this._focusLevel);
      }
    }

    public virtual bool IsActiveControl
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isActive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._isActive).set_Value(value);
      }
    }

    protected IObservable<bool> OnActiveChangedAsObservable()
    {
      if (this._activeChanged == null)
      {
        this._activeChanged = (IConnectableObservable<bool>) Observable.Publish<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isActive, ((Component) this).get_gameObject()));
        this._activeChanged.Connect();
      }
      return (IObservable<bool>) this._activeChanged;
    }

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
      this.OnBeforeStart();
      foreach (ICommandData actionCommand in this._actionCommands)
        this._commands.Add(actionCommand);
      foreach (ICommandData keyCommand in this._keyCommands)
        this._commands.Add(keyCommand);
      this.OnAfterStart();
    }

    protected virtual void OnBeforeStart()
    {
    }

    protected virtual void OnAfterStart()
    {
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    public virtual void OnInputMoveDirection(MoveDirection moveDir)
    {
    }

    public virtual void OnInputSubMoveDirection(MoveDirection moveDir)
    {
    }

    public void OnUpdateInput(Input instance)
    {
      foreach (ICommandData command in this._commands)
        command.Invoke(instance);
    }

    public void SetFocusLevel(int level)
    {
      this._focusLevel = level;
    }
  }
}
