// Decompiled with JetBrains decompiler
// Type: PicoGames.QuickRopes.JointSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace PicoGames.QuickRopes
{
  [Serializable]
  public struct JointSettings
  {
    [SerializeField]
    [Min(0.001f)]
    public float breakForce;
    [SerializeField]
    [Min(0.001f)]
    public float breakTorque;
    [SerializeField]
    [Range(0.0f, 180f)]
    public float twistLimit;
    [SerializeField]
    [Range(0.0f, 180f)]
    public float swingLimit;
    [SerializeField]
    [Min(0.0f)]
    public float spring;
    [SerializeField]
    [Min(0.0f)]
    public float damper;
  }
}
