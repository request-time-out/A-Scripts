// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.WildGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  public class WildGround : AnimalGroundDesire
  {
    [SerializeField]
    [Tooltip("視界に入って逃げる確率")]
    private float escapePercent = 80f;
    [SerializeField]
    [Tooltip("餌を上げてペットになる確率")]
    private float getPercent = 30f;
    [SerializeField]
    private ItemIDKeyPair foodItemID = new ItemIDKeyPair();
    [SerializeField]
    private ItemIDKeyPair getItemID = new ItemIDKeyPair();
    private Vector3 targetPosition = Vector3.get_zero();
    private float stoppingDistance = 1f;
    private GroundAnimalHabitatPoint habitatPoint;
    private IDisposable setDepopPointDisposable;
    private IDisposable lovelyModeDisposable;
    private IDisposable badMoodDisposable;
    protected CommandLabel.CommandInfo[] giveFoodLabels;
    protected CommandLabel.CommandInfo[] getAnimalLabels;
    private float speed;
    protected float followWaitCounter;

    public ItemIDKeyPair FoodItemID
    {
      get
      {
        return this.FoodItemID;
      }
    }

    public ItemIDKeyPair GetItemID
    {
      get
      {
        return this.getItemID;
      }
    }

    public override bool WaitPossible
    {
      get
      {
        return base.WaitPossible || this.CurrentState == AnimalState.Sleep;
      }
    }

    public bool IsCapturable
    {
      get
      {
        return this.IsLovely || this.lovelyModeDisposable != null;
      }
    }

    public bool IsEscape { get; protected set; }

    public bool SetIsEscape { get; private set; }

    public bool ToDepopState
    {
      get
      {
        return this.IsRain || this.SetIsEscape;
      }
    }

    public override bool DepopPossible
    {
      get
      {
        return this.CurrentState == AnimalState.Idle || this.CurrentState == AnimalState.Locomotion;
      }
    }

    public override bool IsWithAgentFree(AgentActor _actor)
    {
      return base.IsWithAgentFree(_actor);
    }

    public float EscapePercent
    {
      get
      {
        return this.escapePercent;
      }
    }

    public float GetPercent
    {
      get
      {
        return this.getPercent;
      }
    }

    public void Initialize(GroundAnimalHabitatPoint _habitatPoint)
    {
      this.Clear();
      if (Object.op_Equality((Object) (this.habitatPoint = _habitatPoint), (Object) null))
        this.SetState(AnimalState.Destroyed, (Action) null);
      else if (!this.habitatPoint.Available || !this.habitatPoint.SetUse(this))
      {
        this.SetState(AnimalState.Destroyed, (Action) null);
      }
      else
      {
        this.DeactivateNavMeshElements();
        this.SetWaypoints(this.habitatPoint);
        this.stateController.Initialize((AnimalBase) this);
        this.desireController.Initialize(Object.op_Implicit((Object) this));
        this.SearchAction.SetSearchEnabled(false, true);
        this.SearchActor.SetSearchEnabled(false, true);
        this.LoadBody();
        this.SetStateData();
        this.BodyEnabled = false;
        this.IsEscape = (double) Random.Range(0.0f, 100f) < (double) this.escapePercent;
        this.SetIsEscape = false;
        this.SetState(AnimalState.Repop, (Action) null);
      }
    }

    protected void SetWaypoints(GroundAnimalHabitatPoint _habitatPoint)
    {
      if (this.MoveArea != null)
        this.MoveArea.Clear();
      if (this.MoveArea == null)
        this.MoveArea = new LocomotionArea();
      if (Object.op_Equality((Object) _habitatPoint, (Object) null))
        return;
      this.MoveArea.SetWaypoint(_habitatPoint.Waypoints);
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      if (Object.op_Inequality((Object) this.habitatPoint, (Object) null))
      {
        this.habitatPoint.StopUse(this);
        this.habitatPoint = (GroundAnimalHabitatPoint) null;
      }
      this.Dispose();
      base.OnDestroy();
    }

    private void Dispose()
    {
      if (this.setDepopPointDisposable != null)
        this.setDepopPointDisposable.Dispose();
      if (this.lovelyModeDisposable != null)
        this.lovelyModeDisposable.Dispose();
      if (this.badMoodDisposable == null)
        return;
      this.badMoodDisposable.Dispose();
    }

    public override bool BadMood
    {
      get
      {
        return this.badMoodDisposable != null;
      }
      set
      {
        if (this.badMoodDisposable != null)
          this.badMoodDisposable.Dispose();
        this.badMoodDisposable = (IDisposable) null;
        if (!value)
          return;
        if (this.lovelyModeDisposable != null)
          this.lovelyModeDisposable.Dispose();
        this.lovelyModeDisposable = (IDisposable) null;
        float num = 600f;
        if (Singleton<Resources>.IsInstance())
        {
          AnimalDefinePack animalDefinePack = Singleton<Resources>.Instance.AnimalDefinePack;
          if (animalDefinePack?.GroundWildInfo != null)
            num = animalDefinePack.GroundWildInfo.BadMoodTime;
        }
        this.badMoodDisposable = ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) num)), (Component) this), (Action<M0>) (_ =>
        {
          this.badMoodDisposable = (IDisposable) null;
          this.ChangeAnimalLabel();
        }));
      }
    }

    public override bool IsNeutralCommand
    {
      get
      {
        if (this.BadMood || !this.isNeutralCommand)
          return false;
        return this.CurrentState == AnimalState.Idle || this.CurrentState == AnimalState.Locomotion || (this.CurrentState == AnimalState.Grooming || this.CurrentState == AnimalState.Peck) || (this.CurrentState == AnimalState.Action0 || this.CurrentState == AnimalState.Sleep) || this.IsLovely;
      }
    }

    public override CommandLabel.CommandInfo[] Labels
    {
      get
      {
        if (!this.IsNeutralCommand)
          return AnimalBase.emptyLabels;
        return !this.IsCapturable ? this.giveFoodLabels : this.getAnimalLabels;
      }
    }

    public override bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      return this.CurrentState != AnimalState.Escape && base.Entered(basePosition, distance, radiusA, radiusB, angle, forward);
    }

    private void ChangeAnimalLabel()
    {
      LabelTypes labelType = this.LabelType;
      LabelTypes labelTypes = this.IsNeutralCommand ? (this.IsCapturable ? LabelTypes.GetAnimal : LabelTypes.GiveFood) : LabelTypes.None;
      if (labelTypes == labelType)
        return;
      this.LabelType = labelTypes;
    }

    private void ChangeGetAnimalLabels()
    {
      LabelTypes labelType = this.LabelType;
      LabelTypes labelTypes = this.IsNeutralCommand ? (this.IsCapturable ? LabelTypes.GetAnimal : LabelTypes.GiveFood) : LabelTypes.None;
      if (labelTypes == labelType)
        return;
      this.LabelType = labelTypes;
    }

    private void ChangeEmptyLabels()
    {
      this.LabelType = LabelTypes.None;
    }

    protected override void InitializeCommandLabels()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      if (((IReadOnlyList<CommandLabel.CommandInfo>) this.giveFoodLabels).IsNullOrEmpty<CommandLabel.CommandInfo>())
      {
        Resources instance = Singleton<Resources>.Instance;
        CommonDefine.CommonIconGroup icon = instance.CommonDefine.Icon;
        StuffItemInfo stuffItemInfo = !Singleton<Resources>.IsInstance() ? (StuffItemInfo) null : Singleton<Resources>.Instance.GameInfo.GetItem(this.foodItemID.categoryID, this.foodItemID.itemID);
        int guideCancelId = icon.GuideCancelID;
        Sprite sprite;
        instance.itemIconTables.InputIconTable.TryGetValue(guideCancelId, out sprite);
        string _foodItemName = stuffItemInfo?.Name ?? "エサ";
        Dictionary<int, List<string>> commandLabelTextTable = instance.Map.EventPointCommandLabelTextTable;
        List<string> source;
        commandLabelTextTable.TryGetValue(19, out source);
        List<string> _textList1;
        commandLabelTextTable.TryGetValue(20, out _textList1);
        int _langIdx = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
        this.giveFoodLabels = new CommandLabel.CommandInfo[1]
        {
          new CommandLabel.CommandInfo()
          {
            Text = string.Format(source.GetElement<string>(_langIdx) ?? "{0}をあげる", (object) _foodItemName),
            Transform = this.LabelPoint,
            IsHold = false,
            Icon = sprite,
            TargetSpriteInfo = icon?.CharaSpriteInfo,
            Condition = (Func<PlayerActor, bool>) (_player => this.HaveFoodItem()),
            ErrorText = (Func<PlayerActor, string>) (_player => string.Format(_textList1.GetElement<string>(_langIdx) ?? "{0}を持っていません。", (object) _foodItemName)),
            Event = (Action) (() =>
            {
              this.ChangeEmptyLabels();
              List<StuffItem> self = !Singleton<Manager.Map>.IsInstance() ? (List<StuffItem>) null : Singleton<Manager.Map>.Instance.Player?.PlayerData?.ItemList;
              if (!((IReadOnlyList<StuffItem>) self).IsNullOrEmpty<StuffItem>())
                self.RemoveItem(new StuffItem(this.foodItemID.categoryID, this.foodItemID.itemID, 1));
              if (this.CurrentState != AnimalState.Sleep)
                this.SetState(AnimalState.LovelyIdle, (Action) (() => this.ChangeGetAnimalLabels()));
              else
                this.ChangeGetAnimalLabels();
              float num = 120f;
              if (Singleton<Resources>.IsInstance())
              {
                AnimalDefinePack animalDefinePack = Singleton<Resources>.Instance.AnimalDefinePack;
                if (animalDefinePack?.GroundWildInfo != null)
                  num = animalDefinePack.GroundWildInfo.LovelyTime;
              }
              if (this.lovelyModeDisposable != null)
                this.lovelyModeDisposable.Dispose();
              this.lovelyModeDisposable = ObservableExtensions.Subscribe<long>(Observable.Do<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) num)), ((Component) this).get_gameObject()), (Action<M0>) (_ => this.lovelyModeDisposable = (IDisposable) null)), (Action<M0>) (_ =>
              {
                if (this.IsLovely)
                  this.ChangeEmptyLabels();
                switch (this.CurrentState)
                {
                  case AnimalState.LovelyIdle:
                    this.SetState(AnimalState.Locomotion, (Action) (() => this.ChangeAnimalLabel()));
                    break;
                  case AnimalState.LovelyFollow:
                    this.SetState(AnimalState.Idle, (Action) (() => this.ChangeAnimalLabel()));
                    break;
                  default:
                    this.ChangeAnimalLabel();
                    break;
                }
              }));
            })
          }
        };
      }
      string _getItemName = (!Singleton<Resources>.IsInstance() ? (StuffItemInfo) null : Singleton<Resources>.Instance.GameInfo.GetItem(this.getItemID.categoryID, this.getItemID.itemID))?.Name ?? "捕獲アイテム";
      if (!((IReadOnlyList<CommandLabel.CommandInfo>) this.getAnimalLabels).IsNullOrEmpty<CommandLabel.CommandInfo>())
        return;
      Resources instance1 = Singleton<Resources>.Instance;
      CommonDefine.CommonIconGroup icon1 = instance1.CommonDefine.Icon;
      int guideCancelId1 = icon1.GuideCancelID;
      Sprite sprite1;
      instance1.itemIconTables.InputIconTable.TryGetValue(guideCancelId1, out sprite1);
      Dictionary<int, List<string>> commandLabelTextTable1 = instance1.Map.EventPointCommandLabelTextTable;
      List<string> source1;
      commandLabelTextTable1.TryGetValue(21, out source1);
      List<string> _textList1_1;
      commandLabelTextTable1.TryGetValue(20, out _textList1_1);
      int _langIdx1 = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      this.getAnimalLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = string.Format(source1.GetElement<string>(_langIdx1) ?? "{0}を捕まえる", (object) this.Name),
          Transform = this.LabelPoint,
          IsHold = false,
          Icon = sprite1,
          Condition = (Func<PlayerActor, bool>) (_player => this.HaveGetItem()),
          ErrorText = (Func<PlayerActor, string>) (_player => string.Format(_textList1_1.GetElement<string>(_langIdx1) ?? "{0}を持っていません。", (object) _getItemName)),
          TargetSpriteInfo = icon1?.CharaSpriteInfo,
          Event = (Action) (() =>
          {
            this.ChangeEmptyLabels();
            this.SetState(AnimalState.WithPlayer, (Action) null);
            PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
            if (!Object.op_Inequality((Object) playerActor, (Object) null))
              return;
            playerActor.Animal = (AnimalBase) this;
            playerActor.PlayerController.ChangeState("CatchAnimal");
          })
        }
      };
    }

    private bool HaveFoodItem()
    {
      List<StuffItem> stuffItemList = !Singleton<Manager.Map>.IsInstance() ? (List<StuffItem>) null : Singleton<Manager.Map>.Instance.Player?.PlayerData?.ItemList;
      return !((IReadOnlyList<StuffItem>) stuffItemList).IsNullOrEmpty<StuffItem>() && stuffItemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == this.foodItemID.categoryID && x.ID == this.foodItemID.itemID && 0 < x.Count));
    }

    private bool HaveGetItem()
    {
      List<StuffItem> stuffItemList = !Singleton<Manager.Map>.IsInstance() ? (List<StuffItem>) null : Singleton<Manager.Map>.Instance.Player?.PlayerData?.ItemList;
      return !((IReadOnlyList<StuffItem>) stuffItemList).IsNullOrEmpty<StuffItem>() && stuffItemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == this.getItemID.categoryID && x.ID == this.getItemID.itemID && 0 < x.Count));
    }

    protected override void OnNearPlayerActorEnter(PlayerActor _player)
    {
      this.ChangeAnimalLabel();
    }

    protected override void OnFarPlayerActorEnter(PlayerActor _player)
    {
      if (!this.WaitPossible)
        return;
      if (this.lovelyModeDisposable != null)
      {
        if (this.IsLovely || this.CurrentState == AnimalState.Sleep)
          return;
        this.LabelType = LabelTypes.None;
        this.SetState(AnimalState.LovelyFollow, (Action) (() => this.LabelType = LabelTypes.GetAnimal));
      }
      else
      {
        if (!this.IsEscape)
          return;
        this.Target = ((Component) _player.Locomotor).get_transform();
        this.SetState(AnimalState.Escape, (Action) null);
      }
    }

    protected override void OnFarPlayerActorStay(PlayerActor _player)
    {
      if (!this.WaitPossible || !this.IsEscape || this.CurrentState == AnimalState.Escape)
        return;
      this.SetIsEscape = true;
      this.Target = ((Component) _player.Locomotor).get_transform();
      this.SetState(AnimalState.Escape, (Action) null);
    }

    public override void OnWeatherChanged(EnvironmentSimulator _simulator)
    {
      base.OnWeatherChanged(_simulator);
      if (!this.DepopPossible || this.weather != Weather.Rain && this.weather != Weather.Storm)
        return;
      this.TryToDepopState();
    }

    protected override bool DesireFilledEvent(DesireType _desireType)
    {
      List<AnimalState> source;
      if (!this.desireController.TargetStateTable.TryGetValue(_desireType, out source) || ((IReadOnlyList<AnimalState>) source).IsNullOrEmpty<AnimalState>())
        return false;
      this.MissingActionPoint();
      AnimalState _nextState = source.Rand<AnimalState>();
      switch (_nextState)
      {
        case AnimalState.LovelyIdle:
        case AnimalState.LovelyFollow:
          return false;
        default:
          this.SetState(_nextState, (Action) null);
          return true;
      }
    }

    protected override bool ChangedCandidateDesire(DesireType _desireType)
    {
      return false;
    }

    protected override void EnterRepop()
    {
      this.ActivateNavMeshObstacle();
      this.AutoChangeAnimation = false;
      this.AnimationEndUpdate = false;
      this.EnabledStateUpdate = false;
      Transform outsidePoint = this.habitatPoint.OutsidePoint;
      this.destination = new Vector3?(this.habitatPoint.InsidePoint.get_position());
      this.targetPosition = this.habitatPoint.InsidePoint.get_position();
      this.Position = outsidePoint.get_position();
      Vector3 eulerAngles = outsidePoint.get_eulerAngles();
      eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
      this.EulerAngles = eulerAngles;
      this.BodyEnabled = true;
      this.MarkerEnabled = true;
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
      this.speed = 0.0f;
      this.ClearWaypoint();
      this.StartCheckCoroutine();
      this.EnabledStateUpdate = true;
    }

    protected override void OnRepop()
    {
      if ((double) Time.get_timeScale() == 0.0)
        return;
      float deltaTime = Time.get_deltaTime();
      Vector3 forward = this.Forward;
      Vector3 vector3_1 = Vector3.op_Subtraction(this.targetPosition, this.Position);
      forward.y = (__Null) (double) (vector3_1.y = (__Null) 0.0f);
      float num1 = Vector3.SignedAngle(forward, vector3_1, Vector3.get_up());
      if (!Mathf.Approximately(num1, 0.0f))
      {
        float num2 = Mathf.Abs(num1);
        Quaternion rotation = this.Rotation;
        float num3 = this.addAngle * deltaTime;
        if ((double) num2 < (double) num3)
          num3 = num2;
        float num4 = num3 * Mathf.Sign(num1);
        this.Rotation = Quaternion.op_Multiply(rotation, Quaternion.Euler(0.0f, num4, 0.0f));
      }
      Vector3 vector3_2 = Vector3.op_Subtraction(this.destination.Value, this.Position);
      this.speed = Mathf.Clamp(this.speed + this.Agent.get_acceleration() * deltaTime, 0.0f, this.WalkSpeed);
      Vector3 vector3_3 = Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_2).get_normalized(), this.speed), deltaTime);
      if ((double) ((Vector3) ref vector3_2).get_sqrMagnitude() <= (double) ((Vector3) ref vector3_3).get_magnitude())
      {
        this.AnimationEndUpdate = true;
        this.Position = this.destination.Value;
        float num2 = 10f;
        if (Singleton<Resources>.IsInstance())
        {
          AnimalDefinePack animalDefinePack = Singleton<Resources>.Instance.AnimalDefinePack;
          if (animalDefinePack?.SystemInfo != null)
            num2 = animalDefinePack.SystemInfo.PopPointCoolTimeRange.RandomRange();
        }
        this.SetState(AnimalState.Locomotion, (Action) null);
        this.SearchActor.SetSearchEnabled(true, false);
      }
      else
      {
        WildGround wildGround = this;
        wildGround.Position = Vector3.op_Addition(wildGround.Position, vector3_3);
      }
    }

    protected override void AnimationRepop()
    {
      float num = Mathf.Clamp((double) this.WalkSpeed == 0.0 ? 0.0f : this.speed / this.WalkSpeed, 0.0f, 1f) * 0.5f;
      string locomotionParamName = AnimalBase.DefaultLocomotionParamName;
      if (Singleton<Resources>.IsInstance())
      {
        AnimalDefinePack.AnimatorInfoGroup animatorInfo = Singleton<Resources>.Instance.AnimalDefinePack?.AnimatorInfo;
        if (animatorInfo != null)
          locomotionParamName = animatorInfo.LocomotionParamName;
      }
      this.SetFloat(locomotionParamName, num);
    }

    protected override void ExitRepop()
    {
      this.AutoChangeAnimation = true;
      this.AnimationEndUpdate = true;
      this.EnabledStateUpdate = true;
      this.Relocate(LocateTypes.NavMesh);
      this.LabelType = LabelTypes.GiveFood;
      float num = 1440f;
      if (Singleton<Resources>.IsInstance())
      {
        AnimalDefinePack animalDefinePack = Singleton<Resources>.Instance.AnimalDefinePack;
        if (animalDefinePack?.GroundWildInfo != null)
          num = animalDefinePack.GroundWildInfo.DestroyTimeSeconds;
      }
      if (this.setDepopPointDisposable != null)
        this.setDepopPointDisposable.Dispose();
      this.setDepopPointDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) num), TimeSpan.FromSeconds(5.0)), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => this.Active && ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ =>
      {
        if (!this.DepopPossible || !this.TryToDepopState())
          ;
      }));
      this.Active = true;
      EnvironmentSimulator environmentSimulator = !Singleton<Manager.Map>.IsInstance() ? (EnvironmentSimulator) null : Singleton<Manager.Map>.Instance.Simulator;
      if (!Object.op_Inequality((Object) environmentSimulator, (Object) null))
        return;
      this.timeZone = environmentSimulator.TimeZone;
      this.weather = environmentSimulator.Weather;
    }

    private bool TryToDepopState()
    {
      if (Object.op_Equality((Object) this.habitatPoint, (Object) null))
        return false;
      if (this.calculatePath == null)
        this.calculatePath = new NavMeshPath();
      if (!NavMesh.CalculatePath(this.Position, this.habitatPoint.InsidePoint.get_position(), this.Agent.get_areaMask(), this.calculatePath) || this.calculatePath.get_status() != null)
        return false;
      this.RemoveActionPoint();
      this.SetState(AnimalState.ToDepop, (Action) null);
      return true;
    }

    protected override void EnterToDepop()
    {
      this.ActivateNavMeshAgent();
      this.ClearCurrentWaypoint();
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
      this.SetAgentSpeed(AnimalGround.LocomotionTypes.Walk);
      this.targetPosition = this.habitatPoint.InsidePoint.get_position();
      this.Agent.SetDestination(this.targetPosition);
      this.Agent.set_isStopped(false);
      this.stoppingDistance = this.Agent.get_stoppingDistance() + 0.25f;
      this.LabelType = LabelTypes.None;
      this.LocomotionCount = 1;
    }

    protected override void OnToDepop()
    {
      if (this.AgentPathPending)
        return;
      if (this.IsNearPoint(this.targetPosition, this.stoppingDistance))
      {
        this.SetState(AnimalState.Depop, (Action) null);
      }
      else
      {
        if (!((Behaviour) this.Agent).get_isActiveAndEnabled())
          return;
        this.Agent.SetDestination(this.targetPosition);
      }
    }

    protected override void AnimationToDepop()
    {
      this.WalkAnimationUpdate();
    }

    protected override void EnterDepop()
    {
      this.AutoChangeAnimation = false;
      this.AnimationEndUpdate = false;
      this.ActivateNavMeshObstacle();
      this.targetPosition = this.habitatPoint.OutsidePoint.get_position();
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
      Vector3 velocity = this.Agent.get_velocity();
      this.speed = ((Vector3) ref velocity).get_magnitude();
    }

    protected override void OnDepop()
    {
      if ((double) Time.get_timeScale() == 0.0)
        return;
      float deltaTime = Time.get_deltaTime();
      Vector3 forward = this.Forward;
      Vector3 vector3_1 = Vector3.op_Subtraction(this.targetPosition, this.Position);
      forward.y = (__Null) (double) (vector3_1.y = (__Null) 0.0f);
      float num1 = Vector3.SignedAngle(forward, vector3_1, Vector3.get_up());
      if (!Mathf.Approximately(num1, 0.0f))
      {
        float num2 = Mathf.Abs(num1);
        Quaternion rotation = this.Rotation;
        float num3 = this.addAngle * deltaTime;
        if ((double) num2 < (double) num3)
          num3 = num2;
        float num4 = num3 * Mathf.Sign(num1);
        this.Rotation = Quaternion.op_Multiply(rotation, Quaternion.Euler(0.0f, num4, 0.0f));
      }
      Vector3 vector3_2 = Vector3.op_Subtraction(this.targetPosition, this.Position);
      this.speed = Mathf.Clamp(this.speed + this.Agent.get_acceleration() * deltaTime, 0.0f, this.WalkSpeed);
      Vector3 vector3_3 = Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_2).get_normalized(), this.speed), deltaTime);
      if ((double) ((Vector3) ref vector3_2).get_sqrMagnitude() <= (double) ((Vector3) ref vector3_3).get_sqrMagnitude())
      {
        this.CrossFade(-1f);
        this.Position = this.targetPosition;
        this.SetState(AnimalState.Destroyed, (Action) null);
      }
      else
      {
        WildGround wildGround = this;
        wildGround.Position = Vector3.op_Addition(wildGround.Position, vector3_3);
      }
    }

    protected override void AnimationDepop()
    {
      float num = Mathf.Clamp((double) this.WalkSpeed == 0.0 ? 0.0f : this.speed / this.WalkSpeed, 0.0f, 1f) * 0.5f;
      string locomotionParamName = AnimalBase.DefaultLocomotionParamName;
      if (Singleton<Resources>.IsInstance())
      {
        AnimalDefinePack.AnimatorInfoGroup animatorInfo = Singleton<Resources>.Instance.AnimalDefinePack?.AnimatorInfo;
        if (animatorInfo != null)
          locomotionParamName = animatorInfo.LocomotionParamName;
      }
      this.SetFloat(locomotionParamName, num);
    }

    protected override void ExitDepop()
    {
      this.RemoveActionPoint();
      if (Object.op_Inequality((Object) this.habitatPoint, (Object) null))
      {
        this.habitatPoint.StopUse(this);
        this.habitatPoint = (GroundAnimalHabitatPoint) null;
      }
      this.StateTimeLimit = 1f;
      this.AutoChangeAnimation = true;
      this.AnimationEndUpdate = true;
    }

    protected override void OnLocomotion()
    {
      if (this.Wait() || this.AgentPathPending)
        return;
      if (this.ToDepopState)
      {
        this.StateCounter += Time.get_deltaTime();
        if ((double) this.StateTimeLimit < (double) this.StateCounter)
        {
          this.StateCounter = 0.0f;
          if (this.TryToDepopState())
            return;
        }
      }
      if (this.IsNearPoint())
      {
        if (this.ToDepopState && this.TryToDepopState())
          return;
        if (!this.HasActionPoint)
          this.ChangeNextWaypoint();
        else if (!this.SetNextState())
        {
          this.StateEndEvent();
          return;
        }
      }
      else if (this.HasNotAgentPath)
      {
        if (this.HasActionPoint)
          this.Agent.SetDestination(this.actionPoint.Destination);
        else
          this.ChangeNextWaypoint();
      }
      if (!((Behaviour) this.Agent).get_isActiveAndEnabled() || !this.Agent.get_isOnNavMesh() || !this.Agent.get_hasPath())
        return;
      this.Agent.SetDestination(this.Agent.get_destination());
    }

    protected override void EnterLovelyIdle()
    {
      this.ActivateNavMeshAgent();
      this.PlayInAnim(AnimationCategoryID.Idle, this.AnimalType != AnimalTypes.Cat ? 0 : 1, (Action) null);
      if (this.IsPrevLovely)
        return;
      this.EnterLovely();
    }

    protected override void OnLovelyIdle()
    {
      if (this.Wait())
        return;
      Vector3 vector3 = Vector3.op_Subtraction(this.FollowActor.Position, this.Position);
      float num1 = 20f;
      float num2 = 10f;
      if (Singleton<Resources>.IsInstance())
      {
        AnimalDefinePack.GroundAnimalInfoGroup groundAnimalInfo = Singleton<Resources>.Instance.AnimalDefinePack?.GroundAnimalInfo;
        if (groundAnimalInfo != null)
        {
          num1 = groundAnimalInfo.FollowStopDistance;
          num2 = groundAnimalInfo.FollowIdleSpace;
        }
      }
      float num3 = num1 + num2;
      if ((double) num3 * (double) num3 >= (double) ((Vector3) ref vector3).get_sqrMagnitude())
        return;
      this.SetState(AnimalState.LovelyFollow, (Action) null);
    }

    protected override void ExitLovelyIdle()
    {
      if (this.IsLovely)
        return;
      this.ExitLovely();
    }

    protected override void EnterLovelyFollow()
    {
      this.ActivateNavMeshAgent();
      this.AnimationEndUpdate = false;
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
      this.StateTimeLimit = 10f;
      if (!this.IsPrevLovely)
        this.EnterLovely();
      this.SetAgentSpeed(AnimalGround.LocomotionTypes.Walk);
      this.followWaitCounter = 0.0f;
    }

    protected override void OnLovelyFollow()
    {
      this.Agent.set_isStopped(this.IsNearPoint(this.FollowActor.Position));
      if (!this.Agent.get_isStopped())
      {
        this.followWaitCounter = 0.0f;
        this.Agent.SetDestination(this.FollowActor.Position);
      }
      else
      {
        this.followWaitCounter += Time.get_deltaTime();
        if (2.0 <= (double) this.followWaitCounter)
        {
          this.followWaitCounter = 0.0f;
          this.SetState(AnimalState.LovelyIdle, (Action) null);
        }
        else
          this.Agent.SetDestination(this.FollowActor.Position);
      }
    }

    protected override void ExitLovelyFollow()
    {
      this.AnimationEndUpdate = true;
      if (this.IsLovely)
        return;
      this.ExitLovely();
    }

    protected override void AnimationLovelyFollow()
    {
      this.WalkAnimationUpdate();
    }

    private void EnterLovely()
    {
      this.FollowActor = !Singleton<Manager.Map>.IsInstance() ? (Actor) null : (Actor) Singleton<Manager.Map>.Instance.Player;
      float num = 5f;
      if (Singleton<Resources>.IsInstance())
      {
        AnimalDefinePack.GroundAnimalInfoGroup groundAnimalInfo = Singleton<Resources>.Instance.AnimalDefinePack?.GroundAnimalInfo;
        if (groundAnimalInfo != null)
          num = groundAnimalInfo.FollowStopDistance;
      }
      this.Agent.set_stoppingDistance(num);
    }

    private void ExitLovely()
    {
      this.Agent.set_stoppingDistance(this.FirstStoppingDistance);
      this.FollowActor = (Actor) null;
    }

    protected override void EnterSleep()
    {
      this.ModelInfo.EyesShapeInfo.SetBlendShape(100f);
      this.ActivateNavMeshObstacle();
      this.EnterAction((Action) (() =>
      {
        if (this.lovelyModeDisposable == null)
          this.SetState(AnimalState.Locomotion, (Action) null);
        else
          this.SetState(AnimalState.LovelyFollow, (Action) null);
      }));
      this.PlayInAnim(AnimationCategoryID.Sleep, (Action) null);
      this.SetSchedule(this.CurrentAnimState);
    }

    protected bool SetEscapePoint()
    {
      if (!Object.op_Implicit((Object) this.Target))
        return false;
      Waypoint waypoint = (Waypoint) null;
      this.SetAgentSpeed(AnimalGround.LocomotionTypes.Run);
      LocomotionArea moveArea = this.MoveArea;
      Vector3 position1 = this.Position;
      Vector3 position2 = this.Target.get_position();
      double farSearchRadius = (double) this.SearchActor.FarSearchRadius;
      Vector3 vector3 = Vector3.op_Subtraction(this.Position, this.Target.get_position());
      Vector3 normalized = ((Vector3) ref vector3).get_normalized();
      ref Waypoint local = ref waypoint;
      if (!moveArea.GetRandomPoint(position1, position2, (float) farSearchRadius, normalized, 135f, ref local, LocomotionArea.AreaType.All) || !this.Agent.SetDestination(((Component) waypoint).get_transform().get_position()))
        return false;
      this.Agent.set_isStopped(false);
      this.Agent.set_updateRotation(true);
      this.ClearCurrentWaypoint();
      waypoint.Reserver = (INavMeshActor) this;
      this.currentWaypoint = waypoint;
      this.destination = new Vector3?();
      this.LocomotionCount = 1;
      return true;
    }

    protected override void EnterEscape()
    {
      this.ActivateNavMeshAgent();
      this.CancelActionPoint();
      this.AnimationEndUpdate = false;
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
      this.SetAgentSpeed(AnimalGround.LocomotionTypes.Run);
      this.SetIsEscape = true;
      if (this.SetEscapePoint())
        return;
      this.SetState(AnimalState.Locomotion, (Action) null);
    }

    protected override void OnEscape()
    {
      if (this.IsNearPoint() || this.IsActiveAgent && !this.Agent.get_hasPath() && !this.Agent.get_pathPending())
      {
        if (this.SearchActor.CheckPlayerOnFarSearchArea() && this.SetEscapePoint())
          return;
        this.SetState(AnimalState.Locomotion, (Action) null);
      }
      else
      {
        if (!((Behaviour) this.Agent).get_isActiveAndEnabled() || !this.Agent.get_isOnNavMesh() || !this.Agent.get_hasPath())
          return;
        this.Agent.SetDestination(this.Agent.get_destination());
      }
    }

    protected override void ExitEscape()
    {
      this.ClearCurrentWaypoint();
      this.Target = (Transform) null;
      this.Agent.set_velocity(Vector3.get_zero());
      this.AnimationEndUpdate = true;
    }

    protected override void AnimationEscape()
    {
      this.RunAnimationUpdate();
    }

    public void StartAvoid(Vector3 _avoidPoint, Action _endEvent = null)
    {
      this.BadMood = true;
      this.SetState(AnimalState.Locomotion, (Action) (() =>
      {
        if (this.CurrentState != AnimalState.Locomotion)
          return;
        Waypoint _waypoint = (Waypoint) null;
        LocomotionArea moveArea = this.MoveArea;
        Vector3 position = this.Position;
        Vector3 _targetPoint = _avoidPoint;
        double nearSearchRadius = (double) this.SearchActor.NearSearchRadius;
        Vector3 vector3 = Vector3.op_Subtraction(this.Position, _avoidPoint);
        Vector3 normalized = ((Vector3) ref vector3).get_normalized();
        ref Waypoint local = ref _waypoint;
        if (moveArea.GetRandomPoint(position, _targetPoint, (float) nearSearchRadius, normalized, 200f, ref local, LocomotionArea.AreaType.Normal) && this.SetNextWaypoint(_waypoint))
          return;
        this.SetState(AnimalState.Idle, (Action) null);
      }));
    }
  }
}
