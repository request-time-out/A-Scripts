// Decompiled with JetBrains decompiler
// Type: PicoGames.QuickRopes.RigidbodySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace PicoGames.QuickRopes
{
  [Serializable]
  public struct RigidbodySettings
  {
    [Min(0.001f)]
    public float mass;
    [Min(0.0f)]
    public float drag;
    [Min(0.0f)]
    public float angularDrag;
    public bool useGravity;
    public bool isKinematic;
    public RigidbodyInterpolation interpolate;
    public CollisionDetectionMode collisionDetection;
    [SerializeField]
    public RigidbodyConstraints constraints;
    [Range(6f, 100f)]
    public int solverCount;
  }
}
