// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FishHitBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using ReMotion;
using UnityEngine;

namespace AIProject.MiniGames.Fishing
{
  public class FishHitBehaviour : MonoBehaviour
  {
    [SerializeField]
    private Fish fish;
    private FishingManager fishingSystem;
    private bool firstHit;
    private float firstCounter;
    private float firstTimeLimit;
    private float angleCounter;
    private float angleTimeLimit;
    private float radiusCounter;
    private float radiusTimeLimit;
    private bool angleWait;
    private bool positionWait;
    private bool radiusWait;
    private float angleWaitCounter;
    private float angleWaitTimeLimit;
    private float positionWaitCounter;
    private float positionWaitTimeLimit;
    private float radiusWaitCounter;
    private float radiusWaitTimeLimit;
    private bool activeNextAngle;
    private bool activeNextPosition;
    private bool activeNextRadius;
    private float nextAngle;
    private Vector3 startPosition;
    private Vector3 nextPosition;
    private float startHitMoveAreaRadius;
    private float currentHitMoveAreaRadius;
    private float nextHitMoveAreaRadius;
    private float firstHitUseTime;

    public FishHitBehaviour()
    {
      base.\u002Ector();
    }

    private FishingDefinePack.FishHitParamGroup HitParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.FishParam.HitParam;
      }
    }

    private FishingDefinePack.SystemParamGroup SystemParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.SystemParam;
      }
    }

    private float DeltaTime
    {
      get
      {
        return Time.get_deltaTime();
      }
    }

    private void Awake()
    {
      if (Object.op_Implicit((Object) this.fish))
        return;
      this.fish = (Fish) ((Component) this).GetComponent<Fish>();
    }

    private void ResetAllCounter()
    {
      this.firstCounter = this.angleCounter = this.radiusCounter = 0.0f;
    }

    private void ResetAllTimeLimit()
    {
      this.firstTimeLimit = this.angleTimeLimit = this.radiusTimeLimit = 0.0f;
    }

    private void ResetAllWaitCounter()
    {
      this.angleWaitCounter = this.positionWaitCounter = this.radiusWaitCounter = 0.0f;
    }

    private void ResetAllWaitTimeLimit()
    {
      this.angleWaitTimeLimit = this.positionWaitTimeLimit = this.radiusWaitTimeLimit = 0.0f;
    }

    private void ResetAllWaitFlag()
    {
      this.angleWait = this.positionWait = this.radiusWait = true;
    }

    private void ResetAllActiveFlag()
    {
      this.activeNextAngle = this.activeNextPosition = this.activeNextRadius = false;
    }

    private void ResetAllSCNMember()
    {
      this.nextAngle = 0.0f;
      this.startPosition = this.nextPosition = Vector3.get_zero();
      this.startHitMoveAreaRadius = this.currentHitMoveAreaRadius = this.nextHitMoveAreaRadius = 0.0f;
    }

    private void ResetAllOther()
    {
      this.firstHitUseTime = 0.0f;
    }

    private void ResetAllMember()
    {
      this.ResetAllCounter();
      this.ResetAllTimeLimit();
      this.ResetAllWaitCounter();
      this.ResetAllWaitTimeLimit();
      this.ResetAllWaitFlag();
      this.ResetAllActiveFlag();
      this.ResetAllSCNMember();
      this.ResetAllOther();
    }

    public void StartHit()
    {
      this.fishingSystem = this.fish.fishingSystem;
      this.InitializeFirst();
    }

    private void InitializeFirst()
    {
      this.fishingSystem = this.fish.fishingSystem;
      this.firstHit = true;
      this.ResetAllMember();
      this.currentHitMoveAreaRadius = this.startHitMoveAreaRadius = this.nextHitMoveAreaRadius = this.HitParam.MoveAreaMaxRadius;
      float num1 = this.HitParam.MoveAreaAngle / 2f;
      float num2 = this.HitParam.MoveAreaMaxRadius + 1f;
      this.fishingSystem.HitMoveArea.get_forward();
      this.nextPosition = Vector3.op_Addition(this.fishingSystem.HitMoveArea.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, Random.Range(-num1, num1), 0.0f), Vector3.get_forward()), num2)), this.fishingSystem.HitMoveArea.get_position());
      this.startPosition = ((Component) this).get_transform().get_position();
      Vector3 vector3 = Vector3.op_Subtraction(this.nextPosition, this.startPosition);
      this.firstHitUseTime = ((Vector3) ref vector3).get_magnitude() / this.HitParam.MoveSpeed;
      this.firstTimeLimit = this.firstHitUseTime + this.HitParam.AngleMinTimeLimit;
    }

    private void ResetAngle()
    {
      float num1 = Random.Range(-90f, 90f);
      float num2 = this.Angle360To180((float) ((Component) this).get_transform().get_localEulerAngles().y);
      float num3 = Mathf.Abs(num1 - num2);
      if ((double) this.HitParam.NextMinAngle > (double) num3 || (double) num3 > (double) this.HitParam.NextMaxAngle)
        return;
      this.nextAngle = num1;
      this.angleTimeLimit = Random.Range(this.HitParam.AngleMinTimeLimit, this.HitParam.AngleMaxTimeLimit);
      this.activeNextAngle = true;
    }

    private void ResetPosition()
    {
      this.activeNextPosition = true;
    }

    private void ResetRadius()
    {
      this.currentHitMoveAreaRadius = this.nextHitMoveAreaRadius;
      Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), ((Component) this.fishingSystem.HitMoveArea).get_transform().get_position());
      this.startHitMoveAreaRadius = ((Vector3) ref vector3).get_magnitude();
      float num1 = Random.Range(this.HitParam.MoveAreaMinRadius, this.HitParam.MoveAreaMaxRadius);
      float num2 = Mathf.Abs(num1 - this.currentHitMoveAreaRadius);
      if ((double) this.HitParam.NextMinRadius > (double) num2 || (double) num2 >= (double) this.HitParam.NextMaxRadius)
        return;
      this.currentHitMoveAreaRadius = this.startHitMoveAreaRadius;
      this.nextHitMoveAreaRadius = num1;
      this.radiusTimeLimit = num2 / Random.Range(this.HitParam.RadiusMinSpeed, this.HitParam.RadiusMaxSpeed);
      this.activeNextRadius = true;
    }

    public void OnHit()
    {
      this.fishingSystem.HitMoveArea.set_localPosition(this.HitParam.MoveAreaOffsetPosition);
      if (this.firstHit)
      {
        this.UpdateFirst();
      }
      else
      {
        this.UpdateAngle();
        this.UpdatePosition();
        this.UpdateRadius();
      }
    }

    private void UpdateFirst()
    {
      ((Component) this).get_transform().set_position(this.GetOnHitMoveAreaRadius(Vector3.Lerp(this.startPosition, this.nextPosition, Mathf.InverseLerp(0.0f, this.firstHitUseTime, Mathf.Clamp(this.firstCounter, 0.0f, this.firstHitUseTime)))));
      float deltaTime = Time.get_deltaTime();
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector((float) ((Component) this).get_transform().get_forward().x, (float) ((Component) this).get_transform().get_forward().z);
      Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector((float) (this.nextPosition.x - ((Component) this).get_transform().get_position().x), (float) (this.nextPosition.z - ((Component) this).get_transform().get_position().z));
      Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
      float num1 = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
      Vector3 vector3 = Vector3.Cross(new Vector3((float) normalized1.x, 0.0f, (float) normalized1.y), new Vector3((float) normalized2.x, 0.0f, (float) normalized2.y));
      if (0.0 < (double) num1)
      {
        float num2 = this.HitParam.FirstAddAngle * Mathf.Sign((float) vector3.y) * deltaTime;
        if ((double) num1 <= (double) Mathf.Abs(num2))
          num2 = num1 * Mathf.Sign((float) vector3.y);
        Vector3 eulerAngles = ((Component) this).get_transform().get_eulerAngles();
        eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
        eulerAngles.y = (__Null) (double) this.AngleAbs((float) eulerAngles.y + num2);
        ((Component) this).get_transform().set_eulerAngles(eulerAngles);
      }
      if ((double) this.firstTimeLimit <= (double) this.firstCounter)
      {
        this.firstHit = false;
        this.radiusWaitTimeLimit = Random.Range(this.HitParam.RadiusMinWaitTimeLimit, this.HitParam.RadiusMaxWaitTimeLimit);
      }
      else
        this.firstCounter += deltaTime;
    }

    private void UpdateAngle()
    {
      float deltaTime = Time.get_deltaTime();
      if (this.angleWait)
      {
        if (!this.activeNextAngle)
          this.ResetAngle();
        if (this.activeNextAngle && (double) this.angleWaitTimeLimit <= (double) this.angleWaitCounter)
        {
          this.angleWaitCounter = 0.0f;
          this.angleWait = false;
        }
        else
          this.angleWaitCounter += deltaTime;
      }
      if (this.angleWait)
        return;
      Vector3 vector3_1 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, (float) ((Component) this).get_transform().get_localEulerAngles().y, 0.0f), Vector3.get_forward());
      Vector3 vector3_2 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, this.nextAngle, 0.0f), Vector3.get_forward());
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector((float) vector3_1.x, (float) vector3_1.z);
      Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector((float) vector3_2.x, (float) vector3_2.z);
      Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
      float num1 = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
      Vector3 vector3_3 = Vector3.Cross(new Vector3((float) normalized1.x, 0.0f, (float) normalized1.y), new Vector3((float) normalized2.x, 0.0f, (float) normalized2.y));
      float num2 = this.HitParam.AddAngle * Mathf.Sign((float) vector3_3.y) * deltaTime;
      if ((double) num1 <= (double) Mathf.Abs(num2))
        num2 = num1 * Mathf.Sign((float) vector3_3.y);
      Vector3 localEulerAngles = ((Component) this).get_transform().get_localEulerAngles();
      localEulerAngles.x = (__Null) (double) (localEulerAngles.z = (__Null) 0.0f);
      localEulerAngles.y = (__Null) (double) this.AngleAbs((float) localEulerAngles.y + num2);
      ((Component) this).get_transform().set_localEulerAngles(localEulerAngles);
      if ((double) this.angleTimeLimit <= (double) this.angleCounter)
      {
        this.angleWait = true;
        this.activeNextAngle = false;
        this.angleCounter = 0.0f;
      }
      else
        this.angleCounter += deltaTime;
    }

    private void UpdatePosition()
    {
      float deltaTime = Time.get_deltaTime();
      if (this.positionWait)
      {
        if (!this.activeNextPosition)
          this.ResetPosition();
        if (this.activeNextPosition && (double) this.positionWaitTimeLimit <= (double) this.positionWaitCounter)
        {
          this.positionWaitCounter = 0.0f;
          this.positionWait = false;
        }
        else
          this.positionWaitCounter += deltaTime;
      }
      if (this.positionWait)
        return;
      float num = this.HitParam.MoveSpeed * deltaTime;
      Vector3 _checkPos = this.GetOnHitMoveAreaAngle(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(((Component) this).get_transform().get_forward(), num)));
      Vector3 vector3_1 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), this.fishingSystem.HitMoveArea.get_position());
      Vector3 vector3_2 = Vector3.op_Subtraction(_checkPos, this.fishingSystem.HitMoveArea.get_position());
      float magnitude1 = ((Vector3) ref vector3_1).get_magnitude();
      float magnitude2 = ((Vector3) ref vector3_2).get_magnitude();
      float hitMoveAreaRadius = this.currentHitMoveAreaRadius;
      if (0.0 <= (double) hitMoveAreaRadius - (double) magnitude1)
        _checkPos = this.GetOnHitMoveAreaRadius(_checkPos);
      else if (0.0 <= (double) magnitude2 - (double) magnitude1)
      {
        _checkPos = Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref vector3_2).get_normalized(), magnitude1 - num), this.fishingSystem.HitMoveArea.get_position());
        Vector3 vector3_3 = Vector3.op_Subtraction(_checkPos, this.fishingSystem.HitMoveArea.get_position());
        float magnitude3 = ((Vector3) ref vector3_3).get_magnitude();
        if (0.0 <= (double) hitMoveAreaRadius - (double) magnitude3)
        {
          Vector3 vector3_4 = Vector3.op_Subtraction(_checkPos, this.fishingSystem.HitMoveArea.get_position());
          _checkPos = Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref vector3_4).get_normalized(), hitMoveAreaRadius), this.fishingSystem.HitMoveArea.get_position());
        }
      }
      ((Component) this).get_transform().set_position(_checkPos);
    }

    private void UpdateRadius()
    {
      float deltaTime = Time.get_deltaTime();
      if (this.radiusWait)
      {
        if (!this.activeNextRadius)
          this.ResetRadius();
        if (this.activeNextRadius && (double) this.radiusWaitTimeLimit <= (double) this.radiusWaitCounter)
        {
          this.radiusWaitCounter = 0.0f;
          this.radiusWait = false;
        }
        else
          this.radiusWaitCounter += deltaTime;
      }
      if (this.radiusWait)
        return;
      float time = Mathf.InverseLerp(0.0f, this.radiusTimeLimit, Mathf.Clamp(this.radiusCounter, 0.0f, this.radiusTimeLimit));
      if (this.HitParam.ChangeRadiusEasing)
        time = EasingFunctions.EaseOutQuart(time, 1f);
      this.currentHitMoveAreaRadius = Mathf.Lerp(this.startHitMoveAreaRadius, this.nextHitMoveAreaRadius, time);
      if ((double) this.radiusTimeLimit <= (double) this.radiusCounter)
      {
        this.radiusWait = true;
        this.radiusCounter = 0.0f;
        this.currentHitMoveAreaRadius = this.nextHitMoveAreaRadius;
        this.radiusWaitTimeLimit = Random.Range(this.HitParam.RadiusMinWaitTimeLimit, this.HitParam.RadiusMaxWaitTimeLimit);
        this.activeNextRadius = false;
      }
      else
        this.radiusCounter += deltaTime;
    }

    public bool CheckOnHitMoveAreaInWorld(Vector3 _checkPos)
    {
      Vector3 vector3 = Vector3.op_Subtraction(_checkPos, this.fishingSystem.HitMoveArea.get_position());
      return (double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) this.currentHitMoveAreaRadius * (double) this.currentHitMoveAreaRadius;
    }

    private Vector3 GetOnHitMoveAreaRadius(Vector3 _checkPos)
    {
      if (!this.CheckOnHitMoveAreaInWorld(_checkPos))
      {
        Vector3 vector3_1 = Vector3.op_Addition(Vector3.ClampMagnitude(Vector3.op_Subtraction(_checkPos, this.fishingSystem.HitMoveArea.get_position()), this.currentHitMoveAreaRadius), this.fishingSystem.HitMoveArea.get_position());
        Vector3 vector3_2 = Vector3.op_Subtraction(_checkPos, vector3_1);
        float num = this.HitParam.MoveSpeed * Time.get_deltaTime();
        _checkPos = (double) num >= (double) ((Vector3) ref vector3_2).get_magnitude() ? vector3_1 : Vector3.op_Subtraction(_checkPos, Vector3.op_Multiply(((Vector3) ref vector3_2).get_normalized(), num));
      }
      return _checkPos;
    }

    private Vector3 GetOnHitMoveAreaAngle(Vector3 _checkPos)
    {
      Transform hitMoveArea = this.fishingSystem.HitMoveArea;
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector((float) hitMoveArea.get_forward().x, (float) hitMoveArea.get_forward().z);
      Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector((float) (_checkPos.x - hitMoveArea.get_position().x), (float) (_checkPos.z - hitMoveArea.get_position().z));
      Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
      float num = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
      Vector3 vector3 = Vector3.Cross(new Vector3((float) normalized1.x, 0.0f, (float) normalized1.y), new Vector3((float) normalized2.x, 0.0f, (float) normalized2.y));
      if ((double) this.HitParam.MoveAreaAngle * 0.5 < (double) num)
        _checkPos = Vector3.op_Addition(hitMoveArea.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, (float) ((double) this.HitParam.MoveAreaAngle * (double) Mathf.Sign((float) vector3.y) * 0.5), 0.0f), Vector3.get_forward()), ((Vector2) ref vector2_2).get_magnitude())), hitMoveArea.get_position());
      return _checkPos;
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
  }
}
