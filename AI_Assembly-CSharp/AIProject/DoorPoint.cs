// Decompiled with JetBrains decompiler
// Type: AIProject.DoorPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using IllusionUtility.GetUtility;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  public class DoorPoint : ActionPoint
  {
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private DoorPoint.OpenTypeState _openType;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private NavMeshObstacle _obstacleInOpenRight;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private NavMeshObstacle _obstacleInOpenLeft;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private NavMeshObstacle _obstacleInClose;
    [SerializeField]
    private DoorPoint _doorPointFragment;
    private OffMeshLink _offMeshLink;
    private CommandLabel.CommandInfo[] _openingLabels;
    private CommandLabel.CommandInfo[] _closingLabels;
    private CommandLabel.CommandInfo[] _sickOpeningLabels;
    private CommandLabel.CommandInfo[] _sickClosingLabels;

    public DoorPoint.OpenTypeState OpenType
    {
      get
      {
        return this._openType;
      }
    }

    public NavMeshObstacle ObstacleInOpenRight
    {
      get
      {
        return this._obstacleInOpenRight;
      }
      set
      {
        this._obstacleInOpenRight = value;
      }
    }

    public NavMeshObstacle ObstacleInOpenLeft
    {
      get
      {
        return this._obstacleInOpenLeft;
      }
      set
      {
        this._obstacleInOpenLeft = value;
      }
    }

    public NavMeshObstacle ObstacleInClose
    {
      get
      {
        return this._obstacleInClose;
      }
      set
      {
        this._obstacleInClose = value;
      }
    }

    public DoorAnimation DoorAnimation { get; private set; }

    public void SetOpenState(DoorPoint.OpenPattern pattern, bool isSelf)
    {
      if (this.OpenState == pattern)
        return;
      this.OpenState = pattern;
      if (!isSelf)
        return;
      if (Object.op_Inequality((Object) this._doorPointFragment, (Object) null))
        this._doorPointFragment.SetOpenState(pattern, isSelf);
      if (Object.op_Inequality((Object) this._offMeshLink, (Object) null))
        this._offMeshLink.set_activated(pattern == DoorPoint.OpenPattern.Close);
      if (Object.op_Inequality((Object) this._obstacleInOpenRight, (Object) null))
        ((Component) this._obstacleInOpenRight).get_gameObject().SetActive(pattern == DoorPoint.OpenPattern.OpenRight);
      if (Object.op_Inequality((Object) this._obstacleInOpenLeft, (Object) null))
        ((Component) this._obstacleInOpenLeft).get_gameObject().SetActive(pattern == DoorPoint.OpenPattern.OpenLeft);
      if (!Object.op_Inequality((Object) this._obstacleInClose, (Object) null))
        return;
      ((Component) this._obstacleInClose).get_gameObject().SetActive(pattern == DoorPoint.OpenPattern.Close);
    }

    public DoorPoint.OpenPattern OpenState { get; private set; }

    public bool IsOpen
    {
      get
      {
        return this.OpenState != DoorPoint.OpenPattern.Close;
      }
    }

    public override CommandLabel.CommandInfo[] Labels
    {
      get
      {
        if (Singleton<Manager.Map>.Instance.Player.PlayerController.State is Onbu)
        {
          switch (this.OpenState)
          {
            case DoorPoint.OpenPattern.OpenRight:
            case DoorPoint.OpenPattern.OpenLeft:
              return this._sickClosingLabels;
            default:
              return this._sickOpeningLabels;
          }
        }
        else
        {
          switch (this.OpenState)
          {
            case DoorPoint.OpenPattern.OpenRight:
            case DoorPoint.OpenPattern.OpenLeft:
              return this._closingLabels;
            default:
              return this._openingLabels;
          }
        }
      }
    }

    public override CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        switch (this.OpenState)
        {
          case DoorPoint.OpenPattern.OpenRight:
          case DoorPoint.OpenPattern.OpenLeft:
            return this._closingLabels;
          default:
            return this._openingLabels;
        }
      }
    }

    public override bool IsNeutralCommand
    {
      get
      {
        return base.IsNeutralCommand && ((Behaviour) this).get_isActiveAndEnabled();
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
      Vector3 position1 = ((Component) this.DoorAnimation.Animator).get_transform().get_position();
      position1.y = (__Null) 0.0;
      Vector3 vector3 = basePosition;
      vector3.y = (__Null) 0.0;
      Quaternion quaternion1 = Quaternion.LookRotation(Vector3.op_Subtraction(position1, vector3));
      Vector3 eulerAngles = ((Quaternion) ref quaternion1).get_eulerAngles();
      Vector3 position2 = ((Component) this).get_transform().get_position();
      position2.y = (__Null) 0.0;
      Quaternion quaternion2 = Quaternion.LookRotation(Vector3.op_Subtraction(position1, position2));
      if ((double) Mathf.Abs((float) (((Quaternion) ref quaternion2).get_eulerAngles().y - eulerAngles.y)) > 90.0)
        return false;
      if ((this._openType == DoorPoint.OpenTypeState.Right || this._openType == DoorPoint.OpenTypeState.Right90) && this.OpenState == DoorPoint.OpenPattern.OpenLeft || (this._openType == DoorPoint.OpenTypeState.Left || this._openType == DoorPoint.OpenTypeState.Left90) && this.OpenState == DoorPoint.OpenPattern.OpenRight)
      {
        if ((double) distance < (double) Singleton<Resources>.Instance.LocomotionProfile.MinDistanceDoor || (double) distance > (double) Singleton<Resources>.Instance.LocomotionProfile.MaxDistanceDoor)
          return false;
      }
      else if ((double) distance > (double) radiusB)
        return false;
      return true;
    }

    public override bool SetImpossible(bool value, Actor actor)
    {
      if (this.IsImpossible == value || !value && Object.op_Inequality((Object) this._actionSlot.Actor, (Object) actor))
        return false;
      this.IsImpossible = value;
      if (value)
        this.SetSlot(actor);
      else
        this.ReleaseSlot(actor);
      return true;
    }

    protected override void InitSub()
    {
      this._offMeshLink = (OffMeshLink) ((Component) this).GetComponent<OffMeshLink>();
      this.DoorAnimation = (DoorAnimation) ((Component) this).GetComponent<DoorAnimation>();
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      EventType playerEventMask = Singleton<Resources>.Instance.DefinePack.MapDefines.PlayerEventMask;
      List<CommandLabel.CommandInfo> toRelease1 = ListPool<CommandLabel.CommandInfo>.Get();
      List<CommandLabel.CommandInfo> toRelease2 = ListPool<CommandLabel.CommandInfo>.Get();
      foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.LabelTable)
      {
        KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
        DoorPoint doorPoint = this;
        if (this._playerEventType.Contains(pair.Key) && playerEventMask.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
        {
          ActionPointInfo actionPointInfo = this._playerInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == pair.Key));
          string actionName = actionPointInfo.actionName;
          Sprite sprite;
          Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
          Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
          if (pair.Key == EventType.DoorOpen)
          {
            toRelease1.Add(new CommandLabel.CommandInfo()
            {
              Text = actionName,
              Icon = sprite,
              IsHold = false,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = transform,
              Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) doorPoint))
            });
            if (actionPointInfo.doorOpenType > -1)
              this._openType = (DoorPoint.OpenTypeState) actionPointInfo.doorOpenType;
          }
          else if (pair.Key == EventType.DoorClose)
          {
            toRelease2.Add(new CommandLabel.CommandInfo()
            {
              Text = actionName,
              Icon = sprite,
              IsHold = false,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = transform,
              Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) doorPoint))
            });
            if (actionPointInfo.doorOpenType > -1)
              this._openType = (DoorPoint.OpenTypeState) actionPointInfo.doorOpenType;
          }
        }
      }
      this._openingLabels = toRelease1.ToArray();
      this._closingLabels = toRelease2.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease1);
      ListPool<CommandLabel.CommandInfo>.Release(toRelease2);
      List<CommandLabel.CommandInfo> toRelease3 = ListPool<CommandLabel.CommandInfo>.Get();
      List<CommandLabel.CommandInfo> toRelease4 = ListPool<CommandLabel.CommandInfo>.Get();
      foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.SickLabelTable)
      {
        KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
        DoorPoint doorPoint = this;
        if (this._playerEventType.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
        {
          ActionPointInfo actionPointInfo = this._playerInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == pair.Key));
          string actionName = actionPointInfo.actionName;
          Sprite sprite;
          Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
          Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
          if (pair.Key == EventType.DoorOpen)
          {
            toRelease3.Add(new CommandLabel.CommandInfo()
            {
              Text = actionName,
              Icon = sprite,
              IsHold = false,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = transform,
              Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) doorPoint))
            });
            if (actionPointInfo.doorOpenType > -1)
              this._openType = (DoorPoint.OpenTypeState) actionPointInfo.doorOpenType;
          }
          else if (pair.Key == EventType.DoorClose)
          {
            toRelease4.Add(new CommandLabel.CommandInfo()
            {
              Text = actionName,
              Icon = sprite,
              IsHold = false,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = transform,
              Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) doorPoint))
            });
            if (actionPointInfo.doorOpenType > -1)
              this._openType = (DoorPoint.OpenTypeState) actionPointInfo.doorOpenType;
          }
        }
      }
      this._sickOpeningLabels = toRelease3.ToArray();
      this._sickClosingLabels = toRelease4.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease3);
      ListPool<CommandLabel.CommandInfo>.Release(toRelease4);
    }

    public enum OpenTypeState
    {
      Right,
      Left,
      Right90,
      Left90,
    }

    public enum OpenPattern
    {
      Close,
      OpenRight,
      OpenLeft,
    }
  }
}
