// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.SetValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Sets the value of the Vector2.")]
  public class SetValue : Action
  {
    [Tooltip("The Vector2 to get the values of")]
    public SharedVector2 vector2Value;
    [Tooltip("The Vector2 to set the values of")]
    public SharedVector2 vector2Variable;

    public SetValue()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.vector2Variable.set_Value(this.vector2Value.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector2Value = this.vector2Variable = (SharedVector2) Vector2.get_zero();
    }
  }
}
