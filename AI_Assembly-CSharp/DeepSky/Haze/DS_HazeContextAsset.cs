// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.DS_HazeContextAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace DeepSky.Haze
{
  [AddComponentMenu("")]
  [Serializable]
  public class DS_HazeContextAsset : ScriptableObject
  {
    [SerializeField]
    private DS_HazeContext m_Context;

    public DS_HazeContextAsset()
    {
      base.\u002Ector();
    }

    public DS_HazeContext Context
    {
      get
      {
        return this.m_Context;
      }
    }
  }
}
