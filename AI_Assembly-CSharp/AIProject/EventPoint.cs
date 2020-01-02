// Decompiled with JetBrains decompiler
// Type: AIProject.EventPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIProject.Definitions;
using AIProject.Player;
using AIProject.SaveData;
using AIProject.UI;
using AIProject.UI.Popup;
using Cinemachine;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  public class EventPoint : Point, ICommandable
  {
    public static readonly string PlayerPointName = "player_point";
    public static readonly string MerchantPointName = "merchant_point";
    public static readonly string HeroinePointName = "heroine_point";
    public static readonly string RecoverPointName = "recover_point";
    public static readonly string SEPointName = "se_point";
    public static readonly string AnimStartPointName = "anim_start_point";
    public static readonly string AnimEndPointName = "anim_end_point";
    private static Subject<Unit> _commandRefreshEvent = (Subject<Unit>) null;
    private static bool _changeNoneMode = false;
    private static Dictionary<int, Dictionary<int, EventPoint>> _eventPointTable = new Dictionary<int, Dictionary<int, EventPoint>>();
    private static NavMeshPath _getNavPath = (NavMeshPath) null;
    [SerializeField]
    private int _pointID = -1;
    [SerializeField]
    private bool _enableRangeCheck = true;
    [SerializeField]
    private float _checkRadius = 1f;
    private bool _isHold = true;
    private List<CommandLabel.CommandInfo[]> _labelsList = new List<CommandLabel.CommandInfo[]>();
    private List<CommandLabel.CommandInfo[]> _dateLabelsList = new List<CommandLabel.CommandInfo[]>();
    protected IntReactiveProperty _labelIndex = new IntReactiveProperty(0);
    protected IntReactiveProperty _dateLabelIndex = new IntReactiveProperty(0);
    private EventPoint.BackUpInfo _merchantBackUp = new EventPoint.BackUpInfo();
    private Dictionary<int, EventPoint.BackUpInfo> _agentBackUpTable = new Dictionary<int, EventPoint.BackUpInfo>();
    [SerializeField]
    private int _groupID;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _labelPoint;
    [SerializeField]
    private CommandType _commandType;
    private int? _instanceID;
    private NavMeshPath _pathForCalc;
    private int _flavorBorder;
    protected List<GameObject> _safeguardObjects;
    protected bool _safeguardObjectFlag;
    private Transform _playerPoint;
    private Transform _merchantPoint;
    private Transform _recoverPoint;
    private CinemachineBlendDefinition.Style _prevCameraStyle;
    private Dictionary<int, AgentActor> _normalChangeActorTable;

    public int GroupID
    {
      get
      {
        return this._groupID;
      }
    }

    public int PointID
    {
      get
      {
        return this._pointID;
      }
    }

    public Transform LabelPoint
    {
      get
      {
        return Object.op_Inequality((Object) this._labelPoint, (Object) null) ? this._labelPoint : ((Component) this).get_transform();
      }
    }

    public bool EnableRangeCheck
    {
      get
      {
        return this._enableRangeCheck;
      }
    }

    public float CheckRadius
    {
      get
      {
        return this._checkRadius;
      }
    }

    public int OpenAreaID { get; protected set; } = -1;

    public EventPoint.EventTypes EventType { get; protected set; } = EventPoint.EventTypes.Other;

    public int EventID { get; protected set; } = -1;

    public int WarningID { get; protected set; } = -1;

    private OpenData _openData { get; } = new OpenData();

    private EventPoint.PackData _eventStoryPackData { get; set; }

    private EventPoint.MessagePackData _messagePackData { get; set; }

    private EventPoint.MessageCommandData _messageCommandData { get; set; }

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
      if (this.IsStopped || this._groupID == 2)
        return false;
      Vector3 commandCenter = this.CommandCenter;
      commandCenter.y = (__Null) 0.0;
      float num1 = Vector3.Distance(basePosition, commandCenter);
      bool flag;
      if (this._commandType == CommandType.Forward)
      {
        if ((!this._enableRangeCheck ? (double) radiusA : (double) radiusA + (double) this._checkRadius) < (double) num1)
          return false;
        Vector3 vector3 = commandCenter;
        vector3.y = (__Null) 0.0;
        float num2 = angle / 2f;
        flag = (double) Vector3.Angle(Vector3.op_Subtraction(vector3, basePosition), forward) <= (double) num2;
      }
      else
      {
        float num2 = !this._enableRangeCheck ? radiusB : radiusB + this._checkRadius;
        flag = (double) distance <= (double) num2;
      }
      if (!flag)
        return false;
      PlayerActor player = Manager.Map.GetPlayer();
      return (!Object.op_Inequality((Object) player, (Object) null) || !(player.PlayerController.State is Onbu)) && this.IsNeutralCommand && flag;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      bool flag2;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        if (this._groupID == 1 && this._pointID == 3)
          return true;
        nmAgent.CalculatePath(this.Position, this._pathForCalc);
        flag2 = flag1 & this._pathForCalc.get_status() == 0;
        if (!flag2)
          return false;
        float num1 = 0.0f;
        Vector3[] corners = this._pathForCalc.get_corners();
        float num2 = this.CommandType != CommandType.Forward ? radiusB : radiusA;
        for (int index = 0; index < corners.Length - 1; ++index)
        {
          float num3 = Vector3.Distance(corners[index], corners[index + 1]);
          num1 += num3;
          if ((double) num2 < (double) num1)
          {
            flag2 = false;
            break;
          }
        }
      }
      else
        flag2 = false;
      return flag2;
    }

    public bool IsImpossible { get; protected set; }

    public Actor CommandPartner { get; private set; }

    public bool SetImpossible(bool value, Actor actor)
    {
      if (this.IsImpossible == value || Object.op_Inequality((Object) this.CommandPartner, (Object) null) && Object.op_Inequality((Object) this.CommandPartner, (Object) actor))
        return false;
      this.IsImpossible = value;
      this.CommandPartner = actor;
      return true;
    }

    public bool IsRunable { get; set; } = true;

    public bool IsNeutralCommand
    {
      get
      {
        return ((Behaviour) this).get_isActiveAndEnabled() && this.IsRunable && this.IsNeutral;
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
        return Object.op_Inequality((Object) this._commandBasePoint, (Object) null) ? this._commandBasePoint.get_position() : ((Component) this).get_transform().get_position();
      }
    }

    public ObjectLayer Layer
    {
      get
      {
        return ObjectLayer.Command;
      }
    }

    public CommandType CommandType
    {
      get
      {
        return this._commandType;
      }
    }

    public int LabelIndex
    {
      get
      {
        return ((ReactiveProperty<int>) this._labelIndex).get_Value();
      }
    }

    public int DateLabelIndex
    {
      get
      {
        return ((ReactiveProperty<int>) this._dateLabelIndex).get_Value();
      }
    }

    public static int TargetGroupID { get; set; } = -1;

    public static int TargetPointID { get; set; } = -1;

    public bool TargetPoint
    {
      get
      {
        return this._groupID == EventPoint.TargetGroupID && this._pointID == EventPoint.TargetPointID;
      }
    }

    public static ValueTuple<int, int> GetTargetID()
    {
      return new ValueTuple<int, int>(EventPoint.TargetGroupID, EventPoint.TargetPointID);
    }

    public static EventPoint GetTargetPoint()
    {
      return EventPoint.Get(EventPoint.TargetGroupID, EventPoint.TargetPointID);
    }

    public static void SetTargetID(int groupID, int pointID)
    {
      EventPoint.TargetGroupID = groupID;
      EventPoint.TargetPointID = pointID;
    }

    public static void SetTargetID(EventPoint point)
    {
      if (Object.op_Inequality((Object) point, (Object) null))
      {
        EventPoint.TargetGroupID = point.GroupID;
        EventPoint.TargetPointID = point.PointID;
      }
      else
      {
        EventPoint.TargetGroupID = -1;
        EventPoint.TargetPointID = -1;
      }
    }

    public bool IsFreeMode { get; private set; }

    public bool IsStopped { get; private set; }

    protected void SetSafeguardObjectActive(bool setFlag, bool forced = false)
    {
      if (this._safeguardObjectFlag != setFlag || forced)
      {
        Debug.Log((object) string.Format("SetSafetuardObjectActive setFlag[{0}] forced[{1}] _safeFlag[{2}]", (object) setFlag, (object) forced, (object) this._safeguardObjectFlag));
        if (!this._safeguardObjects.IsNullOrEmpty<GameObject>())
        {
          using (List<GameObject>.Enumerator enumerator = this._safeguardObjects.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GameObject current = enumerator.Current;
              if (Object.op_Inequality((Object) current, (Object) null) && current.get_activeSelf() != setFlag)
                current.SetActive(setFlag);
            }
          }
        }
      }
      this._safeguardObjectFlag = setFlag;
    }

    protected virtual void Awake()
    {
      this.IsFreeMode = Game.IsFreeMode;
      this.IsStopped = this.CheckStopState();
      if (EventPoint._commandRefreshEvent == null)
      {
        EventPoint._commandRefreshEvent = new Subject<Unit>();
        ObservableExtensions.Subscribe<IList<Unit>>(Observable.Where<IList<Unit>>(Observable.TakeUntilDestroy<IList<Unit>>((IObservable<M0>) Observable.Buffer<Unit, Unit>((IObservable<M0>) EventPoint._commandRefreshEvent, (IObservable<M1>) Observable.ThrottleFrame<Unit>((IObservable<M0>) EventPoint._commandRefreshEvent, 1, (FrameCountType) 0)), (Component) Singleton<Manager.Map>.Instance), (Func<M0, bool>) (_ => !_.IsNullOrEmpty<Unit>())), (System.Action<M0>) (_ =>
        {
          CommandArea commandArea = Singleton<Manager.Map>.Instance?.Player?.PlayerController?.CommandArea;
          if (Object.op_Equality((Object) commandArea, (Object) null))
            return;
          commandArea.RefreshCommands();
        }));
      }
      Dictionary<int, EventPoint> dictionary;
      if (!EventPoint._eventPointTable.TryGetValue(this._groupID, out dictionary))
        EventPoint._eventPointTable[this._groupID] = dictionary = new Dictionary<int, EventPoint>();
      if (!dictionary.ContainsKey(this._pointID))
        dictionary[this._pointID] = this;
      if (this._groupID != 2 || this._pointID != 1)
        return;
      MerchantProfile merchantProfile = !Singleton<Resources>.IsInstance() ? (MerchantProfile) null : Singleton<Resources>.Instance.MerchantProfile;
      if (!Object.op_Inequality((Object) merchantProfile, (Object) null))
        return;
      this._safeguardObjects = MapItemData.Get(merchantProfile.LastADVSafeguardID);
      this.SetSafeguardObjectActive(false, true);
      if (this.GetDedicatedNumber() != 0)
        return;
      ObservableExtensions.Subscribe<long>(Observable.TakeWhile<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => this.GetDedicatedNumber() == 0)), (System.Action<M0>) (_ =>
      {
        PlayerActor player = Manager.Map.GetPlayer();
        if (Object.op_Equality((Object) player, (Object) null))
          return;
        bool setFlag = false | player.PlayerController.State is Follow;
        if (!setFlag)
        {
          AgentActor agentActor = player.CommCompanion as AgentActor;
          if (Object.op_Equality((Object) agentActor, (Object) null))
            agentActor = player.Partner as AgentActor;
          setFlag = Object.op_Inequality((Object) agentActor, (Object) null) && (agentActor.Mode == Desire.ActionType.EndTaskH || agentActor.Mode == Desire.ActionType.ReverseRape || agentActor.Mode == Desire.ActionType.TakeHPoint);
        }
        this.SetSafeguardObjectActive(setFlag, false);
      }), (System.Action<Exception>) (ex => Debug.LogException(ex)), (System.Action) (() => this.SetSafeguardObjectActive(false, true)));
    }

    public bool CheckStopState()
    {
      switch (this._groupID)
      {
        case 0:
          if (this.IsFreeMode && 0 <= this._pointID && this._pointID <= 6)
            return true;
          break;
        case 1:
          if (this.IsFreeMode && 0 <= this._pointID && this._pointID <= 6)
            return true;
          break;
        case 2:
          if (this.IsFreeMode && 0 <= this._pointID && this._pointID <= 1)
            return true;
          break;
      }
      return false;
    }

    public void RemoveConsiderationCommand()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      CommandArea commandArea = Singleton<Manager.Map>.Instance.Player?.PlayerController?.CommandArea;
      bool? nullable = commandArea?.ContainsConsiderationObject((ICommandable) this);
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0)
        return;
      commandArea.RemoveConsiderationObject((ICommandable) this);
      if (EventPoint._commandRefreshEvent == null)
        return;
      EventPoint._commandRefreshEvent.OnNext(Unit.get_Default());
    }

    protected override void Start()
    {
      if (this.IsStopped)
        return;
      base.Start();
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName);
        this._commandBasePoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      if (Object.op_Equality((Object) this._labelPoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.EventPointLabelTargetName);
        this._labelPoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      this.InitializeCommandLabels();
      ObservableExtensions.Subscribe<int>(Observable.DistinctUntilChanged<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._labelIndex, ((Component) this).get_gameObject())), (System.Action<M0>) (idx =>
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return;
        bool? nullable = Singleton<Manager.Map>.Instance.Player?.PlayerController?.CommandArea?.ContainsConsiderationObject((ICommandable) this);
        if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0 && EventPoint._commandRefreshEvent != null)
          EventPoint._commandRefreshEvent.OnNext(Unit.get_Default());
        if (!Singleton<MapUIContainer>.IsInstance() || MapUIContainer.CommandLabel.Acception == CommandLabel.AcceptionState.InvokeAcception)
          return;
        EventPoint._changeNoneMode = true;
      }));
      ObservableExtensions.Subscribe<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._dateLabelIndex, ((Component) this).get_gameObject()), (System.Action<M0>) (idx =>
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return;
        bool? nullable = Singleton<Manager.Map>.Instance.Player?.PlayerController?.CommandArea?.ContainsConsiderationObject((ICommandable) this);
        if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0 && EventPoint._commandRefreshEvent != null)
          EventPoint._commandRefreshEvent.OnNext(Unit.get_Default());
        if (!Singleton<MapUIContainer>.IsInstance() || MapUIContainer.CommandLabel.Acception == CommandLabel.AcceptionState.InvokeAcception)
          return;
        EventPoint._changeNoneMode = true;
      }));
      if (this._groupID == 2)
        ((Component) this).GetOrAddComponent<CheckEventPointArea>().EventPoint = this;
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnUpdate()));
    }

    protected override void OnEnable()
    {
      base.OnEnable();
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.RemoveConsiderationCommand();
    }

    private void OnDestroy()
    {
      Dictionary<int, EventPoint> dictionary;
      if (!EventPoint._eventPointTable.TryGetValue(this._groupID, out dictionary) || !dictionary.ContainsKey(this._pointID))
        return;
      dictionary.Remove(this._pointID);
    }

    private void OnUpdate()
    {
      bool flag = true;
      if (Singleton<Manager.Map>.IsInstance())
      {
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        if (Object.op_Inequality((Object) player, (Object) null) && player.Mode == Desire.ActionType.Date)
          flag = false;
      }
      int idx1 = 0;
      switch (this._groupID)
      {
        case 0:
          this.Labels_Group_00(ref idx1);
          break;
        case 1:
          this.Labels_Group_01(ref idx1);
          break;
        case 2:
          this.Labels_Group_02(ref idx1);
          break;
      }
      ((ReactiveProperty<int>) this._labelIndex).set_Value(idx1);
      if (flag)
        return;
      int idx2 = 0;
      switch (this._groupID)
      {
        case 0:
          this.Date_Labels_Group_00(ref idx2);
          break;
        case 1:
          this.Date_Labels_Group_01(ref idx2);
          break;
        case 2:
          this.Date_Labels_Group_02(ref idx2);
          break;
      }
      ((ReactiveProperty<int>) this._dateLabelIndex).set_Value(idx2);
    }

    public static int LangIdx
    {
      get
      {
        return Singleton<GameSystem>.IsInstance() ? Singleton<GameSystem>.Instance.languageInt : 0;
      }
    }

    public void SetActive(bool active)
    {
      if (((Component) this).get_gameObject().get_activeSelf() == active)
        return;
      ((Component) this).get_gameObject().SetActive(active);
    }

    public void ChangeActive()
    {
      ((Component) this).get_gameObject().SetActive(!((Component) this).get_gameObject().get_activeSelf());
    }

    protected void PopupWarning(AIProject.Definitions.Popup.Warning.Type warningType)
    {
      MapUIContainer.PushWarningMessage(warningType);
      this.RemoveConsiderationCommand();
    }

    public void SetOpenArea(bool active)
    {
      this.SetOpenArea(this.OpenAreaID, active);
    }

    protected void SetOpenArea(int areaID, bool active)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Singleton<Manager.Map>.Instance.SetOpenAreaState(areaID, active);
    }

    protected bool GetOpenArea(int areaID)
    {
      return Singleton<Manager.Map>.IsInstance() && Singleton<Manager.Map>.Instance.GetOpenAreaState(areaID);
    }

    protected bool GetOpenArea(Manager.Map map, int areaID)
    {
      bool? openAreaState = map?.GetOpenAreaState(areaID);
      return openAreaState.HasValue && openAreaState.Value;
    }

    protected bool GetTimeObjOpen(int groupID)
    {
      return Singleton<Manager.Map>.IsInstance() && Singleton<Manager.Map>.Instance.GetTimeObjOpenState(groupID);
    }

    protected void ChangeRequestEventMode(int eventID, int warningID = -1)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return;
      EventPoint.SetCurrentPlayerStateName();
      this.EventID = eventID;
      this.WarningID = warningID;
      player.CurrentEventPoint = this;
      player.PlayerController.ChangeState("RequestEvent");
    }

    protected bool CheckCharaPhaseProgress(int border)
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<GameSystem>.IsInstance())
        return false;
      ReadOnlyDictionary<int, AgentActor> agentTable = Singleton<Manager.Map>.Instance.AgentTable;
      if (agentTable.IsNullOrEmpty<int, AgentActor>())
        return false;
      string userUuid = Singleton<GameSystem>.Instance.UserUUID;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = agentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor agentActor = enumerator.Current.Value;
          if (agentActor?.AgentData != null && border <= agentActor.ChaControl.fileGameInfo.phase)
            return true;
        }
      }
      return false;
    }

    public static bool SetDedicatedNumber(int groupID, int pointID, int num)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, Dictionary<int, int>> eventPointStateTable = Singleton<Game>.Instance.Environment?.EventPointStateTable;
      if (eventPointStateTable == null)
        return false;
      Dictionary<int, int> dictionary;
      if (!eventPointStateTable.TryGetValue(groupID, out dictionary))
        eventPointStateTable[groupID] = dictionary = new Dictionary<int, int>();
      dictionary[pointID] = num;
      return true;
    }

    public bool SetDedicatedNumber(int num)
    {
      return EventPoint.SetDedicatedNumber(this.GroupID, this.PointID, num);
    }

    public static bool SetDedicatedNumber(EventPoint point, int num)
    {
      return !Object.op_Equality((Object) point, (Object) null) && EventPoint.SetDedicatedNumber(point.GroupID, point.PointID, num);
    }

    public static int GetDedicatedNumber(int groupID, int pointID)
    {
      if (!Singleton<Game>.IsInstance())
        return 0;
      Dictionary<int, Dictionary<int, int>> eventPointStateTable = Singleton<Game>.Instance.Environment?.EventPointStateTable;
      if (eventPointStateTable == null)
        return 0;
      Dictionary<int, int> dictionary;
      if (!eventPointStateTable.TryGetValue(groupID, out dictionary))
        eventPointStateTable[groupID] = dictionary = new Dictionary<int, int>();
      int num;
      if (!dictionary.TryGetValue(pointID, out num))
        dictionary[pointID] = num = 0;
      return num;
    }

    public int GetDedicatedNumber()
    {
      return EventPoint.GetDedicatedNumber(this.GroupID, this.PointID);
    }

    public static int GetDedicatedNumber(EventPoint point)
    {
      return Object.op_Equality((Object) point, (Object) null) ? 0 : EventPoint.GetDedicatedNumber(point.GroupID, point.PointID);
    }

    public static bool SetAgentOpenState(int agentID, bool state)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, AgentData> agentTable = Singleton<Game>.Instance.WorldData?.AgentTable;
      AgentData agentData;
      if (agentTable.IsNullOrEmpty<int, AgentData>() || !agentTable.TryGetValue(agentID, out agentData) || agentData == null)
        return false;
      agentData.OpenState = state;
      return true;
    }

    public bool SetAgentOpenState(bool state)
    {
      int agentID = -1;
      if (this._groupID == 0)
      {
        if (this._pointID == 4)
          agentID = 1;
        else if (this._pointID == 5)
          agentID = 2;
        else if (this._pointID == 6)
          agentID = 3;
      }
      return agentID >= 0 && EventPoint.SetAgentOpenState(agentID, state);
    }

    public static bool GetAgentOpenState(int agentID)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, AgentData> agentTable = Singleton<Game>.Instance.WorldData?.AgentTable;
      AgentData agentData;
      return !agentTable.IsNullOrEmpty<int, AgentData>() && agentTable.TryGetValue(agentID, out agentData) && agentData != null && agentData.OpenState;
    }

    private int GetNeedFlavorAdditionAmount(int requestID)
    {
      if (Singleton<Resources>.IsInstance())
      {
        ReadOnlyDictionary<int, RequestInfo> requestTable = Singleton<Resources>.Instance.PopupInfo.RequestTable;
        Dictionary<int, int> additionBorderTable = Singleton<Resources>.Instance.PopupInfo.RequestFlavorAdditionBorderTable;
        RequestInfo requestInfo;
        if (requestTable.TryGetValue(requestID, ref requestInfo) && requestInfo != null && requestInfo.Type == 2)
        {
          int? nullable = requestInfo.Items.GetElement<Tuple<int, int, int>>(0)?.Item1;
          int num = !nullable.HasValue ? 0 : nullable.Value;
          if (!additionBorderTable.IsNullOrEmpty<int, int>())
          {
            foreach (KeyValuePair<int, int> keyValuePair in additionBorderTable)
            {
              if (keyValuePair.Key < requestID)
                num += keyValuePair.Value;
              else
                break;
            }
          }
          return num;
        }
      }
      return 0;
    }

    public static EventPoint Get(int groupID, int pointID)
    {
      Dictionary<int, EventPoint> dictionary;
      if (!EventPoint._eventPointTable.TryGetValue(groupID, out dictionary))
        return (EventPoint) null;
      EventPoint eventPoint;
      dictionary.TryGetValue(pointID, out eventPoint);
      return eventPoint;
    }

    public static bool IsGameCleared()
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      WorldData worldData = Singleton<Game>.Instance.WorldData;
      return worldData != null && worldData.Cleared;
    }

    public static void OpenEventStart(
      PlayerActor player,
      float startFadeTime,
      float endFadeTime,
      int SEID,
      float delayTime,
      float endIntervalTime,
      System.Action changeAction = null,
      System.Action endAction = null)
    {
      MapUIContainer.SetVisibleHUD(false);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, startFadeTime, true), (System.Action<M0>) (_ => {}), (System.Action) (() => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) delayTime)), (System.Action<M0>) (_ =>
      {
        System.Action action1 = changeAction;
        if (action1 != null)
          action1();
        AudioSource audioSource = (AudioSource) null;
        if (Singleton<Resources>.IsInstance())
          audioSource = Singleton<Resources>.Instance.SoundPack.Play(SEID, Sound.Type.GameSE3D, 0.0f);
        if (Object.op_Inequality((Object) audioSource, (Object) null))
        {
          audioSource.Stop();
          if (Object.op_Inequality((Object) player, (Object) null))
            ((Component) audioSource).get_transform().SetPositionAndRotation(player.Position, player.Rotation);
          audioSource.Play();
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) audioSource), (System.Action<M0>) (__ => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) endIntervalTime)), (System.Action<M0>) (___ => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, endFadeTime, true), (System.Action<M0>) (____ => {}), (System.Action) (() =>
          {
            MapUIContainer.SetVisibleHUD(true);
            System.Action action2 = endAction;
            if (action2 == null)
              return;
            action2();
          }))))));
        }
        else
          ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) endIntervalTime)), (System.Action<M0>) (__ => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, endFadeTime, true), (System.Action<M0>) (___ => {}), (System.Action) (() =>
          {
            MapUIContainer.SetVisibleHUD(true);
            System.Action action2 = endAction;
            if (action2 == null)
              return;
            action2();
          }))));
      }))));
    }

    public void StartMessageDialogDisplay(int id, System.Action submitAction = null)
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Game>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, ValueTuple<int, List<string>>> eventDialogInfoTable = Singleton<Resources>.Instance.Map.EventDialogInfoTable;
      ValueTuple<int, List<string>> valuePair;
      if (eventDialogInfoTable.IsNullOrEmpty<int, ValueTuple<int, List<string>>>() || !eventDialogInfoTable.TryGetValue(id, out valuePair))
        return;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      EventPoint.SetCurrentPlayerStateName();
      player.PlayerController.ChangeState("Idle");
      MapUIContainer.SetVisibleHUD(false);
      if (this._messageCommandData == null)
      {
        this._messageCommandData = new EventPoint.MessageCommandData();
        this._messageCommandData.SendFlag = true;
      }
      if (this._messagePackData == null)
      {
        this._messagePackData = new EventPoint.MessagePackData();
        this._messagePackData.SetCommandData((ADV.ICommandData) this._messageCommandData);
        this._messagePackData.SetParam((IParams) player.PlayerData);
      }
      ADVScene advScene = Singleton<MapUIContainer>.Instance.advScene;
      advScene.Scenario.advUI.Visible(false);
      this._messagePackData.onComplete = (System.Action) (() =>
      {
        EventDialogUI eventDialogUi = MapUIContainer.EventDialogUI;
        eventDialogUi.SubmitEvent = submitAction;
        eventDialogUi.CancelEvent = (System.Action) (() =>
        {
          EventPoint.ChangePrevPlayerMode();
          MapUIContainer.SetVisibleHUD(true);
        });
        eventDialogUi.ClosedEvent = (System.Action) (() => {});
        eventDialogUi.MessageText = ((List<string>) valuePair.Item2).GetElement<string>(EventPoint.LangIdx) ?? string.Empty;
        eventDialogUi.IsActiveControl = true;
      });
      this._openData.FindLoadMessage("event", string.Format("{0}", (object) id));
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDisableAsObservable((Component) advScene), 1), (System.Action<M0>) (_ => advScene.Scenario.advUI.Visible(true)));
      Singleton<MapUIContainer>.Instance.OpenADV(this._openData, (IPack) this._messagePackData);
    }

    public static void ChangeNormalState()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return;
      if (Singleton<MapUIContainer>.IsInstance() && MapUIContainer.CommandLabel.Acception != CommandLabel.AcceptionState.InvokeAcception)
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      if (Singleton<Manager.Input>.IsInstance())
      {
        Manager.Input instance = Singleton<Manager.Input>.Instance;
        instance.FocusLevel = -1;
        if (instance.State != Manager.Input.ValidType.Action)
        {
          instance.ReserveState(Manager.Input.ValidType.Action);
          instance.SetupState();
        }
      }
      if (player.CurrentInteractionState)
        return;
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
    }

    public static void ChangeNormalMode()
    {
      EventPoint.ChangeNormalState();
      (!Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player)?.PlayerController.ChangeState("Normal");
      if (!EventPoint._changeNoneMode)
        return;
      MapUIContainer.CommandLabel.RefreshCommands();
      EventPoint._changeNoneMode = false;
    }

    public static void ChangePlayerMode(string playerStateName = "Normal")
    {
      EventPoint.ChangeNormalState();
      (!Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player)?.PlayerController.ChangeState(playerStateName);
      if (!EventPoint._changeNoneMode)
        return;
      MapUIContainer.CommandLabel.RefreshCommands();
      EventPoint._changeNoneMode = false;
    }

    public static void ChangePrevPlayerMode()
    {
      EventPoint.ChangePlayerMode(EventPoint.PrevPlayerStateName);
    }

    private Dictionary<int, AgentActor> NormalChangeActorTable
    {
      get
      {
        return this._normalChangeActorTable ?? (this._normalChangeActorTable = new Dictionary<int, AgentActor>());
      }
    }

    public static string PrevPlayerStateName { get; set; } = string.Empty;

    public static void SetCurrentPlayerStateName()
    {
      EventPoint.PrevPlayerStateName = string.Empty;
      PlayerActor player = Manager.Map.GetPlayer();
      PlayerController playerController = !Object.op_Inequality((Object) player, (Object) null) ? (PlayerController) null : player.PlayerController;
      IState state = !Object.op_Inequality((Object) playerController, (Object) null) ? (IState) null : playerController.State;
      if (state == null)
        return;
      EventPoint.PrevPlayerStateName = state.GetType().ToString();
      int num = EventPoint.PrevPlayerStateName.LastIndexOf('.');
      if (0 <= num)
        EventPoint.PrevPlayerStateName = EventPoint.PrevPlayerStateName.Substring(num + 1);
      if (!(EventPoint.PrevPlayerStateName == "Houchi"))
        return;
      EventPoint.PrevPlayerStateName = "Normal";
    }

    public void StartMerchantADV(int id)
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Game>.IsInstance())
        return;
      Manager.Map instance1 = Singleton<Manager.Map>.Instance;
      Game instance2 = Singleton<Game>.Instance;
      PlayerActor player = instance1.Player;
      MerchantActor merchant = instance1.Merchant;
      if (instance1?.Simulator?.EnabledTimeProgression.HasValue)
        instance1.Simulator.EnabledTimeProgression = false;
      this._openData.FindLoad(string.Format("{0}", (object) id), -90, 1);
      if (this._eventStoryPackData == null)
      {
        this._eventStoryPackData = new EventPoint.PackData();
        this._eventStoryPackData.SetParam((IParams) merchant.MerchantData, (IParams) player.PlayerData);
        this._eventStoryPackData.SetCommandData((ADV.ICommandData) Singleton<Game>.Instance.Environment);
      }
      this._eventStoryPackData.onComplete = (System.Action) (() => this.EndMerchantADV(id));
      this._eventStoryPackData.Init();
      Transform transform = this._commandBasePoint ?? ((Component) this).get_transform();
      this._playerPoint = ((Component) this).get_transform().FindLoop(EventPoint.PlayerPointName)?.get_transform() ?? transform;
      this._merchantPoint = ((Component) this).get_transform().FindLoop(EventPoint.MerchantPointName)?.get_transform() ?? transform;
      this._recoverPoint = ((Component) this).get_transform().FindLoop(EventPoint.RecoverPointName)?.get_transform();
      EventPoint.SetCurrentPlayerStateName();
      player.PlayerController.ChangeState("Idle");
      MapUIContainer.SetVisibleHUD(false);
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>(Observable.TakeUntilDestroy<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (Component) this), (Component) player), (Component) merchant), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
          return;
        MapUIContainer.SetVisibleHUD(false);
        this.NormalChangeActorTable.Clear();
        Actor partner = merchant.Partner;
        if (Object.op_Inequality((Object) partner, (Object) null))
        {
          if (partner is AgentActor)
          {
            AgentActor agentActor = partner as AgentActor;
            agentActor.ChangeBehavior(Desire.ActionType.Idle);
            agentActor.DisableBehavior();
            if (!this.NormalChangeActorTable.ContainsKey(agentActor.InstanceID))
              this.NormalChangeActorTable[agentActor.InstanceID] = agentActor;
            agentActor.Partner = (Actor) null;
          }
          merchant.Partner = (Actor) null;
        }
        Actor commandPartner = merchant.CommandPartner;
        if (Object.op_Inequality((Object) commandPartner, (Object) null))
        {
          if (commandPartner is AgentActor)
          {
            AgentActor agentActor = commandPartner as AgentActor;
            agentActor.ChangeBehavior(Desire.ActionType.Idle);
            agentActor.DisableBehavior();
            if (!this.NormalChangeActorTable.ContainsKey(agentActor.InstanceID))
              this.NormalChangeActorTable[agentActor.InstanceID] = agentActor;
            agentActor.CommandPartner = (Actor) null;
          }
          merchant.CommandPartner = (Actor) null;
        }
        if (!this.NormalChangeActorTable.IsNullOrEmpty<int, AgentActor>())
        {
          foreach (KeyValuePair<int, AgentActor> keyValuePair in this.NormalChangeActorTable)
          {
            AgentActor agentActor = keyValuePair.Value;
            if (!Object.op_Equality((Object) agentActor, (Object) null))
            {
              agentActor.SetStand(agentActor.Animation.RecoveryPoint, false, 0.0f, 0);
              agentActor.Animation.RecoveryPoint = (Transform) null;
              agentActor.Animation.EndIgnoreEvent();
              agentActor.Animation.ResetDefaultAnimatorController();
              NavMeshAgent navMeshAgent = agentActor.NavMeshAgent;
              if (((Behaviour) navMeshAgent).get_isActiveAndEnabled() && navMeshAgent.get_hasPath())
                navMeshAgent.ResetPath();
            }
          }
        }
        Debug.Log((object) "EventStart: Partnerの処理を終えた");
        if (Singleton<Manager.Voice>.IsInstance())
          Singleton<Manager.Voice>.Instance.Stop(-90);
        merchant.SetStand(merchant.Animation.RecoveryPoint, false, 0.0f, 0);
        merchant.Animation.RecoveryPoint = (Transform) null;
        merchant.Animation.EndIgnoreEvent();
        merchant.Animation.ResetDefaultAnimatorController();
        merchant.ChangeBehavior(Merchant.ActionType.TalkWithPlayer);
        this._merchantBackUp.Set((Actor) merchant);
        if (!merchant.ChaControl.visibleAll)
          merchant.ChaControl.visibleAll = true;
        StoryPointEffect storyPointEffect = Singleton<Manager.Map>.Instance.StoryPointEffect;
        if (Object.op_Inequality((Object) storyPointEffect, (Object) null))
          storyPointEffect.Hide();
        merchant.StopStand();
        merchant.Animation.StopAllAnimCoroutine();
        merchant.Animation.Targets.Clear();
        merchant.Animation.Animator.InterruptMatchTarget(false);
        merchant.Animation.Animator.Play(Singleton<Resources>.Instance.DefinePack.AnimatorState.MerchantIdleState);
        if (((Behaviour) merchant.NavMeshAgent).get_isActiveAndEnabled() && (merchant.NavMeshAgent.get_isOnNavMesh() || merchant.NavMeshAgent.get_isOnOffMeshLink()))
          merchant.NavMeshAgent.ResetPath();
        this.HideAgents();
        this.SetPosAndRot((Actor) player, this._playerPoint);
        this.SetPosAndRot((Actor) merchant, this._merchantPoint);
        merchant.ActivateNavMeshObstacle(this._merchantPoint.get_position());
        player.CommCompanion = (Actor) merchant;
        player.PlayerController.ChangeState("Communication");
        ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.TimerFrame(2, (FrameCountType) 0), (Component) this), (Component) player), (Component) merchant), (System.Action<M0>) (_ =>
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          this._prevCameraStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
          Manager.ADV.ChangeADVCamera((Actor) merchant);
          merchant.SetLookPtn(1, 3);
          merchant.SetLookTarget(1, 0, ((Component) player.CameraControl.CameraComponent).get_transform());
          ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) player), 1), (Func<M0, bool>) (__ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), (System.Action<M0>) (__ => Singleton<MapUIContainer>.Instance.OpenADV(this._openData, (IPack) this._eventStoryPackData)));
        }));
      }));
    }

    private void SetPosAndRot(Actor actor, Transform point)
    {
      if (((Behaviour) actor.NavMeshAgent).get_enabled())
        actor.NavMeshAgent.Warp(point.get_position());
      else
        actor.Position = point.get_position();
      actor.Rotation = point.get_rotation();
    }

    private void HideAgents()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      ReadOnlyDictionary<int, AgentActor> agentTable = Singleton<Manager.Map>.Instance.AgentTable;
      if (agentTable.IsNullOrEmpty<int, AgentActor>())
        return;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = agentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor agentActor = enumerator.Current.Value;
          if (!Object.op_Equality((Object) agentActor, (Object) null))
            agentActor.DisableEntity();
        }
      }
    }

    private void ShowAgents()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      ReadOnlyDictionary<int, AgentActor> agentTable = Singleton<Manager.Map>.Instance.AgentTable;
      if (agentTable.IsNullOrEmpty<int, AgentActor>())
        return;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = agentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor agentActor = enumerator.Current.Value;
          if (!Object.op_Equality((Object) agentActor, (Object) null))
            agentActor.EnableEntity();
        }
      }
    }

    private void EndMerchantADV(int id)
    {
      this._eventStoryPackData.Release();
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Manager.Map map = Singleton<Manager.Map>.Instance;
      PlayerActor player = map.Player;
      MerchantActor merchant = map.Merchant;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return;
        this.ShowAgents();
        if (!this.NormalChangeActorTable.IsNullOrEmpty<int, AgentActor>())
        {
          foreach (KeyValuePair<int, AgentActor> keyValuePair in this.NormalChangeActorTable)
          {
            AgentActor agentActor = keyValuePair.Value;
            if (!Object.op_Equality((Object) agentActor, (Object) null))
              agentActor.ChangeBehavior(Desire.ActionType.Normal);
          }
          this.NormalChangeActorTable.Clear();
        }
        if (this._merchantBackUp.Visibled != merchant.ChaControl.visibleAll)
          merchant.ChaControl.visibleAll = this._merchantBackUp.Visibled;
        merchant.Position = this._merchantBackUp.Position;
        merchant.Rotation = this._merchantBackUp.Rotation;
        merchant.SetLookPtn(0, 3);
        merchant.SetLookTarget(0, 0, (Transform) null);
        merchant.ChangeBehavior(merchant.LastNormalMode);
        StoryPointEffect storyPointEffect = Singleton<Manager.Map>.Instance.StoryPointEffect;
        if (Object.op_Inequality((Object) storyPointEffect, (Object) null))
          storyPointEffect.Show();
        player.PlayerController.ChangeState("Idle");
        if (Object.op_Inequality((Object) this._recoverPoint, (Object) null))
          this.SetPosAndRot((Actor) player, this._recoverPoint);
        player.CameraControl.Mode = CameraMode.Normal;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) this._prevCameraStyle;
        Transform transform = this._recoverPoint ?? ((Component) player.Locomotor).get_transform();
        if (player.CameraControl.ShotType == ShotType.PointOfView)
        {
          ActorCameraControl cameraControl = player.CameraControl;
          Quaternion rotation = transform.get_rotation();
          // ISSUE: variable of the null type
          __Null y = ((Quaternion) ref rotation).get_eulerAngles().y;
          cameraControl.XAxisValue = (float) y;
          player.CameraControl.YAxisValue = 0.5f;
        }
        else
        {
          ActorCameraControl cameraControl = player.CameraControl;
          Quaternion rotation = transform.get_rotation();
          double num = ((Quaternion) ref rotation).get_eulerAngles().y - 5.0;
          cameraControl.XAxisValue = (float) num;
          player.CameraControl.YAxisValue = 0.6f;
        }
        if (EventPoint._commandRefreshEvent != null)
          EventPoint._commandRefreshEvent.OnNext(Unit.get_Default());
        switch (id)
        {
          case 1:
            this.SetOpenArea(this.OpenAreaID, true);
            Manager.Map.ForcedSetTutorialProgress(18);
            break;
          case 2:
            if (Singleton<Resources>.IsInstance() && Singleton<Game>.IsInstance())
            {
              List<StuffItem> self = !Object.op_Inequality((Object) player, (Object) null) ? (List<StuffItem>) null : player.PlayerData?.ItemList;
              CommonDefine commonDefine = !Singleton<Resources>.IsInstance() ? (CommonDefine) null : Singleton<Resources>.Instance.CommonDefine;
              ItemIDKeyPair? nullable = !Object.op_Inequality((Object) commonDefine, (Object) null) ? new ItemIDKeyPair?() : commonDefine.ItemIDDefine?.ShansKeyID;
              if (self != null && nullable.HasValue)
              {
                ItemIDKeyPair keyID = nullable.Value;
                if (!self.Exists((Predicate<StuffItem>) (x => x.CategoryID == keyID.categoryID && x.ID == keyID.itemID && 0 < x.Count)))
                  self.AddItem(new StuffItem(keyID.categoryID, keyID.itemID, 1));
              }
            }
            this.SetOpenArea(this.OpenAreaID, true);
            Manager.Map.ForcedSetTutorialProgress(21);
            break;
          case 3:
            this.SetDedicatedNumber(2);
            Manager.Map.ForcedSetTutorialProgress(24);
            break;
          case 4:
            this.SetDedicatedNumber(1);
            break;
        }
        ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(1.0)), (System.Action<M0>) (_ =>
        {
          if (!Singleton<MapUIContainer>.IsInstance())
            return;
          MapUIContainer.SetVisibleHUD(true);
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 2f, true), (System.Action<M0>) (__ =>
          {
            if (Object.op_Inequality((Object) map, (Object) null) && Object.op_Inequality((Object) map.Simulator, (Object) null))
              map.Simulator.EnabledTimeProgression = true;
            EventPoint.ChangePrevPlayerMode();
            this.AddLog(id);
          }));
        }));
      }));
    }

    private void AddLog(int advID)
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, List<string>> commandLabelTextTable = Singleton<Resources>.Instance.Map.EventPointCommandLabelTextTable;
      switch (advID)
      {
        case 1:
          List<string> source1;
          if (!commandLabelTextTable.TryGetValue(12, out source1) || !source1.InRange<string>(EventPoint.LangIdx))
            break;
          MapUIContainer.AddSystemLog(source1.GetElement<string>(EventPoint.LangIdx) ?? string.Empty, true);
          break;
        case 2:
          List<string> source2;
          if (!commandLabelTextTable.TryGetValue(13, out source2) || !source2.InRange<string>(EventPoint.LangIdx))
            break;
          MapUIContainer.AddSystemLog(source2.GetElement<string>(EventPoint.LangIdx) ?? string.Empty, true);
          break;
      }
    }

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return this._labelsList.GetElement<CommandLabel.CommandInfo[]>(((ReactiveProperty<int>) this._labelIndex).get_Value());
      }
    }

    public CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        return this._dateLabelsList.GetElement<CommandLabel.CommandInfo[]>(((ReactiveProperty<int>) this._dateLabelIndex).get_Value());
      }
    }

    protected void Labels_Group_00(ref int idx)
    {
    }

    protected void Labels_Group_01(ref int idx)
    {
      switch (this._pointID)
      {
        case 0:
          switch (this.GetDedicatedNumber())
          {
            case 0:
              idx = Manager.Map.GetTotalAgentFlavorAdditionAmount() >= this._flavorBorder ? 1 : 0;
              if (idx != 1)
                return;
              this.SetDedicatedNumber(1);
              return;
            case 1:
              idx = 1;
              return;
            default:
              return;
          }
        case 1:
          if (Manager.Map.GetTutorialProgress() == 18)
          {
            idx = 0;
            break;
          }
          switch (this.GetDedicatedNumber())
          {
            case 0:
              idx = Manager.Map.GetTotalAgentFlavorAdditionAmount() >= this._flavorBorder ? 1 : 0;
              if (idx != 1)
                return;
              this.SetDedicatedNumber(1);
              return;
            case 1:
              idx = 1;
              return;
            default:
              return;
          }
        case 2:
          if (Manager.Map.GetTutorialProgress() == 21)
          {
            idx = 0;
            break;
          }
          switch (this.GetDedicatedNumber())
          {
            case 0:
              idx = Manager.Map.GetTotalAgentFlavorAdditionAmount() >= this._flavorBorder ? 1 : 0;
              if (idx != 1)
                return;
              this.SetDedicatedNumber(1);
              return;
            case 1:
              idx = 1;
              return;
            case 2:
              idx = this.GetTimeObjOpen(0) ? 3 : 2;
              if (idx != 3)
                return;
              this.SetDedicatedNumber(3);
              return;
            case 3:
              idx = 3;
              return;
            default:
              return;
          }
        case 3:
          EventPoint eventPoint = EventPoint.Get(1, 2);
          if (Object.op_Inequality((Object) eventPoint, (Object) null))
          {
            idx = eventPoint.LabelIndex == 2 ? 1 : 0;
            break;
          }
          idx = 0;
          break;
        case 4:
          idx = this.GetTimeObjOpen(0) ? 1 : 0;
          break;
        case 5:
          idx = 0;
          if (!Singleton<Manager.Map>.IsInstance())
            break;
          PlayerActor player = Singleton<Manager.Map>.Instance.Player;
          Actor actor = !Object.op_Inequality((Object) player, (Object) null) ? (Actor) null : player.Partner;
          if (!Object.op_Inequality((Object) player, (Object) null) || player.Mode != Desire.ActionType.Date && !(player.PlayerController.State is Onbu) || (!Object.op_Inequality((Object) actor, (Object) null) || !((Behaviour) player.NavMeshAgent).get_isActiveAndEnabled()))
            break;
          if (EventPoint._getNavPath == null)
            EventPoint._getNavPath = new NavMeshPath();
          if (!player.NavMeshAgent.CalculatePath(actor.Position, EventPoint._getNavPath) || EventPoint._getNavPath.get_status() != null)
            break;
          float num = 0.0f;
          Vector3[] corners = EventPoint._getNavPath.get_corners();
          for (int index = 0; index < corners.Length - 1; ++index)
            num += Vector3.Distance(corners[index], corners[index + 1]);
          idx = 30.0 >= (double) num ? 1 : 0;
          break;
      }
    }

    protected void Labels_Group_02(ref int idx)
    {
      switch (this._pointID)
      {
      }
    }

    protected void Date_Labels_Group_00(ref int idx)
    {
    }

    protected void Date_Labels_Group_01(ref int idx)
    {
      switch (this._pointID)
      {
        case 2:
          idx = ((ReactiveProperty<int>) this._labelIndex).get_Value();
          break;
        case 4:
          idx = ((ReactiveProperty<int>) this._labelIndex).get_Value();
          break;
        case 5:
          idx = 0;
          if (!Singleton<Manager.Map>.IsInstance())
            break;
          PlayerActor player = Singleton<Manager.Map>.Instance.Player;
          Actor actor = !Object.op_Inequality((Object) player, (Object) null) ? (Actor) null : player.Partner;
          if (!Object.op_Inequality((Object) player, (Object) null) || !Object.op_Inequality((Object) actor, (Object) null) || !((Behaviour) player.NavMeshAgent).get_isActiveAndEnabled())
            break;
          if (EventPoint._getNavPath == null)
            EventPoint._getNavPath = new NavMeshPath();
          if (!player.NavMeshAgent.CalculatePath(actor.Position, EventPoint._getNavPath) || EventPoint._getNavPath.get_status() != null)
            break;
          float num = 0.0f;
          Vector3[] corners = EventPoint._getNavPath.get_corners();
          for (int index = 0; index < corners.Length - 1; ++index)
            num += Vector3.Distance(corners[index], corners[index + 1]);
          idx = 30.0 >= (double) num ? 1 : 0;
          break;
      }
    }

    protected void Date_Labels_Group_02(ref int idx)
    {
    }

    protected bool IsNeutral
    {
      get
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return false;
        switch (this._groupID)
        {
          case 0:
            return this.IsNeutral_Group_00();
          case 1:
            return this.IsNeutral_Group_01();
          case 2:
            return this.IsNeutral_Group_02();
          default:
            return false;
        }
      }
    }

    protected bool IsNeutral_Group_00()
    {
      switch (this._pointID)
      {
        case 0:
          return !this.GetOpenArea(0);
        case 1:
          return !this.GetOpenArea(0);
        case 2:
          return this.GetOpenArea(1) && !this.GetOpenArea(3);
        case 3:
          return this.GetOpenArea(2) && !this.GetOpenArea(3);
        case 4:
          return this.GetOpenArea(1) && !EventPoint.GetAgentOpenState(1);
        case 5:
          return this.GetOpenArea(2) && !EventPoint.GetAgentOpenState(2);
        case 6:
          return this.GetOpenArea(6) && !EventPoint.GetAgentOpenState(3);
        default:
          return false;
      }
    }

    protected bool IsNeutral_Group_01()
    {
      switch (this._pointID)
      {
        case 0:
          return !this.GetOpenArea(1);
        case 1:
          return this.GetOpenArea(1) && !this.GetOpenArea(2);
        case 2:
          if (!this.GetOpenArea(2) || this.GetOpenArea(4))
            return false;
          if (this.GetDedicatedNumber() != 3)
            return true;
          PlayerActor player1 = Manager.Map.GetPlayer();
          return !((!Object.op_Inequality((Object) player1, (Object) null) ? (IState) null : player1.PlayerController.State) is Onbu);
        case 3:
          return this.GetOpenArea(2) && !this.GetTimeObjOpen(0);
        case 4:
          if (!this.GetOpenArea(2) || this.GetOpenArea(5))
            return false;
          if (!this.GetTimeObjOpen(0))
            return true;
          PlayerActor player2 = Manager.Map.GetPlayer();
          return !((!Object.op_Inequality((Object) player2, (Object) null) ? (IState) null : player2.PlayerController.State) is Onbu);
        case 5:
          return this.GetOpenArea(5) && !this.GetOpenArea(6);
        case 6:
          return this.GetOpenArea(4) && !EventPoint.IsGameCleared();
        default:
          return false;
      }
    }

    protected bool IsNeutral_Group_02()
    {
      switch (this._pointID)
      {
        case 0:
          return this.GetDedicatedNumber() == 0;
        case 1:
          return this.GetOpenArea(4) && this.GetDedicatedNumber() == 0;
        default:
          return false;
      }
    }

    private void InitializeCommandLabels()
    {
      switch (this._groupID)
      {
        case 0:
          this.InitializeCommandLabels_Group_00();
          break;
        case 1:
          this.InitializeCommandLabels_Group_01();
          break;
        case 2:
          this.InitializeCommandLabels_Group_02();
          break;
      }
      switch (this._groupID)
      {
        case 0:
          this.InitializeCommandDate_Labels_Group_00();
          break;
        case 1:
          this.InitializeCommandDate_Labels_Group_01();
          break;
        case 2:
          this.InitializeCommandDate_Labels_Group_02();
          break;
      }
    }

    private void InitializeCommandLabels_Group_00()
    {
      CommonDefine commonDefine = Singleton<Resources>.Instance.CommonDefine;
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      Sprite sprite = (Sprite) null;
      Resources instance = Singleton<Resources>.Instance;
      Manager.Map map = Singleton<Manager.Map>.Instance;
      int eventIconId = icon.EventIconID;
      instance.itemIconTables.ActionIconTable.TryGetValue(eventIconId, out sprite);
      Dictionary<int, List<string>> commandLabelTextTable = instance.Map.EventPointCommandLabelTextTable;
      CommonDefine.EventStoryInfoGroup playInfo = Singleton<Resources>.Instance.CommonDefine.EventStoryInfo;
      switch (this._pointID)
      {
        case 0:
          this.EventType = EventPoint.EventTypes.AreaOpen;
          this.OpenAreaID = 0;
          List<string> source1;
          commandLabelTextTable.TryGetValue(0, out source1);
          this._labelsList.Add(new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source1.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.NotOpenThisSide))
            }
          });
          break;
        case 1:
          this.EventType = EventPoint.EventTypes.AreaOpen;
          this.OpenAreaID = 0;
          List<string> source2;
          commandLabelTextTable.TryGetValue(0, out source2);
          commandLabelTextTable.TryGetValue(4, out List<string> _);
          this._labelsList.Add(new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source2.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite,
              IsHold = true,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.StartMessageDialogDisplay(0, (System.Action) (() =>
              {
                if (playInfo == null)
                  return;
                EventPoint.OpenEventStart(map.Player, playInfo.StartEventFadeTime, playInfo.EndEventFadeTime, playInfo.JunkRoad.SEID, playInfo.JunkRoad.SoundPlayDelayTime, playInfo.JunkRoad.EndIntervalTime, (System.Action) (() => this.SetOpenArea(true)), (System.Action) (() => EventPoint.ChangePrevPlayerMode()));
              })))
            }
          });
          break;
        case 2:
          this.EventType = EventPoint.EventTypes.AreaOpen;
          this.OpenAreaID = 3;
          List<string> source3;
          commandLabelTextTable.TryGetValue(0, out source3);
          this._labelsList.Add(new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source3.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.NotOpenThisSide))
            }
          });
          break;
        case 3:
          this.EventType = EventPoint.EventTypes.AreaOpen;
          this.OpenAreaID = 3;
          List<string> source4;
          commandLabelTextTable.TryGetValue(0, out source4);
          commandLabelTextTable.TryGetValue(5, out List<string> _);
          this._labelsList.Add(new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source4.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.StartMessageDialogDisplay(1, (System.Action) (() =>
              {
                if (playInfo == null)
                  return;
                EventPoint.OpenEventStart(map.Player, playInfo.StartEventFadeTime, playInfo.EndEventFadeTime, playInfo.FenceDoor.SEID, playInfo.FenceDoor.SoundPlayDelayTime, playInfo.FenceDoor.EndIntervalTime, (System.Action) (() => this.SetOpenArea(true)), (System.Action) (() => EventPoint.ChangePrevPlayerMode()));
              })))
            }
          });
          break;
        case 4:
        case 5:
        case 6:
          this.EventType = EventPoint.EventTypes.Request;
          List<string> source5;
          commandLabelTextTable.TryGetValue(0, out source5);
          this._labelsList.Add(new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source5.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.ChangeRequestEventMode(2, 14))
            }
          });
          break;
      }
    }

    private void InitializeCommandLabels_Group_01()
    {
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      Sprite sprite1 = (Sprite) null;
      Sprite sprite2 = (Sprite) null;
      Resources instance = Singleton<Resources>.Instance;
      Manager.Map map = Singleton<Manager.Map>.Instance;
      instance.itemIconTables.ActionIconTable.TryGetValue(icon.EventIconID, out sprite2);
      instance.itemIconTables.ActionIconTable.TryGetValue(icon.StoryIconID, out sprite1);
      Dictionary<int, List<string>> commandLabelTextTable = instance.Map.EventPointCommandLabelTextTable;
      switch (this._pointID)
      {
        case 0:
          this.OpenAreaID = 1;
          List<string> source1;
          commandLabelTextTable.TryGetValue(0, out source1);
          int requestID1 = 4;
          this._flavorBorder = this.GetNeedFlavorAdditionAmount(requestID1);
          CommandLabel.CommandInfo[] commandInfoArray1 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source1.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.ChangeRequestEventMode(requestID1, -1))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray2 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source1.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.StartMerchantADV(1))
            }
          };
          this._labelsList.Add(commandInfoArray1);
          this._labelsList.Add(commandInfoArray2);
          break;
        case 1:
          List<string> source2;
          commandLabelTextTable.TryGetValue(0, out source2);
          this.OpenAreaID = 2;
          int requestID2 = 5;
          this._flavorBorder = this.GetNeedFlavorAdditionAmount(requestID2);
          CommandLabel.CommandInfo[] commandInfoArray3 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source2.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() =>
              {
                this.ChangeRequestEventMode(requestID2, -1);
                if (Manager.Map.GetTutorialProgress() != 18)
                  return;
                if (Manager.Map.GetTotalAgentFlavorAdditionAmount() < this._flavorBorder)
                  Manager.Map.ForcedSetTutorialProgress(19);
                else
                  Manager.Map.ForcedSetTutorialProgress(20);
              })
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray4 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source2.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.StartMerchantADV(2))
            }
          };
          this._labelsList.Add(commandInfoArray3);
          this._labelsList.Add(commandInfoArray4);
          break;
        case 2:
          List<string> source3;
          commandLabelTextTable.TryGetValue(0, out source3);
          this.OpenAreaID = 4;
          int requestID3 = 6;
          this._flavorBorder = this.GetNeedFlavorAdditionAmount(requestID3);
          CommandLabel.CommandInfo[] commandInfoArray5 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source3.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() =>
              {
                this.ChangeRequestEventMode(requestID3, -1);
                if (Manager.Map.GetTutorialProgress() != 21)
                  return;
                if (Manager.Map.GetTotalAgentFlavorAdditionAmount() < this._flavorBorder)
                  Manager.Map.ForcedSetTutorialProgress(22);
                else
                  Manager.Map.ForcedSetTutorialProgress(23);
              })
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray6 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source3.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.StartMerchantADV(3))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray7 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source3.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.CanOpenWithElec))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray8 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source3.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() =>
              {
                this.RemoveConsiderationCommand();
                map.Player.CurrentEventPoint = this;
                map.Player.PlayerController.ChangeState("OpenHarbordoor");
              })
            }
          };
          this._labelsList.Add(commandInfoArray5);
          this._labelsList.Add(commandInfoArray6);
          this._labelsList.Add(commandInfoArray7);
          this._labelsList.Add(commandInfoArray8);
          break;
        case 3:
          this.OpenAreaID = 4;
          List<string> source4;
          commandLabelTextTable.TryGetValue(0, out source4);
          CommandLabel.CommandInfo[] commandInfoArray9 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source4.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.IsBroken))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray10 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source4.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.ChangeRequestEventMode(0, -1))
            }
          };
          this._labelsList.Add(commandInfoArray9);
          this._labelsList.Add(commandInfoArray10);
          break;
        case 4:
          this.OpenAreaID = 5;
          List<string> source5;
          commandLabelTextTable.TryGetValue(0, out source5);
          CommandLabel.CommandInfo[] commandInfoArray11 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source5.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite2,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.CanOpenWithElec))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray12 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source5.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite2,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() =>
              {
                this.RemoveConsiderationCommand();
                map.Player.CurrentEventPoint = this;
                map.Player.PlayerController.ChangeState("OpenHarbordoor");
              })
            }
          };
          this._labelsList.Add(commandInfoArray11);
          this._labelsList.Add(commandInfoArray12);
          break;
        case 5:
          this.OpenAreaID = 6;
          List<string> source6;
          commandLabelTextTable.TryGetValue(0, out source6);
          CommonDefine.EventStoryInfoGroup playInfo = instance.CommonDefine.EventStoryInfo;
          CommandLabel.CommandInfo[] commandInfoArray13 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source6.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite2,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.DontReactAlone))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray14 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source6.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite2,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.StartMessageDialogDisplay(2, (System.Action) (() =>
              {
                if (playInfo == null)
                  return;
                EventPoint.OpenEventStart(map.Player, playInfo.StartEventFadeTime, playInfo.EndEventFadeTime, playInfo.AutomaticDoor.SEID, playInfo.AutomaticDoor.SoundPlayDelayTime, playInfo.AutomaticDoor.EndIntervalTime, (System.Action) (() => this.SetOpenArea(true)), (System.Action) (() => EventPoint.ChangePrevPlayerMode()));
              })))
            }
          };
          this._labelsList.Add(commandInfoArray13);
          this._labelsList.Add(commandInfoArray14);
          break;
        case 6:
          List<string> source7;
          commandLabelTextTable.TryGetValue(0, out source7);
          this._labelsList.Add(new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source7.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() =>
              {
                this.ChangeRequestEventMode(1, -1);
                if (Manager.Map.GetTutorialProgress() != 26)
                  return;
                Manager.Map.ForcedSetTutorialProgress(27);
              })
            }
          });
          break;
      }
    }

    private void InitializeCommandLabels_Group_02()
    {
    }

    private void InitializeCommandDate_Labels_Group_00()
    {
    }

    private void InitializeCommandDate_Labels_Group_01()
    {
      Sprite sprite1 = (Sprite) null;
      Sprite sprite2 = (Sprite) null;
      Resources instance = Singleton<Resources>.Instance;
      Manager.Map map = Singleton<Manager.Map>.Instance;
      CommonDefine.CommonIconGroup icon = instance.CommonDefine.Icon;
      instance.itemIconTables.ActionIconTable.TryGetValue(icon.EventIconID, out sprite1);
      instance.itemIconTables.ActionIconTable.TryGetValue(icon.StoryIconID, out sprite2);
      Dictionary<int, List<string>> commandLabelTextTable = instance.Map.EventPointCommandLabelTextTable;
      CommonDefine.EventStoryInfoGroup playInfo = instance.CommonDefine.EventStoryInfo;
      switch (this._pointID)
      {
        case 2:
          List<string> source1;
          commandLabelTextTable.TryGetValue(0, out source1);
          CommandLabel.CommandInfo[] commandInfoArray1 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source1.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite2,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.CanOpenWithElec))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray2 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source1.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite2,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() =>
              {
                this.RemoveConsiderationCommand();
                map.Player.CurrentEventPoint = this;
                map.Player.PlayerController.ChangeState("DateOpenHarbordoor");
              })
            }
          };
          this._dateLabelsList.Add((CommandLabel.CommandInfo[]) null);
          this._dateLabelsList.Add((CommandLabel.CommandInfo[]) null);
          this._dateLabelsList.Add(commandInfoArray1);
          this._dateLabelsList.Add(commandInfoArray2);
          break;
        case 4:
          List<string> source2;
          commandLabelTextTable.TryGetValue(0, out source2);
          CommandLabel.CommandInfo[] commandInfoArray3 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source2.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.CanOpenWithElec))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray4 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source2.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() =>
              {
                this.RemoveConsiderationCommand();
                map.Player.CurrentEventPoint = this;
                map.Player.PlayerController.ChangeState("DateOpenHarbordoor");
              })
            }
          };
          this._dateLabelsList.Add(commandInfoArray3);
          this._dateLabelsList.Add(commandInfoArray4);
          break;
        case 5:
          List<string> source3;
          commandLabelTextTable.TryGetValue(0, out source3);
          CommandLabel.CommandInfo[] commandInfoArray5 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source3.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.PopupWarning(AIProject.Definitions.Popup.Warning.Type.DontReactAlone))
            }
          };
          CommandLabel.CommandInfo[] commandInfoArray6 = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = source3.GetElement<string>(EventPoint.LangIdx) ?? string.Empty,
              Icon = sprite1,
              IsHold = this._isHold,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => this.StartMessageDialogDisplay(2, (System.Action) (() =>
              {
                if (playInfo == null)
                  return;
                EventPoint.OpenEventStart(map.Player, playInfo.StartEventFadeTime, playInfo.EndEventFadeTime, playInfo.AutomaticDoor.SEID, playInfo.AutomaticDoor.SoundPlayDelayTime, playInfo.AutomaticDoor.EndIntervalTime, (System.Action) (() => this.SetOpenArea(true)), (System.Action) (() => EventPoint.ChangePrevPlayerMode()));
              })))
            }
          };
          this._dateLabelsList.Add(commandInfoArray5);
          this._dateLabelsList.Add(commandInfoArray6);
          break;
      }
    }

    private void InitializeCommandDate_Labels_Group_02()
    {
    }

    private class PackData : CharaPackData
    {
    }

    private class MessagePackData : CharaPackData
    {
    }

    private class MessageCommandData : ADV.ICommandData
    {
      public bool SendFlag { get; set; }

      public System.Action<bool> SendAction { get; set; }

      public IEnumerable<CommandData> CreateCommandData(string head)
      {
        return (IEnumerable<CommandData>) new List<CommandData>()
        {
          new CommandData(CommandData.Command.BOOL, head + "SendFlag", (Func<object>) (() => (object) this.SendFlag), (System.Action<object>) (o =>
          {
            System.Action<bool> sendAction = this.SendAction;
            if (sendAction == null)
              return;
            sendAction((bool) o);
          }))
        };
      }
    }

    public enum EventTypes
    {
      Request,
      Warning,
      AreaOpen,
      Other,
    }

    public class BackUpInfo
    {
      public BackUpInfo()
      {
        this.Set();
      }

      public BackUpInfo(Actor actor)
      {
        this.Set(actor);
      }

      public Actor Actor { get; set; }

      public bool Visibled { get; set; } = true;

      public Vector3 Position { get; set; } = Vector3.get_zero();

      public Quaternion Rotation { get; set; } = Quaternion.get_identity();

      public bool ControllerEnabled { get; set; } = true;

      public bool AnimationEnabled { get; set; } = true;

      public ValueTuple<Desire.ActionType, Desire.ActionType> ModeCache { get; protected set; } = new ValueTuple<Desire.ActionType, Desire.ActionType>(Desire.ActionType.Idle, Desire.ActionType.Idle);

      public bool BehaviorEnabled { get; set; } = true;

      public void Set()
      {
        if (Object.op_Equality((Object) this.Actor, (Object) null))
        {
          this.Visibled = true;
          this.Position = Vector3.get_zero();
          this.Rotation = Quaternion.get_identity();
          this.ControllerEnabled = true;
          this.AnimationEnabled = true;
          this.BehaviorEnabled = true;
          this.ModeCache = new ValueTuple<Desire.ActionType, Desire.ActionType>(Desire.ActionType.Idle, Desire.ActionType.Idle);
        }
        else
        {
          this.Visibled = this.Actor.ChaControl.visibleAll;
          this.Position = this.Actor.Position;
          this.Rotation = this.Actor.Rotation;
          this.ControllerEnabled = ((Behaviour) this.Actor.Controller).get_enabled();
          this.AnimationEnabled = ((Behaviour) this.Actor.Animation).get_enabled();
          if (this.IsAgent)
          {
            AgentActor agent = this.Agent;
            this.ModeCache = new ValueTuple<Desire.ActionType, Desire.ActionType>(agent.Mode, agent.BehaviorResources.Mode);
            this.BehaviorEnabled = ((Behaviour) agent.BehaviorResources).get_enabled();
          }
          else
          {
            this.ModeCache = new ValueTuple<Desire.ActionType, Desire.ActionType>(Desire.ActionType.Idle, Desire.ActionType.Idle);
            this.BehaviorEnabled = true;
          }
        }
      }

      public void Set(Actor actor)
      {
        this.Actor = actor;
        if (Object.op_Equality((Object) actor, (Object) null))
        {
          this.Visibled = false;
          this.Position = Vector3.get_zero();
          this.Rotation = Quaternion.get_identity();
          this.ControllerEnabled = false;
          this.AnimationEnabled = false;
          this.BehaviorEnabled = false;
          this.ModeCache = new ValueTuple<Desire.ActionType, Desire.ActionType>(Desire.ActionType.Idle, Desire.ActionType.Idle);
        }
        else
        {
          this.Visibled = !Object.op_Inequality((Object) this.Actor.ChaControl, (Object) null) || this.Actor.ChaControl.visibleAll;
          if (this.Actor is PlayerActor)
          {
            PlayerActor actor1 = this.Actor as PlayerActor;
            PlayerData playerData = actor1.PlayerData;
            if (playerData != null)
            {
              this.Position = playerData.Position;
              this.Rotation = playerData.Rotation;
            }
            else
            {
              this.Position = actor1.Position;
              this.Rotation = actor1.Rotation;
            }
          }
          else if (this.Actor is AgentActor)
          {
            AgentActor actor1 = this.Actor as AgentActor;
            AgentData agentData = actor1.AgentData;
            if (agentData != null)
            {
              this.Position = agentData.Position;
              this.Rotation = agentData.Rotation;
            }
            else
            {
              this.Position = actor1.Position;
              this.Rotation = actor1.Rotation;
            }
          }
          else if (this.Actor is MerchantActor)
          {
            MerchantActor actor1 = this.Actor as MerchantActor;
            MerchantData merchantData = actor1.MerchantData;
            if (merchantData != null)
            {
              this.Position = merchantData.Position;
              this.Rotation = merchantData.Rotation;
            }
            else
            {
              this.Position = actor1.Position;
              this.Rotation = actor1.Rotation;
            }
          }
          this.ControllerEnabled = !Object.op_Inequality((Object) this.Actor.Controller, (Object) null) || ((Behaviour) this.Actor.Controller).get_enabled();
          this.AnimationEnabled = !Object.op_Inequality((Object) this.Actor.Animation, (Object) null) || ((Behaviour) this.Actor.Animation).get_enabled();
          if (this.IsAgent)
          {
            AgentActor agent = this.Agent;
            this.ModeCache = new ValueTuple<Desire.ActionType, Desire.ActionType>(agent.Mode, agent.BehaviorResources.Mode);
            this.BehaviorEnabled = !Object.op_Inequality((Object) agent.BehaviorResources, (Object) null) || ((Behaviour) agent.BehaviorResources).get_enabled();
          }
          else
          {
            this.ModeCache = new ValueTuple<Desire.ActionType, Desire.ActionType>(Desire.ActionType.Idle, Desire.ActionType.Idle);
            this.BehaviorEnabled = true;
          }
        }
      }

      public void Clear()
      {
        this.Actor = (Actor) null;
        this.Visibled = true;
        this.Position = Vector3.get_zero();
        this.Rotation = Quaternion.get_identity();
        this.ControllerEnabled = true;
        this.AnimationEnabled = true;
      }

      public PlayerActor Player
      {
        get
        {
          return this.Actor as PlayerActor;
        }
      }

      public AgentActor Agent
      {
        get
        {
          return this.Actor as AgentActor;
        }
      }

      public MerchantActor Merchant
      {
        get
        {
          return this.Actor as MerchantActor;
        }
      }

      public bool IsEmpty
      {
        get
        {
          return Object.op_Equality((Object) this.Actor, (Object) null);
        }
      }

      public bool IsPlayer
      {
        get
        {
          return this.Actor is PlayerActor;
        }
      }

      public bool IsAgent
      {
        get
        {
          return this.Actor is AgentActor;
        }
      }

      public bool IsMerchant
      {
        get
        {
          return this.Actor is MerchantActor;
        }
      }
    }
  }
}
