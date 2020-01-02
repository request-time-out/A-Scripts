// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController.SetRadius
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController
{
  [TaskCategory("Unity/CharacterController")]
  [TaskDescription("Sets the radius of the CharacterController. Returns Success.")]
  public class SetRadius : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The radius of the CharacterController")]
    public SharedFloat radius;
    private CharacterController characterController;
    private GameObject prevGameObject;

    public SetRadius()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.characterController = (CharacterController) defaultGameObject.GetComponent<CharacterController>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.characterController, (Object) null))
      {
        Debug.LogWarning((object) "CharacterController is null");
        return (TaskStatus) 1;
      }
      this.characterController.set_radius(this.radius.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.radius = (SharedFloat) 0.0f;
    }
  }
}
