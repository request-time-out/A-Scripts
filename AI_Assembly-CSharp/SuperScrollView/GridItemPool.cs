// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridItemPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
  public class GridItemPool
  {
    private int mInitCreateCount = 1;
    private List<LoopGridViewItem> mTmpPooledItemList = new List<LoopGridViewItem>();
    private List<LoopGridViewItem> mPooledItemList = new List<LoopGridViewItem>();
    private GameObject mPrefabObj;
    private string mPrefabName;
    private static int mCurItemIdCount;
    private RectTransform mItemParent;

    public void Init(GameObject prefabObj, int createCount, RectTransform parent)
    {
      this.mPrefabObj = prefabObj;
      this.mPrefabName = ((Object) this.mPrefabObj).get_name();
      this.mInitCreateCount = createCount;
      this.mItemParent = parent;
      this.mPrefabObj.SetActive(false);
      for (int index = 0; index < this.mInitCreateCount; ++index)
        this.RecycleItemReal(this.CreateItem());
    }

    public LoopGridViewItem GetItem()
    {
      ++GridItemPool.mCurItemIdCount;
      LoopGridViewItem loopGridViewItem;
      if (this.mTmpPooledItemList.Count > 0)
      {
        int count = this.mTmpPooledItemList.Count;
        loopGridViewItem = this.mTmpPooledItemList[count - 1];
        this.mTmpPooledItemList.RemoveAt(count - 1);
        ((Component) loopGridViewItem).get_gameObject().SetActive(true);
      }
      else
      {
        int count = this.mPooledItemList.Count;
        if (count == 0)
        {
          loopGridViewItem = this.CreateItem();
        }
        else
        {
          loopGridViewItem = this.mPooledItemList[count - 1];
          this.mPooledItemList.RemoveAt(count - 1);
          ((Component) loopGridViewItem).get_gameObject().SetActive(true);
        }
      }
      loopGridViewItem.ItemId = GridItemPool.mCurItemIdCount;
      return loopGridViewItem;
    }

    public void DestroyAllItem()
    {
      this.ClearTmpRecycledItem();
      int count = this.mPooledItemList.Count;
      for (int index = 0; index < count; ++index)
        Object.DestroyImmediate((Object) ((Component) this.mPooledItemList[index]).get_gameObject());
      this.mPooledItemList.Clear();
    }

    public LoopGridViewItem CreateItem()
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.mPrefabObj, Vector3.get_zero(), Quaternion.get_identity(), (Transform) this.mItemParent);
      gameObject.SetActive(true);
      RectTransform component1 = (RectTransform) gameObject.GetComponent<RectTransform>();
      ((Transform) component1).set_localScale(Vector3.get_one());
      component1.set_anchoredPosition3D(Vector3.get_zero());
      ((Transform) component1).set_localEulerAngles(Vector3.get_zero());
      LoopGridViewItem component2 = (LoopGridViewItem) gameObject.GetComponent<LoopGridViewItem>();
      component2.ItemPrefabName = this.mPrefabName;
      return component2;
    }

    private void RecycleItemReal(LoopGridViewItem item)
    {
      ((Component) item).get_gameObject().SetActive(false);
      this.mPooledItemList.Add(item);
    }

    public void RecycleItem(LoopGridViewItem item)
    {
      item.PrevItem = (LoopGridViewItem) null;
      item.NextItem = (LoopGridViewItem) null;
      this.mTmpPooledItemList.Add(item);
    }

    public void ClearTmpRecycledItem()
    {
      int count = this.mTmpPooledItemList.Count;
      if (count == 0)
        return;
      for (int index = 0; index < count; ++index)
        this.RecycleItemReal(this.mTmpPooledItemList[index]);
      this.mTmpPooledItemList.Clear();
    }
  }
}
