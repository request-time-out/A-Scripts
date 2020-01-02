// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
  public class ItemArgs : EventArgs
  {
    public ItemArgs(object[] item, PointerEventData pointerEventData)
    {
      this.Items = item;
      this.PointerEventData = pointerEventData;
    }

    public object[] Items { get; private set; }

    public PointerEventData PointerEventData { get; private set; }
  }
}
