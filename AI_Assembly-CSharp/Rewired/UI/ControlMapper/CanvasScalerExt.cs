﻿// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.CanvasScalerExt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class CanvasScalerExt : CanvasScaler
  {
    public CanvasScalerExt()
    {
      base.\u002Ector();
    }

    public void ForceRefresh()
    {
      this.Handle();
    }
  }
}
