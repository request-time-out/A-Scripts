// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.LeaderFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Follow the leader using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=14")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}LeaderFollowIcon.png")]
  public class LeaderFollow : NavMeshGroupMovement
  {
    [Tooltip("Agents less than this distance apart are neighbors")]
    public SharedFloat neighborDistance = (SharedFloat) 10f;
    [Tooltip("How far behind the leader the agents should follow the leader")]
    public SharedFloat leaderBehindDistance = (SharedFloat) 2f;
    [Tooltip("The distance that the agents should be separated")]
    public SharedFloat separationDistance = (SharedFloat) 2f;
    [Tooltip("The agent is getting too close to the front of the leader if they are within the aheadDistance")]
    public SharedFloat aheadDistance = (SharedFloat) 2f;
    [Tooltip("The leader to follow")]
    public SharedGameObject leader;
    private Transform leaderTransform;
    private NavMeshAgent leaderAgent;

    public override void OnStart()
    {
      this.leaderTransform = this.leader.get_Value().get_transform();
      this.leaderAgent = (NavMeshAgent) this.leader.get_Value().GetComponent<NavMeshAgent>();
      base.OnStart();
    }

    public virtual TaskStatus OnUpdate()
    {
      Vector3 vector3_1 = this.LeaderBehindPosition();
      for (int index1 = 0; index1 < this.agents.Length; ++index1)
      {
        if (this.LeaderLookingAtAgent(index1) && (double) Vector3.Magnitude(Vector3.op_Subtraction(this.leaderTransform.get_position(), this.transforms[index1].get_position())) < (double) this.aheadDistance.get_Value())
        {
          int index2 = index1;
          Vector3 position = this.transforms[index1].get_position();
          Vector3 vector3_2 = Vector3.op_Subtraction(this.transforms[index1].get_position(), this.leaderTransform.get_position());
          Vector3 vector3_3 = Vector3.op_Multiply(((Vector3) ref vector3_2).get_normalized(), this.aheadDistance.get_Value());
          Vector3 target = Vector3.op_Addition(position, vector3_3);
          this.SetDestination(index2, target);
        }
        else
          this.SetDestination(index1, Vector3.op_Addition(vector3_1, this.DetermineSeparation(index1)));
      }
      return (TaskStatus) 3;
    }

    private Vector3 LeaderBehindPosition()
    {
      Vector3 position = this.leaderTransform.get_position();
      Vector3 vector3_1 = Vector3.op_UnaryNegation(this.leaderAgent.get_velocity());
      Vector3 vector3_2 = Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), this.leaderBehindDistance.get_Value());
      return Vector3.op_Addition(position, vector3_2);
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

    public bool LeaderLookingAtAgent(int agentIndex)
    {
      return (double) Vector3.Dot(this.leaderTransform.get_forward(), this.transforms[agentIndex].get_forward()) < -0.5;
    }

    public override void OnReset()
    {
      base.OnReset();
      this.neighborDistance = (SharedFloat) 10f;
      this.leaderBehindDistance = (SharedFloat) 2f;
      this.separationDistance = (SharedFloat) 2f;
      this.aheadDistance = (SharedFloat) 2f;
      this.leader = (SharedGameObject) null;
    }
  }
}
