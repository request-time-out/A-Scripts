// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Flock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Flock around the scene using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=13")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FlockIcon.png")]
  public class Flock : NavMeshGroupMovement
  {
    [Tooltip("Agents less than this distance apart are neighbors")]
    public SharedFloat neighborDistance = (SharedFloat) 100f;
    [Tooltip("How far the agent should look ahead when determine its pathfinding destination")]
    public SharedFloat lookAheadDistance = (SharedFloat) 5f;
    [Tooltip("The greater the alignmentWeight is the more likely it is that the agents will be facing the same direction")]
    public SharedFloat alignmentWeight = (SharedFloat) 0.4f;
    [Tooltip("The greater the cohesionWeight is the more likely it is that the agents will be moving towards a common position")]
    public SharedFloat cohesionWeight = (SharedFloat) 0.5f;
    [Tooltip("The greater the separationWeight is the more likely it is that the agents will be separated")]
    public SharedFloat separationWeight = (SharedFloat) 0.6f;

    public virtual TaskStatus OnUpdate()
    {
      for (int index = 0; index < this.agents.Length; ++index)
      {
        Vector3 alignment;
        Vector3 cohesion;
        Vector3 separation;
        this.DetermineFlockParameters(index, out alignment, out cohesion, out separation);
        Vector3 vector3_1 = Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(alignment, this.alignmentWeight.get_Value()), Vector3.op_Multiply(cohesion, this.cohesionWeight.get_Value())), Vector3.op_Multiply(separation, this.separationWeight.get_Value()));
        if (!this.SetDestination(index, Vector3.op_Addition(this.transforms[index].get_position(), Vector3.op_Multiply(vector3_1, this.lookAheadDistance.get_Value()))))
        {
          Vector3 vector3_2 = Vector3.op_Multiply(vector3_1, -1f);
          this.SetDestination(index, Vector3.op_Addition(this.transforms[index].get_position(), Vector3.op_Multiply(vector3_2, this.lookAheadDistance.get_Value())));
        }
      }
      return (TaskStatus) 3;
    }

    private void DetermineFlockParameters(
      int index,
      out Vector3 alignment,
      out Vector3 cohesion,
      out Vector3 separation)
    {
      alignment = cohesion = separation = Vector3.get_zero();
      int num = 0;
      Transform transform = this.transforms[index];
      for (int index1 = 0; index1 < this.agents.Length; ++index1)
      {
        if (index != index1 && (double) Vector3.Magnitude(Vector3.op_Subtraction(this.transforms[index1].get_position(), transform.get_position())) < (double) this.neighborDistance.get_Value())
        {
          alignment = Vector3.op_Addition(alignment, this.Velocity(index1));
          cohesion = Vector3.op_Addition(cohesion, this.transforms[index1].get_position());
          separation = Vector3.op_Addition(separation, Vector3.op_Subtraction(this.transforms[index1].get_position(), transform.get_position()));
          ++num;
        }
      }
      if (num == 0)
        return;
      ref Vector3 local1 = ref alignment;
      Vector3 vector3_1 = Vector3.op_Division(alignment, (float) num);
      Vector3 normalized1 = ((Vector3) ref vector3_1).get_normalized();
      local1 = normalized1;
      ref Vector3 local2 = ref cohesion;
      Vector3 vector3_2 = Vector3.op_Subtraction(Vector3.op_Division(cohesion, (float) num), transform.get_position());
      Vector3 normalized2 = ((Vector3) ref vector3_2).get_normalized();
      local2 = normalized2;
      ref Vector3 local3 = ref separation;
      Vector3 vector3_3 = Vector3.op_Multiply(Vector3.op_Division(separation, (float) num), -1f);
      Vector3 normalized3 = ((Vector3) ref vector3_3).get_normalized();
      local3 = normalized3;
    }

    public override void OnReset()
    {
      base.OnReset();
      this.neighborDistance = (SharedFloat) 100f;
      this.lookAheadDistance = (SharedFloat) 5f;
      this.alignmentWeight = (SharedFloat) 0.4f;
      this.cohesionWeight = (SharedFloat) 0.5f;
      this.separationWeight = (SharedFloat) 0.6f;
    }
  }
}
