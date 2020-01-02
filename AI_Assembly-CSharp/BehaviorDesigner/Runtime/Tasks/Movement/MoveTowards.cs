// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.MoveTowards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Move towards the specified position. The position can either be specified by a transform or position. If the transform is used then the position will not be used.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=1")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}MoveTowardsIcon.png")]
  public class MoveTowards : Action
  {
    [Tooltip("The speed of the agent")]
    public SharedFloat speed;
    [Tooltip("The agent has arrived when the magnitude is less than this value")]
    public SharedFloat arriveDistance;
    [Tooltip("Should the agent be looking at the target position?")]
    public SharedBool lookAtTarget;
    [Tooltip("Max rotation delta if lookAtTarget is enabled")]
    public SharedFloat maxLookAtRotationDelta;
    [Tooltip("The GameObject that the agent is moving towards")]
    public SharedGameObject target;
    [Tooltip("If target is null then use the target position")]
    public SharedVector3 targetPosition;

    public MoveTowards()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Vector3 vector3_1 = this.Target();
      if ((double) Vector3.Magnitude(Vector3.op_Subtraction(((Transform) ((Task) this).transform).get_position(), vector3_1)) < (double) this.arriveDistance.get_Value())
        return (TaskStatus) 2;
      ((Transform) ((Task) this).transform).set_position(Vector3.MoveTowards(((Transform) ((Task) this).transform).get_position(), vector3_1, this.speed.get_Value() * Time.get_deltaTime()));
      if (this.lookAtTarget.get_Value())
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, ((Transform) ((Task) this).transform).get_position());
        if ((double) ((Vector3) ref vector3_2).get_sqrMagnitude() > 0.00999999977648258)
          ((Transform) ((Task) this).transform).set_rotation(Quaternion.RotateTowards(((Transform) ((Task) this).transform).get_rotation(), Quaternion.LookRotation(Vector3.op_Subtraction(vector3_1, ((Transform) ((Task) this).transform).get_position())), this.maxLookAtRotationDelta.get_Value()));
      }
      return (TaskStatus) 3;
    }

    private Vector3 Target()
    {
      return this.target == null || Object.op_Equality((Object) this.target.get_Value(), (Object) null) ? this.targetPosition.get_Value() : this.target.get_Value().get_transform().get_position();
    }

    public virtual void OnReset()
    {
      this.arriveDistance = (SharedFloat) 0.1f;
      this.lookAtTarget = (SharedBool) true;
    }
  }
}
