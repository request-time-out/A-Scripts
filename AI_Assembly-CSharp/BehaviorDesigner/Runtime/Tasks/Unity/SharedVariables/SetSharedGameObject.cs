// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedGameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedGameObject variable to the specified object. Returns Success.")]
  public class SetSharedGameObject : Action
  {
    [Tooltip("The value to set the SharedGameObject to. If null the variable will be set to the current GameObject")]
    public SharedGameObject targetValue;
    [RequiredField]
    [Tooltip("The SharedGameObject to set")]
    public SharedGameObject targetVariable;
    [Tooltip("Can the target value be null?")]
    public SharedBool valueCanBeNull;

    public SetSharedGameObject()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.targetVariable.set_Value(Object.op_Inequality((Object) this.targetValue.get_Value(), (Object) null) || this.valueCanBeNull.get_Value() ? this.targetValue.get_Value() : (GameObject) ((Task) this).gameObject);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.valueCanBeNull = (SharedBool) false;
      this.targetValue = (SharedGameObject) null;
      this.targetVariable = (SharedGameObject) null;
    }
  }
}
