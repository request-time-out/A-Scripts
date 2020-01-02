// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform.RotateAround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
  [TaskCategory("Unity/Transform")]
  [TaskDescription("Applies a rotation. Returns Success.")]
  public class RotateAround : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("Point to rotate around")]
    public SharedVector3 point;
    [Tooltip("Axis to rotate around")]
    public SharedVector3 axis;
    [Tooltip("Amount to rotate")]
    public SharedFloat angle;
    private Transform targetTransform;
    private GameObject prevGameObject;

    public RotateAround()
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
      this.targetTransform.RotateAround(this.point.get_Value(), this.axis.get_Value(), this.angle.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.point = (SharedVector3) Vector3.get_zero();
      this.axis = (SharedVector3) Vector3.get_zero();
      this.angle = (SharedFloat) 0.0f;
    }
  }
}
