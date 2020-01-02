// Decompiled with JetBrains decompiler
// Type: AIProject.PlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Player;
using Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (ActorLocomotion))]
  public class PlayerController : ActorController
  {
    private static Dictionary<string, Func<IState>> _getTypeEventTable = new Dictionary<string, Func<IState>>()
    {
      ["Normal"] = (Func<IState>) (() => (IState) new Normal()),
      ["Sleep"] = (Func<IState>) (() => (IState) new AIProject.Player.Sleep()),
      ["Lie"] = (Func<IState>) (() => (IState) new Lie()),
      ["Break"] = (Func<IState>) (() => (IState) new AIProject.Player.Break()),
      ["DateSleep"] = (Func<IState>) (() => (IState) new DateSleep()),
      ["DateBreak"] = (Func<IState>) (() => (IState) new DateBreak()),
      ["DateEat"] = (Func<IState>) (() => (IState) new DateEat()),
      ["DateSearch"] = (Func<IState>) (() => (IState) new DateSearch()),
      ["DateOpenHarbordoor"] = (Func<IState>) (() => (IState) new DateOpenHarbordoor()),
      ["SpecialH"] = (Func<IState>) (() => (IState) new SpecialH()),
      ["Search"] = (Func<IState>) (() => (IState) new AIProject.Player.Search()),
      ["Kitchen"] = (Func<IState>) (() => (IState) new Kitchen()),
      ["Cook"] = (Func<IState>) (() => (IState) new AIProject.Player.Cook()),
      ["ItemBox"] = (Func<IState>) (() => (IState) new ItemBox()),
      ["Pantry"] = (Func<IState>) (() => (IState) new Pantry()),
      ["Fishing"] = (Func<IState>) (() => (IState) new Fishing()),
      ["Sex"] = (Func<IState>) (() => (IState) new Sex()),
      ["Move"] = (Func<IState>) (() => (IState) new AIProject.Player.Move()),
      ["CatchAnimal"] = (Func<IState>) (() => (IState) new CatchAnimal()),
      ["Photo"] = (Func<IState>) (() => (IState) new Photo()),
      ["Communication"] = (Func<IState>) (() => (IState) new Communication()),
      ["Menu"] = (Func<IState>) (() => (IState) new Menu()),
      ["WMap"] = (Func<IState>) (() => (IState) new WMap()),
      ["Houchi"] = (Func<IState>) (() => (IState) new Houchi()),
      ["RequestEvent"] = (Func<IState>) (() => (IState) new RequestEvent()),
      ["Tutorial"] = (Func<IState>) (() => (IState) new Tutorial()),
      ["OpeningWakeUp"] = (Func<IState>) (() => (IState) new OpeningWakeUp()),
      ["Onbu"] = (Func<IState>) (() => (IState) new Onbu()),
      ["DoorOpen"] = (Func<IState>) (() => (IState) new DoorOpen()),
      ["DoorClose"] = (Func<IState>) (() => (IState) new DoorClose()),
      ["OpenHarbordoor"] = (Func<IState>) (() => (IState) new OpenHarbordoor()),
      ["BaseMenu"] = (Func<IState>) (() => (IState) new BaseMenu()),
      ["Housing"] = (Func<IState>) (() => (IState) new AIProject.Player.Housing()),
      ["ChickenCoopMenu"] = (Func<IState>) (() => (IState) new ChickenCoopMenu()),
      ["DeviceMenu"] = (Func<IState>) (() => (IState) new DeviceMenu()),
      ["CharaEnter"] = (Func<IState>) (() => (IState) new CharaEnter()),
      ["CharaChange"] = (Func<IState>) (() => (IState) new CharaChange()),
      ["Harvest"] = (Func<IState>) (() => (IState) new Harvest()),
      ["Craft"] = (Func<IState>) (() => (IState) new Craft()),
      ["ShipMenu"] = (Func<IState>) (() => (IState) new ShipMenu()),
      ["DressRoom"] = (Func<IState>) (() => (IState) new DressRoom()),
      ["ClothChange"] = (Func<IState>) (() => (IState) new AIProject.Player.ClothChange()),
      ["EntryChara"] = (Func<IState>) (() => (IState) new EntryChara()),
      ["EditChara"] = (Func<IState>) (() => (IState) new EditChara()),
      ["EditPlayer"] = (Func<IState>) (() => (IState) new EditPlayer()),
      ["CharaLookEdit"] = (Func<IState>) (() => (IState) new CharaLookEdit()),
      ["PlayerLookEdit"] = (Func<IState>) (() => (IState) new PlayerLookEdit()),
      ["CharaMigration"] = (Func<IState>) (() => (IState) new CharaMigration()),
      ["Idle"] = (Func<IState>) (() => (IState) new Idle()),
      ["Follow"] = (Func<IState>) (() => (IState) new Follow()),
      ["ExitEatEvent"] = (Func<IState>) (() => (IState) new ExitEatEvent()),
      ["Warp"] = (Func<IState>) (() => (IState) new AIProject.Player.Warp())
    };
    private static ReadOnlyDictionary<string, EventType> _statePairedEventTable = new ReadOnlyDictionary<string, EventType>((IDictionary<string, EventType>) new Dictionary<string, EventType>()
    {
      ["Sleep"] = EventType.Sleep,
      ["Lie"] = (EventType) 0,
      ["Break"] = EventType.Break,
      ["DateSleep"] = EventType.Sleep,
      ["DateBreak"] = EventType.Break,
      ["DateEat"] = EventType.Eat,
      ["DateSearch"] = EventType.Search,
      ["DateOpenHarbordoor"] = (EventType) 0,
      ["SpecialH"] = (EventType) 0,
      ["Search"] = EventType.Search,
      ["Kitchen"] = (EventType) 0,
      ["Cook"] = (EventType) 0,
      ["Pantry"] = (EventType) 0,
      ["ItemBox"] = EventType.StorageIn,
      ["Fishing"] = EventType.Search,
      ["Move"] = EventType.Move,
      ["CatchAnimal"] = (EventType) 0,
      ["Photo"] = (EventType) 0,
      ["Communication"] = (EventType) 0,
      ["Menu"] = (EventType) 0,
      ["WMap"] = (EventType) 0,
      ["Houchi"] = (EventType) 0,
      ["RequestEvent"] = (EventType) 0,
      ["Tutorial"] = (EventType) 0,
      ["OpeningWakeUp"] = (EventType) 0,
      ["Onbu"] = (EventType) 0,
      ["DoorOpen"] = (EventType) 0,
      ["DoorClose"] = (EventType) 0,
      ["OpenHarbordoor"] = (EventType) 0,
      ["BaseMenu"] = (EventType) 0,
      ["Housing"] = (EventType) 0,
      ["ChickenCoopMenu"] = (EventType) 0,
      ["DeviceMenu"] = (EventType) 0,
      ["CharaEnter"] = (EventType) 0,
      ["CharaChange"] = (EventType) 0,
      ["Harvest"] = (EventType) 0,
      ["Craft"] = (EventType) 0,
      ["ShipMenu"] = (EventType) 0,
      ["DressRoom"] = (EventType) 0,
      ["ClothChange"] = (EventType) 0,
      ["EntryChara"] = (EventType) 0,
      ["EditChara"] = (EventType) 0,
      ["EditPlayer"] = (EventType) 0,
      ["CharaLookEdit"] = (EventType) 0,
      ["PlayerLookEdit"] = (EventType) 0,
      ["CharaMigration"] = (EventType) 0,
      ["Idle"] = (EventType) 0,
      ["Follow"] = (EventType) 0,
      ["ExitEatEvent"] = (EventType) 0,
      ["Warp"] = (EventType) 0
    });
    public bool CanCrouch = true;
    public bool CanJump = true;
    public bool WalkByDefault;
    [SerializeField]
    protected Transform _cameraTransform;
    [SerializeField]
    protected ActorLocomotionThirdPerson _character;
    [SerializeField]
    private CommandArea _commandArea;

    public Transform CameraTransform
    {
      get
      {
        return this._cameraTransform;
      }
      set
      {
        this._cameraTransform = value;
      }
    }

    public CommandArea CommandArea
    {
      get
      {
        return this._commandArea;
      }
      set
      {
        this._commandArea = value;
      }
    }

    public string PrevStateName { get; set; }

    private void OnEnable()
    {
      if (!Object.op_Implicit((Object) this._character))
        return;
      ((Behaviour) this._character).set_enabled(true);
    }

    private void OnDisable()
    {
      if (Object.op_Implicit((Object) this._character))
        ((Behaviour) this._character).set_enabled(false);
      this._actor.StateInfo.Init();
      if (!Object.op_Implicit((Object) this._cameraTransform))
        return;
      Actor.InputInfo stateInfo = this._actor.StateInfo;
      stateInfo.lookPos = Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(this._cameraTransform.get_forward(), 100f));
      this._actor.StateInfo = stateInfo;
    }

    protected override void Start()
    {
      base.Start();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    public override void StartBehavior()
    {
      if (Object.op_Equality((Object) this._cameraTransform, (Object) null))
        this._cameraTransform = ((Component) Camera.get_main()).get_transform();
      if (!Object.op_Equality((Object) this._character, (Object) null))
        return;
      this._character = (ActorLocomotionThirdPerson) ((Component) this).GetComponent<ActorLocomotionThirdPerson>();
    }

    private void OnUpdate()
    {
      if (Singleton<Scene>.Instance.IsNowLoadingFade)
        return;
      bool? nullable = this._actor != null ? new bool?(this._actor.IsInit) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0)
        return;
      Actor.InputInfo stateInfo = this._actor.StateInfo;
      if (this._state != null)
        this._state.Update(this._actor, ref stateInfo);
      this._actor.StateInfo = stateInfo;
      if (this._state != null)
        this._state.AfterUpdate(this._actor, this._actor.StateInfo);
      this._character.Move(Vector3.get_zero());
      PlayerActor actor = this._actor as PlayerActor;
      if (!Object.op_Inequality((Object) actor, (Object) null) || !Object.op_Inequality((Object) actor.Partner, (Object) null) || !actor.Partner.IsSlave)
        return;
      actor.Partner.Position = actor.Position;
      actor.Partner.Rotation = actor.Rotation;
    }

    protected override void SubFixedUpdate()
    {
      if (Object.op_Equality((Object) this._actor, (Object) null) || this._state == null)
        return;
      Actor.InputInfo stateInfo = this._actor.StateInfo;
      if (this._state != null)
        this._state.FixedUpdate(this._actor, stateInfo);
      this._actor.StateInfo = stateInfo;
    }

    public void ChangeState(string target, ActionPoint point, Action onCompleted = null)
    {
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      if (Object.op_Inequality((Object) this.CommandArea, (Object) null))
        this.CommandArea.SequenceSetConsiderations((Action<ICommandable>) (x => this.CleanPoint(x, point)));
      this._actor.CurrentPoint = point;
      this.ChangeState(target);
      (this._state as PlayerStateBase).OnCompleted = onCompleted;
    }

    private void CleanPoint(ICommandable commandable, ActionPoint point)
    {
      if (!(commandable is ActionPoint) || Object.op_Equality((Object) (commandable as ActionPoint), (Object) point))
        return;
      this.CommandArea.RemoveConsiderationObject(commandable);
    }

    public override void ChangeState(string target)
    {
      if (Object.op_Inequality((Object) this.CommandArea, (Object) null) && target != "Normal" && (target != "Onbu" && target != "Houchi"))
        ((Behaviour) this.CommandArea).set_enabled(false);
      Func<IState> func;
      if (!PlayerController._getTypeEventTable.TryGetValue(target, out func))
        return;
      IState state = this._state;
      this._state = func();
      if (state != null)
      {
        string name = state.GetType().Name;
        this.PrevStateName = name;
        EventType type = (EventType) 0;
        PlayerController._statePairedEventTable.TryGetValue(name, ref type);
        state.Release(this._actor, type);
        Action onCompleted = (state as PlayerStateBase).OnCompleted;
        if (onCompleted != null)
          onCompleted();
      }
      this._state.Awake(this._actor);
    }
  }
}
