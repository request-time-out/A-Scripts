// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.GetSubstring
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Stores a substring of the target string")]
  public class GetSubstring : Action
  {
    [Tooltip("The target string")]
    public SharedString targetString;
    [Tooltip("The start substring index")]
    public SharedInt startIndex;
    [Tooltip("The length of the substring. Don't use if -1")]
    public SharedInt length;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedString storeResult;

    public GetSubstring()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.length.get_Value() != -1)
        this.storeResult.set_Value(this.targetString.get_Value().Substring(this.startIndex.get_Value(), this.length.get_Value()));
      else
        this.storeResult.set_Value(this.targetString.get_Value().Substring(this.startIndex.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetString = (SharedString) string.Empty;
      this.startIndex = (SharedInt) 0;
      this.length = (SharedInt) -1;
      this.storeResult = (SharedString) string.Empty;
    }
  }
}
