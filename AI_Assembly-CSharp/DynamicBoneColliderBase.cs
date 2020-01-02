// Decompiled with JetBrains decompiler
// Type: DynamicBoneColliderBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class DynamicBoneColliderBase : MonoBehaviour
{
  public DynamicBoneColliderBase.Direction m_Direction;
  public Vector3 m_Center;
  public DynamicBoneColliderBase.Bound m_Bound;

  public DynamicBoneColliderBase()
  {
    base.\u002Ector();
  }

  public virtual void Collide(ref Vector3 particlePosition, float particleRadius)
  {
  }

  public enum Direction
  {
    X,
    Y,
    Z,
  }

  public enum Bound
  {
    Outside,
    Inside,
  }
}
