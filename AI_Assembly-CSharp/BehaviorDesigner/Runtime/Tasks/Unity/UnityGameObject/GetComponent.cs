// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.GetComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Returns the component of Type type if the game object has one attached, null if it doesn't. Returns Success.")]
  public class GetComponent : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The type of component")]
    public SharedString type;
    [Tooltip("The component")]
    [RequiredField]
    public SharedObject storeValue;

    public GetComponent()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeValue.set_Value((Object) ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).GetComponent(this.type.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.type.set_Value(string.Empty);
      this.storeValue.set_Value((Object) null);
    }
  }
}
