// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform.GetAngleToTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
  [TaskCategory("Unity/Transform")]
  [TaskDescription("Gets the Angle between a GameObject's forward direction and a target. Returns Success.")]
  public class GetAngleToTarget : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The target object to measure the angle to. If null the targetPosition will be used.")]
    public SharedGameObject targetObject;
    [Tooltip("The world position to measure an angle to. If the targetObject is also not null, this value is used as an offset from that object's position.")]
    public SharedVector3 targetPosition;
    [Tooltip("Ignore height differences when calculating the angle?")]
    public SharedBool ignoreHeight;
    [Tooltip("The angle to the target")]
    [RequiredField]
    public SharedFloat storeValue;
    private Transform targetTransform;
    private GameObject prevGameObject;

    public GetAngleToTarget()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.targetTransform = (Transform) defaultGameObject.GetComponent<Transform>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.targetTransform, (Object) null))
      {
        Debug.LogWarning((object) "Transform is null");
        return (TaskStatus) 1;
      }
      Vector3 vector3 = !Object.op_Inequality((Object) this.targetObject.get_Value(), (Object) null) ? this.targetPosition.get_Value() : this.targetObject.get_Value().get_transform().InverseTransformPoint(this.targetPosition.get_Value());
      if (this.ignoreHeight.get_Value())
        vector3.y = this.targetTransform.get_position().y;
      this.storeValue.set_Value(Vector3.Angle(Vector3.op_Subtraction(vector3, this.targetTransform.get_position()), this.targetTransform.get_forward()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.targetObject = (SharedGameObject) null;
      this.targetPosition = (SharedVector3) Vector3.get_zero();
      this.ignoreHeight = (SharedBool) true;
      this.storeValue = (SharedFloat) 0.0f;
    }
  }
}
