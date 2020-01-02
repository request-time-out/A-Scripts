// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.RotateTowards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Rotates towards the specified rotation. The rotation can either be specified by a transform or rotation. If the transform is used then the rotation will not be used.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=2")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}RotateTowardsIcon.png")]
  public class RotateTowards : Action
  {
    [Tooltip("Should the 2D version be used?")]
    public bool usePhysics2D;
    [Tooltip("The agent is done rotating when the angle is less than this value")]
    public SharedFloat rotationEpsilon;
    [Tooltip("The maximum number of angles the agent can rotate in a single tick")]
    public SharedFloat maxLookAtRotationDelta;
    [Tooltip("Should the rotation only affect the Y axis?")]
    public SharedBool onlyY;
    [Tooltip("The GameObject that the agent is rotating towards")]
    public SharedGameObject target;
    [Tooltip("If target is null then use the target rotation")]
    public SharedVector3 targetRotation;

    public RotateTowards()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Quaternion quaternion = this.Target();
      if ((double) Quaternion.Angle(((Transform) ((Task) this).transform).get_rotation(), quaternion) < (double) this.rotationEpsilon.get_Value())
        return (TaskStatus) 2;
      ((Transform) ((Task) this).transform).set_rotation(Quaternion.RotateTowards(((Transform) ((Task) this).transform).get_rotation(), quaternion, this.maxLookAtRotationDelta.get_Value()));
      return (TaskStatus) 3;
    }

    private Quaternion Target()
    {
      if (this.target == null || Object.op_Equality((Object) this.target.get_Value(), (Object) null))
        return Quaternion.Euler(this.targetRotation.get_Value());
      Vector3 vector3 = Vector3.op_Subtraction(this.target.get_Value().get_transform().get_position(), ((Transform) ((Task) this).transform).get_position());
      if (this.onlyY.get_Value())
        vector3.y = (__Null) 0.0;
      return this.usePhysics2D ? Quaternion.AngleAxis(Mathf.Atan2((float) vector3.y, (float) vector3.x) * 57.29578f, Vector3.get_forward()) : Quaternion.LookRotation(vector3);
    }

    public virtual void OnReset()
    {
      this.usePhysics2D = false;
      this.rotationEpsilon = (SharedFloat) 0.5f;
      this.maxLookAtRotationDelta = (SharedFloat) 1f;
      this.onlyY = (SharedBool) false;
      this.target = (SharedGameObject) null;
      this.targetRotation = (SharedVector3) Vector3.get_zero();
    }
  }
}
