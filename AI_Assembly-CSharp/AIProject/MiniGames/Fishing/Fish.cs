// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.Fish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Scene;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.MiniGames.Fishing
{
  public class Fish : MonoBehaviour
  {
    [NonSerialized]
    public Lure lure;
    [SerializeField]
    private FishHitBehaviour fishHitBehaviour;
    [SerializeField]
    private FishLureSearcher lureSearcher;
    private float escapeSpeed;
    private Fish.State state_;
    private Vector3 targetPosition;
    public static Action<Fish> FishHitEvent;
    private Vector3 originScale;
    private Vector3 startScale;
    private GameObject bodyObj;
    private Animator fishAnim;
    private Transform eyeT;
    private float stateCounter;
    private float stateTimeLimit;
    private bool activeNextPosition;
    private float destroyCounter;
    private float destroyTimeLimit;
    [NonSerialized]
    public Vector3 startFadePosition;
    [NonSerialized]
    public Vector3 endFadePosition;
    [NonSerialized]
    public Vector3 startFadeOutScale;

    public Fish()
    {
      base.\u002Ector();
    }

    public bool OnSearch
    {
      get
      {
        return this.state == Fish.State.Wait || this.state == Fish.State.SearchNextPos || this.state == Fish.State.Swim || this.state == Fish.State.FollowLure;
      }
    }

    private FishingDefinePack.FishParamGroup Param
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.FishParam;
      }
    }

    private FishingDefinePack.SystemParamGroup SystemParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.SystemParam;
      }
    }

    public Vector3 Forward
    {
      get
      {
        return ((Component) this).get_transform().get_forward();
      }
    }

    public Fish.State state
    {
      get
      {
        return this.state_;
      }
      set
      {
        if (this.state_ == value)
          return;
        Fish.State state = this.state_;
        this.state_ = value;
        this.ChangedState(state);
      }
    }

    public float HeartPoint
    {
      get
      {
        return (float) this.fishInfo.HeartPoint;
      }
    }

    public FishingManager fishingSystem { get; private set; }

    public FishInfo fishInfo { get; private set; }

    public bool WarningMode { get; private set; }

    private void Awake()
    {
      this.originScale = this.startScale = ((Component) this).get_transform().get_localScale();
      this.fishHitBehaviour = (FishHitBehaviour) ((Component) this).GetComponent<FishHitBehaviour>();
    }

    private void OnEnable()
    {
      this.activeNextPosition = false;
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnDisable()
    {
      this.state = Fish.State.None;
    }

    private void OnDestroy()
    {
    }

    public void Initialize(FishingManager _fishingSystem, FishInfo _fishInfo)
    {
      this.state = Fish.State.None;
      this.fishInfo = _fishInfo;
      this.fishingSystem = _fishingSystem;
      this.activeNextPosition = false;
      if (Object.op_Inequality((Object) this.bodyObj, (Object) null))
        Object.Destroy((Object) this.bodyObj);
      ((Component) this).get_transform().set_localScale(this.originScale);
      Tuple<AssetBundleInfo, RuntimeAnimatorController, string> tuple;
      if (Singleton<Resources>.Instance.Fishing.FishBodyTable.TryGetValue(this.fishInfo.SizeID, out tuple))
      {
        // ISSUE: variable of the null type
        __Null assetbundle = tuple.Item1.assetbundle;
        // ISSUE: variable of the null type
        __Null asset = tuple.Item1.asset;
        // ISSUE: variable of the null type
        __Null manifest = tuple.Item1.manifest;
        GameObject gameObject;
        if (Object.op_Inequality((Object) (gameObject = CommonLib.LoadAsset<GameObject>((string) assetbundle, (string) asset, false, (string) manifest)), (Object) null))
        {
          AssetBundleInfo assetBundleInfo = tuple.Item1;
          MapScene.AddAssetBundlePath((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.manifest);
          this.bodyObj = (GameObject) Object.Instantiate<GameObject>((M0) gameObject, Vector3.get_zero(), Quaternion.get_identity());
          this.fishAnim = (Animator) this.bodyObj.GetComponent<Animator>();
          if (Object.op_Inequality((Object) this.fishAnim, (Object) null))
            this.fishAnim.set_runtimeAnimatorController(tuple.Item2);
          this.eyeT = this.bodyObj.get_transform().FindLoop(tuple.Item3)?.get_transform() ?? ((Component) this).get_transform();
          Renderer componentInChildren = (Renderer) this.bodyObj.GetComponentInChildren<Renderer>(true);
          float createOffsetHeight = Singleton<Resources>.Instance.FishingDefinePack.FishParam.CreateOffsetHeight;
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          {
            Bounds bounds1 = componentInChildren.get_bounds();
            Vector3 center = ((Bounds) ref bounds1).get_center();
            Bounds bounds2 = componentInChildren.get_bounds();
            Vector3 extents = ((Bounds) ref bounds2).get_extents();
            Vector3 position = this.bodyObj.get_transform().get_position();
            ref Vector3 local1 = ref position;
            local1.z = local1.z - (extents.z + center.z);
            ref Vector3 local2 = ref position;
            local2.y = (__Null) (local2.y - (extents.y + center.y + 0.25));
            ref Vector3 local3 = ref position;
            local3.y = (__Null) (local3.y + ((double) createOffsetHeight - extents.y));
            this.bodyObj.get_transform().set_position(position);
          }
          else
          {
            Vector3 position = this.bodyObj.get_transform().get_position();
            ref Vector3 local = ref position;
            local.y = (__Null) (local.y + (double) createOffsetHeight);
            this.bodyObj.get_transform().set_position(position);
          }
          this.bodyObj.get_transform().SetParent(((Component) this).get_transform(), true);
          if (this.fishAnim != null)
          {
            this.fishAnim.CrossFadeInFixedTime(this.Param.AnimLoopName, 0.2f, 0, 0.0f);
            goto label_12;
          }
          else
            goto label_12;
        }
      }
      this.fishAnim = (Animator) null;
      this.eyeT = ((Component) this).get_transform();
      this.bodyObj = (GameObject) null;
label_12:
      this.lureSearcher.ResetFollowPercentage();
      this.state = Fish.State.FadeIn;
      this.ResetDestroyCount();
    }

    public void SetWaitOrSwim()
    {
      this.state = (double) Random.get_value() >= 0.5 ? Fish.State.Swim : Fish.State.Wait;
    }

    public void ChangeState(Fish.State _state)
    {
      this.state = _state;
    }

    private void ChangedState(Fish.State _prev)
    {
      this.stateCounter = 0.0f;
      if (_prev == Fish.State.Hit)
      {
        this.fishingSystem.StopParticle(ParticleType.FishHitNormal, ((Component) this).get_transform(), (ParticleSystemStopBehavior) 0);
        this.fishingSystem.StopParticle(ParticleType.FishHitAngry, ((Component) this).get_transform(), (ParticleSystemStopBehavior) 0);
        this.fishingSystem.StopSE(SEType.FishResist, ((Component) this).get_transform());
      }
      switch (this.state)
      {
        case Fish.State.FadeIn:
          this.StartFadeIn();
          break;
        case Fish.State.FadeOut:
          this.StartFadeOut();
          break;
        case Fish.State.Wait:
          this.StartWait();
          break;
        case Fish.State.SearchNextPos:
          this.StartSearchNextPos();
          break;
        case Fish.State.Swim:
          this.StartSwim();
          break;
        case Fish.State.FollowLure:
          this.StartFollowLure();
          break;
        case Fish.State.Hit:
          this.StartHit();
          break;
        case Fish.State.Escape:
          this.StartEscape();
          break;
        case Fish.State.HitToEscape:
          this.StartHitToEscape();
          break;
        case Fish.State.Get:
          this.StartGet();
          break;
      }
    }

    private void OnUpdate()
    {
      this.DestroyCountDown();
      switch (this.state)
      {
        case Fish.State.FadeIn:
          this.OnFadeIn();
          break;
        case Fish.State.FadeOut:
          this.OnFadeOut();
          break;
        case Fish.State.Wait:
          this.OnWait();
          break;
        case Fish.State.SearchNextPos:
          this.OnSearchNextPos();
          break;
        case Fish.State.Swim:
          this.OnSwim();
          break;
        case Fish.State.FollowLure:
          this.OnFollowLure();
          break;
        case Fish.State.Hit:
          this.OnHit();
          break;
        case Fish.State.Escape:
          this.OnEscape();
          break;
        case Fish.State.HitToEscape:
          this.OnHitToEscape();
          break;
        case Fish.State.Get:
          this.OnGet();
          break;
      }
    }

    private void ResetDestroyCount()
    {
      this.destroyCounter = 0.0f;
      this.destroyTimeLimit = Random.Range(this.Param.DestroyMinTime, this.Param.DestroyMaxTime);
    }

    private float VecToRand(Vector2 _vec)
    {
      return Random.Range((float) _vec.x, (float) _vec.y);
    }

    private bool ValueOnVec(float _value, Vector2 _vec)
    {
      return _vec.x <= (double) _value && (double) _value < _vec.y;
    }

    private void DestroyCountDown()
    {
      if (this.state < Fish.State.Wait || Fish.State.Hit <= this.state)
        return;
      if (this.state == Fish.State.FollowLure)
      {
        this.ResetDestroyCount();
      }
      else
      {
        this.destroyCounter += Time.get_deltaTime();
        if ((double) this.destroyTimeLimit > (double) this.destroyCounter)
          return;
        this.state = Fish.State.FadeOut;
      }
    }

    private void ToDestroy()
    {
      this.state = Fish.State.None;
      this.fishingSystem.RemoveSE(SEType.FishResist, ((Component) this).get_transform());
      this.fishingSystem.RemoveParticle(ParticleType.FishHitNormal, ((Component) this).get_transform());
      this.fishingSystem.RemoveParticle(ParticleType.FishHitAngry, ((Component) this).get_transform());
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }

    private void StartFadeIn()
    {
      this.stateTimeLimit = 1f;
    }

    private void OnFadeIn()
    {
      this.stateCounter += Time.get_deltaTime();
      if ((double) this.stateTimeLimit < (double) this.stateCounter)
      {
        ((Component) this).get_transform().set_localScale(this.originScale);
        ((Component) this).get_transform().set_localPosition(this.endFadePosition);
        this.SetWaitOrSwim();
      }
      else
      {
        float num = Mathf.InverseLerp(0.0f, this.stateTimeLimit, this.stateCounter);
        ((Component) this).get_transform().set_localScale(Vector3.Lerp(Vector3.get_zero(), this.originScale, num));
        ((Component) this).get_transform().set_localPosition(Vector3.Lerp(this.startFadePosition, this.endFadePosition, num));
      }
    }

    private void StartFadeOut()
    {
      this.startFadeOutScale = ((Component) this).get_transform().get_localScale();
      this.startFadePosition = this.endFadePosition = ((Component) this).get_transform().get_localPosition();
      ref Vector3 local = ref this.endFadePosition;
      local.y = (__Null) (local.y - 5.0);
      this.stateTimeLimit = 1f;
    }

    private void OnFadeOut()
    {
      this.stateCounter += Time.get_deltaTime();
      if ((double) this.stateTimeLimit < (double) this.stateCounter)
      {
        ((Component) this).get_transform().set_localScale(Vector3.get_zero());
        this.ToDestroy();
      }
      else
      {
        float num = Mathf.InverseLerp(0.0f, this.stateTimeLimit, this.stateCounter);
        ((Component) this).get_transform().set_localScale(Vector3.Lerp(this.startFadeOutScale, Vector3.get_zero(), num));
        ((Component) this).get_transform().set_localPosition(Vector3.Lerp(this.startFadePosition, this.endFadePosition, num));
      }
    }

    private void StartWait()
    {
      this.stateTimeLimit = Random.Range(2f, 6f);
      this.activeNextPosition = false;
    }

    private void OnWait()
    {
      if (!this.activeNextPosition)
        this.SetTargetPos();
      this.stateCounter += Time.get_deltaTime();
      if ((double) this.stateTimeLimit > (double) this.stateCounter || !this.activeNextPosition)
        return;
      this.state = Fish.State.Swim;
    }

    private void StartSearchNextPos()
    {
    }

    private void OnSearchNextPos()
    {
      if (!this.activeNextPosition)
        this.SetTargetPos();
      if (!this.activeNextPosition)
        return;
      this.state = Fish.State.Swim;
    }

    private void StartSwim()
    {
      if (this.activeNextPosition)
        return;
      this.state = Fish.State.SearchNextPos;
    }

    private void OnSwim()
    {
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector((float) ((Component) this).get_transform().get_forward().x, (float) ((Component) this).get_transform().get_forward().z);
      Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector((float) (this.targetPosition.x - ((Component) this).get_transform().get_position().x), (float) (this.targetPosition.z - ((Component) this).get_transform().get_position().z));
      Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
      float num1 = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
      if (0.0 < (double) num1)
      {
        Vector3 vector3 = Vector3.Cross(new Vector3((float) normalized1.x, 0.0f, (float) normalized1.y), new Vector3((float) normalized2.x, 0.0f, (float) normalized2.y));
        Vector3 localEulerAngles = ((Component) this).get_transform().get_localEulerAngles();
        float num2 = this.Param.SwimAddAngle * Time.get_deltaTime() * Mathf.Sign((float) vector3.y);
        if ((double) num1 <= (double) Mathf.Abs(num2))
        {
          ref Vector3 local = ref localEulerAngles;
          local.y = (__Null) (local.y + (double) num1 * (double) Mathf.Sign((float) vector3.y));
        }
        else
        {
          ref Vector3 local = ref localEulerAngles;
          local.y = (__Null) (local.y + (double) num2);
        }
        localEulerAngles.y = (__Null) (double) this.AngleAbs((float) localEulerAngles.y);
        ((Component) this).get_transform().set_localEulerAngles(localEulerAngles);
      }
      ((Component) this).get_transform().set_position(this.fishingSystem.ClampMoveAreaInWorld(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.Param.SwimSpeed), Time.get_deltaTime()))));
      if ((double) this.GetWorldDistanceFishToTarget() > (double) this.Param.SwimStopDistance)
        return;
      this.state = Fish.State.Wait;
    }

    private bool FishOnArea()
    {
      float distanceFishToLure = this.GetWorldDistanceFishToLure();
      if (0.0 >= (double) distanceFishToLure)
        return true;
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector((float) ((Component) this).get_transform().get_forward().x, (float) ((Component) this).get_transform().get_forward().z);
      Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector((float) (((Component) this.lure).get_transform().get_position().x - ((Component) this).get_transform().get_position().x), (float) (((Component) this.lure).get_transform().get_position().z - ((Component) this).get_transform().get_position().z));
      Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
      float num1 = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
      Vector3 vector3_1 = Vector3.Cross(new Vector3((float) normalized1.x, 0.0f, (float) normalized1.y), new Vector3((float) normalized2.x, 0.0f, (float) normalized2.y));
      float num2 = this.Param.FollowAddAngle * Mathf.Sign((float) vector3_1.y) * Time.get_deltaTime();
      Vector3 eulerAngles = ((Component) this).get_transform().get_eulerAngles();
      if ((double) num1 <= (double) Mathf.Abs(num2))
        num2 = num1 * Mathf.Sign((float) vector3_1.y);
      eulerAngles.y = (__Null) (double) this.AngleAbs((float) eulerAngles.y + num2);
      ((Component) this).get_transform().set_eulerAngles(eulerAngles);
      Vector3 vector3_2 = Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.Param.FollowSpeed * Time.get_deltaTime());
      if ((double) distanceFishToLure * (double) distanceFishToLure <= (double) ((Vector3) ref vector3_2).get_sqrMagnitude() || (double) distanceFishToLure <= (double) this.Param.HitDistance)
      {
        ((Component) this).get_transform().set_position(((Component) this.lure).get_transform().get_position());
        return true;
      }
      ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_position(), vector3_2));
      return false;
    }

    private void StartFollowLure()
    {
      this.activeNextPosition = false;
    }

    private void OnFollowLure()
    {
      if (!this.FishOnArea())
        return;
      ((Component) this).get_transform().set_position(((Component) this.lure).get_transform().get_position());
      this.state = Fish.State.Hit;
      if (Fish.FishHitEvent == null)
        return;
      Fish.FishHitEvent(this);
    }

    private void StartHit()
    {
      if (this.fishAnim != null)
        this.fishAnim.CrossFadeInFixedTime(this.Param.AnimHitName, 0.2f, 0, 0.0f);
      this.fishingSystem.PlaySE(SEType.FishResist, ((Component) this).get_transform(), true, 0.0f);
      this.StartCoroutine(this.WarningCoroutine());
      this.fishHitBehaviour.StartHit();
    }

    private void OnHit()
    {
      this.fishHitBehaviour.OnHit();
    }

    [DebuggerHidden]
    private IEnumerator WarningCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Fish.\u003CWarningCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void SetEscapeAngle()
    {
      this.escapeSpeed = this.Param.EscapeSpeed;
    }

    private void Escape()
    {
      if ((double) this.stateCounter < (double) this.Param.EscapeFadeTime)
      {
        Transform transform = ((Component) this).get_transform();
        transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.escapeSpeed * Time.get_deltaTime())));
        ((Component) this).get_transform().set_localScale(Vector3.Lerp(this.startScale, Vector3.get_zero(), Mathf.InverseLerp(0.0f, this.Param.EscapeFadeTime, this.stateCounter)));
        this.stateCounter += Time.get_deltaTime();
      }
      else
        this.ToDestroy();
    }

    private void StartEscape()
    {
      this.startScale = ((Component) this).get_transform().get_localScale();
      ((Component) this).get_transform().LookAt(((Component) this.lure).get_transform());
      Vector3 localEulerAngles = ((Component) this).get_transform().get_localEulerAngles();
      localEulerAngles.y = (__Null) (double) this.AngleAbs((float) (localEulerAngles.y + 180.0));
      localEulerAngles.z = (__Null) 0.0;
      ((Component) this).get_transform().set_localEulerAngles(localEulerAngles);
      this.SetEscapeAngle();
    }

    private bool CheckOnWater(float _distance)
    {
      return FishingManager.CheckOnWater(Vector3.op_Addition(((Component) this).get_transform().TransformDirection(Vector3.op_Multiply(Vector3.get_forward(), _distance)), ((Component) this).get_transform().get_position()));
    }

    private void OnEscape()
    {
      this.Escape();
    }

    private void StartHitToEscape()
    {
      this.startScale = ((Component) this).get_transform().get_localScale();
      this.SetEscapeAngle();
    }

    private void OnHitToEscape()
    {
      this.Escape();
    }

    private void StartGet()
    {
    }

    private void OnGet()
    {
    }

    private int GetRandom(int _max, int _notNum)
    {
      int num = _notNum;
      while (num == _notNum)
        num = Random.Range(0, _max);
      return num;
    }

    public FishInfo Get()
    {
      this.state = Fish.State.Get;
      this.ToDestroy();
      return this.fishInfo;
    }

    public bool CheckLureInAngleFindArea()
    {
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector((float) ((Component) this).get_transform().get_forward().x, (float) ((Component) this).get_transform().get_forward().z);
      Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector((float) (((Component) this.lure).get_transform().get_position().x - this.eyeT.get_position().x), (float) (((Component) this.lure).get_transform().get_position().z - this.eyeT.get_position().z));
      Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
      return (double) (Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f) * 2.0 <= (double) this.Param.FindAngle;
    }

    private void SetTargetPos()
    {
      Vector3 vector3_1 = this.fishingSystem.MoveArea.get_transform().TransformPoint(this.fishingSystem.GetRandomPosOnMoveArea());
      Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, ((Component) this).get_transform().get_position());
      if ((double) ((Vector3) ref vector3_2).get_sqrMagnitude() < (double) this.Param.SwimDistance * (double) this.Param.SwimDistance)
        return;
      this.targetPosition = vector3_1;
      this.activeNextPosition = true;
    }

    private float GetWorldDistanceFishToTarget()
    {
      return Vector3.Distance(this.targetPosition, ((Component) this).get_transform().get_position());
    }

    private float GetWorldDistanceFishToLure()
    {
      return Vector3.Distance(((Component) this.lure).get_transform().get_position(), ((Component) this).get_transform().get_position());
    }

    public float GetWorldDistanceFishEyeToLure()
    {
      Vector3 position = this.eyeT.get_position();
      position.y = ((Component) this).get_transform().get_position().y;
      return Vector3.Distance(((Component) this.lure).get_transform().get_position(), position);
    }

    private float AngleAbs(float _angle)
    {
      if ((double) _angle < 0.0)
        _angle = (float) ((double) _angle % 360.0 + 360.0);
      else if ((double) _angle > 360.0)
        _angle %= 360f;
      return _angle;
    }

    private float Angle360To180(float _angle)
    {
      _angle = this.AngleAbs(_angle);
      if (180.0 < (double) _angle)
        _angle -= 360f;
      return _angle;
    }

    public enum State
    {
      None,
      FadeIn,
      FadeOut,
      Wait,
      SearchNextPos,
      Swim,
      FollowLure,
      Hit,
      Escape,
      HitToEscape,
      Get,
    }
  }
}
