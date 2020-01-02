// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.GetXY
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Stores the X and Y values of the Vector2.")]
  public class GetXY : Action
  {
    [Tooltip("The Vector2 to get the values of")]
    public SharedVector2 vector2Variable;
    [Tooltip("The X value")]
    [RequiredField]
    public SharedFloat storeX;
    [Tooltip("The Y value")]
    [RequiredField]
    public SharedFloat storeY;

    public GetXY()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeX.set_Value((float) this.vector2Variable.get_Value().x);
      this.storeY.set_Value((float) this.vector2Variable.get_Value().y);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector2Variable = (SharedVector2) Vector2.get_zero();
      this.storeX = this.storeY = (SharedFloat) 0.0f;
    }
  }
}
