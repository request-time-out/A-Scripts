// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.SetString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Sets the variable string to the value string.")]
  public class SetString : Action
  {
    [Tooltip("The target string")]
    [RequiredField]
    public SharedString variable;
    [Tooltip("The value string")]
    public SharedString value;

    public SetString()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.variable.set_Value(this.value.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedString) string.Empty;
      this.value = (SharedString) string.Empty;
    }
  }
}
