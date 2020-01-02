// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemDropArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
  public class ItemDropArgs : EventArgs
  {
    public ItemDropArgs(
      object[] dragItems,
      object dropTarget,
      ItemDropAction action,
      bool isExternal,
      PointerEventData pointerEventData)
    {
      this.DragItems = dragItems;
      this.DropTarget = dropTarget;
      this.Action = action;
      this.IsExternal = isExternal;
      this.PointerEventData = pointerEventData;
    }

    public object[] DragItems { get; private set; }

    public object DropTarget { get; private set; }

    public ItemDropAction Action { get; private set; }

    public bool IsExternal { get; private set; }

    public PointerEventData PointerEventData { get; private set; }
  }
}
