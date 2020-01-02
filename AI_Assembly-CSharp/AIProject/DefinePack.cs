// Decompiled with JetBrains decompiler
// Type: AIProject.DefinePack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace AIProject
{
  public class DefinePack : SerializedScriptableObject
  {
    [SerializeField]
    private DefinePack.AssetBundleManifestsDefine _abManifests;
    [SerializeField]
    private DefinePack.AssetBundleDirectoriesDefine _abDirectories;
    [SerializeField]
    private DefinePack.AssetBundlePathsDefine _abPaths;
    [SerializeField]
    [Header("Scene Name")]
    private DefinePack.SceneNameGroup _sceneNames;
    [SerializeField]
    [Header("Animator State")]
    private DefinePack.AnimatorStateNameGroup _animatorState;
    [SerializeField]
    [Header("Animator Parameter")]
    private DefinePack.BasicAnimatorParameter _animatorParameter;
    [SerializeField]
    [Header("Map")]
    private DefinePack.MapGroup _mapDefines;
    [SerializeField]
    [Header("ItemBoxCapacity")]
    private DefinePack.ItemBoxCapacity _itemBoxCapacityDefines;
    [SerializeField]
    [Header("MinimapUIDefine")]
    private DefinePack.MinimapUI _minimapUIDefine;
    [SerializeField]
    [Header("MapLesbianDefine")]
    private DefinePack.MapLes _MapLes;
    [SerializeField]
    [Header("RecyclingSetting")]
    private DefinePack.RecyclingSetting _recycling;

    public DefinePack()
    {
      base.\u002Ector();
    }

    public DefinePack.AssetBundleManifestsDefine ABManifests
    {
      get
      {
        return this._abManifests;
      }
    }

    public DefinePack.AssetBundleDirectoriesDefine ABDirectories
    {
      get
      {
        return this._abDirectories;
      }
    }

    public DefinePack.AssetBundlePathsDefine ABPaths
    {
      get
      {
        return this._abPaths;
      }
    }

    public DefinePack.SceneNameGroup SceneNames
    {
      get
      {
        return this._sceneNames;
      }
    }

    public DefinePack.AnimatorStateNameGroup AnimatorState
    {
      get
      {
        return this._animatorState;
      }
    }

    public DefinePack.BasicAnimatorParameter AnimatorParameter
    {
      get
      {
        return this._animatorParameter;
      }
    }

    public DefinePack.MapGroup MapDefines
    {
      get
      {
        return this._mapDefines;
      }
    }

    public DefinePack.ItemBoxCapacity ItemBoxCapacityDefines
    {
      get
      {
        return this._itemBoxCapacityDefines;
      }
    }

    public DefinePack.MinimapUI MinimapUIDefine
    {
      get
      {
        return this._minimapUIDefine;
      }
    }

    public DefinePack.MapLes MapLesDefine
    {
      get
      {
        return this._MapLes;
      }
    }

    public DefinePack.RecyclingSetting Recycling
    {
      get
      {
        return this._recycling;
      }
    }

    [Serializable]
    public class AssetBundleManifestsDefine
    {
      [SerializeField]
      private string _default = string.Empty;
      [SerializeField]
      private string _add05 = string.Empty;
      [SerializeField]
      private string _add07 = string.Empty;
      [SerializeField]
      private string _add12 = string.Empty;

      public string Default
      {
        get
        {
          return this._default;
        }
      }

      public string Add05
      {
        get
        {
          return this._add05;
        }
      }

      public string Add07
      {
        get
        {
          return this._add07;
        }
      }

      public string Add12
      {
        get
        {
          return this._add12;
        }
      }
    }

    [Serializable]
    public class AssetBundleDirectoriesDefine
    {
      [SerializeField]
      private string _inputIconList = string.Empty;
      [SerializeField]
      private string _actionIconList = string.Empty;
      [SerializeField]
      private string _actorIconList = string.Empty;
      [SerializeField]
      private string _weatherIconList = string.Empty;
      [SerializeField]
      private string _equipItemIconList = string.Empty;
      [SerializeField]
      private string _statusIconList = string.Empty;
      [SerializeField]
      private string _sickIconList = string.Empty;
      [SerializeField]
      private string _searchEquipItemObjList = string.Empty;
      [SerializeField]
      private string _commonEquipItemObjList = string.Empty;
      [SerializeField]
      private string _accessoryItem = string.Empty;
      [SerializeField]
      private string _actionPointPrefabList = string.Empty;
      [SerializeField]
      private string _basePointPrefabList = string.Empty;
      [SerializeField]
      private string _devicePointPrefabList = string.Empty;
      [SerializeField]
      private string _farmPointPrefabList = string.Empty;
      [SerializeField]
      private string _shipPointPrefabList = string.Empty;
      [SerializeField]
      private string _playerActionPointList = string.Empty;
      [SerializeField]
      private string _agentActionPointList = string.Empty;
      [SerializeField]
      private string _merchantActionPointList = string.Empty;
      [SerializeField]
      private string _lightSwitchPointList = string.Empty;
      [SerializeField]
      private string _eventPointList = string.Empty;
      [SerializeField]
      private string _storyPointList = string.Empty;
      [SerializeField]
      private string _mapList = string.Empty;
      [SerializeField]
      private string _chunkList = string.Empty;
      [SerializeField]
      private string _waypointList = string.Empty;
      [SerializeField]
      private string _AreaGroupList = string.Empty;
      [SerializeField]
      private string _plantItemList = string.Empty;
      [SerializeField]
      private string _ivyFilterList = string.Empty;
      [SerializeField]
      private string _eventItemList = string.Empty;
      [SerializeField]
      private string _eventParticleList = string.Empty;
      [SerializeField]
      private string _popupInfoList = string.Empty;
      [SerializeField]
      private string _areaOpenStateList = string.Empty;
      [SerializeField]
      private string _timeRelationInfoList = string.Empty;
      [SerializeField]
      private string _foodInfo = string.Empty;
      [SerializeField]
      private string _drinkInfo = string.Empty;
      [SerializeField]
      private string _itemList = string.Empty;
      [SerializeField]
      private string _gatheringTable = string.Empty;
      [SerializeField]
      private string _frogItemTable = string.Empty;
      [SerializeField]
      private string _comCamera = string.Empty;
      [SerializeField]
      private string _mapActionVoiceInfoList = string.Empty;
      [SerializeField]
      private string _defaultFemaleFootStepSEInfoList = string.Empty;
      [SerializeField]
      private string _defaultMaleFootStepSEInfoList = string.Empty;
      [SerializeField]
      private string _mapBGMInfoList = string.Empty;
      [SerializeField]
      private string _enviroSEInfoList = string.Empty;
      [SerializeField]
      private string _actorAnimatorList = string.Empty;
      [SerializeField]
      private string _playerFemaleAnimeInfo = string.Empty;
      [SerializeField]
      private string _playerMaleAnimeInfo = string.Empty;
      [SerializeField]
      private string _agentPhase = string.Empty;
      [SerializeField]
      private string _agentPersonalityMotivation = string.Empty;
      [SerializeField]
      private string _lifestyleTable = string.Empty;
      [SerializeField]
      private string _flavorPickSkillTable = string.Empty;
      [SerializeField]
      private string _flavorPickHSkillTable = string.Empty;
      [SerializeField]
      private string _agentDesire = string.Empty;
      [SerializeField]
      private string _agentCommunicationFlags = string.Empty;
      [SerializeField]
      private string _agentAnimeInfo = string.Empty;
      [SerializeField]
      private string _agentLocomotionBreath = string.Empty;
      [SerializeField]
      private string _gravurePoseInfo = string.Empty;
      [SerializeField]
      private string _surpriseItemInfo = string.Empty;
      [SerializeField]
      private string _agentActionResult = string.Empty;
      [SerializeField]
      private string _agentSituationResult = string.Empty;
      [SerializeField]
      private string _actorVanishList = string.Empty;
      [SerializeField]
      private string _behaviorTree = string.Empty;
      [SerializeField]
      private string _tutrialBehaviorTree = string.Empty;
      [SerializeField]
      private string _merchantAnimeInfo = string.Empty;
      [SerializeField]
      private string _merchantBehaviorTree = string.Empty;
      [SerializeField]
      private string _mapIKList = string.Empty;
      [SerializeField]
      private string _minimapIconNameList = string.Empty;
      [SerializeField]
      private string _vanishCameraList = string.Empty;
      [SerializeField]
      private string _expList = string.Empty;
      [SerializeField]
      private string _actionExpList = string.Empty;
      [SerializeField]
      private string _actionExpKeyFrameList = string.Empty;
      [SerializeField]
      private string _actionBustCtrlList = string.Empty;
      [SerializeField]
      private string _actionCameraData = string.Empty;
      [SerializeField]
      private string _actionByproductList = string.Empty;
      [SerializeField]
      private string _enviroInfoList = string.Empty;
      [SerializeField]
      private string _recyclingInfoList = string.Empty;
      [SerializeField]
      private string _loadingSpriteList;

      public string InputIconList
      {
        get
        {
          return this._inputIconList;
        }
      }

      public string ActionIconList
      {
        get
        {
          return this._actionIconList;
        }
      }

      public string ActorIconList
      {
        get
        {
          return this._actorIconList;
        }
      }

      public string WeatherIconList
      {
        get
        {
          return this._weatherIconList;
        }
      }

      public string EquipItemIconList
      {
        get
        {
          return this._equipItemIconList;
        }
      }

      public string StatusIconList
      {
        get
        {
          return this._statusIconList;
        }
      }

      public string SickIconList
      {
        get
        {
          return this._sickIconList;
        }
      }

      public string LoadingSpriteList
      {
        get
        {
          return this._loadingSpriteList;
        }
      }

      public string SearchEquipItemObjList
      {
        get
        {
          return this._searchEquipItemObjList;
        }
      }

      public string CommonEquipItemObjList
      {
        get
        {
          return this._commonEquipItemObjList;
        }
      }

      public string AccessoryItem
      {
        get
        {
          return this._accessoryItem;
        }
      }

      public string ActionPointPrefabList
      {
        get
        {
          return this._actionPointPrefabList;
        }
      }

      public string BasePointPrefabList
      {
        get
        {
          return this._basePointPrefabList;
        }
      }

      public string DevicePointPrefabList
      {
        get
        {
          return this._devicePointPrefabList;
        }
      }

      public string FarmPointPrefabList
      {
        get
        {
          return this._farmPointPrefabList;
        }
      }

      public string ShipPointPrefabList
      {
        get
        {
          return this._shipPointPrefabList;
        }
      }

      public string PlayerActionPointList
      {
        get
        {
          return this._playerActionPointList;
        }
      }

      public string AgentActionPointList
      {
        get
        {
          return this._agentActionPointList;
        }
      }

      public string MerchantActionPointList
      {
        get
        {
          return this._merchantActionPointList;
        }
      }

      public string LightSwitchPointList
      {
        get
        {
          return this._lightSwitchPointList;
        }
      }

      public string EventPointList
      {
        get
        {
          return this._eventPointList;
        }
      }

      public string StoryPointList
      {
        get
        {
          return this._storyPointList;
        }
      }

      public string MapList
      {
        get
        {
          return this._mapList;
        }
      }

      public string ChunkList
      {
        get
        {
          return this._chunkList;
        }
      }

      public string WaypointList
      {
        get
        {
          return this._waypointList;
        }
      }

      public string AreaGroupList
      {
        get
        {
          return this._AreaGroupList;
        }
      }

      public string PlantItemList
      {
        get
        {
          return this._plantItemList;
        }
      }

      public string IvyFilterList
      {
        get
        {
          return this._ivyFilterList;
        }
      }

      public string EventItemList
      {
        get
        {
          return this._eventItemList;
        }
      }

      public string EventParticleList
      {
        get
        {
          return this._eventParticleList;
        }
      }

      public string PopupInfoList
      {
        get
        {
          return this._popupInfoList;
        }
      }

      public string AreaOpenStateList
      {
        get
        {
          return this._areaOpenStateList;
        }
      }

      public string TimeRelationInfoList
      {
        get
        {
          return this._timeRelationInfoList;
        }
      }

      public string FoodInfo
      {
        get
        {
          return this._foodInfo;
        }
      }

      public string DrinkInfo
      {
        get
        {
          return this._drinkInfo;
        }
      }

      public string ItemList
      {
        get
        {
          return this._itemList;
        }
      }

      public string GatheringTable
      {
        get
        {
          return this._gatheringTable;
        }
      }

      public string FrogItemTable
      {
        get
        {
          return this._frogItemTable;
        }
      }

      public string ComCamera
      {
        get
        {
          return this._comCamera;
        }
      }

      public string MapActionVoiceInfoList
      {
        get
        {
          return this._mapActionVoiceInfoList;
        }
      }

      public string DefaultFemaleFootStepSEInfoList
      {
        get
        {
          return this._defaultFemaleFootStepSEInfoList;
        }
      }

      public string DefaultMaleFootStepSEInfoList
      {
        get
        {
          return this._defaultMaleFootStepSEInfoList;
        }
      }

      public string MapBGMInfoList
      {
        get
        {
          return this._mapBGMInfoList;
        }
      }

      public string EnviroSEInfoList
      {
        get
        {
          return this._enviroSEInfoList;
        }
      }

      public string ActorAnimatorList
      {
        get
        {
          return this._actorAnimatorList;
        }
      }

      public string PlayerFemaleAnimeInfo
      {
        get
        {
          return this._playerFemaleAnimeInfo;
        }
      }

      public string PlayerMaleAnimeInfo
      {
        get
        {
          return this._playerMaleAnimeInfo;
        }
      }

      public string AgentPhase
      {
        get
        {
          return this._agentPhase;
        }
      }

      public string AgentPersonalityMotivation
      {
        get
        {
          return this._agentPersonalityMotivation;
        }
      }

      public string LifestyleTable
      {
        get
        {
          return this._lifestyleTable;
        }
      }

      public string FlavorPickSkillTable
      {
        get
        {
          return this._flavorPickSkillTable;
        }
      }

      public string FlavorPickHSkillTable
      {
        get
        {
          return this._flavorPickHSkillTable;
        }
      }

      public string AgentDesire
      {
        get
        {
          return this._agentDesire;
        }
      }

      public string AgentCommunicationFlags
      {
        get
        {
          return this._agentCommunicationFlags;
        }
      }

      public string AgentAnimeInfo
      {
        get
        {
          return this._agentAnimeInfo;
        }
      }

      public string AgentLocomotionBreath
      {
        get
        {
          return this._agentLocomotionBreath;
        }
      }

      public string GravurePoseInfo
      {
        get
        {
          return this._gravurePoseInfo;
        }
      }

      public string SurpriseItemInfo
      {
        get
        {
          return this._surpriseItemInfo;
        }
      }

      public string AgentActionResult
      {
        get
        {
          return this._agentActionResult;
        }
      }

      public string AgentSituationResult
      {
        get
        {
          return this._agentSituationResult;
        }
      }

      public string ActorVanishList
      {
        get
        {
          return this._actorVanishList;
        }
      }

      public string BehaviorTree
      {
        get
        {
          return this._behaviorTree;
        }
      }

      public string TutorialBehaviorTree
      {
        get
        {
          return this._tutrialBehaviorTree;
        }
      }

      public string MerchantAnimeInfo
      {
        get
        {
          return this._merchantAnimeInfo;
        }
      }

      public string MerchantBehaviorTree
      {
        get
        {
          return this._merchantBehaviorTree;
        }
      }

      public string MapIKList
      {
        get
        {
          return this._mapIKList;
        }
      }

      public string MinimapIconNameList
      {
        get
        {
          return this._minimapIconNameList;
        }
      }

      public string VanishCameraList
      {
        get
        {
          return this._vanishCameraList;
        }
      }

      public string ExpList
      {
        get
        {
          return this._expList;
        }
      }

      public string ActionExpList
      {
        get
        {
          return this._actionExpList;
        }
      }

      public string ActionExpKeyFrameList
      {
        get
        {
          return this._actionExpKeyFrameList;
        }
      }

      public string ActionBustCtrlList
      {
        get
        {
          return this._actionBustCtrlList;
        }
      }

      public string ActionCameraData
      {
        get
        {
          return this._actionCameraData;
        }
      }

      public string ActionByproductList
      {
        get
        {
          return this._actionByproductList;
        }
      }

      public string EnviroInfoList
      {
        get
        {
          return this._enviroInfoList;
        }
      }

      public string RecyclingInfoList
      {
        get
        {
          return this._recyclingInfoList;
        }
      }
    }

    [Serializable]
    public class AssetBundlePathsDefine
    {
      [SerializeField]
      private string _mapScene = string.Empty;
      [SerializeField]
      private string _mapScenePrefab = string.Empty;
      [SerializeField]
      private string _mapScenePrefabAdd05 = string.Empty;
      [SerializeField]
      private string _mapScenePrefabAdd07 = string.Empty;
      [SerializeField]
      private string _mapScenePrefabAdd12 = string.Empty;
      [SerializeField]
      private string _mapDebug = string.Empty;
      [SerializeField]
      private string _actorPrefab = string.Empty;
      [SerializeField]
      private string _agent = string.Empty;
      [SerializeField]
      private string _camera = string.Empty;
      [SerializeField]
      private string _cameraAdd05 = string.Empty;

      public string MapScene
      {
        get
        {
          return this._mapScene;
        }
      }

      public string MapScenePrefab
      {
        get
        {
          return this._mapScenePrefab;
        }
      }

      public string MapScenePrefabAdd05
      {
        get
        {
          return this._mapScenePrefabAdd05;
        }
      }

      public string MapScenePrefabAdd07
      {
        get
        {
          return this._mapScenePrefabAdd07;
        }
      }

      public string MapScenePrefabAdd12
      {
        get
        {
          return this._mapScenePrefabAdd12;
        }
      }

      public string MapDebug
      {
        get
        {
          return this._mapDebug;
        }
      }

      public string ActorPrefab
      {
        get
        {
          return this._actorPrefab;
        }
      }

      public string Agent
      {
        get
        {
          return this._agent;
        }
      }

      public string Camera
      {
        get
        {
          return this._camera;
        }
      }

      public string CameraAdd05
      {
        get
        {
          return this._cameraAdd05;
        }
      }
    }

    [Serializable]
    public class SceneNameGroup
    {
      [SerializeField]
      private string _logoScene = string.Empty;
      [SerializeField]
      private string _titleScene = string.Empty;
      [SerializeField]
      private string _mapScene = string.Empty;
      [SerializeField]
      private string _mapUIScene = string.Empty;
      [SerializeField]
      private string _hScene = string.Empty;
      [SerializeField]
      private string _mapShortcutScene = string.Empty;
      [SerializeField]
      private string _configScene = string.Empty;
      [SerializeField]
      private string _dialogScene = string.Empty;
      [SerializeField]
      private string _exitScene = string.Empty;

      public string LogoScene
      {
        get
        {
          return this._logoScene;
        }
      }

      public string TitleScene
      {
        get
        {
          return this._titleScene;
        }
      }

      public string MapScene
      {
        get
        {
          return this._mapScene;
        }
      }

      public string MapUIScene
      {
        get
        {
          return this._mapUIScene;
        }
      }

      public string HScene
      {
        get
        {
          return this._hScene;
        }
      }

      public string MapShortcutScene
      {
        get
        {
          return this._mapShortcutScene;
        }
      }

      public string ConfigScene
      {
        get
        {
          return this._configScene;
        }
      }

      public string DialogScene
      {
        get
        {
          return this._dialogScene;
        }
      }

      public string ExitScene
      {
        get
        {
          return this._exitScene;
        }
      }
    }

    [Serializable]
    public struct PlayerDefine
    {
      public string walk;
      public string run;
    }

    [Serializable]
    public class AnimatorStateNameGroup
    {
      [SerializeField]
      private string _fastTurnAssetBundleName = string.Empty;
      [SerializeField]
      private string _fastTurnAnimatorName = string.Empty;
      [SerializeField]
      private string _idleState = string.Empty;
      [SerializeField]
      private string _housingAnimationDefault = string.Empty;
      [SerializeField]
      private string _merchantIdleState = string.Empty;
      [SerializeField]
      private PlayState.AnimStateInfo _idleStateInfo;
      [SerializeField]
      private PlayState.AnimStateInfo _turnStateInfo;
      [SerializeField]
      private PlayState.AnimStateInfo _fastTurnStateInfo;
      [SerializeField]
      private string _turnState;
      [SerializeField]
      private int _onbuStateID;

      public PlayState.AnimStateInfo IdleStateInfo
      {
        get
        {
          return this._idleStateInfo;
        }
      }

      public PlayState.AnimStateInfo TurnStateInfo
      {
        get
        {
          return this._turnStateInfo;
        }
      }

      public PlayState.AnimStateInfo FastTurnStateInfo
      {
        get
        {
          return this._fastTurnStateInfo;
        }
      }

      public string FastTurnAssetBundleName
      {
        get
        {
          return this._fastTurnAssetBundleName;
        }
      }

      public string FastTurnAnimatorName
      {
        get
        {
          return this._fastTurnAnimatorName;
        }
      }

      public string IdleState
      {
        get
        {
          return this._idleState;
        }
      }

      public string TurnState
      {
        get
        {
          return this._turnState;
        }
      }

      public string HousingAnimationDefault
      {
        get
        {
          return this._housingAnimationDefault;
        }
      }

      public int OnbuStateID
      {
        get
        {
          return this._onbuStateID;
        }
      }

      public string MerchantIdleState
      {
        get
        {
          return this._merchantIdleState;
        }
      }
    }

    [Serializable]
    public class BasicAnimatorParameter
    {
      [SerializeField]
      private string _forwardMove = string.Empty;
      [SerializeField]
      private string _heightParameterName = string.Empty;
      [SerializeField]
      private string _height1ParameterName = string.Empty;
      [SerializeField]
      private string _directionParameterName = string.Empty;
      [SerializeField]
      private string _speedParameterName = string.Empty;

      public string ForwardMove
      {
        get
        {
          return this._forwardMove;
        }
      }

      public string HeightParameterName
      {
        get
        {
          return this._heightParameterName;
        }
      }

      public string Height1ParameterName
      {
        get
        {
          return this._height1ParameterName;
        }
      }

      public string DirectionParameterName
      {
        get
        {
          return this._directionParameterName;
        }
      }

      public string SpeedParameterName
      {
        get
        {
          return this._speedParameterName;
        }
      }
    }

    [Serializable]
    public class MapGroup
    {
      [SerializeField]
      [Range(0.1f, 10f)]
      private float _worldSize = 1f;
      [SerializeField]
      private LayerMask _charaLayer = LayerMask.op_Implicit(0);
      [SerializeField]
      private LayerMask _mapLayer = LayerMask.op_Implicit(0);
      [SerializeField]
      private LayerMask _areaDetectionLayer = LayerMask.op_Implicit(0);
      [SerializeField]
      private LayerMask _roofLayer = LayerMask.op_Implicit(0);
      [SerializeField]
      private LayerMask _seLayer = LayerMask.op_Implicit(0);
      [SerializeField]
      private LayerMask _hLayer = LayerMask.op_Implicit(0);
      [SerializeField]
      private LayerMask _envLightCulMask = LayerMask.op_Implicit(-1);
      [SerializeField]
      private LayerMask _envLightCulMaskCustom = LayerMask.op_Implicit(-1);
      [SerializeField]
      private string _onbuMeshTag = string.Empty;
      [SerializeField]
      private int _itemSlotMax = 1;
      [SerializeField]
      private string _navMeshTargetName = string.Empty;
      [SerializeField]
      private string _commandTargetName = string.Empty;
      [SerializeField]
      private string _doorOpenCommandTargetName = string.Empty;
      [SerializeField]
      private string _doorCloseCommandTargetName = string.Empty;
      [SerializeField]
      private string _basePointLabelTargetName = string.Empty;
      [SerializeField]
      private string _basePointWarpTargetName = string.Empty;
      [SerializeField]
      private string _housingCenterTargetName = string.Empty;
      [SerializeField]
      private string _devicePointLabelTargetName = string.Empty;
      [SerializeField]
      private string _devicePointPivotTargetName = string.Empty;
      [SerializeField]
      private string[] _devicePointRecoveryTargetNames = new string[1]
      {
        string.Empty
      };
      [SerializeField]
      private string _devicePointPlayerRecoveryTargetName = string.Empty;
      [SerializeField]
      private string _farmPointLabelTargetName = string.Empty;
      [SerializeField]
      private string _craftPointLabelTargetName = string.Empty;
      [SerializeField]
      private string _eventPointLabelTargetName = string.Empty;
      [SerializeField]
      private string _petHomePointLabelTargetName = string.Empty;
      [SerializeField]
      private string _jukeBoxPointLabelTargetName = string.Empty;
      [SerializeField]
      private string _jukeBoxSoundRootTargetName = string.Empty;
      [SerializeField]
      private string _lightPointLabelTargetName = string.Empty;
      [SerializeField]
      private string _shipPointLabelTargetName = string.Empty;
      [SerializeField]
      private string _stealPivotName = string.Empty;
      [SerializeField]
      private int _shadowDistance = 400;
      [SerializeField]
      private int _agentDefaultNum;
      [SerializeField]
      private int _agentMax;
      [SerializeField]
      private string[] _floorTypeHMeshTag;
      [SerializeField]
      private string[] _lesTypeHMeshTag;
      [SerializeField]
      private string[] _merchantHMeshTag;
      [SerializeField]
      [EnumMask]
      private EventType _playerEventMask;
      [SerializeField]
      private int _defaultMotivation;
      [SerializeField]
      private int _itemStackUpperLimit;

      public float WorldSize
      {
        get
        {
          return this._worldSize;
        }
      }

      public int AgentDefaultNum
      {
        get
        {
          return this._agentDefaultNum;
        }
      }

      public int AgentMax
      {
        get
        {
          return this._agentMax;
        }
      }

      public LayerMask CharaLayer
      {
        get
        {
          return this._charaLayer;
        }
      }

      public LayerMask MapLayer
      {
        get
        {
          return this._mapLayer;
        }
      }

      public LayerMask AreaDetectionLayer
      {
        get
        {
          return this._areaDetectionLayer;
        }
      }

      public LayerMask RoofLayer
      {
        get
        {
          return this._roofLayer;
        }
      }

      public LayerMask SELayer
      {
        get
        {
          return this._seLayer;
        }
      }

      public LayerMask HLayer
      {
        get
        {
          return this._hLayer;
        }
      }

      public LayerMask EnvLightCulMask
      {
        get
        {
          return this._envLightCulMask;
        }
      }

      public LayerMask EnvLightCulMaskCustom
      {
        get
        {
          return this._envLightCulMaskCustom;
        }
      }

      public string OnbuMeshTag
      {
        get
        {
          return this._onbuMeshTag;
        }
      }

      public string[] FloorTypeHMeshTag
      {
        get
        {
          return this._floorTypeHMeshTag;
        }
      }

      public string[] LesTypeHMeshTag
      {
        get
        {
          return this._lesTypeHMeshTag;
        }
      }

      public string[] MerchantHMeshTag
      {
        get
        {
          return this._merchantHMeshTag;
        }
      }

      public EventType PlayerEventMask
      {
        get
        {
          return this._playerEventMask;
        }
      }

      public int DefaultMotivation
      {
        get
        {
          return this._defaultMotivation;
        }
      }

      public int ItemStackUpperLimit
      {
        get
        {
          return this._itemStackUpperLimit;
        }
      }

      public int ItemSlotMax
      {
        get
        {
          return this._itemSlotMax;
        }
      }

      public string NavMeshTargetName
      {
        get
        {
          return this._navMeshTargetName;
        }
      }

      public string CommandTargetName
      {
        get
        {
          return this._commandTargetName;
        }
      }

      public string DoorOpenCommandTargetName
      {
        get
        {
          return this._doorOpenCommandTargetName;
        }
      }

      public string DoorCloseCommandTargetName
      {
        get
        {
          return this._doorCloseCommandTargetName;
        }
      }

      public string BasePointLabelTargetName
      {
        get
        {
          return this._basePointLabelTargetName;
        }
      }

      public string BasePointWarpTargetName
      {
        get
        {
          return this._basePointWarpTargetName;
        }
      }

      public string HousingCenterTargetName
      {
        get
        {
          return this._housingCenterTargetName;
        }
      }

      public string DevicePointLabelTargetName
      {
        get
        {
          return this._devicePointLabelTargetName;
        }
      }

      public string DevicePointPivotTargetName
      {
        get
        {
          return this._devicePointPivotTargetName;
        }
      }

      public string[] DevicePointRecoveryTargetNames
      {
        get
        {
          return this._devicePointRecoveryTargetNames;
        }
      }

      public string DevicePointPlayerRecoveryTargetName
      {
        get
        {
          return this._devicePointPlayerRecoveryTargetName;
        }
      }

      public string FarmPointLabelTargetName
      {
        get
        {
          return this._farmPointLabelTargetName;
        }
      }

      public string CraftPointLabelTargetName
      {
        get
        {
          return this._craftPointLabelTargetName;
        }
      }

      public string EventPointLabelTargetName
      {
        get
        {
          return this._eventPointLabelTargetName;
        }
      }

      public string PetHomePointLabelTargetName
      {
        get
        {
          return this._petHomePointLabelTargetName;
        }
      }

      public string JukeBoxPointLabelTargetName
      {
        get
        {
          return this._jukeBoxPointLabelTargetName;
        }
      }

      public string JukeBoxSoundRootTargetName
      {
        get
        {
          return this._jukeBoxSoundRootTargetName;
        }
      }

      public string LightPointLabelTargetName
      {
        get
        {
          return this._lightPointLabelTargetName;
        }
      }

      public string ShipPointLabelTargetName
      {
        get
        {
          return this._shipPointLabelTargetName;
        }
      }

      public string StealPivotName
      {
        get
        {
          return this._stealPivotName;
        }
      }

      public int ShadowDistance
      {
        get
        {
          return this._shadowDistance;
        }
      }
    }

    [Serializable]
    public class ItemBoxCapacity
    {
      [SerializeField]
      private int _storageCapacity = 200;
      [SerializeField]
      private int _pantryCapacity = 200;

      public int StorageCapacity
      {
        get
        {
          return this._storageCapacity;
        }
      }

      public int PantryCapacity
      {
        get
        {
          return this._pantryCapacity;
        }
      }
    }

    [Serializable]
    public class MinimapUI
    {
      [SerializeField]
      private int _baseIconID = -1;
      [SerializeField]
      private int _deviceIconID = -1;
      [SerializeField]
      private int _farmIconID = -1;
      [SerializeField]
      private int _eventIconID = -1;
      [SerializeField]
      private int _chickenIconID = -1;
      [SerializeField]
      private int _dragDeskIconID = -1;
      [SerializeField]
      private int _petUnionIconID = -1;
      [SerializeField]
      private int _recycleIconID = -1;
      [SerializeField]
      private int _jukeIconID = -1;

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

      public int EventIconID
      {
        get
        {
          return this._eventIconID;
        }
      }

      public int ChickenIconID
      {
        get
        {
          return this._chickenIconID;
        }
      }

      public int DragDeskIconID
      {
        get
        {
          return this._dragDeskIconID;
        }
      }

      public int PetUnionIconID
      {
        get
        {
          return this._petUnionIconID;
        }
      }

      public int RecycleIconID
      {
        get
        {
          return this._recycleIconID;
        }
      }

      public int JukeIconID
      {
        get
        {
          return this._jukeIconID;
        }
      }
    }

    [Serializable]
    public class MapLes
    {
      [SerializeField]
      private float _loopChangeTime;
      [SerializeField]
      private string _motionParameterName;

      public float LoopChangeTime
      {
        get
        {
          return this._loopChangeTime;
        }
      }

      public string MotionParameterName
      {
        get
        {
          return this._motionParameterName;
        }
      }
    }

    [Serializable]
    public class RecyclingSetting
    {
      [SerializeField]
      private int _decidedItemCapacity = 100;
      [SerializeField]
      private int _createItemCapacity = 100;
      [SerializeField]
      private float _itemCreateTime = 10f;
      [SerializeField]
      private int _needNumber = 10;

      public int DecidedItemCapacity
      {
        get
        {
          return this._decidedItemCapacity;
        }
      }

      public int CreateItemCapacity
      {
        get
        {
          return this._createItemCapacity;
        }
      }

      public float ItemCreateTime
      {
        get
        {
          return this._itemCreateTime;
        }
      }

      public int NeedNumber
      {
        get
        {
          return this._needNumber;
        }
      }
    }
  }
}
