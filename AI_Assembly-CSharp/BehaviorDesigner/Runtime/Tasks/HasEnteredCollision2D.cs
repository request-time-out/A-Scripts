// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.HasEnteredCollision2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Returns success when a 2D collision starts. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
  [TaskCategory("Physics")]
  public class HasEnteredCollision2D : Conditional
  {
    [Tooltip("The tag of the GameObject to check for a collision against")]
    public SharedString tag;
    [Tooltip("The object that started the collision")]
    public SharedGameObject collidedGameObject;
    private bool enteredCollision;

    public HasEnteredCollision2D()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.enteredCollision ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnEnd()
    {
      this.enteredCollision = false;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
      if (!string.IsNullOrEmpty(this.tag.get_Value()) && !this.tag.get_Value().Equals(collision.get_gameObject().get_tag()))
        return;
      this.collidedGameObject.set_Value(collision.get_gameObject());
      this.enteredCollision = true;
    }

    public virtual void OnReset()
    {
      this.tag = (SharedString) string.Empty;
      this.collidedGameObject = (SharedGameObject) null;
    }
  }
}
