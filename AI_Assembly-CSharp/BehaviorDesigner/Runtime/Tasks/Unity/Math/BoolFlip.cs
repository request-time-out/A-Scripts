// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.BoolFlip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Flips the value of the bool.")]
  public class BoolFlip : Action
  {
    [Tooltip("The bool to flip the value of")]
    public SharedBool boolVariable;

    public BoolFlip()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.boolVariable.set_Value(!this.boolVariable.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.boolVariable.set_Value(false);
    }
  }
}
