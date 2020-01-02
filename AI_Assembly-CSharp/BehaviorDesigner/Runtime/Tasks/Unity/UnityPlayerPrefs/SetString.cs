// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs.SetString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
  [TaskCategory("Unity/PlayerPrefs")]
  [TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
  public class SetString : Action
  {
    [Tooltip("The key to store")]
    public SharedString key;
    [Tooltip("The value to set")]
    public SharedString value;

    public SetString()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      PlayerPrefs.SetString(this.key.get_Value(), this.value.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.key = (SharedString) string.Empty;
      this.value = (SharedString) string.Empty;
    }
  }
}
