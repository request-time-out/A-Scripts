// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.CompareLayerMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Returns Success if the layermasks match, otherwise Failure.")]
  public class CompareLayerMask : Conditional
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The layermask to compare against")]
    public LayerMask layermask;

    public CompareLayerMask()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return (1 << ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).get_layer() & ((LayerMask) ref this.layermask).get_value()) != 0 ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
    }
  }
}
