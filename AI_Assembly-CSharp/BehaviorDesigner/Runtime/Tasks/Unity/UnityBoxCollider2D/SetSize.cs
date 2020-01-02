// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider2D.SetSize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider2D
{
  [TaskCategory("Unity/BoxCollider2D")]
  [TaskDescription("Sets the size of the BoxCollider2D. Returns Success.")]
  public class SetSize : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The size of the BoxCollider2D")]
    public SharedVector2 size;
    private BoxCollider2D boxCollider2D;
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
      this.boxCollider2D = (BoxCollider2D) defaultGameObject.GetComponent<BoxCollider2D>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.boxCollider2D, (Object) null))
      {
        Debug.LogWarning((object) "BoxCollider2D is null");
        return (TaskStatus) 1;
      }
      this.boxCollider2D.set_size(this.size.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.size = (SharedVector2) Vector2.get_zero();
    }
  }
}
