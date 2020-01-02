// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedInt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedInt variable to the specified object. Returns Success.")]
  public class SetSharedInt : Action
  {
    [Tooltip("The value to set the SharedInt to")]
    public SharedInt targetValue;
    [RequiredField]
    [Tooltip("The SharedInt to set")]
    public SharedInt targetVariable;

    public SetSharedInt()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.targetVariable.set_Value(this.targetValue.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetValue = (SharedInt) 0;
      this.targetVariable = (SharedInt) 0;
    }
  }
}
