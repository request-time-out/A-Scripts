// Decompiled with JetBrains decompiler
// Type: SuperScrollView.DataSourceMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
  public class DataSourceMgr : MonoBehaviour
  {
    private List<ItemData> mItemDataList;
    private Action mOnRefreshFinished;
    private Action mOnLoadMoreFinished;
    private int mLoadMoreCount;
    private float mDataLoadLeftTime;
    private float mDataRefreshLeftTime;
    private bool mIsWaittingRefreshData;
    private bool mIsWaitLoadingMoreData;
    public int mTotalDataCount;
    private static DataSourceMgr instance;

    public DataSourceMgr()
    {
      base.\u002Ector();
    }

    public static DataSourceMgr Get
    {
      get
      {
        if (Object.op_Equality((Object) DataSourceMgr.instance, (Object) null))
          DataSourceMgr.instance = (DataSourceMgr) Object.FindObjectOfType<DataSourceMgr>();
        return DataSourceMgr.instance;
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

    public ItemData GetItemDataByIndex(int index)
    {
      return index < 0 || index >= this.mItemDataList.Count ? (ItemData) null : this.mItemDataList[index];
    }

    public ItemData GetItemDataById(int itemId)
    {
      int count = this.mItemDataList.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.mItemDataList[index].mId == itemId)
          return this.mItemDataList[index];
      }
      return (ItemData) null;
    }

    public int TotalItemCount
    {
      get
      {
        return this.mItemDataList.Count;
      }
    }

    public void RequestRefreshDataList(Action onReflushFinished)
    {
      this.mDataRefreshLeftTime = 1f;
      this.mOnRefreshFinished = onReflushFinished;
      this.mIsWaittingRefreshData = true;
    }

    public void RequestLoadMoreDataList(int loadCount, Action onLoadMoreFinished)
    {
      this.mLoadMoreCount = loadCount;
      this.mDataLoadLeftTime = 1f;
      this.mOnLoadMoreFinished = onLoadMoreFinished;
      this.mIsWaitLoadingMoreData = true;
    }

    public void Update()
    {
      if (this.mIsWaittingRefreshData)
      {
        this.mDataRefreshLeftTime -= Time.get_deltaTime();
        if ((double) this.mDataRefreshLeftTime <= 0.0)
        {
          this.mIsWaittingRefreshData = false;
          this.DoRefreshDataSource();
          if (this.mOnRefreshFinished != null)
            this.mOnRefreshFinished();
        }
      }
      if (!this.mIsWaitLoadingMoreData)
        return;
      this.mDataLoadLeftTime -= Time.get_deltaTime();
      if ((double) this.mDataLoadLeftTime > 0.0)
        return;
      this.mIsWaitLoadingMoreData = false;
      this.DoLoadMoreDataSource();
      if (this.mOnLoadMoreFinished == null)
        return;
      this.mOnLoadMoreFinished();
    }

    public void SetDataTotalCount(int count)
    {
      this.mTotalDataCount = count;
      this.DoRefreshDataSource();
    }

    public void ExchangeData(int index1, int index2)
    {
      ItemData mItemData1 = this.mItemDataList[index1];
      ItemData mItemData2 = this.mItemDataList[index2];
      this.mItemDataList[index1] = mItemData2;
      this.mItemDataList[index2] = mItemData1;
    }

    public void RemoveData(int index)
    {
      this.mItemDataList.RemoveAt(index);
    }

    public void InsertData(int index, ItemData data)
    {
      this.mItemDataList.Insert(index, data);
    }

    private void DoRefreshDataSource()
    {
      this.mItemDataList.Clear();
      for (int index = 0; index < this.mTotalDataCount; ++index)
        this.mItemDataList.Add(new ItemData()
        {
          mId = index,
          mName = "Item" + (object) index,
          mDesc = "Item Desc For Item " + (object) index,
          mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24)),
          mStarCount = Random.Range(0, 6),
          mFileSize = Random.Range(20, 999),
          mChecked = false,
          mIsExpand = false
        });
    }

    private void DoLoadMoreDataSource()
    {
      int count = this.mItemDataList.Count;
      for (int index = 0; index < this.mLoadMoreCount; ++index)
      {
        int num = index + count;
        this.mItemDataList.Add(new ItemData()
        {
          mId = num,
          mName = "Item" + (object) num,
          mDesc = "Item Desc For Item " + (object) num,
          mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24)),
          mStarCount = Random.Range(0, 6),
          mFileSize = Random.Range(20, 999),
          mChecked = false,
          mIsExpand = false
        });
      }
      this.mTotalDataCount = this.mItemDataList.Count;
    }

    public void CheckAllItem()
    {
      int count = this.mItemDataList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemDataList[index].mChecked = true;
    }

    public void UnCheckAllItem()
    {
      int count = this.mItemDataList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemDataList[index].mChecked = false;
    }

    public bool DeleteAllCheckedItem()
    {
      int count = this.mItemDataList.Count;
      this.mItemDataList.RemoveAll((Predicate<ItemData>) (it => it.mChecked));
      return count != this.mItemDataList.Count;
    }
  }
}
