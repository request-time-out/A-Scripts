// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.GetKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Stores the pressed state of the specified key.")]
  public class GetKey : Action
  {
    [Tooltip("The key to test.")]
    public KeyCode key;
    [RequiredField]
    [Tooltip("The stored result")]
    public SharedBool storeResult;

    public GetKey()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Input.GetKey(this.key));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.key = (KeyCode) 0;
      this.storeResult = (SharedBool) false;
    }
  }
}
