// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.Destroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Destorys the specified GameObject. Returns Success.")]
  public class Destroy : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("Time to destroy the GameObject in")]
    public float time;

    public Destroy()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if ((double) this.time == 0.0)
        Object.Destroy((Object) defaultGameObject);
      else
        Object.Destroy((Object) defaultGameObject, this.time);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.time = 0.0f;
    }
  }
}
