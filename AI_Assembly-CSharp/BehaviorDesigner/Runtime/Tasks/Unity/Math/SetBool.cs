// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.SetBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Sets a bool value")]
  public class SetBool : Action
  {
    [Tooltip("The bool value to set")]
    public SharedBool boolValue;
    [Tooltip("The variable to store the result")]
    public SharedBool storeResult;

    public SetBool()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(this.boolValue.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.boolValue.set_Value(false);
      this.storeResult.set_Value(false);
    }
  }
}
