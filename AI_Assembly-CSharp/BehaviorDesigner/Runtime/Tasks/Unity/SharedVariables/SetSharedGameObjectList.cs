// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedGameObjectList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedGameObjectList variable to the specified object. Returns Success.")]
  public class SetSharedGameObjectList : Action
  {
    [Tooltip("The value to set the SharedGameObjectList to.")]
    public SharedGameObjectList targetValue;
    [RequiredField]
    [Tooltip("The SharedGameObjectList to set")]
    public SharedGameObjectList targetVariable;

    public SetSharedGameObjectList()
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
      this.targetValue = (SharedGameObjectList) null;
      this.targetVariable = (SharedGameObjectList) null;
    }
  }
}
