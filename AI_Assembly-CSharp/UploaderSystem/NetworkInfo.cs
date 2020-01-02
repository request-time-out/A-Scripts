// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetworkInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using CharaCustom;
using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UploaderSystem
{
  public class NetworkInfo : Singleton<NetworkInfo>
  {
    public NetworkInfo.Profile profile = new NetworkInfo.Profile();
    public Dictionary<int, NetworkInfo.UserInfo> dictUserInfo = new Dictionary<int, NetworkInfo.UserInfo>();
    public List<NetworkInfo.CharaInfo> lstCharaInfo = new List<NetworkInfo.CharaInfo>();
    public List<NetworkInfo.HousingInfo> lstHousingInfo = new List<NetworkInfo.HousingInfo>();
    [HideInInspector]
    public Version newestVersion = new Version(0, 0, 0);
    public Dictionary<string, int>[] dictUploaded = new Dictionary<string, int>[Enum.GetNames(typeof (DataType)).Length];
    [SerializeField]
    public Net_PopupMsg popupMsg;
    [SerializeField]
    public LogView logview;
    [SerializeField]
    public Net_PopupCheck popupCheck;
    [SerializeField]
    public NetCacheControl cacheCtrl;
    [SerializeField]
    public NetSelectHNScrollController netSelectHN;
    [SerializeField]
    public CustomCharaWindow selectCharaFWindow;
    [SerializeField]
    public CustomCharaWindow selectCharaMWindow;
    [SerializeField]
    public HousingLoadWindow selectHousingWindow;
    [SerializeField]
    private GameObject objBlockUI;
    [HideInInspector]
    public bool changeCharaList;
    [HideInInspector]
    public bool changeHosingList;
    [HideInInspector]
    public bool updateProfile;
    [HideInInspector]
    public bool updateVersion;
    public bool noUserControl;
    public IReadOnlyDictionary<int, Dictionary<int, ItemData.Param>> SkillTable;

    public void DrawMessage(Color color, string msg)
    {
      if (Object.op_Inequality((Object) null, (Object) this.logview) && this.logview.IsActive)
        this.logview.AddLog(color, msg, (object[]) Array.Empty<object>());
      else
        this.popupMsg.StartMessage(0.2f, 2f, 0.2f, msg, !this.noUserControl ? 0 : 2);
    }

    public void BlockUI()
    {
      if (!Object.op_Implicit((Object) this.objBlockUI))
        return;
      this.objBlockUI.SetActiveIfDifferent(true);
    }

    public void UnblockUI()
    {
      if (!Object.op_Implicit((Object) this.objBlockUI))
        return;
      this.objBlockUI.SetActiveIfDifferent(false);
    }

    public IReadOnlyDictionary<int, LifeStyleData.Param> AgentLifeStyleInfoTable { get; private set; }

    private void LoadAgentLifeStyleInfoTable()
    {
      Dictionary<int, LifeStyleData.Param> dictionary = new Dictionary<int, LifeStyleData.Param>();
      List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/agent/lifestyle/", true);
      nameListFromPath.Sort();
      foreach (string assetBundleName in nameListFromPath)
      {
        foreach (LifeStyleData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (LifeStyleData), (string) null).GetAllAssets<LifeStyleData>())
        {
          foreach (LifeStyleData.Param obj in allAsset.param)
            dictionary[obj.ID] = obj;
        }
        AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
      }
      this.AgentLifeStyleInfoTable = (IReadOnlyDictionary<int, LifeStyleData.Param>) dictionary;
    }

    public string GetLifeStyleName(int id)
    {
      string str = "---------------";
      LifeStyleData.Param obj;
      if (this.AgentLifeStyleInfoTable.TryGetValue(id, ref obj))
        str = obj.Name;
      return str;
    }

    private void LoadItemList_Skill()
    {
      int[] numArray = new int[2]{ 16, 17 };
      List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/gameitem/info/item/itemlist/", true);
      nameListFromPath.Sort();
      Dictionary<int, Dictionary<int, ItemData.Param>> dictionary1 = new Dictionary<int, Dictionary<int, ItemData.Param>>();
      foreach (string assetBundleName in nameListFromPath)
      {
        foreach (ItemData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ItemData), (string) null).GetAllAssets<ItemData>())
        {
          int result;
          if (!int.TryParse(((Object) allAsset).get_name(), out result))
            Debug.LogError((object) string.Format("ItemList Category Name:{0}", (object) ((Object) allAsset).get_name()));
          else if (((IEnumerable<int>) numArray).Contains<int>(result))
          {
            Dictionary<int, ItemData.Param> dictionary2;
            if (!dictionary1.TryGetValue(result, out dictionary2))
              dictionary1[result] = dictionary2 = new Dictionary<int, ItemData.Param>();
            foreach (ItemData.Param obj in allAsset.param)
              dictionary2[obj.ID] = obj;
          }
        }
        AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
      }
      this.SkillTable = (IReadOnlyDictionary<int, Dictionary<int, ItemData.Param>>) dictionary1;
    }

    public string GetNormalSkillName(int id)
    {
      string str = "---------------";
      Dictionary<int, ItemData.Param> dictionary;
      ItemData.Param obj;
      if (this.SkillTable.TryGetValue(16, ref dictionary) && dictionary.TryGetValue(id, out obj))
        str = obj.Name;
      return str;
    }

    public string GetHSkillName(int id)
    {
      string str = "---------------";
      Dictionary<int, ItemData.Param> dictionary;
      ItemData.Param obj;
      if (this.SkillTable.TryGetValue(17, ref dictionary) && dictionary.TryGetValue(id, out obj))
        str = obj.Name;
      return str;
    }

    public void Start()
    {
      this.LoadAgentLifeStyleInfoTable();
      this.LoadItemList_Skill();
      for (int index = 0; index < this.dictUploaded.Length; ++index)
        this.dictUploaded[index] = new Dictionary<string, int>();
    }

    public class SelectHNInfo
    {
      public int userIdx = -1;
      public string handlename = string.Empty;
      public string drawname = string.Empty;
    }

    public class Profile
    {
      public int userIdx = -1;
    }

    public class UserInfo
    {
      public string handleName = string.Empty;
    }

    public class BaseIndex
    {
      public int idx = -1;
      public string data_uid = string.Empty;
      public int user_idx = -1;
      public string name = string.Empty;
      public string comment = string.Empty;
      public DateTime updateTime = new DateTime();
      public int rankingT = 999999;
      public int rankingW = 999999;
      public DateTime createTime = new DateTime();
      public int update_idx;
      public int dlCount;
      public int weekCount;
      public int applause;
    }

    public class CharaInfo : NetworkInfo.BaseIndex
    {
      public int birthmonth = 1;
      public int birthday = 1;
      public string strBirthDay = string.Empty;
      public int height = 1;
      public int bust = 1;
      public int lifestyle = -1;
      public int skill_n01 = -1;
      public int skill_n02 = -1;
      public int skill_n03 = -1;
      public int skill_n04 = -1;
      public int skill_n05 = -1;
      public int skill_h01 = -1;
      public int skill_h02 = -1;
      public int skill_h03 = -1;
      public int skill_h04 = -1;
      public int skill_h05 = -1;
      public int wish_01 = -1;
      public int wish_02 = -1;
      public int wish_03 = -1;
      public int type;
      public int sex;
      public int hair;
      public int phase;
      public int pheromone;
      public int reliability;
      public int reason;
      public int instinct;
      public int dirty;
      public int wariness;
      public int sociability;
      public int darkness;
      public int registration;
    }

    public class HousingInfo : NetworkInfo.BaseIndex
    {
      public int mapSize;
    }
  }
}
