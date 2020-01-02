// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Queue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Queue in a line using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=15")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}QueueIcon.png")]
  public class Queue : NavMeshGroupMovement
  {
    [Tooltip("Agents less than this distance apart are neighbors")]
    public SharedFloat neighborDistance = (SharedFloat) 10f;
    [Tooltip("The distance that the agents should be separated")]
    public SharedFloat separationDistance = (SharedFloat) 2f;
    [Tooltip("The distance the the agent should look ahead to see if another agent is in the way")]
    public SharedFloat maxQueueAheadDistance = (SharedFloat) 2f;
    [Tooltip("The radius that the agent should check to see if another agent is in the way")]
    public SharedFloat maxQueueRadius = (SharedFloat) 20f;
    [Tooltip("The multiplier to slow down if an agent is in front of the current agent")]
    public SharedFloat slowDownSpeed = (SharedFloat) 0.15f;
    [Tooltip("The target to seek towards")]
    public SharedGameObject target;

    public virtual TaskStatus OnUpdate()
    {
      for (int index = 0; index < this.agents.Length; ++index)
      {
        if (this.AgentAhead(index))
          this.SetDestination(index, Vector3.op_Addition(Vector3.op_Addition(this.transforms[index].get_position(), Vector3.op_Multiply(this.transforms[index].get_forward(), this.slowDownSpeed.get_Value())), this.DetermineSeparation(index)));
        else
          this.SetDestination(index, this.target.get_Value().get_transform().get_position());
      }
      return (TaskStatus) 3;
    }

    private bool AgentAhead(int index)
    {
      Vector3 vector3 = Vector3.op_Multiply(this.Velocity(index), this.maxQueueAheadDistance.get_Value());
      for (int index1 = 0; index1 < this.agents.Length; ++index1)
      {
        if (index != index1 && (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(vector3, this.transforms[index1].get_position())) < (double) this.maxQueueRadius.get_Value())
          return true;
      }
      return false;
    }

    private Vector3 DetermineSeparation(int agentIndex)
    {
      Vector3 vector3_1 = Vector3.get_zero();
      int num = 0;
      Transform transform = this.transforms[agentIndex];
      for (int index = 0; index < this.agents.Length; ++index)
      {
        if (agentIndex != index && (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(this.transforms[index].get_position(), transform.get_position())) < (double) this.neighborDistance.get_Value())
        {
          vector3_1 = Vector3.op_Addition(vector3_1, Vector3.op_Subtraction(this.transforms[index].get_position(), transform.get_position()));
          ++num;
        }
      }
      if (num == 0)
        return Vector3.get_zero();
      Vector3 vector3_2 = Vector3.op_Multiply(Vector3.op_Division(vector3_1, (float) num), -1f);
      return Vector3.op_Multiply(((Vector3) ref vector3_2).get_normalized(), this.separationDistance.get_Value());
    }

    public override void OnReset()
    {
      base.OnReset();
      this.neighborDistance = (SharedFloat) 10f;
      this.separationDistance = (SharedFloat) 2f;
      this.maxQueueAheadDistance = (SharedFloat) 2f;
      this.maxQueueRadius = (SharedFloat) 20f;
      this.slowDownSpeed = (SharedFloat) 0.15f;
    }
  }
}
