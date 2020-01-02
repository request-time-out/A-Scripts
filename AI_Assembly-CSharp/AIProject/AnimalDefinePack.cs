// Decompiled with JetBrains decompiler
// Type: AIProject.AnimalDefinePack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace AIProject
{
  public class AnimalDefinePack : ScriptableObject
  {
    [SerializeField]
    [Header("Asset Bundle")]
    private AnimalDefinePack.AssetBundleNameGroup _assetBundleNames;
    [SerializeField]
    [Header("Animator")]
    private AnimalDefinePack.AnimatorInfoGroup _animatorInfo;
    [SerializeField]
    [Header("System Info")]
    private AnimalDefinePack.SystemInfoGroup _systemInfo;
    [SerializeField]
    [Header("Create Start Time Info")]
    private AnimalDefinePack.CreateStartTimeInfoGroup _createStartTimeInfo;
    [SerializeField]
    [Header("NavMesh Agent Info")]
    private AnimalDefinePack.NavMeshAgentInfoGroup _agentInfo;
    [SerializeField]
    [Header("All Animal Info")]
    private AnimalDefinePack.AllAnimalInfoGroup _allAnimalInfo;
    [SerializeField]
    [Header("WithActor Info")]
    private AnimalDefinePack.WithActorInfoGroup _withActorInfo;
    [SerializeField]
    [Header("Ground Animal Info")]
    private AnimalDefinePack.GroundAnimalInfoGroup _groundAnimalInfo;
    [SerializeField]
    [Header("Ground Wild Animal Info")]
    private AnimalDefinePack.GroundWildInfoGroup _groundWildInfo;
    [SerializeField]
    [Header("Pet Cat Info")]
    private AnimalDefinePack.PetCatInfoGroup _petCatInfo;
    [SerializeField]
    [Header("Desire Info")]
    private AnimalDefinePack.DesireInfoGroup _desireInfo;
    [SerializeField]
    [Header("Chicken Coop Waypoint Setting")]
    private AnimalDefinePack.ChickenCoopWaypointSettings _chickenCoopWaypointSetting;
    [SerializeField]
    [Header("Sound ID Info")]
    private AnimalDefinePack.SoundIDInfo _soundID;

    public AnimalDefinePack()
    {
      base.\u002Ector();
    }

    public AnimalDefinePack.AssetBundleNameGroup AssetBundleNames
    {
      get
      {
        return this._assetBundleNames;
      }
    }

    public AnimalDefinePack.AnimatorInfoGroup AnimatorInfo
    {
      get
      {
        return this._animatorInfo;
      }
    }

    public AnimalDefinePack.SystemInfoGroup SystemInfo
    {
      get
      {
        return this._systemInfo;
      }
    }

    public AnimalDefinePack.CreateStartTimeInfoGroup CreateStartTimeInfo
    {
      get
      {
        return this._createStartTimeInfo;
      }
    }

    public AnimalDefinePack.NavMeshAgentInfoGroup AgentInfo
    {
      get
      {
        return this._agentInfo;
      }
    }

    public AnimalDefinePack.AllAnimalInfoGroup AllAnimalInfo
    {
      get
      {
        return this._allAnimalInfo;
      }
    }

    public AnimalDefinePack.WithActorInfoGroup WithActorInfo
    {
      get
      {
        return this._withActorInfo;
      }
    }

    public AnimalDefinePack.GroundAnimalInfoGroup GroundAnimalInfo
    {
      get
      {
        return this._groundAnimalInfo;
      }
    }

    public AnimalDefinePack.GroundWildInfoGroup GroundWildInfo
    {
      get
      {
        return this._groundWildInfo;
      }
    }

    public AnimalDefinePack.PetCatInfoGroup PetCatInfo
    {
      get
      {
        return this._petCatInfo;
      }
    }

    public AnimalDefinePack.DesireInfoGroup DesireInfo
    {
      get
      {
        return this._desireInfo;
      }
    }

    public AnimalDefinePack.ChickenCoopWaypointSettings ChickenCoopWaypointSetting
    {
      get
      {
        return this._chickenCoopWaypointSetting;
      }
    }

    public AnimalDefinePack.SoundIDInfo SoundID
    {
      get
      {
        return this._soundID;
      }
    }

    [Serializable]
    public class AssetBundleNameGroup
    {
      [Header("AssetBundle Directory")]
      [SerializeField]
      [Tooltip("動物の色々なリストまとめ")]
      private string _animalInfoDirectory = string.Empty;
      [SerializeField]
      [Tooltip("ペットのベースのプレファブのパス")]
      private string _prefabsBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("行動に関する情報リストのパス")]
      private string _actionInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("各動物のアニメーターのパス")]
      private string _animatorListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("行動に対するアニメーション名のリストのパス")]
      private string _animeInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("Actor(Player/Agent)と連携されたアニメーション名リストのパス")]
      private string _withActorAnimeInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("モデルデータのリストのパス")]
      private string _modelInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("注視条件情報のリストのパス")]
      private string _lookInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("状態に関する情報のリストのパス")]
      private string _stateInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("欲求に関する情報のリストのパス")]
      private string _desireInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("動物に関係したプレイヤーの設定情報等のリストのパス")]
      private string _playerInfoListBundleDirectory = string.Empty;
      [SerializeField]
      [Tooltip("動物ポイントのプレファブ情報のリストのパス")]
      private string _animalPointPrefabBundleDirectory = string.Empty;

      public string AnimalInfoDirectory
      {
        get
        {
          return this._animalInfoDirectory;
        }
      }

      public string PrefabsBundleDirectory
      {
        get
        {
          return this._prefabsBundleDirectory;
        }
      }

      public string ActionInfoListBundleDirectory
      {
        get
        {
          return this._actionInfoListBundleDirectory;
        }
      }

      public string AnimatorListBundleDirectory
      {
        get
        {
          return this._animatorListBundleDirectory;
        }
      }

      public string AnimeInfoListBundleDirectory
      {
        get
        {
          return this._animeInfoListBundleDirectory;
        }
      }

      public string WithActorAnimeInfoListBundleDirectory
      {
        get
        {
          return this._withActorAnimeInfoListBundleDirectory;
        }
      }

      public string ModelInfoListBundleDirector
      {
        get
        {
          return this._modelInfoListBundleDirectory;
        }
      }

      public string LookInfoListBundleDirectory
      {
        get
        {
          return this._lookInfoListBundleDirectory;
        }
      }

      public string StateInfoListBundleDirectory
      {
        get
        {
          return this._stateInfoListBundleDirectory;
        }
      }

      public string DesireInfoListBundleDirectory
      {
        get
        {
          return this._desireInfoListBundleDirectory;
        }
      }

      public string PlayerInfoListBundleDirectory
      {
        get
        {
          return this._playerInfoListBundleDirectory;
        }
      }

      public string AnimalPointPrefabBundleDirectory
      {
        get
        {
          return this._animalPointPrefabBundleDirectory;
        }
      }
    }

    [Serializable]
    public class AnimatorInfoGroup
    {
      [SerializeField]
      [Tooltip("Locomotionのパラメーター名")]
      private string _locomotionParamName = string.Empty;
      [SerializeField]
      [Tooltip("アニメーション速度のパラメーター名")]
      private string _animationSpeedParamName = string.Empty;
      [SerializeField]
      [Tooltip("トリのアニメーション速度(通常１)")]
      private float _birdAnimationSpeed = 1f;
      [SerializeField]
      [Tooltip("チョウのアニメーション速度(通常１)")]
      private float _butterflyAnimationSpeed = 1f;

      public string LocomotionParamName
      {
        get
        {
          return this._locomotionParamName;
        }
      }

      public string AnimationSpeedParamName
      {
        get
        {
          return this._animationSpeedParamName;
        }
      }

      public float BirdAnimationSpeed
      {
        get
        {
          return this._birdAnimationSpeed;
        }
      }

      public float ButterflyAnimationSpeed
      {
        get
        {
          return this._butterflyAnimationSpeed;
        }
      }
    }

    [Serializable]
    public class SystemInfoGroup
    {
      [SerializeField]
      [Tooltip("地面系野生動物同時最大出現数")]
      private int _WildGroundMaxNum = 4;
      [SerializeField]
      [Tooltip("野生ネコ同時最大出現数")]
      private int _wildCatMaxNum = 2;
      [SerializeField]
      [Tooltip("野生ニワトリ同時最大出現数")]
      private int _wildChickenMaxNum = 2;
      [SerializeField]
      [Tooltip("野生メカ同時最大出現数")]
      private int _wildMechaMaxNum = 2;
      [SerializeField]
      [Tooltip("野生カエル同時最大出現数")]
      private int _wildFrogMaxNum = 2;
      [SerializeField]
      [Tooltip("野生チョウ同時最大出現数")]
      private int _wildButterflyMaxNum = 3;
      [SerializeField]
      [Tooltip("野生トリの群れ同時最大出現数")]
      private int _wildBirdFlockMaxNum = 1;
      [SerializeField]
      [RangeZeroOver("野生ネコ生成間隔(秒)の範囲")]
      private Vector2 _wildCatCreateCoolTime = Vector2.get_zero();
      [SerializeField]
      [RangeZeroOver("野生ニワトリ生成間隔(秒)の範囲")]
      private Vector2 _wildChickenCreateCoolTime = Vector2.get_zero();
      [SerializeField]
      [Min(0.0f)]
      [Tooltip("Playerからどのくらい離れていたら生成されるかの距離")]
      private float _popDistance = 50f;
      [SerializeField]
      [RangeZeroOver("地面系動物:出現ポイント使用後のクールタイム")]
      private Vector2 _popPointCoolTimeRange = Vector2.get_zero();
      [SerializeField]
      [Min(0.0f)]
      [Tooltip("生成時の視界判定時の範囲内のポイントのスケール")]
      private float _viewportPointScale = 0.75f;
      [SerializeField]
      [RangeZeroOver("野生カエル：クールタイム")]
      private Vector2 _frogCoolTimeRange = Vector2.get_zero();

      public int WildGroundMaxNum
      {
        get
        {
          return this._WildGroundMaxNum;
        }
      }

      public int WildCatMaxNum
      {
        get
        {
          return this._wildCatMaxNum;
        }
      }

      public int WildChickenMaxNum
      {
        get
        {
          return this._wildChickenMaxNum;
        }
      }

      public int WildMechaMaxNum
      {
        get
        {
          return this._wildMechaMaxNum;
        }
      }

      public int WildFrogMaxNum
      {
        get
        {
          return this._wildFrogMaxNum;
        }
      }

      public int WildButterflyMaxNum
      {
        get
        {
          return this._wildButterflyMaxNum;
        }
      }

      public int WildBirdFlockMaxNum
      {
        get
        {
          return this._wildBirdFlockMaxNum;
        }
      }

      public Vector2 WildCatCreateCoolTime
      {
        get
        {
          return this._wildCatCreateCoolTime;
        }
      }

      public Vector2 WildChickenCreateCoolTime
      {
        get
        {
          return this._wildChickenCreateCoolTime;
        }
      }

      public float PopDistance
      {
        get
        {
          return this._popDistance;
        }
      }

      public Vector2 PopPointCoolTimeRange
      {
        get
        {
          return this._popPointCoolTimeRange;
        }
      }

      public float ViewportPointScale
      {
        get
        {
          return this._viewportPointScale;
        }
      }

      public Vector2 FrogCoolTimeRange
      {
        get
        {
          return this._frogCoolTimeRange;
        }
      }
    }

    [Serializable]
    public class ChickenCoopWaypointSettings
    {
      [SerializeField]
      private Vector3 _installation = (Vector3) null;
      [SerializeField]
      private float _sampleDistance = 0.25f;
      [SerializeField]
      private float _closestEdgeDistance = 1f;
      [SerializeField]
      private LayerMask _layerMask = LayerMask.op_Implicit(0);
      [SerializeField]
      private string _tagName = string.Empty;
      [SerializeField]
      private float _canEatEdgeDistance = 3f;
      [SerializeField]
      private int _agentAreaMask;

      public Vector3 Installation
      {
        get
        {
          return this._installation;
        }
      }

      public float SampleDistance
      {
        get
        {
          return this._sampleDistance;
        }
      }

      public float ClosestEdgeDistance
      {
        get
        {
          return this._closestEdgeDistance;
        }
      }

      public LayerMask Layer
      {
        get
        {
          return this._layerMask;
        }
      }

      public string TagName
      {
        get
        {
          return this._tagName;
        }
      }

      public float CanEatEdgeDistance
      {
        get
        {
          return this._canEatEdgeDistance;
        }
      }

      public int AgentAreaMask
      {
        get
        {
          return this._agentAreaMask;
        }
      }
    }

    [Serializable]
    public struct SoundIDInfo
    {
      [SerializeField]
      private int _mechaStartup;
      [SerializeField]
      private int _getCat;
      [SerializeField]
      private int _getChicken;

      public int MechaStartup
      {
        get
        {
          return this._mechaStartup;
        }
      }

      public int GetCat
      {
        get
        {
          return this._getCat;
        }
      }

      public int GetChicken
      {
        get
        {
          return this._getChicken;
        }
      }
    }

    [Serializable]
    public class CreateStartTimeInfoGroup
    {
      [SerializeField]
      [RangeZeroOver("ネコの生成開始時間")]
      private Vector2 _cat = Vector2.get_zero();
      [SerializeField]
      [RangeZeroOver("ニワトリの生成開始時間")]
      private Vector2 _chicken = Vector2.get_zero();
      [SerializeField]
      [RangeZeroOver("ネコ＆ニワトリの生成開始時間")]
      private Vector2 _catAndChicken = Vector2.get_zero();
      [SerializeField]
      [RangeZeroOver("メカの生成開始時間")]
      private Vector2 _mecha = Vector2.get_zero();
      [SerializeField]
      [RangeZeroOver("カエルの生成開始時間")]
      private Vector2 _frog = Vector2.get_zero();
      [SerializeField]
      [RangeZeroOver("トリの群れの生成開始時間")]
      private Vector2 _birdFlock = Vector2.get_zero();

      public Vector2 Cat
      {
        get
        {
          return this._cat;
        }
      }

      public Vector2 Chicken
      {
        get
        {
          return this._chicken;
        }
      }

      public Vector2 CatAndChicken
      {
        get
        {
          return this._catAndChicken;
        }
      }

      public Vector2 Mecha
      {
        get
        {
          return this._mecha;
        }
      }

      public Vector2 Frog
      {
        get
        {
          return this._frog;
        }
      }

      public Vector2 BirdFlock
      {
        get
        {
          return this._birdFlock;
        }
      }
    }

    [Serializable]
    public class NavMeshAgentInfoGroup
    {
      [SerializeField]
      [Tooltip("地面系動物の開始優先度")]
      private int _groundAnimalStartPriority = 51;
      [SerializeField]
      [Tooltip("優先度の間隔")]
      private int _priorityMargin = 5;

      public int GroundAnimalStartPriority
      {
        get
        {
          return this._groundAnimalStartPriority;
        }
      }

      public int PriorityMargin
      {
        get
        {
          return this._priorityMargin;
        }
      }
    }

    [Serializable]
    public class AllAnimalInfoGroup
    {
      [SerializeField]
      [Tooltip("行動ポイント検索後のクールタイム")]
      private float _actionPointSearchedCoolTime = 30f;
      [SerializeField]
      [Tooltip("行動ポイント使用後のクールタイム")]
      private float _actionPointUsedCoolTime = 180f;
      [SerializeField]
      [Tooltip("なつき度の最大値")]
      private float _lovelyMaxParam = 100f;

      public float ActionPointSearchedCoolTime
      {
        get
        {
          return this._actionPointSearchedCoolTime;
        }
      }

      public float ActionPointUsedCoolTime
      {
        get
        {
          return this._actionPointUsedCoolTime;
        }
      }

      public float LovelyMaxParam
      {
        get
        {
          return this._lovelyMaxParam;
        }
      }
    }

    [Serializable]
    public class GroundAnimalInfoGroup
    {
      [SerializeField]
      [Tooltip("停止距離からどれくらいの距離で追尾再開するか")]
      private float _followIdleSpace = 5f;
      [SerializeField]
      [Tooltip("追尾中のターゲットからどのくらい距離を離れて止まるか")]
      private float _followStopDistance = 20f;
      [SerializeField]
      [Tooltip("")]
      private float _forwardAnimationLerpValue = 0.5f;

      public float FollowIdleSpace
      {
        get
        {
          return this._followIdleSpace;
        }
      }

      public float FollowStopDistance
      {
        get
        {
          return this._followStopDistance;
        }
      }

      public float ForwardAnimationLerpValue
      {
        get
        {
          return this._forwardAnimationLerpValue;
        }
      }
    }

    [Serializable]
    public class GroundWildInfoGroup
    {
      [SerializeField]
      [Tooltip("懐き状態の時間")]
      private float _lovelyTime = 120f;
      [SerializeField]
      [Tooltip("不機嫌状態のリアルタイムの時間(秒)")]
      private float _badMoodTime = 600f;
      [SerializeField]
      [Tooltip("地面系野生動物が消滅するのにかかる時間(秒)")]
      private float _destroyTimeSeconds = 1440f;
      [SerializeField]
      [Min(0.0f)]
      [Tooltip("消滅するとき CrossFadeが再生される距離")]
      private float _depopCrossFadeDistance;
      [SerializeField]
      [Min(0.0f)]
      [MaxValue(180.0)]
      [Tooltip("消滅するとき CrossFadeが再生される角度(0～180)")]
      private float _depopCrossFadeAngle;

      public float LovelyTime
      {
        get
        {
          return this._lovelyTime;
        }
      }

      public float BadMoodTime
      {
        get
        {
          return this._badMoodTime;
        }
      }

      public float DestroyTimeSeconds
      {
        get
        {
          return this._destroyTimeSeconds;
        }
      }

      public float DepopCrossFadeDistance
      {
        get
        {
          return this._depopCrossFadeDistance;
        }
      }

      public float DepopCrossFadeAngle
      {
        get
        {
          return this._depopCrossFadeAngle;
        }
      }
    }

    [Serializable]
    public class PetCatInfoGroup
    {
      [SerializeField]
      [Tooltip("追尾状態になる必要なつき度")]
      private float _lovelyFollowParam = 10f;
      [SerializeField]
      [Tooltip("寂しい時、最も懐いているキャラが優先される距離")]
      private float _lonelinessFollowDistance = 300f;

      public float LovelyFollowParam
      {
        get
        {
          return this._lovelyFollowParam;
        }
      }

      public float LonelinessFollowDistance
      {
        get
        {
          return this._lonelinessFollowDistance;
        }
      }
    }

    [Serializable]
    public class DesireInfoGroup
    {
      [SerializeField]
      [Tooltip("欲求値がMaxに達した時の制限時間")]
      private float _motivationSecondTime = 300f;

      public float MotivationSecondTime
      {
        get
        {
          return this._motivationSecondTime;
        }
      }
    }

    [Serializable]
    public class WithActorInfoGroup
    {
      [SerializeField]
      [Tooltip("アニメーションを再生させるキャラクターからの距離")]
      private float _actionPointDistance = 11.5f;
      [SerializeField]
      [Tooltip("野生動物を捕まえる時のキャラクターからの距離")]
      private float _getPointDistance = 12.5f;

      public float ActionPointDistance
      {
        get
        {
          return this._actionPointDistance;
        }
      }

      public float GetPointDistance
      {
        get
        {
          return this._getPointDistance;
        }
      }
    }
  }
}
