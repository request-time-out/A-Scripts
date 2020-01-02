// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.NavMeshGroupMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  public abstract class NavMeshGroupMovement : GroupMovement
  {
    [Tooltip("The speed of the agents")]
    public SharedFloat speed = (SharedFloat) 10f;
    [Tooltip("The angular speed of the agents")]
    public SharedFloat angularSpeed = (SharedFloat) 120f;
    [Tooltip("All of the agents")]
    public SharedGameObject[] agents;
    private NavMeshAgent[] navMeshAgents;
    protected Transform[] transforms;

    public virtual void OnStart()
    {
      this.navMeshAgents = new NavMeshAgent[this.agents.Length];
      this.transforms = new Transform[this.agents.Length];
      for (int index = 0; index < this.agents.Length; ++index)
      {
        this.transforms[index] = this.agents[index].get_Value().get_transform();
        this.navMeshAgents[index] = (NavMeshAgent) this.agents[index].get_Value().GetComponent<NavMeshAgent>();
        this.navMeshAgents[index].set_speed(this.speed.get_Value());
        this.navMeshAgents[index].set_angularSpeed(this.angularSpeed.get_Value());
        this.navMeshAgents[index].set_isStopped(false);
      }
    }

    protected override bool SetDestination(int index, Vector3 target)
    {
      return Vector3.op_Equality(this.navMeshAgents[index].get_destination(), target) || this.navMeshAgents[index].SetDestination(target);
    }

    protected override Vector3 Velocity(int index)
    {
      return this.navMeshAgents[index].get_velocity();
    }

    public virtual void OnEnd()
    {
      for (int index = 0; index < this.navMeshAgents.Length; ++index)
      {
        if (Object.op_Inequality((Object) this.navMeshAgents[index], (Object) null))
          this.navMeshAgents[index].set_isStopped(true);
      }
    }

    public virtual void OnReset()
    {
      this.agents = (SharedGameObject[]) null;
    }
  }
}
