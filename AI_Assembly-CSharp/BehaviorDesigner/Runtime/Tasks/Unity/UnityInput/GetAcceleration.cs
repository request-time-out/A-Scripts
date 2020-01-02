// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.GetAcceleration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Stores the acceleration value.")]
  public class GetAcceleration : Action
  {
    [RequiredField]
    [Tooltip("The stored result")]
    public SharedVector3 storeResult;

    public GetAcceleration()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Input.get_acceleration());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.storeResult = (SharedVector3) Vector3.get_zero();
    }
  }
}
