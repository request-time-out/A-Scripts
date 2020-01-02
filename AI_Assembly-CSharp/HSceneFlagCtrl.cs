// Decompiled with JetBrains decompiler
// Type: HSceneFlagCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Cinemachine;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HSceneFlagCtrl : Singleton<HSceneFlagCtrl>
{
  public readonly int gotoFaintnessCount = 3;
  [Tooltip("0:なし　1:メタボール　2:パーティクル")]
  public int semenType = 1;
  public bool BeforeHWait = true;
  [RangeIntLabel("夜這いがバレる確率", 0, 100)]
  [Tooltip("0～100％で(小数不可)")]
  public int YobaiBareRate = 50;
  [Range(0.0f, 1f)]
  public float speedGuageRate = 0.01f;
  [Tooltip("ホイール一回でどれだけ回したことにするか")]
  public float wheelActionCount = 0.05f;
  public List<int>[,] lstSyncAnimLayers = new List<int>[2, 2];
  public Dictionary<int, List<int>> lstMapSyncAnimLayers = new Dictionary<int, List<int>>();
  public int categoryMotionList = -1;
  [SerializeField]
  public HScene.AnimationListInfo nowAnimationInfo = new HScene.AnimationListInfo();
  [Range(0.0f, 1f)]
  public float rateNipMax = 0.3f;
  private List<int>[] endAdd = new List<int>[2]
  {
    new List<int>(),
    new List<int>()
  };
  private List<int> endAddSkil = new List<int>();
  [EnumFlags]
  public List<HSceneFlagCtrl.JudgeSelect> isJudgeSelect = new List<HSceneFlagCtrl.JudgeSelect>();
  public bool isNotCtrl = true;
  public HSceneFlagCtrl.VoiceFlag voice = new HSceneFlagCtrl.VoiceFlag();
  public Dictionary<int, HSceneFlagCtrl.VoiceFlag> MapHvoices = new Dictionary<int, HSceneFlagCtrl.VoiceFlag>();
  public Dictionary<int, HVoiceCtrl> ctrlMapHVoice = new Dictionary<int, HVoiceCtrl>();
  [Range(0.0f, 2f)]
  public float speed;
  [Tooltip("0:弱ループ 1:強ループ -1:その他")]
  public int loopType;
  [Range(0.0f, 1f)]
  public float[] motions;
  [Tooltip("現在の場所を示すID")]
  public int nPlace;
  [Tooltip("Hポイントのマーカーを識別するID")]
  public int HPointID;
  public HPoint nowHPoint;
  public bool bFutanari;
  public float HBeforeHouchiTime;
  public float StartHouchiTime;
  public bool pointMoveAnimChange;
  public bool nowOrgasm;
  public float changeMotionTimeMin;
  public float changeMotionTimeMax;
  public AnimationCurve changeMotionCurve;
  public float changeMotionMinRate;
  [Range(0.0f, 1f)]
  public float feel_f;
  [Range(0.0f, 1f)]
  public float feel_m;
  [Tooltip("どの汁を制御するか")]
  public string ctrlSiru;
  public bool isInsert;
  public float changeAutoMotionTimeMin;
  public float changeAutoMotionTimeMax;
  [Range(0.0f, 1f)]
  public float guageDecreaseRate;
  [Range(0.0f, 1f)]
  public float feelSpnking;
  public float timeMasturbationChangeSpeed;
  public VirtualCameraController cameraCtrl;
  public CinemachineVirtualCamera HBeforeCamera;
  private HScene.AnimationListInfo _selectAnimationListInfo;
  public bool stopFeelFemal;
  public bool stopFeelMale;
  public HSceneFlagCtrl.ClickKind click;
  public bool isFaintness;
  public int initiative;
  public bool isLeaveItToYou;
  public bool isAutoActionChange;
  public bool isGaugeHit;
  public bool isGaugeHit_M;
  public bool nowSpeedStateFast;
  [Range(0.0f, 1f)]
  public float rateTuya;
  [Range(0.0f, 1f)]
  public float rateNip;
  public HSceneFlagCtrl.HousingID[] HousingAreaID;
  public int numOrgasm;
  public int numOrgasmTotal;
  public int numOrgasmFemalePlayer;
  [Tooltip("通常絶頂回数もカウントされます 同時絶頂したら、こいつも+1、絶頂回数も+1、中出しか外出しも+1")]
  public int numSameOrgasm;
  public int numInside;
  public int numOutSide;
  public int numDrink;
  public int numVomit;
  public int numAibu;
  public int numHoushi;
  public int numSonyu;
  public int numLes;
  public int numMulti;
  public int numUrine;
  public int numFaintness;
  public int numKokan;
  public int numAnal;
  public int numLeadFemale;
  public bool isHoushiFinish;
  public bool isPainActionParam;
  public bool isPainAction;
  public bool isConstraintAction;
  public bool isItemRelease;
  public bool isFemaleNaked;
  public bool isToilet;
  private GameObject MapHVoiceParent;

  public HScene.AnimationListInfo selectAnimationListInfo
  {
    get
    {
      return this._selectAnimationListInfo;
    }
    set
    {
      this._selectAnimationListInfo = value;
    }
  }

  public Dictionary<int, int> ChangeParams { get; private set; } = new Dictionary<int, int>();

  public void AddOrgasm()
  {
    this.numOrgasmTotal = Mathf.Clamp(this.numOrgasmTotal + 1, 0, 999999);
    if (this.numOrgasmTotal != this.gotoFaintnessCount)
      return;
    if (Singleton<HSceneManager>.Instance.isForce)
      this.AddParam(4, 0);
    else
      this.AddParam(6, 0);
  }

  public void AddParam(int ptn, int mode)
  {
    ParameterPacket parameterPacket = new ParameterPacket();
    switch (mode)
    {
      case 0:
        if (!Singleton<Resources>.Instance.HSceneTable.HBaseParamTable.TryGetValue(ptn, out parameterPacket))
          return;
        break;
      case 1:
        if (!Singleton<Resources>.Instance.HSceneTable.HactionParamTable.TryGetValue(ptn, out parameterPacket))
          return;
        break;
    }
    if (this.endAdd[mode].Contains(ptn))
      return;
    this.endAdd[mode].Add(ptn);
    foreach (KeyValuePair<int, TriThreshold> parameter in parameterPacket.Parameters)
    {
      int num = Mathf.RoundToInt(Random.Range(parameter.Value.SThreshold, parameter.Value.LThreshold));
      if (!this.ChangeParams.ContainsKey(parameter.Key))
      {
        this.ChangeParams.Add(parameter.Key, num);
      }
      else
      {
        Dictionary<int, int> changeParams;
        int key;
        (changeParams = this.ChangeParams)[key = parameter.Key] = changeParams[key] + num;
      }
    }
  }

  public void AddSkileParam(int id)
  {
    Dictionary<int, float> dictionary = new Dictionary<int, float>();
    if (!Singleton<Resources>.Instance.HSceneTable.HSkileParamTable.TryGetValue(id, out dictionary) || this.endAddSkil.Contains(id))
      return;
    this.endAddSkil.Add(id);
    foreach (KeyValuePair<int, float> keyValuePair in dictionary)
    {
      if (keyValuePair.Key != Resources.HSceneTables.HTagTable["ゲージ"])
      {
        int num = Mathf.RoundToInt(keyValuePair.Value);
        if (!this.ChangeParams.ContainsKey(keyValuePair.Key))
        {
          this.ChangeParams.Add(keyValuePair.Key, num);
        }
        else
        {
          Dictionary<int, int> changeParams;
          int key;
          (changeParams = this.ChangeParams)[key = keyValuePair.Key] = changeParams[key] + num;
        }
      }
    }
  }

  public float SkilChangeSpeed(int id)
  {
    Dictionary<int, float> dictionary = new Dictionary<int, float>();
    if (!Singleton<Resources>.Instance.HSceneTable.HSkileParamTable.TryGetValue(id, out dictionary))
      return 1f;
    foreach (KeyValuePair<int, float> keyValuePair in dictionary)
    {
      if (keyValuePair.Key == Resources.HSceneTables.HTagTable["ゲージ"])
        return keyValuePair.Value;
    }
    return 1f;
  }

  private void Start()
  {
    this.Init();
  }

  public void Init()
  {
    this.isNotCtrl = true;
    this.isFaintness = false;
    this.lstSyncAnimLayers[0, 0] = new List<int>();
    this.lstSyncAnimLayers[0, 1] = new List<int>();
    this.lstSyncAnimLayers[1, 0] = new List<int>();
    this.lstSyncAnimLayers[1, 1] = new List<int>();
    this.voice.playShorts = new int[2]{ -1, -1 };
  }

  private void OnDisable()
  {
    this.EndProc();
  }

  public void EndProc()
  {
    this.bFutanari = false;
    this.BeforeHWait = true;
    this.nowAnimationInfo = new HScene.AnimationListInfo();
    this.selectAnimationListInfo = (HScene.AnimationListInfo) null;
    this.feel_f = 0.0f;
    this.feel_m = 0.0f;
    this.initiative = 0;
    this.isLeaveItToYou = false;
    this.isAutoActionChange = false;
    this.isGaugeHit = false;
    this.isGaugeHit_M = false;
    this.nowSpeedStateFast = false;
    this.speed = 0.0f;
    for (int index = 0; index < this.motions.Length; ++index)
      this.motions[index] = 0.0f;
    this.nPlace = 0;
    this.HPointID = 0;
    if (Object.op_Inequality((Object) this.nowHPoint, (Object) null))
    {
      ChangeHItem componentInChildren = (ChangeHItem) ((Component) this.nowHPoint).GetComponentInChildren<ChangeHItem>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        componentInChildren.ChangeActive(true);
    }
    this.nowHPoint = (HPoint) null;
    this.stopFeelFemal = false;
    this.stopFeelMale = false;
    this.isFaintness = false;
    this.rateTuya = 0.0f;
    this.rateNip = 0.0f;
    this.numOrgasm = 0;
    this.numOrgasmTotal = 0;
    this.numOrgasmFemalePlayer = 0;
    this.numSameOrgasm = 0;
    this.numInside = 0;
    this.numOutSide = 0;
    this.numDrink = 0;
    this.numVomit = 0;
    this.numAibu = 0;
    this.numHoushi = 0;
    this.numSonyu = 0;
    this.numLes = 0;
    this.numUrine = 0;
    this.numFaintness = 0;
    this.numKokan = 0;
    this.numAnal = 0;
    this.numLeadFemale = 0;
    this.isJudgeSelect = new List<HSceneFlagCtrl.JudgeSelect>();
    this.isHoushiFinish = false;
    this.isNotCtrl = true;
    this.isPainActionParam = false;
    this.isPainAction = false;
    this.isConstraintAction = false;
    this.nowOrgasm = false;
    this.voice.MemberInit();
    this.ChangeParams.Clear();
    this.endAdd[0].Clear();
    this.endAdd[1].Clear();
    this.endAddSkil.Clear();
    for (int index1 = 0; index1 < 2; ++index1)
    {
      for (int index2 = 0; index2 < 2; ++index2)
      {
        if (this.lstSyncAnimLayers[index1, index2] != null)
          this.lstSyncAnimLayers[index1, index2].Clear();
      }
    }
  }

  public void MapHVoiceInit()
  {
    int agentMax = Singleton<Resources>.Instance.DefinePack.MapDefines.AgentMax;
    if (this.MapHvoices == null)
      this.MapHvoices = new Dictionary<int, HSceneFlagCtrl.VoiceFlag>();
    if (Object.op_Equality((Object) this.MapHVoiceParent, (Object) null))
    {
      this.MapHVoiceParent = new GameObject("MapHVoiceCtrls");
      this.MapHVoiceParent.get_transform().SetParent(((Component) this).get_transform());
    }
    for (int key = 0; key < agentMax; ++key)
    {
      if (!this.ctrlMapHVoice.ContainsKey(key) || !Object.op_Inequality((Object) this.ctrlMapHVoice[key], (Object) null))
      {
        this.ctrlMapHVoice.Add(key, (HVoiceCtrl) new GameObject(string.Format("MapHVoiceCtrl_{0}", (object) key)).AddComponent<HVoiceCtrl>());
        ((Component) this.ctrlMapHVoice[key]).get_transform().SetParent(this.MapHVoiceParent.get_transform());
        this.ctrlMapHVoice[key].MapHID = key;
        this.ctrlMapHVoice[key].ctrlFlag = this;
      }
    }
  }

  public int AddMapHvoices()
  {
    int key1 = 0;
    if (this.MapHvoices.ContainsKey(key1))
    {
      int key2 = 0;
      while (this.MapHvoices.ContainsKey(key2))
        ++key2;
      key1 = key2;
    }
    this.MapHvoices.Add(key1, new HSceneFlagCtrl.VoiceFlag());
    this.MapHvoices[key1].MemberInit();
    if (Object.op_Equality((Object) this.MapHVoiceParent, (Object) null))
    {
      this.MapHVoiceParent = new GameObject("MapHVoiceCtrls");
      this.MapHVoiceParent.get_transform().SetParent(((Component) this).get_transform());
    }
    if (!this.ctrlMapHVoice.ContainsKey(key1))
    {
      this.ctrlMapHVoice.Add(key1, (HVoiceCtrl) new GameObject(string.Format("MapHVoiceCtrl_{0}", (object) key1)).AddComponent<HVoiceCtrl>());
      ((Component) this.ctrlMapHVoice[key1]).get_transform().SetParent(this.MapHVoiceParent.get_transform());
      this.ctrlMapHVoice[key1].MapHID = key1;
      this.ctrlMapHVoice[key1].ctrlFlag = this;
    }
    return key1;
  }

  public void RemoveMapHvoices(int mapHID)
  {
    if (!this.MapHvoices.ContainsKey(mapHID))
      return;
    for (int index = 0; index < this.MapHvoices[mapHID].lstUseAsset.Count; ++index)
      AssetBundleManager.UnloadAssetBundle(this.MapHvoices[mapHID].lstUseAsset[index], true, (string) null, false);
    this.MapHvoices.Remove(mapHID);
  }

  public void AddMapSyncAnimLayer(int addID)
  {
    this.lstMapSyncAnimLayers.Add(addID, new List<int>());
  }

  public void RemoveMapSyncAnimLayer(int mapHID)
  {
    if (!this.lstMapSyncAnimLayers.ContainsKey(mapHID))
      return;
    this.lstMapSyncAnimLayers[mapHID].Clear();
    this.lstMapSyncAnimLayers.Remove(mapHID);
  }

  public enum ClickKind
  {
    None = -1, // 0xFFFFFFFF
    FinishBefore = 0,
    FinishInSide = 1,
    FinishOutSide = 2,
    FinishSame = 3,
    FinishDrink = 4,
    FinishVomit = 5,
    RecoverFaintness = 6,
    Spnking = 7,
    PeepingRestart = 8,
    LeaveItToYou = 9,
    SceneEnd = 10, // 0x0000000A
  }

  [Flags]
  public enum JudgeSelect
  {
    Kiss = 1,
    Kokan = 2,
    Breast = 4,
    Anal = 8,
    Pain = 16, // 0x00000010
    Constraint = 32, // 0x00000020
  }

  [Serializable]
  public class VoiceFlag
  {
    public bool[] playVoices = new bool[2];
    public int[] playShorts = new int[2]{ -1, -1 };
    public int oldFinish = -1;
    public int playStart = -1;
    public bool[] urines = new bool[2];
    public Transform[] voiceTrs = new Transform[2];
    public List<string> lstUseAsset = new List<string>();
    public bool dialog;
    public bool sleep;

    public void MemberInit()
    {
      this.playVoices = new bool[2];
      this.playShorts = new int[2]{ -1, -1 };
      this.oldFinish = -1;
      this.playStart = -1;
      this.dialog = false;
      this.urines = new bool[2];
      this.sleep = false;
      this.voiceTrs = new Transform[2];
      this.lstUseAsset = new List<string>();
    }
  }

  [Serializable]
  public struct HousingID
  {
    public int mapID;
    public int[] areaID;
  }
}
