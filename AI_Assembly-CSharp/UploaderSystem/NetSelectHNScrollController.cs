// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetSelectHNScrollController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UploaderSystem
{
  [Serializable]
  public class NetSelectHNScrollController : MonoBehaviour
  {
    public DownUIControl uiCtrl;
    [SerializeField]
    private Button btnClose;
    [SerializeField]
    private LoopListView2 view;
    [SerializeField]
    private GameObject original;
    [SerializeField]
    private int countPerRow;
    [SerializeField]
    private Text text;
    private NetSelectHNScrollController.ScrollData[] scrollerDatas;
    private bool noProc;
    private bool skip;

    public NetSelectHNScrollController()
    {
      base.\u002Ector();
    }

    private NetworkInfo netInfo
    {
      get
      {
        return Singleton<NetworkInfo>.Instance;
      }
    }

    public NetSelectHNScrollController.ScrollData selectInfo { get; private set; }

    public void Start()
    {
      if (!Object.op_Implicit((Object) this.btnClose))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnClose), (Action<M0>) (push =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        this.ShowSelectHNWindow(false);
      }));
    }

    public void Init()
    {
      List<NetSelectHNScrollController.ScrollData> source = new List<NetSelectHNScrollController.ScrollData>();
      foreach (KeyValuePair<int, NetworkInfo.UserInfo> keyValuePair in this.netInfo.dictUserInfo)
      {
        KeyValuePair<int, NetworkInfo.UserInfo> n = keyValuePair;
        int num = this.netInfo.lstCharaInfo.Where<NetworkInfo.CharaInfo>((Func<NetworkInfo.CharaInfo, bool>) (x => x.user_idx == n.Key)).ToArray<NetworkInfo.CharaInfo>().Length + this.netInfo.lstHousingInfo.Where<NetworkInfo.HousingInfo>((Func<NetworkInfo.HousingInfo, bool>) (x => x.user_idx == n.Key)).ToArray<NetworkInfo.HousingInfo>().Length;
        if (num != 0)
          source.Add(new NetSelectHNScrollController.ScrollData()
          {
            info = {
              userIdx = n.Key,
              drawname = TextCorrectLimit.CorrectString(this.text, string.Format("({0}) {1}", (object) num, (object) n.Value.handleName), "…"),
              handlename = n.Value.handleName
            }
          });
      }
      using (new GameSystem.CultureScope())
        source = source.OrderBy<NetSelectHNScrollController.ScrollData, string>((Func<NetSelectHNScrollController.ScrollData, string>) (n => n.info.handlename)).ToList<NetSelectHNScrollController.ScrollData>();
      this.scrollerDatas = source.ToArray();
      int num1 = !((IList<NetSelectHNScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<NetSelectHNScrollController.ScrollData>() ? this.scrollerDatas.Length / this.countPerRow : 0;
      if (!((IList<NetSelectHNScrollController.ScrollData>) this.scrollerDatas).IsNullOrEmpty<NetSelectHNScrollController.ScrollData>() && this.scrollerDatas.Length % this.countPerRow > 0)
        ++num1;
      if (!this.view.IsInit)
      {
        this.view.InitListView(num1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnUpdate), (LoopListViewInitParam) null);
      }
      else
      {
        if (this.view.SetListItemCount(num1, true))
          return;
        this.view.RefreshAllShownItem();
      }
    }

    public void ShowSelectHNWindow(bool show)
    {
      if (show)
      {
        int hnidx = this.uiCtrl.searchSortHNIdx;
        this.noProc = true;
        this.selectInfo = hnidx != -1 ? ((IEnumerable<NetSelectHNScrollController.ScrollData>) this.scrollerDatas).FirstOrDefault<NetSelectHNScrollController.ScrollData>((Func<NetSelectHNScrollController.ScrollData, bool>) (d => d.info.userIdx == hnidx)) : (NetSelectHNScrollController.ScrollData) null;
        this.SetNowSelectToggle();
        this.noProc = false;
        ((Component) this).get_gameObject().SetActive(true);
      }
      else
        ((Component) this).get_gameObject().SetActive(false);
    }

    private void SetNowSelectToggle()
    {
      for (int index1 = 0; index1 < this.view.ShownItemCount; ++index1)
      {
        LoopListViewItem2 shownItemByIndex = this.view.GetShownItemByIndex(index1);
        if (!Object.op_Equality((Object) shownItemByIndex, (Object) null))
        {
          NetSelectHNScrollViewInfo component = (NetSelectHNScrollViewInfo) ((Component) shownItemByIndex).GetComponent<NetSelectHNScrollViewInfo>();
          for (int index2 = 0; index2 < this.countPerRow; ++index2)
            component.SetToggleON(this.IsNowSelectInfo(component.GetListInfo()));
        }
      }
    }

    private void OnValueChanged(bool _isOn, int _idx)
    {
      if (this.skip)
        return;
      this.skip = true;
      if (!this.noProc)
      {
        this.uiCtrl.searchSortHNIdx = !_isOn ? -1 : this.scrollerDatas[_idx].info.userIdx;
        this.uiCtrl.changeSearchSetting = true;
      }
      if (_isOn)
      {
        bool flag = !this.IsNowSelectInfo(this.scrollerDatas[_idx].info);
        this.selectInfo = this.scrollerDatas[_idx];
        if (flag)
        {
          for (int index1 = 0; index1 < this.view.ShownItemCount; ++index1)
          {
            LoopListViewItem2 shownItemByIndex = this.view.GetShownItemByIndex(index1);
            if (!Object.op_Equality((Object) shownItemByIndex, (Object) null))
            {
              NetSelectHNScrollViewInfo component = (NetSelectHNScrollViewInfo) ((Component) shownItemByIndex).GetComponent<NetSelectHNScrollViewInfo>();
              for (int index2 = 0; index2 < this.countPerRow; ++index2)
              {
                if (!this.IsNowSelectInfo(component.GetListInfo()))
                  component.SetToggleON(false);
              }
            }
          }
        }
      }
      else if (this.IsNowSelectInfo(this.scrollerDatas[_idx].info))
        this.selectInfo = (NetSelectHNScrollController.ScrollData) null;
      this.skip = false;
    }

    private bool IsNowSelectInfo(NetworkInfo.SelectHNInfo _info)
    {
      return _info != null && this.selectInfo != null && this.selectInfo.info == _info;
    }

    private LoopListViewItem2 OnUpdate(LoopListView2 _view, int _index)
    {
      if (_index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = _view.NewListViewItem(((Object) this.original).get_name());
      NetSelectHNScrollViewInfo component = (NetSelectHNScrollViewInfo) ((Component) loopListViewItem2).GetComponent<NetSelectHNScrollViewInfo>();
      for (int index1 = 0; index1 < this.countPerRow; ++index1)
      {
        int index = _index * this.countPerRow + index1;
        NetworkInfo.SelectHNInfo info = this.scrollerDatas.SafeGet<NetSelectHNScrollController.ScrollData>(index)?.info;
        component.SetData(info, (Action<bool>) (_isOn => this.OnValueChanged(_isOn, index)));
        this.noProc = true;
        component.SetToggleON(this.IsNowSelectInfo(info));
        this.noProc = false;
      }
      return loopListViewItem2;
    }

    public class ScrollData
    {
      public NetworkInfo.SelectHNInfo info = new NetworkInfo.SelectHNInfo();
      public int index;
    }
  }
}
