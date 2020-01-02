// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityTime.GetDeltaTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime
{
  [TaskCategory("Unity/Time")]
  [TaskDescription("Returns the time in seconds it took to complete the last frame.")]
  public class GetDeltaTime : Action
  {
    [Tooltip("The variable to store the result")]
    public SharedFloat storeResult;

    public GetDeltaTime()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Time.get_deltaTime());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.storeResult.set_Value(0.0f);
    }
  }
}
