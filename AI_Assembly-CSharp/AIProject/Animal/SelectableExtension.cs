// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.SelectableExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace AIProject.Animal
{
  public static class SelectableExtension
  {
    public static void SetInteractable(this Selectable sel, bool active)
    {
      if (!Object.op_Inequality((Object) sel, (Object) null) && sel.get_interactable() == active)
        return;
      sel.set_interactable(active);
    }
  }
}
