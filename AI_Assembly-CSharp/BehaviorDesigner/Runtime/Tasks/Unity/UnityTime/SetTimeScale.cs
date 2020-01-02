// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityTime.SetTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime
{
  [TaskCategory("Unity/Time")]
  [TaskDescription("Sets the scale at which time is passing.")]
  public class SetTimeScale : Action
  {
    [Tooltip("The timescale")]
    public SharedFloat timeScale;

    public SetTimeScale()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Time.set_timeScale(this.timeScale.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.timeScale.set_Value(0.0f);
    }
  }
}
