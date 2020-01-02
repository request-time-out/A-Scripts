// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingParentChangedEventArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace Battlehub.UIControls
{
  public class VirtualizingParentChangedEventArgs : EventArgs
  {
    public VirtualizingParentChangedEventArgs(
      TreeViewItemContainerData oldParent,
      TreeViewItemContainerData newParent)
    {
      this.OldParent = oldParent;
      this.NewParent = newParent;
    }

    public TreeViewItemContainerData OldParent { get; private set; }

    public TreeViewItemContainerData NewParent { get; private set; }
  }
}
