// Decompiled with JetBrains decompiler
// Type: PicoGames.QuickRopes.TransformSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace PicoGames.QuickRopes
{
  [Serializable]
  public struct TransformSettings
  {
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public Vector3 eulerRotation;
    [SerializeField]
    public Vector3 scale;

    [SerializeField]
    public Quaternion rotation
    {
      get
      {
        return Quaternion.Euler(this.eulerRotation);
      }
      set
      {
        this.eulerRotation = ((Quaternion) ref value).get_eulerAngles();
      }
    }
  }
}
