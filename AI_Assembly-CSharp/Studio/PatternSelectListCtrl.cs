// Decompiled with JetBrains decompiler
// Type: Studio.PatternSelectListCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UGUI_AssistLibrary;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class PatternSelectListCtrl : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI textDrawName;
    [SerializeField]
    private RectTransform rtfScrollRect;
    [SerializeField]
    private RectTransform rtfContant;
    [SerializeField]
    private GameObject objContent;
    [SerializeField]
    private GameObject objTemp;
    [SerializeField]
    private Button btnPrev;
    [SerializeField]
    private Button btnNext;
    private string selectDrawName;
    private List<PatternSelectInfo> _lstSelectInfo;
    public PatternSelectListCtrl.OnChangeItemFunc onChangeItemFunc;

    public PatternSelectListCtrl()
    {
      base.\u002Ector();
    }

    public List<PatternSelectInfo> lstSelectInfo
    {
      get
      {
        return this._lstSelectInfo;
      }
    }

    public void ClearList()
    {
      this._lstSelectInfo.Clear();
    }

    public void AddList(int index, string name, string assetBundle, string assetName)
    {
      this._lstSelectInfo.Add(new PatternSelectInfo()
      {
        index = index,
        name = name,
        assetBundle = assetBundle,
        assetName = assetName
      });
    }

    public void AddOutside(int _start)
    {
      string[] files = Directory.GetFiles(UserData.Create("pattern_thumb"), "*.png");
      // ISSUE: reference to a compiler-generated field
      if (PatternSelectListCtrl.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PatternSelectListCtrl.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileName);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, string> fMgCache0 = PatternSelectListCtrl.\u003C\u003Ef__mg\u0024cache0;
      List<string> list = ((IEnumerable<string>) files).Select<string, string>(fMgCache0).ToList<string>();
      for (int index = 0; index < list.Count; ++index)
        this.AddList(_start + index, Path.GetFileNameWithoutExtension(list[index]), string.Empty, list[index]);
    }

    public void Create(
      PatternSelectListCtrl.OnChangeItemFunc _onChangeItemFunc)
    {
      this.onChangeItemFunc = _onChangeItemFunc;
      for (int index = this.objContent.get_transform().get_childCount() - 1; index >= 0; --index)
        Object.Destroy((Object) ((Component) this.objContent.get_transform().GetChild(index)).get_gameObject());
      ToggleGroup component1 = (ToggleGroup) this.objContent.GetComponent<ToggleGroup>();
      int num = 0;
      for (int index = 0; index < this._lstSelectInfo.Count; ++index)
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objTemp);
        PatternSelectInfoComponent component2 = (PatternSelectInfoComponent) gameObject.GetComponent<PatternSelectInfoComponent>();
        component2.info = this._lstSelectInfo[index];
        component2.info.sic = component2;
        component2.tgl.set_group(component1);
        gameObject.get_transform().SetParent(this.objContent.get_transform(), false);
        this.SetToggleHandler(gameObject, component2);
        component2.img = (Image) gameObject.GetComponent<Image>();
        if (Object.op_Implicit((Object) component2.img))
        {
          Texture2D texture2D = !this._lstSelectInfo[index].assetBundle.IsNullOrEmpty() ? CommonLib.LoadAsset<Texture2D>(this._lstSelectInfo[index].assetBundle, this._lstSelectInfo[index].assetName, false, string.Empty) : PngAssist.LoadTexture(UserData.Path + "pattern_thumb/" + this._lstSelectInfo[index].assetName);
          if (Object.op_Implicit((Object) texture2D))
            component2.img.set_sprite(Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).get_width(), (float) ((Texture) texture2D).get_height()), new Vector2(0.5f, 0.5f)));
        }
        component2.Disvisible(this._lstSelectInfo[index].disvisible);
        component2.Disable(this._lstSelectInfo[index].disable);
        ++num;
      }
      this.ToggleAllOff();
    }

    public PatternSelectInfo GetSelectInfoFromIndex(int index)
    {
      return this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.index == index));
    }

    public PatternSelectInfo GetSelectInfoFromName(string name)
    {
      return this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.name == name));
    }

    public string GetNameFormIndex(int index)
    {
      PatternSelectInfo patternSelectInfo = this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.index == index));
      return patternSelectInfo != null ? patternSelectInfo.name : string.Empty;
    }

    public int GetIndexFromName(string name)
    {
      PatternSelectInfo patternSelectInfo = this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.name == name));
      return patternSelectInfo != null ? patternSelectInfo.index : -1;
    }

    public int GetSelectIndex()
    {
      PatternSelectInfo patternSelectInfo = this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (psi => psi.interactable & psi.activeSelf & psi.isOn));
      return patternSelectInfo != null ? patternSelectInfo.index : -1;
    }

    public PatternSelectInfoComponent GetSelectTopItem()
    {
      int selectIndex = this.GetSelectIndex();
      if (selectIndex == -1)
        return (PatternSelectInfoComponent) null;
      return this.GetSelectInfoFromIndex(selectIndex)?.sic;
    }

    public PatternSelectInfoComponent GetSelectableTopItem()
    {
      SortedDictionary<int, PatternSelectInfoComponent> source = new SortedDictionary<int, PatternSelectInfoComponent>();
      for (int index = 0; index < this._lstSelectInfo.Count; ++index)
      {
        if (((Selectable) this._lstSelectInfo[index].sic.tgl).get_interactable() && ((Component) this._lstSelectInfo[index].sic).get_gameObject().get_activeSelf())
          source[((Component) this._lstSelectInfo[index].sic).get_gameObject().get_transform().GetSiblingIndex()] = this._lstSelectInfo[index].sic;
      }
      PatternSelectInfoComponent selectInfoComponent = (PatternSelectInfoComponent) null;
      if (source.Count != 0)
        selectInfoComponent = source.First<KeyValuePair<int, PatternSelectInfoComponent>>().Value;
      return selectInfoComponent;
    }

    public int GetDrawOrderFromIndex(int index)
    {
      SortedDictionary<int, PatternSelectInfoComponent> source = new SortedDictionary<int, PatternSelectInfoComponent>();
      for (int index1 = 0; index1 < this._lstSelectInfo.Count; ++index1)
      {
        if (((Component) this._lstSelectInfo[index1].sic).get_gameObject().get_activeSelf())
          source[((Component) this._lstSelectInfo[index1].sic).get_gameObject().get_transform().GetSiblingIndex()] = this._lstSelectInfo[index1].sic;
      }
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType15<KeyValuePair<int, PatternSelectInfoComponent>, int> anonType15 in source.Select<KeyValuePair<int, PatternSelectInfoComponent>, \u003C\u003E__AnonType15<KeyValuePair<int, PatternSelectInfoComponent>, int>>((Func<KeyValuePair<int, PatternSelectInfoComponent>, int, \u003C\u003E__AnonType15<KeyValuePair<int, PatternSelectInfoComponent>, int>>) ((val, idx) => new \u003C\u003E__AnonType15<KeyValuePair<int, PatternSelectInfoComponent>, int>(val, idx))))
      {
        if (anonType15.val.Value.info.index == index)
          return anonType15.idx;
      }
      return -1;
    }

    public int GetInclusiveCount()
    {
      return this._lstSelectInfo.Count<PatternSelectInfo>((Func<PatternSelectInfo, bool>) (_psi => _psi.activeSelf));
    }

    public void SelectPrevItem()
    {
      List<PatternSelectInfo> list = this._lstSelectInfo.Where<PatternSelectInfo>((Func<PatternSelectInfo, bool>) (lst => ((Selectable) lst.sic.tgl).get_interactable() && ((Component) lst.sic).get_gameObject().get_activeSelf())).ToList<PatternSelectInfo>();
      int index1 = list.FindIndex((Predicate<PatternSelectInfo>) (lst => lst.sic.tgl.get_isOn()));
      if (index1 == -1)
        return;
      int count = list.Count;
      int index2 = (index1 + count - 1) % count;
      this.SelectItem(list[index2].index);
    }

    public void SelectNextItem()
    {
      List<PatternSelectInfo> list = this._lstSelectInfo.Where<PatternSelectInfo>((Func<PatternSelectInfo, bool>) (lst => ((Selectable) lst.sic.tgl).get_interactable() && ((Component) lst.sic).get_gameObject().get_activeSelf())).ToList<PatternSelectInfo>();
      int index1 = list.FindIndex((Predicate<PatternSelectInfo>) (lst => lst.sic.tgl.get_isOn()));
      if (index1 == -1)
        return;
      int count = list.Count;
      int index2 = (index1 + 1) % count;
      this.SelectItem(list[index2].index);
    }

    public void SelectItem(int index)
    {
      PatternSelectInfo patternSelectInfo = this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.index == index));
      if (patternSelectInfo == null)
        return;
      patternSelectInfo.sic.tgl.set_isOn(true);
      this.ChangeItem(patternSelectInfo.sic);
      this.UpdateScrollPosition();
    }

    public void SelectItem(string name)
    {
      PatternSelectInfo patternSelectInfo = this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.name == name));
      if (patternSelectInfo == null)
        return;
      patternSelectInfo.sic.tgl.set_isOn(true);
      this.ChangeItem(patternSelectInfo.sic);
      this.UpdateScrollPosition();
    }

    public void UpdateScrollPosition()
    {
    }

    public void OnPointerClick(PatternSelectInfoComponent _psic)
    {
      if (Object.op_Equality((Object) null, (Object) _psic) || !((Selectable) _psic.tgl).get_interactable())
        return;
      this.ChangeItem(_psic);
    }

    public void OnPointerEnter(PatternSelectInfoComponent _psic)
    {
      if (Object.op_Equality((Object) null, (Object) _psic) || !((Selectable) _psic.tgl).get_interactable() || !Object.op_Implicit((Object) this.textDrawName))
        return;
      ((TMP_Text) this.textDrawName).set_text(_psic.info.name);
    }

    public void OnPointerExit(PatternSelectInfoComponent _psic)
    {
      if (Object.op_Equality((Object) null, (Object) _psic) || !((Selectable) _psic.tgl).get_interactable() || !Object.op_Implicit((Object) this.textDrawName))
        return;
      ((TMP_Text) this.textDrawName).set_text(this.selectDrawName);
    }

    public void ChangeItem(PatternSelectInfoComponent _psic)
    {
      if (this.onChangeItemFunc != null)
        this.onChangeItemFunc(_psic.info.index);
      this.selectDrawName = _psic.info.name;
      if (!Object.op_Implicit((Object) this.textDrawName))
        return;
      ((TMP_Text) this.textDrawName).set_text(this.selectDrawName);
    }

    public void ToggleAllOff()
    {
      for (int index = 0; index < this._lstSelectInfo.Count; ++index)
        this._lstSelectInfo[index].sic.tgl.set_isOn(false);
    }

    public void DisableItem(int index, bool _disable)
    {
      this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.index == index)).SafeProc<PatternSelectInfo>((Action<PatternSelectInfo>) (psi =>
      {
        psi.disable = _disable;
        psi.sic.Disable(_disable);
      }));
    }

    public void DisableItem(string name, bool _disable)
    {
      this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.name == name)).SafeProc<PatternSelectInfo>((Action<PatternSelectInfo>) (psi =>
      {
        psi.disable = _disable;
        psi.sic.Disable(_disable);
      }));
    }

    public void DisvisibleItem(int index, bool _disvisible)
    {
      this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.index == index)).SafeProc<PatternSelectInfo>((Action<PatternSelectInfo>) (psi =>
      {
        psi.disvisible = _disvisible;
        psi.sic.Disvisible(_disvisible);
      }));
    }

    public void DisvisibleItem(string name, bool _disvisible)
    {
      this._lstSelectInfo.Find((Predicate<PatternSelectInfo>) (item => item.name == name)).SafeProc<PatternSelectInfo>((Action<PatternSelectInfo>) (psi =>
      {
        psi.disvisible = _disvisible;
        psi.sic.Disvisible(_disvisible);
      }));
    }

    private void SetToggleHandler(GameObject obj, PatternSelectInfoComponent _psic)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PatternSelectListCtrl.\u003CSetToggleHandler\u003Ec__AnonStoreyA handlerCAnonStoreyA = new PatternSelectListCtrl.\u003CSetToggleHandler\u003Ec__AnonStoreyA();
      // ISSUE: reference to a compiler-generated field
      handlerCAnonStoreyA._psic = _psic;
      // ISSUE: reference to a compiler-generated field
      handlerCAnonStoreyA.\u0024this = this;
      UIAL_EventTrigger uialEventTrigger = (UIAL_EventTrigger) obj.AddComponent<UIAL_EventTrigger>();
      uialEventTrigger.triggers = new List<UIAL_EventTrigger.Entry>();
      UIAL_EventTrigger.Entry entry1 = new UIAL_EventTrigger.Entry();
      entry1.eventID = (EventTriggerType) 4;
      entry1.buttonType = UIAL_EventTrigger.ButtonType.Left;
      // ISSUE: method pointer
      entry1.callback.AddListener(new UnityAction<BaseEventData>((object) handlerCAnonStoreyA, __methodptr(\u003C\u003Em__0)));
      uialEventTrigger.triggers.Add(entry1);
      if (!Object.op_Implicit((Object) this.textDrawName))
        return;
      UIAL_EventTrigger.Entry entry2 = new UIAL_EventTrigger.Entry();
      entry2.eventID = (EventTriggerType) 0;
      // ISSUE: method pointer
      entry2.callback.AddListener(new UnityAction<BaseEventData>((object) handlerCAnonStoreyA, __methodptr(\u003C\u003Em__1)));
      uialEventTrigger.triggers.Add(entry2);
      UIAL_EventTrigger.Entry entry3 = new UIAL_EventTrigger.Entry();
      entry3.eventID = (EventTriggerType) 1;
      // ISSUE: method pointer
      entry3.callback.AddListener(new UnityAction<BaseEventData>((object) handlerCAnonStoreyA, __methodptr(\u003C\u003Em__2)));
      uialEventTrigger.triggers.Add(entry3);
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnPrev), (Action<M0>) (_ => this.SelectPrevItem()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNext), (Action<M0>) (_ => this.SelectNextItem()));
    }

    public delegate void OnChangeItemFunc(int index);
  }
}
