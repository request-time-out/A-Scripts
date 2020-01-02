// Decompiled with JetBrains decompiler
// Type: AIProject.EnterActorInSight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class EnterActorInSight : AgentConditional
  {
    [SerializeField]
    private EnterActorInSight.ActorType _targetType;

    public virtual TaskStatus OnUpdate()
    {
      switch (this._targetType)
      {
        case EnterActorInSight.ActorType.Player:
          this.CheckPlayer();
          break;
        case EnterActorInSight.ActorType.Agent:
          this.CheckAgent();
          break;
        case EnterActorInSight.ActorType.Merchant:
          this.CheckMerchant();
          break;
      }
      if (Object.op_Inequality((Object) this.Agent.TargetInSightActor, (Object) null))
      {
        switch (this._targetType)
        {
          case EnterActorInSight.ActorType.Player:
            if (this.Agent.TargetInSightActor is PlayerActor)
              return (TaskStatus) 2;
            break;
          case EnterActorInSight.ActorType.Agent:
            if (this.Agent.TargetInSightActor is AgentActor)
              return (TaskStatus) 2;
            break;
          case EnterActorInSight.ActorType.Merchant:
            if (this.Agent.TargetInSightActor is MerchantActor)
              return (TaskStatus) 2;
            break;
          default:
            return (TaskStatus) 1;
        }
      }
      return (TaskStatus) 1;
    }

    private void CheckPlayer()
    {
      List<Actor> toRelease = ListPool<Actor>.Get();
      foreach (Actor targetActor in this.Agent.TargetActors)
      {
        CollisionState collisionState;
        if (targetActor is PlayerActor && this.Agent.ActorCollisionStateTable.TryGetValue(targetActor.InstanceID, out collisionState) && collisionState == CollisionState.Enter)
          toRelease.Add(targetActor);
      }
      if (toRelease.Count > 0)
      {
        List<Actor> actorList = ListPool<Actor>.Get();
        foreach (Actor actor in toRelease)
          actorList.Add(actor);
        ActorController capturedInSight = AgentActor.GetCapturedInSight(this.Agent, actorList);
        ListPool<Actor>.Release(actorList);
        if (Object.op_Equality((Object) capturedInSight, (Object) null) || !(capturedInSight.Actor is PlayerActor))
        {
          ListPool<Actor>.Release(toRelease);
          return;
        }
        if (!capturedInSight.Actor.IsNeutralCommand)
          return;
        this.Agent.TargetInSightActor = capturedInSight.Actor;
      }
      ListPool<Actor>.Release(toRelease);
    }

    private void CheckAgent()
    {
      List<Desire.ActionType> encounterWhitelist = Singleton<Resources>.Instance.AgentProfile.EncounterWhitelist;
      List<Actor> toRelease = ListPool<Actor>.Get();
      foreach (Actor targetActor in this.Agent.TargetActors)
      {
        CollisionState collisionState;
        if (targetActor is AgentActor && this.Agent.ActorCollisionStateTable.TryGetValue(targetActor.InstanceID, out collisionState) && ((targetActor as AgentActor).IsEncounterable && collisionState == CollisionState.Enter))
          toRelease.Add(targetActor);
      }
      if (toRelease.Count > 0)
      {
        List<Actor> actorList = ListPool<Actor>.Get();
        foreach (Actor actor in toRelease)
          actorList.Add(actor);
        ActorController capturedInSight = AgentActor.GetCapturedInSight(this.Agent, actorList);
        ListPool<Actor>.Release(actorList);
        if (Object.op_Equality((Object) capturedInSight, (Object) null) || !(capturedInSight.Actor is AgentActor))
        {
          ListPool<Actor>.Release(toRelease);
          return;
        }
        AgentActor actor1 = capturedInSight.Actor as AgentActor;
        if (!capturedInSight.Actor.IsNeutralCommand || Object.op_Inequality((Object) actor1.CommandPartner, (Object) null) || (Object.op_Inequality((Object) capturedInSight.Actor.Partner, (Object) null) || !encounterWhitelist.Contains(capturedInSight.Actor.Mode)) || !encounterWhitelist.Contains(actor1.BehaviorResources.Mode))
          return;
        Debug.Log((object) "何者かを発見");
        this.Agent.TargetInSightActor = capturedInSight.Actor;
      }
      ListPool<Actor>.Release(toRelease);
    }

    private void CheckMerchant()
    {
      List<Actor> actorList1 = ListPool<Actor>.Get();
      foreach (Actor targetActor in this.Agent.TargetActors)
      {
        MerchantActor merchantActor = targetActor as MerchantActor;
        CollisionState collisionState;
        if (!Object.op_Equality((Object) merchantActor, (Object) null) && this.Agent.ActorCollisionStateTable.TryGetValue(targetActor.InstanceID, out collisionState) && (collisionState == CollisionState.Enter && merchantActor.IsNeutralCommand))
          actorList1.Add(targetActor);
      }
      if (actorList1.IsNullOrEmpty<Actor>())
      {
        ListPool<Actor>.Release(actorList1);
      }
      else
      {
        List<Actor> actorList2 = ListPool<Actor>.Get();
        foreach (Actor actor in actorList1)
          actorList2.Add(actor);
        ActorController capturedInSight = AgentActor.GetCapturedInSight(this.Agent, actorList2);
        ListPool<Actor>.Release(actorList2);
        if (Object.op_Equality((Object) capturedInSight, (Object) null) || !(capturedInSight.Actor is MerchantActor))
        {
          ListPool<Actor>.Release(actorList1);
        }
        else
        {
          this.Agent.TargetInSightActor = capturedInSight.Actor;
          ListPool<Actor>.Release(actorList1);
        }
      }
    }

    private enum ActorType
    {
      None,
      Player,
      Agent,
      Merchant,
    }
  }
}
