// Decompiled with JetBrains decompiler
// Type: Manager.HSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using AIProject.Animal;
using AIProject.Definitions;
using AIProject.SaveData;
using Housing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEx;

namespace Manager
{
  public class HSceneManager : Singleton<HSceneManager>
  {
    public static readonly Dictionary<string, int> HmeshTag = new Dictionary<string, int>()
    {
      {
        "bath",
        11
      },
      {
        "stand",
        1
      },
      {
        "standfloor",
        -1
      },
      {
        "wall",
        2
      },
      {
        "bed",
        0
      },
      {
        "chair",
        3
      },
      {
        "table",
        4
      },
      {
        "stairs",
        10
      },
      {
        "sofa",
        6
      },
      {
        "sofabed",
        7
      },
      {
        "wasiki",
        14
      },
      {
        "counter",
        9
      },
      {
        "shower",
        12
      }
    };
    public static readonly Dictionary<string, int> HmeshNomalKindTag = new Dictionary<string, int>()
    {
      {
        "bath",
        11
      },
      {
        "wall",
        2
      },
      {
        "bed",
        0
      },
      {
        "chair",
        3
      },
      {
        "table",
        4
      },
      {
        "stairs",
        10
      },
      {
        "sofa",
        6
      },
      {
        "sofabed",
        7
      },
      {
        "wasiki",
        14
      },
      {
        "counter",
        9
      },
      {
        "shower",
        12
      }
    };
    public static readonly Dictionary<string, int> HmeshLesTag = new Dictionary<string, int>()
    {
      {
        "standfloor",
        -1
      },
      {
        "wall",
        2
      },
      {
        "bed",
        0
      },
      {
        "chair",
        3
      },
      {
        "table",
        4
      },
      {
        "stand",
        1
      },
      {
        "sofa",
        6
      }
    };
    public AgentActor[] Agent = new AgentActor[2];
    public Actor[] females = new Actor[2];
    public string[] pngFemales = new string[2];
    public HashSet<string> hashUseAssetBundle = new HashSet<string>();
    public int MerchantLimit = -1;
    public float SerchHmeshRange = 50f;
    public Dictionary<int, int> HSkil = new Dictionary<int, int>()
    {
      {
        0,
        0
      },
      {
        1,
        0
      },
      {
        2,
        0
      },
      {
        3,
        0
      },
      {
        4,
        0
      }
    };
    public int[] PersonalPhase = new int[2]{ -1, -1 };
    public int[] Personality = new int[2];
    public int height = -1;
    public float[] Mood = new float[2];
    [DisabledGroup("ヒロインからHに誘ってきたパターン")]
    public int nInvitePtn = -1;
    public ValueTuple<RaycastHit, GameObject, int> hitHmesh = (ValueTuple<RaycastHit, GameObject, int>) null;
    private Dictionary<int, bool> tmpHadItems = new Dictionary<int, bool>();
    public bool[] isHAddTaii = new bool[2];
    [HideInInspector]
    public readonly string strAssetCameraList = "list/h/camera/";
    [HideInInspector]
    public readonly string strAssetAnimationInfoListFolder = "list/h/animationinfo/";
    [HideInInspector]
    public readonly string strAssetStartAnimationListFolder = "list/h/startanimation/";
    [HideInInspector]
    public readonly string strAssetStartWaitAnimListFolder = "list/h/startwaitanim/";
    [HideInInspector]
    public readonly string strAssetEndAnimationInfoFolder = "list/h/endanimation/";
    [HideInInspector]
    public readonly string strAssetMoveOffsetListFolder = "list/h/move/";
    [HideInInspector]
    public readonly string strAssetNeckCtrlListFolder = "list/h/neckcontrol/";
    [HideInInspector]
    public readonly string strAssetYureListFolder = "list/h/yure/";
    [HideInInspector]
    public readonly string strAssetLayerCtrlListFolder = "list/h/layer/";
    [HideInInspector]
    public readonly string strAssetDankonListFolder = "list/h/lookatdan/";
    [HideInInspector]
    public readonly string strAssetDynamicBoneListFolder = "list/h/dynamicbone/";
    [HideInInspector]
    public readonly string strAssetHAutoListFolder = "list/h/hauto/hauto/01.unity3d";
    [HideInInspector]
    public readonly string strAssetLeaveItToYouFolder = "list/h/hauto/leaveittoyou/";
    [HideInInspector]
    public readonly string strAssetHParticleListFolder = "list/h/hparticle/";
    [HideInInspector]
    public readonly string strAssetSiruPasteListFolder = "list/h/sirupaste/";
    [HideInInspector]
    public readonly string strAssetMetaBallListFolder = "list/h/sirumetaball/";
    [HideInInspector]
    public readonly string strAssetHpointPrefabListFolder = "list/h/hpoint/prefab/";
    [HideInInspector]
    public readonly string strAssetHpointListFolder = "list/h/hpoint/pointinfo/";
    [HideInInspector]
    public readonly string strAssetFeelHitListFolder = "list/h/feelhit/";
    [HideInInspector]
    public readonly string strAssetHItemInfoListFolder = "list/h/hitem/";
    [HideInInspector]
    public readonly string strAssetHItemObjInfoListFolder = "list/h/hitemobj/";
    [HideInInspector]
    public readonly string strAssetHitObjListFolder = "list/h/hit/hitobject/";
    [HideInInspector]
    public readonly string strAssetCollisionListFolder = "list/h/hit/collision/";
    [HideInInspector]
    public readonly string strAssetSEListFolder = "list/h/sound/se/";
    [HideInInspector]
    public readonly string strAssetBGMListFolder = "list/h/sound/bgm/";
    [HideInInspector]
    public readonly string strAssetVoiceListFolder = "list/h/sound/voice/";
    [HideInInspector]
    public readonly string strAssetBreathListFolder = "list/h/sound/breath/";
    [HideInInspector]
    public readonly string strAssetSE = "sound/data/se/h";
    [HideInInspector]
    public readonly string strAssetParam = "list/h/param/";
    [HideInInspector]
    public readonly string strAssetIKListFolder = "list/h/ikinfo/";
    [HideInInspector]
    public readonly string HmeshListFolder = "list/map/chunk/";
    public Dictionary<AgentActor, Desire.ActionType> ReturnActionTypes = new Dictionary<AgentActor, Desire.ActionType>();
    private Vector3 start = Vector3.get_zero();
    public bool[] FemaleLumpActive = new bool[2];
    public PlayerActor Player;
    public MerchantActor merchantActor;
    public Actor male;
    public string pngMale;
    public HSceneManager.HEvent EventKind;
    public bool bMerchant;
    public GameObject HSceneSet;
    public GameObject HSceneUISet;
    public int numFemaleClothCustom;
    public int hMainKind;
    public bool isForce;
    public float Temperature;
    public float Toilet;
    public float? ReserveToilet;
    public float Bath;
    private int Reliability;
    private int Instinct;
    private int Dirty;
    private int RiskManagement;
    private int Darkness;
    private int Sociability;
    private int Pheromone;
    public bool handsIK;
    [DisabledGroup("ふたなりボディか")]
    public bool bFutanari;
    private bool bSleepStart;
    public RaycastHit[] hits;
    [DisabledGroup("cameramesh")]
    public GameObject CameraMesh;
    [Label("キャラの位置にパーティクルを出すか")]
    public bool isParticle;
    public int maleFinish;
    public int femalePlayerFinish;
    public int femaleFinish;
    public bool isCtrl;
    public byte endStatus;
    public bool onDeskChair;
    public IDisposable choiceDisposable;
    private bool _isH;
    public bool IsHousingHEnter;
    public HPoint enterPoint;

    public static bool SleepStart
    {
      set
      {
        if (!Singleton<HSceneManager>.IsInstance())
          return;
        Singleton<HSceneManager>.Instance.bSleepStart = value;
      }
      get
      {
        return Singleton<HSceneManager>.IsInstance() && Singleton<HSceneManager>.Instance.bSleepStart;
      }
    }

    public void SetEventInfo(HSceneManager.HEvent _event)
    {
      this.EventKind = _event;
    }

    public static bool isHScene
    {
      get
      {
        return Singleton<HSceneManager>.IsInstance() && Singleton<HSceneManager>.Instance._isH;
      }
    }

    public void HsceneEnter(
      Actor actor,
      int isMerchantLimitType = -1,
      AgentActor agent2 = null,
      HSceneManager.HEvent _numEvent = HSceneManager.HEvent.Normal)
    {
      this._isH = true;
      this.enterPoint = (HPoint) null;
      this.IsHousingHEnter = false;
      MapUIContainer.SetActiveCommandList(false);
      this.choiceDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) MapUIContainer.CommandList.OnCompletedStopAsObservable(), 1), (System.Action<M0>) (_ => MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None)));
      Singleton<MapUIContainer>.Instance.MinimapUI.MiniMap.SetActive(false);
      Singleton<MapUIContainer>.Instance.MinimapUI.MiniMapIcon.SetActive(false);
      Singleton<Map>.Instance.SetActiveMapEffect(false);
      if (Object.op_Equality((Object) this.HSceneSet, (Object) null))
        this.HSceneSet = Singleton<Resources>.Instance.HSceneTable.HSceneSet;
      if (Object.op_Equality((Object) this.HSceneUISet, (Object) null))
        this.HSceneUISet = Singleton<Resources>.Instance.HSceneTable.HSceneUISet;
      this.Player = Singleton<Map>.Instance.Player;
      ((Behaviour) this.Player).set_enabled(false);
      this.handsIK = ((Behaviour) this.Player.HandsHolder).get_enabled();
      if (this.handsIK)
        ((Behaviour) this.Player.HandsHolder).set_enabled(false);
      this.Player.SetActiveOnEquipedItem(false);
      this.Player.ChaControl.setAllLayerWeight(0.0f);
      this.bMerchant = Object.op_Inequality((Object) ((Component) actor).GetComponent<MerchantActor>(), (Object) null);
      this.MerchantLimit = isMerchantLimitType;
      this.FemaleLumpActive[0] = false;
      this.FemaleLumpActive[1] = false;
      if (!this.bMerchant)
      {
        this.Agent[0] = (AgentActor) ((Component) actor).GetComponent<AgentActor>();
        this.Agent[0].BehaviorResources.ChangeMode(Desire.ActionType.Idle);
        this.Agent[0].DisableBehavior();
        this.Agent[0].AnimationAgent.DisableItems();
        ((Behaviour) this.Agent[0].Controller).set_enabled(false);
        ((Behaviour) this.Agent[0].AnimationAgent).set_enabled(false);
        this.PersonalPhase[0] = this.Agent[0].ChaControl.fileGameInfo.phase;
        this.Personality[0] = this.Agent[0].ChaControl.chaFile.parameter.personality;
        if (Object.op_Inequality((Object) this.Agent[0].ChaControl.objExtraAccessory[3], (Object) null))
        {
          this.FemaleLumpActive[0] = this.Agent[0].ChaControl.objExtraAccessory[3].get_activeSelf();
          this.Agent[0].ChaControl.ShowExtraAccessory(ChaControlDefine.ExtraAccessoryParts.Waist, false);
        }
        this.Agent[0].SetActiveOnEquipedItem(false);
        this.Agent[0].ChaControl.setAllLayerWeight(0.0f);
        this.HSkil = this.Agent[0].ChaControl.fileGameInfo.hSkill;
      }
      else
      {
        this.merchantActor = (MerchantActor) ((Component) actor).GetComponent<MerchantActor>();
        this.merchantActor.DisableBehavior();
        ((Behaviour) this.merchantActor.Controller).set_enabled(false);
        ((Behaviour) this.merchantActor.AnimationMerchant).set_enabled(false);
        this.PersonalPhase[0] = 3;
      }
      if (Object.op_Inequality((Object) agent2, (Object) null))
      {
        this.Agent[1] = agent2;
        this.Agent[1].BehaviorResources.ChangeMode(Desire.ActionType.Idle);
        this.Agent[1].DisableBehavior();
        this.Agent[1].AnimationAgent.DisableItems();
        ((Behaviour) this.Agent[1].Controller).set_enabled(false);
        ((Behaviour) this.Agent[1].AnimationAgent).set_enabled(false);
        this.PersonalPhase[1] = this.Agent[1].ChaControl.fileGameInfo.phase;
        this.Personality[1] = this.Agent[1].ChaControl.chaFile.parameter.personality;
        if (Object.op_Inequality((Object) this.Agent[1].ChaControl.objExtraAccessory[3], (Object) null))
        {
          this.FemaleLumpActive[1] = this.Agent[1].ChaControl.objExtraAccessory[3].get_activeSelf();
          this.Agent[1].ChaControl.ShowExtraAccessory(ChaControlDefine.ExtraAccessoryParts.Waist, false);
        }
        this.Agent[1].SetActiveOnEquipedItem(false);
        this.Agent[1].ChaControl.setAllLayerWeight(0.0f);
      }
      this.isCtrl = false;
      this.endStatus = (byte) 0;
      AnimalBase.CreateDisplay = false;
      AnimalManager instance = Singleton<AnimalManager>.Instance;
      for (int index1 = 0; index1 < instance.Animals.Count; ++index1)
      {
        int index2 = index1;
        instance.Animals[index2].BodyEnabled = false;
        ((Behaviour) instance.Animals[index2]).set_enabled(false);
      }
      instance.ClearAnimalPointBehavior();
      if (!this.bMerchant)
      {
        this.Temperature = this.Agent[0].AgentData.StatsTable[0];
        this.Mood[0] = this.Agent[0].AgentData.StatsTable[1];
        this.Mood[1] = !Object.op_Implicit((Object) this.Agent[1]) ? 0.0f : this.Agent[1].AgentData.StatsTable[1];
        if (this.ReserveToilet.HasValue)
        {
          this.Toilet = this.ReserveToilet.Value;
          this.ReserveToilet = new float?();
        }
        else
          this.Toilet = this.Agent[0].AgentData.DesireTable[Desire.GetDesireKey(Desire.Type.Toilet)];
        this.Bath = this.Agent[0].AgentData.DesireTable[Desire.GetDesireKey(Desire.Type.Bath)];
        this.Reliability = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Reliability);
        this.Instinct = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Instinct);
        this.Dirty = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Dirty);
        this.RiskManagement = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Wariness);
        this.Darkness = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Darkness);
        this.Sociability = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Sociability);
        this.Pheromone = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Pheromone);
        this.isHAddTaii = new bool[2]
        {
          this.Agent[0].ChaControl.fileGameInfo.isHAddTaii0,
          this.Agent[0].ChaControl.fileGameInfo.isHAddTaii1
        };
      }
      this.Player.Controller.ChangeState("Sex");
      this.ReturnActionTypes.Clear();
      this.EventKind = _numEvent;
      Singleton<Voice>.Instance.StopAll(true);
      if (!this.bMerchant)
        this.StartCoroutine(this.HsceneInit(this.Agent));
      else
        this.StartCoroutine(this.HsceneInit(this.merchantActor, !Object.op_Inequality((Object) agent2, (Object) null) ? (AgentActor) null : this.Agent[1]));
    }

    public void HousingHEnter(Actor actor, HPoint hpoint)
    {
      this._isH = true;
      this.IsHousingHEnter = true;
      MapUIContainer.SetActiveCommandList(false);
      this.choiceDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) MapUIContainer.CommandList.OnCompletedStopAsObservable(), 1), (System.Action<M0>) (_ => MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None)));
      Singleton<MapUIContainer>.Instance.MinimapUI.MiniMap.SetActive(false);
      Singleton<MapUIContainer>.Instance.MinimapUI.MiniMapIcon.SetActive(false);
      Singleton<Map>.Instance.SetActiveMapEffect(false);
      if (Object.op_Equality((Object) this.HSceneSet, (Object) null))
        this.HSceneSet = Singleton<Resources>.Instance.HSceneTable.HSceneSet;
      if (Object.op_Equality((Object) this.HSceneUISet, (Object) null))
        this.HSceneUISet = Singleton<Resources>.Instance.HSceneTable.HSceneUISet;
      this.Player = Singleton<Map>.Instance.Player;
      ((Behaviour) this.Player).set_enabled(false);
      this.handsIK = ((Behaviour) this.Player.HandsHolder).get_enabled();
      if (this.handsIK)
        ((Behaviour) this.Player.HandsHolder).set_enabled(false);
      this.Player.SetActiveOnEquipedItem(false);
      this.Player.ChaControl.setAllLayerWeight(0.0f);
      this.Agent[0] = (AgentActor) ((Component) actor).GetComponent<AgentActor>();
      this.Agent[0].BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.Agent[0].DisableBehavior();
      this.Agent[0].AnimationAgent.DisableItems();
      ((Behaviour) this.Agent[0].Controller).set_enabled(false);
      ((Behaviour) this.Agent[0].AnimationAgent).set_enabled(false);
      this.PersonalPhase[0] = this.Agent[0].ChaControl.fileGameInfo.phase;
      this.Personality[0] = this.Agent[0].ChaControl.chaFile.parameter.personality;
      this.Agent[0].SetActiveOnEquipedItem(false);
      this.Agent[0].ChaControl.setAllLayerWeight(0.0f);
      this.HSkil = this.Agent[0].ChaControl.fileGameInfo.hSkill;
      this.isCtrl = false;
      this.endStatus = (byte) 0;
      AnimalBase.CreateDisplay = false;
      AnimalManager instance = Singleton<AnimalManager>.Instance;
      for (int index1 = 0; index1 < instance.Animals.Count; ++index1)
      {
        int index2 = index1;
        instance.Animals[index2].BodyEnabled = false;
        ((Behaviour) instance.Animals[index2]).set_enabled(false);
      }
      instance.ClearAnimalPointBehavior();
      if (!this.bMerchant)
      {
        this.Temperature = this.Agent[0].AgentData.StatsTable[0];
        this.Mood[0] = this.Agent[0].AgentData.StatsTable[1];
        this.Mood[1] = !Object.op_Implicit((Object) this.Agent[1]) ? 0.0f : this.Agent[1].AgentData.StatsTable[1];
        if ((double) this.Mood[0] < (double) this.Agent[0].ChaControl.fileGameInfo.moodBound.lower)
          this.isForce = true;
        this.Toilet = this.Agent[0].AgentData.DesireTable[Desire.GetDesireKey(Desire.Type.Toilet)];
        this.Bath = this.Agent[0].AgentData.DesireTable[Desire.GetDesireKey(Desire.Type.Bath)];
        this.Reliability = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Reliability);
        this.Instinct = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Instinct);
        this.Dirty = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Dirty);
        this.RiskManagement = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Wariness);
        this.Darkness = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Darkness);
        this.Sociability = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Sociability);
        this.Pheromone = this.Agent[0].GetFlavorSkill(FlavorSkill.Type.Pheromone);
        this.isHAddTaii = new bool[2]
        {
          this.Agent[0].ChaControl.fileGameInfo.isHAddTaii0,
          this.Agent[0].ChaControl.fileGameInfo.isHAddTaii1
        };
      }
      this.Player.Controller.ChangeState("Sex");
      this.ReturnActionTypes.Clear();
      this.EventKind = HSceneManager.HEvent.Normal;
      if (this.EventKind == HSceneManager.HEvent.Back)
        Singleton<HSceneFlagCtrl>.Instance.AddParam(31, 1);
      Singleton<Voice>.Instance.StopAll(true);
      this.height = (int) hpoint._nPlace[0].Item1;
      this.enterPoint = hpoint;
      this.StartCoroutine(this.HsceneInit(this.Agent));
    }

    [DebuggerHidden]
    private IEnumerator HsceneInit(AgentActor[] agent)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new HSceneManager.\u003CHsceneInit\u003Ec__Iterator0()
      {
        agent = agent,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator HsceneInit(MerchantActor Merchant, AgentActor agent = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new HSceneManager.\u003CHsceneInit\u003Ec__Iterator1()
      {
        agent = agent,
        \u0024this = this
      };
    }

    public bool HMeshCheck(int mode, List<HScene.DeskChairInfo> deskChairInfos)
    {
      this.hits = new RaycastHit[10];
      this.start = Vector3.get_zero();
      this.start = !this.bMerchant ? (mode != 1 ? ((Component) this.Agent[0].Controller).get_transform().get_position() : ((Component) this.Agent[0].ChaControl).get_transform().get_position()) : (mode != 1 ? ((Component) this.merchantActor.Controller).get_transform().get_position() : ((Component) this.merchantActor.ChaControl).get_transform().get_position());
      this.start.y = !this.bMerchant ? this.Agent[0].ChaControl.objHeadBone.get_transform().get_position().y : this.merchantActor.ChaControl.objHeadBone.get_transform().get_position().y;
      int num1 = Physics.RaycastNonAlloc(this.start, Vector3.get_down(), this.hits, this.SerchHmeshRange, 131072);
      if (num1 == 0)
        return false;
      int mapId = Singleton<Map>.Instance.MapID;
      int areaId = this.Player.AreaID;
      HSceneFlagCtrl.HousingID[] housingAreaId = Singleton<HSceneFlagCtrl>.Instance.HousingAreaID;
      BasePoint[] basePoints = Singleton<Map>.Instance.PointAgent.BasePoints;
      int num2 = -1;
      int _id = -1;
      for (int index1 = 0; index1 < housingAreaId.Length; ++index1)
      {
        if (mapId == housingAreaId[index1].mapID)
        {
          for (int index2 = 0; index2 < housingAreaId[index1].areaID.Length; ++index2)
          {
            if (housingAreaId[index1].areaID[index2] == areaId)
            {
              num2 = areaId;
              break;
            }
          }
        }
      }
      for (int index = 0; index < basePoints.Length; ++index)
      {
        if (basePoints[index].AreaIDInHousing == num2)
        {
          _id = basePoints[index].ID;
          break;
        }
      }
      HPoint[] hpointArray = (HPoint[]) null;
      if (_id != -1)
        hpointArray = Singleton<Manager.Housing>.Instance.GetHPoint(_id);
      bool flag1 = false;
      for (int index = 0; index < num1; ++index)
      {
        flag1 = HSceneManager.HmeshNomalKindTag.ContainsKey(((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag());
        if (flag1)
          break;
      }
      RaycastHit hit = this.hits[0];
      if (num1 > 1)
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(((RaycastHit) ref hit).get_point(), this.start);
        float num3 = ((Vector3) ref vector3_1).get_sqrMagnitude();
        for (int index = 1; index < num1; ++index)
        {
          if (!this.bMerchant)
          {
            if (this.EventKind == HSceneManager.HEvent.FromFemale)
            {
              if ((Singleton<Map>.Instance.Player.ChaControl.sex == (byte) 0 || Singleton<Map>.Instance.Player.ChaControl.sex == (byte) 1 && this.bFutanari) && (((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() != "standfloor" && ((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() != "bed"))
                continue;
            }
            else if (this.EventKind == HSceneManager.HEvent.Yobai)
            {
              if (((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() != "bed" && ((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() != "sofabed")
                continue;
            }
            else if (this.EventKind == HSceneManager.HEvent.Normal && (flag1 && !HSceneManager.HmeshNomalKindTag.ContainsKey(((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag()) || this.EventKind != HSceneManager.HEvent.GyakuYobai && HSceneManager.SleepStart && (((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() != "standfloor" && ((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() != "bed")))
              continue;
            if (Singleton<Map>.Instance.Player.ChaControl.sex == (byte) 1 && !this.bFutanari && !HSceneManager.HmeshLesTag.ContainsKey(((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag()))
              continue;
          }
          else if ("sofa" == ((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() || !HSceneManager.HmeshTag.ContainsKey(((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag()) || HSceneManager.HmeshTag[((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag()] > 4)
            continue;
          Vector3 vector3_2 = Vector3.op_Subtraction(((RaycastHit) ref this.hits[index]).get_point(), this.start);
          float sqrMagnitude = ((Vector3) ref vector3_2).get_sqrMagnitude();
          if ((double) num3 >= (double) sqrMagnitude && !(((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() == "Untagged"))
          {
            hit = this.hits[index];
            num3 = sqrMagnitude;
          }
        }
      }
      else if (((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() == "Untagged")
        return false;
      if (hpointArray != null && hpointArray.Length > 0)
      {
        float num3 = -1f;
        for (int index1 = 0; index1 < hpointArray.Length; ++index1)
        {
          List<Collider> housingHcol = this.GetHousingHcol(hpointArray[index1]);
          for (int index2 = 0; index2 < num1; ++index2)
          {
            if (housingHcol.Contains(((RaycastHit) ref this.hits[index2]).get_collider()))
            {
              if (!this.bMerchant)
              {
                if (this.EventKind == HSceneManager.HEvent.FromFemale)
                {
                  if ((Singleton<Map>.Instance.Player.ChaControl.sex == (byte) 0 || Singleton<Map>.Instance.Player.ChaControl.sex == (byte) 1 && this.bFutanari) && (((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() != "standfloor" && ((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() != "bed"))
                    continue;
                }
                else if (this.EventKind == HSceneManager.HEvent.Yobai)
                {
                  if (((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() != "standfloor" && ((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() != "bed" && ((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() != "sofabed")
                    continue;
                }
                else if (this.EventKind == HSceneManager.HEvent.Normal && (flag1 && !HSceneManager.HmeshNomalKindTag.ContainsKey(((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag()) || this.EventKind != HSceneManager.HEvent.GyakuYobai && HSceneManager.SleepStart && (((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() != "standfloor" && ((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() != "bed")))
                  continue;
                if (Singleton<Map>.Instance.Player.ChaControl.sex == (byte) 1 && !this.bFutanari && !HSceneManager.HmeshLesTag.ContainsKey(((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag()))
                  continue;
              }
              else if ("sofa" == ((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() || !HSceneManager.HmeshTag.ContainsKey(((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag()) || HSceneManager.HmeshTag[((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag()] > 4)
                continue;
              Vector3 vector3 = Vector3.op_Subtraction(((RaycastHit) ref this.hits[index2]).get_point(), this.start);
              float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
              if (((double) num3 < 0.0 || (double) num3 >= (double) sqrMagnitude) && !(((Component) ((RaycastHit) ref this.hits[index2]).get_collider()).get_gameObject().get_tag() == "Untagged"))
              {
                hit = this.hits[index2];
                num3 = sqrMagnitude;
              }
            }
          }
        }
      }
      if (!this.bMerchant)
      {
        if (this.EventKind == HSceneManager.HEvent.FromFemale)
        {
          if (((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() != "standfloor" && ((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() != "bed")
            return false;
        }
        else if (this.EventKind == HSceneManager.HEvent.Yobai && ((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() != "standfloor" && (((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() != "bed" && ((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() != "sofabed"))
          return false;
        if (Singleton<Map>.Instance.Player.ChaControl.sex == (byte) 1 && !this.bFutanari && !HSceneManager.HmeshLesTag.ContainsKey(((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()))
          return false;
      }
      else if ("sofa" == ((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() || !HSceneManager.HmeshTag.ContainsKey(((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()) || HSceneManager.HmeshTag[((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()] > 4)
        return false;
      if (((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag() != "table" || this.bMerchant)
      {
        this.hitHmesh.Item1 = (__Null) hit;
        this.hitHmesh.Item2 = (__Null) ((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject();
      }
      else
      {
        bool flag2 = false;
        RaycastHit raycastHit = (RaycastHit) null;
        GameObject gameObject = (GameObject) null;
        for (int index = 0; index < num1; ++index)
        {
          if (!(((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject().get_tag() != "chair"))
          {
            flag2 = true;
            raycastHit = this.hits[index];
            gameObject = ((Component) ((RaycastHit) ref this.hits[index]).get_collider()).get_gameObject();
            break;
          }
        }
        if (flag2)
        {
          this.onDeskChair = false;
          for (int index1 = 0; index1 < deskChairInfos.Count; ++index1)
          {
            int index2 = index1;
            if (deskChairInfos[index2].eventID == this.Agent[0].ActionID && deskChairInfos[index2].poseID == this.Agent[0].PoseID)
            {
              this.onDeskChair = true;
              break;
            }
          }
          if (this.onDeskChair)
          {
            this.hitHmesh.Item1 = (__Null) hit;
            this.hitHmesh.Item2 = (__Null) ((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject();
          }
          else
          {
            this.hitHmesh.Item1 = (__Null) raycastHit;
            this.hitHmesh.Item2 = (__Null) gameObject;
            this.hitHmesh.Item3 = !HSceneManager.HmeshTag.ContainsKey(gameObject.get_tag()) ? (__Null) -2 : (__Null) HSceneManager.HmeshTag[gameObject.get_tag()];
            return true;
          }
        }
      }
      switch (this.EventKind)
      {
        case HSceneManager.HEvent.Yobai:
          this.hitHmesh.Item3 = !HSceneManager.HmeshTag.ContainsKey(((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()) ? (__Null) 0 : (HSceneManager.HmeshTag[((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()] != -1 ? (__Null) HSceneManager.HmeshTag[((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()] : (__Null) 0);
          break;
        case HSceneManager.HEvent.Bath:
          this.hitHmesh.Item3 = (__Null) 11;
          break;
        case HSceneManager.HEvent.Toilet1:
          this.hitHmesh.Item3 = (__Null) 13;
          break;
        case HSceneManager.HEvent.Toilet2:
        case HSceneManager.HEvent.ShagmiBare:
          this.hitHmesh.Item3 = (__Null) 14;
          break;
        case HSceneManager.HEvent.Back:
        case HSceneManager.HEvent.GyakuYobai:
        case HSceneManager.HEvent.FromFemale:
          this.hitHmesh.Item3 = (__Null) 0;
          break;
        case HSceneManager.HEvent.Kitchen:
          this.hitHmesh.Item3 = (__Null) 9;
          break;
        case HSceneManager.HEvent.Tachi:
        case HSceneManager.HEvent.MapBath:
          this.hitHmesh.Item3 = (__Null) 1;
          break;
        case HSceneManager.HEvent.Stairs:
        case HSceneManager.HEvent.StairsBare:
          this.hitHmesh.Item3 = (__Null) 10;
          break;
        case HSceneManager.HEvent.KabeanaBack:
        case HSceneManager.HEvent.KabeanaFront:
          this.hitHmesh.Item3 = (__Null) 15;
          break;
        case HSceneManager.HEvent.Neonani:
          this.hitHmesh.Item3 = (__Null) 0;
          break;
        case HSceneManager.HEvent.TsukueBare:
          this.hitHmesh.Item3 = (__Null) 4;
          break;
        default:
          this.hitHmesh.Item3 = Object.op_Equality((Object) this.females[1], (Object) null) || Object.op_Inequality((Object) ((Component) this.females[1]).GetComponent<PlayerActor>(), (Object) null) ? (!HSceneManager.HmeshTag.ContainsKey(((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()) ? (__Null) -2 : (__Null) HSceneManager.HmeshTag[((Component) ((RaycastHit) ref hit).get_collider()).get_gameObject().get_tag()]) : (__Null) 0;
          break;
      }
      if (HSceneManager.SleepStart)
        this.hitHmesh.Item3 = (__Null) 0;
      return true;
    }

    public void SetFlaverSkillParamator(int kind, int val)
    {
      switch (kind)
      {
        case 0:
          this.Reliability += val;
          this.Agent[0].SetFlavorSkill(FlavorSkill.Type.Reliability, this.Reliability);
          break;
        case 1:
          this.Instinct += val;
          this.Agent[0].SetFlavorSkill(FlavorSkill.Type.Instinct, this.Instinct);
          break;
        case 2:
          this.Dirty += val;
          this.Agent[0].SetFlavorSkill(FlavorSkill.Type.Dirty, this.Dirty);
          break;
        case 3:
          this.RiskManagement += val;
          this.Agent[0].SetFlavorSkill(FlavorSkill.Type.Wariness, this.RiskManagement);
          break;
        case 4:
          this.Darkness += val;
          this.Agent[0].SetFlavorSkill(FlavorSkill.Type.Darkness, this.Darkness);
          break;
        case 5:
          this.Sociability += val;
          this.Agent[0].SetFlavorSkill(FlavorSkill.Type.Sociability, this.Sociability);
          break;
        case 6:
          this.Pheromone += val;
          this.Agent[0].SetFlavorSkill(FlavorSkill.Type.Pheromone, this.Pheromone);
          break;
      }
    }

    public void SetParamator(int kind, int val)
    {
      if (kind < 10)
        this.Agent[0].AddStatus(kind, (float) val);
      else if (kind < 100)
        this.Agent[0].AddFlavorSkill(kind - 10, val);
      else
        this.Agent[0].SetDesire(kind - 100, (float) val);
    }

    public int GetFlaverSkillLevel(int kind)
    {
      switch (kind)
      {
        case 0:
          return this.Reliability;
        case 1:
          return this.Instinct;
        case 2:
          return this.Dirty;
        case 3:
          return this.RiskManagement;
        case 4:
          return this.Darkness;
        case 5:
          return this.Sociability;
        default:
          return -1;
      }
    }

    public bool CheckHadItem(int _mode, int _id)
    {
      List<Dictionary<int, List<HItemCtrl.ListItem>>>[] lstHitemObjInfo = Singleton<Resources>.Instance.HSceneTable.lstHItemObjInfo;
      List<StuffItem> itemList = Singleton<Map>.Instance.Player.PlayerData.ItemList;
      List<StuffItem> itemListInStorage = Singleton<Game>.Instance.Environment.ItemListInStorage;
      this.tmpHadItems.Clear();
      for (int index1 = 0; index1 < lstHitemObjInfo[_mode].Count; ++index1)
      {
        if (lstHitemObjInfo[_mode][index1].ContainsKey(_id))
        {
          for (int index2 = 0; index2 < lstHitemObjInfo[_mode][index1][_id].Count; ++index2)
          {
            int key = index2;
            if (lstHitemObjInfo[_mode][index1][_id][key].itemkind != 1)
            {
              this.tmpHadItems.Add(key, true);
            }
            else
            {
              int itemId = lstHitemObjInfo[_mode][index1][_id][key].itemID;
              foreach (StuffItem stuffItem in itemList)
              {
                if (stuffItem.CategoryID == 14 && stuffItem.ID == itemId)
                {
                  this.tmpHadItems.Add(key, true);
                  break;
                }
              }
              if (!this.tmpHadItems.ContainsKey(key) || !this.tmpHadItems[key])
              {
                foreach (StuffItem stuffItem in itemListInStorage)
                {
                  if (stuffItem.CategoryID == 14 && stuffItem.ID == itemId)
                  {
                    if (this.tmpHadItems.ContainsKey(key))
                      this.tmpHadItems[key] = true;
                    else
                      this.tmpHadItems.Add(key, true);
                  }
                }
                if (!this.tmpHadItems.ContainsKey(key))
                  this.tmpHadItems.Add(key, false);
              }
            }
          }
        }
      }
      foreach (bool flag in this.tmpHadItems.Values)
      {
        if (!flag)
          return false;
      }
      return true;
    }

    public void EndHScene()
    {
      this._isH = false;
    }

    private List<Collider> GetHousingHcol(HPoint hPoint)
    {
      ItemComponent componentInParent = (ItemComponent) ((Component) hPoint).GetComponentInParent<ItemComponent>();
      List<Collider> colliderList = new List<Collider>();
      if (Object.op_Inequality((Object) componentInParent, (Object) null))
      {
        foreach (Collider componentsInChild in (Collider[]) ((Component) componentInParent).GetComponentsInChildren<Collider>())
        {
          if (((Component) componentsInChild).get_gameObject().get_layer() == LayerMask.NameToLayer("HArea"))
            colliderList.Add(componentsInChild);
        }
      }
      return colliderList;
    }

    public enum HEvent
    {
      Hpoint = -1, // 0xFFFFFFFF
      Normal = 0,
      Yobai = 1,
      Bath = 2,
      Toilet1 = 3,
      Toilet2 = 4,
      ShagmiBare = 5,
      Back = 6,
      Kitchen = 7,
      Tachi = 8,
      Stairs = 9,
      StairsBare = 10, // 0x0000000A
      GyakuYobai = 11, // 0x0000000B
      FromFemale = 12, // 0x0000000C
      MapBath = 13, // 0x0000000D
      KabeanaBack = 14, // 0x0000000E
      KabeanaFront = 15, // 0x0000000F
      Neonani = 16, // 0x00000010
      TsukueBare = 17, // 0x00000011
    }
  }
}
