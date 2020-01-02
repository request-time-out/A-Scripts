// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.IsIntPositive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Is the int a positive value?")]
  public class IsIntPositive : Conditional
  {
    [Tooltip("The int to check if positive")]
    public SharedInt intVariable;

    public IsIntPositive()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.intVariable.get_Value() > 0 ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.intVariable = (SharedInt) 0;
    }
  }
}
