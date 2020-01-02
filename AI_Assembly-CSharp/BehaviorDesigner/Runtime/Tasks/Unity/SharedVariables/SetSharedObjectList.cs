// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedObjectList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedObjectList variable to the specified object. Returns Success.")]
  public class SetSharedObjectList : Action
  {
    [Tooltip("The value to set the SharedObjectList to.")]
    public SharedObjectList targetValue;
    [RequiredField]
    [Tooltip("The SharedObjectList to set")]
    public SharedObjectList targetVariable;

    public SetSharedObjectList()
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
      this.targetValue = (SharedObjectList) null;
      this.targetVariable = (SharedObjectList) null;
    }
  }
}
