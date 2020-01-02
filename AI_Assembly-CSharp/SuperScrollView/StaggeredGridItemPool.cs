// Decompiled with JetBrains decompiler
// Type: SuperScrollView.StaggeredGridItemPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
  public class StaggeredGridItemPool
  {
    private int mInitCreateCount = 1;
    private List<LoopStaggeredGridViewItem> mTmpPooledItemList = new List<LoopStaggeredGridViewItem>();
    private List<LoopStaggeredGridViewItem> mPooledItemList = new List<LoopStaggeredGridViewItem>();
    private GameObject mPrefabObj;
    private string mPrefabName;
    private float mPadding;
    private static int mCurItemIdCount;
    private RectTransform mItemParent;

    public void Init(GameObject prefabObj, float padding, int createCount, RectTransform parent)
    {
      this.mPrefabObj = prefabObj;
      this.mPrefabName = ((Object) this.mPrefabObj).get_name();
      this.mInitCreateCount = createCount;
      this.mPadding = padding;
      this.mItemParent = parent;
      this.mPrefabObj.SetActive(false);
      for (int index = 0; index < this.mInitCreateCount; ++index)
        this.RecycleItemReal(this.CreateItem());
    }

    public LoopStaggeredGridViewItem GetItem()
    {
      ++StaggeredGridItemPool.mCurItemIdCount;
      LoopStaggeredGridViewItem staggeredGridViewItem;
      if (this.mTmpPooledItemList.Count > 0)
      {
        int count = this.mTmpPooledItemList.Count;
        staggeredGridViewItem = this.mTmpPooledItemList[count - 1];
        this.mTmpPooledItemList.RemoveAt(count - 1);
        ((Component) staggeredGridViewItem).get_gameObject().SetActive(true);
      }
      else
      {
        int count = this.mPooledItemList.Count;
        if (count == 0)
        {
          staggeredGridViewItem = this.CreateItem();
        }
        else
        {
          staggeredGridViewItem = this.mPooledItemList[count - 1];
          this.mPooledItemList.RemoveAt(count - 1);
          ((Component) staggeredGridViewItem).get_gameObject().SetActive(true);
        }
      }
      staggeredGridViewItem.Padding = this.mPadding;
      staggeredGridViewItem.ItemId = StaggeredGridItemPool.mCurItemIdCount;
      return staggeredGridViewItem;
    }

    public void DestroyAllItem()
    {
      this.ClearTmpRecycledItem();
      int count = this.mPooledItemList.Count;
      for (int index = 0; index < count; ++index)
        Object.DestroyImmediate((Object) ((Component) this.mPooledItemList[index]).get_gameObject());
      this.mPooledItemList.Clear();
    }

    public LoopStaggeredGridViewItem CreateItem()
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.mPrefabObj, Vector3.get_zero(), Quaternion.get_identity(), (Transform) this.mItemParent);
      gameObject.SetActive(true);
      RectTransform component1 = (RectTransform) gameObject.GetComponent<RectTransform>();
      ((Transform) component1).set_localScale(Vector3.get_one());
      component1.set_anchoredPosition3D(Vector3.get_zero());
      ((Transform) component1).set_localEulerAngles(Vector3.get_zero());
      LoopStaggeredGridViewItem component2 = (LoopStaggeredGridViewItem) gameObject.GetComponent<LoopStaggeredGridViewItem>();
      component2.ItemPrefabName = this.mPrefabName;
      component2.StartPosOffset = 0.0f;
      return component2;
    }

    private void RecycleItemReal(LoopStaggeredGridViewItem item)
    {
      ((Component) item).get_gameObject().SetActive(false);
      this.mPooledItemList.Add(item);
    }

    public void RecycleItem(LoopStaggeredGridViewItem item)
    {
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
