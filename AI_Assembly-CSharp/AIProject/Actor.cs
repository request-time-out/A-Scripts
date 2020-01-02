// Decompiled with JetBrains decompiler
// Type: AIProject.Actor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.Animal;
using AIProject.Definitions;
using AIProject.SaveData;
using AIProject.Scene;
using ConfigScene;
using IllusionUtility.GetUtility;
using Manager;
using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  [DisallowMultipleComponent]
  public abstract class Actor : SerializedMonoBehaviour, IVisible
  {
    public int ID;
    [SerializeField]
    protected Rigidbody _rigidbody;
    [SerializeField]
    protected NavMeshAgent _navMeshAgent;
    [SerializeField]
    protected NavMeshObstacle _navMeshObstacle;
    [SerializeField]
    private float _accelerationTime;
    [SerializeField]
    private float _maxVerticalVelocityOnGround;
    [SerializeField]
    private float _slopeStartAngle;
    [SerializeField]
    private float _slopeEndAngle;
    private ReadOnlyDictionary<Actor.FovBodyPart, Transform> _fovBodyPartReadOnlyTable;
    [SerializeField]
    protected Dictionary<Actor.FovBodyPart, Transform> _fovTargetPointTable;
    private Transform[] _fovTargets;
    private ReadOnlyDictionary<Actor.BodyPart, Transform> _chaBodyPartsReadonlyDic;
    [SerializeField]
    protected Dictionary<Actor.BodyPart, Transform> _chaBodyParts;
    protected Transform[] _chaFovTargets;
    private LocomotionProfile _locomotionProfile;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private MapArea _mapArea;
    protected IntReactiveProperty _mapAreaID;
    private ObservableStateMachineTrigger _observableStateMachine;
    private int? _instanceID;
    protected ValueTuple<int, int> _scheduleThreshold;
    protected Actor.BehaviorSchedule _schedule;
    protected ValueTuple<bool, TimeSpan> _gestureTimeLimit;
    protected int _originAvoidancePriority;
    private IEnumerator _talkEnumerator;
    private IDisposable _talkDisposable;
    private IEnumerator _withAnimalEnumerator;
    private IDisposable _withAnimalDisposable;
    private IDisposable _standDisposable;
    protected List<Actor.ItemScaleInfo> _scaleCtrlInfos;

    protected Actor()
    {
      base.\u002Ector();
    }

    public EventType EventKey { get; set; }

    public ChaControl ChaControl { get; set; }

    public bool IsVisible { get; set; }

    public int ActionID { get; set; }

    public int PoseID { get; set; }

    public abstract string CharaName { get; }

    public ItemCache EquipedItem { get; protected set; }

    public bool IsKinematic
    {
      get
      {
        return this._rigidbody.get_isKinematic();
      }
      set
      {
        this._rigidbody.set_isKinematic(value);
      }
    }

    public Rigidbody Rigidbody
    {
      get
      {
        return this._rigidbody;
      }
    }

    public NavMeshAgent NavMeshAgent
    {
      get
      {
        return this._navMeshAgent;
      }
    }

    public NavMeshObstacle NavMeshObstacle
    {
      get
      {
        return this._navMeshObstacle;
      }
    }

    public float AccelerationTime
    {
      get
      {
        return this._accelerationTime;
      }
    }

    public float MaxVerticalVelocityOnGround
    {
      get
      {
        return this._maxVerticalVelocityOnGround;
      }
    }

    public Actor.InputInfo StateInfo { get; set; }

    public ReadOnlyDictionary<Actor.FovBodyPart, Transform> FovTargetPointTable
    {
      get
      {
        return this._fovBodyPartReadOnlyTable ?? (this._fovBodyPartReadOnlyTable = new ReadOnlyDictionary<Actor.FovBodyPart, Transform>((IDictionary<Actor.FovBodyPart, Transform>) this._fovTargetPointTable));
      }
    }

    public Transform[] FovTargetPoints
    {
      get
      {
        return this._fovTargets ?? (this._fovTargets = ((IEnumerable<Transform>) this._fovTargetPointTable.Values).ToArray<Transform>());
      }
    }

    public ReadOnlyDictionary<Actor.BodyPart, Transform> ChaBodyParts
    {
      get
      {
        return this._chaBodyPartsReadonlyDic ?? (this._chaBodyPartsReadonlyDic = new ReadOnlyDictionary<Actor.BodyPart, Transform>((IDictionary<Actor.BodyPart, Transform>) this._chaBodyParts));
      }
    }

    public Transform[] ChaFovTargets
    {
      get
      {
        if (this._chaFovTargets.IsNullOrEmpty<Transform>())
          this._chaFovTargets = ((IEnumerable<Transform>) this._chaBodyParts.Values).ToArray<Transform>();
        return this._chaFovTargets;
      }
    }

    public LocomotionProfile LocomotionProfile
    {
      get
      {
        if (Object.op_Equality((Object) this._locomotionProfile, (Object) null))
          this._locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
        return this._locomotionProfile;
      }
    }

    public bool Visible { get; set; }

    public Vector3 Normal { get; set; }

    public float ForwardMLP { get; set; }

    public abstract ActorController Controller { get; }

    public abstract ICharacterInfo TiedInfo { get; }

    public abstract ActorAnimation Animation { get; }

    public int MapID { get; set; }

    public int ChunkID { get; set; }

    public int AreaID { get; set; }

    public MapArea MapArea
    {
      get
      {
        return this._mapArea;
      }
      set
      {
        this._mapArea = value;
      }
    }

    public MapArea.AreaType AreaType { get; set; }

    public IObservable<int> OnMapAreaChangedAsObservable()
    {
      return (IObservable<int>) this._mapAreaID ?? (IObservable<int>) (this._mapAreaID = new IntReactiveProperty(-1));
    }

    public virtual bool IsNeutralCommand
    {
      get
      {
        return true;
      }
    }

    public virtual Desire.ActionType Mode { get; set; }

    public bool IsInit { get; private set; }

    public abstract ActorLocomotion Locomotor { get; }

    public Actor Partner { get; set; }

    public Vector3 Position
    {
      get
      {
        return Object.op_Inequality((Object) this.Locomotor, (Object) null) ? ((Component) this.Locomotor).get_transform().get_position() : Vector3.get_zero();
      }
      set
      {
        ((Component) this.Locomotor).get_transform().set_position(value);
      }
    }

    public Vector3 CommandCenter
    {
      get
      {
        return ((Component) this.Locomotor).get_transform().get_position();
      }
    }

    public Vector3 Forward
    {
      get
      {
        return ((Component) this.Locomotor).get_transform().get_forward();
      }
    }

    public Vector3 Back
    {
      get
      {
        return Quaternion.op_Multiply(((Component) this.Locomotor).get_transform().get_rotation(), Vector3.get_back());
      }
    }

    public Quaternion Rotation
    {
      get
      {
        return ((Component) this.Locomotor).get_transform().get_rotation();
      }
      set
      {
        ((Component) this.Locomotor).get_transform().set_rotation(value);
      }
    }

    public PoseType PoseType { get; set; }

    public void Relocate()
    {
      RaycastHit[] raycastHitArray = Physics.RaycastAll(Vector3.op_Addition(this.Position, Vector3.op_Multiply(Vector3.get_up(), 10f)), Vector3.get_down(), 100f, LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer));
      RaycastHit raycastHit1 = (RaycastHit) null;
      foreach (RaycastHit raycastHit2 in raycastHitArray)
      {
        if (Object.op_Inequality((Object) ((Component) ((RaycastHit) ref raycastHit2).get_collider()).get_gameObject(), (Object) ((Component) this.Controller).get_gameObject()))
        {
          raycastHit1 = raycastHit2;
          break;
        }
      }
      this.Position = ((RaycastHit) ref raycastHit1).get_point();
    }

    public bool IsOnGround { get; set; }

    public ActionPoint CurrentPoint { get; set; }

    public ActionPoint NextPoint { get; set; }

    public EquipState EquipState { get; private set; }

    public ObservableStateMachineTrigger ObservableStateMachine
    {
      get
      {
        return this._observableStateMachine ?? (this._observableStateMachine = (ObservableStateMachineTrigger) this.Animation.Animator.GetBehaviour<ObservableStateMachineTrigger>());
      }
    }

    public int InstanceID
    {
      get
      {
        if (!this._instanceID.HasValue)
          this._instanceID = new int?(((Object) this).GetInstanceID());
        return this._instanceID.Value;
      }
    }

    public bool IsSlave { get; set; }

    public void SetCurrentSchedule(
      bool scheduled,
      string name_,
      int minMin,
      int maxMin,
      bool gestures,
      bool useGameTime)
    {
      this._scheduleThreshold = new ValueTuple<int, int>(minMin, maxMin);
      int num = minMin == maxMin ? minMin : Random.Range(minMin, maxMin + 1);
      this._schedule = new Actor.BehaviorSchedule(scheduled, name_, (float) num);
      this._schedule.useGameTime = useGameTime;
      TimeSpan timeSpan = TimeSpan.FromMinutes((double) (num / 2));
      this._gestureTimeLimit = new ValueTuple<bool, TimeSpan>(gestures, timeSpan);
    }

    public Actor.BehaviorSchedule Schedule
    {
      get
      {
        return this._schedule;
      }
      set
      {
        this._schedule = value;
      }
    }

    public bool EnabledSchedule
    {
      get
      {
        return this._schedule.enabled;
      }
      set
      {
        this._schedule.enabled = value;
      }
    }

    public virtual void EnableEntity()
    {
    }

    public virtual void DisableEntity()
    {
    }

    public void ActivateNavMeshAgent()
    {
      if (((Behaviour) this._navMeshObstacle).get_enabled())
        ((Behaviour) this._navMeshObstacle).set_enabled(false);
      if (((Behaviour) this._navMeshAgent).get_enabled())
        return;
      ((Behaviour) this._navMeshAgent).set_enabled(true);
    }

    public void DeactivateNavMeshAgent()
    {
      if (((Behaviour) this._navMeshAgent).get_enabled())
        ((Behaviour) this._navMeshAgent).set_enabled(false);
      if (((Behaviour) this._navMeshObstacle).get_enabled())
        return;
      ((Behaviour) this._navMeshObstacle).set_enabled(true);
    }

    public void StopNavMeshAgent()
    {
      if (!((Behaviour) this.NavMeshAgent).get_isActiveAndEnabled() || !this.NavMeshAgent.get_isOnNavMesh())
        return;
      this.NavMeshAgent.set_velocity(Vector3.get_zero());
      if (this.NavMeshAgent.get_isStopped())
        return;
      this.NavMeshAgent.set_isStopped(true);
    }

    public void ChangeStaticNavMeshAgentAvoidance()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      this._navMeshAgent.set_avoidancePriority(Singleton<Resources>.Instance.AgentProfile.AvoidancePriorityStationary);
    }

    public void ChangeDynamicNavMeshAgentAvoidance()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      this._navMeshAgent.set_avoidancePriority(Singleton<Resources>.Instance.AgentProfile.AvoidancePriorityDefault);
    }

    public void RecoverNavMeshAgentAvoidance()
    {
      if (this._originAvoidancePriority < 0)
        return;
      this._navMeshAgent.set_avoidancePriority(this._originAvoidancePriority);
      this._originAvoidancePriority = -1;
    }

    public void StartTalkSequence(Actor listener)
    {
      this.StartTalkSequence(listener, 0, 0);
    }

    public void StartTalkSequence(Actor listener, int speakID, int listenID)
    {
      switch (listener)
      {
        case AgentActor _:
          IEnumerator enumerator1 = this._talkEnumerator = this.StartTalkSequenceCoroutine(listener, speakID, listenID);
          if (this._talkDisposable != null)
            this._talkDisposable.Dispose();
          this._talkDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator1), false));
          AgentActor agentActor = listener as AgentActor;
          agentActor.CommandPartner = this;
          agentActor.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
          break;
        case MerchantActor _:
          IEnumerator enumerator2 = this._talkEnumerator = this.StartTalkSequenceCoroutine(listener, speakID, listenID);
          if (this._talkDisposable != null)
            this._talkDisposable.Dispose();
          this._talkDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator2), false));
          MerchantActor merchantActor = listener as MerchantActor;
          merchantActor.CommandPartner = this;
          merchantActor.ChangeBehavior(Merchant.ActionType.TalkWithAgent);
          break;
      }
    }

    [DebuggerHidden]
    private IEnumerator StartTalkSequenceCoroutine(
      Actor listener,
      int speakID,
      int listenID)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Actor.\u003CStartTalkSequenceCoroutine\u003Ec__Iterator0()
      {
        listener = listener,
        speakID = speakID,
        listenID = listenID,
        \u0024this = this
      };
    }

    public bool LivesTalkSequence
    {
      get
      {
        return this._talkEnumerator != null;
      }
    }

    public void StopTalkSequence()
    {
      if (this._talkDisposable != null)
      {
        this._talkDisposable.Dispose();
        this._talkDisposable = (IDisposable) null;
      }
      if (this._talkEnumerator == null)
        return;
      this._talkEnumerator = (IEnumerator) null;
    }

    public void StartWithAnimalSequence(AnimalBase animal)
    {
      this.StartWithAnimalSequence(animal, 0, 0);
    }

    public void StartWithAnimalSequence(AnimalBase animal, int poseID)
    {
      this.StartWithAnimalSequence(animal, poseID, poseID);
    }

    public void StartWithAnimalSequence(AnimalBase animal, int actorPoseID, int animalPoseID)
    {
      if (Object.op_Equality((Object) animal, (Object) null))
        return;
      switch (this)
      {
        case AgentActor _:
        case PlayerActor _:
          this._withAnimalEnumerator = this.StartWithAnimalSequenceCoroutine(animal, actorPoseID, animalPoseID);
          animal.SetImpossible(true, this);
          animal.SetState(!(this is AgentActor) ? AnimalState.WithPlayer : AnimalState.WithAgent, (System.Action) null);
          if (this._withAnimalDisposable != null)
            this._withAnimalDisposable.Dispose();
          this._withAnimalDisposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._withAnimalEnumerator), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex =>
          {
            Debug.LogException(ex);
            this._withAnimalEnumerator = (IEnumerator) null;
          }), (System.Action) (() => this._withAnimalEnumerator = (IEnumerator) null)), (Component) this);
          break;
      }
    }

    [DebuggerHidden]
    private IEnumerator StartWithAnimalSequenceCoroutine(
      AnimalBase animal,
      int actorPoseID,
      int animalPoseID)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Actor.\u003CStartWithAnimalSequenceCoroutine\u003Ec__Iterator1()
      {
        animal = animal,
        animalPoseID = animalPoseID,
        actorPoseID = actorPoseID,
        \u0024this = this
      };
    }

    public void StopWithAnimalSequence()
    {
      if (this._withAnimalDisposable != null)
      {
        this._withAnimalDisposable.Dispose();
        this._withAnimalDisposable = (IDisposable) null;
      }
      this._withAnimalEnumerator = (IEnumerator) null;
    }

    public bool IsMatchAnimalWithActor(AnimalBase animal)
    {
      if (Object.op_Equality((Object) animal, (Object) null) || !animal.Active)
        return false;
      switch (animal.CurrentState)
      {
        case AnimalState.WithPlayer:
          return this is PlayerActor;
        case AnimalState.WithAgent:
          return this is AgentActor;
        case AnimalState.WithMerchant:
          return this is MerchantActor;
        default:
          return false;
      }
    }

    public bool LivesWithAnimalSequence
    {
      get
      {
        return this._withAnimalEnumerator != null;
      }
    }

    private void Start()
    {
      this._rigidbody.set_sleepThreshold(-1f);
      this.OnStart();
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (System.Action<M0>) (_ =>
      {
        if (Object.op_Inequality((Object) this._rigidbody, (Object) null) && this._rigidbody.IsSleeping())
          this._rigidbody.WakeUp();
        GraphicSystem graphicData = Manager.Config.GraphicData;
        if (Object.op_Equality((Object) this.Controller, (Object) null) || graphicData == null)
          return;
        this.Controller.FaceLightActive = graphicData.FaceLight;
      }));
    }

    [DebuggerHidden]
    public virtual IEnumerator LoadAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Actor.\u003CLoadAsync\u003Ec__Iterator2 loadAsyncCIterator2 = new Actor.\u003CLoadAsync\u003Ec__Iterator2();
      return (IEnumerator) loadAsyncCIterator2;
    }

    public virtual void LoadEquipments()
    {
    }

    protected void LoadEquipmentItem(StuffItem item, ChaControlDefine.ExtraAccessoryParts parts)
    {
      int id = item.CategoryID == -1 ? -1 : item.ID;
      this.ChaControl.ChangeExtraAccessory(parts, id);
      this.ChaControl.ShowExtraAccessory(parts, true);
    }

    protected virtual void InitializeIK()
    {
      this.IsInit = true;
    }

    protected abstract IEnumerator LoadCharAsync(string fileName);

    protected abstract void OnStart();

    public virtual void OnDayUpdated(TimeSpan timeSpan)
    {
    }

    public abstract void OnMinuteUpdated(TimeSpan timeSpan);

    public virtual void OnSecondUpdated(TimeSpan timeSpan)
    {
    }

    private void InitializeRagdoll()
    {
    }

    protected float GetSlopeDamper(Vector3 velocity, Vector3 groundNormal)
    {
      return 1f - Mathf.Clamp((90f - Vector3.Angle(velocity, groundNormal) - this._slopeStartAngle) / (this._slopeEndAngle - this._slopeStartAngle), 0.0f, 1f);
    }

    public virtual bool CanUnlock(int category, int id)
    {
      return false;
    }

    public virtual bool CanAddItem(StuffItem item)
    {
      return true;
    }

    public virtual bool CanAddItem(StuffItemInfo item)
    {
      return true;
    }

    public int Lottery(Dictionary<int, ItemTableElement> table)
    {
      float num1 = 0.0f;
      foreach (KeyValuePair<int, ItemTableElement> keyValuePair in table)
        num1 += keyValuePair.Value.Rate;
      float num2 = Random.Range(0.0f, num1);
      int num3 = -1;
      foreach (KeyValuePair<int, ItemTableElement> keyValuePair in table)
      {
        float rate = keyValuePair.Value.Rate;
        if ((double) num2 <= (double) rate)
        {
          num3 = keyValuePair.Key;
          break;
        }
        num2 -= rate;
      }
      return num3;
    }

    public ItemTableElement.GatherElement Lottery(
      List<ItemTableElement.GatherElement> list)
    {
      float num1 = 0.0f;
      foreach (ItemTableElement.GatherElement gatherElement in list)
        num1 += gatherElement.prob;
      float num2 = Random.Range(0.0f, num1);
      int index = -1;
      int num3 = 0;
      foreach (ItemTableElement.GatherElement gatherElement in list)
      {
        float prob = gatherElement.prob;
        if ((double) num2 <= (double) prob)
        {
          index = num3;
          break;
        }
        num2 -= prob;
        ++num3;
      }
      return list[index];
    }

    public virtual Actor.SearchInfo RandomAddItem(
      Dictionary<int, ItemTableElement> itemTable,
      bool containsNone)
    {
      Resources instance = Singleton<Resources>.Instance;
      if (itemTable == null)
        return new Actor.SearchInfo() { IsSuccess = false };
      Resources.GameInfoTables gameInfo = Singleton<Resources>.Instance.GameInfo;
      int key = this.Lottery(itemTable);
      ItemTableElement itemTableElement;
      if (itemTable.TryGetValue(key, out itemTableElement))
      {
        Actor.SearchInfo searchInfo = new Actor.SearchInfo()
        {
          IsSuccess = true
        };
        foreach (ItemTableElement.GatherElement element in itemTableElement.Elements)
        {
          if ((double) Random.get_value() < (double) element.prob)
          {
            StuffItemInfo stuffItemInfo = gameInfo.GetItem(element.categoryID, element.itemID);
            if (stuffItemInfo != null)
            {
              int num = Random.Range(element.minCount, element.maxCount + 1);
              if (num > 0)
                searchInfo.ItemList.Add(new Actor.ItemSearchInfo()
                {
                  name = stuffItemInfo.Name,
                  categoryID = element.categoryID,
                  id = element.itemID,
                  count = num
                });
            }
          }
        }
        if (searchInfo.ItemList.IsNullOrEmpty<Actor.ItemSearchInfo>())
          searchInfo.IsSuccess = false;
        return searchInfo;
      }
      return new Actor.SearchInfo() { IsSuccess = false };
    }

    public void ResetEquipEventItem(EquipEventItemInfo itemInfo)
    {
      if (this.EquipedItem != null)
      {
        if (itemInfo != null)
        {
          if (this.EquipedItem.EventItemID == itemInfo.EventItemID)
          {
            if (this.EquipedItem.AsGameObject.get_activeSelf())
              return;
            this.EquipedItem.AsGameObject.SetActive(true);
          }
          else
          {
            this.ReleaseEquipedEventItem();
            this.LoadEquipedEventItem(itemInfo);
          }
        }
        else
          this.ReleaseEquipedEventItem();
      }
      else
      {
        if (itemInfo == null)
          return;
        this.LoadEquipedEventItem(itemInfo);
      }
    }

    public virtual void LoadEventItems(PlayState playState)
    {
    }

    public virtual void LoadEventParticles(int eventID, int poseID)
    {
    }

    public void ReleaseEquipedEventItem()
    {
      if (this.EquipedItem == null)
        return;
      if (Object.op_Inequality((Object) this.EquipedItem.AsGameObject, (Object) null))
      {
        Object.Destroy((Object) this.EquipedItem.AsGameObject);
        this.EquipedItem.AsGameObject = (GameObject) null;
      }
      this.EquipedItem = (ItemCache) null;
    }

    protected abstract void LoadEquipedEventItem(EquipEventItemInfo eventItemInfo);

    public void LoadEventItem(
      int itemID,
      PlayState.ItemInfo itemInfo,
      ActionItemInfo eventItemInfo)
    {
      this.LoadEventItem(itemID, itemInfo.parentName, itemInfo.isSync, eventItemInfo);
    }

    public GameObject LoadEventItem(
      int itemID,
      string parentName,
      bool isSync,
      ActionItemInfo eventItemInfo)
    {
      GameObject gameObject = (GameObject) null;
      if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        gameObject = this.CurrentPoint.CreateEventItems(itemID, parentName, isSync);
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        gameObject = CommonLib.LoadAsset<GameObject>((string) eventItemInfo.assetbundleInfo.assetbundle, (string) eventItemInfo.assetbundleInfo.asset, false, (string) eventItemInfo.assetbundleInfo.manifest);
        if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == (string) eventItemInfo.assetbundleInfo.assetbundle && (string) x.Item2 == (string) eventItemInfo.assetbundleInfo.manifest)))
          MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>((string) eventItemInfo.assetbundleInfo.assetbundle, (string) eventItemInfo.assetbundleInfo.manifest));
      }
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        GameObject loop = ((Component) this.ChaControl.animBody).get_transform().FindLoop(parentName);
        GameObject self = (GameObject) Object.Instantiate<GameObject>((M0) gameObject, loop.get_transform(), false);
        self.SetActiveIfDifferent(true);
        ((Object) self.get_gameObject()).set_name(((Object) gameObject.get_gameObject()).get_name());
        self.get_transform().set_localPosition(Vector3.get_zero());
        self.get_transform().set_localRotation(Quaternion.get_identity());
        self.get_transform().set_localScale(Vector3.get_one());
        this.Animation.Items.Add(new ValueTuple<int, GameObject>(itemID, self));
        Renderer[] componentsInChildren = (Renderer[]) self.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in componentsInChildren)
          renderer.set_enabled(false);
        this.Animation.ItemRenderers.Add(new ValueTuple<int, Renderer[]>(itemID, componentsInChildren));
        if (eventItemInfo.existsAnimation)
        {
          Animator component = (Animator) self.GetComponent<Animator>();
          RuntimeAnimatorController animatorController = AssetUtility.LoadAsset<RuntimeAnimatorController>(eventItemInfo.animeAssetBundle);
          component.set_runtimeAnimatorController(animatorController);
          this.Animation.ItemAnimatorTable[((Object) self).GetInstanceID()] = new ItemAnimInfo()
          {
            Animator = component,
            Parameters = component.get_parameters(),
            Sync = isSync
          };
        }
        Resources.TriValues triValues;
        if (Singleton<Resources>.Instance.Animation.ItemScaleTable.TryGetValue(itemID, out triValues))
          this._scaleCtrlInfos.Add(new Actor.ItemScaleInfo()
          {
            TargetItem = self,
            ScaleMode = triValues.ScaleType,
            SThreshold = triValues.SThreshold,
            MThreshold = triValues.MThreshold,
            LThreshold = triValues.LThreshold
          });
        return self;
      }
      AssetBundleInfo assetbundleInfo = eventItemInfo.assetbundleInfo;
      Debug.LogError((object) string.Format("イベントアイテム読み込み失敗: バンドルパス[{0}] プレハブ[{1}] マニフェスト[{2}]", (object) assetbundleInfo.assetbundle, (object) assetbundleInfo.asset, (object) assetbundleInfo.manifest));
      return (GameObject) null;
    }

    protected void LoadEventParticle(
      Dictionary<int, List<AnimeParticleEventInfo>> eventTable)
    {
      this.ClearParticles();
      foreach (KeyValuePair<int, List<AnimeParticleEventInfo>> keyValuePair in eventTable)
      {
        if (!keyValuePair.Value.IsNullOrEmpty<AnimeParticleEventInfo>())
        {
          foreach (AnimeParticleEventInfo particleEventInfo in keyValuePair.Value)
          {
            if (particleEventInfo.EventID % 2 != 1)
            {
              int particleId = particleEventInfo.ParticleID;
              string root = particleEventInfo.Root;
              int key = particleEventInfo.EventID / 2;
              Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>> dictionary;
              if (!this.Animation.Particles.TryGetValue(key, out dictionary))
                this.Animation.Particles[key] = dictionary = new Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>();
              AssetBundleInfo assetInfo;
              if (!dictionary.ContainsKey(particleId) && Singleton<Resources>.Instance.Map.EventParticleTable.TryGetValue(particleId, out assetInfo))
              {
                GameObject gameObject1 = CommonLib.LoadAsset<GameObject>((string) assetInfo.assetbundle, (string) assetInfo.asset, false, (string) assetInfo.manifest);
                if (!Object.op_Equality((Object) gameObject1, (Object) null))
                {
                  if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == (string) assetInfo.assetbundle && (string) x.Item2 == (string) assetInfo.manifest)))
                    MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>((string) assetInfo.assetbundle, (string) assetInfo.manifest));
                  Transform transform = ((Component) this.ChaControl.animBody).get_transform().FindLoop(root)?.get_transform() ?? ((Component) this.Locomotor).get_transform();
                  GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, transform, false);
                  ParticleSystem particleSystem = (ParticleSystem) gameObject2.GetComponent<ParticleSystem>();
                  if (Object.op_Equality((Object) particleSystem, (Object) null))
                    particleSystem = (ParticleSystem) gameObject2.GetComponentInChildren<ParticleSystem>();
                  if (Object.op_Equality((Object) particleSystem, (Object) null))
                  {
                    if (Object.op_Inequality((Object) gameObject2, (Object) null))
                      Object.Destroy((Object) gameObject2);
                  }
                  else
                  {
                    ((Object) gameObject2).set_name(((Object) gameObject1).get_name());
                    gameObject2.get_transform().set_localPosition(Vector3.get_zero());
                    gameObject2.get_transform().set_localRotation(Quaternion.get_identity());
                    gameObject2.get_transform().set_localScale(Vector3.get_one());
                    ParticleSystem.MainModule main = particleSystem.get_main();
                    if (((ParticleSystem.MainModule) ref main).get_playOnAwake())
                      ((ParticleSystem.MainModule) ref main).set_playOnAwake(false);
                    if (particleSystem.get_isPlaying())
                      particleSystem.Stop(true, (ParticleSystemStopBehavior) 0);
                    ParticleSystemRenderer component = (ParticleSystemRenderer) ((Component) particleSystem).GetComponent<ParticleSystemRenderer>();
                    dictionary[particleId] = new Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>(gameObject2, particleSystem, component);
                  }
                }
              }
            }
          }
        }
      }
    }

    public void SetDefaultStateHousingItem()
    {
      if (!Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        return;
      string animationDefault = Singleton<Resources>.Instance.DefinePack.AnimatorState.HousingAnimationDefault;
      HousingActionPointAnimation[] componentsInChildren = (HousingActionPointAnimation[]) ((Component) this.CurrentPoint).GetComponentsInChildren<HousingActionPointAnimation>(true);
      if (componentsInChildren.IsNullOrEmpty<HousingActionPointAnimation>())
        return;
      foreach (ActionPointAnimation actionPointAnimation in componentsInChildren)
        actionPointAnimation.Animator.Play(animationDefault, 0);
    }

    public void ChangeAnimator(string assetbundle, string asset)
    {
      RuntimeAnimatorController rac = AssetUtility.LoadAsset<RuntimeAnimatorController>(assetbundle, asset, string.Empty);
      if (!Object.op_Inequality((Object) rac, (Object) null))
        return;
      this.Animation.SetAnimatorController(rac);
    }

    public void ChangeAnimator(RuntimeAnimatorController rac)
    {
      if (!Object.op_Inequality((Object) rac, (Object) null))
        return;
      this.Animation.SetAnimatorController(rac);
    }

    public void ClearItems()
    {
      this.Animation.ClearItems();
      this._scaleCtrlInfos.Clear();
    }

    public void SetActiveOnEquipedItem(bool active)
    {
      if (this.EquipedItem == null || Object.op_Equality((Object) this.EquipedItem.AsGameObject, (Object) null) || this.EquipedItem.AsGameObject.get_activeSelf() == active)
        return;
      this.EquipedItem.AsGameObject.SetActive(active);
    }

    public void ClearParticles()
    {
      this.Animation.ClearParticles();
    }

    public void SetStand(Transform t, bool enableFade, float fadeTime, int dirType)
    {
      if (this._standDisposable != null)
        this._standDisposable.Dispose();
      IConnectableObservable<TimeInterval<float>> iconnectableObservable = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(fadeTime, false), false));
      this._standDisposable = iconnectableObservable.Connect();
      Vector3 position = this.Position;
      Quaternion rotation = this.Rotation;
      if (!Object.op_Inequality((Object) t, (Object) null))
        return;
      switch (dirType)
      {
        case 0:
          if (enableFade)
          {
            ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (System.Action<M0>) (x =>
            {
              this.Position = Vector3.Lerp(position, t.get_position(), ((TimeInterval<float>) ref x).get_Value());
              this.Rotation = Quaternion.Slerp(rotation, t.get_rotation(), ((TimeInterval<float>) ref x).get_Value());
            }));
            break;
          }
          if (((Behaviour) this._navMeshAgent).get_enabled())
          {
            this.NavMeshWarp(t, 0, 100f);
            break;
          }
          this.Position = t.get_position();
          this.Rotation = t.get_rotation();
          break;
        case 1:
          Vector3 position1 = ((Component) this).get_transform().get_position();
          position1.y = (__Null) 0.0;
          Vector3 position2 = this.Position;
          position2.y = (__Null) 0.0;
          Quaternion lookRotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.op_Subtraction(position1, position2)));
          if (enableFade)
          {
            ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (System.Action<M0>) (x => this.Rotation = Quaternion.Slerp(rotation, lookRotation, ((TimeInterval<float>) ref x).get_Value())));
            break;
          }
          this.Rotation = lookRotation;
          break;
      }
    }

    public void StopStand()
    {
      if (this._standDisposable == null)
        return;
      this._standDisposable.Dispose();
      this._standDisposable = (IDisposable) null;
    }

    public void NavMeshWarp(Transform t, int idx, float limitDistance = 100f)
    {
      if (Object.op_Equality((Object) t, (Object) null))
        return;
      this.NavMeshWarp(t.get_position(), t.get_rotation(), idx, limitDistance);
    }

    public void NavMeshWarp(Vector3 position, Quaternion rotation, int idx, float limitDistance = 100f)
    {
      if (!this._navMeshAgent.Warp(position))
      {
        List<Vector3> vector3List = ListPool<Vector3>.Get();
        for (int index = 0; index < 3; ++index)
        {
          Vector3 queryPos;
          ((Vector3) ref queryPos).\u002Ector(0.0f, 0.0f, (float) (10 + index * 10));
          this.SearchAroundPosition(position, queryPos, vector3List);
        }
        Vector3? nullable = new Vector3?();
        if (!vector3List.IsNullOrEmpty<Vector3>())
          nullable = new Vector3?(this.Nearest(position, vector3List));
        ListPool<Vector3>.Release(vector3List);
        if (nullable.HasValue)
        {
          this._navMeshAgent.Warp(nullable.Value);
        }
        else
        {
          NavMeshHit navMeshHit;
          if (NavMesh.SamplePosition(position, ref navMeshHit, limitDistance, this._navMeshAgent.get_areaMask()))
          {
            this._navMeshAgent.Warp(((NavMeshHit) ref navMeshHit).get_position());
          }
          else
          {
            foreach (BasePoint basePoint in Singleton<Manager.Map>.Instance.PointAgent.BasePoints)
            {
              if (basePoint.AreaIDInHousing == this.AreaID)
                this._navMeshAgent.Warp(basePoint.RecoverPoints[0].get_position());
            }
          }
        }
      }
      this.Rotation = rotation;
    }

    private void SearchAroundPosition(Vector3 center, Vector3 queryPos, List<Vector3> list)
    {
      for (int index = 0; index < 9; ++index)
      {
        Quaternion quaternion = Quaternion.Euler(0.0f, (float) (40 * index), 0.0f);
        Vector3 vector3 = Vector3.op_Addition(center, Quaternion.op_Multiply(quaternion, queryPos));
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(vector3, ref navMeshHit, 10f, this._navMeshAgent.get_areaMask()))
          list.Add(vector3);
      }
    }

    private Vector3 Nearest(Vector3 center, List<Vector3> list)
    {
      if (list.IsNullOrEmpty<Vector3>())
        return Vector3.get_zero();
      float num1 = float.MaxValue;
      Vector3 vector3 = Vector3.get_zero();
      using (List<Vector3>.Enumerator enumerator = list.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Vector3 current = enumerator.Current;
          float num2 = Vector3.Distance(center, current);
          if ((double) num1 > (double) num2)
          {
            num1 = num2;
            vector3 = current;
          }
        }
      }
      return vector3;
    }

    public List<Actor.ItemScaleInfo> ScaleCtrlInfo
    {
      get
      {
        return this._scaleCtrlInfos;
      }
    }

    public void SetLookPtn(int ptnEye, int ptnNeck)
    {
      this.ChaControl.ChangeLookEyesPtn(ptnEye);
      this.ChaControl.ChangeLookNeckPtn(ptnNeck, 1f);
    }

    public void SetLookTarget(int targetEye, int targetNeck, Transform target = null)
    {
      this.ChaControl.ChangeLookEyesTarget(targetEye, target, 0.5f, 0.0f, 1f, 2f);
      this.ChaControl.ChangeLookNeckTarget(targetNeck, target, 0.5f, 0.0f, 1f, 0.8f);
    }

    public bool IsVisibleDistanceAll(Transform cameraTransform)
    {
      float charaVisibleDistance = Singleton<Resources>.Instance.LocomotionProfile.CharaVisibleDistance;
      foreach (Transform chaFovTarget in this.ChaFovTargets)
      {
        if ((double) Vector3.Distance(chaFovTarget.get_position(), cameraTransform.get_position()) < (double) charaVisibleDistance)
          return false;
      }
      return true;
    }

    public bool IsVisibleDistance(
      Transform cameraTransform,
      Actor.BodyPart parts,
      float limitDistance)
    {
      Transform transform;
      return this.ChaBodyParts.TryGetValue(parts, ref transform) && (double) Vector3.Distance(transform.get_position(), cameraTransform.get_position()) >= (double) limitDistance;
    }

    public struct BehaviorSchedule
    {
      public bool enabled;
      public string name;
      public float duration;
      public float elapsedTime;
      public bool useGameTime;
      public bool progress;

      public BehaviorSchedule(bool enabled_, string name_, float duration_)
      {
        this.enabled = enabled_;
        this.name = name_;
        this.duration = duration_;
        this.elapsedTime = 0.0f;
        this.useGameTime = false;
        this.progress = true;
      }
    }

    public struct InputInfo
    {
      public Vector3 move;
      public Vector3 lookPos;

      public void Init()
      {
        this.move = Vector3.get_zero();
        this.lookPos = Vector3.get_zero();
      }
    }

    public enum FovBodyPart
    {
      Body,
      Head,
      Foot,
    }

    public enum BodyPart
    {
      Body,
      Bust,
      Head,
      LeftFoot,
      RightFoot,
    }

    public class SearchInfo
    {
      public bool IsSuccess { get; set; }

      public List<Actor.ItemSearchInfo> ItemList { get; set; } = new List<Actor.ItemSearchInfo>();
    }

    public struct ItemSearchInfo
    {
      public string name;
      public int categoryID;
      public int id;
      public int count;
    }

    public class ItemScaleInfo
    {
      public GameObject TargetItem { get; set; }

      public int ScaleMode { get; set; }

      public float SThreshold { get; set; }

      public float MThreshold { get; set; }

      public float LThreshold { get; set; }

      public float Evaluate(float t)
      {
        t = Mathf.Clamp01(t);
        return (double) t < 0.5 ? Mathf.Lerp(this.SThreshold, this.MThreshold, Mathf.InverseLerp(0.0f, 0.5f, t)) : Mathf.Lerp(this.MThreshold, this.LThreshold, Mathf.InverseLerp(0.5f, 1f, t));
      }
    }
  }
}
