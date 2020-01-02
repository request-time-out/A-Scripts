// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomPushScrollController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  [Serializable]
  public class CustomPushScrollController : MonoBehaviour
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
    private CustomPushScrollController.ScrollData[] scrollerDatas;
    public Action<CustomPushInfo> onPush;

    public CustomPushScrollController()
    {
      base.\u002Ector();
    }

    public void Start()
    {
    }

    public void CreateList(List<CustomPushInfo> _lst)
    {
      this.scrollerDatas = _lst.Select<CustomPushInfo, CustomPushScrollController.ScrollData>((Func<CustomPushInfo, int, CustomPushScrollController.ScrollData>) ((value, idx) => new CustomPushScrollController.ScrollData()
      {
        index = idx,
        info = value
      })).ToArray<CustomPushScrollController.ScrollData>();
      int num = !((IList<CustomPushScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<CustomPushScrollController.ScrollData>() ? this.scrollerDatas.Length / this.countPerRow : 0;
      if (!((IList<CustomPushScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<CustomPushScrollController.ScrollData>() && this.scrollerDatas.Length % this.countPerRow > 0)
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

    public void SetTopLine()
    {
      if (!this.view.IsInit)
        return;
      this.view.MovePanelToItemIndex(0, 0.0f);
    }

    public void SetLine(int _line)
    {
      this.view.MovePanelToItemIndex(_line, 0.0f);
    }

    private void OnClick(CustomPushScrollController.ScrollData _data)
    {
      if (this.onPush == null)
        return;
      this.onPush(_data?.info);
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

    private LoopListViewItem2 OnUpdate(LoopListView2 _view, int _index)
    {
      if (_index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = _view.NewListViewItem(((Object) this.original).get_name());
      CustomPushScrollViewInfo component = (CustomPushScrollViewInfo) ((Component) loopListViewItem2).GetComponent<CustomPushScrollViewInfo>();
      for (int _index1 = 0; _index1 < this.countPerRow; ++_index1)
      {
        CustomPushScrollController.ScrollData data = this.scrollerDatas.SafeGet<CustomPushScrollController.ScrollData>(_index * this.countPerRow + _index1);
        component.SetData(_index1, data?.info, (Action) (() => this.OnClick(data)), new Action<string>(this.OnPointerEnter), new Action(this.OnPointerExit));
      }
      return loopListViewItem2;
    }

    public class ScrollData
    {
      public CustomPushInfo info = new CustomPushInfo();
      public int index;
    }
  }
}
