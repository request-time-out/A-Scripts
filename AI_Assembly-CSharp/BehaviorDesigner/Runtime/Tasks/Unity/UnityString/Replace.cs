// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.Replace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Replaces a string with the new string")]
  public class Replace : Action
  {
    [Tooltip("The target string")]
    public SharedString targetString;
    [Tooltip("The old replace")]
    public SharedString oldString;
    [Tooltip("The new string")]
    public SharedString newString;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedString storeResult;

    public Replace()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(this.targetString.get_Value().Replace(this.oldString.get_Value(), this.newString.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetString = (SharedString) string.Empty;
      this.oldString = (SharedString) string.Empty;
      this.newString = (SharedString) string.Empty;
      this.storeResult = (SharedString) string.Empty;
    }
  }
}
