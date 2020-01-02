// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingTreeViewDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class VirtualizingTreeViewDemo : MonoBehaviour
  {
    public VirtualizingTreeView TreeView;
    public int GameObjectNum;

    public VirtualizingTreeViewDemo()
    {
      base.\u002Ector();
    }

    public static bool IsPrefab(Transform This)
    {
      if (Application.get_isEditor() && !Application.get_isPlaying())
        throw new InvalidOperationException("Does not work in edit mode");
      Scene scene = ((Component) This).get_gameObject().get_scene();
      return ((Scene) ref scene).get_buildIndex() < 0;
    }

    private void Awake()
    {
      for (int index = 0; index < this.GameObjectNum; ++index)
        ((Object) new GameObject()).set_name("Instantiated GameObject" + (object) index);
    }

    private void Start()
    {
      this.TreeView.ItemDataBinding += new EventHandler<VirtualizingTreeViewItemDataBindingArgs>(this.OnItemDataBinding);
      this.TreeView.SelectionChanged += new EventHandler<SelectionChangedArgs>(this.OnSelectionChanged);
      this.TreeView.ItemsRemoved += new EventHandler<ItemsRemovedArgs>(this.OnItemsRemoved);
      this.TreeView.ItemExpanding += new EventHandler<VirtualizingItemExpandingArgs>(this.OnItemExpanding);
      this.TreeView.ItemBeginDrag += new EventHandler<ItemArgs>(this.OnItemBeginDrag);
      this.TreeView.ItemDrop += new EventHandler<ItemDropArgs>(this.OnItemDrop);
      this.TreeView.ItemBeginDrop += new EventHandler<ItemDropCancelArgs>(this.OnItemBeginDrop);
      this.TreeView.ItemEndDrag += new EventHandler<ItemArgs>(this.OnItemEndDrag);
      Scene activeScene = SceneManager.GetActiveScene();
      this.TreeView.Items = (IEnumerable) ((Scene) ref activeScene).GetRootGameObjects();
    }

    private void OnDestroy()
    {
      this.TreeView.ItemDataBinding -= new EventHandler<VirtualizingTreeViewItemDataBindingArgs>(this.OnItemDataBinding);
      this.TreeView.SelectionChanged -= new EventHandler<SelectionChangedArgs>(this.OnSelectionChanged);
      this.TreeView.ItemsRemoved -= new EventHandler<ItemsRemovedArgs>(this.OnItemsRemoved);
      this.TreeView.ItemExpanding -= new EventHandler<VirtualizingItemExpandingArgs>(this.OnItemExpanding);
      this.TreeView.ItemBeginDrag -= new EventHandler<ItemArgs>(this.OnItemBeginDrag);
      this.TreeView.ItemBeginDrop -= new EventHandler<ItemDropCancelArgs>(this.OnItemBeginDrop);
      this.TreeView.ItemDrop -= new EventHandler<ItemDropArgs>(this.OnItemDrop);
      this.TreeView.ItemEndDrag -= new EventHandler<ItemArgs>(this.OnItemEndDrag);
    }

    private void OnItemExpanding(object sender, VirtualizingItemExpandingArgs e)
    {
      GameObject gameObject1 = (GameObject) e.Item;
      if (gameObject1.get_transform().get_childCount() <= 0)
        return;
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < gameObject1.get_transform().get_childCount(); ++index)
      {
        GameObject gameObject2 = ((Component) gameObject1.get_transform().GetChild(index)).get_gameObject();
        gameObjectList.Add(gameObject2);
      }
      e.Children = (IEnumerable) gameObjectList;
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
    }

    private void OnItemsRemoved(object sender, ItemsRemovedArgs e)
    {
      for (int index = 0; index < e.Items.Length; ++index)
      {
        GameObject gameObject = (GameObject) e.Items[index];
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          Object.Destroy((Object) gameObject);
      }
    }

    private void OnItemDataBinding(object sender, VirtualizingTreeViewItemDataBindingArgs e)
    {
      GameObject gameObject = e.Item as GameObject;
      if (!Object.op_Inequality((Object) gameObject, (Object) null))
        return;
      ((Text) e.ItemPresenter.GetComponentInChildren<Text>(true)).set_text(((Object) gameObject).get_name());
      ((Image) e.ItemPresenter.GetComponentsInChildren<Image>()[4]).set_sprite((Sprite) Resources.Load<Sprite>("IconNew"));
      e.HasChildren = gameObject.get_transform().get_childCount() > 0;
    }

    private void OnItemBeginDrag(object sender, ItemArgs e)
    {
    }

    private void OnItemBeginDrop(object sender, ItemDropCancelArgs e)
    {
    }

    private void OnItemEndDrag(object sender, ItemArgs e)
    {
    }

    private void OnItemDrop(object sender, ItemDropArgs e)
    {
      if (e.DropTarget == null)
        return;
      Transform transform1 = ((GameObject) e.DropTarget).get_transform();
      if (e.Action == ItemDropAction.SetLastChild)
      {
        for (int index = 0; index < e.DragItems.Length; ++index)
        {
          Transform transform2 = ((GameObject) e.DragItems[index]).get_transform();
          transform2.SetParent(transform1, true);
          transform2.SetAsLastSibling();
        }
      }
      else if (e.Action == ItemDropAction.SetNextSibling)
      {
        for (int index = e.DragItems.Length - 1; index >= 0; --index)
        {
          Transform transform2 = ((GameObject) e.DragItems[index]).get_transform();
          int siblingIndex1 = transform1.GetSiblingIndex();
          if (Object.op_Inequality((Object) transform2.get_parent(), (Object) transform1.get_parent()))
          {
            transform2.SetParent(transform1.get_parent(), true);
            transform2.SetSiblingIndex(siblingIndex1 + 1);
          }
          else
          {
            int siblingIndex2 = transform2.GetSiblingIndex();
            if (siblingIndex1 < siblingIndex2)
              transform2.SetSiblingIndex(siblingIndex1 + 1);
            else
              transform2.SetSiblingIndex(siblingIndex1);
          }
        }
      }
      else
      {
        if (e.Action != ItemDropAction.SetPrevSibling)
          return;
        for (int index = 0; index < e.DragItems.Length; ++index)
        {
          Transform transform2 = ((GameObject) e.DragItems[index]).get_transform();
          if (Object.op_Inequality((Object) transform2.get_parent(), (Object) transform1.get_parent()))
            transform2.SetParent(transform1.get_parent(), true);
          int siblingIndex1 = transform1.GetSiblingIndex();
          int siblingIndex2 = transform2.GetSiblingIndex();
          if (siblingIndex1 > siblingIndex2)
            transform2.SetSiblingIndex(siblingIndex1 - 1);
          else
            transform2.SetSiblingIndex(siblingIndex1);
        }
      }
    }
  }
}
