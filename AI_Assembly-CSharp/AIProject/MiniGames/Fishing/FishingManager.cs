// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FishingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.Scene;
using AIProject.UI;
using LuxWater;
using Manager;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.MiniGames.Fishing
{
  public class FishingManager : MonoBehaviour
  {
    public static RaycastHit[] raycastHits = new RaycastHit[3];
    [NonSerialized]
    public System.Action FishGetEvent;
    [SerializeField]
    private GameObject rootObject;
    [SerializeField]
    private Transform soundRoot;
    [SerializeField]
    private Transform hitMoveArea;
    [SerializeField]
    private BoxCollider waterBox;
    [SerializeField]
    private Camera uiCamera;
    public Camera fishModelCamera;
    private FishingManager.FishingScene scene_;
    [NonSerialized]
    public PlayerInfo playerInfo;
    public AIProject.Player.Fishing playerFishing;
    [SerializeField]
    private GameObject moveArea;
    [SerializeField]
    private LineRenderer fishingLine;
    private GameObject _fishPrefab;
    private Fish hitFish;
    private float fishHeartPoint;
    private List<Fish> fishList;
    public Lure lure;
    private Vector3 axisArrowOriginScale_;
    [SerializeField]
    private Transform spriteRootObject;
    [SerializeField]
    private Transform axisArrowObject;
    [SerializeField]
    private Transform circleRootObject;
    [SerializeField]
    private FishingManager.SpriteColorPair outCircleSprite;
    [SerializeField]
    private FishingManager.SpriteColorPair normalCircleSprite;
    [SerializeField]
    private FishingManager.SpriteColorPair criticalCircleSprite;
    private IDisposable createFishDisposable;
    private int fishCreateNumber;
    private bool prevWarningMode;
    private Vector2 prevInputAxis;
    private Vector2 prevMouseAxis;
    private float fishingTimeCounter;
    private Dictionary<SEType, Dictionary<int, AudioSource>> SETable;

    public FishingManager()
    {
      base.\u002Ector();
    }

    public GameObject RootObject
    {
      get
      {
        return this.rootObject;
      }
    }

    public Transform SoundRoot
    {
      get
      {
        return this.soundRoot;
      }
    }

    public Transform HitMoveArea
    {
      get
      {
        return this.hitMoveArea;
      }
    }

    public BoxCollider WaterBox
    {
      get
      {
        return this.waterBox;
      }
    }

    public FishingManager.FishingScene scene
    {
      get
      {
        return this.scene_;
      }
      set
      {
        FishingManager.FishingScene scene = this.scene_;
        this.scene_ = value;
        this.ChangeScene(this.scene_, scene);
        MapUIContainer.FishingUI.ChangeFishScene(this.scene_);
      }
    }

    private void ChangeScene(FishingManager.FishingScene _scene, FishingManager.FishingScene _prev)
    {
      if (_prev == FishingManager.FishingScene.WaitHit)
      {
        if (this.createFishDisposable != null)
          this.createFishDisposable.Dispose();
        this.createFishDisposable = (IDisposable) null;
      }
      if (_scene != FishingManager.FishingScene.WaitHit)
      {
        if (_scene != FishingManager.FishingScene.Fishing)
          return;
        this.prevInputAxis = this.prevMouseAxis = Vector2.get_zero();
      }
      else
      {
        Vector3 eulerAngles = ((Component) this.playerFishing.player.CameraControl.CameraComponent).get_transform().get_eulerAngles();
        eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
        this.moveArea.get_transform().set_eulerAngles(eulerAngles);
        this.hitMoveArea.set_localPosition(Singleton<Resources>.Instance.FishingDefinePack.FishParam.HitParam.MoveAreaOffsetPosition);
      }
    }

    public GameObject MoveArea
    {
      get
      {
        return this.moveArea;
      }
    }

    private GameObject FishPrefab
    {
      get
      {
        if (Object.op_Inequality((Object) this._fishPrefab, (Object) null))
          return this._fishPrefab;
        this._fishPrefab = AssetUtility.LoadAsset<GameObject>(Singleton<Resources>.Instance.AnimalDefinePack.AssetBundleNames.PrefabsBundleDirectory, "fishing_fish", "abdata");
        return this._fishPrefab;
      }
    }

    public Vector2 ArrowPowerVector { get; private set; }

    public Vector3 AxisArrowOriginScale
    {
      get
      {
        return this.axisArrowOriginScale_;
      }
    }

    public float MaxDamage { get; private set; }

    public Color RBG(float r, float g, float b)
    {
      return new Color(r / (float) byte.MaxValue, g / (float) byte.MaxValue, b / (float) byte.MaxValue);
    }

    public Color RBGA(float r, float g, float b, float a)
    {
      return new Color(r / (float) byte.MaxValue, g / (float) byte.MaxValue, b / (float) byte.MaxValue, a / (float) byte.MaxValue);
    }

    public bool InputCancel
    {
      get
      {
        if (!Singleton<Manager.Input>.IsInstance() || !MapUIContainer.FishingUI.FocusInOn)
          return true;
        bool? nullable = this.playerFishing != null ? new bool?(this.playerFishing.player.CameraControl.CinemachineBrain.get_IsBlending()) : new bool?();
        return (!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0;
      }
    }

    public bool NextButtonDown
    {
      get
      {
        if (this.InputCancel)
          return false;
        Manager.Input instance = Singleton<Manager.Input>.Instance;
        return instance.IsPressedSubmit() || instance.IsPressedKey(ActionID.MouseLeft);
      }
    }

    public bool BackButtonDown
    {
      get
      {
        if (this.InputCancel)
          return false;
        Manager.Input instance = Singleton<Manager.Input>.Instance;
        return instance.IsPressedCancel() || instance.IsPressedKey(ActionID.MouseRight);
      }
    }

    public bool IsPressedVertical
    {
      get
      {
        return !this.InputCancel && Singleton<Manager.Input>.Instance.IsPressedVertical();
      }
    }

    public bool IsPressedAxis(ActionID _actionID)
    {
      return !this.InputCancel && Singleton<Manager.Input>.Instance.IsPressedAxis(_actionID);
    }

    public float GetAxis(ActionID _actionID)
    {
      return this.InputCancel ? 0.0f : Singleton<Manager.Input>.Instance.GetAxis(_actionID);
    }

    public Vector2 GetLeftStickAxis()
    {
      return this.InputCancel ? Vector2.get_zero() : Singleton<Manager.Input>.Instance.LeftStickAxis;
    }

    public Vector2 GetMouseAxis()
    {
      return this.InputCancel ? Vector2.get_zero() : Singleton<Manager.Input>.Instance.MouseAxis;
    }

    public static bool GetWaterPosition(Vector3 _checkPos, out Vector3 _hitPosition)
    {
      if (!Singleton<Resources>.IsInstance())
      {
        _hitPosition = _checkPos;
        return false;
      }
      _hitPosition = _checkPos;
      _checkPos = Vector3.op_Addition(_checkPos, Vector3.op_Multiply(Vector3.get_up(), 10f));
      Ray ray;
      ((Ray) ref ray).\u002Ector(_checkPos, Vector3.get_down());
      LayerMask fishingLayerMask = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingLayerMask;
      int num1 = Physics.RaycastNonAlloc(ray, FishingManager.raycastHits, 50f, LayerMask.op_Implicit(fishingLayerMask));
      bool flag = false;
      for (int index = 0; index < num1; ++index)
      {
        RaycastHit raycastHit = FishingManager.raycastHits[index];
        GameObject gameObject = !Object.op_Inequality((Object) ((RaycastHit) ref raycastHit).get_collider(), (Object) null) ? ((Component) ((RaycastHit) ref raycastHit).get_transform())?.get_gameObject() : ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject();
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          int num2 = 1 << gameObject.get_layer();
          string tag = gameObject.get_tag();
          if (num2 == LayerMask.op_Implicit(fishingLayerMask) && tag == Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingMeshTagName)
          {
            if (!flag)
            {
              _hitPosition = ((RaycastHit) ref raycastHit).get_point();
              flag = true;
            }
            else if (_hitPosition.y < ((RaycastHit) ref raycastHit).get_point().y)
              _hitPosition = ((RaycastHit) ref raycastHit).get_point();
          }
        }
      }
      return flag;
    }

    public static bool CheckOnWater(Vector3 _checkPos, out Vector3 _hitPoint)
    {
      if (!Singleton<Resources>.IsInstance())
      {
        _hitPoint = _checkPos;
        return false;
      }
      _hitPoint = _checkPos;
      _checkPos = Vector3.op_Addition(_checkPos, Vector3.op_Multiply(Vector3.get_up(), 10f));
      Ray ray;
      ((Ray) ref ray).\u002Ector(_checkPos, Vector3.get_down());
      LayerMask fishingLayerMask = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingLayerMask;
      int num1 = Physics.RaycastNonAlloc(ray, FishingManager.raycastHits, 50f, LayerMask.op_Implicit(fishingLayerMask));
      bool flag = false;
      for (int index = 0; index < num1; ++index)
      {
        RaycastHit raycastHit = FishingManager.raycastHits[index];
        GameObject gameObject = !Object.op_Inequality((Object) ((RaycastHit) ref raycastHit).get_collider(), (Object) null) ? ((Component) ((RaycastHit) ref raycastHit).get_transform())?.get_gameObject() : ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject();
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          int num2 = 1 << gameObject.get_layer();
          string tag = gameObject.get_tag();
          if (num2 == LayerMask.op_Implicit(fishingLayerMask) && tag == Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingMeshTagName)
          {
            if (!flag)
            {
              _hitPoint = ((RaycastHit) ref raycastHit).get_point();
              flag = true;
            }
            else if (_hitPoint.y < ((RaycastHit) ref raycastHit).get_point().y)
              _hitPoint = ((RaycastHit) ref raycastHit).get_point();
          }
        }
      }
      return flag;
    }

    public static bool CheckOnWater(Vector3 _checkPos, ref Collider _collider)
    {
      if (!Singleton<Resources>.IsInstance())
        return false;
      _checkPos = Vector3.op_Addition(_checkPos, Vector3.op_Multiply(Vector3.get_up(), 10f));
      Ray ray;
      ((Ray) ref ray).\u002Ector(_checkPos, Vector3.get_down());
      LayerMask fishingLayerMask = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingLayerMask;
      int num1 = Physics.RaycastNonAlloc(ray, FishingManager.raycastHits, 50f, LayerMask.op_Implicit(fishingLayerMask));
      bool flag = false;
      Vector3 vector3 = Vector3.get_zero();
      for (int index = 0; index < num1; ++index)
      {
        RaycastHit raycastHit = FishingManager.raycastHits[index];
        GameObject gameObject = !Object.op_Inequality((Object) ((RaycastHit) ref raycastHit).get_collider(), (Object) null) ? ((Component) ((RaycastHit) ref raycastHit).get_transform())?.get_gameObject() : ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject();
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          int num2 = 1 << gameObject.get_layer();
          string tag = gameObject.get_tag();
          if (num2 == LayerMask.op_Implicit(fishingLayerMask) && tag == Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingMeshTagName)
          {
            if (!flag)
            {
              vector3 = ((RaycastHit) ref raycastHit).get_point();
              _collider = ((RaycastHit) ref raycastHit).get_collider();
              flag = true;
            }
            else if (vector3.y < ((RaycastHit) ref raycastHit).get_point().y)
            {
              vector3 = ((RaycastHit) ref raycastHit).get_point();
              _collider = ((RaycastHit) ref raycastHit).get_collider();
            }
          }
        }
      }
      return flag;
    }

    public static bool CheckOnWater(Vector3 _checkPos)
    {
      if (!Singleton<Resources>.IsInstance())
        return false;
      _checkPos = Vector3.op_Addition(_checkPos, Vector3.op_Multiply(Vector3.get_up(), 10f));
      Ray ray;
      ((Ray) ref ray).\u002Ector(_checkPos, Vector3.get_down());
      LayerMask fishingLayerMask = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingLayerMask;
      int num1 = Physics.RaycastNonAlloc(ray, FishingManager.raycastHits, 50f, LayerMask.op_Implicit(fishingLayerMask));
      bool flag = false;
      Vector3 vector3 = Vector3.get_zero();
      for (int index = 0; index < num1; ++index)
      {
        RaycastHit raycastHit = FishingManager.raycastHits[index];
        GameObject gameObject = !Object.op_Inequality((Object) ((RaycastHit) ref raycastHit).get_collider(), (Object) null) ? ((Component) ((RaycastHit) ref raycastHit).get_transform())?.get_gameObject() : ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject();
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          int num2 = 1 << gameObject.get_layer();
          string tag = gameObject.get_tag();
          if (num2 == LayerMask.op_Implicit(fishingLayerMask) && tag == Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingMeshTagName)
          {
            if (!flag)
            {
              vector3 = ((RaycastHit) ref raycastHit).get_point();
              flag = true;
            }
            else if (vector3.y < ((RaycastHit) ref raycastHit).get_point().y)
              vector3 = ((RaycastHit) ref raycastHit).get_point();
          }
        }
      }
      return flag;
    }

    public static Material GetLuxWaterMaterial(Vector3 _checkPos)
    {
      if (!Singleton<Resources>.IsInstance())
        return (Material) null;
      _checkPos = Vector3.op_Addition(_checkPos, Vector3.op_Multiply(Vector3.get_up(), 10f));
      Ray ray;
      ((Ray) ref ray).\u002Ector(_checkPos, Vector3.get_down());
      LayerMask luxWaterLayerMask = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.LuxWaterLayerMask;
      int num = Physics.RaycastNonAlloc(ray, FishingManager.raycastHits, 50f, LayerMask.op_Implicit(luxWaterLayerMask));
      for (int index = 0; index < num; ++index)
      {
        RaycastHit raycastHit = FishingManager.raycastHits[index];
        GameObject gameObject = !Object.op_Inequality((Object) ((RaycastHit) ref raycastHit).get_collider(), (Object) null) ? ((Component) ((RaycastHit) ref raycastHit).get_transform())?.get_gameObject() : ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject();
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          Renderer component = (Renderer) ((Component) gameObject.GetComponent<LuxWater_WaterVolume>())?.GetComponent<Renderer>();
          if (Object.op_Inequality((Object) component, (Object) null))
            return component.get_material();
        }
      }
      return (Material) null;
    }

    public bool CheckOnMoveAreaInWorld(Vector3 _checkPos)
    {
      float moveAreaRadius = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MoveAreaRadius;
      Vector3 vector3 = Vector3.op_Subtraction(_checkPos, this.moveArea.get_transform().get_position());
      return (double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) moveAreaRadius * (double) moveAreaRadius;
    }

    public bool CheckOnMoveAreaInLocal(Vector3 _checkPos)
    {
      float moveAreaRadius = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MoveAreaRadius;
      return (double) ((Vector3) ref _checkPos).get_sqrMagnitude() <= (double) moveAreaRadius * (double) moveAreaRadius;
    }

    public Vector3 ClampMoveAreaInWorld(Vector3 _position)
    {
      if (!this.CheckOnMoveAreaInWorld(_position))
        _position = Vector3.op_Addition(Vector3.ClampMagnitude(Vector3.op_Subtraction(_position, this.moveArea.get_transform().get_position()), Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MoveAreaRadius), this.moveArea.get_transform().get_position());
      return _position;
    }

    public Vector3 ClampMoveAreaInLocal(Vector3 _position)
    {
      if (!this.CheckOnMoveAreaInLocal(_position))
        _position = Vector3.ClampMagnitude(_position, Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MoveAreaRadius);
      return _position;
    }

    private static void DebugLog(
      Vector3 _originPos,
      bool _hitWater,
      bool _hitGround,
      Vector3 _waterHitPoint,
      Vector3 _groundHitPoint)
    {
    }

    public Vector3 moveAreaOriginLocalPosition { get; set; }

    private void Awake()
    {
      Vector3? nullable = this.axisArrowObject != null ? new Vector3?(this.axisArrowObject.get_localScale()) : new Vector3?();
      this.axisArrowOriginScale_ = !nullable.HasValue ? Vector3.get_one() : nullable.Value;
      Transform hitMoveArea = this.hitMoveArea;
      Vector3 zero = Vector3.get_zero();
      this.hitMoveArea.set_localEulerAngles(zero);
      Vector3 vector3 = zero;
      hitMoveArea.set_localPosition(vector3);
      this.moveAreaOriginLocalPosition = this.moveArea.get_transform().get_localPosition();
      Vector3 size = this.waterBox.get_size();
      Vector3 center = this.waterBox.get_center();
      size.x = (__Null) (double) (size.z = (__Null) (Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MoveAreaRadius * 4f));
      center.x = (__Null) (double) (center.z = (__Null) 0.0f);
      center.y = (__Null) -(size.y / 2.0);
      this.waterBox.set_center(center);
      this.waterBox.set_size(size);
    }

    private void OnEnable()
    {
      ((Component) this.uiCamera).get_gameObject().SetActive(true);
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnUpdate()));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryLateUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnLateUpdate()));
    }

    private void OnDisable()
    {
      ((Component) this.uiCamera).get_gameObject().SetActive(false);
      this.RemoveAllSE();
      this.RemoveAllParticle();
    }

    public void Initialize(AIProject.Player.Fishing _playerFishing)
    {
      this.Release();
      if (this.playerFishing != _playerFishing)
        this.playerFishing = _playerFishing;
      this.scene = FishingManager.FishingScene.SelectFood;
      this.playerInfo.Set(_playerFishing);
      this.InitializeLineRenderer();
      this.lure.Initialize(this, this.playerInfo.lurePos.get_transform(), this.fishingLine);
      Fish.FishHitEvent += new System.Action<Fish>(this.SceneToFishing);
      this.hitFish = (Fish) null;
      this.InitializeCamera();
      this.InitializeSprite();
    }

    private void SetEnable(Behaviour _be, bool _enable)
    {
      if (_be.get_enabled() == _enable)
        return;
      _be.set_enabled(_enable);
    }

    private void SetActive(Component _c, bool _a)
    {
      if (_c.get_gameObject().get_activeSelf() == _a)
        return;
      _c.get_gameObject().SetActive(_a);
    }

    private void SetActive(GameObject _g, bool _a)
    {
      if (_g.get_activeSelf() == _a)
        return;
      _g.SetActive(_a);
    }

    private void InitializeCamera()
    {
      Camera cameraComponent = this.playerFishing.player.CameraControl.CameraComponent;
      ((Component) this.uiCamera).get_transform().set_position(((Component) cameraComponent).get_transform().get_position());
      ((Component) this.uiCamera).get_transform().set_eulerAngles(((Component) cameraComponent).get_transform().get_eulerAngles());
      ((Component) this.uiCamera).get_gameObject().SetActive(true);
      Camera uiCamera = this.uiCamera;
      float fieldOfView = cameraComponent.get_fieldOfView();
      this.fishModelCamera.set_fieldOfView(fieldOfView);
      double num = (double) fieldOfView;
      uiCamera.set_fieldOfView((float) num);
      this.uiCamera.set_depth(cameraComponent.get_depth() + 1f);
      this.fishModelCamera.set_depth(this.uiCamera.get_depth() + 1f);
    }

    private void ChangeSpriteColor(int _mode)
    {
      if (this.outCircleSprite != null)
        this.outCircleSprite.ChangeColor(_mode);
      if (this.normalCircleSprite != null)
        this.normalCircleSprite.ChangeColor(_mode);
      if (this.criticalCircleSprite == null)
        return;
      this.criticalCircleSprite.ChangeColor(_mode);
    }

    private void SetImageFillAmount(Image _image, float _angle)
    {
      if (Object.op_Equality((Object) _image, (Object) null))
        return;
      _image.set_fillAmount(_angle / 360f);
      Vector3 localEulerAngles = ((Component) _image).get_transform().get_localEulerAngles();
      localEulerAngles.y = (__Null) ((double) _angle / 2.0);
      ((Component) _image).get_transform().set_localEulerAngles(localEulerAngles);
    }

    private void InitializeSprite()
    {
      ((Component) this.spriteRootObject).get_gameObject().SetActive(false);
      this.spriteRootObject.set_localEulerAngles(Vector3.get_zero());
      this.ChangeSpriteColor(0);
    }

    private void InitializeLineRenderer()
    {
      this.fishingLine.set_startWidth(0.02f);
      this.fishingLine.set_endWidth(0.02f);
      LineRenderer fishingLine = this.fishingLine;
      Color white = Color.get_white();
      this.fishingLine.set_endColor(white);
      Color color = white;
      fishingLine.set_startColor(color);
      this.fishingLine.set_useWorldSpace(true);
      ((Renderer) this.fishingLine).set_enabled(false);
    }

    public void Release()
    {
      this.FishAllDelete();
      this.RemoveAllSE();
      this.RemoveAllParticle();
      Lure.ThrowEndEvent = (System.Action) null;
      Fish.FishHitEvent = (System.Action<Fish>) null;
    }

    private void SetPlayerAnimation(AIProject.Player.Fishing.PoseID _poseID)
    {
      if (this.playerFishing == null)
        return;
      this.playerFishing.PlayAnimation(_poseID);
    }

    private void OnUpdate()
    {
      this.NullCheckFishList();
      this.moveArea.get_transform().set_localPosition(Vector3.op_Addition(this.moveAreaOriginLocalPosition, Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MoveAreaOffsetPosition));
      Vector3 position = this.moveArea.get_transform().get_position();
      position.y = (__Null) (double) this.playerFishing.MoveAreaPosY;
      this.moveArea.get_transform().set_position(position);
      switch (this.scene)
      {
        case FishingManager.FishingScene.WaitHit:
          if (!this.BackButtonDown)
            break;
          this.SelectFishFoodScene();
          break;
        case FishingManager.FishingScene.Fishing:
          this.UpdateSprite();
          this.CheckArrowInCircle();
          break;
      }
    }

    private void SetPositionAndAngle(Transform _t1, Transform _t2)
    {
      _t1.SetPositionAndRotation(_t2.get_position(), _t2.get_rotation());
    }

    private void SetCameraInfo(Camera _copy, Camera _source)
    {
      if (Object.op_Equality((Object) _copy, (Object) null) || Object.op_Equality((Object) _source, (Object) null))
        return;
      Transform transform = ((Component) _source).get_transform();
      ((Component) _copy).get_transform().SetPositionAndRotation(transform.get_position(), transform.get_rotation());
      _copy.set_fieldOfView(_source.get_fieldOfView());
    }

    private void SetPositionAndAngleLocal(
      Transform _t1,
      Vector3 _localPosition,
      Quaternion _localRotation)
    {
      _t1.set_localPosition(_localPosition);
      _t1.set_localRotation(_localRotation);
    }

    private void SetLocalAngleY(Transform _t, float _angleY)
    {
      Vector3 localEulerAngles = _t.get_localEulerAngles();
      localEulerAngles.y = (__Null) (double) _angleY;
      _t.set_localEulerAngles(localEulerAngles);
    }

    private void SetLocalAngleZ(Transform _t, float _angleZ)
    {
      Vector3 localEulerAngles = _t.get_localEulerAngles();
      localEulerAngles.z = (__Null) (double) _angleZ;
      _t.set_localEulerAngles(localEulerAngles);
    }

    private void OnLateUpdate()
    {
      Camera cameraComponent = Manager.Map.GetCameraComponent();
      Transform transform = !Object.op_Inequality((Object) cameraComponent, (Object) null) ? (Transform) null : ((Component) cameraComponent).get_transform();
      if (this.scene == FishingManager.FishingScene.Fishing)
      {
        this.SetCameraInfo(this.uiCamera, cameraComponent);
        this.SetPositionAndAngleLocal(this.spriteRootObject, ((Component) this.hitFish).get_transform().get_localPosition(), Quaternion.get_identity());
        this.circleRootObject.set_localEulerAngles(((Component) this.hitFish).get_transform().get_localEulerAngles());
        this.SetArrowAngle(this.ArrowPowerVector);
        Vector3 vector3 = Vector3.op_Subtraction(this.spriteRootObject.get_position(), transform.get_position());
        this.spriteRootObject.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(((Vector3) ref vector3).get_normalized(), Singleton<Resources>.Instance.FishingDefinePack.SystemParam.DistanceToCircle)));
      }
      this.soundRoot.set_position(Vector3.op_Addition(Vector3.op_Multiply(transform.get_forward(), Singleton<Resources>.Instance.FishingDefinePack.SystemParam.SoundRoodDistance), transform.get_position()));
      using (Dictionary<SEType, Dictionary<int, AudioSource>>.Enumerator enumerator1 = this.SETable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          using (Dictionary<int, AudioSource>.Enumerator enumerator2 = enumerator1.Current.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              AudioSource audioSource = enumerator2.Current.Value;
              if (Object.op_Implicit((Object) audioSource))
                ((Component) audioSource).get_transform().SetPositionAndRotation(this.soundRoot.get_position(), this.soundRoot.get_rotation());
            }
          }
        }
      }
    }

    public void ChangeFishScene(FishingManager.FishingScene _scene)
    {
      switch (this.scene)
      {
        case FishingManager.FishingScene.StartMotion:
          break;
        case FishingManager.FishingScene.WaitHit:
          break;
        case FishingManager.FishingScene.Fishing:
          break;
        case FishingManager.FishingScene.Failure:
          break;
        default:
          if (_scene != FishingManager.FishingScene.StartMotion)
          {
            if (_scene != FishingManager.FishingScene.SelectFood)
              break;
            this.scene = _scene;
            break;
          }
          this.scene = _scene;
          this.StartCoroutine(this.StartThrow());
          break;
      }
    }

    [DebuggerHidden]
    private IEnumerator StartThrow()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingManager.\u003CStartThrow\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void NullCheckFishList()
    {
      for (int index = 0; index < this.fishList.Count; ++index)
      {
        if (Object.op_Equality((Object) this.fishList[index], (Object) null))
        {
          this.fishList.RemoveAt(index);
          --index;
        }
      }
    }

    private void CreateFish()
    {
      if (this.createFishDisposable != null)
        return;
      IEnumerator _coroutine = this.CreateFishCoroutine();
      this.createFishDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    public Vector3 GetRandomPosOnMoveArea()
    {
      float num1 = (float) ((double) Random.get_value() * 3.14159274101257 * 2.0);
      float num2 = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MoveAreaRadius * Mathf.Sqrt(Random.get_value());
      return new Vector3(num2 * Mathf.Cos(num1), 0.0f, num2 * Mathf.Sin(num1));
    }

    public bool CreateFishEnable
    {
      get
      {
        return this.fishList.Count < Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishMaxNum && this.scene == FishingManager.FishingScene.WaitHit;
      }
    }

    [DebuggerHidden]
    private IEnumerator CreateFishCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingManager.\u003CCreateFishCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void CreateFishPattern(ValueTuple<int, int, int> _fishInfoValue, Vector3 _createPos)
    {
      Dictionary<int, Dictionary<int, FishInfo>> fishInfoTable = Singleton<Resources>.Instance.Fishing.FishInfoTable;
      Dictionary<int, FishInfo> dictionary;
      FishInfo _fishInfo;
      if (fishInfoTable.IsNullOrEmpty<int, Dictionary<int, FishInfo>>() || !fishInfoTable.TryGetValue((int) _fishInfoValue.Item2, out dictionary) || (fishInfoTable.IsNullOrEmpty<int, Dictionary<int, FishInfo>>() || !dictionary.TryGetValue((int) _fishInfoValue.Item3, out _fishInfo)))
        return;
      this.CreateFish(_fishInfo, _createPos);
    }

    private void CreateFish(FishInfo _fishInfo, Vector3 _createPos)
    {
      if (!_fishInfo.IsActive)
        return;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.FishPrefab, Vector3.get_zero(), Quaternion.get_identity());
      ((Object) gameObject).set_name(string.Format("Fish_{0:000}", (object) this.fishCreateNumber++));
      Fish component = (Fish) gameObject.GetComponent<Fish>();
      component.lure = this.lure;
      component.Initialize(this, _fishInfo);
      ((Component) component).get_transform().set_parent(this.moveArea.get_transform());
      _createPos.y = (__Null) 0.0;
      ((Component) component).get_transform().set_localPosition(_createPos);
      ((Component) component).get_transform().set_localEulerAngles(new Vector3(0.0f, 360f * Random.get_value(), 0.0f));
      ((Component) component).get_transform().set_localScale(Vector3.get_zero());
      component.startFadePosition = component.endFadePosition = ((Component) component).get_transform().get_localPosition();
      ref Vector3 local = ref component.startFadePosition;
      local.y = (__Null) (local.y - 5.0);
      ((Component) component).get_transform().set_localPosition(component.startFadePosition);
      this.fishList.Add(component);
    }

    [DebuggerHidden]
    private IEnumerator AddFish()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingManager.\u003CAddFish\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void FishAllDelete()
    {
      foreach (Fish fish in this.fishList)
      {
        if (Object.op_Inequality((Object) fish, (Object) null) && Object.op_Inequality((Object) ((Component) fish).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) fish).get_gameObject());
      }
      this.fishList.Clear();
    }

    private void SceneToWaitHit()
    {
      this.scene = FishingManager.FishingScene.WaitHit;
      this.lure.state = Lure.State.Float;
      this.CreateFish();
      this.StartCoroutine(this.AddFish());
    }

    public Vector2 SetArrowAngle(Vector2 _powerVector)
    {
      float arrowMaxPower = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.ArrowMaxPower;
      _powerVector = Vector2.ClampMagnitude(_powerVector, arrowMaxPower);
      this.SetArrowLocalAngleY(this.axisArrowObject, _powerVector);
      return _powerVector;
    }

    private void SetArrowLocalAngleY(Transform _arrow, Vector2 _powerVector)
    {
      if ((double) ((Vector2) ref _powerVector).get_magnitude() == 0.0)
        return;
      Vector3 localEulerAngles = _arrow.get_localEulerAngles();
      Vector2 vector2 = Vector2.op_Implicit(Quaternion.op_Multiply(Quaternion.Euler(0.0f, 0.0f, (float) -localEulerAngles.y), Vector2.op_Implicit(Vector2.op_Multiply(Vector2.get_up(), -1f))));
      float num1 = Vector2.SignedAngle(((Vector2) ref _powerVector).get_normalized(), vector2);
      float num2 = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.ArrowAddAngle * Time.get_deltaTime();
      if ((double) Mathf.Abs(num1) < (double) num2)
        num2 = Mathf.Abs(num1);
      float num3 = num2 * Mathf.Sign(num1);
      ref Vector3 local = ref localEulerAngles;
      local.y = (__Null) (local.y + (double) num3);
      _arrow.set_localRotation(Quaternion.Euler(localEulerAngles));
    }

    private Vector2 SetArrowAxis(Vector2 _axis)
    {
      if (this.playerFishing == null)
        return Vector2.get_zero();
      Transform transform = ((Component) this.playerFishing.player.CameraControl.CameraComponent).get_transform();
      Transform axisArrowObject = this.axisArrowObject;
      Vector3 position1 = transform.get_position();
      Vector3 position2 = axisArrowObject.get_position();
      return Vector2.op_Implicit(Quaternion.op_Multiply(Quaternion.Euler(0.0f, 0.0f, Vector3.SignedAngle(transform.get_forward(), Vector3.op_Subtraction(position2, position1), Vector3.get_up()) * -0.3f), Vector2.op_Implicit(_axis)));
    }

    private void SceneToFishing(Fish _hitFish)
    {
      this.scene = FishingManager.FishingScene.Fishing;
      this.hitFish = _hitFish;
      int level = Singleton<Manager.Map>.Instance.Player.PlayerData.FishingSkill.Level;
      FishingDefinePack.SystemParamGroup systemParam = Singleton<Resources>.Instance.FishingDefinePack.SystemParam;
      float num = Mathf.InverseLerp(1f, (float) systemParam.MaxLevel, (float) level);
      this.MaxDamage = Mathf.Lerp(systemParam.DefaultDamage, systemParam.MaxDamage, num);
      this.PlaySE(SEType.FishEatFood, ((Component) this).get_transform(), false, 0.0f);
      this.ArrowPowerVector = this.SetArrowAxis(Vector2.op_Multiply(Vector2.get_up(), Singleton<Resources>.Instance.FishingDefinePack.SystemParam.ArrowMaxPower));
      MapUIContainer.FishingUI.UseFishFood();
      this.SetPlayerAnimation(AIProject.Player.Fishing.PoseID.Hit);
      this.fishHeartPoint = this.hitFish.HeartPoint;
      this.lure.state = Lure.State.Hit;
      this.lure.HitFish = _hitFish;
      this.spriteRootObject.set_localPosition(((Component) this.hitFish).get_transform().get_localPosition());
      this.spriteRootObject.set_localEulerAngles(Vector3.get_zero());
      ((Component) this.spriteRootObject).get_gameObject().SetActive(true);
      this.fishingTimeCounter = Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingTimeLimit;
      this.prevWarningMode = false;
      this.ChangeSpriteColor(0);
      foreach (Fish fish in this.fishList)
      {
        if (Object.op_Inequality((Object) fish, (Object) null) && Object.op_Inequality((Object) _hitFish, (Object) fish))
          fish.ChangeState(Fish.State.Escape);
      }
    }

    private void SceneToSuccess()
    {
      if (this.FishGetEvent != null)
        this.FishGetEvent();
      this.SetPlayerAnimation(AIProject.Player.Fishing.PoseID.Success);
      FishInfo _info = this.hitFish.Get();
      this.SceneToResult();
      MapUIContainer.FishingUI.StartDrawResult(_info);
    }

    private void SceneToFailure()
    {
      this.hitFish.ChangeState(Fish.State.HitToEscape);
      this.fishList.Remove(this.hitFish);
      this.SceneToResult();
      FishingUI fishingUi = MapUIContainer.FishingUI;
      if (!fishingUi.HaveSomeFishFood())
      {
        this.playerFishing.MissAnimationEndEvent = (System.Action) (() =>
        {
          MapUIContainer.PushWarningMessage(Popup.Warning.Type.NonFishFood);
          this.playerFishing.MissAnimationEndEvent = (System.Action) null;
        });
        this.playerFishing.PlayMissEndAnimation();
        this.scene = FishingManager.FishingScene.Finish;
      }
      else if (fishingUi.FishFoodNum <= 0)
        this.SelectFishFoodScene();
      else
        this.StartCoroutine(this.ReplayFishing());
    }

    [DebuggerHidden]
    private IEnumerator ReplayFishing()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingManager.\u003CReplayFishing\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    private void SceneToResult()
    {
      ((Component) this.spriteRootObject).get_gameObject().SetActive(false);
      this.lure.state = Lure.State.None;
      this.playerInfo.fishingRod.get_transform().set_localEulerAngles(Vector3.get_zero());
      this.hitFish = (Fish) null;
    }

    private Rewired.Player Player0
    {
      get
      {
        return ReInput.get_players().GetPlayer(0);
      }
    }

    private void UpdateSprite()
    {
      FishingDefinePack.SystemParamGroup systemParam = Singleton<Resources>.Instance.FishingDefinePack.SystemParam;
      Vector2 vector2_1 = Vector2.op_Multiply(Vector2.op_Multiply(this.GetLeftStickAxis(), systemParam.DeviceArrowPowerScale), Time.get_deltaTime());
      Vector2 vector2_2 = Vector2.op_Multiply(Vector2.op_Multiply(this.GetMouseAxis(), systemParam.MouseArrowPowerScale), Time.get_deltaTime());
      Vector2 _axis = Vector2.op_Addition(Vector2.op_Division(Vector2.op_Addition(vector2_1, this.prevInputAxis), 2f), Vector2.op_Division(Vector2.op_Addition(vector2_2, this.prevMouseAxis), 2f));
      this.prevInputAxis = vector2_1;
      this.prevMouseAxis = vector2_2;
      if (0.0 < (double) ((Vector2) ref _axis).get_sqrMagnitude())
        this.ArrowPowerVector = Vector2.ClampMagnitude(Vector2.op_Addition(this.ArrowPowerVector, this.SetArrowAxis(this.SetArrowAxis(_axis))), systemParam.ArrowMaxPower);
      if (this.prevWarningMode != this.hitFish.WarningMode)
        this.ChangeSpriteColor(!this.hitFish.WarningMode ? 0 : 1);
      this.prevWarningMode = this.hitFish.WarningMode;
    }

    public float HeartPointScale
    {
      get
      {
        return Object.op_Equality((Object) this.hitFish, (Object) null) ? 1f : this.fishHeartPoint / this.hitFish.HeartPoint;
      }
    }

    public float TimeScale
    {
      get
      {
        return this.fishingTimeCounter / Singleton<Resources>.Instance.FishingDefinePack.SystemParam.FishingTimeLimit;
      }
    }

    private void CheckArrowInCircle()
    {
      FishingDefinePack.SystemParamGroup systemParam = Singleton<Resources>.Instance.FishingDefinePack.SystemParam;
      float num1 = systemParam.NormalDamageAngle / 2f;
      bool flag = false;
      float num2 = Mathf.Abs(Vector3.SignedAngle(this.hitFish.Forward, this.axisArrowObject.get_forward(), Vector3.get_up()));
      if ((double) num2 <= (double) num1)
      {
        if (!this.hitFish.WarningMode)
        {
          float num3 = this.MaxDamage * 0.5f * Time.get_deltaTime();
          this.fishHeartPoint -= Mathf.InverseLerp(num1, 0.0f, num2) * num3 + num3;
        }
        else
          flag = true;
      }
      else if (this.hitFish.WarningMode)
        this.fishHeartPoint -= this.MaxDamage * systemParam.AngryDamageScale * Time.get_deltaTime();
      if ((double) this.fishHeartPoint <= 0.0)
      {
        this.fishHeartPoint = 0.0f;
        this.scene = FishingManager.FishingScene.Success;
        this.SceneToSuccess();
      }
      else
      {
        this.fishingTimeCounter -= (flag ? systemParam.AngryCountDownScale : 1f) * Time.get_deltaTime();
        if ((double) this.fishingTimeCounter > 0.0)
          return;
        this.fishingTimeCounter = 0.0f;
        this.scene = FishingManager.FishingScene.Failure;
        this.SceneToFailure();
      }
    }

    public void SelectFishFoodScene()
    {
      switch (this.scene)
      {
        case FishingManager.FishingScene.WaitHit:
          this.scene = FishingManager.FishingScene.None;
          foreach (Fish fish in this.fishList)
            fish?.ChangeState(Fish.State.Escape);
          this.fishList.Clear();
          this.playerFishing.StopAnimationEndEvent = (System.Action) (() =>
          {
            this.scene = FishingManager.FishingScene.SelectFood;
            this.playerFishing.StopAnimationEndEvent = (System.Action) null;
          });
          this.playerFishing.PlayStopAnimation();
          break;
        case FishingManager.FishingScene.Success:
          this.scene = FishingManager.FishingScene.None;
          this.playerFishing.PlayStandbyMotion(true);
          this.scene = FishingManager.FishingScene.SelectFood;
          break;
        case FishingManager.FishingScene.Failure:
          this.scene = FishingManager.FishingScene.None;
          this.playerFishing.MissAnimationEndEvent = (System.Action) (() =>
          {
            this.scene = FishingManager.FishingScene.SelectFood;
            this.playerFishing.MissAnimationEndEvent = (System.Action) null;
          });
          this.playerFishing.PlayMissAnimation();
          break;
      }
    }

    public void EndFishing()
    {
      this.scene = FishingManager.FishingScene.Finish;
      this.Release();
      this.playerFishing.EndFishing();
    }

    private void OnDestroy()
    {
      this.Release();
    }

    private float AngleAbs(float _angle)
    {
      if ((double) _angle < 0.0)
        _angle = (float) ((double) _angle % 360.0 + 360.0);
      else if (360.0 <= (double) _angle)
        _angle %= 360f;
      return _angle;
    }

    private Vector3 AngleAbs(Vector3 _angle)
    {
      _angle.x = (__Null) (double) this.AngleAbs((float) _angle.x);
      _angle.y = (__Null) (double) this.AngleAbs((float) _angle.y);
      _angle.z = (__Null) (double) this.AngleAbs((float) _angle.z);
      return _angle;
    }

    public Vector2 MouseAxis
    {
      get
      {
        if (!((Rewired.Player.ControllerHelper) this.Player0.controllers).get_hasMouse())
          return Vector2.get_zero();
        Mouse mouse = ((Rewired.Player.ControllerHelper) this.Player0.controllers).get_Mouse();
        return new Vector2(((ControllerWithAxes) mouse).GetAxis(0), ((ControllerWithAxes) mouse).GetAxis(1));
      }
    }

    public void PlaySE(SEType _type, Transform _root, bool _loop, float _fadeTime = 0.0f)
    {
      int instanceId = ((Object) _root).GetInstanceID();
      int key = 0;
      AudioSource audioSource1 = (AudioSource) null;
      if (this.SETable.ContainsKey(_type) && this.SETable[_type].TryGetValue(instanceId, out audioSource1) && Object.op_Inequality((Object) audioSource1, (Object) null))
      {
        ((Component) audioSource1).get_transform().SetPositionAndRotation(this.soundRoot.get_position(), this.soundRoot.get_rotation());
        audioSource1.Play();
      }
      else
      {
        AudioSource audioSource2;
        if (!Singleton<Resources>.Instance.FishingDefinePack.SETable.TryGetValue(_type, out key) || !Object.op_Inequality((Object) (audioSource2 = Singleton<Resources>.Instance.SoundPack.Play(key, Sound.Type.GameSE3D, _fadeTime)), (Object) null))
          return;
        ((Component) audioSource2).get_transform().SetPositionAndRotation(this.soundRoot.get_position(), this.soundRoot.get_rotation());
        audioSource2.set_loop(_loop);
        if (!this.SETable.ContainsKey(_type))
          this.SETable[_type] = new Dictionary<int, AudioSource>();
        this.SETable[_type][instanceId] = audioSource2;
      }
    }

    public void StopSE(SEType _type, Transform _root)
    {
      AudioSource audioSource = (AudioSource) null;
      int instanceId = ((Object) _root).GetInstanceID();
      if (!this.SETable.ContainsKey(_type) || !this.SETable[_type].TryGetValue(instanceId, out audioSource))
        return;
      if (!Object.op_Implicit((Object) audioSource))
      {
        this.SETable[_type].Remove(instanceId);
        if (this.SETable[_type].Count > 0)
          return;
        this.SETable.Remove(_type);
      }
      else
        audioSource.Stop();
    }

    public void StopAllSE()
    {
      List<Tuple<SEType, int>> toRelease1 = ListPool<Tuple<SEType, int>>.Get();
      List<SEType> toRelease2 = ListPool<SEType>.Get();
      using (Dictionary<SEType, Dictionary<int, AudioSource>>.Enumerator enumerator1 = this.SETable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<SEType, Dictionary<int, AudioSource>> current1 = enumerator1.Current;
          using (Dictionary<int, AudioSource>.Enumerator enumerator2 = current1.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              KeyValuePair<int, AudioSource> current2 = enumerator2.Current;
              AudioSource audioSource = current2.Value;
              if (Object.op_Implicit((Object) audioSource))
                audioSource.Stop();
              else
                toRelease1.Add(new Tuple<SEType, int>(current1.Key, current2.Key));
            }
          }
        }
      }
      for (int index = 0; index < toRelease1.Count; ++index)
      {
        Tuple<SEType, int> tuple = toRelease1[index];
        this.SETable[tuple.Item1].Remove(tuple.Item2);
        if (this.SETable[tuple.Item1].Count == 0)
          toRelease2.Add(tuple.Item1);
      }
      for (int index = 0; index < toRelease2.Count; ++index)
        this.SETable.Remove(toRelease2[index]);
      ListPool<Tuple<SEType, int>>.Release(toRelease1);
      ListPool<SEType>.Release(toRelease2);
    }

    public void RemoveSE(SEType _type, Transform _root)
    {
      AudioSource audioSource = (AudioSource) null;
      int instanceId = ((Object) _root).GetInstanceID();
      if (!this.SETable.ContainsKey(_type) || !this.SETable[_type].TryGetValue(instanceId, out audioSource))
        return;
      if (Object.op_Implicit((Object) audioSource))
        Object.Destroy((Object) ((Component) audioSource).get_gameObject());
      this.SETable[_type].Remove(instanceId);
    }

    public void RemoveAllSE()
    {
      using (Dictionary<SEType, Dictionary<int, AudioSource>>.Enumerator enumerator1 = this.SETable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          using (Dictionary<int, AudioSource>.Enumerator enumerator2 = enumerator1.Current.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              AudioSource audioSource = enumerator2.Current.Value;
              if (Object.op_Implicit((Object) audioSource))
                Object.Destroy((Object) ((Component) audioSource).get_gameObject());
            }
          }
        }
      }
      this.SETable.Clear();
    }

    private Dictionary<int, Dictionary<int, Tuple<ParticleSystem, Transform>>> PlayEffectTable { get; set; }

    public ParticleSystem PlayParticle(int _id, Transform _t)
    {
      if (Object.op_Equality((Object) _t, (Object) null))
        return (ParticleSystem) null;
      int instanceId = ((Object) _t).GetInstanceID();
      Dictionary<int, Tuple<ParticleSystem, Transform>> dictionary;
      Tuple<ParticleSystem, Transform> tuple;
      if (this.PlayEffectTable.TryGetValue(_id, out dictionary) && dictionary.TryGetValue(instanceId, out tuple) && Object.op_Inequality((Object) tuple?.Item1, (Object) null))
      {
        this.SetActive((Component) tuple.Item1, true);
        tuple.Item1.Play(true);
        return tuple.Item1;
      }
      AssetBundleInfo assetBundleInfo;
      if (!Singleton<Resources>.Instance.Fishing.EffectTable.TryGetValue(_id, out assetBundleInfo))
        return (ParticleSystem) null;
      GameObject gameObject = CommonLib.LoadAsset<GameObject>((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset, false, (string) assetBundleInfo.manifest);
      if (Object.op_Equality((Object) gameObject, (Object) null))
        return (ParticleSystem) null;
      MapScene.AddAssetBundlePath((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.manifest);
      ParticleSystem component = (ParticleSystem) gameObject.GetComponent<ParticleSystem>();
      if (Object.op_Equality((Object) component, (Object) null))
        return (ParticleSystem) null;
      ParticleSystem particleSystem = (ParticleSystem) Object.Instantiate<ParticleSystem>((M0) component, _t);
      ((Component) particleSystem).get_transform().set_localPosition(Vector3.get_zero());
      this.SetActive((Component) particleSystem, true);
      particleSystem.Play(true);
      if (!this.PlayEffectTable.ContainsKey(_id))
        this.PlayEffectTable[_id] = new Dictionary<int, Tuple<ParticleSystem, Transform>>();
      this.PlayEffectTable[_id][instanceId] = new Tuple<ParticleSystem, Transform>(particleSystem, _t);
      return particleSystem;
    }

    public ParticleSystem PlayParticle(ParticleType _type, Transform _t)
    {
      return this.PlayParticle((int) _type, _t);
    }

    public void PlayDelayParticle(int _id, Transform _t, Func<bool> _enableFunc, float _delayTime)
    {
      IEnumerator _coroutine = this.PlayDelayParticleCoroutine(_id, _t, _enableFunc, _delayTime);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    public void PlayDelayParticle(
      ParticleType _type,
      Transform _t,
      Func<bool> _enableFunc,
      float _delayTime)
    {
      IEnumerator _coroutine = this.PlayDelayParticleCoroutine((int) _type, _t, _enableFunc, _delayTime);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator PlayDelayParticleCoroutine(
      int _id,
      Transform _t,
      Func<bool> _enableFunc,
      float _delayTime)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingManager.\u003CPlayDelayParticleCoroutine\u003Ec__Iterator4()
      {
        _delayTime = _delayTime,
        _enableFunc = _enableFunc,
        _id = _id,
        _t = _t,
        \u0024this = this
      };
    }

    public void StopParticle(int _id, Transform _t, ParticleSystemStopBehavior _stopBehavior = 0)
    {
      Dictionary<int, Tuple<ParticleSystem, Transform>> dictionary;
      if (Object.op_Equality((Object) _t, (Object) null) || !this.PlayEffectTable.TryGetValue(_id, out dictionary))
        return;
      int instanceId = ((Object) _t).GetInstanceID();
      Tuple<ParticleSystem, Transform> tuple;
      if (!dictionary.TryGetValue(instanceId, out tuple))
        return;
      if (Object.op_Inequality((Object) tuple.Item1, (Object) null))
      {
        tuple.Item1.Stop(true, _stopBehavior);
        this.SetActive((Component) tuple.Item1, false);
      }
      else
        this.PlayEffectTable[_id].Remove(instanceId);
    }

    public void StopParticle(
      ParticleType _type,
      Transform _t,
      ParticleSystemStopBehavior _stopBehavior = 0)
    {
      this.StopParticle((int) _type, _t, _stopBehavior);
    }

    public void StopAllParticle()
    {
      List<Tuple<int, int>> toRelease1 = ListPool<Tuple<int, int>>.Get();
      using (Dictionary<int, Dictionary<int, Tuple<ParticleSystem, Transform>>>.Enumerator enumerator1 = this.PlayEffectTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, Tuple<ParticleSystem, Transform>>> current1 = enumerator1.Current;
          using (Dictionary<int, Tuple<ParticleSystem, Transform>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              KeyValuePair<int, Tuple<ParticleSystem, Transform>> current2 = enumerator2.Current;
              if (Object.op_Inequality((Object) current2.Value.Item1, (Object) null))
              {
                current2.Value.Item1.Stop(true);
                this.SetActive((Component) current2.Value.Item1, false);
              }
              else
                toRelease1.Add(new Tuple<int, int>(current1.Key, current2.Key));
            }
          }
        }
      }
      List<int> toRelease2 = ListPool<int>.Get();
      for (int index = 0; index < toRelease1.Count; ++index)
      {
        Tuple<int, int> tuple = toRelease1[index];
        this.PlayEffectTable[tuple.Item1].Remove(tuple.Item2);
        if (this.PlayEffectTable[tuple.Item1].Count == 0)
          toRelease2.Add(tuple.Item1);
      }
      for (int index = 0; index < toRelease2.Count; ++index)
        this.PlayEffectTable.Remove(toRelease2[index]);
      ListPool<Tuple<int, int>>.Release(toRelease1);
      ListPool<int>.Release(toRelease2);
    }

    public void RemoveParticle(int _id, Transform _t)
    {
      Dictionary<int, Tuple<ParticleSystem, Transform>> dictionary;
      if (Object.op_Equality((Object) _t, (Object) null) || !this.PlayEffectTable.TryGetValue(_id, out dictionary))
        return;
      int instanceId = ((Object) _t).GetInstanceID();
      Tuple<ParticleSystem, Transform> tuple;
      if (!dictionary.TryGetValue(instanceId, out tuple))
        return;
      if (Object.op_Inequality((Object) tuple?.Item1, (Object) null))
        Object.Destroy((Object) ((Component) tuple.Item1).get_gameObject());
      this.PlayEffectTable.Remove(instanceId);
      if (this.PlayEffectTable[_id].Count != 0)
        return;
      this.PlayEffectTable.Remove(_id);
    }

    public void RemoveParticle(ParticleType _type, Transform _t)
    {
      this.RemoveParticle((int) _type, _t);
    }

    public void RemoveAllParticle()
    {
      using (Dictionary<int, Dictionary<int, Tuple<ParticleSystem, Transform>>>.Enumerator enumerator1 = this.PlayEffectTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          using (Dictionary<int, Tuple<ParticleSystem, Transform>>.Enumerator enumerator2 = enumerator1.Current.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              KeyValuePair<int, Tuple<ParticleSystem, Transform>> current = enumerator2.Current;
              if (current.Value != null && Object.op_Inequality((Object) current.Value.Item1, (Object) null))
                Object.Destroy((Object) ((Component) current.Value.Item1).get_gameObject());
            }
          }
        }
      }
      this.PlayEffectTable.Clear();
    }

    public enum FishingScene
    {
      SelectFood,
      StartMotion,
      WaitHit,
      Fishing,
      Success,
      Failure,
      Finish,
      None,
    }

    [Serializable]
    public class SpriteColorPair
    {
      [SerializeField]
      private Color _normalColor = Color.get_white();
      [SerializeField]
      private Color _warningColor = Color.get_white();
      [SerializeField]
      private SpriteRenderer _renderer;

      public SpriteRenderer Renderer
      {
        get
        {
          return this._renderer;
        }
      }

      public Color NormalColor
      {
        get
        {
          return this._normalColor;
        }
      }

      public Color WarningColor
      {
        get
        {
          return this._warningColor;
        }
      }

      public void ChangeColor(int _mode)
      {
        if (Object.op_Equality((Object) this._renderer, (Object) null))
          return;
        this._renderer.set_color(_mode != 0 ? this._warningColor : this._normalColor);
      }
    }

    [Serializable]
    public class ImageColorPair
    {
      [SerializeField]
      private Color _normalColor = Color.get_white();
      [SerializeField]
      private Color _warningColor = Color.get_white();
      [SerializeField]
      private Image _image;

      public Image Image
      {
        get
        {
          return this._image;
        }
      }

      public Color NormalColor
      {
        get
        {
          return this._normalColor;
        }
      }

      public Color WarningColor
      {
        get
        {
          return this._warningColor;
        }
      }

      public void ChangeColor(int _mode)
      {
        if (Object.op_Equality((Object) this._image, (Object) null))
          return;
        ((Graphic) this._image).set_color(_mode != 0 ? this._warningColor : this._normalColor);
      }
    }
  }
}
