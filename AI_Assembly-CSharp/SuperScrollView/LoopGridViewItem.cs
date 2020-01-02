// Decompiled with JetBrains decompiler
// Type: SuperScrollView.LoopGridViewItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SuperScrollView
{
  public class LoopGridViewItem : MonoBehaviour
  {
    private int mItemIndex;
    private int mRow;
    private int mColumn;
    private int mItemId;
    private LoopGridView mParentGridView;
    private bool mIsInitHandlerCalled;
    private string mItemPrefabName;
    private RectTransform mCachedRectTransform;
    private int mItemCreatedCheckFrameCount;
    private object mUserObjectData;
    private int mUserIntData1;
    private int mUserIntData2;
    private string mUserStringData1;
    private string mUserStringData2;
    private LoopGridViewItem mPrevItem;
    private LoopGridViewItem mNextItem;

    public LoopGridViewItem()
    {
      base.\u002Ector();
    }

    public object UserObjectData
    {
      get
      {
        return this.mUserObjectData;
      }
      set
      {
        this.mUserObjectData = value;
      }
    }

    public int UserIntData1
    {
      get
      {
        return this.mUserIntData1;
      }
      set
      {
        this.mUserIntData1 = value;
      }
    }

    public int UserIntData2
    {
      get
      {
        return this.mUserIntData2;
      }
      set
      {
        this.mUserIntData2 = value;
      }
    }

    public string UserStringData1
    {
      get
      {
        return this.mUserStringData1;
      }
      set
      {
        this.mUserStringData1 = value;
      }
    }

    public string UserStringData2
    {
      get
      {
        return this.mUserStringData2;
      }
      set
      {
        this.mUserStringData2 = value;
      }
    }

    public int ItemCreatedCheckFrameCount
    {
      get
      {
        return this.mItemCreatedCheckFrameCount;
      }
      set
      {
        this.mItemCreatedCheckFrameCount = value;
      }
    }

    public RectTransform CachedRectTransform
    {
      get
      {
        if (Object.op_Equality((Object) this.mCachedRectTransform, (Object) null))
          this.mCachedRectTransform = (RectTransform) ((Component) this).get_gameObject().GetComponent<RectTransform>();
        return this.mCachedRectTransform;
      }
    }

    public string ItemPrefabName
    {
      get
      {
        return this.mItemPrefabName;
      }
      set
      {
        this.mItemPrefabName = value;
      }
    }

    public int Row
    {
      get
      {
        return this.mRow;
      }
      set
      {
        this.mRow = value;
      }
    }

    public int Column
    {
      get
      {
        return this.mColumn;
      }
      set
      {
        this.mColumn = value;
      }
    }

    public int ItemIndex
    {
      get
      {
        return this.mItemIndex;
      }
      set
      {
        this.mItemIndex = value;
      }
    }

    public int ItemId
    {
      get
      {
        return this.mItemId;
      }
      set
      {
        this.mItemId = value;
      }
    }

    public bool IsInitHandlerCalled
    {
      get
      {
        return this.mIsInitHandlerCalled;
      }
      set
      {
        this.mIsInitHandlerCalled = value;
      }
    }

    public LoopGridView ParentGridView
    {
      get
      {
        return this.mParentGridView;
      }
      set
      {
        this.mParentGridView = value;
      }
    }

    public LoopGridViewItem PrevItem
    {
      get
      {
        return this.mPrevItem;
      }
      set
      {
        this.mPrevItem = value;
      }
    }

    public LoopGridViewItem NextItem
    {
      get
      {
        return this.mNextItem;
      }
      set
      {
        this.mNextItem = value;
      }
    }
  }
}
