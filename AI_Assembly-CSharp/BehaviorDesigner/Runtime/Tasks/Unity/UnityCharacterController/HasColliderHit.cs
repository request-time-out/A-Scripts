// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController.HasColliderHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController
{
  [TaskCategory("Unity/CharacterController")]
  [TaskDescription("Returns Success if the collider hit another object, otherwise Failure.")]
  public class HasColliderHit : Conditional
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The tag of the GameObject to check for a collision against")]
    public SharedString tag;
    [Tooltip("The object that started the collision")]
    public SharedGameObject collidedGameObject;
    private bool enteredCollision;

    public HasColliderHit()
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

    public virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {
      if (!string.IsNullOrEmpty(this.tag.get_Value()) && !this.tag.get_Value().Equals(hit.get_gameObject().get_tag()))
        return;
      this.collidedGameObject.set_Value(hit.get_gameObject());
      this.enteredCollision = true;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.tag = (SharedString) string.Empty;
      this.collidedGameObject = (SharedGameObject) null;
    }
  }
}
