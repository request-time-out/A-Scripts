// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Pursue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Pursue the target specified using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=5")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PursueIcon.png")]
  public class Pursue : NavMeshMovement
  {
    [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
    public SharedFloat targetDistPrediction = (SharedFloat) 20f;
    [Tooltip("Multiplier for predicting the look ahead distance")]
    public SharedFloat targetDistPredictionMult = (SharedFloat) 20f;
    [Tooltip("The GameObject that the agent is pursuing")]
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
      if (this.HasArrived())
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
      return Vector3.op_Addition(this.targetPosition, Vector3.op_Multiply(Vector3.op_Subtraction(this.targetPosition, targetPosition), num));
    }

    public override void OnReset()
    {
      base.OnReset();
      this.targetDistPrediction = (SharedFloat) 20f;
      this.targetDistPredictionMult = (SharedFloat) 20f;
      this.target = (SharedGameObject) null;
    }
  }
}
