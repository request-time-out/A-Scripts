// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.SetInt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Sets an int value")]
  public class SetInt : Action
  {
    [Tooltip("The int value to set")]
    public SharedInt intValue;
    [Tooltip("The variable to store the result")]
    public SharedInt storeResult;

    public SetInt()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(this.intValue.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.intValue.set_Value(0);
      this.storeResult.set_Value(0);
    }
  }
}
