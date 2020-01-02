// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.Format
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Stores a string with the specified format.")]
  public class Format : Action
  {
    [Tooltip("The format of the string")]
    public SharedString format;
    [Tooltip("Any variables to appear in the string")]
    public SharedGenericVariable[] variables;
    [Tooltip("The result of the format")]
    [RequiredField]
    public SharedString storeResult;
    private object[] variableValues;

    public Format()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      this.variableValues = new object[this.variables.Length];
    }

    public virtual TaskStatus OnUpdate()
    {
      for (int index = 0; index < this.variableValues.Length; ++index)
        this.variableValues[index] = ((SharedVariable) ((SharedVariable<GenericVariable>) this.variables[index]).get_Value().value).GetValue();
      try
      {
        this.storeResult.set_Value(string.Format(this.format.get_Value(), this.variableValues));
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
        return (TaskStatus) 1;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.format = (SharedString) string.Empty;
      this.variables = (SharedGenericVariable[]) null;
      this.storeResult = (SharedString) null;
    }
  }
}
