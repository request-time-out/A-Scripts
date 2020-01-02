// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomSelectScrollController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  [Serializable]
  public class CustomSelectScrollController : MonoBehaviour
  {
    [SerializeField]
    private LoopListView2 view;
    [SerializeField]
    private GameObject original;
    [SerializeField]
    private int countPerRow;
    [SerializeField]
    private Text text;
    private string SelectName;
    private CustomSelectScrollController.ScrollData[] scrollerDatas;
    public Action<CustomSelectInfo> onSelect;
    public Action onDeSelect;

    public CustomSelectScrollController()
    {
      base.\u002Ector();
    }

    public CustomSelectScrollController.ScrollData selectInfo { get; private set; }

    public void Start()
    {
    }

    public void CreateList(List<CustomSelectInfo> _lst)
    {
      this.scrollerDatas = _lst.Select<CustomSelectInfo, CustomSelectScrollController.ScrollData>((Func<CustomSelectInfo, int, CustomSelectScrollController.ScrollData>) ((value, idx) => new CustomSelectScrollController.ScrollData()
      {
        index = idx,
        info = value
      })).ToArray<CustomSelectScrollController.ScrollData>();
      int num = !((IList<CustomSelectScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<CustomSelectScrollController.ScrollData>() ? this.scrollerDatas.Length / this.countPerRow : 0;
      if (!((IList<CustomSelectScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<CustomSelectScrollController.ScrollData>() && this.scrollerDatas.Length % this.countPerRow > 0)
        ++num;
      if (!this.view.IsInit)
        this.view.InitListViewAndSize(num, new Func<LoopListView2, int, LoopListViewItem2>(this.OnUpdate));
      else
        this.view.ReSetListItemCount(num);
      this.SelectName = string.Empty;
      if (!Object.op_Implicit((Object) this.text))
        return;
      this.text.set_text(string.Empty);
    }

    public void SelectInfoClear()
    {
      this.selectInfo = (CustomSelectScrollController.ScrollData) null;
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

    public void SetToggleID(int _id)
    {
      CustomSelectScrollController.ScrollData scrollData = ((IEnumerable<CustomSelectScrollController.ScrollData>) this.scrollerDatas).FirstOrDefault<CustomSelectScrollController.ScrollData>((Func<CustomSelectScrollController.ScrollData, bool>) (x => x.info.id == _id));
      if (scrollData == null)
        return;
      this.selectInfo = scrollData;
      this.view.RefreshAllShownItem();
    }

    private void SetNowSelectToggle()
    {
      for (int index = 0; index < this.view.ShownItemCount; ++index)
      {
        LoopListViewItem2 shownItemByIndex = this.view.GetShownItemByIndex(index);
        if (!Object.op_Equality((Object) shownItemByIndex, (Object) null))
        {
          CustomSelectScrollViewInfo component = (CustomSelectScrollViewInfo) ((Component) shownItemByIndex).GetComponent<CustomSelectScrollViewInfo>();
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

    private void OnValueChange(CustomSelectScrollController.ScrollData _data, bool _isOn)
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
            CustomSelectScrollViewInfo component = (CustomSelectScrollViewInfo) ((Component) shownItemByIndex).GetComponent<CustomSelectScrollViewInfo>();
            for (int _index = 0; _index < this.countPerRow; ++_index)
            {
              if (!this.IsNowSelectInfo(component.GetListInfo(_index)))
                component.SetToggleON(_index, false);
              else
                component.SetNewFlagOff(_index);
            }
          }
        }
        if (this.onSelect != null)
          this.onSelect(this.selectInfo.info);
        if (this.selectInfo == null)
          return;
        this.SelectName = this.selectInfo.info.name;
      }
      else
      {
        if (!this.IsNowSelectInfo(_data?.info))
          return;
        _data.toggle.SetIsOnWithoutCallback(true);
      }
    }

    private void OnPointerEnter(string name)
    {
      if (!Object.op_Implicit((Object) this.text))
        return;
      this.text.set_text(name);
    }

    private void OnPointerExit()
    {
      if (!Object.op_Implicit((Object) this.text))
        return;
      this.text.set_text(this.SelectName);
    }

    private bool IsNowSelectInfo(CustomSelectInfo _info)
    {
      return _info != null && this.selectInfo != null && this.selectInfo.info == _info;
    }

    private LoopListViewItem2 OnUpdate(LoopListView2 _view, int _index)
    {
      if (_index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = _view.NewListViewItem(((Object) this.original).get_name());
      CustomSelectScrollViewInfo component = (CustomSelectScrollViewInfo) ((Component) loopListViewItem2).GetComponent<CustomSelectScrollViewInfo>();
      for (int _index1 = 0; _index1 < this.countPerRow; ++_index1)
      {
        CustomSelectScrollController.ScrollData data = this.scrollerDatas.SafeGet<CustomSelectScrollController.ScrollData>(_index * this.countPerRow + _index1);
        component.SetData(_index1, data, (Action<bool>) (_isOn => this.OnValueChange(data, _isOn)), new Action<string>(this.OnPointerEnter), new Action(this.OnPointerExit));
        component.SetToggleON(_index1, this.IsNowSelectInfo(data?.info));
      }
      return loopListViewItem2;
    }

    public class ScrollData
    {
      public CustomSelectInfo info = new CustomSelectInfo();
      public int index;
      public Toggle toggle;
    }
  }
}
