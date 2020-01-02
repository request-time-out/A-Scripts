// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Evade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Evade the target specified using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=6")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}EvadeIcon.png")]
  public class Evade : NavMeshMovement
  {
    [Tooltip("The agent has evaded when the magnitude is greater than this value")]
    public SharedFloat evadeDistance = (SharedFloat) 10f;
    [Tooltip("The distance to look ahead when evading")]
    public SharedFloat lookAheadDistance = (SharedFloat) 5f;
    [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
    public SharedFloat targetDistPrediction = (SharedFloat) 20f;
    [Tooltip("Multiplier for predicting the look ahead distance")]
    public SharedFloat targetDistPredictionMult = (SharedFloat) 20f;
    [Tooltip("The GameObject that the agent is evading")]
    public SharedGameObject target;
    private Vector3 targetPosition;

    public override void OnStart()
    {
      base.OnStart();
      this.targetPosition = this.target.get_Value().get_transform().get_position();
      this.SetDestination(this.Target());
    }

    public virtual TaskStatus OnUpdate()
    {
      if ((double) Vector3.Magnitude(Vector3.op_Subtraction(((Transform) ((Task) this).transform).get_position(), this.target.get_Value().get_transform().get_position())) > (double) this.evadeDistance.get_Value())
        return (TaskStatus) 2;
      this.SetDestination(this.Target());
      return (TaskStatus) 3;
    }

    private Vector3 Target()
    {
      Vector3 vector3_1 = Vector3.op_Subtraction(this.target.get_Value().get_transform().get_position(), ((Transform) ((Task) this).transform).get_position());
      float magnitude1 = ((Vector3) ref vector3_1).get_magnitude();
      Vector3 vector3_2 = this.Velocity();
      float magnitude2 = ((Vector3) ref vector3_2).get_magnitude();
      float num = (double) magnitude2 > (double) magnitude1 / (double) this.targetDistPrediction.get_Value() ? magnitude1 / magnitude2 * this.targetDistPredictionMult.get_Value() : this.targetDistPrediction.get_Value();
      Vector3 targetPosition = this.targetPosition;
      this.targetPosition = this.target.get_Value().get_transform().get_position();
      Vector3 vector3_3 = Vector3.op_Addition(this.targetPosition, Vector3.op_Multiply(Vector3.op_Subtraction(this.targetPosition, targetPosition), num));
      Vector3 position = ((Transform) ((Task) this).transform).get_position();
      Vector3 vector3_4 = Vector3.op_Subtraction(((Transform) ((Task) this).transform).get_position(), vector3_3);
      Vector3 vector3_5 = Vector3.op_Multiply(((Vector3) ref vector3_4).get_normalized(), this.lookAheadDistance.get_Value());
      return Vector3.op_Addition(position, vector3_5);
    }

    public override void OnReset()
    {
      base.OnReset();
      this.evadeDistance = (SharedFloat) 10f;
      this.lookAheadDistance = (SharedFloat) 5f;
      this.targetDistPrediction = (SharedFloat) 20f;
      this.targetDistPredictionMult = (SharedFloat) 20f;
      this.target = (SharedGameObject) null;
    }
  }
}
