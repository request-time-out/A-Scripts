// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemsCancelArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace Battlehub.UIControls
{
  public class ItemsCancelArgs : EventArgs
  {
    public ItemsCancelArgs(List<object> items)
    {
      this.Items = items;
    }

    public List<object> Items { get; set; }
  }
}
