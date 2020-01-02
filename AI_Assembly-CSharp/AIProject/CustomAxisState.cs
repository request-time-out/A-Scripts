// Decompiled with JetBrains decompiler
// Type: AIProject.CustomAxisState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Cinemachine;
using Manager;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AIProject
{
  [Serializable]
  public struct CustomAxisState
  {
    [NoSaveDuringPlay]
    [Tooltip("現在の軸")]
    public float value;
    [Tooltip("最大速度")]
    public float maxSpeed;
    [Tooltip("加速時間")]
    public float accelTime;
    [Tooltip("減速時間")]
    public float decelTime;
    [FormerlySerializedAs("axisID")]
    [Tooltip("入力軸 (ActionID)")]
    public ActionID inputAxisID;
    [FormerlySerializedAs("axisName")]
    [Tooltip("入力軸 (Input)")]
    public string inputAxisName;
    [NoSaveDuringPlay]
    [Tooltip("フレーム内の入力値")]
    public float inputAxisValue;
    [NoSaveDuringPlay]
    [FormerlySerializedAs("invertAxis")]
    [Tooltip("入力された軸の値を反転するか")]
    public bool invertInput;
    [Tooltip("最小値")]
    public float minValue;
    [Tooltip("最大値")]
    public float maxValue;
    [Tooltip("最小値・最大値の範囲を超えたらループするか")]
    public bool wrap;
    private float _currentSpeed;
    private const float Epsilon = 0.0001f;

    public CustomAxisState(
      float minValue_,
      float maxValue_,
      bool wrap_,
      bool rangeLocked,
      float maxSpeed_,
      float accelTime_,
      float decelTime_,
      ActionID id,
      string axisName,
      bool invert)
    {
      this.minValue = minValue_;
      this.maxValue = maxValue_;
      this.wrap = wrap_;
      this.ValueRangeLocked = rangeLocked;
      this.maxSpeed = maxSpeed_;
      this.accelTime = accelTime_;
      this.decelTime = decelTime_;
      this.value = (float) (((double) this.minValue + (double) this.maxValue) / 2.0);
      this.inputAxisID = id;
      this.inputAxisName = axisName;
      this.inputAxisValue = 0.0f;
      this.invertInput = invert;
      this._currentSpeed = 0.0f;
    }

    internal bool ValueRangeLocked { get; set; }

    public void Validate()
    {
      this.maxSpeed = Mathf.Max(0.0f, this.maxSpeed);
      this.accelTime = Mathf.Max(0.0f, this.accelTime);
      this.decelTime = Mathf.Max(0.0f, this.decelTime);
      this.maxValue = Mathf.Clamp(this.maxValue, this.minValue, this.maxValue);
    }

    public bool Update(float deltaTime)
    {
      if (Application.get_isPlaying())
      {
        try
        {
          Input instance = Singleton<Input>.Instance;
          if (instance.State == Input.ValidType.Action)
          {
            float? axis = instance?.GetAxis(this.inputAxisID);
            this.inputAxisValue = Mathf.Clamp((!axis.HasValue ? 0.0f : axis.Value) + Input.GetAxis(this.inputAxisName), -1f, 1f);
          }
          else
            this.inputAxisValue = 0.0f;
        }
        catch (ArgumentException ex)
        {
          Debug.LogException((Exception) ex);
          return false;
        }
      }
      float inputAxisValue = this.inputAxisValue;
      if (this.invertInput)
        inputAxisValue *= -1f;
      if ((double) this.maxSpeed > 9.99999974737875E-05)
      {
        float num1 = inputAxisValue * this.maxSpeed;
        if ((double) Mathf.Abs(num1) < 9.99999974737875E-05 || (double) Mathf.Sign(this._currentSpeed) == (double) Mathf.Sign(num1) && (double) Mathf.Abs(num1) < (double) Mathf.Abs(this._currentSpeed))
        {
          this._currentSpeed -= Mathf.Sign(this._currentSpeed) * Mathf.Min(Mathf.Abs(num1 - this._currentSpeed) / Mathf.Max(0.0001f, this.decelTime) * deltaTime, Mathf.Abs(this._currentSpeed));
        }
        else
        {
          float num2 = Mathf.Abs(num1 - this._currentSpeed) / Mathf.Max(0.0001f, this.accelTime);
          this._currentSpeed += Mathf.Sign(num1) * num2 * deltaTime;
          if ((double) Mathf.Sign(this._currentSpeed) == (double) Mathf.Sign(num1) && (double) Mathf.Abs(this._currentSpeed) > (double) Mathf.Abs(num1))
            this._currentSpeed = num1;
        }
      }
      float maxSpeed = this.GetMaxSpeed();
      this._currentSpeed = Mathf.Clamp(this._currentSpeed, -maxSpeed, maxSpeed);
      this.value += this._currentSpeed * deltaTime;
      if ((double) this.value > (double) this.maxValue || (double) this.value < (double) this.minValue)
      {
        if (this.wrap)
        {
          this.value = (double) this.value <= (double) this.maxValue ? this.maxValue + (this.value - this.minValue) : this.minValue + (this.value - this.maxValue);
        }
        else
        {
          this.value = Mathf.Clamp(this.value, this.minValue, this.maxValue);
          this._currentSpeed = 0.0f;
        }
      }
      return (double) Mathf.Abs(inputAxisValue) > 9.99999974737875E-05;
    }

    private float GetMaxSpeed()
    {
      float num1 = this.maxValue - this.minValue;
      if (!this.wrap && (double) num1 > 0.0)
      {
        float num2 = num1 / 10f;
        if ((double) this._currentSpeed > 0.0 && (double) this.maxValue - (double) this.value < (double) num2)
          return Mathf.Lerp(0.0f, this.maxSpeed, this.maxValue - this.value / num2);
        if ((double) this._currentSpeed < 0.0 && (double) this.value - (double) this.minValue < (double) num2)
          return Mathf.Lerp(0.0f, this.maxSpeed, (this.value - this.minValue) / num2);
      }
      return this.maxSpeed;
    }

    [Serializable]
    public struct Recentering
    {
      [Tooltip("If checked, will enable automatic recentering of the axis. If unchecked, recenting is disabled.")]
      public bool enabled;
      [FormerlySerializedAs("waitTime")]
      [Tooltip("If no user input has been detected on the axis, the axis will wait this long in seconds before recentering.")]
      public float waitTime;
      [Tooltip("Maximum angular speed of recentering. Will accelerate into and decelerate out of this.")]
      public float recenteringTime;
      private float _lastAxisInputTime;
      private float _recenteringVelocity;
      [SerializeField]
      [HideInInspector]
      [FormerlySerializedAs("_headingDefinition")]
      private int _legacyHeadingDefinition;
      [SerializeField]
      [HideInInspector]
      [FormerlySerializedAs("_velocityFilterStrength")]
      private int _legacyVelocityFilterStrength;

      public Recentering(bool enabled_, float recenterWaitTime, float recenteringSpeed)
      {
        this.enabled = enabled_;
        this.waitTime = recenterWaitTime;
        this.recenteringTime = recenteringSpeed;
        this._lastAxisInputTime = this._recenteringVelocity = 0.0f;
        this._legacyHeadingDefinition = this._legacyVelocityFilterStrength = 0;
      }

      public void Validate()
      {
        this.waitTime = Mathf.Max(0.0f, this.waitTime);
        this.recenteringTime = Mathf.Max(0.0f, this.recenteringTime);
      }

      public void CancelRecentering()
      {
        this._lastAxisInputTime = Time.get_time();
        this._recenteringVelocity = 0.0f;
      }

      public void DoRecentering(ref CustomAxisState axis, float deltaTime, float recenterTarget)
      {
        if (!this.enabled)
          return;
        if ((double) deltaTime < 0.0)
        {
          this.CancelRecentering();
          axis.value = recenterTarget;
        }
        else
        {
          if ((double) Time.get_time() <= (double) this._lastAxisInputTime + (double) this.waitTime)
            return;
          float num1 = this.recenteringTime / 3f;
          if ((double) num1 <= (double) deltaTime)
          {
            axis.value = recenterTarget;
          }
          else
          {
            float num2 = Mathf.DeltaAngle(axis.value, recenterTarget);
            float num3 = Mathf.Abs(num2);
            if ((double) num3 < 9.99999974737875E-05)
            {
              axis.value = recenterTarget;
              this._recenteringVelocity = 0.0f;
            }
            else
            {
              float num4 = deltaTime / num1;
              float num5 = Mathf.Sign(num2) * Mathf.Min(num3, num3 * num4);
              float num6 = num5 * this._recenteringVelocity;
              if ((double) num5 < 0.0 && (double) num6 < 0.0 || (double) num5 > 0.0 && (double) num6 > 0.0)
                num5 = this._recenteringVelocity + num5 * num4;
              axis.value += num5;
              this._recenteringVelocity = num5;
            }
          }
        }
      }

      internal bool LegacyUpdate(ref int heading, ref int velocityFilter)
      {
        bool flag;
        if (this._legacyHeadingDefinition != -1 && this._legacyVelocityFilterStrength != -1)
        {
          heading = this._legacyHeadingDefinition;
          velocityFilter = this._legacyVelocityFilterStrength;
          this._legacyHeadingDefinition = this._legacyVelocityFilterStrength = -1;
          flag = true;
        }
        else
          flag = false;
        return flag;
      }
    }
  }
}
