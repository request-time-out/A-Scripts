// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemsControl`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Battlehub.UIControls
{
  public class ItemsControl<TDataBindingArgs> : ItemsControl where TDataBindingArgs : ItemDataBindingArgs, new()
  {
    public event EventHandler<TDataBindingArgs> ItemDataBinding;

    public event EventHandler<TDataBindingArgs> ItemBeginEdit;

    public event EventHandler<TDataBindingArgs> ItemEndEdit;

    protected override void OnItemBeginEdit(object sender, EventArgs e)
    {
      if (!this.CanHandleEvent(sender))
        return;
      ItemContainer itemContainer = (ItemContainer) sender;
      if (this.ItemBeginEdit == null)
        return;
      this.ItemBeginEdit((object) this, new TDataBindingArgs()
      {
        Item = itemContainer.Item,
        ItemPresenter = !Object.op_Equality((Object) itemContainer.ItemPresenter, (Object) null) ? itemContainer.ItemPresenter : ((Component) this).get_gameObject(),
        EditorPresenter = !Object.op_Equality((Object) itemContainer.EditorPresenter, (Object) null) ? itemContainer.EditorPresenter : ((Component) this).get_gameObject()
      });
    }

    protected override void OnItemEndEdit(object sender, EventArgs e)
    {
      if (!this.CanHandleEvent(sender))
        return;
      ItemContainer itemContainer = (ItemContainer) sender;
      if (this.ItemBeginEdit == null)
        return;
      this.ItemEndEdit((object) this, new TDataBindingArgs()
      {
        Item = itemContainer.Item,
        ItemPresenter = !Object.op_Equality((Object) itemContainer.ItemPresenter, (Object) null) ? itemContainer.ItemPresenter : ((Component) this).get_gameObject(),
        EditorPresenter = !Object.op_Equality((Object) itemContainer.EditorPresenter, (Object) null) ? itemContainer.EditorPresenter : ((Component) this).get_gameObject()
      });
    }

    public override void DataBindItem(object item, ItemContainer itemContainer)
    {
      TDataBindingArgs args = new TDataBindingArgs();
      args.Item = item;
      args.ItemPresenter = !Object.op_Equality((Object) itemContainer.ItemPresenter, (Object) null) ? itemContainer.ItemPresenter : ((Component) this).get_gameObject();
      args.EditorPresenter = !Object.op_Equality((Object) itemContainer.EditorPresenter, (Object) null) ? itemContainer.EditorPresenter : ((Component) this).get_gameObject();
      this.RaiseItemDataBinding(args);
      itemContainer.CanEdit = args.CanEdit;
      itemContainer.CanDrag = args.CanDrag;
      itemContainer.CanDrop = args.CanBeParent;
    }

    protected void RaiseItemDataBinding(TDataBindingArgs args)
    {
      if (this.ItemDataBinding == null)
        return;
      this.ItemDataBinding((object) this, args);
    }
  }
}
