// Decompiled with JetBrains decompiler
// Type: SuperScrollView.TreeViewDataSourceMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
  public class TreeViewDataSourceMgr : MonoBehaviour
  {
    private List<TreeViewItemData> mItemDataList;
    private static TreeViewDataSourceMgr instance;
    private int mTreeViewItemCount;
    private int mTreeViewChildItemCount;

    public TreeViewDataSourceMgr()
    {
      base.\u002Ector();
    }

    public static TreeViewDataSourceMgr Get
    {
      get
      {
        if (Object.op_Equality((Object) TreeViewDataSourceMgr.instance, (Object) null))
          TreeViewDataSourceMgr.instance = (TreeViewDataSourceMgr) Object.FindObjectOfType<TreeViewDataSourceMgr>();
        return TreeViewDataSourceMgr.instance;
      }
    }

    private void Awake()
    {
      this.Init();
    }

    public void Init()
    {
      this.DoRefreshDataSource();
    }

    public TreeViewItemData GetItemDataByIndex(int index)
    {
      return index < 0 || index >= this.mItemDataList.Count ? (TreeViewItemData) null : this.mItemDataList[index];
    }

    public ItemData GetItemChildDataByIndex(int itemIndex, int childIndex)
    {
      return this.GetItemDataByIndex(itemIndex)?.GetChild(childIndex);
    }

    public int TreeViewItemCount
    {
      get
      {
        return this.mItemDataList.Count;
      }
    }

    public int TotalTreeViewItemAndChildCount
    {
      get
      {
        int count = this.mItemDataList.Count;
        int num = 0;
        for (int index = 0; index < count; ++index)
          num += this.mItemDataList[index].ChildCount;
        return num;
      }
    }

    private void DoRefreshDataSource()
    {
      this.mItemDataList.Clear();
      for (int index1 = 0; index1 < this.mTreeViewItemCount; ++index1)
      {
        TreeViewItemData treeViewItemData = new TreeViewItemData();
        treeViewItemData.mName = "Item" + (object) index1;
        treeViewItemData.mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24));
        this.mItemDataList.Add(treeViewItemData);
        int viewChildItemCount = this.mTreeViewChildItemCount;
        for (int index2 = 1; index2 <= viewChildItemCount; ++index2)
        {
          ItemData data = new ItemData()
          {
            mName = "Item" + (object) index1 + ":Child" + (object) index2
          };
          data.mDesc = "Item Desc For " + data.mName;
          data.mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24));
          data.mStarCount = Random.Range(0, 6);
          data.mFileSize = Random.Range(20, 999);
          treeViewItemData.AddChild(data);
        }
      }
    }
  }
}
