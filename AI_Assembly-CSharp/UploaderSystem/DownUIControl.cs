// Decompiled with JetBrains decompiler
// Type: UploaderSystem.DownUIControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UploaderSystem
{
  public class DownUIControl : MonoBehaviour
  {
    public DownloadScene downScene;
    public DownPhpControl phpCtrl;
    public DownUIControl.SearchSetting[] searchSettings;
    [HideInInspector]
    public int searchSortSort;
    private IntReactiveProperty _searchSortHNIdx;
    [Header("---< モード別表示OBJ >-----------------------")]
    private GameObject[] objModeAll;
    [SerializeField]
    private GameObject[] objModeDownload;
    [SerializeField]
    private GameObject[] objModeMyData;
    [Header("---< タイプ別表示OBJ >-----------------------")]
    private GameObject[] objTypeAll;
    [SerializeField]
    private GameObject[] objTypeChara;
    [SerializeField]
    private GameObject[] objTypeHousing;
    [SerializeField]
    private GameObject[] objHideH;
    [Header("---< モード・タイプ切り替え >----------------")]
    [SerializeField]
    private Toggle[] tglMainMode;
    [SerializeField]
    private Toggle[] tglDataType;
    [Header("---< 選択情報・キャラ >----------------------")]
    [SerializeField]
    private DownUIControl.SelectInfoChara selInfoCha;
    [Header("---< 選択情報・ハウジング >----------------------")]
    [SerializeField]
    private DownUIControl.SelectInfoHousing selInfoHousing;
    [Header("---< 検索関連 >------------------------------")]
    [SerializeField]
    private DownUIControl.SearchItem searchItem;
    [Header("---< データ一覧情報 >------------------------")]
    [SerializeField]
    private DownUIControl.PageDataInfo pageDataInfo;
    [Header("---< サーバーデータ >--------------------------")]
    [SerializeField]
    private DownUIControl.ServerItem serverItem;
    [Header("---< ページ制御 >----------------------------")]
    [SerializeField]
    private DownUIControl.PageControlItem pageCtrlItem;
    private IntReactiveProperty _pageMax;
    private IntReactiveProperty _pageNow;
    [Header("---< その他 >--------------------------------")]
    [SerializeField]
    private GameObject objUpdatingThumbnailCanvas;
    [SerializeField]
    private Button btnTitle;
    [SerializeField]
    private Button btnUploader;
    [SerializeField]
    private Button btnReload;
    [SerializeField]
    private Text textNewestVersion;
    private VoiceInfo.Param[] personalities;
    private IntReactiveProperty _mainMode;
    private IntReactiveProperty _dataType;
    [HideInInspector]
    public bool changeSearchSetting;
    private BoolReactiveProperty _updateCharaInfo;
    private BoolReactiveProperty _updateHousingInfo;
    private BoolReactiveProperty _updateAllInfo;
    public DownUIControl.PageSelectInfo downloadSel;
    public List<NetworkInfo.CharaInfo> lstSearchCharaInfo;
    public List<NetworkInfo.HousingInfo> lstSearchHousingInfo;
    private Dictionary<int, int> dictVoiceInfo;
    private Dictionary<int, Tuple<int, string>> dictMapSizeInfo;
    private NetworkInfo.BaseIndex selBaseIndex;
    private bool skipToggleChange;

    public DownUIControl()
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

    private NetCacheControl cacheCtrl
    {
      get
      {
        return Singleton<NetworkInfo>.IsInstance() ? this.netInfo.cacheCtrl : (NetCacheControl) null;
      }
    }

    public int searchSortHNIdx
    {
      get
      {
        return ((ReactiveProperty<int>) this._searchSortHNIdx).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._searchSortHNIdx).set_Value(value);
      }
    }

    public int pageMax
    {
      get
      {
        return ((ReactiveProperty<int>) this._pageMax).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._pageMax).set_Value(value);
      }
    }

    public int pageNow
    {
      get
      {
        return ((ReactiveProperty<int>) this._pageNow).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._pageNow).set_Value(value);
      }
    }

    public int mainMode
    {
      get
      {
        return ((ReactiveProperty<int>) this._mainMode).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._mainMode).set_Value(value);
      }
    }

    public int dataType
    {
      get
      {
        return ((ReactiveProperty<int>) this._dataType).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._dataType).set_Value(value);
      }
    }

    public bool updateCharaInfo
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateCharaInfo).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateCharaInfo).set_Value(value);
      }
    }

    public bool updateHousingInfo
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateHousingInfo).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateHousingInfo).set_Value(value);
      }
    }

    public bool updateAllInfo
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateAllInfo).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateAllInfo).set_Value(value);
      }
    }

    public void ShowNewestVersion()
    {
      if (!Object.op_Implicit((Object) this.textNewestVersion))
        return;
      ((Component) this.textNewestVersion).get_gameObject().SetActiveIfDifferent(true);
    }

    public void UpdateInfoChara()
    {
      NetworkInfo.CharaInfo charaInfo = (NetworkInfo.CharaInfo) null;
      string str1 = string.Empty;
      switch ((DownUIControl.MainMode) this.mainMode)
      {
        case DownUIControl.MainMode.MM_Download:
        case DownUIControl.MainMode.MM_MyData:
          this.selInfoCha.hnUserIndex = -1;
          if (this.downloadSel.selChara != -1 && this.lstSearchCharaInfo.Count > this.downloadSel.selChara)
          {
            charaInfo = this.lstSearchCharaInfo[this.downloadSel.selChara];
            str1 = this.GetHandleNameFromUserIndex(charaInfo.user_idx);
            this.selInfoCha.hnUserIndex = charaInfo.user_idx;
            break;
          }
          break;
      }
      if (charaInfo == null)
      {
        if (Object.op_Implicit((Object) this.selInfoCha.textHN))
          this.selInfoCha.textHN.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textName))
          this.selInfoCha.textName.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textType))
          this.selInfoCha.textType.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textBirthDay))
          this.selInfoCha.textBirthDay.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase01))
          this.selInfoCha.objOnPhase01.SetActiveIfDifferent(false);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase02))
          this.selInfoCha.objOnPhase02.SetActiveIfDifferent(false);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase03))
          this.selInfoCha.objOnPhase03.SetActiveIfDifferent(false);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase04))
          this.selInfoCha.objOnPhase04.SetActiveIfDifferent(false);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textLifeStyle))
          this.selInfoCha.textLifeStyle.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textPheromone))
          this.selInfoCha.textPheromone.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textReliability))
          this.selInfoCha.textReliability.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textReason))
          this.selInfoCha.textReason.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textInstinct))
          this.selInfoCha.textInstinct.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textDirty))
          this.selInfoCha.textDirty.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWariness))
          this.selInfoCha.textWariness.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSociability))
          this.selInfoCha.textSociability.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textDarkness))
          this.selInfoCha.textDarkness.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n01))
          this.selInfoCha.textSkill_n01.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n02))
          this.selInfoCha.textSkill_n02.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n03))
          this.selInfoCha.textSkill_n03.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n04))
          this.selInfoCha.textSkill_n04.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n05))
          this.selInfoCha.textSkill_n05.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h01))
          this.selInfoCha.textSkill_h01.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h02))
          this.selInfoCha.textSkill_h02.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h03))
          this.selInfoCha.textSkill_h03.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h04))
          this.selInfoCha.textSkill_h04.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h05))
          this.selInfoCha.textSkill_h05.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_01))
          this.selInfoCha.textWish_01.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_02))
          this.selInfoCha.textWish_02.set_text(string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_03))
          this.selInfoCha.textWish_03.set_text(string.Empty);
        if (!Object.op_Implicit((Object) this.selInfoCha.textComment))
          return;
        this.selInfoCha.textComment.set_text(string.Empty);
      }
      else
      {
        bool active = 1 == charaInfo.sex;
        if (Object.op_Implicit((Object) this.selInfoCha.textHN))
          this.selInfoCha.textHN.set_text(str1);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textName))
          this.selInfoCha.textName.set_text(charaInfo.name);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textType))
          this.selInfoCha.textType.set_text(active ? Singleton<Character>.Instance.GetCharaTypeName(charaInfo.type) : string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textBirthDay))
          this.selInfoCha.textBirthDay.set_text(charaInfo.strBirthDay);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase01))
          this.selInfoCha.objOnPhase01.SetActiveIfDifferent(active);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase02))
          this.selInfoCha.objOnPhase02.SetActiveIfDifferent(charaInfo.phase >= 1 && active);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase03))
          this.selInfoCha.objOnPhase03.SetActiveIfDifferent(charaInfo.phase >= 2 && active);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase04))
          this.selInfoCha.objOnPhase04.SetActiveIfDifferent(charaInfo.phase == 3 && active);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textLifeStyle))
          this.selInfoCha.textLifeStyle.set_text(charaInfo.lifestyle == -1 || !active ? "---------------" : this.netInfo.GetLifeStyleName(charaInfo.lifestyle));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textPheromone))
          this.selInfoCha.textPheromone.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.pheromone.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textReliability))
          this.selInfoCha.textReliability.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.reliability.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textReason))
          this.selInfoCha.textReason.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.reason.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textInstinct))
          this.selInfoCha.textInstinct.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.instinct.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textDirty))
          this.selInfoCha.textDirty.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.dirty.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWariness))
          this.selInfoCha.textWariness.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.wariness.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSociability))
          this.selInfoCha.textSociability.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.sociability.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textDarkness))
          this.selInfoCha.textDarkness.set_text(charaInfo.registration == 0 || !active ? "---------------" : charaInfo.darkness.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n01))
          this.selInfoCha.textSkill_n01.set_text(charaInfo.skill_n01 == -1 || !active ? "---------------" : this.netInfo.GetNormalSkillName(charaInfo.skill_n01));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n02))
          this.selInfoCha.textSkill_n02.set_text(charaInfo.skill_n02 == -1 || !active ? "---------------" : this.netInfo.GetNormalSkillName(charaInfo.skill_n02));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n03))
          this.selInfoCha.textSkill_n03.set_text(charaInfo.skill_n03 == -1 || !active ? "---------------" : this.netInfo.GetNormalSkillName(charaInfo.skill_n03));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n04))
          this.selInfoCha.textSkill_n04.set_text(charaInfo.skill_n04 == -1 || !active ? "---------------" : this.netInfo.GetNormalSkillName(charaInfo.skill_n04));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n05))
          this.selInfoCha.textSkill_n05.set_text(charaInfo.skill_n05 == -1 || !active ? "---------------" : this.netInfo.GetNormalSkillName(charaInfo.skill_n05));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h01))
          this.selInfoCha.textSkill_h01.set_text(charaInfo.skill_h01 == -1 || !active ? "---------------" : this.netInfo.GetHSkillName(charaInfo.skill_h01));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h02))
          this.selInfoCha.textSkill_h02.set_text(charaInfo.skill_h02 == -1 || !active ? "---------------" : this.netInfo.GetHSkillName(charaInfo.skill_h02));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h03))
          this.selInfoCha.textSkill_h03.set_text(charaInfo.skill_h03 == -1 || !active ? "---------------" : this.netInfo.GetHSkillName(charaInfo.skill_h03));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h04))
          this.selInfoCha.textSkill_h04.set_text(charaInfo.skill_h04 == -1 || !active ? "---------------" : this.netInfo.GetHSkillName(charaInfo.skill_h04));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h05))
          this.selInfoCha.textSkill_h05.set_text(charaInfo.skill_h05 == -1 || !active ? "---------------" : this.netInfo.GetHSkillName(charaInfo.skill_h05));
        Dictionary<int, ListInfoBase> categoryInfo = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.init_wish_param);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_01))
        {
          string str2 = "---------------";
          ListInfoBase listInfoBase;
          if (charaInfo.wish_01 != -1 && active && categoryInfo.TryGetValue(charaInfo.wish_01, out listInfoBase))
            str2 = listInfoBase.Name;
          this.selInfoCha.textWish_01.set_text(str2);
        }
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_02))
        {
          string str2 = "---------------";
          ListInfoBase listInfoBase;
          if (charaInfo.wish_02 != -1 && active && categoryInfo.TryGetValue(charaInfo.wish_02, out listInfoBase))
            str2 = listInfoBase.Name;
          this.selInfoCha.textWish_02.set_text(str2);
        }
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_03))
        {
          string str2 = "---------------";
          ListInfoBase listInfoBase;
          if (charaInfo.wish_03 != -1 && active && categoryInfo.TryGetValue(charaInfo.wish_03, out listInfoBase))
            str2 = listInfoBase.Name;
          this.selInfoCha.textWish_03.set_text(str2);
        }
        if (!Object.op_Implicit((Object) this.selInfoCha.textComment))
          return;
        this.selInfoCha.textComment.set_text(charaInfo.comment);
      }
    }

    public void UpdateInfoHousing()
    {
      NetworkInfo.HousingInfo housinginfo = (NetworkInfo.HousingInfo) null;
      string str = string.Empty;
      switch ((DownUIControl.MainMode) this.mainMode)
      {
        case DownUIControl.MainMode.MM_Download:
        case DownUIControl.MainMode.MM_MyData:
          this.selInfoHousing.hnUserIndex = -1;
          if (this.downloadSel.selHousing != -1 && this.lstSearchHousingInfo.Count > this.downloadSel.selHousing)
          {
            housinginfo = this.lstSearchHousingInfo[this.downloadSel.selHousing];
            str = this.GetHandleNameFromUserIndex(housinginfo.user_idx);
            this.selInfoHousing.hnUserIndex = housinginfo.user_idx;
            break;
          }
          break;
      }
      if (housinginfo == null)
      {
        if (Object.op_Implicit((Object) this.selInfoHousing.textHN))
          this.selInfoHousing.textHN.set_text(string.Empty);
        if (Object.op_Implicit((Object) this.selInfoHousing.textName))
          this.selInfoHousing.textName.set_text(string.Empty);
        if (Object.op_Implicit((Object) this.selInfoHousing.textMapSize))
          this.selInfoHousing.textMapSize.set_text(string.Empty);
        if (!Object.op_Implicit((Object) this.selInfoHousing.textComment))
          return;
        this.selInfoHousing.textComment.set_text(string.Empty);
      }
      else
      {
        if (Object.op_Implicit((Object) this.selInfoHousing.textHN))
          this.selInfoHousing.textHN.set_text(str);
        if (Object.op_Implicit((Object) this.selInfoHousing.textName))
          this.selInfoHousing.textName.set_text(housinginfo.name);
        if (Object.op_Implicit((Object) this.selInfoHousing.textMapSize))
        {
          KeyValuePair<int, Tuple<int, string>> keyValuePair = this.dictMapSizeInfo.FirstOrDefault<KeyValuePair<int, Tuple<int, string>>>((Func<KeyValuePair<int, Tuple<int, string>>, bool>) (x => x.Value.Item1 == housinginfo.mapSize));
          if (keyValuePair.Equals((object) null))
            this.selInfoHousing.textMapSize.set_text("不明");
          else
            this.selInfoHousing.textMapSize.set_text(keyValuePair.Value.Item2);
        }
        if (!Object.op_Implicit((Object) this.selInfoHousing.textComment))
          return;
        this.selInfoHousing.textComment.set_text(housinginfo.comment);
      }
    }

    public void UpdateCharaSearchList()
    {
      int num1 = -1;
      int selChara = this.downloadSel.selChara;
      if (this.downloadSel.selChara != -1)
        num1 = this.lstSearchCharaInfo[this.downloadSel.selChara].idx;
      this.lstSearchCharaInfo.Clear();
      bool[] flagArray1 = new bool[2];
      bool flag1 = false;
      for (int index = 0; index < 2; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.searchItem.tglSex[index]))
        {
          flagArray1[index] = this.searchItem.tglSex[index].get_isOn();
          if (flagArray1[index])
            flag1 = true;
        }
      }
      bool[] flagArray2 = new bool[3];
      bool flag2 = false;
      for (int index = 0; index < 3; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.searchItem.tglHeight[index]))
        {
          flagArray2[index] = this.searchItem.tglHeight[index].get_isOn();
          if (flagArray2[index])
            flag2 = true;
        }
      }
      bool[] flagArray3 = new bool[3];
      bool flag3 = false;
      for (int index = 0; index < 3; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.searchItem.tglBust[index]))
        {
          flagArray3[index] = this.searchItem.tglBust[index].get_isOn();
          if (flagArray3[index])
            flag3 = true;
        }
      }
      bool[] flagArray4 = new bool[6];
      bool flag4 = false;
      for (int index = 0; index < 6; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.searchItem.tglHair[index]))
        {
          flagArray4[index] = this.searchItem.tglHair[index].get_isOn();
          if (flagArray4[index])
            flag4 = true;
        }
      }
      bool flag5 = false;
      List<int> intList = new List<int>();
      foreach (KeyValuePair<int, int> keyValuePair in this.dictVoiceInfo)
      {
        int key = keyValuePair.Key;
        if (!Object.op_Equality((Object) null, (Object) this.searchItem.tglVoice[key]) && this.searchItem.tglVoice[key].get_isOn())
        {
          intList.Add(keyValuePair.Value);
          flag5 = true;
        }
      }
      int num2 = this.mainMode != 1 ? this.searchSortHNIdx : this.netInfo.profile.userIdx;
      int count = this.netInfo.lstCharaInfo.Count;
      for (int index = 0; index < count; ++index)
      {
        NetworkInfo.CharaInfo charaInfo = this.netInfo.lstCharaInfo[index];
        if (MathfEx.IsRange<int>(0, charaInfo.sex, 1, true) && (!flag1 || flagArray1[charaInfo.sex]) && (charaInfo.sex != 0 || !flag5 && !flag3 && !flag2) && (MathfEx.IsRange<int>(0, charaInfo.height, flagArray2.Length - 1, true) && (!flag2 || flagArray2[charaInfo.height]) && ((charaInfo.sex != 1 || MathfEx.IsRange<int>(0, charaInfo.bust, flagArray3.Length - 1, true)) && (!flag3 || flagArray3[charaInfo.bust])) && (MathfEx.IsRange<int>(0, charaInfo.hair, flagArray4.Length - 1, true) && (!flag4 || flagArray4[charaInfo.hair]) && ((!flag5 || intList.Contains(charaInfo.type)) && (num2 == -1 || num2 == charaInfo.user_idx)))))
        {
          if (this.searchItem.textKeywordDummy.get_text() != string.Empty)
          {
            string text = this.searchItem.textKeywordDummy.get_text();
            if (!charaInfo.name.Contains(text) && !charaInfo.comment.Contains(text))
              continue;
          }
          this.lstSearchCharaInfo.Add(charaInfo);
        }
      }
      this.UpdateSortChara();
      this.UpdatePageMax();
      this.downloadSel.selChara = -1;
      for (int index = 0; index < this.lstSearchCharaInfo.Count; ++index)
      {
        if (this.lstSearchCharaInfo[index].idx == num1)
        {
          this.downloadSel.selChara = index;
          break;
        }
      }
      int index1 = this.CheckSelectInPage(this.downloadSel.selChara);
      if (index1 == -1)
      {
        this.downloadSel.selChara = -1;
        if (selChara != -1)
        {
          foreach (NetFileComponent netFileComponent in this.pageDataInfo.nfcChara)
            netFileComponent.tglItem.set_isOn(false);
        }
      }
      else
        this.pageDataInfo.nfcChara[index1].tglItem.set_isOn(true);
      this.UpdateInfoChara();
      this.UpdatePage();
    }

    public void UpdateSortChara()
    {
      switch (this.searchSortSort)
      {
        case 0:
          this.lstSearchCharaInfo = this.lstSearchCharaInfo.OrderByDescending<NetworkInfo.CharaInfo, DateTime>((Func<NetworkInfo.CharaInfo, DateTime>) (n => n.createTime)).ThenBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.idx)).ToList<NetworkInfo.CharaInfo>();
          break;
        case 1:
          this.lstSearchCharaInfo = this.lstSearchCharaInfo.OrderBy<NetworkInfo.CharaInfo, DateTime>((Func<NetworkInfo.CharaInfo, DateTime>) (n => n.createTime)).ThenBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.idx)).ToList<NetworkInfo.CharaInfo>();
          break;
        case 2:
          this.lstSearchCharaInfo = this.lstSearchCharaInfo.OrderByDescending<NetworkInfo.CharaInfo, DateTime>((Func<NetworkInfo.CharaInfo, DateTime>) (n => n.updateTime)).ThenBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.idx)).ToList<NetworkInfo.CharaInfo>();
          break;
        case 3:
          this.lstSearchCharaInfo = this.lstSearchCharaInfo.OrderBy<NetworkInfo.CharaInfo, DateTime>((Func<NetworkInfo.CharaInfo, DateTime>) (n => n.updateTime)).ThenBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.idx)).ToList<NetworkInfo.CharaInfo>();
          break;
        case 4:
          this.lstSearchCharaInfo = this.lstSearchCharaInfo.OrderBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.rankingT)).ThenBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.idx)).ToList<NetworkInfo.CharaInfo>();
          break;
        case 5:
          this.lstSearchCharaInfo = this.lstSearchCharaInfo.OrderBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.rankingW)).ThenBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.idx)).ToList<NetworkInfo.CharaInfo>();
          break;
        case 6:
          this.lstSearchCharaInfo = this.lstSearchCharaInfo.OrderByDescending<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.applause)).ThenBy<NetworkInfo.CharaInfo, int>((Func<NetworkInfo.CharaInfo, int>) (n => n.idx)).ToList<NetworkInfo.CharaInfo>();
          break;
      }
    }

    public void UpdateHousingSearchList()
    {
      int num1 = -1;
      int selHousing = this.downloadSel.selHousing;
      if (this.downloadSel.selHousing != -1)
        num1 = this.lstSearchHousingInfo[this.downloadSel.selHousing].idx;
      this.lstSearchHousingInfo.Clear();
      bool flag = false;
      HashSet<int> intSet1 = new HashSet<int>();
      HashSet<int> intSet2 = new HashSet<int>();
      foreach (KeyValuePair<int, Tuple<int, string>> keyValuePair in this.dictMapSizeInfo)
      {
        int key = keyValuePair.Key;
        if (!Object.op_Equality((Object) null, (Object) this.searchItem.tglMapSize[key]))
        {
          intSet1.Add(keyValuePair.Value.Item1);
          if (this.searchItem.tglMapSize[key].get_isOn())
          {
            intSet2.Add(keyValuePair.Value.Item1);
            flag = true;
          }
        }
      }
      int num2 = this.mainMode != 1 ? this.searchSortHNIdx : this.netInfo.profile.userIdx;
      int count = this.netInfo.lstHousingInfo.Count;
      for (int index = 0; index < count; ++index)
      {
        NetworkInfo.HousingInfo housingInfo = this.netInfo.lstHousingInfo[index];
        if (flag)
        {
          if (!intSet2.Contains(housingInfo.mapSize))
            continue;
        }
        else if (!intSet1.Contains(housingInfo.mapSize))
          continue;
        if (num2 == -1 || num2 == housingInfo.user_idx)
        {
          if (this.searchItem.textKeywordDummy.get_text() != string.Empty)
          {
            string text = this.searchItem.textKeywordDummy.get_text();
            if (!housingInfo.name.Contains(text) && !housingInfo.comment.Contains(text))
              continue;
          }
          this.lstSearchHousingInfo.Add(housingInfo);
        }
      }
      this.UpdateSortScene();
      this.UpdatePageMax();
      this.downloadSel.selHousing = -1;
      for (int index = 0; index < this.lstSearchHousingInfo.Count; ++index)
      {
        if (this.lstSearchHousingInfo[index].idx == num1)
        {
          this.downloadSel.selHousing = index;
          break;
        }
      }
      int index1 = this.CheckSelectInPage(this.downloadSel.selHousing);
      if (index1 == -1)
      {
        this.downloadSel.selHousing = -1;
        if (selHousing != -1)
        {
          foreach (NetFileComponent netFileComponent in this.pageDataInfo.nfcHousing)
            netFileComponent.tglItem.set_isOn(false);
        }
      }
      else
        this.pageDataInfo.nfcHousing[index1].tglItem.set_isOn(true);
      this.UpdateInfoHousing();
      this.UpdatePage();
    }

    public void UpdateSortScene()
    {
      switch (this.searchSortSort)
      {
        case 0:
          this.lstSearchHousingInfo = this.lstSearchHousingInfo.OrderByDescending<NetworkInfo.HousingInfo, DateTime>((Func<NetworkInfo.HousingInfo, DateTime>) (n => n.createTime)).ThenBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.idx)).ToList<NetworkInfo.HousingInfo>();
          break;
        case 1:
          this.lstSearchHousingInfo = this.lstSearchHousingInfo.OrderBy<NetworkInfo.HousingInfo, DateTime>((Func<NetworkInfo.HousingInfo, DateTime>) (n => n.createTime)).ThenBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.idx)).ToList<NetworkInfo.HousingInfo>();
          break;
        case 2:
          this.lstSearchHousingInfo = this.lstSearchHousingInfo.OrderByDescending<NetworkInfo.HousingInfo, DateTime>((Func<NetworkInfo.HousingInfo, DateTime>) (n => n.updateTime)).ThenBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.idx)).ToList<NetworkInfo.HousingInfo>();
          break;
        case 3:
          this.lstSearchHousingInfo = this.lstSearchHousingInfo.OrderBy<NetworkInfo.HousingInfo, DateTime>((Func<NetworkInfo.HousingInfo, DateTime>) (n => n.updateTime)).ThenBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.idx)).ToList<NetworkInfo.HousingInfo>();
          break;
        case 4:
          this.lstSearchHousingInfo = this.lstSearchHousingInfo.OrderBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.rankingT)).ThenBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.idx)).ToList<NetworkInfo.HousingInfo>();
          break;
        case 5:
          this.lstSearchHousingInfo = this.lstSearchHousingInfo.OrderBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.rankingW)).ThenBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.idx)).ToList<NetworkInfo.HousingInfo>();
          break;
        case 6:
          this.lstSearchHousingInfo = this.lstSearchHousingInfo.OrderByDescending<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.applause)).ThenBy<NetworkInfo.HousingInfo, int>((Func<NetworkInfo.HousingInfo, int>) (n => n.idx)).ToList<NetworkInfo.HousingInfo>();
          break;
      }
    }

    private int GetPageDataCount()
    {
      switch (this.dataType)
      {
        case 0:
          return this.lstSearchCharaInfo.Count;
        case 1:
          return this.lstSearchHousingInfo.Count;
        default:
          return 0;
      }
    }

    private int GetPageDrawCount()
    {
      switch (this.dataType)
      {
        case 0:
          return this.pageDataInfo.nfcChara.Length;
        case 1:
          return this.pageDataInfo.nfcHousing.Length;
        default:
          return 0;
      }
    }

    private NetFileComponent[] GetPageFileComponents()
    {
      switch (this.dataType)
      {
        case 0:
          return this.pageDataInfo.nfcChara;
        case 1:
          return this.pageDataInfo.nfcHousing;
        default:
          return (NetFileComponent[]) null;
      }
    }

    private List<NetworkInfo.BaseIndex> GetBaseIndexListFromSearch()
    {
      switch (this.dataType)
      {
        case 0:
          return this.lstSearchCharaInfo.Count == 0 ? (List<NetworkInfo.BaseIndex>) null : this.lstSearchCharaInfo.OfType<NetworkInfo.BaseIndex>().ToList<NetworkInfo.BaseIndex>();
        case 1:
          return this.lstSearchHousingInfo.Count == 0 ? (List<NetworkInfo.BaseIndex>) null : this.lstSearchHousingInfo.OfType<NetworkInfo.BaseIndex>().ToList<NetworkInfo.BaseIndex>();
        default:
          return (List<NetworkInfo.BaseIndex>) null;
      }
    }

    private void UpdatePageMax()
    {
      int pageDrawCount = this.GetPageDrawCount();
      int pageDataCount = this.GetPageDataCount();
      this.pageMax = Mathf.Max(pageDataCount / pageDrawCount + (pageDataCount % pageDrawCount != 0 ? 1 : 0), 1);
      if (this.pageNow < this.pageMax)
        return;
      this.pageNow = 0;
    }

    private int CheckSelectInPage(int _sel)
    {
      int pageDrawCount = this.GetPageDrawCount();
      int pageDataCount = this.GetPageDataCount();
      int num1 = this.pageNow * pageDrawCount;
      for (int index = 0; index < pageDrawCount; ++index)
      {
        int num2 = num1 + index;
        if (pageDataCount > num2)
        {
          if (num2 == _sel)
            return num2 - num1;
        }
        else
          break;
      }
      return -1;
    }

    private void UpdatePage()
    {
      NetFileComponent[] cmpNetFile = this.GetPageFileComponents();
      if (cmpNetFile == null)
        return;
      int length = cmpNetFile.Length;
      this.pageCtrlItem.drawIndex = new int[length];
      this.pageCtrlItem.updateIndex = new int[length];
      for (int index = 0; index < length; ++index)
      {
        cmpNetFile[index].SetState(false, false);
        this.pageCtrlItem.drawIndex[index] = -1;
        this.pageCtrlItem.updateIndex[index] = 0;
      }
      int pageDataCount = this.GetPageDataCount();
      if (pageDataCount == 0)
        return;
      List<NetworkInfo.BaseIndex> lstBaseIdx = this.GetBaseIndexListFromSearch();
      int num = this.pageNow * length;
      for (int index1 = 0; index1 < length; ++index1)
      {
        int index = num + index1;
        if (pageDataCount > index)
        {
          this.pageCtrlItem.drawIndex[index1] = lstBaseIdx[index].idx;
          this.pageCtrlItem.updateIndex[index1] = lstBaseIdx[index].update_idx;
          cmpNetFile[index1].SetState(true, true);
          cmpNetFile[index1].UpdateSortType(this.searchSortSort);
          switch (this.searchSortSort)
          {
            case 0:
              cmpNetFile[index1].SetUpdateTime(lstBaseIdx[index].createTime, 0);
              break;
            case 1:
              cmpNetFile[index1].SetUpdateTime(lstBaseIdx[index].createTime, 0);
              break;
            case 2:
              cmpNetFile[index1].SetUpdateTime(lstBaseIdx[index].updateTime, 1);
              break;
            case 3:
              cmpNetFile[index1].SetUpdateTime(lstBaseIdx[index].updateTime, 1);
              break;
            case 4:
              cmpNetFile[index1].SetRanking(lstBaseIdx[index].rankingT + 1);
              break;
            case 5:
              cmpNetFile[index1].SetRanking(lstBaseIdx[index].rankingW + 1);
              break;
            case 6:
              cmpNetFile[index1].SetApplauseNum(lstBaseIdx[index].applause);
              break;
          }
          if (Singleton<GameSystem>.Instance.IsApplause((DataType) this.dataType, lstBaseIdx[index].data_uid))
          {
            cmpNetFile[index1].actApplause = (Action) null;
          }
          else
          {
            int no = index1;
            cmpNetFile[index1].actApplause = (Action) (() =>
            {
              EventSystem.get_current().SetSelectedGameObject((GameObject) null);
              Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
              this.netInfo.BlockUI();
              ObservableExtensions.Subscribe<bool>(Observable.FromCoroutine<bool>((Func<IObserver<M0>, IEnumerator>) (res => this.phpCtrl.AddApplauseCount(res, (DataType) this.dataType, lstBaseIdx[index]))), (Action<M0>) (res =>
              {
                Singleton<GameSystem>.Instance.AddApplause((DataType) this.dataType, lstBaseIdx[index].data_uid);
                ++lstBaseIdx[index].applause;
                this.changeSearchSetting = true;
                cmpNetFile[no].SetApplauseNum(lstBaseIdx[index].applause);
              }), (Action<Exception>) (err => this.netInfo.UnblockUI()), (Action) (() => this.netInfo.UnblockUI()));
            });
          }
        }
        else
          break;
      }
      if (!this.pageCtrlItem.updatingThumb)
      {
        this.pageCtrlItem.updatingThumb = true;
        this.objUpdatingThumbnailCanvas.SetActiveIfDifferent(true);
        ObservableExtensions.Subscribe<bool>(Observable.FromCoroutine<bool>((Func<IObserver<M0>, IEnumerator>) (res => this.phpCtrl.GetThumbnail(res, (DataType) this.dataType))), (Action<M0>) (__ => {}), (Action<Exception>) (err =>
        {
          this.pageCtrlItem.updatingThumb = false;
          this.objUpdatingThumbnailCanvas.SetActiveIfDifferent(false);
          this.netInfo.popupMsg.EndMessage();
        }), (Action) (() =>
        {
          this.pageCtrlItem.updatingThumb = false;
          this.objUpdatingThumbnailCanvas.SetActiveIfDifferent(false);
          this.netInfo.popupMsg.EndMessage();
        }));
      }
      else
        this.pageCtrlItem.updateThumb = true;
    }

    public void ChangeThumbnail(byte[][] data)
    {
      NetFileComponent[] pageFileComponents = this.GetPageFileComponents();
      if (pageFileComponents == null)
        return;
      int length = data.GetLength(0);
      for (int index = 0; index < pageFileComponents.Length; ++index)
      {
        Texture tex = index >= length ? (Texture) null : (data[index] != null ? (Texture) PngAssist.ChangeTextureFromByte(data[index], 0, 0, (TextureFormat) 5, false) : (Texture) null);
        pageFileComponents[index].SetImage(tex);
      }
    }

    public void UpdateSearchSettingUI()
    {
      DownUIControl.SearchSetting searchSetting = this.searchSettings[this.dataType];
      int searchSortSort = this.searchSortSort;
      for (int index = 0; index < this.searchItem.tglSortType.Length; ++index)
      {
        if (Object.op_Implicit((Object) this.searchItem.tglSortType[index]))
          this.searchItem.tglSortType[index].set_isOn(false);
      }
      this.searchItem.tglSortType[searchSortSort].set_isOn(true);
      for (int index = 0; index < this.searchItem.tglSex.Length; ++index)
      {
        if (Object.op_Implicit((Object) this.searchItem.tglSex[index]))
          this.searchItem.tglSex[index].set_isOn(searchSetting.sex[index]);
      }
      for (int index = 0; index < this.searchItem.tglHeight.Length; ++index)
      {
        if (Object.op_Implicit((Object) this.searchItem.tglHeight[index]))
          this.searchItem.tglHeight[index].set_isOn(searchSetting.height[index]);
      }
      for (int index = 0; index < this.searchItem.tglBust.Length; ++index)
      {
        if (Object.op_Implicit((Object) this.searchItem.tglBust[index]))
          this.searchItem.tglBust[index].set_isOn(searchSetting.bust[index]);
      }
      for (int index = 0; index < this.searchItem.tglHair.Length; ++index)
      {
        if (Object.op_Implicit((Object) this.searchItem.tglHair[index]))
          this.searchItem.tglHair[index].set_isOn(searchSetting.hair[index]);
      }
      for (int index = 0; index < this.searchItem.tglVoice.Length; ++index)
      {
        if (Object.op_Implicit((Object) this.searchItem.tglVoice[index]))
          this.searchItem.tglVoice[index].set_isOn(searchSetting.voice[index]);
      }
      for (int index = 0; index < this.searchItem.tglMapSize.Length; ++index)
      {
        if (Object.op_Implicit((Object) this.searchItem.tglMapSize[index]))
          this.searchItem.tglMapSize[index].set_isOn(searchSetting.mapSize[index]);
      }
    }

    public Tuple<int, int>[] GetThumbnailIndex(DataType type)
    {
      Tuple<int, int>[] tupleArray = new Tuple<int, int>[((IEnumerable<int>) this.pageCtrlItem.drawIndex).Count<int>((Func<int, bool>) (x => -1 != x))];
      for (int index = 0; index < tupleArray.Length; ++index)
        tupleArray[index] = new Tuple<int, int>(this.pageCtrlItem.drawIndex[index], this.pageCtrlItem.updateIndex[index]);
      return tupleArray;
    }

    public NetworkInfo.BaseIndex GetSelectServerInfo(DataType type)
    {
      int index = new int[2]
      {
        this.downloadSel.selChara,
        this.downloadSel.selHousing
      }[(int) type];
      if (index == -1)
        return (NetworkInfo.BaseIndex) null;
      return this.GetBaseIndexListFromSearch()?[index];
    }

    public string GetHandleNameFromUserIndex(int index)
    {
      NetworkInfo.UserInfo userInfo;
      return !this.netInfo.dictUserInfo.TryGetValue(index, out userInfo) ? "不明" : userInfo.handleName;
    }

    public void UpdateDownloadList()
    {
      switch (this.dataType)
      {
        case 0:
          this.UpdateCharaSearchList();
          break;
        case 1:
          this.UpdateHousingSearchList();
          break;
      }
    }

    private void SaveDownloadFile(byte[] bytes, NetworkInfo.BaseIndex info)
    {
      switch ((DataType) this.dataType)
      {
        case DataType.Chara:
          NetworkInfo.CharaInfo charaInfo = info as NetworkInfo.CharaInfo;
          string[] strArray = new string[2]
          {
            UserData.Path + "chara/male/",
            UserData.Path + "chara/female/"
          };
          string str1 = new string[2]
          {
            "AISChaM_",
            "AISChaF_"
          }[charaInfo.sex] + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
          using (FileStream fileStream = new FileStream(strArray[charaInfo.sex] + str1, FileMode.Create, FileAccess.Write))
          {
            using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
            {
              binaryWriter.Write(bytes);
              break;
            }
          }
        case DataType.Housing:
          int mapSize = (info as NetworkInfo.HousingInfo).mapSize;
          DateTime now = DateTime.Now;
          string str2 = string.Format("{0}_{1:00}{2:00}_{3:00}{4:00}_{5:00}_{6:000}.png", (object) now.Year, (object) now.Month, (object) now.Day, (object) now.Hour, (object) now.Minute, (object) now.Second, (object) now.Millisecond);
          using (FileStream fileStream = new FileStream(UserData.Create(string.Format("{0}{1:00}", (object) "housing/", (object) (mapSize + 1))) + str2, FileMode.Create, FileAccess.Write))
          {
            using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
            {
              binaryWriter.Write(bytes);
              break;
            }
          }
      }
    }

    private void Awake()
    {
      HashSet<GameObject> gameObjectSet1 = new HashSet<GameObject>((IEnumerable<GameObject>) this.objTypeChara);
      gameObjectSet1.UnionWith((IEnumerable<GameObject>) this.objTypeHousing);
      this.objTypeAll = ((IEnumerable<GameObject>) gameObjectSet1).ToArray<GameObject>();
      HashSet<GameObject> gameObjectSet2 = new HashSet<GameObject>((IEnumerable<GameObject>) this.objModeDownload);
      gameObjectSet2.UnionWith((IEnumerable<GameObject>) this.objModeMyData);
      this.objModeAll = ((IEnumerable<GameObject>) gameObjectSet2).ToArray<GameObject>();
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DownUIControl.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public enum MainMode
    {
      MM_Download,
      MM_MyData,
    }

    public enum SortType
    {
      ST_NEW_C,
      ST_OLD_C,
      ST_NEW_U,
      ST_OLD_U,
      ST_DL_ALL,
      ST_DL_WEEK,
      ST_APPLAUSE,
    }

    public class SearchSetting
    {
      public bool[] sex = new bool[2];
      public bool[] height = new bool[3];
      public bool[] bust = new bool[3];
      public bool[] hair = new bool[6];
      public bool[] voice;
      public bool[] mapSize;

      public void Reset(bool excludeSort = true)
      {
        for (int index = 0; index < this.sex.Length; ++index)
          this.sex[index] = false;
        for (int index = 0; index < this.height.Length; ++index)
          this.height[index] = false;
        for (int index = 0; index < this.bust.Length; ++index)
          this.bust[index] = false;
        for (int index = 0; index < this.hair.Length; ++index)
          this.hair[index] = false;
        if (this.voice != null)
        {
          for (int index = 0; index < this.voice.Length; ++index)
            this.voice[index] = false;
        }
        if (this.mapSize == null)
          return;
        for (int index = 0; index < this.mapSize.Length; ++index)
          this.mapSize[index] = false;
      }
    }

    [Serializable]
    public class SelectInfoChara
    {
      [HideInInspector]
      public int hnUserIndex = -1;
      public Text textHN;
      public Text textName;
      public Text textType;
      public Text textBirthDay;
      public GameObject objOnPhase01;
      public GameObject objOnPhase02;
      public GameObject objOnPhase03;
      public GameObject objOnPhase04;
      public Text textLifeStyle;
      public Text textPheromone;
      public Text textReliability;
      public Text textReason;
      public Text textInstinct;
      public Text textDirty;
      public Text textWariness;
      public Text textSociability;
      public Text textDarkness;
      public Text textSkill_n01;
      public Text textSkill_n02;
      public Text textSkill_n03;
      public Text textSkill_n04;
      public Text textSkill_n05;
      public Text textSkill_h01;
      public Text textSkill_h02;
      public Text textSkill_h03;
      public Text textSkill_h04;
      public Text textSkill_h05;
      public Text textWish_01;
      public Text textWish_02;
      public Text textWish_03;
      public Text textComment;
      public Button btnHN;
      public Button btnHNReset;
      public Image imgThumbnail;
    }

    [Serializable]
    public class SelectInfoHousing
    {
      [HideInInspector]
      public int hnUserIndex = -1;
      public Text textHN;
      public Text textName;
      public Text textMapSize;
      public Text textComment;
      public Button btnHN;
      public Button btnHNReset;
      public Image imgThumbnail;
    }

    [Serializable]
    public class SearchItem
    {
      public Button btnResetSearchSetting;
      public Toggle[] tglSortType;
      public Button btnHNOpen;
      public Button btnHNOpenEx;
      public Button btnHNReset;
      public Text textHN;
      public Toggle[] tglSex;
      public Toggle[] tglHeight;
      public Toggle[] tglBust;
      public Toggle[] tglHair;
      [HideInInspector]
      public Toggle[] tglVoice;
      public GameObject objTempVoice;
      [HideInInspector]
      public Toggle[] tglMapSize;
      public GameObject objTempMapSize;
      public InputField inpKeyword;
      public GameObject objKeywordDummy;
      public Text textKeywordDummy;
      public Button btnKeywordReset;
    }

    [Serializable]
    public class PageDataInfo
    {
      public NetFileComponent[] nfcChara;
      public NetFileComponent[] nfcHousing;
    }

    [Serializable]
    public class ServerItem
    {
      public Button btnDownload;
      public Button btnDeleteCache;
      public Button btnDelete;
    }

    [Serializable]
    public class PageControlItem
    {
      public Button[] btnCtrlPage;
      public Text textPageMax;
      public InputField InpPage;
      [HideInInspector]
      public bool updatingThumb;
      [HideInInspector]
      public bool updateThumb;
      [HideInInspector]
      public int[] drawIndex;
      [HideInInspector]
      public int[] updateIndex;
    }

    public class PageSelectInfo
    {
      public int selChara = -1;
      public int selHousing = -1;
    }
  }
}
