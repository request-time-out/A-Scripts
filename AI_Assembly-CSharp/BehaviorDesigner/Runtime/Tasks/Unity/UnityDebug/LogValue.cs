// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug.LogValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
  [TaskCategory("Unity/Debug")]
  [TaskDescription("Log a variable value.")]
  public class LogValue : Action
  {
    [Tooltip("The variable to output")]
    public SharedGenericVariable variable;

    public LogValue()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Debug.Log(((SharedVariable) ((SharedVariable<GenericVariable>) this.variable).get_Value().value).GetValue());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedGenericVariable) null;
    }
  }
}
