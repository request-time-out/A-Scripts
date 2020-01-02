// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.SetValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Sets the value of the Vector3.")]
  public class SetValue : Action
  {
    [Tooltip("The Vector3 to get the values of")]
    public SharedVector3 vector3Value;
    [Tooltip("The Vector3 to set the values of")]
    public SharedVector3 vector3Variable;

    public SetValue()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.vector3Variable.set_Value(this.vector3Value.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector3Value = this.vector3Variable = (SharedVector3) Vector3.get_zero();
    }
  }
}
