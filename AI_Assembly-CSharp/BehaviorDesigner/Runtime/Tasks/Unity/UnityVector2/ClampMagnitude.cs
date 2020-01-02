// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.ClampMagnitude
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Clamps the magnitude of the Vector2.")]
  public class ClampMagnitude : Action
  {
    [Tooltip("The Vector2 to clamp the magnitude of")]
    public SharedVector2 vector2Variable;
    [Tooltip("The max length of the magnitude")]
    public SharedFloat maxLength;
    [Tooltip("The clamp magnitude resut")]
    [RequiredField]
    public SharedVector2 storeResult;

    public ClampMagnitude()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector2.ClampMagnitude(this.vector2Variable.get_Value(), this.maxLength.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector2Variable = this.storeResult = (SharedVector2) Vector2.get_zero();
      this.maxLength = (SharedFloat) 0.0f;
    }
  }
}
