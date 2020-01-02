// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedColor variable to the specified object. Returns Success.")]
  public class SetSharedColor : Action
  {
    [Tooltip("The value to set the SharedColor to")]
    public SharedColor targetValue;
    [RequiredField]
    [Tooltip("The SharedColor to set")]
    public SharedColor targetVariable;

    public SetSharedColor()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.targetVariable.set_Value(this.targetValue.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetValue = (SharedColor) Color.get_black();
      this.targetVariable = (SharedColor) Color.get_black();
    }
  }
}
