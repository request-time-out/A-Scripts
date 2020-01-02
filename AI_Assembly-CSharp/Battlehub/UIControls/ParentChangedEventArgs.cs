// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ParentChangedEventArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace Battlehub.UIControls
{
  public class ParentChangedEventArgs : EventArgs
  {
    public ParentChangedEventArgs(TreeViewItem oldParent, TreeViewItem newParent)
    {
      this.OldParent = oldParent;
      this.NewParent = newParent;
    }

    public TreeViewItem OldParent { get; private set; }

    public TreeViewItem NewParent { get; private set; }
  }
}
