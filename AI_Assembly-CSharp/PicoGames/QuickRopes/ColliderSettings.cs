// Decompiled with JetBrains decompiler
// Type: PicoGames.QuickRopes.ColliderSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace PicoGames.QuickRopes
{
  [Serializable]
  public struct ColliderSettings
  {
    [SerializeField]
    public QuickRope.ColliderType type;
    [SerializeField]
    public ColliderSettings.Direction direction;
    [SerializeField]
    public Vector3 center;
    [SerializeField]
    public Vector3 size;
    [SerializeField]
    [Min(0.0f)]
    public float radius;
    [SerializeField]
    [Min(0.0f)]
    public float height;
    [SerializeField]
    public PhysicMaterial physicsMaterial;

    public enum Direction
    {
      X_Axis,
      Y_Axis,
      Z_Axis,
    }
  }
}
