// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SharedTransformsToTransformList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedTransformList values from the Transforms. Returns Success.")]
  public class SharedTransformsToTransformList : Action
  {
    [Tooltip("The Transforms value")]
    public SharedTransform[] transforms;
    [RequiredField]
    [Tooltip("The SharedTransformList to set")]
    public SharedTransformList storedTransformList;

    public SharedTransformsToTransformList()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      this.storedTransformList.set_Value(new List<Transform>());
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.transforms == null || this.transforms.Length == 0)
        return (TaskStatus) 1;
      this.storedTransformList.get_Value().Clear();
      for (int index = 0; index < this.transforms.Length; ++index)
        this.storedTransformList.get_Value().Add(this.transforms[index].get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.transforms = (SharedTransform[]) null;
      this.storedTransformList = (SharedTransformList) null;
    }
  }
}
