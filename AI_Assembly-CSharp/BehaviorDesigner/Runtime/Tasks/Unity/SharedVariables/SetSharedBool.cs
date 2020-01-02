// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedBool variable to the specified object. Returns Success.")]
  public class SetSharedBool : Action
  {
    [Tooltip("The value to set the SharedBool to")]
    public SharedBool targetValue;
    [RequiredField]
    [Tooltip("The SharedBool to set")]
    public SharedBool targetVariable;

    public SetSharedBool()
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
      this.targetValue = (SharedBool) false;
      this.targetVariable = (SharedBool) false;
    }
  }
}
