// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.GetForwardVector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Stores the forward vector value.")]
  public class GetForwardVector : Action
  {
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedVector3 storeResult;

    public GetForwardVector()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.get_forward());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.storeResult = (SharedVector3) Vector3.get_zero();
    }
  }
}
