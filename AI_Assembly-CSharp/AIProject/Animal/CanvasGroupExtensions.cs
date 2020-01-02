// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.CanvasGroupExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public static class CanvasGroupExtensions
  {
    public static bool SetBlocksRaycasts(this CanvasGroup canvas, bool active)
    {
      if (Object.op_Equality((Object) canvas, (Object) null) || canvas.get_blocksRaycasts() == active)
        return false;
      canvas.set_blocksRaycasts(active);
      return true;
    }

    public static bool SetInteractable(this CanvasGroup canvas, bool active)
    {
      if (Object.op_Equality((Object) canvas, (Object) null) || canvas.get_interactable() == active)
        return false;
      canvas.set_interactable(active);
      return true;
    }
  }
}
