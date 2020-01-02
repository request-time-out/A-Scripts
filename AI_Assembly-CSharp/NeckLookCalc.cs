// Decompiled with JetBrains decompiler
// Type: NeckLookCalc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class NeckLookCalc : MonoBehaviour
{
  public static bool isEnabled = true;
  public Transform rootNode;
  public NeckObject neckObj;
  public Transform controlObj;
  public Vector3 headLookVector;
  public Vector3 headUpVector;
  public NeckTypeState[] neckTypeStates;
  public float angleHRate;
  public float angleVRate;
  private int nowPtnNo;
  private bool initEnd;
  public float sorasiRate;
  public Quaternion fixAngle;
  private Quaternion fixAngleBackup;
  private float changeTypeTimer;
  public float changeTypeLeapTime;
  private NECK_LOOK_TYPE lookType;
  public Vector3 backupPos;

  public NeckLookCalc()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    if (this.initEnd)
      return;
    this.Init();
  }

  private void Start()
  {
    if (this.initEnd)
      return;
    this.Init();
  }

  public void Init()
  {
    if (Object.op_Equality((Object) this.rootNode, (Object) null))
      this.rootNode = ((Component) this).get_transform();
    Quaternion quaternion = Quaternion.Inverse(this.neckObj.neckTransform.get_parent().get_rotation());
    this.neckObj.referenceLookDir = Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion, this.rootNode.get_rotation()), ((Vector3) ref this.headLookVector).get_normalized());
    this.neckObj.referenceUpDir = Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion, this.rootNode.get_rotation()), ((Vector3) ref this.headUpVector).get_normalized());
    this.neckObj.angleH = 0.0f;
    this.neckObj.angleV = 0.0f;
    this.neckObj.dirUp = this.neckObj.referenceUpDir;
    this.neckObj.origRotation = (Quaternion) null;
    this.neckObj.origRotation = this.neckObj.neckTransform.get_localRotation();
    this.angleHRate = 0.0f;
    this.lookType = NECK_LOOK_TYPE.NO_LOOK;
    this.initEnd = true;
  }

  public void UpdateCall(int ptnNo)
  {
    if (ptnNo >= this.neckTypeStates.Length)
      ptnNo = 0;
    if (this.lookType == this.neckTypeStates[ptnNo].lookType)
      return;
    this.lookType = this.neckTypeStates[ptnNo].lookType;
    this.changeTypeTimer = 0.0f;
    if (this.lookType == NECK_LOOK_TYPE.TARGET)
      this.fixAngleBackup = this.neckObj.neckTransform.get_localRotation();
    else
      this.fixAngleBackup = this.fixAngle;
  }

  public void SetFixAngle(Quaternion angle)
  {
    this.fixAngleBackup = this.fixAngle = angle;
    if (this.neckObj == null)
      return;
    this.neckObj.neckTransform.set_localRotation(angle);
  }

  public void NeckUpdateCalc(Vector3 target, int ptnNo)
  {
    this.backupPos = target;
    if (!this.initEnd)
      return;
    this.nowPtnNo = ptnNo;
    if (!NeckLookCalc.isEnabled || (double) Time.get_deltaTime() == 0.0)
      return;
    NeckTypeState neckTypeState = this.neckTypeStates[ptnNo];
    this.changeTypeTimer += Time.get_deltaTime();
    if (this.lookType == NECK_LOOK_TYPE.NO_LOOK)
    {
      this.fixAngle = this.neckObj.neckTransform.get_localRotation();
      this.neckObj.neckTransform.set_localRotation(Quaternion.Lerp(this.fixAngleBackup, this.fixAngle, Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer)));
      if (!Object.op_Inequality((Object) this.controlObj, (Object) null))
        return;
      this.controlObj.set_localRotation(this.fixAngle);
      if (!((Component) this.controlObj).get_gameObject().get_activeSelf())
        return;
      ((Component) this.controlObj).get_gameObject().SetActive(false);
    }
    else
    {
      if (Object.op_Equality((Object) this.controlObj, (Object) null) && this.lookType == NECK_LOOK_TYPE.CONTROL)
        this.lookType = NECK_LOOK_TYPE.FIX;
      if (Object.op_Inequality((Object) this.controlObj, (Object) null))
      {
        if (this.lookType == NECK_LOOK_TYPE.CONTROL)
        {
          if (!((Component) this.controlObj).get_gameObject().get_activeSelf())
            ((Component) this.controlObj).get_gameObject().SetActive(true);
          ((Component) this.controlObj).get_gameObject().SetActive(true);
          this.fixAngle = this.controlObj.get_localRotation();
          this.neckObj.neckTransform.set_localRotation(Quaternion.Lerp(this.fixAngleBackup, this.fixAngle, Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer)));
          return;
        }
        if (((Component) this.controlObj).get_gameObject().get_activeSelf())
          ((Component) this.controlObj).get_gameObject().SetActive(false);
      }
      if (this.lookType == NECK_LOOK_TYPE.FIX)
      {
        this.neckObj.neckTransform.set_localRotation(Quaternion.Lerp(this.fixAngleBackup, this.fixAngle, Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer)));
        if (!Object.op_Inequality((Object) this.controlObj, (Object) null))
          return;
        this.controlObj.set_localRotation(this.fixAngle);
      }
      else
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(target, this.rootNode.get_position());
        if ((double) Vector3.Distance(target, this.rootNode.get_position()) < (double) this.neckTypeStates[ptnNo].nearDis)
        {
          vector3_1 = Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), this.neckTypeStates[ptnNo].nearDis);
          target = Vector3.op_Addition(this.rootNode.get_position(), vector3_1);
        }
        float num1 = Vector3.Angle(new Vector3((float) vector3_1.x, (float) this.rootNode.get_forward().y, (float) vector3_1.z), this.rootNode.get_forward());
        float num2 = Vector3.Angle(new Vector3((float) this.rootNode.get_forward().x, (float) vector3_1.y, (float) vector3_1.z), this.rootNode.get_forward());
        bool flag = false;
        if ((double) num1 > (double) this.neckTypeStates[ptnNo].hAngleLimit || (double) num2 > (double) this.neckTypeStates[ptnNo].vAngleLimit)
          flag = true;
        if (flag || this.lookType == NECK_LOOK_TYPE.FORWARD)
          target = Vector3.op_Addition(this.rootNode.get_position(), Vector3.op_Multiply(this.rootNode.get_forward(), this.neckTypeStates[ptnNo].forntTagDis));
        this.neckObj.neckTransform.set_localRotation(this.neckObj.origRotation);
        Quaternion rotation = this.neckObj.neckTransform.get_parent().get_rotation();
        Quaternion quaternion = Quaternion.Inverse(rotation);
        Vector3 vector3_2 = Vector3.op_Subtraction(target, this.neckObj.neckTransform.get_position());
        Vector3 normalized = ((Vector3) ref vector3_2).get_normalized();
        Vector3 dirB = Quaternion.op_Multiply(quaternion, normalized);
        float num3 = NeckLookCalc.AngleAroundAxis(this.neckObj.referenceLookDir, dirB, this.neckObj.referenceUpDir);
        Vector3 axis = Vector3.Cross(this.neckObj.referenceUpDir, dirB);
        float num4 = NeckLookCalc.AngleAroundAxis(Vector3.op_Subtraction(dirB, Vector3.Project(dirB, this.neckObj.referenceUpDir)), dirB, axis);
        float num5 = Mathf.Max(0.0f, Mathf.Abs(num3) - neckTypeState.thresholdAngleDifference) * Mathf.Sign(num3);
        float num6 = Mathf.Max(0.0f, Mathf.Abs(num4) - neckTypeState.thresholdAngleDifference) * Mathf.Sign(num4);
        float num7 = Mathf.Max(Mathf.Abs(num5) * Mathf.Abs(neckTypeState.bendingMultiplier), Mathf.Abs(num3) - neckTypeState.maxAngleDifference) * Mathf.Sign(num3) * Mathf.Sign(neckTypeState.bendingMultiplier);
        float num8 = Mathf.Max(Mathf.Abs(num6) * Mathf.Abs(neckTypeState.bendingMultiplier), Mathf.Abs(num4) - neckTypeState.maxAngleDifference) * Mathf.Sign(num4) * Mathf.Sign(neckTypeState.bendingMultiplier);
        float maxBendingAngle = neckTypeState.maxBendingAngle;
        float minBendingAngle = neckTypeState.minBendingAngle;
        float num9 = Mathf.Clamp(num7, minBendingAngle, maxBendingAngle);
        float num10 = Mathf.Clamp(num8, neckTypeState.upBendingAngle, neckTypeState.downBendingAngle);
        Vector3 vector3_3 = Vector3.Cross(this.neckObj.referenceUpDir, this.neckObj.referenceLookDir);
        if (this.lookType == NECK_LOOK_TYPE.AWAY)
        {
          float num11 = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.neckTypeStates[this.nowPtnNo].maxBendingAngle, -this.neckTypeStates[this.nowPtnNo].minBendingAngle, this.neckObj.angleH));
          float num12 = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.neckTypeStates[this.nowPtnNo].maxBendingAngle, -this.neckTypeStates[this.nowPtnNo].minBendingAngle, num9));
          float num13 = num11 - num12;
          num9 = (double) Mathf.Abs(num13) >= (double) this.sorasiRate ? this.neckObj.angleH : Mathf.Lerp(-this.neckTypeStates[this.nowPtnNo].maxBendingAngle, -this.neckTypeStates[this.nowPtnNo].minBendingAngle, Mathf.InverseLerp(-1f, 1f, (double) num13 >= 0.0 ? ((double) num13 <= 0.0 ? num12 + this.sorasiRate : ((double) num12 <= (double) this.sorasiRate ? num12 + this.sorasiRate : num12 - this.sorasiRate)) : ((double) num12 >= -(double) this.sorasiRate ? num12 - this.sorasiRate : num12 + this.sorasiRate)));
          num10 = -num10;
        }
        this.neckObj.angleH = Mathf.Lerp(this.neckObj.angleH, num9, Time.get_deltaTime() * neckTypeState.leapSpeed);
        this.neckObj.angleV = Mathf.Lerp(this.neckObj.angleV, num10, Time.get_deltaTime() * neckTypeState.leapSpeed);
        Vector3 vector3_4 = Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.AngleAxis(this.neckObj.angleH, this.neckObj.referenceUpDir), Quaternion.AngleAxis(this.neckObj.angleV, vector3_3)), this.neckObj.referenceLookDir);
        Vector3 referenceUpDir = this.neckObj.referenceUpDir;
        Vector3.OrthoNormalize(ref vector3_4, ref referenceUpDir);
        Vector3 vector3_5 = vector3_4;
        this.neckObj.dirUp = Vector3.Slerp(this.neckObj.dirUp, referenceUpDir, Time.get_deltaTime() * 5f);
        Vector3.OrthoNormalize(ref vector3_5, ref this.neckObj.dirUp);
        this.neckObj.neckTransform.set_rotation(Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.op_Multiply(rotation, Quaternion.LookRotation(vector3_5, this.neckObj.dirUp)), Quaternion.Inverse(Quaternion.op_Multiply(rotation, Quaternion.LookRotation(this.neckObj.referenceLookDir, this.neckObj.referenceUpDir)))), this.neckObj.neckTransform.get_rotation()));
        this.fixAngle = this.neckObj.neckTransform.get_localRotation();
        this.neckObj.neckTransform.set_localRotation(Quaternion.Lerp(this.fixAngleBackup, this.fixAngle, Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer)));
        if (Object.op_Inequality((Object) this.controlObj, (Object) null))
          this.controlObj.set_localRotation(this.fixAngle);
        this.backupPos = target;
        this.AngleHRateCalc();
        this.angleVRate = this.AngleVRateCalc();
      }
    }
  }

  public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
  {
    dirA = Vector3.op_Subtraction(dirA, Vector3.Project(dirA, axis));
    dirB = Vector3.op_Subtraction(dirB, Vector3.Project(dirB, axis));
    return Vector3.Angle(dirA, dirB) * ((double) Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0.0 ? 1f : -1f);
  }

  public void setEnable(bool setFlag)
  {
    NeckLookCalc.isEnabled = setFlag;
  }

  private void AngleHRateCalc()
  {
    if (this.neckObj == null)
      return;
    this.angleHRate = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.neckTypeStates[this.nowPtnNo].maxBendingAngle, -this.neckTypeStates[this.nowPtnNo].minBendingAngle, this.neckObj.angleH));
  }

  private float AngleVRateCalc()
  {
    if (this.neckObj == null)
      return 0.0f;
    return (double) this.neckTypeStates[this.nowPtnNo].downBendingAngle <= (double) this.neckTypeStates[this.nowPtnNo].upBendingAngle ? (0.0 <= (double) this.neckObj.angleV ? -Mathf.InverseLerp(0.0f, this.neckTypeStates[this.nowPtnNo].upBendingAngle, this.neckObj.angleV) : Mathf.InverseLerp(0.0f, this.neckTypeStates[this.nowPtnNo].downBendingAngle, this.neckObj.angleV)) : (0.0 <= (double) this.neckObj.angleV ? -Mathf.InverseLerp(0.0f, this.neckTypeStates[this.nowPtnNo].downBendingAngle, this.neckObj.angleV) : Mathf.InverseLerp(0.0f, this.neckTypeStates[this.nowPtnNo].upBendingAngle, this.neckObj.angleV));
  }

  public float GetAngleHRate()
  {
    return this.angleHRate;
  }

  public float GetAngleVRate()
  {
    return this.angleVRate;
  }
}
