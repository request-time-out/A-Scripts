// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.GetMouseButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Stores the state of the specified mouse button.")]
  public class GetMouseButton : Action
  {
    [Tooltip("The index of the button")]
    public SharedInt buttonIndex;
    [RequiredField]
    [Tooltip("The stored result")]
    public SharedBool storeResult;

    public GetMouseButton()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Input.GetMouseButton(this.buttonIndex.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.buttonIndex = (SharedInt) 0;
      this.storeResult = (SharedBool) false;
    }
  }
}
