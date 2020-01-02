// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.RandomBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Sets a random bool value")]
  public class RandomBool : Action
  {
    [Tooltip("The variable to store the result")]
    public SharedBool storeResult;

    public RandomBool()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value((double) Random.get_value() < 0.5);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.storeResult.set_Value(false);
    }
  }
}
