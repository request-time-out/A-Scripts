// Decompiled with JetBrains decompiler
// Type: Studio.CharaFileSort
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  [Serializable]
  public class CharaFileSort
  {
    public List<CharaFileInfo> cfiList = new List<CharaFileInfo>();
    private int m_Select = -1;
    private bool[] sortType = new bool[2]{ true, true };
    public Transform root;
    public Image[] imageSort;
    public Sprite[] spriteSort;

    public CharaFileSort()
    {
      this.m_Select = -1;
      this.sortKind = -1;
    }

    public int select
    {
      get
      {
        return this.m_Select;
      }
      set
      {
        int select = this.m_Select;
        if (!Utility.SetStruct<int>(ref this.m_Select, value))
          return;
        if (MathfEx.RangeEqualOn<int>(0, select, this.cfiList.Count))
          this.cfiList[select].select = false;
        if (!MathfEx.RangeEqualOn<int>(0, this.m_Select, this.cfiList.Count))
          return;
        this.cfiList[this.m_Select].select = true;
      }
    }

    public int sortKind { get; private set; }

    public string selectPath
    {
      get
      {
        return this.cfiList.Count == 0 || !MathfEx.RangeEqualOn<int>(0, this.select, this.cfiList.Count - 1) ? string.Empty : this.cfiList[this.select].file;
      }
    }

    public void DeleteAllNode()
    {
      int childCount = this.root.get_childCount();
      for (int index = 0; index < childCount; ++index)
        Object.Destroy((Object) ((Component) this.root.GetChild(index)).get_gameObject());
      this.root.DetachChildren();
      this.m_Select = -1;
    }

    public void Sort(int _type, bool _ascend)
    {
      this.sortKind = _type;
      switch (this.sortKind)
      {
        case 0:
          this.SortName(_ascend);
          break;
        case 1:
          this.SortTime(_ascend);
          break;
      }
      for (int index = 0; index < this.imageSort.Length; ++index)
        ((Behaviour) this.imageSort[index]).set_enabled(index == this.sortKind);
    }

    public void Sort(int _type)
    {
      this.Sort(_type, this.sortKind != _type ? this.sortType[_type] : !this.sortType[_type]);
    }

    private void SortName(bool _ascend)
    {
      if (this.cfiList.IsNullOrEmpty<CharaFileInfo>())
        return;
      this.sortType[0] = _ascend;
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
      if (_ascend)
        this.cfiList.Sort((Comparison<CharaFileInfo>) ((a, b) => a.name.CompareTo(b.name)));
      else
        this.cfiList.Sort((Comparison<CharaFileInfo>) ((a, b) => b.name.CompareTo(a.name)));
      Thread.CurrentThread.CurrentCulture = currentCulture;
      for (int index = 0; index < this.cfiList.Count; ++index)
      {
        this.cfiList[index].index = index;
        this.cfiList[index].siblingIndex = index;
      }
      this.select = this.cfiList.FindIndex((Predicate<CharaFileInfo>) (v => v.select));
      this.imageSort[0].set_sprite(this.spriteSort[!this.sortType[0] ? 1 : 0]);
    }

    private void SortTime(bool _ascend)
    {
      if (this.cfiList.IsNullOrEmpty<CharaFileInfo>())
        return;
      this.sortType[1] = _ascend;
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
      if (_ascend)
        this.cfiList.Sort((Comparison<CharaFileInfo>) ((a, b) => a.time.CompareTo(b.time)));
      else
        this.cfiList.Sort((Comparison<CharaFileInfo>) ((a, b) => b.time.CompareTo(a.time)));
      Thread.CurrentThread.CurrentCulture = currentCulture;
      for (int index = 0; index < this.cfiList.Count; ++index)
      {
        this.cfiList[index].index = index;
        this.cfiList[index].siblingIndex = index;
      }
      this.select = this.cfiList.FindIndex((Predicate<CharaFileInfo>) (v => v.select));
      this.imageSort[1].set_sprite(this.spriteSort[!this.sortType[1] ? 1 : 0]);
    }
  }
}
