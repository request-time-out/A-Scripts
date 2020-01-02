// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.GetAxis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Stores the value of the specified axis and stores it in a float.")]
  public class GetAxis : Action
  {
    [Tooltip("The name of the axis")]
    public SharedString axisName;
    [Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range")]
    public SharedFloat multiplier;
    [RequiredField]
    [Tooltip("The stored result")]
    public SharedFloat storeResult;

    public GetAxis()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      float axis = Input.GetAxis(this.axisName.get_Value());
      if (!((SharedVariable) this.multiplier).get_IsNone())
        axis *= this.multiplier.get_Value();
      this.storeResult.set_Value(axis);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.axisName = (SharedString) string.Empty;
      this.multiplier = (SharedFloat) 1f;
      this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
