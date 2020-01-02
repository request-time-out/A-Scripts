// Decompiled with JetBrains decompiler
// Type: GameLoadCharaFileSystem.GameLoadCharaListCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Definitions;
using Illusion.Component.UI;
using Illusion.Extensions;
using Manager;
using SceneAssist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameLoadCharaFileSystem
{
  public class GameLoadCharaListCtrl : MonoBehaviour
  {
    [SerializeField]
    private GameLoadCharaWindow cfWindow;
    [Header("メインWindow")]
    [SerializeField]
    private Toggle tglOrder;
    [SerializeField]
    private Image imgOrder;
    [SerializeField]
    private PointerEnterExitAction actionOrderSelect;
    [SerializeField]
    private GameObject objOrderSelect;
    [SerializeField]
    private Button btnSortWindowOpen;
    [SerializeField]
    private GameCharaFileScrollController charaSelectScrollView;
    [Header("プレイヤーパラメーターWindow")]
    [SerializeField]
    private GameObject objPlayerParameterWindow;
    [SerializeField]
    private UnityEngine.UI.Text txtPlayerCharaName;
    [SerializeField]
    private RawImage riPlayerCard;
    [SerializeField]
    private UnityEngine.UI.Text txtPlayerSex;
    [SerializeField]
    private Texture2D texEmpty;
    [Header("女の子パラメーターWindow")]
    [SerializeField]
    private GameObject objFemaleParameterWindow;
    [SerializeField]
    private UnityEngine.UI.Text txtFemaleCharaName;
    [SerializeField]
    private RawImage riFemaleCard;
    [SerializeField]
    private SpriteChangeCtrl[] sccStateOfProgress;
    [SerializeField]
    private Toggle[] tglFemaleStateSelects;
    [SerializeField]
    private PointerEnterExitAction[] actionStateSelects;
    [SerializeField]
    private GameObject[] objFemaleStateSelectSels;
    [SerializeField]
    private GameObject[] objFemalParameterRoots;
    [Header("女の子パラメーター")]
    [SerializeField]
    private UnityEngine.UI.Text txtLifeStyle;
    [SerializeField]
    private UnityEngine.UI.Text txtGirlPower;
    [SerializeField]
    private UnityEngine.UI.Text txtTrust;
    [SerializeField]
    private UnityEngine.UI.Text txtHumanNature;
    [SerializeField]
    private UnityEngine.UI.Text txtInstinct;
    [SerializeField]
    private UnityEngine.UI.Text txtHentai;
    [SerializeField]
    private UnityEngine.UI.Text txtVigilance;
    [SerializeField]
    private UnityEngine.UI.Text txtSocial;
    [SerializeField]
    private UnityEngine.UI.Text txtDarkness;
    [SerializeField]
    private UnityEngine.UI.Text[] txtNormalSkillSlots;
    [SerializeField]
    private UnityEngine.UI.Text[] txtHSkillSlots;
    [Header("ソートWindow")]
    [SerializeField]
    private GameObject objSortWindow;
    [SerializeField]
    private Button btnSortWindowClose;
    [SerializeField]
    private Toggle tglSortDay;
    [SerializeField]
    private Toggle tglSortName;
    [SerializeField]
    private PointerEnterExitAction actionSortDay;
    [SerializeField]
    private PointerEnterExitAction actionSortName;
    [SerializeField]
    private GameObject objSortDaySelect;
    [SerializeField]
    private GameObject objSortNameSelect;
    private readonly string[] localizeMale;
    private readonly string[] localizeFemale;
    private readonly string[] localizeFutanari;
    private readonly string listAssetBundleName;
    private List<GameCharaFileInfo> lstFileInfo;
    [HideInInspector]
    public bool updateCategory;
    public GameLoadCharaListCtrl.OnChangeItemFunc onChangeItemFunc;
    public System.Action<bool> onChangeItem;
    private int sortSelectNum;
    private int femaleParameterSelectNum;
    private Dictionary<int, List<string>> dicSkill;
    private Dictionary<int, List<string>> dicHSkill;

    public GameLoadCharaListCtrl()
    {
      base.\u002Ector();
    }

    public List<GameCharaFileInfo> GetListCharaFileInfo()
    {
      return this.lstFileInfo;
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglOrder.onValueChanged), (System.Action<M0>) (_isOn =>
      {
        ((Behaviour) this.imgOrder).set_enabled(!_isOn);
        if (this.tglSortDay.get_isOn())
          this.SortDate(_isOn);
        else
          this.SortName(_isOn);
        this.charaSelectScrollView.Init(this.lstFileInfo);
        this.charaSelectScrollView.SetNowSelectToggle();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ((Behaviour) this.imgOrder).set_enabled(!this.tglOrder.get_isOn());
      this.objOrderSelect.SetActiveIfDifferent(false);
      // ISSUE: method pointer
      this.actionOrderSelect.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__1)));
      // ISSUE: method pointer
      this.actionOrderSelect.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnSortWindowOpen), (System.Action<M0>) (_ =>
      {
        this.objSortWindow.SetActiveIfDifferent(!this.objSortWindow.get_activeSelf());
        if (this.objSortWindow.get_activeSelf())
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        else
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.btnSortWindowOpen), (System.Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      this.charaSelectScrollView.onSelect = (System.Action<GameCharaFileInfo>) (_data =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        if (this.onChangeItemFunc != null)
          this.onChangeItemFunc(_data);
        if (this.onChangeItem != null)
          this.onChangeItem(true);
        this.SetParameter(_data);
      });
      this.charaSelectScrollView.onDeSelect = (System.Action) (() =>
      {
        if (this.onChangeItem != null)
          this.onChangeItem(false);
        this.objPlayerParameterWindow.SetActiveIfDifferent(false);
        this.objFemaleParameterWindow.SetActiveIfDifferent(false);
      });
      this.objPlayerParameterWindow.SetActiveIfDifferent(false);
      this.txtPlayerCharaName.set_text("NoName");
      this.riPlayerCard.set_texture((Texture) null);
      this.txtPlayerSex.set_text(string.Empty);
      this.objFemaleParameterWindow.SetActiveIfDifferent(false);
      this.txtFemaleCharaName.set_text("NoName");
      this.riFemaleCard.set_texture((Texture) null);
      for (int index = 0; index < this.sccStateOfProgress.Length; ++index)
        this.sccStateOfProgress[index].OnChangeValue(index != 0 ? 0 : 1);
      for (int index = 0; index < this.tglFemaleStateSelects.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GameLoadCharaListCtrl.\u003CStart\u003Ec__AnonStorey0 startCAnonStorey0 = new GameLoadCharaListCtrl.\u003CStart\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey0.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey0.sel = index;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglFemaleStateSelects[index].onValueChanged), (Func<M0, bool>) new Func<bool, bool>(startCAnonStorey0.\u003C\u003Em__0)), (System.Action<M0>) new System.Action<bool>(startCAnonStorey0.\u003C\u003Em__1));
      }
      for (int index = 0; index < this.actionStateSelects.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GameLoadCharaListCtrl.\u003CStart\u003Ec__AnonStorey1 startCAnonStorey1 = new GameLoadCharaListCtrl.\u003CStart\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey1.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey1.sel = index;
        // ISSUE: method pointer
        this.actionStateSelects[index].listActionEnter.Add(new UnityAction((object) startCAnonStorey1, __methodptr(\u003C\u003Em__0)));
        // ISSUE: method pointer
        this.actionStateSelects[index].listActionExit.Add(new UnityAction((object) startCAnonStorey1, __methodptr(\u003C\u003Em__1)));
      }
      for (int index = 0; index < this.objFemaleStateSelectSels.Length; ++index)
        this.objFemaleStateSelectSels[index].SetActiveIfDifferent(false);
      this.txtLifeStyle.set_text(string.Empty);
      this.txtGirlPower.set_text("0");
      this.txtTrust.set_text("0");
      this.txtHumanNature.set_text("0");
      this.txtInstinct.set_text("0");
      this.txtHentai.set_text("0");
      this.txtVigilance.set_text("0");
      this.txtSocial.set_text("0");
      this.txtDarkness.set_text("0");
      for (int index = 0; index < this.txtNormalSkillSlots.Length; ++index)
        this.txtNormalSkillSlots[index].set_text("--------------------");
      for (int index = 0; index < this.txtHSkillSlots.Length; ++index)
        this.txtHSkillSlots[index].set_text("--------------------");
      this.objSortWindow.SetActiveIfDifferent(false);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnSortWindowClose), (System.Action<M0>) (_ =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        this.objSortWindow.SetActiveIfDifferent(false);
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.btnSortWindowClose), (System.Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      // ISSUE: method pointer
      this.actionSortDay.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__9)));
      // ISSUE: method pointer
      this.actionSortDay.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__A)));
      // ISSUE: method pointer
      this.actionSortName.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__B)));
      // ISSUE: method pointer
      this.actionSortName.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__C)));
      this.objSortDaySelect.SetActiveIfDifferent(false);
      this.objSortNameSelect.SetActiveIfDifferent(false);
      if (Object.op_Implicit((Object) this.tglSortDay))
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglSortDay.onValueChanged), (Func<M0, bool>) (_ => this.sortSelectNum != 0)), (System.Action<M0>) (_isOn =>
        {
          if (!_isOn)
            return;
          this.sortSelectNum = 0;
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          this.SortDate(this.tglOrder.get_isOn());
          this.charaSelectScrollView.Init(this.lstFileInfo);
          this.charaSelectScrollView.SetNowSelectToggle();
        }));
      if (Object.op_Implicit((Object) this.tglSortName))
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglSortName.onValueChanged), (Func<M0, bool>) (_ => this.sortSelectNum != 1)), (System.Action<M0>) (_isOn =>
        {
          if (!_isOn)
            return;
          this.sortSelectNum = 1;
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          this.SortName(this.tglOrder.get_isOn());
          this.charaSelectScrollView.Init(this.lstFileInfo);
          this.charaSelectScrollView.SetNowSelectToggle();
        }));
      this.LoadSkillList();
      this.LoadHSkillList();
    }

    public void SortDate(bool ascend)
    {
      if (this.lstFileInfo.Count == 0)
        return;
      GameCharaFileInfo gameCharaFileInfo = this.lstFileInfo.Find((Predicate<GameCharaFileInfo>) (i => i.personality == "不明"));
      if (gameCharaFileInfo != null)
        this.lstFileInfo.Remove(gameCharaFileInfo);
      using (new GameSystem.CultureScope())
        this.lstFileInfo = !ascend ? this.lstFileInfo.OrderByDescending<GameCharaFileInfo, DateTime>((Func<GameCharaFileInfo, DateTime>) (n => n.time)).ThenByDescending<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.name)).ThenByDescending<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.personality)).ToList<GameCharaFileInfo>() : this.lstFileInfo.OrderBy<GameCharaFileInfo, DateTime>((Func<GameCharaFileInfo, DateTime>) (n => n.time)).ThenBy<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.name)).ThenBy<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.personality)).ToList<GameCharaFileInfo>();
      if (gameCharaFileInfo == null)
        return;
      this.lstFileInfo.Insert(0, gameCharaFileInfo);
    }

    public void SortName(bool ascend)
    {
      if (this.lstFileInfo.Count == 0)
        return;
      GameCharaFileInfo gameCharaFileInfo = this.lstFileInfo.Find((Predicate<GameCharaFileInfo>) (i => i.personality == "不明"));
      if (gameCharaFileInfo != null)
        this.lstFileInfo.Remove(gameCharaFileInfo);
      using (new GameSystem.CultureScope())
        this.lstFileInfo = !ascend ? this.lstFileInfo.OrderByDescending<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.name)).ThenByDescending<GameCharaFileInfo, DateTime>((Func<GameCharaFileInfo, DateTime>) (n => n.time)).ThenByDescending<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.personality)).ToList<GameCharaFileInfo>() : this.lstFileInfo.OrderBy<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.name)).ThenBy<GameCharaFileInfo, DateTime>((Func<GameCharaFileInfo, DateTime>) (n => n.time)).ThenBy<GameCharaFileInfo, string>((Func<GameCharaFileInfo, string>) (n => n.personality)).ToList<GameCharaFileInfo>();
      if (gameCharaFileInfo == null)
        return;
      this.lstFileInfo.Insert(0, gameCharaFileInfo);
    }

    public void SetNowSelectToggle()
    {
      if (this.charaSelectScrollView == null)
        return;
      this.charaSelectScrollView.SetNowSelectToggle();
    }

    public void ClearList()
    {
      this.lstFileInfo.Clear();
    }

    public void AddList(List<GameCharaFileInfo> list)
    {
      this.lstFileInfo = new List<GameCharaFileInfo>((IEnumerable<GameCharaFileInfo>) list);
    }

    public GameCharaFileInfo GetNowSelectCard()
    {
      return this.charaSelectScrollView.selectInfo?.info;
    }

    public void InitSort()
    {
      this.tglOrder.SetIsOnWithoutCallback(false);
      ((Behaviour) this.imgOrder).set_enabled(!this.tglOrder.get_isOn());
      this.sortSelectNum = 0;
      this.tglSortDay.SetIsOnWithoutCallback(true);
    }

    public void Create(bool _isSelectInfoClear)
    {
      if (this.tglSortDay.get_isOn())
        this.SortDate(this.tglOrder.get_isOn());
      else
        this.SortName(this.tglOrder.get_isOn());
      if (_isSelectInfoClear)
        this.charaSelectScrollView.SelectInfoClear();
      this.charaSelectScrollView.Init(this.lstFileInfo);
      if (_isSelectInfoClear)
        this.charaSelectScrollView.SetTopLine();
      this.objPlayerParameterWindow.SetActiveIfDifferent(false);
      this.objFemaleParameterWindow.SetActiveIfDifferent(false);
      this.objSortWindow.SetActiveIfDifferent(false);
    }

    public void CreateListView(bool _isSelectInfoClear)
    {
      if (this.tglSortDay.get_isOn())
        this.SortDate(this.tglOrder.get_isOn());
      else
        this.SortName(this.tglOrder.get_isOn());
      if (_isSelectInfoClear)
      {
        this.charaSelectScrollView.SelectInfoClear();
        this.charaSelectScrollView.SetTopLine();
        this.objPlayerParameterWindow.SetActiveIfDifferent(false);
        this.objFemaleParameterWindow.SetActiveIfDifferent(false);
      }
      this.charaSelectScrollView.Init(this.lstFileInfo);
    }

    private void SetParameter(GameCharaFileInfo _data)
    {
      if (this.cfWindow.windowType == 0)
        this.SetPlayerParameter(_data);
      else
        this.SetFemaleParameter(_data);
    }

    public void SetParameterWindowVisible(bool _isVisible)
    {
      if (this.cfWindow.windowType == 0)
        this.objPlayerParameterWindow.SetActiveIfDifferent(_isVisible);
      else
        this.objFemaleParameterWindow.SetActiveIfDifferent(_isVisible);
    }

    public void ParameterWindowVisibleNoTypeJudge(bool _isVisible)
    {
      this.objPlayerParameterWindow.SetActiveIfDifferent(_isVisible);
      this.objFemaleParameterWindow.SetActiveIfDifferent(_isVisible);
    }

    private void SetPlayerParameter(GameCharaFileInfo _data)
    {
      this.objPlayerParameterWindow.SetActiveIfDifferent(true);
      this.txtPlayerCharaName.set_text(_data.name);
      this.riPlayerCard.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(_data.FullPath), 0, 0, (TextureFormat) 5, false));
      int languageInt = Singleton<GameSystem>.Instance.languageInt;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(_data.sex != 0 ? this.localizeFemale[languageInt] : this.localizeMale[languageInt]);
      if (_data.sex == 1 && _data.futanari)
        stringBuilder.Append(this.localizeFutanari[languageInt]);
      this.txtPlayerSex.set_text(stringBuilder.ToString());
    }

    private void SetFemaleParameter(GameCharaFileInfo _data)
    {
      if (!_data.FullPath.IsNullOrEmpty())
      {
        bool activeSelf = this.objFemaleParameterWindow.get_activeSelf();
        this.objFemaleParameterWindow.SetActiveIfDifferent(true);
        if (!activeSelf)
        {
          this.femaleParameterSelectNum = 0;
          this.tglFemaleStateSelects[0].SetIsOnWithoutCallback(true);
          for (int index = 0; index < this.objFemalParameterRoots.Length; ++index)
            this.objFemalParameterRoots[index].SetActiveIfDifferent(index == 0);
        }
        this.txtFemaleCharaName.set_text(_data.name);
        this.riFemaleCard.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(_data.FullPath), 0, 0, (TextureFormat) 5, false));
        for (int index = 0; index < this.sccStateOfProgress.Length; ++index)
          this.sccStateOfProgress[index].OnChangeValue(_data.phase >= index ? 1 : 0);
        string empty = string.Empty;
        this.txtLifeStyle.set_text(!Lifestyle.LifestyleName.TryGetValue(_data.lifeStyle, out empty) ? "--------------------" : (_data.lifeStyle != 4 ? empty : "E・シーカー"));
        string str1 = "--------";
        this.txtGirlPower.set_text(!_data.gameRegistration ? str1 : _data.flavorState[0].ToString());
        this.txtTrust.set_text(!_data.gameRegistration ? str1 : _data.flavorState[1].ToString());
        this.txtHumanNature.set_text(!_data.gameRegistration ? str1 : _data.flavorState[2].ToString());
        this.txtInstinct.set_text(!_data.gameRegistration ? str1 : _data.flavorState[3].ToString());
        this.txtHentai.set_text(!_data.gameRegistration ? str1 : _data.flavorState[4].ToString());
        this.txtVigilance.set_text(!_data.gameRegistration ? str1 : _data.flavorState[5].ToString());
        this.txtSocial.set_text(!_data.gameRegistration ? str1 : _data.flavorState[7].ToString());
        this.txtDarkness.set_text(!_data.gameRegistration ? str1 : _data.flavorState[6].ToString());
        int languageInt = Singleton<GameSystem>.Instance.languageInt;
        List<string> stringList;
        for (int key = 0; key < this.txtNormalSkillSlots.Length; ++key)
        {
          string str2 = "--------------------";
          if (_data.normalSkill.ContainsKey(key))
            str2 = !this.dicSkill.TryGetValue(_data.normalSkill[key], out stringList) ? "--------------------" : stringList[languageInt];
          this.txtNormalSkillSlots[key].set_text(str2);
        }
        for (int key = 0; key < this.txtHSkillSlots.Length; ++key)
        {
          string str2 = "--------------------";
          if (_data.hSkill.ContainsKey(key))
            str2 = !this.dicHSkill.TryGetValue(_data.hSkill[key], out stringList) ? "--------------------" : stringList[languageInt];
          this.txtHSkillSlots[key].set_text(str2);
        }
      }
      else
      {
        this.txtFemaleCharaName.set_text(string.Empty);
        this.riFemaleCard.set_texture((Texture) this.texEmpty);
        for (int index = 0; index < this.sccStateOfProgress.Length; ++index)
          this.sccStateOfProgress[index].OnChangeValue(_data.phase >= index ? 1 : 0);
        this.txtLifeStyle.set_text(string.Empty);
        this.txtGirlPower.set_text(string.Empty);
        this.txtTrust.set_text(string.Empty);
        this.txtHumanNature.set_text(string.Empty);
        this.txtInstinct.set_text(string.Empty);
        this.txtHentai.set_text(string.Empty);
        this.txtVigilance.set_text(string.Empty);
        this.txtSocial.set_text(string.Empty);
        this.txtDarkness.set_text(string.Empty);
        for (int index = 0; index < this.txtNormalSkillSlots.Length; ++index)
        {
          string str = "--------------------";
          this.txtNormalSkillSlots[index].set_text(str);
        }
        for (int index = 0; index < this.txtHSkillSlots.Length; ++index)
        {
          string str = "--------------------";
          this.txtHSkillSlots[index].set_text(str);
        }
      }
    }

    private bool LoadSkillList()
    {
      string listAssetBundleName = this.listAssetBundleName;
      TitleSkillName titleSkillName = CommonLib.LoadAsset<TitleSkillName>(listAssetBundleName, "title_skill", false, string.Empty);
      AssetBundleManager.UnloadAssetBundle(listAssetBundleName, true, (string) null, false);
      foreach (TitleSkillName.Param obj in titleSkillName.param)
      {
        if (!this.dicSkill.ContainsKey(obj.id))
          this.dicSkill[obj.id] = new List<string>();
        List<string> stringList = this.dicSkill[obj.id];
        stringList.Add(obj.name0);
        stringList.Add(obj.name1);
        stringList.Add(obj.name2);
        stringList.Add(obj.name3);
        stringList.Add(obj.name4);
      }
      return true;
    }

    private bool LoadHSkillList()
    {
      string listAssetBundleName = this.listAssetBundleName;
      TitleSkillName titleSkillName = CommonLib.LoadAsset<TitleSkillName>(listAssetBundleName, "title_h_skill", false, string.Empty);
      AssetBundleManager.UnloadAssetBundle(listAssetBundleName, true, (string) null, false);
      foreach (TitleSkillName.Param obj in titleSkillName.param)
      {
        if (!this.dicHSkill.ContainsKey(obj.id))
          this.dicHSkill[obj.id] = new List<string>();
        List<string> stringList = this.dicHSkill[obj.id];
        stringList.Add(obj.name0);
        stringList.Add(obj.name1);
        stringList.Add(obj.name2);
        stringList.Add(obj.name3);
        stringList.Add(obj.name4);
      }
      return true;
    }

    public delegate void OnChangeItemFunc(GameCharaFileInfo info);
  }
}
