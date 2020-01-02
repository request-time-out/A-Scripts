// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.RandomProbability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The random probability task will return success when the random probability is above the succeed probability. It will otherwise return failure.")]
  public class RandomProbability : Conditional
  {
    [Tooltip("The chance that the task will return success")]
    public SharedFloat successProbability;
    [Tooltip("Seed the random number generator to make things easier to debug")]
    public SharedInt seed;
    [Tooltip("Do we want to use the seed?")]
    public SharedBool useSeed;

    public RandomProbability()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      if (!this.useSeed.get_Value())
        return;
      Random.InitState(this.seed.get_Value());
    }

    public virtual TaskStatus OnUpdate()
    {
      return (double) Random.get_value() < (double) this.successProbability.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.successProbability = (SharedFloat) 0.5f;
      this.seed = (SharedInt) 0;
      this.useSeed = (SharedBool) false;
    }
  }
}
