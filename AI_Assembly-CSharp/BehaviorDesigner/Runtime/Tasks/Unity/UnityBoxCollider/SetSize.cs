// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider.SetSize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider
{
  [TaskCategory("Unity/BoxCollider")]
  [TaskDescription("Sets the size of the BoxCollider. Returns Success.")]
  public class SetSize : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The size of the BoxCollider")]
    public SharedVector3 size;
    private BoxCollider boxCollider;
    private GameObject prevGameObject;

    public SetSize()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.boxCollider = (BoxCollider) defaultGameObject.GetComponent<BoxCollider>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.boxCollider, (Object) null))
      {
        Debug.LogWarning((object) "BoxCollider is null");
        return (TaskStatus) 1;
      }
      this.boxCollider.set_size(this.size.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.size = (SharedVector3) Vector3.get_zero();
    }
  }
}
