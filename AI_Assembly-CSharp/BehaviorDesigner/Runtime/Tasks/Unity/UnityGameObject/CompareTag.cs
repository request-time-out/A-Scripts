// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.CompareTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Returns Success if tags match, otherwise Failure.")]
  public class CompareTag : Conditional
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The tag to compare against")]
    public SharedString tag;

    public CompareTag()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).CompareTag(this.tag.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.tag = (SharedString) string.Empty;
    }
  }
}
