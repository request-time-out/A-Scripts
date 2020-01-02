// Decompiled with JetBrains decompiler
// Type: UploaderSystem.UpUIControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using CharaCustom;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UploaderSystem
{
  public class UpUIControl : MonoBehaviour
  {
    public UpPhpControl phpCtrl;
    [Header("---< タイプ別表示OBJ >-----------------------")]
    [SerializeField]
    private GameObject objCharaTop;
    [SerializeField]
    private GameObject objHousingTop;
    private GameObject[] objSexAll;
    [SerializeField]
    private GameObject[] objMale;
    [SerializeField]
    private GameObject[] objFemale;
    [SerializeField]
    private GameObject[] objHideH;
    [SerializeField]
    private Toggle tglFemale;
    [Header("---< モード・タイプ切り替え >----------------")]
    [SerializeField]
    private Toggle[] tglDataType;
    [Header("---< 選択情報・キャラ >----------------------")]
    [SerializeField]
    private UpUIControl.SelectInfoChara selInfoCha;
    [Header("---< 選択情報・ハウジング >----------------------")]
    [SerializeField]
    private UpUIControl.SelectInfoHousing selInfoHousing;
    [Header("---< アップロード >--------------------------")]
    [SerializeField]
    private UpUIControl.UploadItem uploadItem;
    [Header("---< プロフィール >--------------------------")]
    [SerializeField]
    private UpUIControl.ProfileItem profileItem;
    [Header("---< その他 >--------------------------------")]
    [SerializeField]
    private Button btnTitle;
    [SerializeField]
    private Button btnDownloader;
    [SerializeField]
    private Text textNewestVersion;
    private VoiceInfo.Param[] personalities;
    private IntReactiveProperty _dataType;
    private BoolReactiveProperty _updateCharaInfo;
    private BoolReactiveProperty _updateFemale;
    private BoolReactiveProperty _updateHousingInfo;
    private BoolReactiveProperty _updateAllInfo;
    private Dictionary<int, int> dictVoiceInfo;

    public UpUIControl()
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

    public bool updateFemale
    {
      get
      {
        return ((ReactiveProperty<bool>) this._updateFemale).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._updateFemale).set_Value(value);
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
      if (!Object.op_Inequality((Object) null, (Object) this.textNewestVersion))
        return;
      ((Component) this.textNewestVersion).get_gameObject().SetActiveIfDifferent(true);
    }

    public void ChangeUploadData()
    {
      this.uploadItem.modeUpdate = false;
      int user_idx = this.netInfo.profile.userIdx;
      switch ((DataType) this.dataType)
      {
        case DataType.Chara:
          this.updateCharaInfo = true;
          CustomCharaScrollController.ScrollData info = (CustomCharaScrollController.ScrollData) null;
          info = !this.updateFemale ? this.netInfo.selectCharaMWindow.GetSelectInfo() : this.netInfo.selectCharaFWindow.GetSelectInfo();
          if (info != null)
          {
            if (this.netInfo.lstCharaInfo.Where<NetworkInfo.CharaInfo>((Func<NetworkInfo.CharaInfo, bool>) (x => x.data_uid == info.info.data_uuid && x.user_idx == user_idx)).ToArray<NetworkInfo.CharaInfo>().Length != 0)
            {
              this.uploadItem.modeUpdate = true;
              break;
            }
            if (this.netInfo.dictUploaded[0].ContainsKey(info.info.data_uuid))
            {
              this.uploadItem.modeUpdate = true;
              break;
            }
            break;
          }
          break;
        case DataType.Housing:
          this.uploadItem.modeUpdate = false;
          this.updateHousingInfo = true;
          break;
      }
      if (!Object.op_Implicit((Object) this.uploadItem.btnUpload))
        return;
      Text componentInChildren = (Text) ((Component) this.uploadItem.btnUpload).GetComponentInChildren<Text>(true);
      if (!Object.op_Implicit((Object) componentInChildren))
        return;
      componentInChildren.set_text(!this.uploadItem.modeUpdate ? "アップロード" : "更新");
    }

    private void UpdatePreview(DataType type, string path)
    {
      Image imgThumbnail;
      switch (type)
      {
        case DataType.Chara:
          imgThumbnail = this.selInfoCha.imgThumbnail;
          break;
        case DataType.Housing:
          imgThumbnail = this.selInfoHousing.imgThumbnail;
          break;
        default:
          return;
      }
      if (Object.op_Equality((Object) null, (Object) imgThumbnail))
        return;
      if (Object.op_Inequality((Object) null, (Object) imgThumbnail.get_sprite()))
      {
        if (Object.op_Inequality((Object) null, (Object) imgThumbnail.get_sprite().get_texture()))
          Object.Destroy((Object) imgThumbnail.get_sprite().get_texture());
        Object.Destroy((Object) imgThumbnail.get_sprite());
        imgThumbnail.set_sprite((Sprite) null);
      }
      if (!path.IsNullOrEmpty())
      {
        Sprite sprite = PngAssist.LoadSpriteFromFile(path);
        if (Object.op_Inequality((Object) null, (Object) sprite))
          imgThumbnail.set_sprite(sprite);
        ((Behaviour) imgThumbnail).set_enabled(true);
      }
      else
        ((Behaviour) imgThumbnail).set_enabled(false);
    }

    public void UpdateInfoChara()
    {
      string path = string.Empty;
      CustomCharaScrollController.ScrollData scrollData = !this.updateFemale ? this.netInfo.selectCharaMWindow.GetSelectInfo() : this.netInfo.selectCharaFWindow.GetSelectInfo();
      if (scrollData != null)
      {
        CustomCharaFileInfo info = scrollData.info;
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textName))
          this.selInfoCha.textName.set_text(info.name);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textType))
          this.selInfoCha.textType.set_text(info.sex != 0 ? Singleton<Character>.Instance.GetCharaTypeName(info.type) : string.Empty);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textBirthDay))
          this.selInfoCha.textBirthDay.set_text(info.strBirthDay);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase01))
          this.selInfoCha.objOnPhase01.SetActiveIfDifferent(this.updateFemale);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase02))
          this.selInfoCha.objOnPhase02.SetActiveIfDifferent(info.phase >= 1 && this.updateFemale);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase03))
          this.selInfoCha.objOnPhase03.SetActiveIfDifferent(info.phase >= 2 && this.updateFemale);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.objOnPhase04))
          this.selInfoCha.objOnPhase04.SetActiveIfDifferent(info.phase == 3 && this.updateFemale);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textLifeStyle))
          this.selInfoCha.textLifeStyle.set_text(info.lifestyle == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetLifeStyleName(info.lifestyle));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textPheromone))
          this.selInfoCha.textPheromone.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.pheromone.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textReliability))
          this.selInfoCha.textReliability.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.reliability.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textReason))
          this.selInfoCha.textReason.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.reason.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textInstinct))
          this.selInfoCha.textInstinct.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.instinct.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textDirty))
          this.selInfoCha.textDirty.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.dirty.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWariness))
          this.selInfoCha.textWariness.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.wariness.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSociability))
          this.selInfoCha.textSociability.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.sociability.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textDarkness))
          this.selInfoCha.textDarkness.set_text(!info.gameRegistration || !this.updateFemale ? "---------------" : info.darkness.ToString());
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n01))
          this.selInfoCha.textSkill_n01.set_text(info.skill_n01 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetNormalSkillName(info.skill_n01));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n02))
          this.selInfoCha.textSkill_n02.set_text(info.skill_n02 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetNormalSkillName(info.skill_n02));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n03))
          this.selInfoCha.textSkill_n03.set_text(info.skill_n03 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetNormalSkillName(info.skill_n03));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n04))
          this.selInfoCha.textSkill_n04.set_text(info.skill_n04 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetNormalSkillName(info.skill_n04));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_n05))
          this.selInfoCha.textSkill_n05.set_text(info.skill_n05 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetNormalSkillName(info.skill_n05));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h01))
          this.selInfoCha.textSkill_h01.set_text(info.skill_h01 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetHSkillName(info.skill_h01));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h02))
          this.selInfoCha.textSkill_h02.set_text(info.skill_h02 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetHSkillName(info.skill_h02));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h03))
          this.selInfoCha.textSkill_h03.set_text(info.skill_h03 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetHSkillName(info.skill_h03));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h04))
          this.selInfoCha.textSkill_h04.set_text(info.skill_h04 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetHSkillName(info.skill_h04));
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textSkill_h05))
          this.selInfoCha.textSkill_h05.set_text(info.skill_h05 == -1 || !this.updateFemale ? "---------------" : this.netInfo.GetHSkillName(info.skill_h05));
        Dictionary<int, ListInfoBase> categoryInfo = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.init_wish_param);
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_01))
        {
          string str = "---------------";
          ListInfoBase listInfoBase;
          if (info.wish_01 != -1 && this.updateFemale && categoryInfo.TryGetValue(info.wish_01, out listInfoBase))
            str = listInfoBase.Name;
          this.selInfoCha.textWish_01.set_text(str);
        }
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_02))
        {
          string str = "---------------";
          ListInfoBase listInfoBase;
          if (info.wish_02 != -1 && this.updateFemale && categoryInfo.TryGetValue(info.wish_02, out listInfoBase))
            str = listInfoBase.Name;
          this.selInfoCha.textWish_02.set_text(str);
        }
        if (Object.op_Inequality((Object) null, (Object) this.selInfoCha.textWish_03))
        {
          string str = "---------------";
          ListInfoBase listInfoBase;
          if (info.wish_03 != -1 && this.updateFemale && categoryInfo.TryGetValue(info.wish_03, out listInfoBase))
            str = listInfoBase.Name;
          this.selInfoCha.textWish_03.set_text(str);
        }
        path = info.FullPath;
      }
      else
      {
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
      }
      this.UpdatePreview(DataType.Chara, path);
    }

    public void UpdateInfoHousing()
    {
      this.UpdatePreview(DataType.Housing, this.netInfo.selectHousingWindow.GetSelectPath());
    }

    public string GetUploadFile(DataType type)
    {
      switch (type)
      {
        case DataType.Chara:
          CustomCharaScrollController.ScrollData scrollData = !this.updateFemale ? this.netInfo.selectCharaMWindow.GetSelectInfo() : this.netInfo.selectCharaFWindow.GetSelectInfo();
          return scrollData == null ? string.Empty : scrollData.info.FullPath;
        case DataType.Housing:
          return this.netInfo.selectHousingWindow.GetSelectPath();
        default:
          return string.Empty;
      }
    }

    public string GetComment(DataType type)
    {
      return this.uploadItem.bkComment[(int) type];
    }

    public string GetTitle()
    {
      string str = "NO_TITLE";
      if (Object.op_Inequality((Object) null, (Object) this.uploadItem.inpTitle))
      {
        string text = this.uploadItem.inpTitle.get_text();
        if (!text.IsNullOrEmpty())
          str = text;
      }
      return str;
    }

    public void UpdateProfile()
    {
      this.profileItem.textHandle.set_text(Singleton<GameSystem>.Instance.HandleName);
    }

    private void Awake()
    {
      HashSet<GameObject> gameObjectSet = new HashSet<GameObject>((IEnumerable<GameObject>) this.objMale);
      gameObjectSet.UnionWith((IEnumerable<GameObject>) this.objFemale);
      this.objSexAll = ((IEnumerable<GameObject>) gameObjectSet).ToArray<GameObject>();
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new UpUIControl.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [Serializable]
    public class SelectInfoChara
    {
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
      public Image imgThumbnail;
    }

    [Serializable]
    public class SelectInfoHousing
    {
      public Image imgThumbnail;
    }

    [Serializable]
    public class UploadItem
    {
      [HideInInspector]
      public string[] bkComment = new string[Enum.GetNames(typeof (DataType)).Length];
      public InputField inpTitle;
      public InputField inpComment;
      public Toggle tglAgreePolicy;
      public Button btnPolicy;
      public Button btnUpload;
      public UIBehaviour exitPolicy;
      public GameObject objPolicy;
      [HideInInspector]
      public bool modeUpdate;
    }

    [Serializable]
    public class ProfileItem
    {
      public Text textHandle;
      public Button btnChangeHandle;
    }
  }
}
