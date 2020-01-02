// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.SetXY
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Sets the X and Y values of the Vector2.")]
  public class SetXY : Action
  {
    [Tooltip("The Vector2 to set the values of")]
    public SharedVector2 vector2Variable;
    [Tooltip("The X value. Set to None to have the value ignored")]
    public SharedFloat xValue;
    [Tooltip("The Y value. Set to None to have the value ignored")]
    public SharedFloat yValue;

    public SetXY()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Vector2 vector2 = this.vector2Variable.get_Value();
      if (!((SharedVariable) this.xValue).get_IsNone())
        vector2.x = (__Null) (double) this.xValue.get_Value();
      if (!((SharedVariable) this.yValue).get_IsNone())
        vector2.y = (__Null) (double) this.yValue.get_Value();
      this.vector2Variable.set_Value(vector2);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector2Variable = (SharedVector2) Vector2.get_zero();
      this.xValue = this.yValue = (SharedFloat) 0.0f;
    }
  }
}
