// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody.AddRelativeTorque
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody
{
  [TaskCategory("Unity/Rigidbody")]
  [TaskDescription("Applies a torque to the rigidbody relative to its coordinate system. Returns Success.")]
  public class AddRelativeTorque : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The amount of torque to apply")]
    public SharedVector3 torque;
    [Tooltip("The type of torque")]
    public ForceMode forceMode;
    private Rigidbody rigidbody;
    private GameObject prevGameObject;

    public AddRelativeTorque()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.rigidbody = (Rigidbody) defaultGameObject.GetComponent<Rigidbody>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      this.rigidbody.AddRelativeTorque(this.torque.get_Value(), this.forceMode);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.torque = (SharedVector3) Vector3.get_zero();
      this.forceMode = (ForceMode) 0;
    }
  }
}
