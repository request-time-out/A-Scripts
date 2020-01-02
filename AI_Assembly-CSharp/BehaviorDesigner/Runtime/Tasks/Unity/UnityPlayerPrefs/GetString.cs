// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs.GetString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
  [TaskCategory("Unity/PlayerPrefs")]
  [TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
  public class GetString : Action
  {
    [Tooltip("The key to store")]
    public SharedString key;
    [Tooltip("The default value")]
    public SharedString defaultValue;
    [Tooltip("The value retrieved from the PlayerPrefs")]
    [RequiredField]
    public SharedString storeResult;

    public GetString()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(PlayerPrefs.GetString(this.key.get_Value(), this.defaultValue.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.key = (SharedString) string.Empty;
      this.defaultValue = (SharedString) string.Empty;
      this.storeResult = (SharedString) string.Empty;
    }
  }
}
