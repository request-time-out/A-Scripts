// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody.SetCenterOfMass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody
{
  [TaskCategory("Unity/Rigidbody")]
  [TaskDescription("Sets the center of mass of the Rigidbody. Returns Success.")]
  public class SetCenterOfMass : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The center of mass of the Rigidbody")]
    public SharedVector3 centerOfMass;
    private Rigidbody rigidbody;
    private GameObject prevGameObject;

    public SetCenterOfMass()
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
      if (Object.op_Equality((Object) this.rigidbody, (Object) null))
      {
        Debug.LogWarning((object) "Rigidbody is null");
        return (TaskStatus) 1;
      }
      this.rigidbody.set_centerOfMass(this.centerOfMass.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.centerOfMass = (SharedVector3) Vector3.get_zero();
    }
  }
}
