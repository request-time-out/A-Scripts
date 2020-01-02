// Decompiled with JetBrains decompiler
// Type: AIProject.FishingDefinePack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.MiniGames.Fishing;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class FishingDefinePack : SerializedScriptableObject
  {
    [SerializeField]
    private FishingDefinePack.AssetBundleNameGroup _assetBundleNames;
    [SerializeField]
    private FishingDefinePack.SystemParamGroup _systemParam;
    [SerializeField]
    private FishingDefinePack.IDGroup _idGroup;
    [SerializeField]
    private FishingDefinePack.LureParamGroup _lureParam;
    [SerializeField]
    private FishingDefinePack.FishParamGroup _fishParam;
    [SerializeField]
    private FishingDefinePack.UIParamGroup _uiParam;
    [SerializeField]
    private FishingDefinePack.PlayerParamGroup _playerParam;
    [SerializeField]
    private Dictionary<int, float> _getEfcScaleTable;
    [SerializeField]
    private Dictionary<SEType, int> _seTable;

    public FishingDefinePack()
    {
      base.\u002Ector();
    }

    public FishingDefinePack.AssetBundleNameGroup AssetBundleNames
    {
      get
      {
        return this._assetBundleNames;
      }
    }

    public FishingDefinePack.SystemParamGroup SystemParam
    {
      get
      {
        return this._systemParam;
      }
    }

    public FishingDefinePack.IDGroup IDInfo
    {
      get
      {
        return this._idGroup;
      }
    }

    public FishingDefinePack.LureParamGroup LureParam
    {
      get
      {
        return this._lureParam;
      }
    }

    public FishingDefinePack.FishParamGroup FishParam
    {
      get
      {
        return this._fishParam;
      }
    }

    public FishingDefinePack.UIParamGroup UIParam
    {
      get
      {
        return this._uiParam;
      }
    }

    public FishingDefinePack.PlayerParamGroup PlayerParam
    {
      get
      {
        return this._playerParam;
      }
    }

    public Dictionary<int, float> GetEfcScaleTable
    {
      get
      {
        return this._getEfcScaleTable;
      }
    }

    public Dictionary<SEType, int> SETable
    {
      get
      {
        return this._seTable;
      }
    }

    [Serializable]
    public class AssetBundleNameGroup
    {
      [SerializeField]
      private string _fishingInfoListBundleDirectory = string.Empty;

      public string FishingInfoListBundleDirectory
      {
        get
        {
          return this._fishingInfoListBundleDirectory;
        }
      }
    }

    [Serializable]
    public class SystemParamGroup
    {
      [SerializeField]
      private LayerMask _fishingLayerMask = (LayerMask) null;
      [SerializeField]
      private string _fishingMeshTagName = "Water";
      [SerializeField]
      private LayerMask _lureWaterBoxLayerMask = (LayerMask) null;
      [SerializeField]
      private string _lureWaterBoxTagName = "Water";
      [SerializeField]
      private LayerMask _luxWaterLayerMask = (LayerMask) null;
      [SerializeField]
      private int _fishMaxNum = 3;
      [SerializeField]
      private float _soundRoodDistance = 5f;
      [SerializeField]
      private float _moveAreaRadius = 17.5f;
      [SerializeField]
      private Vector3 _moveAreaOffsetPosition = Vector3.get_zero();
      [SerializeField]
      private int _maxLevel = 100;
      [SerializeField]
      private float _defaultDamage = 10f;
      [SerializeField]
      private float _maxDamage = 10f;
      [SerializeField]
      private float _normalDamageAngle = 90f;
      [SerializeField]
      private float _criticalDamageAngle = 30f;
      [SerializeField]
      private float _angryDamageScale = 0.1f;
      [SerializeField]
      private float _angryCountDownScale = 2f;
      [SerializeField]
      private float _fishingTimeLimit = 30f;
      [SerializeField]
      private int _nextExperience = 100;
      [SerializeField]
      private float _arrowMaxPower = 1f;
      [SerializeField]
      private float _arrowAddAngle = 360f;
      [SerializeField]
      private float _deviceArrowPowerScale = 2.5f;
      [SerializeField]
      private float _mouseArrowPowerScale = 0.3f;
      [SerializeField]
      private float _distanceToCircle = 5f;

      public LayerMask FishingLayerMask
      {
        get
        {
          return this._fishingLayerMask;
        }
      }

      public string FishingMeshTagName
      {
        get
        {
          return this._fishingMeshTagName;
        }
      }

      public LayerMask LureWaterBoxLayerMask
      {
        get
        {
          return this._lureWaterBoxLayerMask;
        }
      }

      public string LureWaterBoxTagName
      {
        get
        {
          return this._lureWaterBoxTagName;
        }
      }

      public LayerMask LuxWaterLayerMask
      {
        get
        {
          return this._luxWaterLayerMask;
        }
      }

      public int FishMaxNum
      {
        get
        {
          return this._fishMaxNum;
        }
      }

      public float SoundRoodDistance
      {
        get
        {
          return this._soundRoodDistance;
        }
      }

      public float MoveAreaRadius
      {
        get
        {
          return this._moveAreaRadius;
        }
      }

      public Vector3 MoveAreaOffsetPosition
      {
        get
        {
          return this._moveAreaOffsetPosition;
        }
      }

      public int MaxLevel
      {
        get
        {
          return this._maxLevel;
        }
      }

      public float DefaultDamage
      {
        get
        {
          return this._defaultDamage;
        }
      }

      public float MaxDamage
      {
        get
        {
          return this._maxDamage;
        }
      }

      public float NormalDamageAngle
      {
        get
        {
          return this._normalDamageAngle;
        }
      }

      public float CriticalDamageAngle
      {
        get
        {
          return this._criticalDamageAngle;
        }
      }

      public float AngryDamageScale
      {
        get
        {
          return this._angryDamageScale;
        }
      }

      public float AngryCountDownScale
      {
        get
        {
          return this._angryCountDownScale;
        }
      }

      public float FishingTimeLimit
      {
        get
        {
          return this._fishingTimeLimit;
        }
      }

      public int NextExperience
      {
        get
        {
          return this._nextExperience;
        }
      }

      public float ArrowMaxPower
      {
        get
        {
          return this._arrowMaxPower;
        }
      }

      public float ArrowAddAngle
      {
        get
        {
          return this._arrowAddAngle;
        }
      }

      public float DeviceArrowPowerScale
      {
        get
        {
          return this._deviceArrowPowerScale;
        }
      }

      public float MouseArrowPowerScale
      {
        get
        {
          return this._mouseArrowPowerScale;
        }
      }

      public float DistanceToCircle
      {
        get
        {
          return this._distanceToCircle;
        }
      }
    }

    [Serializable]
    public class IDGroup
    {
      [SerializeField]
      private int _lureEventItemID = 3;
      [SerializeField]
      private int _fishItemCategoryID = 2;
      [SerializeField]
      private FishingDefinePack.ItemIDPair _fishingRod = new FishingDefinePack.ItemIDPair();
      [SerializeField]
      private FishingDefinePack.ItemIDPair _brokenFishingRod = new FishingDefinePack.ItemIDPair();
      [SerializeField]
      private FishingDefinePack.ItemIDPair _kotsubuuo = new FishingDefinePack.ItemIDPair();
      [SerializeField]
      private List<FishingDefinePack.ItemIDPair> _fishList = new List<FishingDefinePack.ItemIDPair>();
      [SerializeField]
      private FishingDefinePack.ItemIDPair _grilledFish = new FishingDefinePack.ItemIDPair();

      public int LureEventItemID
      {
        get
        {
          return this._lureEventItemID;
        }
      }

      public int FishItemCategoryID
      {
        get
        {
          return this._fishItemCategoryID;
        }
      }

      public FishingDefinePack.ItemIDPair FishingRod
      {
        get
        {
          return this._fishingRod;
        }
      }

      public FishingDefinePack.ItemIDPair BrokenFishingRod
      {
        get
        {
          return this._brokenFishingRod;
        }
      }

      public FishingDefinePack.ItemIDPair Kotsubuuo
      {
        get
        {
          return this._kotsubuuo;
        }
      }

      public List<FishingDefinePack.ItemIDPair> FishList
      {
        get
        {
          return this._fishList;
        }
      }

      public FishingDefinePack.ItemIDPair GrilledFish
      {
        get
        {
          return this._grilledFish;
        }
      }
    }

    [Serializable]
    public class LureParamGroup
    {
      [SerializeField]
      private Vector3 _dropOffsetPosition = Vector3.get_zero();
      [SerializeField]
      private float _dropOffsetHeight = -0.5f;
      [SerializeField]
      private float _throwTime = 0.75f;
      [SerializeField]
      private float _returnTime = 0.75f;
      [SerializeField]
      private float _floatMoveMaxSpeed = 12.5f;
      [SerializeField]
      private float _mouseAxisScale = 1.25f;
      [SerializeField]
      private float _waterDensity = 1f;
      [SerializeField]
      private float _density = 0.75f;
      [SerializeField]
      private float _normalizedVoxelSize = 1f;
      [SerializeField]
      private float _dragInWater = 1f;
      [SerializeField]
      private float _angularDragInWater = 1f;

      public Vector3 DropOffsetPosition
      {
        get
        {
          return this._dropOffsetPosition;
        }
      }

      public float DropOffsetHeight
      {
        get
        {
          return this._dropOffsetHeight;
        }
      }

      public float ThrowTime
      {
        get
        {
          return this._throwTime;
        }
      }

      public float ReturnTime
      {
        get
        {
          return this._returnTime;
        }
      }

      public float FloatMoveMaxSpeed
      {
        get
        {
          return this._floatMoveMaxSpeed;
        }
      }

      public float MouseAxisScale
      {
        get
        {
          return this._mouseAxisScale;
        }
      }

      public float WaterDensity
      {
        get
        {
          return this._waterDensity;
        }
      }

      public float Density
      {
        get
        {
          return this._density;
        }
      }

      public float NormalizedVoxelSize
      {
        get
        {
          return this._normalizedVoxelSize;
        }
      }

      public float DragInWater
      {
        get
        {
          return this._dragInWater;
        }
      }

      public float AngularDragInWater
      {
        get
        {
          return this._angularDragInWater;
        }
      }
    }

    [Serializable]
    public class FishParamGroup
    {
      [SerializeField]
      private float _swimSpeed = 3f;
      [SerializeField]
      private float _followSpeed = 3.6f;
      [SerializeField]
      private float _escapeSpeed = 18f;
      [SerializeField]
      private float _escapeFadeTime = 1f;
      [SerializeField]
      private float _swimDistance = 5f;
      [SerializeField]
      private float _swimStopDistance = 2f;
      [SerializeField]
      private float _swimAddAngle = 80f;
      [SerializeField]
      private float _followAddAngle = 80f;
      [SerializeField]
      private float _findAngle = 90f;
      [SerializeField]
      private float _findDistance = 10f;
      [SerializeField]
      private float _hitDistance = 2f;
      [SerializeField]
      private float _reFindTime = 0.5f;
      [SerializeField]
      private float _destroyMinTime = 30f;
      [SerializeField]
      private float _destroyMaxTime = 60f;
      [SerializeField]
      private string _animLoopName = "shadowfish_loop";
      [SerializeField]
      private string _animHitName = "shadowfish_hit";
      [SerializeField]
      private string _animAngryName = "shadowfish_angry";
      [SerializeField]
      private float _createOffsetHeight;
      [SerializeField]
      private FishingDefinePack.FishHitParamGroup _hitParam;

      public float CreateOffsetHeight
      {
        get
        {
          return this._createOffsetHeight;
        }
      }

      public float SwimSpeed
      {
        get
        {
          return this._swimSpeed;
        }
      }

      public float FollowSpeed
      {
        get
        {
          return this._followSpeed;
        }
      }

      public float EscapeSpeed
      {
        get
        {
          return this._escapeSpeed;
        }
      }

      public float EscapeFadeTime
      {
        get
        {
          return this._escapeFadeTime;
        }
      }

      public float SwimDistance
      {
        get
        {
          return this._swimDistance;
        }
      }

      public float SwimStopDistance
      {
        get
        {
          return this._swimStopDistance;
        }
      }

      public float SwimAddAngle
      {
        get
        {
          return this._swimAddAngle;
        }
      }

      public float FollowAddAngle
      {
        get
        {
          return this._followAddAngle;
        }
      }

      public float FindAngle
      {
        get
        {
          return this._findAngle;
        }
      }

      public float FindDistance
      {
        get
        {
          return this._findDistance;
        }
      }

      public float HitDistance
      {
        get
        {
          return this._hitDistance;
        }
      }

      public float ReFindTime
      {
        get
        {
          return this._reFindTime;
        }
      }

      public float DestroyMinTime
      {
        get
        {
          return this._destroyMinTime;
        }
      }

      public float DestroyMaxTime
      {
        get
        {
          return this._destroyMaxTime;
        }
      }

      public string AnimLoopName
      {
        get
        {
          return this._animLoopName;
        }
      }

      public string AnimHitName
      {
        get
        {
          return this._animHitName;
        }
      }

      public string AnimAngryName
      {
        get
        {
          return this._animAngryName;
        }
      }

      public FishingDefinePack.FishHitParamGroup HitParam
      {
        get
        {
          return this._hitParam;
        }
      }
    }

    [Serializable]
    public class FishHitParamGroup
    {
      [SerializeField]
      private Vector3 _moveAreaOffsetPosition = Vector3.get_zero();
      [SerializeField]
      private float _moveAreaAngle = 65f;
      [SerializeField]
      private float _moveAreaMinRadius = 20f;
      [SerializeField]
      private float _moveAreaMaxRadius = 45f;
      [SerializeField]
      private float _angryNextMinTime = 10f;
      [SerializeField]
      private float _angryNextMaxTime = 20f;
      [SerializeField]
      private float _angryMinTimeLimit = 5f;
      [SerializeField]
      private float _angryMaxTimeLimit = 6f;
      [SerializeField]
      private float _moveSpeed = 25f;
      [SerializeField]
      private float _firstAddAngle = 200f;
      [SerializeField]
      private float _addAngle = 120f;
      [SerializeField]
      private float _nextMinAngle = 50f;
      [SerializeField]
      private float _nextMaxAngle = 170f;
      [SerializeField]
      private float _angleMinTimeLimit = 1f;
      [SerializeField]
      private float _angleMaxTimeLimit = 3f;
      [SerializeField]
      private float _nextMinRadius = 3f;
      [SerializeField]
      private float _nextMaxRadius = 10f;
      [SerializeField]
      private float _radiusMinSpeed = 10f;
      [SerializeField]
      private float _radiusMaxSpeed = 25f;
      [SerializeField]
      private float _radiusMinWaitTimeLimit = 2f;
      [SerializeField]
      private float _radiusMaxWaitTimeLimit = 4f;
      [SerializeField]
      private bool _changeRadiusEasing;

      public Vector3 MoveAreaOffsetPosition
      {
        get
        {
          return this._moveAreaOffsetPosition;
        }
      }

      public float MoveAreaAngle
      {
        get
        {
          return this._moveAreaAngle;
        }
      }

      public float MoveAreaMinRadius
      {
        get
        {
          return this._moveAreaMinRadius;
        }
      }

      public float MoveAreaMaxRadius
      {
        get
        {
          return this._moveAreaMaxRadius;
        }
      }

      public float AngryNextMinTime
      {
        get
        {
          return this._angryNextMinTime;
        }
      }

      public float AngryNextMaxTime
      {
        get
        {
          return this._angryNextMaxTime;
        }
      }

      public float AngryMinTimeLimit
      {
        get
        {
          return this._angryMinTimeLimit;
        }
      }

      public float AngryMaxTimeLimit
      {
        get
        {
          return this._angryMaxTimeLimit;
        }
      }

      public float MoveSpeed
      {
        get
        {
          return this._moveSpeed;
        }
      }

      public float FirstAddAngle
      {
        get
        {
          return this._firstAddAngle;
        }
      }

      public float AddAngle
      {
        get
        {
          return this._addAngle;
        }
      }

      public float NextMinAngle
      {
        get
        {
          return this._nextMinAngle;
        }
      }

      public float NextMaxAngle
      {
        get
        {
          return this._nextMaxAngle;
        }
      }

      public float AngleMinTimeLimit
      {
        get
        {
          return this._angleMinTimeLimit;
        }
      }

      public float AngleMaxTimeLimit
      {
        get
        {
          return this._angleMaxTimeLimit;
        }
      }

      public float NextMinRadius
      {
        get
        {
          return this._nextMinRadius;
        }
      }

      public float NextMaxRadius
      {
        get
        {
          return this._nextMaxRadius;
        }
      }

      public float RadiusMinSpeed
      {
        get
        {
          return this._radiusMinSpeed;
        }
      }

      public float RadiusMaxSpeed
      {
        get
        {
          return this._radiusMaxSpeed;
        }
      }

      public bool ChangeRadiusEasing
      {
        get
        {
          return this._changeRadiusEasing;
        }
      }

      public float RadiusMinWaitTimeLimit
      {
        get
        {
          return this._radiusMinWaitTimeLimit;
        }
      }

      public float RadiusMaxWaitTimeLimit
      {
        get
        {
          return this._radiusMaxWaitTimeLimit;
        }
      }
    }

    [Serializable]
    public class UIParamGroup
    {
      [SerializeField]
      private float _arrowAnimWaitTimeLimit = 1f;
      [SerializeField]
      private float _arrowAnimFadeInTimeLimit = 0.1f;
      [SerializeField]
      private float _arrowAnimFadeOutTimeLimit = 0.2f;
      [SerializeField]
      private float _arrowAnimCancelFadeTimeLimit = 0.1f;
      [SerializeField]
      private string[] _rarelityLabelList = new string[0];
      [SerializeField]
      private float _experienceAddParSecond = 33f;

      public float ArrowAnimWaitTimeLimit
      {
        get
        {
          return this._arrowAnimWaitTimeLimit;
        }
      }

      public float ArrowAnimFadeInTimeLimit
      {
        get
        {
          return this._arrowAnimFadeInTimeLimit;
        }
      }

      public float ArrowAnimFadeOutTimeLimit
      {
        get
        {
          return this._arrowAnimFadeOutTimeLimit;
        }
      }

      public float ArrowAnimCancelFadeTimeLimit
      {
        get
        {
          return this._arrowAnimCancelFadeTimeLimit;
        }
      }

      public string[] RarelityLabelList
      {
        get
        {
          return this._rarelityLabelList;
        }
      }

      public float ExperienceAddParSecond
      {
        get
        {
          return this._experienceAddParSecond;
        }
      }
    }

    [Serializable]
    public class PlayerParamGroup
    {
      [SerializeField]
      private float _waitHitMoveHorizontalScale = 3f;
      [SerializeField]
      private float _waitHitReturnHorizontalScale = 3f;
      [SerializeField]
      private float _fishingHorizontalScale = 4f;
      [SerializeField]
      private float _rodHitWaitAngleSpeed = 5f;
      [SerializeField]
      private float _rodAngleScale = 1.5f;
      [SerializeField]
      private string _fishingGamePrefabName = string.Empty;
      [SerializeField]
      private string _animStandbyName = string.Empty;
      [SerializeField]
      private string _playerAnimParamName = string.Empty;
      [SerializeField]
      private string _rodAnimParamName = string.Empty;

      public float WaitHitMoveHorizontalScale
      {
        get
        {
          return this._waitHitMoveHorizontalScale;
        }
      }

      public float WaitHitReturnHorizontalScale
      {
        get
        {
          return this._waitHitReturnHorizontalScale;
        }
      }

      public float FishingHorizontalScale
      {
        get
        {
          return this._fishingHorizontalScale;
        }
      }

      public float RodHitWaitAngleSpeed
      {
        get
        {
          return this._rodHitWaitAngleSpeed;
        }
      }

      public float RodAngleScale
      {
        get
        {
          return this._rodAngleScale;
        }
      }

      public string FishingGamePrefabName
      {
        get
        {
          return this._fishingGamePrefabName;
        }
      }

      public string AnimStandbyName
      {
        get
        {
          return this._animStandbyName;
        }
      }

      public string PlayerAnimParamName
      {
        get
        {
          return this._playerAnimParamName;
        }
      }

      public string RodAnimParamName
      {
        get
        {
          return this._rodAnimParamName;
        }
      }
    }

    [Serializable]
    public struct ItemIDPair
    {
      public int CategoryID;
      public int ItemID;

      public ItemIDPair(int categoryID, int itemID)
      {
        this.CategoryID = categoryID;
        this.ItemID = itemID;
      }
    }
  }
}
