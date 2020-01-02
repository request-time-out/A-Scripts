// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.Lure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Scene;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.MiniGames.Fishing
{
  public class Lure : MonoBehaviour
  {
    private Lure.State state_;
    private Fish hitFish_;
    private FishInfo hitFishInfo;
    private Vector3[] beziPos;
    private FishingManager fishingSystem;
    private LineRenderer fishingLine;
    public static Action ThrowEndEvent;
    private WaitForEndOfFrame waitForEndOfFrame;
    private GameObject bodyObject;
    private FloatingObject floatingObject;
    private Vector2 movePower;
    private Vector2 prevMouseMove;

    public Lure()
    {
      base.\u002Ector();
    }

    public Lure.State state
    {
      get
      {
        return this.state_;
      }
      set
      {
        if (this.state_ == value)
          return;
        if (this.state_ == Lure.State.Float)
        {
          this.StopFloatParticle();
          this.floatingObject.UseWaterBuoyancy = false;
        }
        this.state_ = value;
        if (this.state_ != Lure.State.Float)
          return;
        this.prevMouseMove = this.movePower = Vector2.get_zero();
        this.PlayFloatParticle();
        this.floatingObject.UseWaterBuoyancy = true;
      }
    }

    public Transform RootPos { get; private set; }

    public Fish HitFish
    {
      get
      {
        return this.hitFish_;
      }
      set
      {
        if (!Object.op_Inequality((Object) (this.hitFish_ = value), (Object) null))
          return;
        this.hitFishInfo = value.fishInfo;
      }
    }

    private FishingDefinePack.LureParamGroup Param
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.LureParam;
      }
    }

    private FishingDefinePack.SystemParamGroup SystemParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.SystemParam;
      }
    }

    private void Awake()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnLateUpdate()));
    }

    public void Initialize(FishingManager _fishingManager, Transform _rootPos, LineRenderer _line)
    {
      ActionItemInfo actionItemInfo;
      GameObject gameObject;
      if (Object.op_Equality((Object) this.bodyObject, (Object) null) && Singleton<Resources>.Instance.Map.EventItemList.TryGetValue(Singleton<Resources>.Instance.FishingDefinePack.IDInfo.LureEventItemID, out actionItemInfo) && Object.op_Inequality((Object) (gameObject = CommonLib.LoadAsset<GameObject>((string) actionItemInfo.assetbundleInfo.assetbundle, (string) actionItemInfo.assetbundleInfo.asset, false, (string) actionItemInfo.assetbundleInfo.manifest)), (Object) null))
      {
        MapScene.AddAssetBundlePath((string) actionItemInfo.assetbundleInfo.assetbundle, (string) actionItemInfo.assetbundleInfo.manifest);
        this.bodyObject = (GameObject) Object.Instantiate<GameObject>((M0) gameObject, Vector3.get_zero(), Quaternion.get_identity());
        this.bodyObject.get_transform().SetParent(((Component) this).get_transform(), false);
        this.floatingObject = this.bodyObject.GetOrAddComponent<FloatingObject>();
        this.floatingObject.fishingSystem = _fishingManager;
        this.floatingObject.WaterEnterChecker = new Func<Collider, bool>(this.WaterInOutCheck);
        this.floatingObject.WaterStayChecker = new Func<Collider, bool>(this.WaterInOutCheck);
        this.floatingObject.WaterExitChecker = new Func<Collider, bool>(this.WaterInOutCheck);
      }
      this.fishingSystem = _fishingManager;
      this.RootPos = _rootPos;
      ((Component) this).get_transform().set_parent(this.RootPos);
      Transform transform = ((Component) this).get_transform();
      Vector3 zero = Vector3.get_zero();
      ((Component) this).get_transform().set_localEulerAngles(zero);
      Vector3 vector3 = zero;
      transform.set_localPosition(vector3);
      this.fishingLine = _line;
      this.state = Lure.State.Have;
      if (!Object.op_Implicit((Object) this.floatingObject))
        return;
      this.floatingObject.UseWaterBuoyancy = false;
    }

    private bool WaterInOutCheck(Collider other)
    {
      return ((Component) other).CompareTag(this.SystemParam.LureWaterBoxTagName) && 1 << ((Component) other).get_gameObject().get_layer() == LayerMask.op_Implicit(this.SystemParam.LureWaterBoxLayerMask) && Object.op_Equality((Object) this.fishingSystem.WaterBox, (Object) other);
    }

    private void OnUpdate()
    {
      switch (this.state)
      {
        case Lure.State.Float:
          this.Float();
          break;
      }
    }

    private void OnLateUpdate()
    {
      if (this.state != Lure.State.Hit)
        return;
      if (Object.op_Equality((Object) this.HitFish, (Object) null))
        this.state = Lure.State.None;
      else
        ((Component) this).get_transform().set_position(((Component) this.HitFish).get_transform().get_position());
    }

    private void LateUpdate()
    {
      if (!Object.op_Inequality((Object) this.fishingLine, (Object) null) || !((Renderer) this.fishingLine).get_enabled())
        return;
      this.fishingLine.SetPosition(0, (!Object.op_Inequality((Object) this.bodyObject, (Object) null) ? ((Component) this).get_transform() : this.bodyObject.get_transform()).get_position());
      this.fishingLine.SetPosition(1, this.RootPos.get_position());
    }

    private void Float()
    {
      Input instance = Singleton<Input>.Instance;
      Vector2 leftStickAxis = instance.LeftStickAxis;
      Vector2 mouseAxis = instance.MouseAxis;
      Vector3 _position = ((Component) this).get_transform().get_localPosition();
      this.movePower = 0.0 >= (double) ((Vector2) ref leftStickAxis).get_sqrMagnitude() ? (0.0 < (double) ((Vector2) ref mouseAxis).get_sqrMagnitude() || 0.0 < (double) ((Vector2) ref this.prevMouseMove).get_sqrMagnitude() ? Vector2.op_Addition(this.movePower, Vector2.op_Multiply(Vector2.op_Subtraction(Vector2.ClampMagnitude(Vector2.op_Multiply(Vector2.op_Division(Vector2.op_Addition(mouseAxis, this.prevMouseMove), 2f), this.Param.MouseAxisScale), this.Param.FloatMoveMaxSpeed), this.movePower), 0.5f)) : Vector2.op_Addition(this.movePower, Vector2.op_Multiply(this.movePower, -0.5f))) : Vector2.op_Addition(this.movePower, Vector2.op_Multiply(Vector2.op_Subtraction(Vector2.op_Multiply(leftStickAxis, this.Param.FloatMoveMaxSpeed), this.movePower), 0.5f));
      Vector3 localEulerAngles = this.bodyObject.get_transform().get_localEulerAngles();
      float num = 60f;
      localEulerAngles.x = (__Null) ((double) Mathf.Abs((float) this.movePower.y) / (double) this.Param.FloatMoveMaxSpeed * (double) num * (double) Mathf.Sign((float) this.movePower.y));
      localEulerAngles.z = (__Null) ((double) Mathf.Abs((float) this.movePower.x) / (double) this.Param.FloatMoveMaxSpeed * (double) num * (double) Mathf.Sign((float) -this.movePower.x));
      this.bodyObject.get_transform().set_localEulerAngles(localEulerAngles);
      ref Vector3 local1 = ref _position;
      local1.x = (__Null) (local1.x + this.movePower.x * (double) Time.get_deltaTime());
      ref Vector3 local2 = ref _position;
      local2.z = (__Null) (local2.z + this.movePower.y * (double) Time.get_deltaTime());
      this.prevMouseMove = mouseAxis;
      bool flag = this.fishingSystem.CheckOnMoveAreaInLocal(((Component) this).get_transform().get_localPosition());
      if (flag)
      {
        ((Component) this).get_transform().set_localPosition(this.fishingSystem.ClampMoveAreaInLocal(_position));
      }
      else
      {
        if (flag)
          return;
        Vector3 vector3_1 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), this.fishingSystem.MoveArea.get_transform().get_position());
        Vector3 vector3_2 = Vector3.ClampMagnitude(vector3_1, this.SystemParam.MoveAreaRadius);
        Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, vector3_2);
        Vector3 vector3_4;
        ((Vector3) ref vector3_4).\u002Ector((float) _position.x, 0.0f, (float) _position.z);
        Vector3 normalized = ((Vector3) ref vector3_4).get_normalized();
        Vector3 vector3_5 = Vector3.op_Multiply(Vector3.op_Multiply(normalized, this.Param.FloatMoveMaxSpeed), Time.get_deltaTime());
        _position = (double) ((Vector3) ref vector3_5).get_sqrMagnitude() >= (double) ((Vector3) ref vector3_3).get_sqrMagnitude() ? Vector3.op_Multiply(normalized, this.SystemParam.MoveAreaRadius) : Vector3.op_Subtraction(_position, vector3_5);
        ((Component) this).get_transform().set_localPosition(_position);
      }
    }

    public void StartThrow()
    {
      this.StartCoroutine(this.Throw());
    }

    private Vector3 GetThrowPosition()
    {
      Transform transform = this.fishingSystem.MoveArea.get_transform();
      Vector3 position1 = transform.get_position();
      Vector3 _position = Vector3.op_Addition(position1, Vector3.op_Subtraction(transform.TransformPoint(this.Param.DropOffsetPosition), position1));
      _position.y = transform.get_position().y;
      Vector3 _checkPos1 = this.fishingSystem.ClampMoveAreaInWorld(_position);
      Collider _collider = (Collider) null;
      this.floatingObject.waterCollider = (Collider) this.fishingSystem.WaterBox;
      if (FishingManager.CheckOnWater(_checkPos1, ref _collider))
        return _checkPos1;
      Vector3 position2 = transform.get_position();
      Vector3 vector3_1 = Vector3.op_Addition(Vector3.op_Multiply(transform.get_forward(), (float) (-(double) this.SystemParam.MoveAreaRadius - 0.100000001490116)), position2);
      Vector3 vector3_2 = Vector3.op_Addition(Vector3.op_Multiply(transform.get_forward(), this.SystemParam.MoveAreaRadius - 0.1f), position2);
      vector3_1.y = (__Null) (double) (vector3_2.y = (__Null) (float) position2.y);
      for (int index1 = 1; index1 <= 4; ++index1)
      {
        for (int index2 = -1; index2 <= 1; index2 += 2)
        {
          float num = Mathf.Clamp((float) ((double) (index1 * index2) / 10.0 + 0.5), 0.0f, 1f);
          Vector3 _checkPos2 = this.fishingSystem.ClampMoveAreaInWorld(Vector3.Lerp(vector3_1, vector3_2, num));
          if (FishingManager.CheckOnWater(_checkPos2, ref _collider))
            return _checkPos2;
        }
      }
      return position2;
    }

    private bool IsFloating
    {
      get
      {
        return this.fishingSystem.scene == FishingManager.FishingScene.StartMotion || this.fishingSystem.scene == FishingManager.FishingScene.WaitHit;
      }
    }

    private Transform ParticleRoot
    {
      get
      {
        return ((Component) this).get_transform();
      }
    }

    private void PlayFloatParticle()
    {
      this.fishingSystem.PlayParticle(ParticleType.LureRippleS, this.ParticleRoot);
      this.fishingSystem.PlayParticle(ParticleType.LureRippleM, this.ParticleRoot);
    }

    private void StopFloatParticle()
    {
      this.fishingSystem.StopParticle(ParticleType.LureRippleS, this.ParticleRoot, (ParticleSystemStopBehavior) 0);
      this.fishingSystem.StopParticle(ParticleType.LureRippleM, this.ParticleRoot, (ParticleSystemStopBehavior) 0);
    }

    private void PlayDelayFloatParticle(float _delayTime)
    {
      this.fishingSystem.PlayDelayParticle(ParticleType.LureRippleS, this.ParticleRoot, (Func<bool>) (() => this.IsFloating), _delayTime);
      this.fishingSystem.PlayDelayParticle(ParticleType.LureRippleM, this.ParticleRoot, (Func<bool>) (() => this.IsFloating), _delayTime);
    }

    [DebuggerHidden]
    private IEnumerator Throw()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Lure.\u003CThrow\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void StartReturn(ParticleType _particleType)
    {
      if (this.state == Lure.State.Return)
        return;
      this.StartCoroutine(this.Return(_particleType));
    }

    [DebuggerHidden]
    private IEnumerator Return(ParticleType _particleType)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Lure.\u003CReturn\u003Ec__Iterator1()
      {
        _particleType = _particleType,
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
    }

    private Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
      float num = 1f - t;
      return Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(num * num, p0), Vector3.op_Multiply(2f * num * t, p1)), Vector3.op_Multiply(t * t, p2));
    }

    private Vector3 Bezier(Vector3[] p, float t)
    {
      if (p.IsNullOrEmpty<Vector3>() || p.Length < 3)
        return Vector3.get_zero();
      float num = 1f - t;
      return Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(num * num, p[0]), Vector3.op_Multiply(2f * num * t, p[1])), Vector3.op_Multiply(t * t, p[2]));
    }

    private float AngleAbs(float _angle)
    {
      if ((double) _angle < 0.0)
        _angle = (float) ((double) _angle % 360.0 + 360.0);
      else if ((double) _angle > 360.0)
        _angle %= 360f;
      return _angle;
    }

    public enum State
    {
      None,
      Have,
      Throw,
      Float,
      Hit,
      Return,
    }
  }
}
