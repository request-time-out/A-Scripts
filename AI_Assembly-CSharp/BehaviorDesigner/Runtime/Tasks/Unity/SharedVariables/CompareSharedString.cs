// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
  public class CompareSharedString : Conditional
  {
    [Tooltip("The first variable to compare")]
    public SharedString variable;
    [Tooltip("The variable to compare to")]
    public SharedString compareTo;

    public CompareSharedString()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.variable.get_Value().Equals(this.compareTo.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedString) string.Empty;
      this.compareTo = (SharedString) string.Empty;
    }
  }
}
