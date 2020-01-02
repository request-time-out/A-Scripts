// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Flee
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Flee from the target specified using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=4")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FleeIcon.png")]
  public class Flee : NavMeshMovement
  {
    [Tooltip("The agent has fleed when the magnitude is greater than this value")]
    public SharedFloat fleedDistance = (SharedFloat) 20f;
    [Tooltip("The distance to look ahead when fleeing")]
    public SharedFloat lookAheadDistance = (SharedFloat) 5f;
    [Tooltip("The GameObject that the agent is fleeing from")]
    public SharedGameObject target;
    private bool hasMoved;

    public override void OnStart()
    {
      base.OnStart();
      this.hasMoved = false;
      this.SetDestination(this.Target());
    }

    public virtual TaskStatus OnUpdate()
    {
      if ((double) Vector3.Magnitude(Vector3.op_Subtraction(((Transform) ((Task) this).transform).get_position(), this.target.get_Value().get_transform().get_position())) > (double) this.fleedDistance.get_Value())
        return (TaskStatus) 2;
      if (this.HasArrived())
      {
        if (!this.hasMoved)
          return (TaskStatus) 1;
        if (!this.SetDestination(this.Target()))
          return (TaskStatus) 1;
        this.hasMoved = false;
      }
      else
      {
        Vector3 vector3 = this.Velocity();
        float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
        if (this.hasMoved && (double) sqrMagnitude <= 0.0)
          return (TaskStatus) 1;
        this.hasMoved = (double) sqrMagnitude > 0.0;
      }
      return (TaskStatus) 3;
    }

    private Vector3 Target()
    {
      Vector3 position = ((Transform) ((Task) this).transform).get_position();
      Vector3 vector3_1 = Vector3.op_Subtraction(((Transform) ((Task) this).transform).get_position(), this.target.get_Value().get_transform().get_position());
      Vector3 vector3_2 = Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), this.lookAheadDistance.get_Value());
      return Vector3.op_Addition(position, vector3_2);
    }

    protected override bool SetDestination(Vector3 destination)
    {
      return this.SamplePosition(destination) && base.SetDestination(destination);
    }

    public override void OnReset()
    {
      base.OnReset();
      this.fleedDistance = (SharedFloat) 20f;
      this.lookAheadDistance = (SharedFloat) 5f;
      this.target = (SharedGameObject) null;
    }
  }
}
