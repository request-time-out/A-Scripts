// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.GetTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Stores the GameObject tag. Returns Success.")]
  public class GetTag : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("Active state of the GameObject")]
    [RequiredField]
    public SharedString storeValue;

    public GetTag()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeValue.set_Value(((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).get_tag());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.storeValue = (SharedString) string.Empty;
    }
  }
}
