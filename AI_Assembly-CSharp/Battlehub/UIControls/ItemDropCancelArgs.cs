// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemDropCancelArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
  public class ItemDropCancelArgs : ItemDropArgs
  {
    public ItemDropCancelArgs(
      object[] dragItems,
      object dropTarget,
      ItemDropAction action,
      bool isExternal,
      PointerEventData pointerEventData)
      : base(dragItems, dropTarget, action, isExternal, pointerEventData)
    {
    }

    public bool Cancel { get; set; }
  }
}
