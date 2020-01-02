// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D.GetAngularDrag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
  [TaskCategory("Unity/Rigidbody2D")]
  [TaskDescription("Stores the angular drag of the Rigidbody2D. Returns Success.")]
  public class GetAngularDrag : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The angular drag of the Rigidbody2D")]
    [RequiredField]
    public SharedFloat storeValue;
    private Rigidbody2D rigidbody2D;
    private GameObject prevGameObject;

    public GetAngularDrag()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.rigidbody2D = (Rigidbody2D) defaultGameObject.GetComponent<Rigidbody2D>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.rigidbody2D, (Object) null))
      {
        Debug.LogWarning((object) "Rigidbody2D is null");
        return (TaskStatus) 1;
      }
      this.storeValue.set_Value(this.rigidbody2D.get_angularDrag());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.storeValue = (SharedFloat) 0.0f;
    }
  }
}
