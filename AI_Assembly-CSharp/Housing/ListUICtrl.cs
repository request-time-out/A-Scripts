// Decompiled with JetBrains decompiler
// Type: Housing.ListUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Battlehub.UIControls;
using Housing.Command;
using Illusion.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Housing
{
  [Serializable]
  public class ListUICtrl : UIDerived
  {
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    [Header("一覧関係")]
    private VirtualizingTreeView virtualizingTreeView;
    [SerializeField]
    [Header("操作関係")]
    private Button buttonDelete;
    [SerializeField]
    private Button buttonDuplicate;
    [SerializeField]
    private Button buttonFolder;

    public bool Visible
    {
      get
      {
        return (double) this.canvasGroup.get_alpha() != 0.0;
      }
      set
      {
        this.canvasGroup.Enable(value, false);
        if (value)
          return;
        this.virtualizingTreeView.SelectedIndex = -1;
      }
    }

    public bool isOnFuncSkip { get; set; }

    public VirtualizingTreeView VirtualizingTreeView
    {
      get
      {
        return this.virtualizingTreeView;
      }
    }

    public override void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      base.Init(_uiCtrl, _tutorial);
      this.virtualizingTreeView.ItemDataBinding += new EventHandler<VirtualizingTreeViewItemDataBindingArgs>(this.OnItemDataBinding);
      this.virtualizingTreeView.SelectionChanged += new EventHandler<SelectionChangedArgs>(this.OnSelectionChanged);
      this.virtualizingTreeView.ItemsRemoving += new EventHandler<ItemsCancelArgs>(this.OnItemsRemoving);
      this.virtualizingTreeView.ItemsRemoved += new EventHandler<ItemsRemovedArgs>(this.OnItemsRemoved);
      this.virtualizingTreeView.ItemExpanding += new EventHandler<VirtualizingItemExpandingArgs>(this.OnItemExpanding);
      this.virtualizingTreeView.ItemCollapsed += new EventHandler<VirtualizingItemCollapsedArgs>(this.OnItemCollapsed);
      this.virtualizingTreeView.ItemBeginDrag += new EventHandler<ItemArgs>(this.OnItemBeginDrag);
      this.virtualizingTreeView.ItemDrop += new EventHandler<ItemDropArgs>(this.OnItemDrop);
      this.virtualizingTreeView.ItemBeginDrop += new EventHandler<ItemDropCancelArgs>(this.OnItemBeginDrop);
      this.virtualizingTreeView.ItemEndDrag += new EventHandler<ItemArgs>(this.OnItemEndDrag);
      this.virtualizingTreeView.AutoExpand = true;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonDelete), (Action<M0>) (_ => this.virtualizingTreeView.RemoveSelectedItems()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonDuplicate), (Action<M0>) (_ => this.Duplicate()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonFolder), (Action<M0>) (_ => Singleton<UndoRedoManager>.Instance.Do((ICommand) new AddFolderCommand())));
      Singleton<Selection>.Instance.onSelectFunc += new Action<ObjectCtrl[]>(this.OnSelectFunc);
      ((Selectable) this.buttonDelete).set_interactable(false);
      ((Selectable) this.buttonDuplicate).set_interactable(false);
    }

    public override void UpdateUI()
    {
      this.virtualizingTreeView.Items = (IEnumerable) Singleton<Manager.Housing>.Instance.ObjectCtrls;
    }

    public void AddList(ObjectCtrl _objectCtrl)
    {
      this.virtualizingTreeView.Add((object) _objectCtrl);
    }

    public void RemoveList(ObjectCtrl _objectCtrl)
    {
      this.virtualizingTreeView.RemoveChild((object) null, (object) _objectCtrl);
    }

    public void Duplicate()
    {
      IEnumerable selectedItems = this.virtualizingTreeView.SelectedItems;
      if (selectedItems == null)
        return;
      ObjectCtrl[] array = selectedItems.OfType<ObjectCtrl>().Where<ObjectCtrl>((Func<ObjectCtrl, bool>) (v => v != null)).ToArray<ObjectCtrl>();
      if (((IList<ObjectCtrl>) array).IsNullOrEmpty<ObjectCtrl>())
        return;
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new DuplicateCommand(array));
    }

    public void UpdateList(bool _select = true)
    {
      this.virtualizingTreeView.SetItems((IEnumerable) Singleton<Manager.Housing>.Instance.ObjectCtrls, _select);
    }

    public void RefreshList()
    {
      this.virtualizingTreeView.Refresh();
    }

    public void ClearList()
    {
      this.virtualizingTreeView.Items = (IEnumerable) null;
    }

    public void Select(ObjectCtrl _objectCtrl)
    {
      this.ExpandedLoop(_objectCtrl?.Parent);
      this.virtualizingTreeView.SelectedItem = (object) _objectCtrl;
    }

    private void ExpandedLoop(ObjectCtrl _objectCtrl)
    {
      if (_objectCtrl == null)
        return;
      TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) this.virtualizingTreeView.GetItemContainerData((object) _objectCtrl);
      if (itemContainerData == null)
        this.ExpandedLoop(_objectCtrl.Parent);
      else if (itemContainerData.IsExpanded)
        return;
      this.virtualizingTreeView.Expand((object) _objectCtrl);
    }

    private void OnSelectFunc(ObjectCtrl[] _objectCtrls)
    {
      bool flag = ((IList<ObjectCtrl>) _objectCtrls).IsNullOrEmpty<ObjectCtrl>();
      ((Selectable) this.buttonDelete).set_interactable(!flag);
      ((Selectable) this.buttonDuplicate).set_interactable(!flag);
    }

    private void OnItemDataBinding(object sender, VirtualizingTreeViewItemDataBindingArgs e)
    {
      if (!(e.Item is ObjectCtrl objectCtrl))
        return;
      Text componentInChildren = (Text) e.ItemPresenter.GetComponentInChildren<Text>(true);
      componentInChildren.set_text(objectCtrl.Name);
      OCItem ocItem = objectCtrl as OCItem;
      ((Graphic) componentInChildren).set_color(ocItem == null ? Color.get_white() : (!ocItem.IsOverlapNow ? Color.get_white() : Color.get_red()));
      OCFolder ocFolder = objectCtrl as OCFolder;
      e.HasChildren = ocFolder != null && ocFolder.Child.Count > 0;
      e.CanBeParent = ocFolder != null;
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
      if (!((IList<object>) e.OldItems).IsNullOrEmpty<object>())
      {
        foreach (object oldItem in e.OldItems)
        {
          if (oldItem is ObjectCtrl objectCtrl)
            objectCtrl.OnDeselected();
        }
      }
      ObjectCtrl[] array = ((IEnumerable<object>) e.NewItems).Select<object, ObjectCtrl>((Func<object, ObjectCtrl>) (v => v as ObjectCtrl)).Where<ObjectCtrl>((Func<ObjectCtrl, bool>) (v => v != null)).ToArray<ObjectCtrl>();
      foreach (ObjectCtrl objectCtrl in array)
        objectCtrl.OnSelected();
      if (!Singleton<Selection>.IsInstance())
        return;
      Singleton<Selection>.Instance.SetSelectObjects(array);
    }

    private void OnItemsRemoving(object sender, ItemsCancelArgs e)
    {
      HashSet<ObjectCtrl> objectCtrlSet = new HashSet<ObjectCtrl>(e.Items.Select<object, ObjectCtrl>((Func<object, ObjectCtrl>) (v => v as ObjectCtrl)));
      foreach (ObjectCtrl _objectCtrl in e.Items.Select<object, ObjectCtrl>((Func<object, ObjectCtrl>) (v => v as ObjectCtrl)))
        this.CheckParent(objectCtrlSet, _objectCtrl);
      bool isMessage = false;
      e.Items = ((IEnumerable<object>) objectCtrlSet.Where<ObjectCtrl>((Func<ObjectCtrl, bool>) (v =>
      {
        bool flag = v.OnRemoving();
        isMessage |= !flag;
        return flag;
      }))).ToList<object>();
      if (!isMessage)
        return;
      MapUIContainer.PushMessageUI("アイテムがいっぱいです", 2, 1, (Action) (() => {}));
    }

    private void OnItemsRemoved(object sender, ItemsRemovedArgs e)
    {
      HashSet<ObjectCtrl> objectCtrlSet = new HashSet<ObjectCtrl>(((IEnumerable<object>) e.Items).Select<object, ObjectCtrl>((Func<object, ObjectCtrl>) (v => v as ObjectCtrl)));
      foreach (ObjectCtrl _objectCtrl in ((IEnumerable<object>) e.Items).Select<object, ObjectCtrl>((Func<object, ObjectCtrl>) (v => v as ObjectCtrl)))
        this.CheckParent(objectCtrlSet, _objectCtrl);
      DeleteCommand deleteCommand = new DeleteCommand(objectCtrlSet.ToArray<ObjectCtrl>());
      if (!this.isOnFuncSkip)
        Singleton<UndoRedoManager>.Instance.Do((ICommand) deleteCommand);
      else
        deleteCommand.Do();
      this.isOnFuncSkip = false;
      IEnumerable selectedItems = this.virtualizingTreeView.SelectedItems;
      ObjectCtrl[] objectCtrlArray = selectedItems != null ? selectedItems.OfType<ObjectCtrl>().ToArray<ObjectCtrl>() : (ObjectCtrl[]) null;
      if (Singleton<Selection>.IsInstance())
      {
        Action<ObjectCtrl[]> onSelectFunc = Singleton<Selection>.Instance.onSelectFunc;
        if (onSelectFunc != null)
          onSelectFunc(objectCtrlArray);
      }
      this.RefreshList();
    }

    private void CheckParent(HashSet<ObjectCtrl> _objects, ObjectCtrl _objectCtrl)
    {
      if (_objectCtrl == null || _objectCtrl.Parent == null)
        return;
      if (_objects.Contains(_objectCtrl.Parent))
      {
        _objects.Remove(_objectCtrl);
        this.RemoveTargetChild(_objects, _objectCtrl as OCFolder);
      }
      else
        this.CheckParent(_objects, _objectCtrl.Parent);
    }

    private void RemoveTargetChild(HashSet<ObjectCtrl> _objects, OCFolder _ocf)
    {
      if (_ocf == null)
        return;
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in _ocf.Child)
      {
        _objects.Remove(keyValuePair.Value);
        this.RemoveTargetChild(_objects, keyValuePair.Value as OCFolder);
      }
    }

    private void OnItemExpanding(object sender, VirtualizingItemExpandingArgs e)
    {
      if (!(e.Item is OCFolder ocFolder))
        return;
      if (ocFolder.Child.Count > 0)
      {
        List<ObjectCtrl> childObjectCtrls = ocFolder.ChildObjectCtrls;
        e.Children = (IEnumerable) childObjectCtrls;
        e.ChildrenExpand = (IEnumerable) childObjectCtrls.Select<ObjectCtrl, bool>((Func<ObjectCtrl, bool>) (v => v.ObjectInfo is OIFolder && ((OIFolder) v.ObjectInfo).Expand));
      }
      ocFolder.OIFolder.Expand = true;
    }

    private void OnItemCollapsed(object sender, VirtualizingItemCollapsedArgs e)
    {
      if (!(e.Item is OCFolder ocFolder))
        return;
      ocFolder.OIFolder.Expand = false;
    }

    private void OnItemBeginDrag(object sender, ItemArgs e)
    {
    }

    private void OnItemDrop(object sender, ItemDropArgs args)
    {
      if (args.DropTarget == null)
        return;
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new ListDropCommand(args));
    }

    private void OnItemBeginDrop(object sender, ItemDropCancelArgs e)
    {
    }

    private void OnItemEndDrag(object sender, ItemArgs e)
    {
    }

    private List<IObjectInfo> ChildrenOf(ObjectCtrl parent)
    {
      if (parent is OCFolder ocFolder)
        return ocFolder.OIFolder.Child;
      return parent != null ? parent.CraftInfo.ObjectInfos : Singleton<Manager.Housing>.Instance.CraftInfo.ObjectInfos;
    }
  }
}
