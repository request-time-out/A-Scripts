// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnitySphereCollider.GetCenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnitySphereCollider
{
  [TaskCategory("Unity/SphereCollider")]
  [TaskDescription("Stores the center of the SphereCollider. Returns Success.")]
  public class GetCenter : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The center of the SphereCollider")]
    [RequiredField]
    public SharedVector3 storeValue;
    private SphereCollider sphereCollider;
    private GameObject prevGameObject;

    public GetCenter()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.sphereCollider = (SphereCollider) defaultGameObject.GetComponent<SphereCollider>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.sphereCollider, (Object) null))
      {
        Debug.LogWarning((object) "SphereCollider is null");
        return (TaskStatus) 1;
      }
      this.storeValue.set_Value(this.sphereCollider.get_center());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.storeValue = (SharedVector3) Vector3.get_zero();
    }
  }
}
