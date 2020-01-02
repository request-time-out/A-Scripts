// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.SelectionChangedArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace Battlehub.UIControls
{
  public class SelectionChangedArgs : EventArgs
  {
    public SelectionChangedArgs(object[] oldItems, object[] newItems)
    {
      this.OldItems = oldItems;
      this.NewItems = newItems;
    }

    public SelectionChangedArgs(object oldItem, object newItem)
    {
      this.OldItems = new object[1]{ oldItem };
      this.NewItems = new object[1]{ newItem };
    }

    public object[] OldItems { get; private set; }

    public object[] NewItems { get; private set; }

    public object OldItem
    {
      get
      {
        if (this.OldItems == null)
          return (object) null;
        return this.OldItems.Length == 0 ? (object) null : this.OldItems[0];
      }
    }

    public object NewItem
    {
      get
      {
        if (this.NewItems == null)
          return (object) null;
        return this.NewItems.Length == 0 ? (object) null : this.NewItems[0];
      }
    }
  }
}
