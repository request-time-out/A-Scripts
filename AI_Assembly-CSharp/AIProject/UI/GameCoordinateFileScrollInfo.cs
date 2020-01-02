// Decompiled with JetBrains decompiler
// Type: AIProject.UI.GameCoordinateFileScrollInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject.UI
{
  [Serializable]
  public class GameCoordinateFileScrollInfo
  {
    [SerializeField]
    private int _countPerRow = 3;
    [SerializeField]
    private LoopListView2 _view;
    [SerializeField]
    private GameObject _original;
    private GameCoordinateFileScrollInfo.ScrollData[] _scrollDatas;
    public Action<GameCoordinateFileInfo> OnSelect;
    public Action OnDeselect;

    public GameCoordinateFileScrollInfo.ScrollData SelectData { get; private set; }

    public void Init(List<GameCoordinateFileInfo> lst)
    {
      this._scrollDatas = lst.Select<GameCoordinateFileInfo, GameCoordinateFileScrollInfo.ScrollData>((Func<GameCoordinateFileInfo, int, GameCoordinateFileScrollInfo.ScrollData>) ((value, idx) => new GameCoordinateFileScrollInfo.ScrollData()
      {
        index = idx,
        info = value
      })).ToArray<GameCoordinateFileScrollInfo.ScrollData>();
      int num = !this._scrollDatas.IsNullOrEmpty<GameCoordinateFileScrollInfo.ScrollData>() ? this._scrollDatas.Length / this._countPerRow : 0;
      if (!this._scrollDatas.IsNullOrEmpty<GameCoordinateFileScrollInfo.ScrollData>() && this._scrollDatas.Length % this._countPerRow > 0)
        ++num;
      if (!this._view.IsInit)
        this._view.InitListViewAndSize(num, new Func<LoopListView2, int, LoopListViewItem2>(this.OnUpdate));
      else
        this._view.ReSetListItemCount(num);
    }

    public void SelectDataClear()
    {
      this.SelectData = (GameCoordinateFileScrollInfo.ScrollData) null;
    }

    public void SetTopLine()
    {
      if (!this._view.IsInit)
        return;
      this._view.MovePanelToItemIndex(0, 0.0f);
    }

    public void SetToggle(int index)
    {
      this.SelectData = this._scrollDatas[index];
      this._view.RefreshAllShownItem();
    }

    public void SetNowSelectToggle()
    {
      for (int index1 = 0; index1 < this._view.ShownItemCount; ++index1)
      {
        LoopListViewItem2 shownItemByIndex = this._view.GetShownItemByIndex(index1);
        if (!Object.op_Equality((Object) shownItemByIndex, (Object) null))
        {
          GameCoordinateFileInfoComponent component = (GameCoordinateFileInfoComponent) ((Component) shownItemByIndex).GetComponent<GameCoordinateFileInfoComponent>();
          for (int index2 = 0; index2 < this._countPerRow; ++index2)
            component.SetToggleOn(index2, this.IsNowSelectInfo(component.GetListInfo(index2)));
        }
      }
    }

    public void SetLine(int line)
    {
      this._view.MovePanelToItemIndex(line, 0.0f);
    }

    public void SetNowLine()
    {
      int itemIndex = 0;
      if (this.SelectData != null)
        itemIndex = this.SelectData.index / this._countPerRow;
      this._view.MovePanelToItemIndex(itemIndex, 0.0f);
    }

    private void OnValueChange(GameCoordinateFileScrollInfo.ScrollData data, bool isOn)
    {
      if (isOn)
      {
        bool flag = !this.IsNowSelectInfo(data?.info);
        this.SelectData = data;
        if (!flag)
          return;
        for (int index1 = 0; index1 < this._view.ShownItemCount; ++index1)
        {
          LoopListViewItem2 shownItemByIndex = this._view.GetShownItemByIndex(index1);
          if (!Object.op_Equality((Object) shownItemByIndex, (Object) null))
          {
            GameCoordinateFileInfoComponent component = (GameCoordinateFileInfoComponent) ((Component) shownItemByIndex).GetComponent<GameCoordinateFileInfoComponent>();
            for (int index2 = 0; index2 < this._countPerRow; ++index2)
            {
              if (!this.IsNowSelectInfo(component.GetListInfo(index2)))
                component.SetToggleOn(index2, false);
            }
          }
        }
        if (this.OnSelect == null)
          return;
        this.OnSelect(this.SelectData.info);
      }
      else
      {
        if (!this.IsNowSelectInfo(data?.info))
          return;
        this.SelectData = (GameCoordinateFileScrollInfo.ScrollData) null;
        if (this.OnDeselect == null)
          return;
        this.OnDeselect();
      }
    }

    private bool IsNowSelectInfo(GameCoordinateFileInfo info)
    {
      return info != null && this.SelectData != null && this.SelectData.info.FullPath == info.FullPath;
    }

    private LoopListViewItem2 OnUpdate(LoopListView2 view, int index)
    {
      if (index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = view.NewListViewItem(((Object) this._original).get_name());
      GameCoordinateFileInfoComponent component = (GameCoordinateFileInfoComponent) ((Component) loopListViewItem2).GetComponent<GameCoordinateFileInfoComponent>();
      for (int index1 = 0; index1 < this._countPerRow; ++index1)
      {
        GameCoordinateFileScrollInfo.ScrollData data = this._scrollDatas.SafeGet<GameCoordinateFileScrollInfo.ScrollData>(index * this._countPerRow + index1);
        component.SetData(index1, data?.info, (Action<bool>) (isOn => this.OnValueChange(data, isOn)));
        component.SetToggleOn(index1, this.IsNowSelectInfo(data?.info));
      }
      return loopListViewItem2;
    }

    public class ScrollData
    {
      public int index;
      public GameCoordinateFileInfo info;
    }
  }
}
