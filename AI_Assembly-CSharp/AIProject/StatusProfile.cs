// Decompiled with JetBrains decompiler
// Type: AIProject.StatusProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using UnityEngine;

namespace AIProject
{
  public class StatusProfile : SerializedScriptableObject
  {
    [SerializeField]
    private Threshold _cookPheromoneBuffMinMax;
    [SerializeField]
    private Threshold _cookPheromoneBuff;
    [SerializeField]
    private float _buffCook;
    [SerializeField]
    private float _buffBath;
    [SerializeField]
    private float _buffAnimal;
    [SerializeField]
    private float _buffSleep;
    [SerializeField]
    private Threshold _sleepSociabilityBuffMinMax;
    [SerializeField]
    private Threshold _sleepSociabilityBuff;
    [SerializeField]
    private float _buffGift;
    [SerializeField]
    private Threshold _giftReliabilityBuffMinMax;
    [SerializeField]
    private Threshold _giftReliabilityBuff;
    [SerializeField]
    private float _buffGimme;
    [SerializeField]
    private Threshold _gimmeDarknessBuffMinMax;
    [SerializeField]
    private Threshold _gimmeDarknessBuff;
    [SerializeField]
    private Threshold _gimmeWarinessBuffMinMax;
    [SerializeField]
    private Threshold _gimmeWarinessBuff;
    [SerializeField]
    private float _buffEat;
    [SerializeField]
    private Threshold _eatPheromoneDebuffMinMax;
    [SerializeField]
    private Threshold _eatPheromoneDebuff;
    [SerializeField]
    private Threshold _eatDarknessDebuffMinMax;
    [SerializeField]
    private Threshold _eatDarknessDebuff;
    [SerializeField]
    private Threshold _eatInstinctBuffMinMax;
    [SerializeField]
    private Threshold _eatInstinctBuff;
    [SerializeField]
    private float _buffPlay;
    [SerializeField]
    private Threshold _playReasonDebuffMinMax;
    [SerializeField]
    private Threshold _playReasonDebuff;
    [SerializeField]
    private Threshold _playInstinctBuffMinMax;
    [SerializeField]
    private Threshold _playInstinctBuff;
    [SerializeField]
    private float _buffH;
    [SerializeField]
    private Threshold _hDirtyBuffMinMax;
    [SerializeField]
    private Threshold _hDirtyBuff;
    [SerializeField]
    private float _cursedHBuff;
    [SerializeField]
    private float _buffLonely;
    [SerializeField]
    private float _buffLonelySuperSense;
    [SerializeField]
    private Threshold _lonelySociabilityBuffMinMax;
    [SerializeField]
    private Threshold _lonelySociabilityBuff;
    [SerializeField]
    private Threshold _breakReasonBuffMinMax;
    [SerializeField]
    private Threshold _breakReasonBuff;
    [SerializeField]
    private Threshold _breakInstinctBuffMinMax;
    [SerializeField]
    private Threshold _breakInstinctBuff;
    [SerializeField]
    private float _buffBreak;
    [SerializeField]
    private float _buffLocation;
    [SerializeField]
    private float _buffSearchTough;
    [SerializeField]
    private float _buffSearch;
    [SerializeField]
    private Threshold _searchWarinessBuffMinMax;
    [SerializeField]
    private Threshold _searchWarinessBuff;
    [SerializeField]
    private Threshold _searchDarknessDebuffMinMax;
    [SerializeField]
    private Threshold _searchDarknessDebuff;
    [SerializeField]
    private Threshold _drinkWarinessBuffMinMax;
    [SerializeField]
    private Threshold _drinkWarinessBuff;
    [SerializeField]
    private float _debuffMood;
    [SerializeField]
    private float _debuffMoodInBathDesire;
    [SerializeField]
    private float _buffImmoral;
    [SerializeField]
    private float _gWifeMotivationBuff;
    [SerializeField]
    private float _activeBuffMotivation;
    [SerializeField]
    private float _healthyPhysicalBorder;
    [SerializeField]
    private float _cursedPhysicalBuff;
    [SerializeField]
    private Threshold _darknessPhysicalBuffMinMax;
    [SerializeField]
    private Threshold _darknessPhysicalBuff;
    [SerializeField]
    private Threshold _dirtyImmoralMinMax;
    [SerializeField]
    private Threshold _dirtyImmoralBuff;
    [SerializeField]
    private float _immoralBuff;
    [SerializeField]
    private int _lustImmoralBuff;
    [SerializeField]
    private int _firedBodyImmoralBuff;
    [SerializeField]
    private float _cursedImmoralBuff;
    [SerializeField]
    private int _lesbianFriendlyRelationBorder;
    [SerializeField]
    private float _canClothChangeBorder;
    [SerializeField]
    private Threshold _clothChangePheromoneValueMinMax;
    [SerializeField]
    private Threshold _clothChangePheromoneValue;
    [SerializeField]
    private int _darknessReduceMaiden;
    [SerializeField]
    private int _reliabilityGWifeBuff;
    [SerializeField]
    private int _masturbationBorder;
    [SerializeField]
    private int _invitationBorder;
    [SerializeField]
    private int _revRapeBorder;
    [SerializeField]
    private int _lesbianBorder;
    [SerializeField]
    private int _holdingHandBorderReliability;
    [SerializeField]
    private int _approachBorderReliability;
    [SerializeField]
    private int _canGreetBorder;
    [SerializeField]
    private float _canDressBorder;
    [SerializeField]
    private int _washFaceBorder;
    [SerializeField]
    private int _nightLightBorder;
    [SerializeField]
    private int _surpriseBorder;
    [SerializeField]
    private int _girlsActionBorder;
    [SerializeField]
    private int _talkRelationUpperBorder;
    [SerializeField]
    private int _lesbianSociabilityBuffBorder;
    [SerializeField]
    private Threshold _flavorCookSuccessBoostMinMax;
    [SerializeField]
    private Threshold _flavorCookSuccessBoost;
    [SerializeField]
    private int _chefCookSuccessBoost;
    [SerializeField]
    private Threshold _flavorCatCaptureMinMax;
    [SerializeField]
    private Threshold _flavorCatCaptureRate;
    [SerializeField]
    private int _catCaptureProbBuff;
    [SerializeField]
    private float _defaultInstructionRate;
    [SerializeField]
    private Threshold _flavorReliabilityInstructionMinMax;
    [SerializeField]
    private Threshold _flavorReliabilityInstruction;
    [SerializeField]
    private float _instructionRateDebuff;
    [SerializeField]
    private float _defaultFollowRate;
    [SerializeField]
    private Threshold _followReliabilityMinMax;
    [SerializeField]
    private Threshold _followRateReliabilityBuff;
    [SerializeField]
    private float _followRateBuff;
    [SerializeField]
    private Threshold _dropBuffMinMax;
    [SerializeField]
    private Threshold _dropBuff;
    [SerializeField]
    private float _girlsActionProb;
    [SerializeField]
    private float _lesbianRate;
    [SerializeField]
    private float _shallowSleepProb;
    [SerializeField]
    private Threshold _yobaiMinMax;
    [SerializeField]
    private float _callProbBaseRate;
    [SerializeField]
    private float _callProbPhaseRate;
    [SerializeField]
    private int[] _callReliabilityBorder;
    [SerializeField]
    private float[] _callReliabilityBuff;
    [SerializeField]
    private float _callLowerMoodProb;
    [SerializeField]
    private float _callUpperMoodProb;
    [SerializeField]
    private float _callSecondTimeProb;
    [SerializeField]
    private float _callOverTimeProb;
    [SerializeField]
    private float _callProbSuperSense;
    [SerializeField]
    private float _handSearchProbBuff;
    [SerializeField]
    private float _fishingSearchProbBuff;
    [SerializeField]
    private float _pickelSearchProbBuff;
    [SerializeField]
    private float _shovelSearchProbBuff;
    [SerializeField]
    private float _netSearchProbBuff;
    [SerializeField]
    private float _coldDefaultIncidence;
    [SerializeField]
    private float _coldLockDuration;
    [SerializeField]
    private int _coldBaseDuration;
    [SerializeField]
    private float _heatStrokeDefaultIncidence;
    [SerializeField]
    private float _heatStrokeLockDuration;
    [SerializeField]
    private float _hurtDefaultIncidence;
    [SerializeField]
    private Threshold _stomachacheRateDebuffMinMax;
    [SerializeField]
    private Threshold _stomachacheRateBuff;
    [SerializeField]
    private Threshold _coldRateBuffMinMax;
    [SerializeField]
    private Threshold _coldRateBuff;
    [SerializeField]
    private Threshold _heatStrokeRateBuffMinMax;
    [SerializeField]
    private Threshold _heatStrokeRateBuff;
    [SerializeField]
    private Threshold _hurtRateBuffMinMax;
    [SerializeField]
    private Threshold _hurtRateBuff;
    [SerializeField]
    private Threshold _sickIncidenceDarknessBuffMinMax;
    [SerializeField]
    private Threshold _sickIncidenceDarknessBuff;
    [SerializeField]
    private float _coldRateDebuffWeak;
    [SerializeField]
    private float _heatStrokeBuffGuts;
    [SerializeField]
    private int _starveWarinessValue;
    [SerializeField]
    private int _starveDarknessValue;
    [SerializeField]
    private float _wetRateInRain;
    [SerializeField]
    private float _wetRateInStorm;
    [SerializeField]
    private float _drySpeed;
    [SerializeField]
    private float _wetTemperatureRate;
    [SerializeField]
    private float _coldTemperatureValue;
    [SerializeField]
    private float _hotTemperatureValue;
    [SerializeField]
    private float _lesbianBorderDesire;
    [SerializeField]
    private float _shallowSleepHungerLowBorder;
    [SerializeField]
    private int _lampEquipableBorder;
    [SerializeField]
    private EnvironmentSimulator.DateTimeSerialization _shouldRestoreCoordTime;
    [SerializeField]
    private float _restoreRangeMinuteTime;
    [SerializeField]
    private int _soineReliabilityBorder;
    [SerializeField]
    private float _potionImmoralAdd;
    [SerializeField]
    private float _diureticToiletAdd;
    [SerializeField]
    private float _pillSleepAdd;

    public StatusProfile()
    {
      base.\u002Ector();
    }

    public Threshold CookPheromoneBuffMinMax
    {
      get
      {
        return this._cookPheromoneBuffMinMax;
      }
    }

    public Threshold CookPheromoneBuff
    {
      get
      {
        return this._cookPheromoneBuff;
      }
    }

    public float BuffCook
    {
      get
      {
        return this._buffCook;
      }
    }

    public float BuffBath
    {
      get
      {
        return this._buffBath;
      }
    }

    public float BuffAnimal
    {
      get
      {
        return this._buffAnimal;
      }
    }

    public float BuffSleep
    {
      get
      {
        return this._buffSleep;
      }
    }

    public Threshold SleepSociabilityBuffMinMax
    {
      get
      {
        return this._sleepSociabilityBuffMinMax;
      }
    }

    public Threshold SleepSociabilityBuff
    {
      get
      {
        return this._sleepSociabilityBuff;
      }
    }

    public float BuffGift
    {
      get
      {
        return this._buffGift;
      }
    }

    public Threshold GiftReliabilityBuffMinMax
    {
      get
      {
        return this._giftReliabilityBuffMinMax;
      }
    }

    public Threshold GiftReliabilityBuff
    {
      get
      {
        return this._giftReliabilityBuff;
      }
    }

    public float BuffGimme
    {
      get
      {
        return this._buffGimme;
      }
    }

    public Threshold GimmeDarknessBuffMinMax
    {
      get
      {
        return this._gimmeDarknessBuffMinMax;
      }
    }

    public Threshold GimmeDarknessBuff
    {
      get
      {
        return this._gimmeDarknessBuff;
      }
    }

    public Threshold GimmeWarinessBuffMinMax
    {
      get
      {
        return this._gimmeWarinessBuffMinMax;
      }
    }

    public Threshold GimmeWarinessBuff
    {
      get
      {
        return this._gimmeWarinessBuff;
      }
    }

    public float BuffEat
    {
      get
      {
        return this._buffEat;
      }
    }

    public Threshold EatPheromoneDebuffMinMax
    {
      get
      {
        return this._eatPheromoneDebuffMinMax;
      }
    }

    public Threshold EatPheromoneDebuff
    {
      get
      {
        return this._eatPheromoneDebuff;
      }
    }

    public Threshold EatDarknessDebuffMinMax
    {
      get
      {
        return this._eatDarknessDebuffMinMax;
      }
    }

    public Threshold EatDarknessDebuff
    {
      get
      {
        return this._eatDarknessDebuff;
      }
    }

    public Threshold EatInstinctBuffMinMax
    {
      get
      {
        return this._eatInstinctBuffMinMax;
      }
    }

    public Threshold EatInstinctBuff
    {
      get
      {
        return this._eatInstinctBuff;
      }
    }

    public float BuffPlay
    {
      get
      {
        return this._buffPlay;
      }
    }

    public Threshold PlayReasonDebuffMinMax
    {
      get
      {
        return this._playReasonDebuffMinMax;
      }
    }

    public Threshold PlayReasonDebuff
    {
      get
      {
        return this._playReasonDebuff;
      }
    }

    public Threshold PlayInstinctBuffMinMax
    {
      get
      {
        return this._playInstinctBuffMinMax;
      }
    }

    public Threshold PlayInstinctBuff
    {
      get
      {
        return this._playInstinctBuff;
      }
    }

    public float BuffH
    {
      get
      {
        return this._buffH;
      }
    }

    public Threshold HDirtyBuffMinMax
    {
      get
      {
        return this._hDirtyBuffMinMax;
      }
    }

    public Threshold HDirtyBuff
    {
      get
      {
        return this._hDirtyBuff;
      }
    }

    public float CursedHBuff
    {
      get
      {
        return this._cursedHBuff;
      }
    }

    public float BuffLonely
    {
      get
      {
        return this._buffLonely;
      }
    }

    public float BuffLonelySuperSense
    {
      get
      {
        return this._buffLonelySuperSense;
      }
    }

    public Threshold LonelySociabilityBuffMinMax
    {
      get
      {
        return this._lonelySociabilityBuffMinMax;
      }
    }

    public Threshold LonelySociabilityBuff
    {
      get
      {
        return this._lonelySociabilityBuff;
      }
    }

    public Threshold BreakReasonBuffMinMax
    {
      get
      {
        return this._breakReasonBuffMinMax;
      }
    }

    public Threshold BreakReasonBuff
    {
      get
      {
        return this._breakReasonBuff;
      }
    }

    public Threshold BreakInstinctBuffMinMax
    {
      get
      {
        return this._breakInstinctBuffMinMax;
      }
    }

    public Threshold BreakInstinctBuff
    {
      get
      {
        return this._breakInstinctBuff;
      }
    }

    public float BuffBreak
    {
      get
      {
        return this._buffBreak;
      }
    }

    public float BuffLocation
    {
      get
      {
        return this._buffLocation;
      }
    }

    public float BuffSearchTough
    {
      get
      {
        return this._buffSearchTough;
      }
    }

    public float BuffSearch
    {
      get
      {
        return this._buffSearch;
      }
    }

    public Threshold SearchWarinessBuffMinMax
    {
      get
      {
        return this._searchWarinessBuffMinMax;
      }
    }

    public Threshold SearchWarinessBuff
    {
      get
      {
        return this._searchWarinessBuff;
      }
    }

    public Threshold SearchDarknessDebuffMinMax
    {
      get
      {
        return this._searchDarknessDebuffMinMax;
      }
    }

    public Threshold SearchDarknessDebuff
    {
      get
      {
        return this._searchDarknessDebuff;
      }
    }

    public Threshold DrinkWarinessBuffMinMax
    {
      get
      {
        return this._drinkWarinessBuffMinMax;
      }
    }

    public Threshold DrinkWarinessBuff
    {
      get
      {
        return this._drinkWarinessBuff;
      }
    }

    public float DebuffMood
    {
      get
      {
        return this._debuffMood;
      }
    }

    public float DebuffMoodInBathDesire
    {
      get
      {
        return this._debuffMoodInBathDesire;
      }
    }

    public float BuffImmoral
    {
      get
      {
        return this._buffImmoral;
      }
    }

    public float GWifeMotivationBuff
    {
      get
      {
        return this._gWifeMotivationBuff;
      }
    }

    public float ActiveBuffMotivation
    {
      get
      {
        return this._activeBuffMotivation;
      }
    }

    public float HealthyPhysicalBorder
    {
      get
      {
        return this._healthyPhysicalBorder;
      }
    }

    public float CursedPhysicalBuff
    {
      get
      {
        return this._cursedPhysicalBuff;
      }
    }

    public Threshold DarknessPhysicalBuffMinMax
    {
      get
      {
        return this._darknessPhysicalBuffMinMax;
      }
    }

    public Threshold DarknessPhysicalBuff
    {
      get
      {
        return this._darknessPhysicalBuff;
      }
    }

    public Threshold DirtyImmoralMinMax
    {
      get
      {
        return this._dirtyImmoralMinMax;
      }
    }

    public Threshold DirtyImmoralBuff
    {
      get
      {
        return this._dirtyImmoralBuff;
      }
    }

    public float ImmoralBuff
    {
      get
      {
        return this._immoralBuff;
      }
    }

    public int LustImmoralBuff
    {
      get
      {
        return this._lustImmoralBuff;
      }
    }

    public int FiredBodyImmoralBuff
    {
      get
      {
        return this._firedBodyImmoralBuff;
      }
    }

    public float CursedImmoralBuff
    {
      get
      {
        return this._cursedImmoralBuff;
      }
    }

    public int LesbianFriendlyRelationBorder
    {
      get
      {
        return this._lesbianFriendlyRelationBorder;
      }
    }

    public float CanClothChangeBorder
    {
      get
      {
        return this._canClothChangeBorder;
      }
    }

    public Threshold ClothChangePheromoneValueMinMax
    {
      get
      {
        return this._clothChangePheromoneValueMinMax;
      }
    }

    public Threshold ClothChangePheromoneValue
    {
      get
      {
        return this._clothChangePheromoneValue;
      }
    }

    public int DarknessReduceMaiden
    {
      get
      {
        return this._darknessReduceMaiden;
      }
    }

    public int ReliabilityGWife
    {
      get
      {
        return this._reliabilityGWifeBuff;
      }
    }

    public int MasturbationBorder
    {
      get
      {
        return this._masturbationBorder;
      }
    }

    public int InvitationBorder
    {
      get
      {
        return this._invitationBorder;
      }
    }

    public int RevRapeBorder
    {
      get
      {
        return this._revRapeBorder;
      }
    }

    public int LesbianBorder
    {
      get
      {
        return this._lesbianBorder;
      }
    }

    public int HoldingHandBorderReliability
    {
      get
      {
        return this._holdingHandBorderReliability;
      }
    }

    public int ApproachBorderReliability
    {
      get
      {
        return this._approachBorderReliability;
      }
    }

    public float CanGreetBorder
    {
      get
      {
        return (float) this._canGreetBorder;
      }
    }

    public float CanDressBorder
    {
      get
      {
        return this._canDressBorder;
      }
    }

    public int WashFaceBorder
    {
      get
      {
        return this._washFaceBorder;
      }
    }

    public int NightLightBorder
    {
      get
      {
        return this._nightLightBorder;
      }
    }

    public int SurpriseBorder
    {
      get
      {
        return this._surpriseBorder;
      }
    }

    public int GirlsActionBorder
    {
      get
      {
        return this._girlsActionBorder;
      }
    }

    public int TalkRelationUpperBorder
    {
      get
      {
        return this._talkRelationUpperBorder;
      }
    }

    public int LesbianSociabilityBuffBorder
    {
      get
      {
        return this._lesbianSociabilityBuffBorder;
      }
    }

    public Threshold FlavorCookSuccessBoostMinMax
    {
      get
      {
        return this._flavorCookSuccessBoostMinMax;
      }
    }

    public Threshold FlavorCookSuccessBoost
    {
      get
      {
        return this._flavorCookSuccessBoost;
      }
    }

    public int ChefCookSuccessBoost
    {
      get
      {
        return this._chefCookSuccessBoost;
      }
    }

    public Threshold FlavorCatCaptureMinMax
    {
      get
      {
        return this._flavorCatCaptureMinMax;
      }
    }

    public Threshold FlavorCatCaptureRate
    {
      get
      {
        return this._flavorCatCaptureRate;
      }
    }

    public int CatCaptureProbBuff
    {
      get
      {
        return this._catCaptureProbBuff;
      }
    }

    public float DefaultInstructionRate
    {
      get
      {
        return this._defaultInstructionRate;
      }
    }

    public Threshold FlavorReliabilityInstructionMinMax
    {
      get
      {
        return this._flavorReliabilityInstructionMinMax;
      }
    }

    public Threshold FlavorReliabilityInstruction
    {
      get
      {
        return this._flavorReliabilityInstruction;
      }
    }

    public float InstructionRateDebuff
    {
      get
      {
        return this._instructionRateDebuff;
      }
    }

    public float DefaultFollowRate
    {
      get
      {
        return this._defaultFollowRate;
      }
    }

    public Threshold FollowReliabilityMinMax
    {
      get
      {
        return this._followReliabilityMinMax;
      }
    }

    public Threshold FollowRateReliabilityBuff
    {
      get
      {
        return this._followRateReliabilityBuff;
      }
    }

    public float FollowRateBuff
    {
      get
      {
        return this._followRateBuff;
      }
    }

    public Threshold DropBuffMinMax
    {
      get
      {
        return this._dropBuffMinMax;
      }
    }

    public Threshold DropBuff
    {
      get
      {
        return this._dropBuff;
      }
    }

    public float GirlsActionProb
    {
      get
      {
        return this._girlsActionProb;
      }
    }

    public float LesbianRate
    {
      get
      {
        return this._lesbianRate;
      }
    }

    public float ShallowSleepProb
    {
      get
      {
        return this._shallowSleepProb;
      }
    }

    public Threshold YobaiMinMax
    {
      get
      {
        return this._yobaiMinMax;
      }
    }

    public float CallProbBaseRate
    {
      get
      {
        return this._callProbBaseRate;
      }
    }

    public float CallProbPhaseRate
    {
      get
      {
        return this._callProbPhaseRate;
      }
    }

    public int[] CallReliabilityBorder
    {
      get
      {
        return this._callReliabilityBorder;
      }
    }

    public float[] CallReliabilityBuff
    {
      get
      {
        return this._callReliabilityBuff;
      }
    }

    public float CallLowerMoodProb
    {
      get
      {
        return this._callLowerMoodProb;
      }
    }

    public float CallUpperMoodProb
    {
      get
      {
        return this._callUpperMoodProb;
      }
    }

    public float CallSecondTimeProb
    {
      get
      {
        return this._callSecondTimeProb;
      }
    }

    public float CallOverTimeProb
    {
      get
      {
        return this._callOverTimeProb;
      }
    }

    public float CallProbSuperSense
    {
      get
      {
        return this._callProbSuperSense;
      }
    }

    public float HandSearchProbBuff
    {
      get
      {
        return this._handSearchProbBuff;
      }
    }

    public float FishingSearchProbBuff
    {
      get
      {
        return this._fishingSearchProbBuff;
      }
    }

    public float PickelSearchProbBuff
    {
      get
      {
        return this._pickelSearchProbBuff;
      }
    }

    public float ShovelSearchProbBuff
    {
      get
      {
        return this._shovelSearchProbBuff;
      }
    }

    public float NetSearchProbBuff
    {
      get
      {
        return this._netSearchProbBuff;
      }
    }

    public float ColdDefaultIncidence
    {
      get
      {
        return this._coldDefaultIncidence;
      }
    }

    public float ColdLockDuration
    {
      get
      {
        return this._coldLockDuration;
      }
    }

    public int ColdBaseDuration
    {
      get
      {
        return this._coldBaseDuration;
      }
    }

    public float HeatStrokeDefaultIncidence
    {
      get
      {
        return this._heatStrokeDefaultIncidence;
      }
    }

    public float HeatStrokeLockDuration
    {
      get
      {
        return this._coldLockDuration;
      }
    }

    public float HurtDefaultIncidence
    {
      get
      {
        return this._hurtDefaultIncidence;
      }
    }

    public Threshold StomachacheRateDebuffMinMax
    {
      get
      {
        return this._stomachacheRateDebuffMinMax;
      }
    }

    public Threshold StomachacheRateBuff
    {
      get
      {
        return this._stomachacheRateBuff;
      }
    }

    public Threshold ColdRateBuffMinMax
    {
      get
      {
        return this._coldRateBuffMinMax;
      }
    }

    public Threshold ColdRateBuff
    {
      get
      {
        return this._coldRateBuff;
      }
    }

    public Threshold HeatStrokeRateBuffMinMax
    {
      get
      {
        return this._coldRateBuffMinMax;
      }
    }

    public Threshold HeatStrokeRateBuff
    {
      get
      {
        return this._heatStrokeRateBuff;
      }
    }

    public Threshold HurtRateBuffMinMax
    {
      get
      {
        return this._hurtRateBuffMinMax;
      }
    }

    public Threshold HurtRateBuff
    {
      get
      {
        return this._hurtRateBuff;
      }
    }

    public Threshold SickIncidenceDarknessBuffMinMax
    {
      get
      {
        return this._sickIncidenceDarknessBuffMinMax;
      }
    }

    public Threshold SickIncidenceDarknessBuff
    {
      get
      {
        return this._sickIncidenceDarknessBuff;
      }
    }

    public float ColdRateDebuffWeak
    {
      get
      {
        return this._coldRateDebuffWeak;
      }
    }

    public float HeatStrokeBuffGuts
    {
      get
      {
        return this._heatStrokeBuffGuts;
      }
    }

    public int StarveWarinessValue
    {
      get
      {
        return this._starveWarinessValue;
      }
    }

    public int StarveDarknessValue
    {
      get
      {
        return this._starveDarknessValue;
      }
    }

    public float WetRateInRain
    {
      get
      {
        return this._wetRateInRain;
      }
    }

    public float WetRateInStorm
    {
      get
      {
        return this._wetRateInStorm;
      }
    }

    public float DrySpeed
    {
      get
      {
        return this._drySpeed;
      }
    }

    public float WetTemperatureRate
    {
      get
      {
        return this._wetTemperatureRate;
      }
    }

    public float ColdTemperatureValue
    {
      get
      {
        return this._coldTemperatureValue;
      }
    }

    public float HotTemperatureValue
    {
      get
      {
        return this._hotTemperatureValue;
      }
    }

    public float LesbianBorderDesire
    {
      get
      {
        return this._lesbianBorderDesire;
      }
    }

    public float ShallowSleepHungerLowBorder
    {
      get
      {
        return this._shallowSleepHungerLowBorder;
      }
    }

    public int LampEquipableBorder
    {
      get
      {
        return this._lampEquipableBorder;
      }
    }

    public EnvironmentSimulator.DateTimeSerialization ShouldRestoreCoordTime
    {
      get
      {
        return this._shouldRestoreCoordTime;
      }
    }

    public float RestoreRangeMinuteTime
    {
      get
      {
        return this._restoreRangeMinuteTime;
      }
    }

    public int SoineReliabilityBorder
    {
      get
      {
        return this._soineReliabilityBorder;
      }
    }

    public float PotionImmoralAdd
    {
      get
      {
        return this._potionImmoralAdd;
      }
    }

    public float DiureticToiletAdd
    {
      get
      {
        return this._diureticToiletAdd;
      }
    }

    public float PillSleepAdd
    {
      get
      {
        return this._pillSleepAdd;
      }
    }
  }
}
