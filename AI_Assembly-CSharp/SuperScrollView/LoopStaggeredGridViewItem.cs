// Decompiled with JetBrains decompiler
// Type: SuperScrollView.LoopStaggeredGridViewItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SuperScrollView
{
  public class LoopStaggeredGridViewItem : MonoBehaviour
  {
    private int mItemIndex;
    private int mItemIndexInGroup;
    private int mItemId;
    private float mPadding;
    private float mExtraPadding;
    private bool mIsInitHandlerCalled;
    private string mItemPrefabName;
    private RectTransform mCachedRectTransform;
    private LoopStaggeredGridView mParentListView;
    private float mDistanceWithViewPortSnapCenter;
    private int mItemCreatedCheckFrameCount;
    private float mStartPosOffset;
    private object mUserObjectData;
    private int mUserIntData1;
    private int mUserIntData2;
    private string mUserStringData1;
    private string mUserStringData2;

    public LoopStaggeredGridViewItem()
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

    public float DistanceWithViewPortSnapCenter
    {
      get
      {
        return this.mDistanceWithViewPortSnapCenter;
      }
      set
      {
        this.mDistanceWithViewPortSnapCenter = value;
      }
    }

    public float StartPosOffset
    {
      get
      {
        return this.mStartPosOffset;
      }
      set
      {
        this.mStartPosOffset = value;
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

    public float Padding
    {
      get
      {
        return this.mPadding;
      }
      set
      {
        this.mPadding = value;
      }
    }

    public float ExtraPadding
    {
      get
      {
        return this.mExtraPadding;
      }
      set
      {
        this.mExtraPadding = value;
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

    public int ItemIndexInGroup
    {
      get
      {
        return this.mItemIndexInGroup;
      }
      set
      {
        this.mItemIndexInGroup = value;
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

    public LoopStaggeredGridView ParentListView
    {
      get
      {
        return this.mParentListView;
      }
      set
      {
        this.mParentListView = value;
      }
    }

    public float TopY
    {
      get
      {
        switch (this.ParentListView.ArrangeType)
        {
          case ListItemArrangeType.TopToBottom:
            return (float) this.CachedRectTransform.get_anchoredPosition3D().y;
          case ListItemArrangeType.BottomToTop:
            __Null y = this.CachedRectTransform.get_anchoredPosition3D().y;
            Rect rect = this.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            return (float) (y + height);
          default:
            return 0.0f;
        }
      }
    }

    public float BottomY
    {
      get
      {
        switch (this.ParentListView.ArrangeType)
        {
          case ListItemArrangeType.TopToBottom:
            __Null y = this.CachedRectTransform.get_anchoredPosition3D().y;
            Rect rect = this.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            return (float) (y - height);
          case ListItemArrangeType.BottomToTop:
            return (float) this.CachedRectTransform.get_anchoredPosition3D().y;
          default:
            return 0.0f;
        }
      }
    }

    public float LeftX
    {
      get
      {
        switch (this.ParentListView.ArrangeType)
        {
          case ListItemArrangeType.LeftToRight:
            return (float) this.CachedRectTransform.get_anchoredPosition3D().x;
          case ListItemArrangeType.RightToLeft:
            __Null x = this.CachedRectTransform.get_anchoredPosition3D().x;
            Rect rect = this.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            return (float) (x - width);
          default:
            return 0.0f;
        }
      }
    }

    public float RightX
    {
      get
      {
        switch (this.ParentListView.ArrangeType)
        {
          case ListItemArrangeType.LeftToRight:
            __Null x = this.CachedRectTransform.get_anchoredPosition3D().x;
            Rect rect = this.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            return (float) (x + width);
          case ListItemArrangeType.RightToLeft:
            return (float) this.CachedRectTransform.get_anchoredPosition3D().x;
          default:
            return 0.0f;
        }
      }
    }

    public float ItemSize
    {
      get
      {
        if (this.ParentListView.IsVertList)
        {
          Rect rect = this.CachedRectTransform.get_rect();
          return ((Rect) ref rect).get_height();
        }
        Rect rect1 = this.CachedRectTransform.get_rect();
        return ((Rect) ref rect1).get_width();
      }
    }

    public float ItemSizeWithPadding
    {
      get
      {
        return this.ItemSize + this.mPadding;
      }
    }
  }
}
