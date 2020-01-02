// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.IsKeyUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Returns success when the specified key is released.")]
  public class IsKeyUp : Conditional
  {
    [Tooltip("The key to test")]
    public KeyCode key;

    public IsKeyUp()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return Input.GetKeyUp(this.key) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.key = (KeyCode) 0;
    }
  }
}
