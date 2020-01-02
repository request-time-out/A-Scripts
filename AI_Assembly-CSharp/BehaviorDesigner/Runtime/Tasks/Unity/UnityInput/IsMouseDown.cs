// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.IsMouseDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Returns success when the specified mouse button is pressed.")]
  public class IsMouseDown : Conditional
  {
    [Tooltip("The button index")]
    public SharedInt buttonIndex;

    public IsMouseDown()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return Input.GetMouseButtonDown(this.buttonIndex.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.buttonIndex = (SharedInt) 0;
    }
  }
}
