// Decompiled with JetBrains decompiler
// Type: AIProject.AgentProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.UI.Viewer;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class AgentProfile : SerializedScriptableObject
  {
    private ReadOnlyDictionary<EventType, EventType> _afterActionReadOnlyTable;
    [SerializeField]
    private Dictionary<EventType, EventType> _afterActionTable;
    [SerializeField]
    private List<Desire.ActionType> _encounterWhitelist;
    [SerializeField]
    private List<Desire.ActionType> _strollWhitelist;
    [SerializeField]
    private List<Desire.ActionType> _blackListInSaveAndLoad;
    [SerializeField]
    private Dictionary<int, int> _defaultAreaIDTable;
    [SerializeField]
    private AgentProfile.WalkParameter _walkSetting;
    [SerializeField]
    private int _avoidancePriorityDefault;
    [SerializeField]
    private int _avoidancePriorityStationary;
    [SerializeField]
    private float _actionPointNearDistance;
    [SerializeField]
    private ThresholdInt _escapeViaPointNumThreshold;
    [SerializeField]
    private AgentProfile.PoseIDCollection _poseIDTable;
    [SerializeField]
    private Dictionary<int, PoseKeyPair> _advIdleTable;
    [SerializeField]
    private Dictionary<int, PoseKeyPair> _advHouchiTable;
    [SerializeField]
    private Dictionary<int, PoseKeyPair> _advLeaveTable;
    [SerializeField]
    private Dictionary<int, PoseKeyPair> _advBreastTable;
    [SerializeField]
    private Dictionary<int, PoseKeyPair> _advBreastNoReactionTable;
    [SerializeField]
    private AgentProfile.NormalSkillIDDefines _normalSkillIDSetting;
    [SerializeField]
    private AgentProfile.HSkillIDDefines _hSkillIDSetting;
    [SerializeField]
    private float _turnMinAngle;
    [SerializeField]
    private AgentProfile.RangeParameter _rangeSetting;
    [SerializeField]
    private AgentProfile.PhotoShotRangeParameter _photoShotRangeSetting;
    [SerializeField]
    private AgentProfile.ActionPointSightSetting _actionPointSight;
    [SerializeField]
    private int _defaultRelationShip;
    [SerializeField]
    private float _hSampleDistance;
    [SerializeField]
    private AgentProfile.SightSetting _characterFarSight;
    [SerializeField]
    private AgentProfile.SightSetting _characterNearSight;
    [SerializeField]
    private AgentProfile.SightSetting _animalSight;
    [SerializeField]
    private float _durationCTForCall;
    [SerializeField]
    private float _talkLockDuration;
    [SerializeField]
    private AgentProfile.DiminuationRates _diminuationRate;
    [SerializeField]
    private Dictionary<int, AgentProfile.DiminuationRates> _diminMotivationRate;
    [SerializeField]
    private AgentProfile.DiminuationRates _weaknessDiminuationRate;
    [SerializeField]
    private AgentProfile.DiminuationRates _talkMotivationDimRate;
    [SerializeField]
    private ThresholdInt _secondSleepDurationMinMax;
    [SerializeField]
    private Threshold _standDurationMinMax;
    [SerializeField]
    private ItemIDKeyPair[] _canStandEatItems;
    [SerializeField]
    private ItemIDKeyPair[] _lowerTempDrinkItems;
    [SerializeField]
    private ItemIDKeyPair[] _raiseTempDrinkItems;
    [SerializeField]
    private ItemIDKeyPair _coconutDrinkID;
    [SerializeField]
    private float _coldTempBorder;
    [SerializeField]
    private float _hotTempBorder;
    [SerializeField]
    private Dictionary<int, float> _motivationMinValueTable;
    [SerializeField]
    private float _activeMotivationBorder;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _mustRunMotivationPercent;
    [SerializeField]
    private Threshold _diminuationInMasturbation;
    [SerializeField]
    private Threshold _diminuationInLesbian;
    [SerializeField]
    private int _itemSlotMaxInInventory;
    [SerializeField]
    private int _itemSlotCountToItemBox;
    [SerializeField]
    private InventoryFacadeViewer.ItemFilter[] _presentItemFilter;
    [SerializeField]
    private ItemIDKeyPair[] _medicineNormalItemList;
    [SerializeField]
    private ItemIDKeyPair[] _medicineColdItemList;
    [SerializeField]
    private ItemIDKeyPair[] _medicineHurtItemList;
    [SerializeField]
    private ItemIDKeyPair[] _medicineStomachacheItemList;
    [SerializeField]
    private ItemIDKeyPair[] _medicineHeatStrokeItemList;
    [SerializeField]
    private ItemIDKeyPair _feverReducerID;
    [SerializeField]
    private ItemIDKeyPair _coldMedicineID;
    [SerializeField]
    private ItemIDKeyPair _stomachMeidicineID;
    [SerializeField]
    private ItemIDKeyPair _wetTowelID;
    [SerializeField]
    private Vector3 _offsetInDate;
    [SerializeField]
    private Vector3 _offsetInPartyS;
    [SerializeField]
    private Vector3 _offsetInPartyM;
    [SerializeField]
    private Vector3 _offsetInPartyL;
    [SerializeField]
    private float _restDistance;
    [SerializeField]
    private float _runDistance;
    [NavMeshAreaEnumMask]
    [SerializeField]
    private int _actionPointSightNavMeshArea;
    [SerializeField]
    private float _catEventBaseProb;
    [SerializeField]
    private Dictionary<int, PoseKeyPair> _tutorialIdlePoseTable;
    [SerializeField]
    private Dictionary<int, PoseKeyPair> _tutorialWakeUpPoseTable;
    [SerializeField]
    private AgentProfile.TutorialSetting _tutorial;

    public AgentProfile()
    {
      base.\u002Ector();
    }

    public ReadOnlyDictionary<EventType, EventType> AfterActionTable
    {
      get
      {
        return this._afterActionReadOnlyTable ?? (this._afterActionReadOnlyTable = new ReadOnlyDictionary<EventType, EventType>((IDictionary<EventType, EventType>) this._afterActionTable));
      }
    }

    public List<Desire.ActionType> EncounterWhitelist
    {
      get
      {
        return this._encounterWhitelist;
      }
    }

    public List<Desire.ActionType> ScrollWhitelist
    {
      get
      {
        return this._strollWhitelist;
      }
    }

    public List<Desire.ActionType> BlackListInSaveAndLoad
    {
      get
      {
        return this._blackListInSaveAndLoad;
      }
    }

    public Dictionary<int, int> DefaultAreaIDTable
    {
      get
      {
        return this._defaultAreaIDTable;
      }
    }

    public AgentProfile.WalkParameter WalkSetting
    {
      get
      {
        return this._walkSetting;
      }
    }

    public int AvoidancePriorityDefault
    {
      get
      {
        return this._avoidancePriorityDefault;
      }
    }

    public int AvoidancePriorityStationary
    {
      get
      {
        return this._avoidancePriorityStationary;
      }
    }

    public float ActionPointNearDistance
    {
      get
      {
        return this._actionPointNearDistance;
      }
    }

    public ThresholdInt EscapeViaPointNumThreshold
    {
      get
      {
        return this._escapeViaPointNumThreshold;
      }
    }

    public AgentProfile.PoseIDCollection PoseIDTable
    {
      get
      {
        return this._poseIDTable;
      }
    }

    public Dictionary<int, PoseKeyPair> ADVIdleTable
    {
      get
      {
        return this._advIdleTable;
      }
    }

    public Dictionary<int, PoseKeyPair> ADVHouchiTable
    {
      get
      {
        return this._advHouchiTable;
      }
    }

    public Dictionary<int, PoseKeyPair> ADVLeaveTable
    {
      get
      {
        return this._advLeaveTable;
      }
    }

    public Dictionary<int, PoseKeyPair> ADVBreastTable
    {
      get
      {
        return this._advBreastTable;
      }
    }

    public Dictionary<int, PoseKeyPair> ADVBreastNoReactionTable
    {
      get
      {
        return this._advBreastNoReactionTable;
      }
    }

    public AgentProfile.NormalSkillIDDefines NormalSkillIDSetting
    {
      get
      {
        return this._normalSkillIDSetting;
      }
    }

    public AgentProfile.HSkillIDDefines HSkillIDSetting
    {
      get
      {
        return this._hSkillIDSetting;
      }
    }

    public float TurnMinAngle
    {
      get
      {
        return this._turnMinAngle;
      }
    }

    public AgentProfile.RangeParameter RangeSetting
    {
      get
      {
        return this._rangeSetting;
      }
    }

    public AgentProfile.PhotoShotRangeParameter PhotoShotRangeSetting
    {
      get
      {
        return this._photoShotRangeSetting;
      }
    }

    public AgentProfile.ActionPointSightSetting ActionPointSight
    {
      get
      {
        return this._actionPointSight;
      }
    }

    public int DefaultRelationShip
    {
      get
      {
        return this._defaultRelationShip;
      }
    }

    public float HSampleDistance
    {
      get
      {
        return this._hSampleDistance;
      }
    }

    public AgentProfile.SightSetting CharacterFarSight
    {
      get
      {
        return this._characterFarSight;
      }
    }

    public AgentProfile.SightSetting CharacterNearSight
    {
      get
      {
        return this._characterNearSight;
      }
    }

    public AgentProfile.SightSetting AnimalSight
    {
      get
      {
        return this._animalSight;
      }
    }

    public float DurationCTForCall
    {
      get
      {
        return this._durationCTForCall;
      }
    }

    public float TalkLockDuration
    {
      get
      {
        return this._talkLockDuration;
      }
    }

    public AgentProfile.DiminuationRates DiminuationRate
    {
      get
      {
        return this._diminuationRate;
      }
    }

    public Dictionary<int, AgentProfile.DiminuationRates> DiminMotivationRate
    {
      get
      {
        return this._diminMotivationRate;
      }
    }

    public AgentProfile.DiminuationRates WeaknessDiminuationRate
    {
      get
      {
        return this._weaknessDiminuationRate;
      }
    }

    public AgentProfile.DiminuationRates TalkMotivationDimRate
    {
      get
      {
        return this._talkMotivationDimRate;
      }
    }

    public ThresholdInt SecondSleepDurationMinMax
    {
      get
      {
        return this._secondSleepDurationMinMax;
      }
    }

    public Threshold StandDurationMinMax
    {
      get
      {
        return this._standDurationMinMax;
      }
    }

    public ItemIDKeyPair[] CanStandEatItems
    {
      get
      {
        return this._canStandEatItems;
      }
    }

    public ItemIDKeyPair[] LowerTempDrinkItems
    {
      get
      {
        return this._lowerTempDrinkItems;
      }
    }

    public ItemIDKeyPair[] RaiseTempDrinkItems
    {
      get
      {
        return this._raiseTempDrinkItems;
      }
    }

    public ItemIDKeyPair CoconutDrinkID
    {
      get
      {
        return this._coconutDrinkID;
      }
    }

    public float ColdTempBorder
    {
      get
      {
        return this._coldTempBorder;
      }
    }

    public float HotTempBorder
    {
      get
      {
        return this._hotTempBorder;
      }
    }

    public Dictionary<int, float> MotivationMinValueTable
    {
      get
      {
        return this._motivationMinValueTable;
      }
    }

    public float ActiveMotivationBorder
    {
      get
      {
        return this._activeMotivationBorder;
      }
    }

    public float MustRunMotivationPercent
    {
      get
      {
        return this._mustRunMotivationPercent;
      }
    }

    public Threshold DiminuationInMasturbation
    {
      get
      {
        return this._diminuationInMasturbation;
      }
    }

    public Threshold DiminuationInLesbian
    {
      get
      {
        return this._diminuationInLesbian;
      }
    }

    public int ItemSlotMaxInInventory
    {
      get
      {
        return this._itemSlotMaxInInventory;
      }
    }

    public int ItemSlotCountToItemBox
    {
      get
      {
        return this._itemSlotCountToItemBox;
      }
    }

    public InventoryFacadeViewer.ItemFilter[] PresentItemFilter
    {
      get
      {
        return this._presentItemFilter;
      }
    }

    public ItemIDKeyPair[] MedicineNormalItemList
    {
      get
      {
        return this._medicineNormalItemList;
      }
    }

    public ItemIDKeyPair[] MedicineColdItemList
    {
      get
      {
        return this._medicineColdItemList;
      }
    }

    public ItemIDKeyPair[] MedicineHurtItemList
    {
      get
      {
        return this._medicineHurtItemList;
      }
    }

    public ItemIDKeyPair[] MedicineStomachacheItemList
    {
      get
      {
        return this._medicineStomachacheItemList;
      }
    }

    public ItemIDKeyPair[] MedicineHeatStrokeItemList
    {
      get
      {
        return this._medicineHeatStrokeItemList;
      }
    }

    public ItemIDKeyPair FeverReducerID
    {
      get
      {
        return this._feverReducerID;
      }
    }

    public ItemIDKeyPair ColdMedicineID
    {
      get
      {
        return this._coldMedicineID;
      }
    }

    public ItemIDKeyPair StomachMedicineID
    {
      get
      {
        return this._stomachMeidicineID;
      }
    }

    public ItemIDKeyPair WetTowelID
    {
      get
      {
        return this._wetTowelID;
      }
    }

    public Vector3 OffsetInDate
    {
      get
      {
        return this._offsetInDate;
      }
    }

    public Vector3 GetOffsetInParty(float rate)
    {
      return (double) rate < 0.5 ? Vector3.Lerp(this._offsetInPartyS, this._offsetInPartyM, Mathf.InverseLerp(0.0f, 0.5f, rate)) : Vector3.Lerp(this._offsetInPartyM, this._offsetInPartyL, Mathf.InverseLerp(0.5f, 1f, rate));
    }

    public Vector3 OffsetInPartyS
    {
      get
      {
        return this._offsetInPartyS;
      }
    }

    public Vector3 OffsetInPartyM
    {
      get
      {
        return this._offsetInPartyM;
      }
    }

    public Vector3 OffsetInPartyL
    {
      get
      {
        return Vector3.get_zero();
      }
    }

    public float RestDistance
    {
      get
      {
        return this._restDistance;
      }
    }

    public float RunDistance
    {
      get
      {
        return this._runDistance;
      }
    }

    public int ActionPointSightNavMeshArea
    {
      get
      {
        return this._actionPointSightNavMeshArea;
      }
    }

    public float CatEventBaseProb
    {
      get
      {
        return this._catEventBaseProb;
      }
    }

    public Dictionary<int, PoseKeyPair> TutorialIdlePoseTable
    {
      get
      {
        return this._tutorialIdlePoseTable;
      }
    }

    public Dictionary<int, PoseKeyPair> TutorialWakeUpPoseTable
    {
      get
      {
        return this._tutorialWakeUpPoseTable;
      }
    }

    public AgentProfile.TutorialSetting Tutorial
    {
      get
      {
        return this._tutorial;
      }
    }

    [Serializable]
    public class NormalSkillIDDefines
    {
    }

    [Serializable]
    public struct HSkillIDDefines
    {
      public int homosexualID;
      public int groperID;
    }

    [Serializable]
    public class PoseIDCollection
    {
      [SerializeField]
      private PoseKeyPair _faintID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _collapseID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _comaID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _medicID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _cureID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _weaknessID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _greetPoseID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _coldPoseID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _hotPoseID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _coughID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _grossID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _yawnID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _wakeUpID = new PoseKeyPair();
      private PoseKeyPair _fearID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _chuckleID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _standHurtID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _surprisedID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _deepBreathID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _wakenUpID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _surpriseMasturbationID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _surpriseInToiletSquatID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _surpriseInToiletSitID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _surpriseInBathStandID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _surpriseInBathSitID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _surpriseInGoemonID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _chairCoughID = new PoseKeyPair();
      [SerializeField]
      [Header("しゃがみアクション指定")]
      [LabelText("しゃがみ怖がる")]
      private PoseKeyPair _squatFearID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _groomyID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _angryID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _hungryID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _ovationID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _rainUmbrellaID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _clearPoseID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _eatStandID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _eatChairID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _eatDeskID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _eatDishID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _snitchFoodID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _stealFoundID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _drinkStandID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _drinkChairID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _peeID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _sleepTogetherRight = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _sleepTogetherLeft = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _wateringID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _faceWash = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _handWash = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _yobai = new PoseKeyPair();
      [SerializeField]
      private int _locomotionID;
      [SerializeField]
      private int _walkLocoID;
      [SerializeField]
      private int _umbrellaLocoID;
      [SerializeField]
      private int _lampLocoID;
      [SerializeField]
      private int _lampWalkLocoID;
      [SerializeField]
      private int _hurtLocoID;
      [SerializeField]
      private int _mojimojiLocoID;
      [SerializeField]
      private int _rainRunLocoID;
      [SerializeField]
      private int _cookMoveLocoID;
      [SerializeField]
      private PoseKeyPair[] _normalIDList;
      [SerializeField]
      private PoseKeyPair[] _playGameStandIDList;
      [SerializeField]
      private PoseKeyPair[] _playGameStandOutdoorIDList;
      [SerializeField]
      private PoseKeyPair[] _appearIDList;

      public int LocomotionID
      {
        get
        {
          return this._locomotionID;
        }
      }

      public int WalkLocoID
      {
        get
        {
          return this._walkLocoID;
        }
      }

      public int UmbrellaLocoID
      {
        get
        {
          return this._umbrellaLocoID;
        }
      }

      public int LampLocoID
      {
        get
        {
          return this._lampLocoID;
        }
      }

      public int LampWalkLocoID
      {
        get
        {
          return this._lampWalkLocoID;
        }
      }

      public int HurtLocoID
      {
        get
        {
          return this._hurtLocoID;
        }
      }

      public int MojimojiLocoID
      {
        get
        {
          return this._mojimojiLocoID;
        }
      }

      public int RainRunLocoID
      {
        get
        {
          return this._rainRunLocoID;
        }
      }

      public int CookMoveLocoID
      {
        get
        {
          return this._cookMoveLocoID;
        }
      }

      public PoseKeyPair FaintID
      {
        get
        {
          return this._faintID;
        }
      }

      public PoseKeyPair CollapseID
      {
        get
        {
          return this._collapseID;
        }
      }

      public PoseKeyPair ComaID
      {
        get
        {
          return this._comaID;
        }
      }

      public PoseKeyPair MedicID
      {
        get
        {
          return this._medicID;
        }
      }

      public PoseKeyPair CureID
      {
        get
        {
          return this._cureID;
        }
      }

      public PoseKeyPair WeaknessID
      {
        get
        {
          return this._weaknessID;
        }
      }

      public PoseKeyPair GreetPoseID
      {
        get
        {
          return this._greetPoseID;
        }
      }

      public PoseKeyPair[] NormalIDList
      {
        get
        {
          return this._normalIDList;
        }
      }

      public PoseKeyPair ColdPoseID
      {
        get
        {
          return this._coldPoseID;
        }
      }

      public PoseKeyPair HotPoseID
      {
        get
        {
          return this._hotPoseID;
        }
      }

      public PoseKeyPair CoughID
      {
        get
        {
          return this._coughID;
        }
      }

      public PoseKeyPair GrossID
      {
        get
        {
          return this._grossID;
        }
      }

      public PoseKeyPair YawnID
      {
        get
        {
          return this._yawnID;
        }
      }

      public PoseKeyPair WakeUpID
      {
        get
        {
          return this._wakeUpID;
        }
      }

      public PoseKeyPair FearID
      {
        get
        {
          return this._fearID;
        }
      }

      public PoseKeyPair ChuckleID
      {
        get
        {
          return this._chuckleID;
        }
      }

      public PoseKeyPair StandHurtID
      {
        get
        {
          return this._standHurtID;
        }
      }

      public PoseKeyPair SurprisedID
      {
        get
        {
          return this._surprisedID;
        }
      }

      public PoseKeyPair DeepBreathID
      {
        get
        {
          return this._deepBreathID;
        }
      }

      public PoseKeyPair WakenUpID
      {
        get
        {
          return this._wakenUpID;
        }
      }

      public PoseKeyPair SurpriseMasturbationID
      {
        get
        {
          return this._surpriseMasturbationID;
        }
      }

      public PoseKeyPair SurpriseInToiletSquatID
      {
        get
        {
          return this._surpriseInToiletSquatID;
        }
      }

      public PoseKeyPair SurpriseInToiletSitID
      {
        get
        {
          return this._surpriseInToiletSitID;
        }
      }

      public PoseKeyPair SurpriseInBathStandID
      {
        get
        {
          return this._surpriseInBathStandID;
        }
      }

      public PoseKeyPair SurpriseInBathSitID
      {
        get
        {
          return this._surpriseInBathSitID;
        }
      }

      public PoseKeyPair SurpriseInGoemonID
      {
        get
        {
          return this._surpriseInGoemonID;
        }
      }

      public PoseKeyPair ChairCoughID
      {
        get
        {
          return this._chairCoughID;
        }
      }

      public PoseKeyPair SquatFearID
      {
        get
        {
          return this._squatFearID;
        }
      }

      public PoseKeyPair GroomyID
      {
        get
        {
          return this._groomyID;
        }
      }

      public PoseKeyPair AngryID
      {
        get
        {
          return this._angryID;
        }
      }

      public PoseKeyPair HungryID
      {
        get
        {
          return this._hungryID;
        }
      }

      public PoseKeyPair OvationID
      {
        get
        {
          return this._ovationID;
        }
      }

      public PoseKeyPair[] PlayGameStandIDList
      {
        get
        {
          return this._playGameStandIDList;
        }
      }

      public PoseKeyPair[] PlayGameStandOutdoorIDList
      {
        get
        {
          return this._playGameStandOutdoorIDList;
        }
      }

      public PoseKeyPair RainUmbrellaID
      {
        get
        {
          return this._rainUmbrellaID;
        }
      }

      public PoseKeyPair ClearPoseID
      {
        get
        {
          return this._clearPoseID;
        }
      }

      public PoseKeyPair EatStandID
      {
        get
        {
          return this._eatStandID;
        }
      }

      public PoseKeyPair EatChairID
      {
        get
        {
          return this._eatChairID;
        }
      }

      public PoseKeyPair EatDeskID
      {
        get
        {
          return this._eatDeskID;
        }
      }

      public PoseKeyPair EatDishID
      {
        get
        {
          return this._eatDishID;
        }
      }

      public PoseKeyPair SnitchFoodID
      {
        get
        {
          return this._snitchFoodID;
        }
      }

      public PoseKeyPair StealFoundID
      {
        get
        {
          return this._stealFoundID;
        }
      }

      public PoseKeyPair DrinkStandID
      {
        get
        {
          return this._drinkStandID;
        }
      }

      public PoseKeyPair DrinkChairID
      {
        get
        {
          return this._drinkChairID;
        }
      }

      public PoseKeyPair PeeID
      {
        get
        {
          return this._peeID;
        }
      }

      public PoseKeyPair SleepTogetherRight
      {
        get
        {
          return this._sleepTogetherRight;
        }
      }

      public PoseKeyPair SleepTogetherLeft
      {
        get
        {
          return this._sleepTogetherLeft;
        }
      }

      public PoseKeyPair WateringID
      {
        get
        {
          return this._wateringID;
        }
      }

      public PoseKeyPair FaceWash
      {
        get
        {
          return this._faceWash;
        }
      }

      public PoseKeyPair HandWash
      {
        get
        {
          return this._handWash;
        }
      }

      public PoseKeyPair Yobai
      {
        get
        {
          return this._yobai;
        }
      }

      public PoseKeyPair[] AppearIDList
      {
        get
        {
          return this._appearIDList;
        }
      }
    }

    [Serializable]
    public struct WalkParameter
    {
      [MinValue(3.0)]
      public int reservedPathCount;
      public float arrivedDistance;
      public ThresholdInt viaPointNumThreshold;

      public WalkParameter(int pathCount, float distance, int min, int max)
      {
        this.reservedPathCount = pathCount;
        this.arrivedDistance = distance;
        this.viaPointNumThreshold = new ThresholdInt(min, max);
      }

      public WalkParameter(int pathCount, float distance, ThresholdInt threshold)
      {
        this.reservedPathCount = pathCount;
        this.arrivedDistance = distance;
        this.viaPointNumThreshold = threshold;
      }
    }

    [Serializable]
    public struct RangeParameter
    {
      public float arrivedDistance;
      public float acceptableHeight;
      public float arrivedDistanceIncludeAct;
      public float acceptableHeightIncludeAct;
      public float leaveDistance;
      public float leaveDistanceInSurprise;

      public RangeParameter(
        float arrivedDistance_,
        float distIncludeAction,
        float height,
        float heightAction,
        float leaveDistance_,
        float leaveDistanceInSurprise_)
      {
        this.arrivedDistance = arrivedDistance_;
        this.arrivedDistanceIncludeAct = distIncludeAction;
        this.acceptableHeight = height;
        this.acceptableHeightIncludeAct = heightAction;
        this.leaveDistance = leaveDistance_;
        this.leaveDistanceInSurprise = leaveDistanceInSurprise_;
      }
    }

    [Serializable]
    public class ActionPointSightSetting
    {
      [SerializeField]
      private float _fovAngle = 90f;
      [SerializeField]
      private float _heightRange = 1f;
      [SerializeField]
      private Vector3 _offset = Vector3.get_zero();
      [SerializeField]
      private float _viewDistance;
      [SerializeField]
      private float _angleOffset2D;

      public float FOVAngle
      {
        get
        {
          return this._fovAngle;
        }
      }

      public float HeightRange
      {
        get
        {
          return this._heightRange;
        }
      }

      public float ViewDistance
      {
        get
        {
          return this._viewDistance;
        }
      }

      public Vector3 Offset
      {
        get
        {
          return this._offset;
        }
      }

      public float AngleOffset2D
      {
        get
        {
          return this._angleOffset2D;
        }
      }

      public bool HasEntered(Transform @base, Vector3 target, float radius)
      {
        if ((double) Vector3.Distance(target, @base.get_position()) > (double) this._viewDistance + (double) radius || (double) Mathf.Abs((float) (((Component) @base).get_transform().get_position().y - target.y)) > (double) this._heightRange)
          return false;
        float num = this._fovAngle / 2f;
        return (double) Vector3.Angle(Vector3.op_Subtraction(target, @base.get_position()), @base.get_forward()) < (double) num;
      }

      public bool HasEntered(Collider collider, Transform @base)
      {
        Vector3 vector3_1;
        switch (collider)
        {
          case SphereCollider _:
            vector3_1 = (collider as SphereCollider).get_center();
            break;
          case CapsuleCollider _:
            vector3_1 = (collider as CapsuleCollider).get_center();
            break;
          default:
            vector3_1 = Vector3.get_zero();
            break;
        }
        Vector3 vector3_2 = vector3_1;
        double num1;
        switch (collider)
        {
          case SphereCollider _:
            num1 = (double) (collider as SphereCollider).get_radius();
            break;
          case CapsuleCollider _:
            num1 = (double) (collider as CapsuleCollider).get_radius();
            break;
          default:
            num1 = 0.0;
            break;
        }
        float num2 = (float) num1;
        if ((double) Vector3.Distance(Vector3.op_Addition(((Component) collider).get_transform().get_position(), vector3_2), @base.get_position()) > (double) this._viewDistance + (double) num2 || (double) Mathf.Abs((float) (((Component) @base).get_transform().get_position().y - ((Component) collider).get_transform().get_position().y)) > (double) this._heightRange)
          return false;
        float num3 = this._fovAngle / 2f;
        return (double) Vector3.Angle(Vector3.op_Subtraction(((Component) collider).get_transform().get_position(), @base.get_position()), @base.get_forward()) < (double) num3;
      }
    }

    [Serializable]
    public class SightSetting
    {
      [SerializeField]
      private float _fovAngle = 90f;
      [SerializeField]
      private float _heightRange = 1f;
      [SerializeField]
      private Vector3 _offset = Vector3.get_zero();
      [SerializeField]
      private float _viewDistance;
      [SerializeField]
      private float _angleOffset2D;

      public float FOVAngle
      {
        get
        {
          return this._fovAngle;
        }
      }

      public float HeightRange
      {
        get
        {
          return this._heightRange;
        }
      }

      public float ViewDistance
      {
        get
        {
          return this._viewDistance;
        }
      }

      public Vector3 Offset
      {
        get
        {
          return this._offset;
        }
      }

      public float AngleOffset2D
      {
        get
        {
          return this._angleOffset2D;
        }
      }

      public bool HasEntered(Transform @base, Vector3 targetPosition)
      {
        if ((double) Vector3.Distance(targetPosition, @base.get_position()) > (double) this._viewDistance || (double) Mathf.Abs((float) (((Component) @base).get_transform().get_position().y - targetPosition.y)) > (double) this._heightRange)
          return false;
        float num = this._fovAngle / 2f;
        Vector3 vector3_1;
        ((Vector3) ref vector3_1).\u002Ector((float) @base.get_position().x, 0.0f, (float) @base.get_position().z);
        Vector3 vector3_2;
        ((Vector3) ref vector3_2).\u002Ector((float) targetPosition.x, 0.0f, (float) targetPosition.z);
        return (double) Vector3.Angle(Vector3.op_Subtraction(vector3_2, vector3_1), @base.get_forward()) < (double) num;
      }

      public bool HasEntered(Transform @base, Vector3 targetPosition, float angleOffsetY)
      {
        if ((double) this._viewDistance < (double) Vector3.Distance(targetPosition, @base.get_position()) || (double) Mathf.Abs((float) (((Component) @base).get_transform().get_position().y - targetPosition.y)) > (double) this._heightRange)
          return false;
        float num = this._fovAngle / 2f;
        Vector3 vector3_1 = Quaternion.op_Multiply(Quaternion.AngleAxis(angleOffsetY, @base.get_up()), @base.get_forward());
        Vector3 vector3_2;
        ((Vector3) ref vector3_2).\u002Ector((float) @base.get_position().x, 0.0f, (float) @base.get_position().z);
        Vector3 vector3_3;
        ((Vector3) ref vector3_3).\u002Ector((float) targetPosition.x, 0.0f, (float) targetPosition.z);
        return (double) Vector3.Angle(Vector3.op_Subtraction(vector3_3, vector3_2), vector3_1) < (double) num;
      }

      public void DrawGizmos(Transform transform)
      {
      }

      public void DrawGizmos(Transform transform, float angleOffsetY)
      {
      }
    }

    [Serializable]
    public class TutorialSetting
    {
      [SerializeField]
      private string _defaultStateName = string.Empty;
      [SerializeField]
      private PoseKeyPair _goGhroughAnimID = new PoseKeyPair();
      [SerializeField]
      private int[] _goGhroughActionIDList = new int[0];
      [SerializeField]
      private PoseKeyPair _threeStepJumpAnimID = new PoseKeyPair();
      [SerializeField]
      private int[] _threeStepJumpActionIDList = new int[0];
      [SerializeField]
      private int _animatorID;
      [SerializeField]
      private float _defaultStateFadeTime;

      public int AnimatorID
      {
        get
        {
          return this._animatorID;
        }
      }

      public string DefaultStateName
      {
        get
        {
          return this._defaultStateName;
        }
      }

      public float DefaultStateFadeTime
      {
        get
        {
          return this._defaultStateFadeTime;
        }
      }

      public PoseKeyPair GoGhroughAnimID
      {
        get
        {
          return this._goGhroughAnimID;
        }
      }

      public int[] GoGhroughActionIDList
      {
        get
        {
          return this._goGhroughActionIDList;
        }
      }

      public PoseKeyPair ThreeStepJumpAnimID
      {
        get
        {
          return this._threeStepJumpAnimID;
        }
      }

      public int[] ThreeStepJumpActionIDList
      {
        get
        {
          return this._threeStepJumpActionIDList;
        }
      }
    }

    [Serializable]
    public struct MapMasturbationSetting
    {
      public int wMotivation;
      public int mMotivation;
      public int sMotivation;
      public int oMotivation;
    }

    [Serializable]
    public struct MapLesbianSetting
    {
      public int wMotivation;
      public int sMotivation;
      public int oMotivation;
    }

    [Serializable]
    public struct DiminuationRates
    {
      [SerializeField]
      private float _durationInHour;
      [SerializeField]
      private float _durationInHourRecovery;
      public float value;
      public float valueRecovery;

      public DiminuationRates(float value_, float recovery)
      {
        this.value = value_;
        this.valueRecovery = recovery;
        this._durationInHour = 0.0f;
        this._durationInHourRecovery = 0.0f;
      }

      public static AgentProfile.DiminuationRates Default
      {
        get
        {
          return new AgentProfile.DiminuationRates()
          {
            value = 0.0f
          };
        }
      }

      private float DurationInHour
      {
        get
        {
          return this._durationInHour;
        }
      }

      private float DurationInHourRecovery
      {
        get
        {
          return this._durationInHourRecovery;
        }
      }
    }

    [Serializable]
    public struct PhotoShotRangeParameter
    {
      public float arriveDistance;
      public float acceptableHeight;
      public float leaveDistance;
      public float sightAngle;
      public float sightOffsetZ;
      public float invisibleAngle;
      public float reliabilityBorder;
    }
  }
}
