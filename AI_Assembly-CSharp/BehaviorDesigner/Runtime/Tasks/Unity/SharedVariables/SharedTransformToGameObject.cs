// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SharedTransformToGameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Gets the GameObject from the Transform component. Returns Success.")]
  public class SharedTransformToGameObject : Action
  {
    [Tooltip("The Transform component")]
    public SharedTransform sharedTransform;
    [RequiredField]
    [Tooltip("The GameObject to set")]
    public SharedGameObject sharedGameObject;

    public SharedTransformToGameObject()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.sharedTransform.get_Value(), (Object) null))
        return (TaskStatus) 1;
      this.sharedGameObject.set_Value(((Component) this.sharedTransform.get_Value()).get_gameObject());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.sharedTransform = (SharedTransform) null;
      this.sharedGameObject = (SharedGameObject) null;
    }
  }
}
