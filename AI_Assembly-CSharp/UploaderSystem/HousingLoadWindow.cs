// Decompiled with JetBrains decompiler
// Type: UploaderSystem.HousingLoadWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Housing;
using Housing.SaveLoad;
using Illusion.Extensions;
using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UploaderSystem
{
  [Serializable]
  public class HousingLoadWindow : MonoBehaviour
  {
    [SerializeField]
    private Transform tabRoot;
    [SerializeField]
    private GameObject tabOriginal;
    [SerializeField]
    private RawImage[] rawsThumbnail;
    [SerializeField]
    private Image[] rawsSelect;
    [SerializeField]
    private Button[] buttonsThumbnail;
    [SerializeField]
    private Texture textureNoData;
    [SerializeField]
    [Header("一覧関係")]
    private LoopListView2 view;
    [SerializeField]
    private GameObject original;
    private List<string> listPath;
    private int thumbnailNum;
    private int thumbnailLimit;
    private int pageNum;
    private int tab;
    private int page;
    private int select;
    private int backSelect;
    private Dictionary<int, HousingLoadWindow.TabInfo> dicTabInfo;
    public Action<int> onSelect;

    public HousingLoadWindow()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      foreach (KeyValuePair<int, Manager.Housing.AreaSizeInfo> keyValuePair in Singleton<Manager.Housing>.Instance.dicAreaSizeInfo)
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.tabOriginal, this.tabRoot);
        gameObject.SetActive(true);
        SizeTab component = (SizeTab) gameObject.GetComponent<SizeTab>();
        int type = keyValuePair.Value.no;
        component.Text = string.Format("{0}X{1}X{2}", (object) ((Vector3Int) ref keyValuePair.Value.limitSize).get_x(), (object) ((Vector3Int) ref keyValuePair.Value.limitSize).get_y(), (object) ((Vector3Int) ref keyValuePair.Value.limitSize).get_z());
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(component.toggle), (Func<M0, bool>) (_b => _b)), (Action<M0>) (_ => this.SelectTab(type, false)));
        this.dicTabInfo[type] = new HousingLoadWindow.TabInfo()
        {
          gameObject = gameObject,
          sizeTab = component
        };
      }
      for (int index = 0; index < this.buttonsThumbnail.Length; ++index)
      {
        int idx = index;
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonsThumbnail[index]), (Action<M0>) (_ =>
        {
          int num = this.thumbnailLimit * this.page + idx;
          this.select = this.backSelect == num ? -1 : num;
          this.UpdateSelectImage();
          if (this.onSelect != null)
            this.onSelect(this.select);
          this.backSelect = this.select;
        }));
      }
      this.dicTabInfo[0].IsOn = true;
      this.SelectTab(0, false);
    }

    private void InitInfo(int _idx)
    {
      List<KeyValuePair<DateTime, string>> list = System.IO.Directory.EnumerateFiles(UserData.Create(string.Format("{0}{1:00}/", (object) "housing/", (object) (_idx + 1))), "*.png").Select<string, HousingLoadWindow.FileInfo>((Func<string, HousingLoadWindow.FileInfo>) (s => new HousingLoadWindow.FileInfo(s))).Where<HousingLoadWindow.FileInfo>((Func<HousingLoadWindow.FileInfo, bool>) (fi => fi.CraftInfo != null)).Where<HousingLoadWindow.FileInfo>((Func<HousingLoadWindow.FileInfo, bool>) (fi => fi.Check(_idx))).Select<HousingLoadWindow.FileInfo, KeyValuePair<DateTime, string>>((Func<HousingLoadWindow.FileInfo, KeyValuePair<DateTime, string>>) (fi => new KeyValuePair<DateTime, string>(File.GetLastWriteTime(fi.Path), fi.Path))).ToList<KeyValuePair<DateTime, string>>();
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
      list.Sort((Comparison<KeyValuePair<DateTime, string>>) ((a, b) => b.Key.CompareTo(a.Key)));
      Thread.CurrentThread.CurrentCulture = currentCulture;
      this.listPath = list.Select<KeyValuePair<DateTime, string>, string>((Func<KeyValuePair<DateTime, string>, string>) (v => v.Value)).ToList<string>();
      this.thumbnailNum = !this.listPath.IsNullOrEmpty<string>() ? this.listPath.Count : 0;
      this.pageNum = this.thumbnailNum / this.thumbnailLimit + (this.thumbnailNum % this.thumbnailLimit == 0 ? 0 : 1);
      this.pageNum = Mathf.Max(this.pageNum, 1);
      if (!this.view.IsInit)
      {
        this.view.InitListView(this.pageNum, (Func<LoopListView2, int, LoopListViewItem2>) ((_view, _index) => this.OnUpdate(_view, _index)), (LoopListViewInitParam) null);
      }
      else
      {
        if (this.view.SetListItemCount(this.pageNum, true))
          return;
        this.view.RefreshAllShownItem();
      }
    }

    private void SelectTab(int _idx, bool _force = false)
    {
      if (!_force && this.tab == _idx)
        return;
      this.InitInfo(_idx);
      this.select = -1;
      this.backSelect = -99;
      if (this.onSelect != null)
        this.onSelect(this.select);
      this.tab = _idx;
      this.SetPage(0, true);
    }

    private void SetPage(int _page, bool _force = false)
    {
      if (!_force && this.page == _page)
        return;
      _page = Mathf.Clamp(_page, 0, this.pageNum - 1);
      int num = this.thumbnailLimit * _page;
      for (int index = 0; index < this.thumbnailLimit; ++index)
      {
        int n = num + index;
        if (!MathfEx.RangeEqualOn<int>(0, n, this.thumbnailNum - 1))
        {
          if (_page == 0 && index == 0)
          {
            this.rawsThumbnail[index].set_texture(this.textureNoData);
            ((Behaviour) this.rawsThumbnail[index]).set_enabled(true);
          }
          else
            ((Behaviour) this.rawsThumbnail[index]).set_enabled(false);
          ((Behaviour) this.buttonsThumbnail[index]).set_enabled(false);
        }
        else
        {
          this.rawsThumbnail[index].set_texture((Texture) PngAssist.LoadTexture(this.listPath[n]));
          ((Behaviour) this.rawsThumbnail[index]).set_enabled(true);
          ((Behaviour) this.buttonsThumbnail[index]).set_enabled(true);
          ((Selectable) this.buttonsThumbnail[index]).set_interactable(true);
        }
      }
      this.page = _page;
      this.UpdateSelectImage();
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }

    private LoopListViewItem2 OnUpdate(LoopListView2 _view, int _index)
    {
      if (_index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = _view.NewListViewItem(((Object) this.original).get_name());
      if (loopListViewItem2 is PageButton pageButton)
        pageButton.SetData(_index, this.page == _index, (Action<int>) (_i => this.SetPage(_i, false)));
      return loopListViewItem2;
    }

    public void UpdateSelectImage()
    {
      for (int index = 0; index < this.rawsSelect.Length; ++index)
      {
        if (Object.op_Inequality((Object) null, (Object) this.rawsSelect[index]))
          ((Behaviour) this.rawsSelect[index]).set_enabled(false);
      }
      int num = this.thumbnailLimit * this.page;
      if (this.select < num || this.select > num + this.thumbnailLimit)
        return;
      ((Behaviour) this.rawsSelect[this.select - num]).set_enabled(true);
    }

    public int GetSelectIndex()
    {
      return this.select;
    }

    public string GetSelectPath()
    {
      return this.listPath == null || this.select == -1 ? string.Empty : this.listPath[this.select];
    }

    private class TabInfo
    {
      public GameObject gameObject;
      public SizeTab sizeTab;

      public bool Interactable
      {
        set
        {
          ((Selectable) this.sizeTab.toggle).set_interactable(value);
        }
      }

      public bool IsOn
      {
        set
        {
          this.sizeTab.toggle.set_isOn(value);
        }
      }

      public bool IsOnWithoutCallback
      {
        set
        {
          this.sizeTab.toggle.SetIsOnWithoutCallback(value);
        }
      }
    }

    private class FileInfo
    {
      public FileInfo(string _path)
      {
        this.Path = _path;
        this.CraftInfo = CraftInfo.LoadStatic(_path);
      }

      public string Path { get; private set; }

      public CraftInfo CraftInfo { get; private set; }

      public bool Check(int _areaType)
      {
        return this.CraftInfo != null && _areaType == Singleton<Manager.Housing>.Instance.GetSizeType(this.CraftInfo.AreaNo);
      }
    }
  }
}
