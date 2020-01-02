// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs.Save
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
  [TaskCategory("Unity/PlayerPrefs")]
  [TaskDescription("Saves the PlayerPrefs.")]
  public class Save : Action
  {
    public Save()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      PlayerPrefs.Save();
      return (TaskStatus) 2;
    }
  }
}
