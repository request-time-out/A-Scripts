// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.GetXYZ
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Stores the X, Y, and Z values of the Vector3.")]
  public class GetXYZ : Action
  {
    [Tooltip("The Vector3 to get the values of")]
    public SharedVector3 vector3Variable;
    [Tooltip("The X value")]
    [RequiredField]
    public SharedFloat storeX;
    [Tooltip("The Y value")]
    [RequiredField]
    public SharedFloat storeY;
    [Tooltip("The Z value")]
    [RequiredField]
    public SharedFloat storeZ;

    public GetXYZ()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeX.set_Value((float) this.vector3Variable.get_Value().x);
      this.storeY.set_Value((float) this.vector3Variable.get_Value().y);
      this.storeZ.set_Value((float) this.vector3Variable.get_Value().z);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector3Variable = (SharedVector3) Vector3.get_zero();
      this.storeX = this.storeY = this.storeZ = (SharedFloat) 0.0f;
    }
  }
}
