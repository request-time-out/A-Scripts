// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.IsNullOrEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Returns success if the string is null or empty")]
  public class IsNullOrEmpty : Conditional
  {
    [Tooltip("The target string")]
    public SharedString targetString;

    public IsNullOrEmpty()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return string.IsNullOrEmpty(this.targetString.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.targetString = (SharedString) string.Empty;
    }
  }
}
