// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedTransformList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedTransformList variable to the specified object. Returns Success.")]
  public class SetSharedTransformList : Action
  {
    [Tooltip("The value to set the SharedTransformList to.")]
    public SharedTransformList targetValue;
    [RequiredField]
    [Tooltip("The SharedTransformList to set")]
    public SharedTransformList targetVariable;

    public SetSharedTransformList()
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
      this.targetValue = (SharedTransformList) null;
      this.targetVariable = (SharedTransformList) null;
    }
  }
}
