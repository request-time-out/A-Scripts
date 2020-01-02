// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.Player;
using AIProject.Scene;
using Illusion.Extensions;
using IllusionUtility.GetUtility;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  public class ActionPoint : Point, ICommandable
  {
    [SerializeField]
    protected EventType[] _playerDateEventType = new EventType[2];
    [SerializeField]
    private bool _enabledRangeCheck = true;
    [SerializeField]
    [ShowIf("_enabledRangeCheck", true)]
    private float _radius = 1f;
    [SerializeField]
    protected ActionPoint.ActionSlot _actionSlot = new ActionPoint.ActionSlot();
    protected CommandLabel.CommandInfo[][] _dateLabels = new CommandLabel.CommandInfo[2][];
    [SerializeField]
    private ObjectLayer _layer = ObjectLayer.Command;
    protected List<ValueTuple<int, int>> _bookingList = new List<ValueTuple<int, int>>();
    [SerializeField]
    private int _registerID;
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected int[] _idList;
    [SerializeField]
    [HideInEditorMode]
    protected EventType _playerEventType;
    [SerializeField]
    [HideInEditorMode]
    protected EventType _agentEventType;
    [SerializeField]
    protected EventType _agentDateEventType;
    protected List<ActionPointInfo> _agentInfos;
    protected List<ActionPointInfo> _playerInfos;
    protected Dictionary<int, List<DateActionPointInfo>> _playerDateInfos;
    protected List<DateActionPointInfo> _agentDateInfos;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    protected Transform _destination;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private GameObject[] _mapItemObjs;
    [SerializeField]
    private MapItemKeyValuePair[] _mapItemData;
    private NavMeshObstacle[] _obstacles;
    private NavMeshPath _pathForCalc;
    protected CommandLabel.CommandInfo[] _labels;
    protected CommandLabel.CommandInfo[] _sickLabels;
    private int? _hashCode;

    public static Dictionary<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> LabelTable { get; } = new Dictionary<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>>()
    {
      [EventType.Sleep] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(0, "寝る", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Sleep", false, (System.Action) null))),
      [EventType.Break] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(1, "休憩する", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Break", false, (System.Action) null))),
      [EventType.Search] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(6, "調べる", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Search", false, (System.Action) null))),
      [EventType.StorageIn] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(7, "ストレージ", (System.Action<PlayerActor, ActionPoint>) ((x, y) =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.BoxOpen);
        y.ChangeStateIn(x, "ItemBox", false, (System.Action) null);
      })),
      [EventType.Cook] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(9, "料理する", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Kitchen", false, (System.Action) null))),
      [EventType.DressIn] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(10, "脱衣所", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DressRoom", false, (System.Action) null))),
      [EventType.Move] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(14, "移動する", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Move", false, (System.Action) null))),
      [EventType.DoorOpen] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(19, "ドアを開ける", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DoorOpen", false, (System.Action) null))),
      [EventType.DoorClose] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(20, "ドアを閉める", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DoorClose", false, (System.Action) null))),
      [EventType.ClothChange] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(22, "クローゼット", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "ClothChange", false, (System.Action) null))),
      [EventType.Warp] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(23, "ワープ装置", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Warp", false, (System.Action) null)))
    };

    public static Dictionary<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> DateLabelTable { get; } = new Dictionary<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>>()
    {
      [EventType.Sleep] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(0, "寝る", (System.Action<PlayerActor, ActionPoint>) ((x, y) =>
      {
        ConfirmScene.Sentence = "一緒に寝た場合2人で行動状態が解除されます。";
        ConfirmScene.OnClickedYes = (System.Action) (() =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
          x.AgentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
          y.ChangeStateIn(x, "DateSleep", false, (System.Action) null);
        });
        ConfirmScene.OnClickedNo = (System.Action) (() =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
          MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
        });
        Singleton<Game>.Instance.LoadDialog();
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      })),
      [EventType.Break] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(1, "一緒に休憩する", (System.Action<PlayerActor, ActionPoint>) ((x, y) =>
      {
        x.AgentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
        y.ChangeStateIn(x, "DateBreak", false, (System.Action) null);
      })),
      [EventType.Eat] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(3, "一緒に御飯を食べる", (System.Action<PlayerActor, ActionPoint>) ((x, y) =>
      {
        x.AgentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
        y.ChangeStateIn(x, "DateEat", false, (System.Action) null);
      })),
      [EventType.Search] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(6, "調べる", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DateSearch", false, (System.Action) null))),
      [EventType.StorageIn] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(7, "ストレージ", (System.Action<PlayerActor, ActionPoint>) ((x, y) =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.BoxOpen);
        y.ChangeStateIn(x, "ItemBox", false, (System.Action) null);
      })),
      [EventType.Lesbian] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(13, "Hポイント", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "SpecialH", false, (System.Action) null))),
      [EventType.Move] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(14, "移動する", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Move", false, (System.Action) null))),
      [EventType.DoorOpen] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(19, "ドアを開ける", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DoorOpen", false, (System.Action) null))),
      [EventType.DoorClose] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(20, "ドアを閉める", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DoorClose", false, (System.Action) null))),
      [EventType.Warp] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(23, "ワープ装置", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Warp", false, (System.Action) null)))
    };

    public static Dictionary<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> SickLabelTable { get; } = new Dictionary<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>>()
    {
      [EventType.DoorOpen] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(19, "ドアを開ける", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DoorOpen", false, (System.Action) null))),
      [EventType.DoorClose] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(20, "ドアを閉める", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "DoorClose", false, (System.Action) null))),
      [EventType.Warp] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(23, "ワープ装置", (System.Action<PlayerActor, ActionPoint>) ((x, y) => y.ChangeStateIn(x, "Warp", false, (System.Action) null))),
      [EventType.Sleep] = new Tuple<int, string, System.Action<PlayerActor, ActionPoint>>(0, "ベッドに寝かせる", (System.Action<PlayerActor, ActionPoint>) ((x, y) => ActionPoint.PutOnTheBed(x, y)))
    };

    private static void PutOnTheBed(PlayerActor player, ActionPoint point)
    {
      player.CameraControl.CrossFade.FadeStart(-1f);
      player.Animation.StopAllAnimCoroutine();
      player.PlayerController.ChangeState("Normal");
      player.CameraControl.Mode = CameraMode.Normal;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      AgentActor agentPartner = player.AgentPartner;
      agentPartner.Animation.StopAllAnimCoroutine();
      agentPartner.IsSlave = false;
      agentPartner.DeactivateNavMeshAgent();
      agentPartner.Partner = (Actor) null;
      player.PlayerController.CommandArea.RemoveConsiderationObject((ICommandable) point);
      player.PlayerController.CommandArea.RefreshCommands();
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ =>
      {
        player.PlayerController.CommandArea.RemoveConsiderationObject((ICommandable) point);
        player.PlayerController.CommandArea.RefreshCommands();
      }));
      agentPartner.CurrentPoint = point;
      Desire.ActionType type = Desire.SickPairTable[agentPartner.PrevMode];
      agentPartner.ChangeBehavior(type);
      player.AgentPartner = (AgentActor) null;
      agentPartner.IsVisible = true;
    }

    public override int RegisterID
    {
      get
      {
        return this._registerID;
      }
      set
      {
        this._registerID = value;
      }
    }

    public int ID
    {
      get
      {
        return this._id;
      }
    }

    public int[] IDList
    {
      get
      {
        return this._idList;
      }
    }

    public EventType PlayerEventType
    {
      get
      {
        return this._playerEventType;
      }
    }

    public EventType AgentEventType
    {
      get
      {
        return this._agentEventType;
      }
    }

    public EventType[] PlayerDateEventType
    {
      get
      {
        return this._playerDateEventType;
      }
    }

    public EventType AgentDateEventType
    {
      get
      {
        return this._agentDateEventType;
      }
    }

    public List<Transform> NavMeshPoints { get; set; } = new List<Transform>();

    public HPoint HPoint { get; set; }

    public Vector3 CommandCenter
    {
      get
      {
        return Object.op_Inequality((Object) this._commandBasePoint, (Object) null) ? this._commandBasePoint.get_position() : ((Component) this).get_transform().get_position();
      }
    }

    protected void ChangeStateIn(
      PlayerActor actor,
      string stateName,
      bool isDate,
      System.Action onCompleted = null)
    {
      actor.PlayerController.ChangeState(stateName, this, onCompleted);
    }

    protected void ChangeStateOut(PlayerActor actor, string stateName)
    {
      actor.PlayerController.ChangeState(stateName);
      actor.CameraControl.Mode = CameraMode.Normal;
    }

    protected void ChangeStateDateIn(PlayerActor actor, string stateName, System.Action onCompleted = null)
    {
      this.ChangeStateIn(actor, stateName, true, onCompleted);
      actor.AgentPartner.AgentController.ChangeState(stateName);
    }

    protected void ChangeStateDateOut(PlayerActor actor, string stateName)
    {
      this.ChangeStateOut(actor, stateName);
      actor.AgentPartner.AgentController.ChangeState(stateName);
    }

    public bool HasPlayerDateActionPointInfo(byte sex, EventType eventType)
    {
      List<DateActionPointInfo> dateActionPointInfoList;
      if (!this._playerDateInfos.TryGetValue((int) sex, out dateActionPointInfoList))
        return false;
      foreach (DateActionPointInfo dateActionPointInfo in dateActionPointInfoList)
      {
        if (dateActionPointInfo.eventTypeMask == eventType)
          return true;
      }
      return false;
    }

    public bool HasAgentActionPointInfo(EventType eventType)
    {
      foreach (ActionPointInfo agentInfo in this._agentInfos)
      {
        if (agentInfo.eventTypeMask == eventType)
          return true;
      }
      return false;
    }

    public bool TryGetAgentActionPointInfo(EventType eventType, out ActionPointInfo outInfo)
    {
      foreach (ActionPointInfo agentInfo in this._agentInfos)
      {
        if (agentInfo.eventTypeMask == eventType)
        {
          outInfo = agentInfo;
          return true;
        }
      }
      outInfo = new ActionPointInfo();
      return false;
    }

    public void GetAgentActionPointInfos(EventType eventType, List<ActionPointInfo> outInfoList)
    {
      foreach (ActionPointInfo agentInfo in this._agentInfos)
      {
        if (agentInfo.eventTypeMask == eventType)
          outInfoList.Add(agentInfo);
      }
    }

    public void GetAgentDateActionPointInfos(
      EventType eventType,
      List<DateActionPointInfo> outInfoList)
    {
      foreach (DateActionPointInfo agentDateInfo in this._agentDateInfos)
      {
        if (agentDateInfo.eventTypeMask == eventType)
          outInfoList.Add(agentDateInfo);
      }
    }

    public ActionPointInfo GetActionPointInfo(AgentActor agent)
    {
      List<ActionPointInfo> actionPointInfoList1 = ListPool<ActionPointInfo>.Get();
      this.GetAgentActionPointInfos(agent.EventKey, actionPointInfoList1);
      int index = (int) AIProject.Definitions.Action.NameTable[agent.EventKey].Item1;
      List<int> intList = Singleton<Resources>.Instance.Animation.PersonalActionListTable[agent.ChaControl.fileParam.personality][index];
      List<ActionPointInfo> actionPointInfoList2 = ListPool<ActionPointInfo>.Get();
      foreach (ActionPointInfo actionPointInfo in actionPointInfoList1)
      {
        if (intList.Contains(actionPointInfo.poseID))
          actionPointInfoList2.Add(actionPointInfo);
      }
      ListPool<ActionPointInfo>.Release(actionPointInfoList1);
      ActionPointInfo actionPointInfo1;
      if ((double) agent.AgentData.StatsTable[1] <= 40.0)
      {
        Dictionary<int, Dictionary<int, ActAnimFlagData>> flagTable = Singleton<Resources>.Instance.Action.AgentActionFlagTable;
        Dictionary<int, ActAnimFlagData> dictionary;
        ActAnimFlagData actAnimFlagData;
        List<ActionPointInfo> all = actionPointInfoList2.FindAll((Predicate<ActionPointInfo>) (x => flagTable.TryGetValue(x.eventID, out dictionary) && dictionary.TryGetValue(x.poseID, out actAnimFlagData) && actAnimFlagData.isBadMood));
        actionPointInfo1 = all.IsNullOrEmpty<ActionPointInfo>() ? actionPointInfoList2.GetElement<ActionPointInfo>(Random.Range(0, actionPointInfoList2.Count)) : all.GetElement<ActionPointInfo>(Random.Range(0, all.Count));
      }
      else
        actionPointInfo1 = actionPointInfoList2.GetElement<ActionPointInfo>(Random.Range(0, actionPointInfoList2.Count));
      ListPool<ActionPointInfo>.Release(actionPointInfoList2);
      return actionPointInfo1;
    }

    public DateActionPointInfo GetDateActionPointInfo(AgentActor agent)
    {
      List<DateActionPointInfo> dateActionPointInfoList1 = ListPool<DateActionPointInfo>.Get();
      this.GetAgentDateActionPointInfos(agent.EventKey, dateActionPointInfoList1);
      int index = (int) AIProject.Definitions.Action.NameTable[agent.EventKey].Item1;
      List<int> intList = Singleton<Resources>.Instance.Animation.PersonalActionListTable[agent.ChaControl.fileParam.personality][index];
      List<DateActionPointInfo> dateActionPointInfoList2 = ListPool<DateActionPointInfo>.Get();
      foreach (DateActionPointInfo dateActionPointInfo in dateActionPointInfoList1)
      {
        if (intList.Contains(dateActionPointInfo.poseIDA))
          dateActionPointInfoList2.Add(dateActionPointInfo);
      }
      ListPool<DateActionPointInfo>.Release(dateActionPointInfoList1);
      DateActionPointInfo element = dateActionPointInfoList2.GetElement<DateActionPointInfo>(Random.Range(0, dateActionPointInfoList2.Count));
      ListPool<DateActionPointInfo>.Release(dateActionPointInfoList2);
      return element;
    }

    public bool FindAgentActionPointInfo(
      EventType eventType,
      int poseID,
      out ActionPointInfo outInfo)
    {
      foreach (ActionPointInfo agentInfo in this._agentInfos)
      {
        if (agentInfo.eventTypeMask == eventType && agentInfo.poseID == poseID)
        {
          outInfo = agentInfo;
          return true;
        }
      }
      outInfo = new ActionPointInfo();
      return false;
    }

    public bool FindPlayerDateActionPointInfo(
      byte sex,
      int pointID,
      out DateActionPointInfo outInfo)
    {
      if (!this._playerDateInfos.TryGetValue((int) sex, out List<DateActionPointInfo> _))
      {
        outInfo = new DateActionPointInfo();
        return false;
      }
      foreach (DateActionPointInfo dateActionPointInfo in this._playerDateInfos[(int) sex])
      {
        if (dateActionPointInfo.pointID == pointID)
        {
          outInfo = dateActionPointInfo;
          return true;
        }
      }
      outInfo = new DateActionPointInfo();
      return false;
    }

    public bool TryGetPlayerActionPointInfo(EventType eventType, out ActionPointInfo outInfo)
    {
      foreach (ActionPointInfo playerInfo in this._playerInfos)
      {
        if (playerInfo.eventTypeMask == eventType)
        {
          outInfo = playerInfo;
          return true;
        }
      }
      outInfo = new ActionPointInfo();
      return false;
    }

    public bool TryGetPlayerDateActionPointInfo(
      byte sex,
      EventType eventType,
      out DateActionPointInfo outInfo)
    {
      List<DateActionPointInfo> dateActionPointInfoList;
      if (!this._playerDateInfos.TryGetValue((int) sex, out dateActionPointInfoList))
      {
        outInfo = new DateActionPointInfo();
        return false;
      }
      foreach (DateActionPointInfo dateActionPointInfo in dateActionPointInfoList)
      {
        if (dateActionPointInfo.eventTypeMask == eventType)
        {
          outInfo = dateActionPointInfo;
          return true;
        }
      }
      outInfo = new DateActionPointInfo();
      return false;
    }

    public bool TryGetAgentDateActionPointInfo(EventType eventType, out DateActionPointInfo outInfo)
    {
      foreach (DateActionPointInfo agentDateInfo in this._agentDateInfos)
      {
        if (agentDateInfo.eventTypeMask == eventType)
        {
          outInfo = agentDateInfo;
          return true;
        }
      }
      outInfo = new DateActionPointInfo();
      return false;
    }

    public bool ContainsAgentDateActionPointInfo(EventType eventType)
    {
      foreach (DateActionPointInfo agentDateInfo in this._agentDateInfos)
      {
        if (agentDateInfo.eventTypeMask == eventType)
          return true;
      }
      return false;
    }

    public Actor Actor { get; set; }

    public bool EnabledRangeCheck
    {
      get
      {
        return this._enabledRangeCheck;
      }
    }

    public float Radius
    {
      get
      {
        return this._radius;
      }
    }

    public GameObject[] MapItemObjs
    {
      get
      {
        return this._mapItemObjs;
      }
      set
      {
        this._mapItemObjs = value;
      }
    }

    public MapItemKeyValuePair[] MapItemData
    {
      get
      {
        return this._mapItemData;
      }
      set
      {
        this._mapItemData = value;
      }
    }

    public Vector3 LocatedPosition
    {
      get
      {
        return Object.op_Inequality((Object) this._destination, (Object) null) ? this._destination.get_position() : ((Component) this).get_transform().get_position();
      }
    }

    public ActionPoint.ActionSlot Slot
    {
      get
      {
        return this._actionSlot;
      }
    }

    public List<ActionPoint> ConnectedActionPoints { get; set; } = new List<ActionPoint>();

    public List<ActionPoint> GroupActionPoints { get; private set; } = new List<ActionPoint>();

    public virtual bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      if (this.TutorialHideMode())
        return false;
      Vector3 commandCenter = this.CommandCenter;
      commandCenter.y = (__Null) 0.0;
      float num1 = Vector3.Distance(basePosition, commandCenter);
      List<ActionPoint> connectedActionPoints = this.ConnectedActionPoints;
      if (connectedActionPoints != null)
      {
        foreach (ActionPoint actionPoint in connectedActionPoints)
        {
          if (!Object.op_Equality((Object) actionPoint, (Object) null) && !actionPoint.IsNeutralCommand)
            return false;
        }
      }
      if (this.CommandType == CommandType.Forward)
      {
        float num2 = !this._enabledRangeCheck ? radiusA : radiusA + this._radius;
        if ((double) num1 > (double) num2)
          return false;
        Vector3 vector3 = commandCenter;
        vector3.y = (__Null) 0.0;
        float num3 = angle / 2f;
        if ((double) Vector3.Angle(Vector3.op_Subtraction(vector3, basePosition), forward) > (double) num3)
          return false;
      }
      else
      {
        float num2 = !this._enabledRangeCheck ? radiusB : radiusB + this._radius;
        if ((double) distance > (double) num2)
          return false;
      }
      return true;
    }

    public bool IsReachable(NavMeshAgent navMeshAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      if (((Behaviour) navMeshAgent).get_isActiveAndEnabled())
      {
        bool flag2 = false;
        using (List<Transform>.Enumerator enumerator = this.NavMeshPoints.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Transform current = enumerator.Current;
            navMeshAgent.CalculatePath(current.get_position(), this._pathForCalc);
            if (this._pathForCalc.get_status() == null)
            {
              float num1 = 0.0f;
              Vector3[] corners = this._pathForCalc.get_corners();
              float num2 = (this.CommandType != CommandType.Forward ? radiusB : radiusA) + this._radius;
              for (int index = 0; index < corners.Length - 1; ++index)
              {
                float num3 = Vector3.Distance(corners[index], corners[index + 1]);
                num1 += num3;
              }
              if ((double) num1 < (double) num2)
              {
                flag2 = true;
                break;
              }
            }
          }
        }
        if (!flag2)
          flag1 = false;
      }
      else
        flag1 = false;
      return flag1;
    }

    public bool IsReachable(NavMeshAgent navMeshAgent, float radius)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      if (((Behaviour) navMeshAgent).get_isActiveAndEnabled())
      {
        bool flag2 = false;
        using (List<Transform>.Enumerator enumerator = this.NavMeshPoints.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Transform current = enumerator.Current;
            navMeshAgent.CalculatePath(current.get_position(), this._pathForCalc);
            if (this._pathForCalc.get_status() == null)
            {
              float num1 = 0.0f;
              Vector3[] corners = this._pathForCalc.get_corners();
              float num2 = radius + this._radius;
              for (int index = 0; index < corners.Length - 1; ++index)
              {
                float num3 = Vector3.Distance(corners[index], corners[index + 1]);
                num1 += num3;
              }
              if ((double) num1 < (double) num2)
              {
                flag2 = true;
                break;
              }
            }
          }
        }
        if (!flag2)
          flag1 = false;
      }
      else
        flag1 = false;
      return flag1;
    }

    public bool IsImpossible { get; protected set; }

    public void SetForceImpossible(bool value)
    {
      this.IsImpossible = value;
    }

    public virtual bool SetImpossible(bool value, Actor actor)
    {
      if (this.IsImpossible != value)
      {
        this.IsImpossible = value;
        if (value)
          this.SetSlot(actor);
        else if (Object.op_Inequality((Object) this._actionSlot.Actor, (Object) null) && Object.op_Equality((Object) this._actionSlot.Actor, (Object) actor))
          this.ReleaseSlot(actor);
        return true;
      }
      if (value)
      {
        if (!Object.op_Equality((Object) this._actionSlot.Actor, (Object) null))
          return false;
        this.IsImpossible = value;
        this.SetSlot(actor);
        return true;
      }
      if (Object.op_Inequality((Object) this._actionSlot.Actor, (Object) null) && Object.op_Equality((Object) this._actionSlot.Actor, (Object) actor))
      {
        this.IsImpossible = value;
        this.ReleaseSlot(actor);
        return true;
      }
      this.IsImpossible = false;
      return true;
    }

    public virtual bool IsNeutralCommand
    {
      get
      {
        return !this.TutorialHideMode() && !Object.op_Inequality((Object) this._actionSlot.Actor, (Object) null) && this._bookingList.IsNullOrEmpty<ValueTuple<int, int>>();
      }
    }

    public Transform TargetCommun { get; set; }

    public Vector3 Position
    {
      get
      {
        return this.LocatedPosition;
      }
    }

    public virtual CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return Singleton<Manager.Map>.Instance.Player.PlayerController.State is Onbu ? this._sickLabels : this._labels;
      }
    }

    public virtual CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        if (!Singleton<Manager.Map>.IsInstance() || Object.op_Equality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null))
          return (CommandLabel.CommandInfo[]) null;
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        if (this.HasPlayerDateActionPointInfo(player.ChaControl.sex, EventType.Lesbian))
        {
          if (player.ChaControl.sex == (byte) 1 && !Singleton<Manager.Map>.Instance.Player.ChaControl.fileParam.futanari)
            return (CommandLabel.CommandInfo[]) null;
        }
        else if (this.HasPlayerDateActionPointInfo(player.ChaControl.sex, EventType.Eat) && player.PlayerData.DateEatTrigger)
          return (CommandLabel.CommandInfo[]) null;
        return this._dateLabels[(int) player.ChaControl.sex];
      }
    }

    public ObjectLayer Layer
    {
      get
      {
        return this._layer;
      }
    }

    public CommandType CommandType
    {
      get
      {
        return this._playerEventType.Contains(EventType.Move) || this._playerEventType.Contains(EventType.DoorOpen) || this._playerEventType.Contains(EventType.DoorClose) ? CommandType.AllAround : CommandType.Forward;
      }
    }

    public virtual bool TutorialHideMode()
    {
      if (!Manager.Map.TutorialMode || this._playerEventType.Contains(EventType.Move))
        return false;
      CommonDefine commonDefine = !Singleton<Resources>.IsInstance() ? (CommonDefine) null : Singleton<Resources>.Instance.CommonDefine;
      if (Object.op_Inequality((Object) commonDefine, (Object) null))
      {
        int[] kitchenPointIdList = commonDefine.Tutorial.KitchenPointIDList;
        if (!kitchenPointIdList.IsNullOrEmpty<int>() && ((IEnumerable<int>) kitchenPointIdList).Contains<int>(this._id))
          return false;
      }
      return true;
    }

    private void Awake()
    {
      this._obstacles = (NavMeshObstacle[]) ((Component) this).GetComponentsInChildren<NavMeshObstacle>(true);
      foreach (NavMeshObstacle obstacle in this._obstacles)
        ;
    }

    public void Init()
    {
      DefinePack.MapGroup mapDefines = Singleton<Resources>.Instance.DefinePack.MapDefines;
      this.NavMeshPoints.Add(((Component) this).get_transform());
      List<GameObject> gameObjectList = ListPool<GameObject>.Get();
      ((Component) this).get_transform().FindLoopPrefix(gameObjectList, mapDefines.NavMeshTargetName);
      if (!gameObjectList.IsNullOrEmpty<GameObject>())
      {
        using (List<GameObject>.Enumerator enumerator = gameObjectList.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.NavMeshPoints.Add(enumerator.Current.get_transform());
        }
      }
      ListPool<GameObject>.Release(gameObjectList);
      if (Object.op_Equality((Object) this._destination, (Object) null))
        this._destination = ((Component) this).get_transform();
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        this._commandBasePoint = ((Component) this).get_transform().FindLoop(mapDefines.CommandTargetName)?.get_transform() ?? ((Component) this).get_transform();
      this._agentInfos = new List<ActionPointInfo>();
      this._playerInfos = new List<ActionPointInfo>();
      this._playerDateInfos = new Dictionary<int, List<DateActionPointInfo>>()
      {
        [0] = new List<DateActionPointInfo>(),
        [1] = new List<DateActionPointInfo>()
      };
      this._agentDateInfos = new List<DateActionPointInfo>();
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      List<ActionPointInfo> actionPointInfoList1;
      if (Singleton<Resources>.Instance.Map.AgentActionPointInfoTable[0].TryGetValue(this._id, out actionPointInfoList1))
      {
        foreach (ActionPointInfo actionPointInfo in actionPointInfoList1)
          this._agentInfos.Add(actionPointInfo);
      }
      List<ActionPointInfo> actionPointInfoList2;
      if (Singleton<Resources>.Instance.Map.PlayerActionPointInfoTable[0].TryGetValue(this._id, out actionPointInfoList2))
      {
        foreach (ActionPointInfo actionPointInfo in actionPointInfoList2)
          this._playerInfos.Add(actionPointInfo);
      }
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      Dictionary<int, Dictionary<int, List<DateActionPointInfo>>> dictionary;
      if (Singleton<Resources>.Instance.Map.PlayerDateActionPointInfoTable.TryGetValue(0, out dictionary))
      {
        foreach (KeyValuePair<int, Dictionary<int, List<DateActionPointInfo>>> keyValuePair in dictionary)
        {
          int key = keyValuePair.Key;
          List<DateActionPointInfo> dateActionPointInfoList1;
          if (keyValuePair.Value.TryGetValue(this._id, out dateActionPointInfoList1))
          {
            List<DateActionPointInfo> dateActionPointInfoList2;
            if (!this._playerDateInfos.TryGetValue(key, out dateActionPointInfoList2))
            {
              List<DateActionPointInfo> dateActionPointInfoList3 = new List<DateActionPointInfo>();
              this._playerDateInfos[key] = dateActionPointInfoList3;
              dateActionPointInfoList2 = dateActionPointInfoList3;
            }
            foreach (DateActionPointInfo dateActionPointInfo in dateActionPointInfoList1)
              dateActionPointInfoList2.Add(dateActionPointInfo);
          }
        }
      }
      List<DateActionPointInfo> dateActionPointInfoList4;
      if (Singleton<Resources>.Instance.Map.AgentDateActionPointInfoTable[0].TryGetValue(this._id, out dateActionPointInfoList4))
      {
        foreach (DateActionPointInfo dateActionPointInfo in dateActionPointInfoList4)
          this._agentDateInfos.Add(dateActionPointInfo);
      }
      if (!this._idList.IsNullOrEmpty<int>())
      {
        this._agentInfos.Clear();
        this._playerInfos.Clear();
        foreach (KeyValuePair<int, List<DateActionPointInfo>> playerDateInfo in this._playerDateInfos)
          playerDateInfo.Value.Clear();
        this._agentDateInfos.Clear();
        foreach (int id in this._idList)
        {
          if (Singleton<Resources>.Instance.Map.PlayerActionPointInfoTable[0].TryGetValue(id, out actionPointInfoList1))
          {
            foreach (ActionPointInfo actionPointInfo in actionPointInfoList1)
              this._playerInfos.Add(actionPointInfo);
          }
          if (Singleton<Resources>.Instance.Map.AgentActionPointInfoTable[0].TryGetValue(id, out actionPointInfoList2))
          {
            foreach (ActionPointInfo actionPointInfo in actionPointInfoList2)
              this._agentInfos.Add(actionPointInfo);
          }
          if (Singleton<Resources>.Instance.Map.PlayerDateActionPointInfoTable.TryGetValue(0, out dictionary))
          {
            foreach (KeyValuePair<int, Dictionary<int, List<DateActionPointInfo>>> keyValuePair in dictionary)
            {
              int key = keyValuePair.Key;
              List<DateActionPointInfo> dateActionPointInfoList1;
              if (keyValuePair.Value.TryGetValue(id, out dateActionPointInfoList1))
              {
                List<DateActionPointInfo> dateActionPointInfoList2;
                if (!this._playerDateInfos.TryGetValue(key, out dateActionPointInfoList2))
                {
                  List<DateActionPointInfo> dateActionPointInfoList3 = new List<DateActionPointInfo>();
                  this._playerDateInfos[key] = dateActionPointInfoList3;
                  dateActionPointInfoList2 = dateActionPointInfoList3;
                }
                foreach (DateActionPointInfo dateActionPointInfo in dateActionPointInfoList1)
                  dateActionPointInfoList2.Add(dateActionPointInfo);
              }
            }
          }
          if (Singleton<Resources>.Instance.Map.AgentDateActionPointInfoTable[0].TryGetValue(id, out dateActionPointInfoList4))
          {
            foreach (DateActionPointInfo dateActionPointInfo in dateActionPointInfoList4)
              this._agentDateInfos.Add(dateActionPointInfo);
          }
        }
      }
      this.Start();
      EventType eventType1 = (EventType) 0;
      foreach (ActionPointInfo playerInfo in this._playerInfos)
        eventType1 |= playerInfo.eventTypeMask;
      this._playerEventType = eventType1;
      EventType eventType2 = (EventType) 0;
      foreach (ActionPointInfo agentInfo in this._agentInfos)
        eventType2 |= agentInfo.eventTypeMask;
      this._agentEventType = eventType2;
      this._playerDateEventType = new EventType[2];
      for (int index = 0; index < 2; ++index)
      {
        EventType eventType3 = (EventType) 0;
        foreach (DateActionPointInfo dateActionPointInfo in this._playerDateInfos[index])
          eventType3 |= dateActionPointInfo.eventTypeMask;
        this._playerDateEventType[index] = eventType3;
      }
      EventType eventType4 = (EventType) 0;
      foreach (DateActionPointInfo agentDateInfo in this._agentDateInfos)
        eventType4 |= agentDateInfo.eventTypeMask;
      this._agentDateEventType = eventType4;
      this.InitSub();
    }

    protected virtual void InitSub()
    {
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      List<CommandLabel.CommandInfo> toRelease1 = ListPool<CommandLabel.CommandInfo>.Get();
      foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.LabelTable)
      {
        KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
        ActionPoint actionPoint = this;
        if (this._playerEventType.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
        {
          ActionPointInfo actionPointInfo = this._playerInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == pair.Key));
          string actionName = actionPointInfo.actionName;
          Sprite sprite;
          Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
          Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
          CommandLabel.CommandInfo commandInfo = new CommandLabel.CommandInfo()
          {
            Text = actionName,
            Icon = sprite,
            IsHold = pair.Key == EventType.Sleep || pair.Key == EventType.Break || (pair.Key == EventType.Search || pair.Key == EventType.StorageIn) || (pair.Key == EventType.Cook || pair.Key == EventType.DressIn) || pair.Key == EventType.ClothChange,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = transform,
            Condition = (Func<PlayerActor, bool>) null,
            Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, actionPoint))
          };
          if (pair.Key == EventType.Sleep)
          {
            commandInfo.Condition = (Func<PlayerActor, bool>) (player => Singleton<Manager.Map>.Instance.CanSleepInTime());
            commandInfo.ErrorText = (Func<PlayerActor, string>) (player => Singleton<Manager.Map>.Instance.CanSleepInTime() ? string.Empty : "夜になるまで寝ることはできません");
          }
          toRelease1.Add(commandInfo);
        }
      }
      this._labels = toRelease1.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease1);
      for (int key = 0; key < 2; ++key)
      {
        List<CommandLabel.CommandInfo> toRelease2 = ListPool<CommandLabel.CommandInfo>.Get();
        foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.DateLabelTable)
        {
          KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
          ActionPoint actionPoint = this;
          List<DateActionPointInfo> dateActionPointInfoList;
          if (this._playerDateEventType[key].Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _) && this._playerDateInfos.TryGetValue(key, out dateActionPointInfoList))
          {
            DateActionPointInfo dateActionPointInfo = dateActionPointInfoList.Find((Predicate<DateActionPointInfo>) (x => x.eventTypeMask == pair.Key));
            string actionName = dateActionPointInfo.actionName;
            Sprite sprite;
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(dateActionPointInfo.iconID, out sprite);
            Transform transform = ((Component) this).get_transform().FindLoop(dateActionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
            toRelease2.Add(new CommandLabel.CommandInfo()
            {
              Text = actionName,
              Icon = sprite,
              IsHold = pair.Key == EventType.Sleep || pair.Key == EventType.Break || (pair.Key == EventType.Eat || pair.Key == EventType.Search) || (pair.Key == EventType.StorageIn || pair.Key == EventType.Cook || (pair.Key == EventType.DressIn || pair.Key == EventType.Lesbian)) || pair.Key == EventType.ClothChange,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = transform,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, actionPoint))
            });
          }
        }
        this._dateLabels[key] = toRelease2.ToArray();
        ListPool<CommandLabel.CommandInfo>.Release(toRelease2);
      }
      List<CommandLabel.CommandInfo> toRelease3 = ListPool<CommandLabel.CommandInfo>.Get();
      foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.SickLabelTable)
      {
        KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
        ActionPoint actionPoint = this;
        if (this._playerEventType.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
        {
          ActionPointInfo actionPointInfo = this._playerInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == pair.Key));
          Sprite sprite;
          Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
          Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
          toRelease3.Add(new CommandLabel.CommandInfo()
          {
            Text = pair.Value.Item2,
            Icon = sprite,
            IsHold = true,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = transform,
            Condition = (Func<PlayerActor, bool>) null,
            Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, actionPoint))
          });
        }
      }
      this._sickLabels = toRelease3.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease3);
    }

    public void SetActiveMapItemObjs(bool active)
    {
      if (!this._mapItemObjs.IsNullOrEmpty<GameObject>())
      {
        foreach (GameObject mapItemObj in this._mapItemObjs)
        {
          if (mapItemObj.get_activeSelf() != active)
            mapItemObj.SetActive(active);
        }
      }
      if (this._mapItemData.IsNullOrEmpty<MapItemKeyValuePair>())
        return;
      foreach (MapItemKeyValuePair itemKeyValuePair in this._mapItemData)
      {
        if (itemKeyValuePair != null && !Object.op_Equality((Object) itemKeyValuePair.ItemObj, (Object) null))
          itemKeyValuePair.ItemObj.SetActiveIfDifferent(active);
      }
    }

    public GameObject CreateEventItems(int itemID, string parentName, bool isSync)
    {
      if (this._mapItemData.IsNullOrEmpty<MapItemKeyValuePair>())
        return (GameObject) null;
      return ((IEnumerable<MapItemKeyValuePair>) this._mapItemData).FirstOrDefault<MapItemKeyValuePair>((Func<MapItemKeyValuePair, bool>) (x => x.ID == itemID))?.ItemObj;
    }

    public override void LocateGround()
    {
      base.LocateGround();
    }

    public Actor Reserver { get; set; }

    public Actor BookingUser { get; private set; }

    protected override void OnDisable()
    {
      PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Inequality((Object) playerActor, (Object) null))
      {
        CommandArea commandArea = playerActor.PlayerController.CommandArea;
        if (this.SetImpossible(false, (Actor) playerActor) && commandArea.ContainsConsiderationObject((ICommandable) this))
        {
          commandArea.RemoveConsiderationObject((ICommandable) this);
          commandArea.RefreshCommands();
        }
      }
      base.OnDisable();
    }

    public int GetPriority(int charaID)
    {
      return 0 <= charaID ? charaID : charaID * -1 * 1000;
    }

    public bool AddBooking(Actor actor)
    {
      int charaID = actor.ID;
      List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
      toRelease.Add(this);
      toRelease.AddRange((IEnumerable<ActionPoint>) this.ConnectedActionPoints);
      bool flag = false;
      foreach (ActionPoint actionPoint in toRelease)
      {
        if (!actionPoint._bookingList.Exists((Predicate<ValueTuple<int, int>>) (x => x.Item2 == charaID)))
        {
          actionPoint._bookingList.Add(new ValueTuple<int, int>(this.GetPriority(charaID), charaID));
          actionPoint._bookingList.Sort((Comparison<ValueTuple<int, int>>) ((x, y) => (int) (x.Item1 - y.Item1)));
          flag = true;
        }
      }
      ListPool<ActionPoint>.Release(toRelease);
      return flag;
    }

    public bool RemoveBooking(Actor actor)
    {
      int charaID = actor.ID;
      List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
      toRelease.Add(this);
      toRelease.AddRange((IEnumerable<ActionPoint>) this.ConnectedActionPoints);
      bool flag = false;
      foreach (ActionPoint actionPoint in toRelease)
      {
        int num = actionPoint._bookingList.RemoveAll((Predicate<ValueTuple<int, int>>) (x => x.Item2 == charaID));
        flag |= 1 <= num;
        if (0 < num)
          actionPoint._bookingList.Sort((Comparison<ValueTuple<int, int>>) ((x, y) => (int) (x.Item1 - y.Item1)));
      }
      if (Object.op_Equality((Object) this.BookingUser, (Object) actor))
        this.RemoveBookingUser(actor);
      ListPool<ActionPoint>.Release(toRelease);
      return flag;
    }

    public bool OffMeshAvailablePoint(Actor actor)
    {
      if (Object.op_Inequality((Object) this.BookingUser, (Object) null))
        return false;
      if (!this._bookingList.IsNullOrEmpty<ValueTuple<int, int>>())
        return this._bookingList[0].Item2 == actor.ID;
      this.AddBooking(actor);
      return true;
    }

    public bool ForceUse(Actor user)
    {
      if (!Object.op_Equality((Object) this.BookingUser, (Object) null) && !Object.op_Equality((Object) this.BookingUser, (Object) user))
        return false;
      this.SetBookingUser(user);
      if (Singleton<Manager.Map>.IsInstance())
      {
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        CommandArea commandArea = player.PlayerController.CommandArea;
        List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
        toRelease.Add(this);
        toRelease.AddRange((IEnumerable<ActionPoint>) this.ConnectedActionPoints);
        bool flag = false;
        foreach (ActionPoint actionPoint in toRelease)
        {
          if (!Object.op_Equality((Object) actionPoint, (Object) null) && actionPoint.SetImpossible(false, (Actor) player) && commandArea.ContainsConsiderationObject((ICommandable) actionPoint))
          {
            commandArea.RemoveConsiderationObject((ICommandable) actionPoint);
            flag = true;
          }
        }
        if (flag)
          commandArea.RefreshCommands();
        ListPool<ActionPoint>.Release(toRelease);
      }
      return true;
    }

    public bool SetBookingUser(Actor user)
    {
      if (Object.op_Equality((Object) user, (Object) null))
        return false;
      List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
      toRelease.Add(this);
      toRelease.AddRange((IEnumerable<ActionPoint>) this.ConnectedActionPoints);
      bool flag = true;
      foreach (ActionPoint actionPoint in toRelease)
      {
        if (!Object.op_Equality((Object) actionPoint, (Object) null))
        {
          flag = ((flag ? 1 : 0) & (Object.op_Equality((Object) actionPoint.BookingUser, (Object) null) ? 1 : (Object.op_Equality((Object) actionPoint.BookingUser, (Object) user) ? 1 : 0))) != 0;
          actionPoint.BookingUser = user;
        }
      }
      ListPool<ActionPoint>.Release(toRelease);
      return flag;
    }

    public bool RemoveBookingUser(Actor user)
    {
      if (Object.op_Equality((Object) user, (Object) null))
        return false;
      List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
      toRelease.Add(this);
      toRelease.AddRange((IEnumerable<ActionPoint>) this.ConnectedActionPoints);
      bool flag = true;
      foreach (ActionPoint actionPoint in toRelease)
      {
        if (!Object.op_Equality((Object) actionPoint, (Object) null))
        {
          flag = ((flag ? 1 : 0) & (Object.op_Equality((Object) actionPoint.BookingUser, (Object) null) ? 1 : (Object.op_Equality((Object) actionPoint.BookingUser, (Object) user) ? 1 : 0))) != 0;
          actionPoint.BookingUser = (Actor) null;
        }
      }
      ListPool<ActionPoint>.Release(toRelease);
      return true;
    }

    private void TransitionAction()
    {
    }

    public Tuple<Transform, Transform> GetSlot(Actor actor)
    {
      return new Tuple<Transform, Transform>(this._actionSlot.Point, this._actionSlot.RecoveryPoint);
    }

    public Tuple<Transform, Transform> SetSlot(Actor actor)
    {
      if (Object.op_Equality((Object) this._actionSlot.Actor, (Object) actor))
      {
        actor.CurrentPoint = this;
        return this.GetSlot(actor);
      }
      this._actionSlot.Actor = actor;
      return this.GetSlot(actor);
    }

    public void ReleaseSlot(Actor actor)
    {
      if (!Object.op_Equality((Object) actor, (Object) this._actionSlot.Actor))
        return;
      this._actionSlot.Actor = (Actor) null;
      this.Reserver = (Actor) null;
      actor.CurrentPoint = (ActionPoint) null;
    }

    public int InstanceID
    {
      get
      {
        if (this._hashCode.HasValue)
          return this._hashCode.Value;
        this._hashCode = new int?(((Object) this).GetInstanceID());
        return this._hashCode.Value;
      }
    }

    public void CreateByproduct(int actionID, int poseID)
    {
      MapItemNull component = (MapItemNull) ((Component) this).GetComponent<MapItemNull>();
      Dictionary<int, ByproductInfo> dictionary;
      ByproductInfo byproductInfo;
      if (Object.op_Equality((Object) component, (Object) null) || !Singleton<Resources>.IsInstance() || (!Singleton<Resources>.Instance.Action.ByproductList.TryGetValue(actionID, out dictionary) || !dictionary.TryGetValue(poseID, out byproductInfo)))
        return;
      List<int> element = byproductInfo.ItemList.GetElement<List<int>>(Random.Range(0, byproductInfo.ItemList.Count));
      component.SetActiveObjs(true, element);
    }

    public void DestroyByproduct(int actionID, int poseID)
    {
      MapItemNull component = (MapItemNull) ((Component) this).GetComponent<MapItemNull>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      component.SetActiveObjs(false, (List<int>) null);
    }

    public bool IsReserved(AgentActor agent)
    {
      if (Object.op_Inequality((Object) this.Reserver, (Object) null) && Object.op_Inequality((Object) this.Reserver, (Object) agent))
      {
        if (!(this.Reserver is AgentActor) || Object.op_Equality((Object) (this.Reserver as AgentActor).TargetInSightActionPoint, (Object) this))
          return true;
        this.Reserver = (Actor) null;
      }
      return false;
    }

    [Serializable]
    public struct PoseTypePair
    {
      public EventType eventType;
      public PoseType poseType;

      public PoseTypePair(EventType eventType_, PoseType poseType_)
      {
        this.eventType = eventType_;
        this.poseType = poseType_;
      }
    }

    [Serializable]
    public class PointPair
    {
      [SerializeField]
      private Transform _point;
      [SerializeField]
      private Transform _recoveryPoint;

      public Transform Point
      {
        get
        {
          return this._point;
        }
        set
        {
          this._point = value;
        }
      }

      public Transform RecoveryPoint
      {
        get
        {
          return this._recoveryPoint ?? (this._recoveryPoint = this._point);
        }
        set
        {
          this._recoveryPoint = value;
        }
      }
    }

    public enum DirectionKind
    {
      Free,
      Lock,
      Look,
    }

    [Serializable]
    public class ActionSlotTable : IEnumerable<ActionPoint.ActionSlot>, IEnumerable
    {
      [SerializeField]
      private List<ActionPoint.ActionSlot> _table = new List<ActionPoint.ActionSlot>();

      public List<ActionPoint.ActionSlot> Table
      {
        get
        {
          return this._table;
        }
      }

      public int Count
      {
        get
        {
          return this._table.Count;
        }
      }

      public ActionPoint.ActionSlot this[int idx]
      {
        get
        {
          return this._table[idx];
        }
        set
        {
          this._table[idx] = value;
        }
      }

      public void Initialize()
      {
        this.Distinct();
      }

      private void Distinct()
      {
        List<ActionPoint.ActionSlot> toRelease1 = ListPool<ActionPoint.ActionSlot>.Get();
        foreach (ActionPoint.ActionSlot actionSlot1 in this._table)
        {
          bool flag = true;
          foreach (ActionPoint.ActionSlot actionSlot2 in toRelease1)
          {
            if (Object.op_Equality((Object) actionSlot2.Point, (Object) actionSlot1.Point))
            {
              flag = false;
              break;
            }
          }
          if (flag)
            toRelease1.Add(actionSlot1);
        }
        List<ActionPoint.ActionSlot> toRelease2 = ListPool<ActionPoint.ActionSlot>.Get();
        foreach (ActionPoint.ActionSlot actionSlot in this._table)
        {
          if (!toRelease1.Contains(actionSlot))
            toRelease2.Add(actionSlot);
        }
        foreach (ActionPoint.ActionSlot actionSlot in toRelease2)
          this._table.Remove(actionSlot);
        ListPool<ActionPoint.ActionSlot>.Release(toRelease2);
        ListPool<ActionPoint.ActionSlot>.Release(toRelease1);
      }

      public ActionPoint.ActionSlotTable.Enumerator GetEnumerator()
      {
        return new ActionPoint.ActionSlotTable.Enumerator(this);
      }

      IEnumerator<ActionPoint.ActionSlot> IEnumerable<ActionPoint.ActionSlot>.GetEnumerator()
      {
        return (IEnumerator<ActionPoint.ActionSlot>) new ActionPoint.ActionSlotTable.Enumerator(this);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) new ActionPoint.ActionSlotTable.Enumerator(this);
      }

      public struct Enumerator : IEnumerator<ActionPoint.ActionSlot>, IDisposable, IEnumerator
      {
        private List<ActionPoint.ActionSlot> _list;
        private int _index;
        private ActionPoint.ActionSlot _current;

        public Enumerator(List<ActionPoint.ActionSlot> list)
        {
          this._list = list;
          this._index = 0;
          this._current = (ActionPoint.ActionSlot) null;
        }

        public Enumerator(ActionPoint.ActionSlotTable table)
        {
          this._list = table._table;
          this._index = 0;
          this._current = (ActionPoint.ActionSlot) null;
        }

        public ActionPoint.ActionSlot Current
        {
          get
          {
            return this._current;
          }
        }

        object IEnumerator.Current
        {
          get
          {
            if (this._index == 0 || this._index == this._list.Count + 1)
              throw new InvalidOperationException();
            return (object) this._current;
          }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
          if (this._index >= this._list.Count)
            return this.MoveNextRare();
          this._current = this._list[this._index];
          ++this._index;
          return true;
        }

        private bool MoveNextRare()
        {
          this._index = this._list.Count + 1;
          this._current = (ActionPoint.ActionSlot) null;
          return false;
        }

        void IEnumerator.Reset()
        {
          this._index = 0;
          this._current = (ActionPoint.ActionSlot) null;
        }
      }
    }

    [Serializable]
    public class ActionSlot
    {
      [SerializeField]
      private EventType _acceptionKey;
      [SerializeField]
      private Transform _point;
      [SerializeField]
      private Transform _recoveryPoint;
      [SerializeField]
      private Actor _actor;

      public EventType AcceptionKey
      {
        get
        {
          return this._acceptionKey;
        }
      }

      public Transform Point
      {
        get
        {
          return this._point;
        }
        set
        {
          this._point = value;
        }
      }

      public Transform RecoveryPoint
      {
        get
        {
          return this._recoveryPoint;
        }
        set
        {
          this._recoveryPoint = value;
        }
      }

      public Actor Actor
      {
        get
        {
          return this._actor;
        }
        set
        {
          this._actor = value;
        }
      }
    }
  }
}
