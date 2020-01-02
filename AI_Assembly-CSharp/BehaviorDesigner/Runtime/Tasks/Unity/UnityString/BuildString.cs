// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.BuildString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Creates a string from multiple other strings.")]
  public class BuildString : Action
  {
    [Tooltip("The array of strings")]
    public SharedString[] source;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedString storeResult;

    public BuildString()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      for (int index = 0; index < this.source.Length; ++index)
      {
        SharedString storeResult = this.storeResult;
        storeResult.set_Value(storeResult.get_Value() + (object) this.source[index]);
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.source = (SharedString[]) null;
      this.storeResult = (SharedString) null;
    }
  }
}
