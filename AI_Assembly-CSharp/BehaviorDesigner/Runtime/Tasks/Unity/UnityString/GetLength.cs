// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.GetLength
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Stores the length of the string")]
  public class GetLength : Action
  {
    [Tooltip("The target string")]
    public SharedString targetString;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedInt storeResult;

    public GetLength()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(this.targetString.get_Value().Length);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetString = (SharedString) string.Empty;
      this.storeResult = (SharedInt) 0;
    }
  }
}
