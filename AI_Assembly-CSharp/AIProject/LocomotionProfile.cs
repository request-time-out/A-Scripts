// Decompiled with JetBrains decompiler
// Type: AIProject.LocomotionProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace AIProject
{
  public class LocomotionProfile : ScriptableObject
  {
    [SerializeField]
    private LocomotionProfile.PlayerSpeedSetting _playerSpeed;
    [SerializeField]
    private LocomotionProfile.AgentSpeedSetting _agentSpeed;
    [SerializeField]
    private LocomotionProfile.MerchantSpeedSetting _merchantSpeed;
    [SerializeField]
    private float _lerpSpeed;
    [SerializeField]
    private float _tutorialLerpSpeed;
    [SerializeField]
    [MinValue(1.0)]
    private float _walkableDistance;
    [SerializeField]
    [MinValue(1.0)]
    private float _mustRunDistance;
    [SerializeField]
    private float _actionPointNavMeshSampleDistance;
    [SerializeField]
    private float _approachDistanceActionPoint;
    [SerializeField]
    private float _approachDistanceActionPointCloser;
    [SerializeField]
    [MinValue(0.0)]
    private float _pointDistanceMargin;
    [SerializeField]
    [MinValue(0.0)]
    private float _waypointTweenMinDistance;
    [SerializeField]
    private float _searchActorRadius;
    [SerializeField]
    private float _accessInvasionRange;
    [SerializeField]
    private float _charaVisibleDistance;
    [SerializeField]
    [MinValue(1.0)]
    private float _effectiveDynamicBoneDistance;
    [SerializeField]
    [MinValue(1.0)]
    private float _crossFadeEnableDistance;
    [SerializeField]
    private float _timeToLeftState;
    [SerializeField]
    private int _obonEventItemID;
    [SerializeField]
    private float _timeToBeware;
    [SerializeField]
    private float _approachDistanceStoryPoint;
    [SerializeField]
    private float _minDistanceDoor;
    [SerializeField]
    private float _maxDistanceDoor;
    [SerializeField]
    private string _playerLocoItemParentName;
    [SerializeField]
    private string _agentLocoItemParentName;
    [SerializeField]
    private string _agentOtherParentName;
    [SerializeField]
    private string _rootParentName;
    [SerializeField]
    private string _leftHandParentName;
    [SerializeField]
    private string _holdingHandTarget;
    [SerializeField]
    private string _holdingElboTarget;
    [SerializeField]
    private string _rightHandParentName;
    [SerializeField]
    private string _faceLightParentName;
    [SerializeField]
    private Vector3 _faceLightOffset;
    [SerializeField]
    private Vector3 _enviroEffectOffset;
    [SerializeField]
    private LocomotionProfile.PhotoShotSetting _photoShot;
    [SerializeField]
    private Vector3 _communicationOffset;
    [SerializeField]
    private string _communicationDiagonalTargetName;
    [SerializeField]
    private LocomotionProfile.LensSettings _defaultLensSetting;
    [SerializeField]
    private Threshold _cameraPowX;
    [SerializeField]
    private Threshold _cameraPowY;
    [SerializeField]
    [Space]
    private Vector2 _defaultCameraAxisPow;
    [SerializeField]
    private Vector2 _cameraAccelRate;
    [SerializeField]
    private float _turnEnableAngle;
    [SerializeField]
    private LocomotionProfile.HousingWaypointSettings _housingWaypointSetting;
    [SerializeField]
    private LocomotionProfile.DropSearchActionPointSettings _dropSearchActionPointSetting;
    [SerializeField]
    private float _fishingWaterCheckDistance;

    public LocomotionProfile()
    {
      base.\u002Ector();
    }

    public LocomotionProfile.PlayerSpeedSetting PlayerSpeed
    {
      get
      {
        return this._playerSpeed;
      }
    }

    public LocomotionProfile.AgentSpeedSetting AgentSpeed
    {
      get
      {
        return this._agentSpeed;
      }
    }

    public LocomotionProfile.MerchantSpeedSetting MerchantSpeed
    {
      get
      {
        return this._merchantSpeed;
      }
    }

    public float LerpSpeed
    {
      get
      {
        return this._lerpSpeed;
      }
    }

    public float TutorialLerpSpeed
    {
      get
      {
        return this._tutorialLerpSpeed;
      }
    }

    public float WalkableDistance
    {
      get
      {
        return this._walkableDistance;
      }
    }

    public float MustRunDistance
    {
      get
      {
        return this._mustRunDistance;
      }
    }

    public float ActionPointNavMeshSampleDistance
    {
      get
      {
        return this._actionPointNavMeshSampleDistance;
      }
    }

    public float ApproachDistanceActionPoint
    {
      get
      {
        return this._approachDistanceActionPoint;
      }
    }

    public float ApproachDistanceActionPointCloser
    {
      get
      {
        return this._approachDistanceActionPointCloser;
      }
    }

    public float PointDistanceMargin
    {
      get
      {
        return this._pointDistanceMargin;
      }
    }

    public float WaypointTweenMinDistance
    {
      get
      {
        return this._waypointTweenMinDistance;
      }
    }

    public float SearchActorRadius
    {
      get
      {
        return this._searchActorRadius;
      }
    }

    public float AccessInvasionRange
    {
      get
      {
        return this._accessInvasionRange;
      }
    }

    public float CharaVisibleDistance
    {
      get
      {
        return this._charaVisibleDistance;
      }
    }

    public float EffectiveDynamicBoneDistance
    {
      get
      {
        return this._effectiveDynamicBoneDistance;
      }
    }

    public float CrossFadeEnableDistance
    {
      get
      {
        return this._crossFadeEnableDistance;
      }
    }

    public float TimeToLeftState
    {
      get
      {
        return this._timeToLeftState;
      }
    }

    public int ObonEventItemID
    {
      get
      {
        return this._obonEventItemID;
      }
    }

    public float TimeToBeware
    {
      get
      {
        return this._timeToBeware;
      }
    }

    public float ApproachDistanceStoryPoint
    {
      get
      {
        return this._approachDistanceStoryPoint;
      }
    }

    public float MinDistanceDoor
    {
      get
      {
        return this._minDistanceDoor;
      }
    }

    public float MaxDistanceDoor
    {
      get
      {
        return this._maxDistanceDoor;
      }
    }

    public string PlayerLocoItemParentName
    {
      get
      {
        return this._playerLocoItemParentName;
      }
    }

    public string AgentLocoItemParentName
    {
      get
      {
        return this._agentLocoItemParentName;
      }
    }

    public string AgentOtherParentName
    {
      get
      {
        return this._agentOtherParentName;
      }
    }

    public string RootParentName
    {
      get
      {
        return this._rootParentName;
      }
    }

    public string LeftHandParentName
    {
      get
      {
        return this._leftHandParentName;
      }
    }

    public string HoldingHandTarget
    {
      get
      {
        return this._holdingHandTarget;
      }
    }

    public string HoldingElboTarget
    {
      get
      {
        return this._holdingElboTarget;
      }
    }

    public string RightHandParentName
    {
      get
      {
        return this._rightHandParentName;
      }
    }

    public string FadeLightParentName
    {
      get
      {
        return this._faceLightParentName;
      }
    }

    public Vector3 FaceLightOffset
    {
      get
      {
        return this._faceLightOffset;
      }
    }

    public Vector3 EnviroEffectOffset
    {
      get
      {
        return this._enviroEffectOffset;
      }
    }

    public LocomotionProfile.PhotoShotSetting PhotoShot
    {
      get
      {
        return this._photoShot;
      }
    }

    public Vector3 CommunicationOffset
    {
      get
      {
        return this._communicationOffset;
      }
    }

    public string CommunicationDiagonalTargetName
    {
      get
      {
        return this._communicationDiagonalTargetName;
      }
    }

    public LocomotionProfile.LensSettings DefaultLensSetting
    {
      get
      {
        return this._defaultLensSetting;
      }
    }

    public Threshold CameraPowX
    {
      get
      {
        return this._cameraPowX;
      }
    }

    public Threshold CameraPowY
    {
      get
      {
        return this._cameraPowY;
      }
    }

    public Vector2 DefaultCameraAxisPow
    {
      get
      {
        return this._defaultCameraAxisPow;
      }
    }

    public Vector3 CameraAccelRate
    {
      get
      {
        return Vector2.op_Implicit(this._cameraAccelRate);
      }
    }

    public float TurnEnableAngle
    {
      get
      {
        return this._turnEnableAngle;
      }
    }

    public LocomotionProfile.HousingWaypointSettings HousingWaypointSetting
    {
      get
      {
        return this._housingWaypointSetting;
      }
    }

    public LocomotionProfile.DropSearchActionPointSettings DropSearchActionPointSetting
    {
      get
      {
        return this._dropSearchActionPointSetting;
      }
    }

    public float FishingWaterCheckDistance
    {
      get
      {
        return this._fishingWaterCheckDistance;
      }
    }

    [Serializable]
    public struct PlayerSpeedSetting
    {
      public float normalSpeed;
      public float walkSpeed;
    }

    [Serializable]
    public struct AgentSpeedSetting
    {
      public float walkSpeed;
      public float runSpeed;
      public float hurtSpeed;
      public float escapeSpeed;
      public float followRunSpeed;
      public float tutorialWalkSpeed;
      public float tutorialRunSpeed;
    }

    [Serializable]
    public struct MerchantSpeedSetting
    {
      public float walkSpeed;
      public float runSpeed;
    }

    [Serializable]
    public struct PhotoShotSetting
    {
      public float mouseZoomScale;
      public Vector3 maxOffset;
      public Vector3 minOffset;
      public Vector3 offsetMoveValue;
    }

    [Serializable]
    public struct LensSettings
    {
      public float FieldOfView;
      public float MinFOV;
      public float MaxFOV;
      public float NearClipPlane;
      public float FarClipPlane;
      public float Dutch;
      public float KeyZoomScale;
      public float KeyRotateScale;
    }

    [Serializable]
    public struct HousingWaypointSettings
    {
      public float InstallationDistance;
      public float InstallationHeight;
      public float ClosestEdgeDistance;
      public float SampleDistance;
    }

    [Serializable]
    public struct DropSearchActionPointSettings
    {
      public float AvailableAngle;
      public float AvailableDistance;
    }
  }
}
