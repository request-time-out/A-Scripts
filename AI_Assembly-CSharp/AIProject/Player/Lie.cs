// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Lie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class Lie : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      Resources instance = Singleton<Resources>.Instance;
      Sprite actionIcon;
      instance.itemIconTables.ActionIconTable.TryGetValue(instance.PlayerProfile.CommonActionIconID, out actionIcon);
      this.ChangeModeRelatedSleep(player);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) MapUIContainer.CommandList.OnCompletedStopAsObservable(), 1), (System.Action<M0>) (_ =>
      {
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.CancelAcception);
        MapUIContainer.CommandLabel.CancelCommand = new CommandLabel.CommandInfo()
        {
          Text = "もどる",
          Icon = actionIcon,
          TargetSpriteInfo = (CommandTargetSpriteInfo) null,
          Transform = (Transform) null,
          Event = (System.Action) (() =>
          {
            MapUIContainer.CommandLabel.CancelCommand = (CommandLabel.CommandInfo) null;
            player.Controller.ChangeState("Sleep");
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
          })
        };
      }));
    }

    private void ChangeModeRelatedSleep(PlayerActor player)
    {
      PoseKeyPair sleepTogetherRight = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SleepTogetherRight;
      PoseKeyPair sleepTogetherLeft = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SleepTogetherLeft;
      List<ActionPoint> groupActionPoints = player.CurrentPoint.GroupActionPoints;
      ActionPoint actionPoint1 = (ActionPoint) null;
      foreach (ActionPoint actionPoint2 in groupActionPoints)
      {
        if (actionPoint2.IsNeutralCommand)
        {
          actionPoint1 = actionPoint2;
          break;
        }
      }
      bool flag = false;
      ActionPointInfo outInfo = new ActionPointInfo();
      if (Object.op_Inequality((Object) actionPoint1, (Object) null))
        flag = actionPoint1.FindAgentActionPointInfo(AIProject.EventType.Sleep, sleepTogetherRight.poseID, out outInfo) || actionPoint1.FindAgentActionPointInfo(AIProject.EventType.Sleep, sleepTogetherLeft.poseID, out outInfo);
      List<AgentActor> agentActorList1 = ListPool<AgentActor>.Get();
      List<AgentActor> agentActorList2 = ListPool<AgentActor>.Get();
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (current.Value.Mode != Desire.ActionType.Cold2A && current.Value.Mode != Desire.ActionType.Cold2B && (current.Value.Mode != Desire.ActionType.Cold3A && current.Value.Mode != Desire.ActionType.Cold3B) && (current.Value.Mode != Desire.ActionType.OverworkA && current.Value.Mode != Desire.ActionType.OverworkB && (current.Value.Mode != Desire.ActionType.Cold2BMedicated && current.Value.Mode != Desire.ActionType.Cold3BMedicated)) && (current.Value.Mode != Desire.ActionType.WeaknessA && current.Value.Mode != Desire.ActionType.WeaknessB && (current.Value.Mode != Desire.ActionType.FoundPeeping && current.Value.EventKey != AIProject.EventType.Sleep) && (current.Value.EventKey != AIProject.EventType.Toilet && current.Value.EventKey != AIProject.EventType.Bath && (current.Value.EventKey != AIProject.EventType.Move && current.Value.EventKey != AIProject.EventType.Masturbation))) && current.Value.EventKey != AIProject.EventType.Lesbian)
          {
            MapArea mapArea = current.Value.MapArea;
            int? nullable1;
            int? nullable2;
            if (mapArea == null)
            {
              nullable1 = new int?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new int?(mapArea.ChunkID);
            nullable1 = nullable2;
            int num = !nullable1.HasValue ? current.Value.AgentData.ChunkID : nullable1.Value;
            if (player.ChunkID == num)
            {
              if (Game.isAdd01 && current.Value.ChaControl.fileGameInfo.hSkill.ContainsValue(13))
                agentActorList1.Add(current.Value);
              else if (flag && current.Value.ChaControl.fileGameInfo.flavorState[1] >= Singleton<Resources>.Instance.StatusProfile.SoineReliabilityBorder && (current.Value.Mode == Desire.ActionType.Normal || current.Value.Mode == Desire.ActionType.SearchSleep || current.Value.Mode == Desire.ActionType.Encounter))
                agentActorList2.Add(current.Value);
            }
          }
        }
      }
      AgentActor element1 = agentActorList1.GetElement<AgentActor>(Random.Range(0, agentActorList1.Count));
      ListPool<AgentActor>.Release(agentActorList1);
      AgentActor element2 = agentActorList2.GetElement<AgentActor>(Random.Range(0, agentActorList2.Count));
      ListPool<AgentActor>.Release(agentActorList2);
      if (Object.op_Inequality((Object) element1, (Object) null))
      {
        element1.Animation.ResetDefaultAnimatorController();
        element1.TargetInSightActionPoint = (ActionPoint) null;
        element1.TargetInSightActor = (Actor) player;
        this.ChangeForceBehavior(element1, Desire.ActionType.ChaseYobai);
      }
      else
      {
        if (!Object.op_Inequality((Object) element2, (Object) null))
          return;
        element2.Animation.ResetDefaultAnimatorController();
        actionPoint1.Reserver = (Actor) element2;
        element2.TargetInSightActionPoint = actionPoint1;
        element2.TargetInSightActor = (Actor) null;
        this.ChangeForceBehavior(element2, Desire.ActionType.ComeSleepTogether);
      }
    }

    private void ChangeForceBehavior(AgentActor agent, Desire.ActionType actionType)
    {
      AgentActor agentActor1 = agent;
      int num1 = -1;
      agent.PoseID = num1;
      int num2 = num1;
      agentActor1.ActionID = num2;
      agent.AgentData.CarryingItem = (StuffItem) null;
      agent.StateType = AIProject.Definitions.State.Type.Normal;
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      if (Object.op_Inequality((Object) agent.CommandPartner, (Object) null))
      {
        Actor commandPartner = agent.CommandPartner;
        switch (commandPartner)
        {
          case AgentActor _:
            AgentActor agentActor2 = commandPartner as AgentActor;
            agentActor2.CommandPartner = (Actor) null;
            agentActor2.ChangeBehavior(Desire.ActionType.Normal);
            break;
          case MerchantActor _:
            MerchantActor merchantActor = commandPartner as MerchantActor;
            merchantActor.CommandPartner = (Actor) null;
            merchantActor.ChangeBehavior(merchantActor.LastNormalMode);
            break;
        }
        agent.CommandPartner = (Actor) null;
      }
      agent.EventKey = (AIProject.EventType) 0;
      agent.CommandPartner = (Actor) null;
      agent.ResetActionFlag();
      if (agent.Schedule.enabled)
      {
        Actor.BehaviorSchedule schedule = agent.Schedule;
        schedule.enabled = false;
        agent.Schedule = schedule;
      }
      agent.ClearItems();
      agent.ClearParticles();
      agent.ActivateNavMeshAgent();
      agent.ActivateTransfer(true);
      agent.Animation.ResetDefaultAnimatorController();
      agent.ChangeBehavior(actionType);
    }

    public override void Release(Actor actor, AIProject.EventType type)
    {
      this.OnRelease(actor as PlayerActor);
    }

    protected override void OnRelease(PlayerActor player)
    {
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }
  }
}
