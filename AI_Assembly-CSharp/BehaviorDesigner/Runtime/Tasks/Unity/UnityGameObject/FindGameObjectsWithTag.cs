// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.FindGameObjectsWithTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Finds a GameObject by tag. Returns Success.")]
  public class FindGameObjectsWithTag : Action
  {
    [Tooltip("The tag of the GameObject to find")]
    public SharedString tag;
    [Tooltip("The objects found by name")]
    [RequiredField]
    public SharedGameObjectList storeValue;

    public FindGameObjectsWithTag()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(this.tag.get_Value()))
        this.storeValue.get_Value().Add(gameObject);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.tag.set_Value((string) null);
      this.storeValue.set_Value((List<GameObject>) null);
    }
  }
}
