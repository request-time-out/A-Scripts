// Decompiled with JetBrains decompiler
// Type: AIProject.CommonDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class CommonDefine : SerializedScriptableObject
  {
    [SerializeField]
    private CommonDefine.FileNamesGroup _fileNames;
    [SerializeField]
    private CommonDefine.CommonIconGroup _icon;
    [SerializeField]
    private CommonDefine.ItemIDDefines _itemIDDefine;
    [SerializeField]
    private ItemIconIDDefine _itemIconIDDefine;
    [SerializeField]
    private Dictionary<int, Dictionary<int, int>> _searchItemGradeTable;
    [SerializeField]
    private CommonDefine.ItemAnimGroup _itemAnims;
    [SerializeField]
    private Dictionary<int, string[]> _appearCameraInStates;
    [SerializeField]
    private Dictionary<int, string[]> _openingWakeUpCameraInStates;
    [SerializeField]
    private RarelityProbability _probRarelityProfile;
    [SerializeField]
    private WeatherProbability _probWeatherProfile;
    [SerializeField]
    private SearchAreaProbabilities _probSearchAreaProfile;
    [SerializeField]
    private CommonDefine.TutorialSetting _tutorial;
    [SerializeField]
    private CommonDefine.EventStoryInfoGroup _eventStoryInfo;

    public CommonDefine()
    {
      base.\u002Ector();
    }

    public CommonDefine.FileNamesGroup FileNames
    {
      get
      {
        return this._fileNames;
      }
    }

    public CommonDefine.CommonIconGroup Icon
    {
      get
      {
        return this._icon;
      }
    }

    public CommonDefine.ItemIDDefines ItemIDDefine
    {
      get
      {
        return this._itemIDDefine;
      }
    }

    public ItemIconIDDefine ItemIconIDDefine
    {
      get
      {
        return this._itemIconIDDefine;
      }
    }

    public Dictionary<int, Dictionary<int, int>> SearchItemGradeTable
    {
      get
      {
        return this._searchItemGradeTable;
      }
    }

    public CommonDefine.ItemAnimGroup ItemAnims
    {
      get
      {
        return this._itemAnims;
      }
    }

    public Dictionary<int, string[]> AppearCameraInStates
    {
      get
      {
        return this._appearCameraInStates;
      }
    }

    public Dictionary<int, string[]> OpeningWakeUpCameraInStates
    {
      get
      {
        return this._openingWakeUpCameraInStates;
      }
    }

    public RarelityProbability ProbRarelityProfile
    {
      get
      {
        return this._probRarelityProfile;
      }
    }

    public WeatherProbability ProbWeatherProfile
    {
      get
      {
        return this._probWeatherProfile;
      }
    }

    public SearchAreaProbabilities ProbSearchAreaProfile
    {
      get
      {
        return this._probSearchAreaProfile;
      }
    }

    public CommonDefine.TutorialSetting Tutorial
    {
      get
      {
        return this._tutorial;
      }
    }

    public CommonDefine.EventStoryInfoGroup EventStoryInfo
    {
      get
      {
        return this._eventStoryInfo;
      }
    }

    [Serializable]
    public class FileNamesGroup
    {
      [SerializeField]
      private string _mainCameraName = string.Empty;
      [SerializeField]
      private string _normalCameraName = string.Empty;
      [SerializeField]
      private string _actionCameraFreeLookName = string.Empty;
      [SerializeField]
      private string _actionCameraNotMoveName = string.Empty;
      [SerializeField]
      private string _fishingCamera = string.Empty;
      [SerializeField]
      private string _trialCamera = string.Empty;

      public string MainCameraName
      {
        get
        {
          return this._mainCameraName;
        }
      }

      public string NormalCameraName
      {
        get
        {
          return this._normalCameraName;
        }
      }

      public string ActionCameraFreeLookName
      {
        get
        {
          return this._actionCameraFreeLookName;
        }
      }

      public string ActionCameraNotMoveName
      {
        get
        {
          return this._actionCameraNotMoveName;
        }
      }

      public string FishingCamera
      {
        get
        {
          return this._fishingCamera;
        }
      }

      public string TrialCamera
      {
        get
        {
          return this._trialCamera;
        }
      }
    }

    [Serializable]
    public class CommonIconGroup
    {
      [SerializeField]
      private CommandTargetSpriteInfo _actionSpriteInfo = new CommandTargetSpriteInfo();
      [SerializeField]
      private CommandTargetSpriteInfo _charaSpriteInfo = new CommandTargetSpriteInfo();
      [SerializeField]
      private Sprite _actionSprite;
      [SerializeField]
      private Sprite _actionSelectedSprite;
      [SerializeField]
      private Sprite _charaSprite;
      [SerializeField]
      private Sprite _charaSelectedSprite;
      [SerializeField]
      private Texture2D _touchCursorTexture;
      [SerializeField]
      private int _baseIconID;
      [SerializeField]
      private int _deviceIconID;
      [SerializeField]
      private int _farmIconID;
      [SerializeField]
      private int _chickenCoopIconID;
      [SerializeField]
      private int _wellIconID;
      [SerializeField]
      private int _craftIconID;
      [SerializeField]
      private int _medicineCraftIconID;
      [SerializeField]
      private int _petCraftIconID;
      [SerializeField]
      private int _recyclingCraftIcon;
      [SerializeField]
      private int _fishTankIconID;
      [SerializeField]
      private int _jukeBoxIconID;
      [SerializeField]
      private int _eventIconID;
      [SerializeField]
      private int _storyIconID;
      [SerializeField]
      private int _guideOKID;
      [SerializeField]
      private int _guideCancelID;
      [SerializeField]
      private int _guideScrollID;
      [SerializeField]
      private int _shipIconID;

      public CommandTargetSpriteInfo ActionSpriteInfo
      {
        get
        {
          return this._actionSpriteInfo;
        }
      }

      public CommandTargetSpriteInfo CharaSpriteInfo
      {
        get
        {
          return this._charaSpriteInfo;
        }
      }

      public Sprite ActionSprite
      {
        get
        {
          return this._actionSprite;
        }
      }

      public Sprite ActionSelectedSprite
      {
        get
        {
          return this._actionSelectedSprite;
        }
      }

      public Sprite CharaSprite
      {
        get
        {
          return this._charaSprite;
        }
      }

      public Sprite CharaSelectedSprite
      {
        get
        {
          return this._charaSelectedSprite;
        }
      }

      public Texture2D TouchCursorTexture
      {
        get
        {
          return this._touchCursorTexture;
        }
      }

      public int BaseIconID
      {
        get
        {
          return this._baseIconID;
        }
      }

      public int DeviceIconID
      {
        get
        {
          return this._deviceIconID;
        }
      }

      public int FarmIconID
      {
        get
        {
          return this._farmIconID;
        }
      }

      public int ChickenCoopIconID
      {
        get
        {
          return this._chickenCoopIconID;
        }
      }

      public int WellIconID
      {
        get
        {
          return this._wellIconID;
        }
      }

      public int CraftIconID
      {
        get
        {
          return this._craftIconID;
        }
      }

      public int MedicineCraftIconID
      {
        get
        {
          return this._medicineCraftIconID;
        }
      }

      public int PetCraftIcon
      {
        get
        {
          return this._petCraftIconID;
        }
      }

      public int RecyclingCraftIcon
      {
        get
        {
          return this._recyclingCraftIcon;
        }
      }

      public int FishTankIconID
      {
        get
        {
          return this._fishTankIconID;
        }
      }

      public int JukeBoxIconID
      {
        get
        {
          return this._jukeBoxIconID;
        }
      }

      public int EventIconID
      {
        get
        {
          return this._eventIconID;
        }
      }

      public int StoryIconID
      {
        get
        {
          return this._storyIconID;
        }
      }

      public int GuideOKID
      {
        get
        {
          return this._guideOKID;
        }
      }

      public int GuideCancelID
      {
        get
        {
          return this._guideCancelID;
        }
      }

      public int GuideScrollID
      {
        get
        {
          return this._guideScrollID;
        }
      }

      public int ShipIconID
      {
        get
        {
          return this._shipIconID;
        }
      }
    }

    [Serializable]
    public class ItemAnimGroup
    {
      [SerializeField]
      private string _chestDefaultState = string.Empty;
      [SerializeField]
      private string[] _chestInStates = new string[1]
      {
        string.Empty
      };
      [SerializeField]
      private string[] _chestOutStates = new string[1]
      {
        string.Empty
      };
      [SerializeField]
      private string _doorDefaultState = string.Empty;
      [SerializeField]
      private string _doorCloseRight = string.Empty;
      [SerializeField]
      private string _doorCloseLeft = string.Empty;
      [SerializeField]
      private string _doorCloseLoopState = string.Empty;
      [SerializeField]
      private string _doorOpenIdleRight = string.Empty;
      [SerializeField]
      private string _doorOpenIdleLeft = string.Empty;
      [SerializeField]
      private string[] _podInStates = new string[1]
      {
        string.Empty
      };
      [SerializeField]
      private string[] _podOutStates = new string[1]
      {
        string.Empty
      };
      [SerializeField]
      private int _chestAnimatorID;
      [SerializeField]
      private int _doorAnimatorID;
      [SerializeField]
      private int _podAnimatorID;
      [SerializeField]
      private int _appearCameraAnimatorID;
      [SerializeField]
      private int _openingWakeUpCameraAnimatorID;

      public int ChestAnimatorID
      {
        get
        {
          return this._chestAnimatorID;
        }
      }

      public string ChestDefaultState
      {
        get
        {
          return this._chestDefaultState;
        }
      }

      public string[] ChestInStates
      {
        get
        {
          return this._chestInStates;
        }
      }

      public string[] ChestOutStates
      {
        get
        {
          return this._chestOutStates;
        }
      }

      public int DoorAnimatorID
      {
        get
        {
          return this._doorAnimatorID;
        }
      }

      public string DoorDefaultState
      {
        get
        {
          return this._doorDefaultState;
        }
      }

      public string DoorCloseRight
      {
        get
        {
          return this._doorCloseRight;
        }
      }

      public string DoorCloseLeft
      {
        get
        {
          return this._doorCloseLeft;
        }
      }

      public string DoorCloseLoopState
      {
        get
        {
          return this._doorCloseLoopState;
        }
      }

      public string DoorOpenIdleRight
      {
        get
        {
          return this._doorOpenIdleRight;
        }
      }

      public string DoorOpenIdleLeft
      {
        get
        {
          return this._doorOpenIdleLeft;
        }
      }

      public int PodAnimatorID
      {
        get
        {
          return this._podAnimatorID;
        }
      }

      public string[] PodInStates
      {
        get
        {
          return this._podInStates;
        }
      }

      public string[] PodOutStates
      {
        get
        {
          return this._podOutStates;
        }
      }

      public int AppearCameraAnimatorID
      {
        get
        {
          return this._appearCameraAnimatorID;
        }
      }

      public int OpeningWakeUpCameraAnimatorID
      {
        get
        {
          return this._openingWakeUpCameraAnimatorID;
        }
      }
    }

    [Serializable]
    public class ItemIDDefines
    {
      [SerializeField]
      private ItemIDKeyPair _rareGloveID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _sRareGloveID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _netID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _rareNetID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _sRareNetID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _shovelID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _rareShovelID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _sRareShovelID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _pickelID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _rarePickelID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _sRarePickelID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _rodID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _umbrellaID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _handLampID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _torchID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _maleLampID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _flashlightID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _driftwoodID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _shansKeyID = new ItemIDKeyPair();
      [SerializeField]
      private ItemIDKeyPair _gauzeID = new ItemIDKeyPair();
      [SerializeField]
      private int _toolCategoryID;
      [SerializeField]
      private int _equipmentCategoryID;
      [SerializeField]
      private int _normalSkillCategoryID;
      [SerializeField]
      private int _hSkillCategoryID;
      [SerializeField]
      private int[] _extendItemIDs;
      [SerializeField]
      private int[] _gloveItemIDs;
      [SerializeField]
      private int[] _netItemIDs;
      [SerializeField]
      private int[] _pickelItemIDs;
      [SerializeField]
      private int[] _shovelItemIDs;
      [SerializeField]
      private int[] _rodItemIDs;
      [SerializeField]
      private int[] _headItemIDs;
      [SerializeField]
      private int[] _backItemIDs;
      [SerializeField]
      private int[] _neckItemIDs;
      [SerializeField]
      private int[] _playerLightItemIDs;
      [SerializeField]
      private int[] _femaleLightItemIDs;

      public ItemIDKeyPair RareGloveID
      {
        get
        {
          return this._rareGloveID;
        }
      }

      public ItemIDKeyPair SRareGloveID
      {
        get
        {
          return this._sRareGloveID;
        }
      }

      public ItemIDKeyPair NetID
      {
        get
        {
          return this._netID;
        }
      }

      public ItemIDKeyPair RareNetID
      {
        get
        {
          return this._rareNetID;
        }
      }

      public ItemIDKeyPair SRareNetID
      {
        get
        {
          return this._sRareNetID;
        }
      }

      public ItemIDKeyPair ShovelID
      {
        get
        {
          return this._shovelID;
        }
      }

      public ItemIDKeyPair RareShovelID
      {
        get
        {
          return this._rareShovelID;
        }
      }

      public ItemIDKeyPair SRareShovelID
      {
        get
        {
          return this._sRareShovelID;
        }
      }

      public ItemIDKeyPair PickelID
      {
        get
        {
          return this._pickelID;
        }
      }

      public ItemIDKeyPair RarePickelID
      {
        get
        {
          return this._rarePickelID;
        }
      }

      public ItemIDKeyPair SRarePickelID
      {
        get
        {
          return this._sRarePickelID;
        }
      }

      public ItemIDKeyPair RodID
      {
        get
        {
          return this._rodID;
        }
      }

      public ItemIDKeyPair UmbrellaID
      {
        get
        {
          return this._umbrellaID;
        }
      }

      public ItemIDKeyPair HandLampID
      {
        get
        {
          return this._handLampID;
        }
      }

      public ItemIDKeyPair TorchID
      {
        get
        {
          return this._torchID;
        }
      }

      public ItemIDKeyPair MaleLampID
      {
        get
        {
          return this._maleLampID;
        }
      }

      public ItemIDKeyPair FlashlightID
      {
        get
        {
          return this._flashlightID;
        }
      }

      public ItemIDKeyPair DriftwoodID
      {
        get
        {
          return this._driftwoodID;
        }
      }

      public ItemIDKeyPair ShansKeyID
      {
        get
        {
          return this._shansKeyID;
        }
      }

      public ItemIDKeyPair GauzeID
      {
        get
        {
          return this._gauzeID;
        }
      }

      public bool ContainsLightItem(StuffItem item)
      {
        return this._torchID.categoryID == item.CategoryID && this._torchID.itemID == item.ID || this._maleLampID.categoryID == item.CategoryID && this._maleLampID.itemID == item.ID || this._flashlightID.categoryID == item.CategoryID && this._flashlightID.itemID == item.ID;
      }

      public int ToolCategoryID
      {
        get
        {
          return this._toolCategoryID;
        }
      }

      public int EquipmentCategoryID
      {
        get
        {
          return this._equipmentCategoryID;
        }
      }

      public int NormalSkillCategoryID
      {
        get
        {
          return this._normalSkillCategoryID;
        }
      }

      public int HSkillCategoryID
      {
        get
        {
          return this._hSkillCategoryID;
        }
      }

      public int[] ExtendItemIDs
      {
        get
        {
          return this._extendItemIDs;
        }
      }

      public int[] GloveItemIDs
      {
        get
        {
          return this._gloveItemIDs;
        }
      }

      public int[] NetItemIDs
      {
        get
        {
          return this._netItemIDs;
        }
      }

      public int[] PickelItemIDs
      {
        get
        {
          return this._pickelItemIDs;
        }
      }

      public int[] ShovelItemlIDs
      {
        get
        {
          return this._shovelItemIDs;
        }
      }

      public int[] RodItemIDs
      {
        get
        {
          return this._rodItemIDs;
        }
      }

      public int[] HeadItemIDs
      {
        get
        {
          return this._headItemIDs;
        }
      }

      public int[] BackItemIDs
      {
        get
        {
          return this._backItemIDs;
        }
      }

      public int[] NeckItemIDs
      {
        get
        {
          return this._neckItemIDs;
        }
      }

      public int[] PlayerLightItemIDs
      {
        get
        {
          return this._playerLightItemIDs;
        }
      }

      public int[] FemaleLightItemIDs
      {
        get
        {
          return this._femaleLightItemIDs;
        }
      }
    }

    [Serializable]
    public class TutorialSetting
    {
      [SerializeField]
      private float _openingWakeUpFadeTime = 1f;
      [SerializeField]
      private float _followGirlWaitTime = 10f;
      [SerializeField]
      private float _walkToRunTime = 1f;
      [SerializeField]
      private float _openingWakeUpStartFadeTime;
      [SerializeField]
      private float _uiDisplayDelayTime;
      [SerializeField]
      private int[] _kitchenPointIDList;
      [SerializeField]
      private int _yotunbaiRegisterID;

      public float OpeningWakeUpStartFadeTime
      {
        get
        {
          return this._openingWakeUpStartFadeTime;
        }
      }

      public float OpeningWakeUpFadeTime
      {
        get
        {
          return this._openingWakeUpFadeTime;
        }
      }

      public float FollowGirlWaitTime
      {
        get
        {
          return this._followGirlWaitTime;
        }
      }

      public float UIDisplayDelayTime
      {
        get
        {
          return this._uiDisplayDelayTime;
        }
      }

      public float WalkToRunTime
      {
        get
        {
          return this._walkToRunTime;
        }
      }

      public int[] KitchenPointIDList
      {
        get
        {
          return this._kitchenPointIDList;
        }
      }

      public int YotunbaiRegisterID
      {
        get
        {
          return this._yotunbaiRegisterID;
        }
      }
    }

    [Serializable]
    public class EventStoryInfoGroup
    {
      [SerializeField]
      private float _startADVFadeTime;
      [SerializeField]
      private float _endADVFadeTime;
      [SerializeField]
      private float _startEventFadeTime;
      [SerializeField]
      private float _endEventFadeTime;
      [SerializeField]
      private CommonDefine.EventStoryInfoGroup.EventSoundInfo _junkRoad;
      [SerializeField]
      private CommonDefine.EventStoryInfoGroup.EventSoundInfo _fenceDoorInfo;
      [SerializeField]
      private CommonDefine.EventStoryInfoGroup.EventSoundInfo _generatorInfo;
      [SerializeField]
      private CommonDefine.EventStoryInfoGroup.EventSoundInfo _shipRepair;
      [SerializeField]
      private CommonDefine.EventStoryInfoGroup.EventSoundInfo _automaticDoor;
      [SerializeField]
      private CommonDefine.EventStoryInfoGroup.EventSoundInfo _podDevice;
      [SerializeField]
      private float _storyCompleteNextSupportChangeTime;

      public float StartADVFadeTime
      {
        get
        {
          return this._startADVFadeTime;
        }
      }

      public float EndADVFadeTime
      {
        get
        {
          return this._endADVFadeTime;
        }
      }

      public float StartEventFadeTime
      {
        get
        {
          return this._startEventFadeTime;
        }
      }

      public float EndEventFadeTime
      {
        get
        {
          return this._endEventFadeTime;
        }
      }

      public CommonDefine.EventStoryInfoGroup.EventSoundInfo JunkRoad
      {
        get
        {
          return this._junkRoad;
        }
      }

      public CommonDefine.EventStoryInfoGroup.EventSoundInfo FenceDoor
      {
        get
        {
          return this._fenceDoorInfo;
        }
      }

      public CommonDefine.EventStoryInfoGroup.EventSoundInfo Generator
      {
        get
        {
          return this._generatorInfo;
        }
      }

      public CommonDefine.EventStoryInfoGroup.EventSoundInfo ShipRepair
      {
        get
        {
          return this._shipRepair;
        }
      }

      public CommonDefine.EventStoryInfoGroup.EventSoundInfo AutomaticDoor
      {
        get
        {
          return this._automaticDoor;
        }
      }

      public CommonDefine.EventStoryInfoGroup.EventSoundInfo PodDevice
      {
        get
        {
          return this._podDevice;
        }
      }

      public float StoryCompleteNextSupportChangeTime
      {
        get
        {
          return this._storyCompleteNextSupportChangeTime;
        }
      }

      [Serializable]
      public class EventSoundInfo
      {
        [SerializeField]
        private int _seID;
        [SerializeField]
        private float _soundPlayDelayTime;
        [SerializeField]
        private float _endIntervalTime;

        public int SEID
        {
          get
          {
            return this._seID;
          }
        }

        public float SoundPlayDelayTime
        {
          get
          {
            return this._soundPlayDelayTime;
          }
        }

        public float EndIntervalTime
        {
          get
          {
            return this._endIntervalTime;
          }
        }
      }
    }
  }
}
