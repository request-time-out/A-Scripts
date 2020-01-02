// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.FindWithTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Finds a GameObject by tag. Returns Success.")]
  public class FindWithTag : Action
  {
    [Tooltip("The tag of the GameObject to find")]
    public SharedString tag;
    [Tooltip("Should a random GameObject be found?")]
    public SharedBool random;
    [Tooltip("The object found by name")]
    [RequiredField]
    public SharedGameObject storeValue;

    public FindWithTag()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.random.get_Value())
      {
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(this.tag.get_Value());
        this.storeValue.set_Value(gameObjectsWithTag[Random.Range(0, gameObjectsWithTag.Length - 1)]);
      }
      else
        this.storeValue.set_Value(GameObject.FindWithTag(this.tag.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.tag.set_Value((string) null);
      this.storeValue.set_Value((GameObject) null);
    }
  }
}
