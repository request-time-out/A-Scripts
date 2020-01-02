// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SharedGameObjectToTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Gets the Transform from the GameObject. Returns Success.")]
  public class SharedGameObjectToTransform : Action
  {
    [Tooltip("The GameObject to get the Transform of")]
    public SharedGameObject sharedGameObject;
    [RequiredField]
    [Tooltip("The Transform to set")]
    public SharedTransform sharedTransform;

    public SharedGameObjectToTransform()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.sharedGameObject.get_Value(), (Object) null))
        return (TaskStatus) 1;
      this.sharedTransform.set_Value((Transform) this.sharedGameObject.get_Value().GetComponent<Transform>());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.sharedGameObject = (SharedGameObject) null;
      this.sharedTransform = (SharedTransform) null;
    }
  }
}
