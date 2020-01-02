// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.FloatAbs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Stores the absolute value of the float.")]
  public class FloatAbs : Action
  {
    [Tooltip("The float to return the absolute value of")]
    public SharedFloat floatVariable;

    public FloatAbs()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.floatVariable.set_Value(Mathf.Abs(this.floatVariable.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.floatVariable = (SharedFloat) 0.0f;
    }
  }
}
