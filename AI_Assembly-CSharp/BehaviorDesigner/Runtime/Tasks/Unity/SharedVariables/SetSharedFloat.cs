// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedFloat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedFloat variable to the specified object. Returns Success.")]
  public class SetSharedFloat : Action
  {
    [Tooltip("The value to set the SharedFloat to")]
    public SharedFloat targetValue;
    [RequiredField]
    [Tooltip("The SharedFloat to set")]
    public SharedFloat targetVariable;

    public SetSharedFloat()
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
      this.targetValue = (SharedFloat) 0.0f;
      this.targetVariable = (SharedFloat) 0.0f;
    }
  }
}
