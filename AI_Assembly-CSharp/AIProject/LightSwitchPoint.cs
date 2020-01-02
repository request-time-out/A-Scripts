// Decompiled with JetBrains decompiler
// Type: AIProject.LightSwitchPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using Housing;
using IllusionUtility.GetUtility;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class LightSwitchPoint : Point, ICommandable
  {
    [SerializeField]
    [DisableInPlayMode]
    private int _id = -1;
    [SerializeField]
    private bool _lightEnablePlaySE = true;
    private static Subject<Unit> _commandRefreshEvent;
    [SerializeField]
    private bool _linkMapObject;
    [SerializeField]
    private int _linkID;
    [SerializeField]
    private GameObject[] _onModeObjects;
    [SerializeField]
    private GameObject[] _offModeObjects;
    [SerializeField]
    private bool _firstLightEnable;
    private bool _lightEnable;
    [SerializeField]
    private bool _useEnv3D;
    [SerializeField]
    private Env3DSEPoint _env3DSEPoint;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _labelPoint;
    [SerializeField]
    private float _rangeRadius;
    [SerializeField]
    private float _height;
    [SerializeField]
    private CommandType _commandType;
    private int? _instanceID;
    private NavMeshPath _pathForCalc;
    private CommandLabel.CommandInfo[] _onLabels;
    private CommandLabel.CommandInfo[] _offLabels;

    public override int RegisterID
    {
      get
      {
        return this._id;
      }
      set
      {
        this._id = value;
      }
    }

    public bool IsLinkedSE
    {
      get
      {
        return this._useEnv3D && Object.op_Inequality((Object) this._env3DSEPoint, (Object) null);
      }
    }

    public Transform CommandBasePoint
    {
      get
      {
        if (Object.op_Inequality((Object) this._commandBasePoint, (Object) null))
          return this._commandBasePoint;
        return Object.op_Inequality((Object) ((Component) this).get_transform(), (Object) null) ? ((Component) this).get_transform() : (Transform) null;
      }
    }

    public Vector3 CommandBasePosition
    {
      get
      {
        Transform commandBasePoint = this.CommandBasePoint;
        return Object.op_Inequality((Object) commandBasePoint, (Object) null) ? commandBasePoint.get_position() : Vector3.get_zero();
      }
    }

    public Transform LabelPoint
    {
      get
      {
        return Object.op_Inequality((Object) this._labelPoint, (Object) null) ? this._labelPoint : ((Component) this).get_transform();
      }
    }

    public bool IsHousingItem { get; private set; }

    public int InstanceID
    {
      get
      {
        if (this._instanceID.HasValue)
          return this._instanceID.Value;
        this._instanceID = new int?(((Object) this).GetInstanceID());
        return this._instanceID.Value;
      }
    }

    public bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      if (this.TutorialHideMode())
        return false;
      bool flag;
      if (this.CommandType == CommandType.Forward)
      {
        if ((double) this._rangeRadius + (!this.IsHousingItem ? 0.0 : (double) radiusA) < (double) distance)
          return false;
        Vector3 commandBasePosition = this.CommandBasePosition;
        commandBasePosition.y = (__Null) 0.0;
        float num = angle / 2f;
        flag = (double) Vector3.Angle(Vector3.op_Subtraction(commandBasePosition, basePosition), forward) <= (double) num;
      }
      else
        flag = (double) distance <= (double) this._rangeRadius + (!this.IsHousingItem ? 0.0 : (double) radiusB);
      if (!flag)
        return false;
      PlayerActor player = Manager.Map.GetPlayer();
      return (!Object.op_Inequality((Object) player, (Object) null) || !(player.PlayerController.State is Onbu)) && flag;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      return ((Behaviour) nmAgent).get_isActiveAndEnabled() && (double) Mathf.Abs((float) (((Component) nmAgent).get_transform().get_position().y - this.CommandBasePosition.y)) <= (double) this._height / 2.0;
    }

    public bool IsImpossible
    {
      get
      {
        return false;
      }
    }

    public bool SetImpossible(bool value, Actor actor)
    {
      return true;
    }

    public bool IsNeutralCommand
    {
      get
      {
        return true;
      }
    }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    public Vector3 CommandCenter
    {
      get
      {
        Vector3 commandBasePosition = this.CommandBasePosition;
        CommandArea commandArea = Manager.Map.GetCommandArea();
        if (Object.op_Inequality((Object) commandArea, (Object) null) && Object.op_Inequality((Object) commandArea.BaseTransform, (Object) null))
          commandBasePosition.y = commandArea.BaseTransform.get_position().y;
        return commandBasePosition;
      }
    }

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return this._lightEnable ? this._offLabels : this._onLabels;
      }
    }

    public CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        return (CommandLabel.CommandInfo[]) null;
      }
    }

    public ObjectLayer Layer { get; } = ObjectLayer.Command;

    public CommandType CommandType
    {
      get
      {
        return this._commandType;
      }
    }

    private void Awake()
    {
      if (LightSwitchPoint._commandRefreshEvent == null)
      {
        LightSwitchPoint._commandRefreshEvent = new Subject<Unit>();
        ObservableExtensions.Subscribe<IList<Unit>>(Observable.Where<IList<Unit>>(Observable.TakeUntilDestroy<IList<Unit>>((IObservable<M0>) Observable.Buffer<Unit, Unit>((IObservable<M0>) LightSwitchPoint._commandRefreshEvent, (IObservable<M1>) Observable.ThrottleFrame<Unit>((IObservable<M0>) LightSwitchPoint._commandRefreshEvent, 1, (FrameCountType) 0)), (Component) Singleton<Manager.Map>.Instance), (Func<M0, bool>) (_ => !_.IsNullOrEmpty<Unit>())), (Action<M0>) (_ =>
        {
          CommandArea commandArea = Manager.Map.GetCommandArea();
          if (Object.op_Equality((Object) commandArea, (Object) null))
            return;
          commandArea.RefreshCommands();
        }));
      }
      if (this._linkMapObject)
      {
        LightSwitchData lightSwitchData = LightSwitchData.Get(this._linkID);
        if (Object.op_Inequality((Object) lightSwitchData, (Object) null))
        {
          this._onModeObjects = lightSwitchData.OnModeObjects;
          this._offModeObjects = lightSwitchData.OffModeObjects;
        }
      }
      this.ActiveChange(this._firstLightEnable);
    }

    protected override void Start()
    {
      base.Start();
      ItemComponent itemComponent = (ItemComponent) ((Component) this).GetComponent<ItemComponent>();
      if (Object.op_Equality((Object) itemComponent, (Object) null))
        itemComponent = (ItemComponent) ((Component) this).GetComponentInParent<ItemComponent>();
      this.IsHousingItem = Object.op_Inequality((Object) itemComponent, (Object) null);
      if (Singleton<Resources>.IsInstance())
      {
        Resources instance = Singleton<Resources>.Instance;
        if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        {
          GameObject loop = ((Component) this).get_transform().FindLoop(instance.DefinePack.MapDefines.CommandTargetName);
          Transform transform = !Object.op_Inequality((Object) loop, (Object) null) ? (Transform) null : loop.get_transform();
          this._commandBasePoint = !Object.op_Inequality((Object) transform, (Object) null) ? ((Component) this).get_transform() : transform;
        }
        if (Object.op_Equality((Object) this._labelPoint, (Object) null))
        {
          GameObject loop = ((Component) this).get_transform().FindLoop(instance.DefinePack.MapDefines.LightPointLabelTargetName);
          Transform transform = !Object.op_Inequality((Object) loop, (Object) null) ? (Transform) null : loop.get_transform();
          this._labelPoint = !Object.op_Inequality((Object) transform, (Object) null) ? ((Component) this).get_transform() : transform;
        }
      }
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        this._commandBasePoint = ((Component) this).get_transform();
      if (Object.op_Equality((Object) this._labelPoint, (Object) null))
        this._labelPoint = ((Component) this).get_transform();
      this.InitializeCommandLabels();
    }

    private void InitializeCommandLabels()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      Resources instance = Singleton<Resources>.Instance;
      CommonDefine.CommonIconGroup icon = instance.CommonDefine.Icon;
      int guideCancelId = instance.CommonDefine.Icon.GuideCancelID;
      Sprite sprite;
      instance.itemIconTables.InputIconTable.TryGetValue(guideCancelId, out sprite);
      Dictionary<int, List<string>> commandLabelTextTable = instance.Map.EventPointCommandLabelTextTable;
      int index = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      List<string> source;
      commandLabelTextTable.TryGetValue(15, out source);
      this._onLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = source.GetElement<string>(index),
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = this._labelPoint,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() => this.Switch(true))
        }
      };
      commandLabelTextTable.TryGetValue(16, out source);
      this._offLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = source.GetElement<string>(index),
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = this._labelPoint,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() => this.Switch(false))
        }
      };
    }

    public void Switch(bool active)
    {
      if (this._lightEnable != active)
        this.RefreshCommand();
      this.ActiveChange(active);
      Dictionary<int, bool> dictionary = !Singleton<Game>.IsInstance() ? (Dictionary<int, bool>) null : Singleton<Game>.Instance.Environment?.LightObjectSwitchStateTable;
      if (dictionary == null)
        return;
      dictionary[this.RegisterID] = active;
    }

    private void ActiveChange(bool active)
    {
      this._lightEnable = active;
      if (!this._onModeObjects.IsNullOrEmpty<GameObject>())
      {
        foreach (GameObject onModeObject in this._onModeObjects)
        {
          if (Object.op_Inequality((Object) onModeObject, (Object) null) && onModeObject.get_activeSelf() != active)
            onModeObject.SetActive(active);
        }
      }
      if (!this._offModeObjects.IsNullOrEmpty<GameObject>())
      {
        foreach (GameObject offModeObject in this._offModeObjects)
        {
          if (Object.op_Inequality((Object) offModeObject, (Object) null) && offModeObject.get_activeSelf() == active)
            offModeObject.SetActive(!active);
        }
      }
      if (!this.IsLinkedSE)
        return;
      if (active == this._lightEnablePlaySE)
        this._env3DSEPoint.SoundForcedPlay(false);
      else
        this._env3DSEPoint.SoundForcedStop();
    }

    public bool IsSwitch()
    {
      Dictionary<int, bool> source = !Singleton<Game>.IsInstance() ? (Dictionary<int, bool>) null : Singleton<Game>.Instance.Environment?.LightObjectSwitchStateTable;
      bool flag;
      return source.IsNullOrEmpty<int, bool>() || !source.TryGetValue(this.RegisterID, out flag) ? this._firstLightEnable : flag;
    }

    private void RefreshCommand()
    {
      CommandArea commandArea = Manager.Map.GetCommandArea();
      if (Object.op_Equality((Object) commandArea, (Object) null) || !commandArea.ContainsConsiderationObject((ICommandable) this) || LightSwitchPoint._commandRefreshEvent == null)
        return;
      LightSwitchPoint._commandRefreshEvent.OnNext(Unit.get_Default());
    }

    public bool TutorialHideMode()
    {
      return Manager.Map.TutorialMode;
    }
  }
}
