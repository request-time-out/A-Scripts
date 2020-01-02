// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.GetMousePosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Stores the mouse position.")]
  public class GetMousePosition : Action
  {
    [RequiredField]
    [Tooltip("The stored result")]
    public SharedVector3 storeResult;

    public GetMousePosition()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Input.get_mousePosition());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.storeResult = (SharedVector3) Vector3.get_zero();
    }
  }
}
