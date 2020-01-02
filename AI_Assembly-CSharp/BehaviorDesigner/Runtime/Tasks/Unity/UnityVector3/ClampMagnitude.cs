// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.ClampMagnitude
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Clamps the magnitude of the Vector3.")]
  public class ClampMagnitude : Action
  {
    [Tooltip("The Vector3 to clamp the magnitude of")]
    public SharedVector3 vector3Variable;
    [Tooltip("The max length of the magnitude")]
    public SharedFloat maxLength;
    [Tooltip("The clamp magnitude resut")]
    [RequiredField]
    public SharedVector3 storeResult;

    public ClampMagnitude()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.ClampMagnitude(this.vector3Variable.get_Value(), this.maxLength.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector3Variable = this.storeResult = (SharedVector3) Vector3.get_zero();
      this.maxLength = (SharedFloat) 0.0f;
    }
  }
}
