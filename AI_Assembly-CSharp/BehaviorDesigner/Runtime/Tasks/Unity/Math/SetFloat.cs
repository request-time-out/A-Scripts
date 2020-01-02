// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.SetFloat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Sets a float value")]
  public class SetFloat : Action
  {
    [Tooltip("The float value to set")]
    public SharedFloat floatValue;
    [Tooltip("The variable to store the result")]
    public SharedFloat storeResult;

    public SetFloat()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(this.floatValue.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.floatValue.set_Value(0.0f);
      this.storeResult.set_Value(0.0f);
    }
  }
}
