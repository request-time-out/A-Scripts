﻿// Decompiled with JetBrains decompiler
// Type: PicoGames.Utilities.SplinePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace PicoGames.Utilities
{
  [Serializable]
  public class SplinePoint
  {
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public Quaternion rotation;

    public SplinePoint(Vector3 _position, Quaternion _rotation)
    {
      this.position = _position;
      this.rotation = _rotation;
    }
  }
}
