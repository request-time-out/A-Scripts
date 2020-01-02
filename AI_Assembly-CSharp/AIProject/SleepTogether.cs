// Decompiled with JetBrains decompiler
// Type: AIProject.SleepTogether
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class SleepTogether : AgentAction
  {
    private Subject<Unit> _endAction = new Subject<Unit>();
    private bool _missing;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      if (Object.op_Equality((Object) this.Agent.CurrentPoint, (Object) null))
        this.Agent.CurrentPoint = this.Agent.TargetInSightActionPoint;
      this._missing = Object.op_Equality((Object) this.Agent.CurrentPoint, (Object) null);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._missing)
      {
        this.Complete();
        return (TaskStatus) 2;
      }
      ActorAnimation animation = this.Agent.Animation;
      if (animation.PlayingActAnimation)
        return (TaskStatus) 3;
      if (this.Agent.Schedule.enabled)
        return (TaskStatus) 3;
      if (this._endAction != null)
        this._endAction.OnNext(Unit.get_Default());
      if (animation.PlayingOutAnimation)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      int desireKey = Desire.GetDesireKey(Desire.Type.Sleep);
      agent.SetDesire(desireKey, 0.0f);
      agent.SetStatus(0, 50f);
      agent.HealSickBySleep();
      this.Agent.ActivateNavMeshAgent();
      this.Agent.SetActiveOnEquipedItem(true);
      agent.Animation.EndStates();
      agent.ClearItems();
      agent.ClearParticles();
      agent.SetDefaultStateHousingItem();
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }
  }
}
