// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.Find
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Finds a GameObject by name. Returns Success.")]
  public class Find : Action
  {
    [Tooltip("The GameObject name to find")]
    public SharedString gameObjectName;
    [Tooltip("The object found by name")]
    [RequiredField]
    public SharedGameObject storeValue;

    public Find()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeValue.set_Value(GameObject.Find(this.gameObjectName.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.gameObjectName = (SharedString) null;
      this.storeValue = (SharedGameObject) null;
    }
  }
}
