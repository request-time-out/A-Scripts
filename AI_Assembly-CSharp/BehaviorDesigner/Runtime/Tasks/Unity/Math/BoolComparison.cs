// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.BoolComparison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Performs a comparison between two bools.")]
  public class BoolComparison : Conditional
  {
    [Tooltip("The first bool")]
    public SharedBool bool1;
    [Tooltip("The second bool")]
    public SharedBool bool2;

    public BoolComparison()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.bool1.get_Value() == this.bool2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.bool1.set_Value(false);
      this.bool2.set_Value(false);
    }
  }
}
