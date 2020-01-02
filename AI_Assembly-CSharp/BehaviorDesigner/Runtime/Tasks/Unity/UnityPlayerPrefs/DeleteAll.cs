// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs.DeleteAll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
  [TaskCategory("Unity/PlayerPrefs")]
  [TaskDescription("Deletes all entries from the PlayerPrefs.")]
  public class DeleteAll : Action
  {
    public DeleteAll()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      PlayerPrefs.DeleteAll();
      return (TaskStatus) 2;
    }
  }
}
