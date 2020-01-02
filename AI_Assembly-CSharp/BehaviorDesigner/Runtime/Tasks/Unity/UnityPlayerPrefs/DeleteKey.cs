// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs.DeleteKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
  [TaskCategory("Unity/PlayerPrefs")]
  [TaskDescription("Deletes the specified key from the PlayerPrefs.")]
  public class DeleteKey : Action
  {
    [Tooltip("The key to delete")]
    public SharedString key;

    public DeleteKey()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      PlayerPrefs.DeleteKey(this.key.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.key = (SharedString) string.Empty;
    }
  }
}
