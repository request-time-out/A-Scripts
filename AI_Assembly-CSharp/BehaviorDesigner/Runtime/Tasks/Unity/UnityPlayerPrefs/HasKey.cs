// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs.HasKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
  [TaskCategory("Unity/PlayerPrefs")]
  [TaskDescription("Retruns success if the specified key exists.")]
  public class HasKey : Conditional
  {
    [Tooltip("The key to check")]
    public SharedString key;

    public HasKey()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return PlayerPrefs.HasKey(this.key.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.key = (SharedString) string.Empty;
    }
  }
}
