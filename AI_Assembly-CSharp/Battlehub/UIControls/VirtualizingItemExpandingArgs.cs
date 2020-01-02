// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingItemExpandingArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;

namespace Battlehub.UIControls
{
  public class VirtualizingItemExpandingArgs : EventArgs
  {
    public VirtualizingItemExpandingArgs(object item)
    {
      this.Item = item;
    }

    public object Item { get; private set; }

    public IEnumerable Children { get; set; }

    public IEnumerable ChildrenExpand { get; set; }
  }
}
