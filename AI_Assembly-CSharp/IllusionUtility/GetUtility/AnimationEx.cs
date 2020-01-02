// Decompiled with JetBrains decompiler
// Type: IllusionUtility.GetUtility.AnimationEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

namespace IllusionUtility.GetUtility
{
  public static class AnimationEx
  {
    public static AnimationClip GetPlayingClip(this Animation animation)
    {
      AnimationClip animationClip = (AnimationClip) null;
      float num = 0.0f;
      IEnumerator enumerator = animation.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          AnimationState current = (AnimationState) enumerator.Current;
          if (current.get_enabled() && (double) num < (double) current.get_weight())
          {
            num = current.get_weight();
            animationClip = current.get_clip();
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      return animationClip;
    }
  }
}
