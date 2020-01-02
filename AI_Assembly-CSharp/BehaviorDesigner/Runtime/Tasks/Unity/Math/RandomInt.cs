// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.RandomInt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Sets a random int value")]
  public class RandomInt : Action
  {
    [Tooltip("The minimum amount")]
    public SharedInt min;
    [Tooltip("The maximum amount")]
    public SharedInt max;
    [Tooltip("Is the maximum value inclusive?")]
    public bool inclusive;
    [Tooltip("The variable to store the result")]
    public SharedInt storeResult;

    public RandomInt()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.inclusive)
        this.storeResult.set_Value(Random.Range(this.min.get_Value(), this.max.get_Value() + 1));
      else
        this.storeResult.set_Value(Random.Range(this.min.get_Value(), this.max.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.min.set_Value(0);
      this.max.set_Value(0);
      this.inclusive = false;
      this.storeResult.set_Value(0);
    }
  }
}
