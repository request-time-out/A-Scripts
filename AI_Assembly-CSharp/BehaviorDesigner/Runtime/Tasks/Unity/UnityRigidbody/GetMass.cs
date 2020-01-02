// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody.GetMass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody
{
  [TaskCategory("Unity/Rigidbody")]
  [TaskDescription("Stores the mass of the Rigidbody. Returns Success.")]
  public class GetMass : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The mass of the Rigidbody")]
    [RequiredField]
    public SharedFloat storeValue;
    private Rigidbody rigidbody;
    private GameObject prevGameObject;

    public GetMass()
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
      this.storeValue.set_Value(this.rigidbody.get_mass());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.storeValue = (SharedFloat) 0.0f;
    }
  }
}
