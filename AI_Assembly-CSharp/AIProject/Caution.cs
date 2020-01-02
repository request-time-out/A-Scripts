// Decompiled with JetBrains decompiler
// Type: AIProject.Caution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Caution : AgentAction
  {
    private float _currentTime;

    public virtual void OnStart()
    {
      this._currentTime = 0.0f;
      ((Task) this).OnStart();
    }

    public virtual TaskStatus OnUpdate()
    {
      Dictionary<int, CollisionState> collisionStateTable = this.Agent.ActorCollisionStateTable;
      List<Actor> toRelease1 = ListPool<Actor>.Get();
      foreach (Actor targetActor in this.Agent.TargetActors)
      {
        CollisionState collisionState;
        if (targetActor is PlayerActor && this.Agent.ActorCollisionStateTable.TryGetValue(targetActor.InstanceID, out collisionState) && collisionState == CollisionState.Enter)
          toRelease1.Add(targetActor);
      }
      if (toRelease1.Count > 0)
      {
        List<Actor> toRelease2 = ListPool<Actor>.Get();
        foreach (Actor actor in toRelease1)
          toRelease2.Add(actor);
        ActorController capturedInSight = this.GetCapturedInSight(this.Agent, toRelease2.ToArray());
        ListPool<Actor>.Release(toRelease2);
        if (Object.op_Equality((Object) capturedInSight, (Object) null) || !(capturedInSight.Actor is PlayerActor))
        {
          ListPool<Actor>.Release(toRelease1);
          return (TaskStatus) 1;
        }
        this.Agent.TargetInSightActor = capturedInSight.Actor;
      }
      if (Object.op_Inequality((Object) this.Agent.TargetInSightActor, (Object) null))
        return (TaskStatus) 2;
      return (double) this._currentTime > (double) Singleton<Resources>.Instance.LocomotionProfile.TimeToBeware ? (TaskStatus) 1 : (TaskStatus) 3;
    }

    private ActorController GetCapturedInSight(AgentActor agent, Actor[] actors)
    {
      Actor element = actors.GetElement<Actor>(Random.Range(0, actors.Length));
      if (Object.op_Equality((Object) element, (Object) null))
        return (ActorController) null;
      ActorController controller = element.Controller;
      if (Object.op_Equality((Object) controller, (Object) null))
        return (ActorController) null;
      Vector3 position = agent.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head).get_position();
      foreach (Transform fovTargetPoint in element.FovTargetPoints)
      {
        Vector3 vector3 = Vector3.op_Subtraction(position, fovTargetPoint.get_position());
        Ray ray;
        ((Ray) ref ray).\u002Ector(position, vector3);
        if (Physics.Raycast(ray, ((Vector3) ref vector3).get_magnitude(), LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.MapLayer)))
          return (ActorController) null;
      }
      return controller;
    }
  }
}
