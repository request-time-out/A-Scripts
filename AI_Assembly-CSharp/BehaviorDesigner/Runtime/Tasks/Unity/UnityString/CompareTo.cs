// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.CompareTo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Compares the first string to the second string. Returns an int which indicates whether the first string precedes, matches, or follows the second string.")]
  public class CompareTo : Action
  {
    [Tooltip("The string to compare")]
    public SharedString firstString;
    [Tooltip("The string to compare to")]
    public SharedString secondString;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedInt storeResult;

    public CompareTo()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(this.firstString.get_Value().CompareTo(this.secondString.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.firstString = (SharedString) string.Empty;
      this.secondString = (SharedString) string.Empty;
      this.storeResult = (SharedInt) 0;
    }
  }
}
