// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.CanvasGroupExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Illusion.Extensions
{
  public static class CanvasGroupExtensions
  {
    public static void Enable(this CanvasGroup canvasGroup, bool enable, bool ignoreParentGroups = false)
    {
      canvasGroup.set_alpha(!enable ? 0.0f : 1f);
      canvasGroup.set_interactable(enable);
      canvasGroup.set_blocksRaycasts(enable);
    }

    public static void Set(
      this CanvasGroup canvasGroup,
      float alpha,
      bool interactable,
      bool blocksRaycasts,
      bool ignoreParentGroups = false)
    {
      canvasGroup.set_alpha(alpha);
      canvasGroup.set_interactable(interactable);
      canvasGroup.set_blocksRaycasts(blocksRaycasts);
      canvasGroup.set_ignoreParentGroups(ignoreParentGroups);
    }
  }
}
