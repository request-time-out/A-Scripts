// Decompiled with JetBrains decompiler
// Type: GameLoadCharaFileSystem.GameCharaFileScrollController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLoadCharaFileSystem
{
  [Serializable]
  public class GameCharaFileScrollController
  {
    [SerializeField]
    private int countPerRow = 3;
    [SerializeField]
    private LoopListView2 view;
    [SerializeField]
    private GameObject original;
    private GameCharaFileScrollController.ScrollData[] scrollerDatas;
    public Action<GameCharaFileInfo> onSelect;
    public Action onDeSelect;

    public GameCharaFileScrollController.ScrollData selectInfo { get; private set; }

    public void Init(List<GameCharaFileInfo> _lst)
    {
      this.scrollerDatas = _lst.Select<GameCharaFileInfo, GameCharaFileScrollController.ScrollData>((Func<GameCharaFileInfo, int, GameCharaFileScrollController.ScrollData>) ((value, idx) => new GameCharaFileScrollController.ScrollData()
      {
        index = idx,
        info = value
      })).ToArray<GameCharaFileScrollController.ScrollData>();
      int num = !((IList<GameCharaFileScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<GameCharaFileScrollController.ScrollData>() ? this.scrollerDatas.Length / this.countPerRow : 0;
      if (!((IList<GameCharaFileScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<GameCharaFileScrollController.ScrollData>() && this.scrollerDatas.Length % this.countPerRow > 0)
        ++num;
      if (!this.view.IsInit)
        this.view.InitListViewAndSize(num, new Func<LoopListView2, int, LoopListViewItem2>(this.OnUpdate));
      else
        this.view.ReSetListItemCount(num);
    }

    public void SelectInfoClear()
    {
      this.selectInfo = (GameCharaFileScrollController.ScrollData) null;
    }

    public void SetTopLine()
    {
      if (!this.view.IsInit)
        return;
      this.view.MovePanelToItemIndex(0, 0.0f);
    }

    public void SetToggle(int _index)
    {
      this.selectInfo = this.scrollerDatas[_index];
      this.view.RefreshAllShownItem();
    }

    public void SetNowSelectToggle()
    {
      for (int index = 0; index < this.view.ShownItemCount; ++index)
      {
        LoopListViewItem2 shownItemByIndex = this.view.GetShownItemByIndex(index);
        if (!Object.op_Equality((Object) shownItemByIndex, (Object) null))
        {
          GameCharaFileInfoComponent component = (GameCharaFileInfoComponent) ((Component) shownItemByIndex).GetComponent<GameCharaFileInfoComponent>();
          for (int _index = 0; _index < this.countPerRow; ++_index)
            component.SetToggleON(_index, this.IsNowSelectInfo(component.GetListInfo(_index)));
        }
      }
    }

    public void SetLine(int _line)
    {
      this.view.MovePanelToItemIndex(_line, 0.0f);
    }

    public void SetNowLine()
    {
      int itemIndex = 0;
      if (this.selectInfo != null)
        itemIndex = this.selectInfo.index / this.countPerRow;
      this.view.MovePanelToItemIndex(itemIndex, 0.0f);
    }

    private void OnValueChange(GameCharaFileScrollController.ScrollData _data, bool _isOn)
    {
      if (_isOn)
      {
        bool flag = !this.IsNowSelectInfo(_data?.info);
        this.selectInfo = _data;
        if (!flag)
          return;
        for (int index = 0; index < this.view.ShownItemCount; ++index)
        {
          LoopListViewItem2 shownItemByIndex = this.view.GetShownItemByIndex(index);
          if (!Object.op_Equality((Object) shownItemByIndex, (Object) null))
          {
            GameCharaFileInfoComponent component = (GameCharaFileInfoComponent) ((Component) shownItemByIndex).GetComponent<GameCharaFileInfoComponent>();
            for (int _index = 0; _index < this.countPerRow; ++_index)
            {
              if (!this.IsNowSelectInfo(component.GetListInfo(_index)))
                component.SetToggleON(_index, false);
            }
          }
        }
        this.onSelect(this.selectInfo.info);
      }
      else
      {
        if (!this.IsNowSelectInfo(_data?.info))
          return;
        this.selectInfo = (GameCharaFileScrollController.ScrollData) null;
        if (this.onDeSelect == null)
          return;
        this.onDeSelect();
      }
    }

    private bool IsNowSelectInfo(GameCharaFileInfo _info)
    {
      return _info != null && this.selectInfo != null && this.selectInfo.info.FullPath == _info.FullPath;
    }

    private LoopListViewItem2 OnUpdate(LoopListView2 _view, int _index)
    {
      if (_index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = _view.NewListViewItem(((Object) this.original).get_name());
      GameCharaFileInfoComponent component = (GameCharaFileInfoComponent) ((Component) loopListViewItem2).GetComponent<GameCharaFileInfoComponent>();
      for (int _index1 = 0; _index1 < this.countPerRow; ++_index1)
      {
        GameCharaFileScrollController.ScrollData data = this.scrollerDatas.SafeGet<GameCharaFileScrollController.ScrollData>(_index * this.countPerRow + _index1);
        component.SetData(_index1, data?.info, (Action<bool>) (_isOn => this.OnValueChange(data, _isOn)));
        component.SetToggleON(_index1, this.IsNowSelectInfo(data?.info));
      }
      return loopListViewItem2;
    }

    public class ScrollData
    {
      public int index;
      public GameCharaFileInfo info;
    }
  }
}
