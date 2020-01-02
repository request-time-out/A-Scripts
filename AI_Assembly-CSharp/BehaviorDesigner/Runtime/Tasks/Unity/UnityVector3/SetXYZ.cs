// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.SetXYZ
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Sets the X, Y, and Z values of the Vector3.")]
  public class SetXYZ : Action
  {
    [Tooltip("The Vector3 to set the values of")]
    public SharedVector3 vector3Variable;
    [Tooltip("The X value. Set to None to have the value ignored")]
    public SharedFloat xValue;
    [Tooltip("The Y value. Set to None to have the value ignored")]
    public SharedFloat yValue;
    [Tooltip("The Z value. Set to None to have the value ignored")]
    public SharedFloat zValue;

    public SetXYZ()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Vector3 vector3 = this.vector3Variable.get_Value();
      if (!((SharedVariable) this.xValue).get_IsNone())
        vector3.x = (__Null) (double) this.xValue.get_Value();
      if (!((SharedVariable) this.yValue).get_IsNone())
        vector3.y = (__Null) (double) this.yValue.get_Value();
      if (!((SharedVariable) this.zValue).get_IsNone())
        vector3.z = (__Null) (double) this.zValue.get_Value();
      this.vector3Variable.set_Value(vector3);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector3Variable = (SharedVector3) Vector3.get_zero();
      this.xValue = this.yValue = this.zValue = (SharedFloat) 0.0f;
    }
  }
}
