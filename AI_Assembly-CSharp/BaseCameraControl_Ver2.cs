// Decompiled with JetBrains decompiler
// Type: BaseCameraControl_Ver2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCameraControl_Ver2 : MonoBehaviour
{
  public Transform transBase;
  public Transform targetObj;
  public float xRotSpeed;
  public float yRotSpeed;
  public float zoomSpeed;
  public float moveSpeed;
  public float keySpeed;
  public float noneTargetDir;
  public float rateSpeedMin;
  public float rateSpeedMax;
  public bool isLimitPos;
  public float limitPos;
  public bool isLimitDir;
  public float limitDir;
  public float limitFovMin;
  public float limitFov;
  public BaseCameraControl_Ver2.NoCtrlFunc NoCtrlCondition;
  public BaseCameraControl_Ver2.NoCtrlFunc ZoomCondition;
  public BaseCameraControl_Ver2.NoCtrlFunc KeyCondition;
  public readonly int CONFIG_SIZE;
  [SerializeField]
  protected BaseCameraControl_Ver2.CameraData CamDat;
  protected BaseCameraControl_Ver2.Config cameraType;
  protected bool[] isDrags;
  protected BaseCameraControl_Ver2.ResetData CamReset;
  protected bool isInit;
  private const float INIT_FOV = 23f;
  protected CapsuleCollider viewCollider;
  protected float rateAddSpeed;

  public BaseCameraControl_Ver2()
  {
    base.\u002Ector();
    this.CamDat.Fov = 23f;
    this.CamReset.Fov = 23f;
  }

  public Camera thisCamera { get; protected set; }

  public bool isControlNow { get; protected set; }

  public BaseCameraControl_Ver2.Config CameraType
  {
    get
    {
      return this.cameraType;
    }
    set
    {
      this.cameraType = value;
    }
  }

  public float CameraInitFov
  {
    get
    {
      return this.CamReset.Fov;
    }
    set
    {
      this.CamReset.Fov = value;
      this.CamDat.Fov = value;
      if (!Object.op_Inequality((Object) this.thisCamera, (Object) null))
        return;
      this.thisCamera.set_fieldOfView(value);
    }
  }

  public Vector3 TargetPos
  {
    get
    {
      return this.CamDat.Pos;
    }
    set
    {
      this.CamDat.Pos = value;
    }
  }

  public Vector3 CameraAngle
  {
    get
    {
      return this.CamDat.Rot;
    }
    set
    {
      ((Component) this).get_transform().set_rotation(Quaternion.Euler(value));
      this.CamDat.Rot = value;
    }
  }

  public Vector3 Rot
  {
    set
    {
      this.CamDat.Rot = value;
    }
  }

  public Vector3 CameraDir
  {
    get
    {
      return this.CamDat.Dir;
    }
    set
    {
      this.CamDat.Dir = value;
    }
  }

  public float CameraFov
  {
    get
    {
      return this.CamDat.Fov;
    }
    set
    {
      this.CamDat.Fov = value;
      if (!Object.op_Inequality((Object) this.thisCamera, (Object) null))
        return;
      this.thisCamera.set_fieldOfView(value);
    }
  }

  public void Reset(int mode)
  {
    int num1 = 0;
    int num2 = mode;
    int num3 = num1;
    int num4 = num3 + 1;
    if (num2 == num3)
    {
      this.CamDat.Copy(this.CamReset);
      if (!Object.op_Inequality((Object) this.thisCamera, (Object) null))
        return;
      this.thisCamera.set_fieldOfView(this.CamDat.Fov);
    }
    else
    {
      int num5 = mode;
      int num6 = num4;
      int num7 = num6 + 1;
      if (num5 == num6)
      {
        this.CamDat.Pos = this.CamReset.Pos;
      }
      else
      {
        int num8 = mode;
        int num9 = num7;
        int num10 = num9 + 1;
        if (num8 == num9)
        {
          ((Component) this).get_transform().set_rotation(this.CamReset.RotQ);
        }
        else
        {
          int num11 = mode;
          int num12 = num10;
          int num13 = num12 + 1;
          if (num11 != num12)
            return;
          this.CamDat.Dir = this.CamReset.Dir;
        }
      }
    }
  }

  protected bool InputTouchProc()
  {
    if (Input.get_touchCount() < 1)
      return false;
    float num1 = 10f * Time.get_deltaTime();
    if (Input.get_touchCount() == 3)
      this.Reset(0);
    else if (Input.get_touchCount() == 1)
    {
      Touch touch = ((IEnumerable<Touch>) Input.get_touches()).First<Touch>();
      TouchPhase phase = ((Touch) ref touch).get_phase();
      if (phase != null && phase == 1)
      {
        float num2 = 0.1f;
        float num3 = 0.01f;
        Vector3 zero = Vector3.get_zero();
        if (this.cameraType == BaseCameraControl_Ver2.Config.Rotation)
        {
          ref Vector3 local1 = ref zero;
          local1.y = (__Null) (local1.y + ((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num2);
          ref Vector3 local2 = ref zero;
          local2.x = (__Null) (local2.x - ((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num2);
          Vector3 vector3 = zero;
          Quaternion rotation = ((Component) this).get_transform().get_rotation();
          Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
          ((Component) this).get_transform().set_rotation(Quaternion.Euler(Vector3.op_Addition(vector3, eulerAngles)));
        }
        else if (this.cameraType == BaseCameraControl_Ver2.Config.Translation)
        {
          ref Vector3 local1 = ref this.CamDat.Dir;
          local1.z = (__Null) (local1.z - ((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num3);
          ref Vector3 local2 = ref this.CamDat.Pos;
          local2.y = (__Null) (local2.y + ((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num3);
        }
        else if (this.cameraType == BaseCameraControl_Ver2.Config.MoveXY)
        {
          zero.x = (__Null) (((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num3);
          zero.y = (__Null) (((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num3);
          ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(zero));
        }
        else if (this.cameraType == BaseCameraControl_Ver2.Config.MoveXZ)
        {
          zero.x = (__Null) (((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num3);
          zero.z = (__Null) (((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num3);
          ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(zero));
        }
      }
    }
    return true;
  }

  protected bool InputMouseWheelZoomProc()
  {
    bool flag = false;
    float num = Input.GetAxis("Mouse ScrollWheel") * this.zoomSpeed;
    if ((double) num != 0.0)
    {
      ref Vector3 local = ref this.CamDat.Dir;
      local.z = (__Null) (local.z + (double) num);
      this.CamDat.Dir.z = (__Null) (double) Mathf.Min(0.0f, (float) this.CamDat.Dir.z);
      flag = true;
    }
    return flag;
  }

  protected virtual bool InputMouseProc()
  {
    bool flag = false;
    bool[] flagArray = new bool[this.CONFIG_SIZE];
    flagArray[1] = Input.GetMouseButton(0);
    flagArray[2] = Input.GetMouseButton(1);
    flagArray[3] = Input.GetMouseButton(2);
    flagArray[0] = Input.GetMouseButton(0) && Input.GetMouseButton(1);
    for (int index = 0; index < this.CONFIG_SIZE; ++index)
    {
      if (flagArray[index])
        this.isDrags[index] = true;
    }
    for (int index = 0; index < this.CONFIG_SIZE; ++index)
    {
      if (this.isDrags[index] && !flagArray[index])
        this.isDrags[index] = false;
    }
    float axis1 = Input.GetAxis("Mouse X");
    float axis2 = Input.GetAxis("Mouse Y");
    for (int index = 0; index < this.CONFIG_SIZE; ++index)
    {
      if (this.isDrags[index])
      {
        Vector3 zero = Vector3.get_zero();
        switch (index)
        {
          case 0:
            zero.x = (__Null) ((double) axis1 * (double) this.moveSpeed * (double) this.rateAddSpeed);
            zero.z = (__Null) ((double) axis2 * (double) this.moveSpeed * (double) this.rateAddSpeed);
            if (Object.op_Inequality((Object) this.transBase, (Object) null))
            {
              ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
              local.Pos = Vector3.op_Addition(local.Pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(zero)));
              break;
            }
            ref BaseCameraControl_Ver2.CameraData local1 = ref this.CamDat;
            local1.Pos = Vector3.op_Addition(local1.Pos, ((Component) this).get_transform().TransformDirection(zero));
            break;
          case 1:
            ref Vector3 local2 = ref zero;
            local2.y = (__Null) (local2.y + (double) axis1 * (double) this.xRotSpeed * (double) this.rateAddSpeed);
            ref Vector3 local3 = ref zero;
            local3.x = (__Null) (local3.x - (double) axis2 * (double) this.yRotSpeed * (double) this.rateAddSpeed);
            this.CamDat.Rot.y = (__Null) ((this.CamDat.Rot.y + zero.y) % 360.0);
            this.CamDat.Rot.x = (__Null) ((this.CamDat.Rot.x + zero.x) % 360.0);
            break;
          case 2:
            ref Vector3 local4 = ref this.CamDat.Pos;
            local4.y = (__Null) (local4.y + (double) axis2 * (double) this.moveSpeed * (double) this.rateAddSpeed);
            ref Vector3 local5 = ref this.CamDat.Dir;
            local5.z = (__Null) (local5.z - (double) axis1 * (double) this.moveSpeed * (double) this.rateAddSpeed);
            this.CamDat.Dir.z = (__Null) (double) Mathf.Min(0.0f, (float) this.CamDat.Dir.z);
            break;
          case 3:
            zero.x = (__Null) ((double) axis1 * (double) this.moveSpeed * (double) this.rateAddSpeed);
            zero.y = (__Null) ((double) axis2 * (double) this.moveSpeed * (double) this.rateAddSpeed);
            if (Object.op_Inequality((Object) this.transBase, (Object) null))
            {
              ref BaseCameraControl_Ver2.CameraData local6 = ref this.CamDat;
              local6.Pos = Vector3.op_Addition(local6.Pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(zero)));
              break;
            }
            ref BaseCameraControl_Ver2.CameraData local7 = ref this.CamDat;
            local7.Pos = Vector3.op_Addition(local7.Pos, ((Component) this).get_transform().TransformDirection(zero));
            break;
        }
        flag = true;
        break;
      }
    }
    return flag;
  }

  protected bool InputKeyProc()
  {
    bool flag = false;
    if (Input.GetKeyDown((KeyCode) 114))
      this.Reset(0);
    else if (Input.GetKeyDown((KeyCode) 261))
    {
      this.CamDat.Rot.x = this.CamReset.Rot.x;
      this.CamDat.Rot.y = this.CamReset.Rot.y;
    }
    else if (Input.GetKeyDown((KeyCode) 47))
      this.CamDat.Rot.z = (__Null) 0.0;
    else if (Input.GetKeyDown((KeyCode) 59))
    {
      this.CamDat.Fov = this.CamReset.Fov;
      if (Object.op_Inequality((Object) this.thisCamera, (Object) null))
        this.thisCamera.set_fieldOfView(this.CamDat.Fov);
    }
    float num1 = Time.get_deltaTime() * this.keySpeed;
    if (Input.GetKey((KeyCode) 278))
    {
      flag = true;
      ref Vector3 local = ref this.CamDat.Dir;
      local.z = (__Null) (local.z + (double) num1);
      this.CamDat.Dir.z = (__Null) (double) Mathf.Min(0.0f, (float) this.CamDat.Dir.z);
    }
    else if (Input.GetKey((KeyCode) 279))
    {
      flag = true;
      ref Vector3 local = ref this.CamDat.Dir;
      local.z = (__Null) (local.z - (double) num1);
    }
    if (Input.GetKey((KeyCode) 275))
    {
      flag = true;
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(num1, 0.0f, 0.0f))));
      }
      else
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(num1, 0.0f, 0.0f)));
      }
    }
    else if (Input.GetKey((KeyCode) 276))
    {
      flag = true;
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(-num1, 0.0f, 0.0f))));
      }
      else
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(-num1, 0.0f, 0.0f)));
      }
    }
    if (Input.GetKey((KeyCode) 273))
    {
      flag = true;
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, num1))));
      }
      else
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, num1)));
      }
    }
    else if (Input.GetKey((KeyCode) 274))
    {
      flag = true;
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, -num1))));
      }
      else
      {
        ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
        local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, -num1)));
      }
    }
    if (Input.GetKey((KeyCode) 280))
    {
      flag = true;
      ref Vector3 local = ref this.CamDat.Pos;
      local.y = (__Null) (local.y + (double) num1);
    }
    else if (Input.GetKey((KeyCode) 281))
    {
      flag = true;
      ref Vector3 local = ref this.CamDat.Pos;
      local.y = (__Null) (local.y - (double) num1);
    }
    float num2 = 10f * Time.get_deltaTime();
    Vector3 zero = Vector3.get_zero();
    if (Input.GetKey((KeyCode) 46))
    {
      flag = true;
      ref Vector3 local = ref zero;
      local.z = (__Null) (local.z + (double) num2);
    }
    else if (Input.GetKey((KeyCode) 92))
    {
      flag = true;
      ref Vector3 local = ref zero;
      local.z = (__Null) (local.z - (double) num2);
    }
    if (Input.GetKey((KeyCode) 258))
    {
      flag = true;
      ref Vector3 local = ref zero;
      local.x = (__Null) (local.x - (double) num2 * (double) this.yRotSpeed);
    }
    else if (Input.GetKey((KeyCode) 264))
    {
      flag = true;
      ref Vector3 local = ref zero;
      local.x = (__Null) (local.x + (double) num2 * (double) this.yRotSpeed);
    }
    if (Input.GetKey((KeyCode) 260))
    {
      flag = true;
      ref Vector3 local = ref zero;
      local.y = (__Null) (local.y + (double) num2 * (double) this.xRotSpeed);
    }
    else if (Input.GetKey((KeyCode) 262))
    {
      flag = true;
      ref Vector3 local = ref zero;
      local.y = (__Null) (local.y - (double) num2 * (double) this.xRotSpeed);
    }
    if (flag)
    {
      this.CamDat.Rot.y = (__Null) ((this.CamDat.Rot.y + zero.y) % 360.0);
      this.CamDat.Rot.x = (__Null) ((this.CamDat.Rot.x + zero.x) % 360.0);
      this.CamDat.Rot.z = (__Null) ((this.CamDat.Rot.z + zero.z) % 360.0);
    }
    float deltaTime = Time.get_deltaTime();
    if (Input.GetKey((KeyCode) 61))
    {
      flag = true;
      this.CamDat.Fov = Mathf.Max(this.CamDat.Fov - deltaTime * 15f, this.limitFovMin);
      if (Object.op_Inequality((Object) this.thisCamera, (Object) null))
        this.thisCamera.set_fieldOfView(this.CamDat.Fov);
    }
    else if (Input.GetKey((KeyCode) 93))
    {
      flag = true;
      this.CamDat.Fov = Mathf.Min(this.CamDat.Fov + deltaTime * 15f, this.limitFov);
      if (Object.op_Inequality((Object) this.thisCamera, (Object) null))
        this.thisCamera.set_fieldOfView(this.CamDat.Fov);
    }
    return flag;
  }

  protected void Start()
  {
    this.SetCtrlSpeed();
    this.thisCamera = (Camera) ((Component) this).GetComponent<Camera>();
    if (Object.op_Inequality((Object) this.thisCamera, (Object) null))
      this.thisCamera.set_fieldOfView(this.CamReset.Fov);
    this.ZoomCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => false);
    this.isControlNow = false;
    this.isDrags = new bool[this.CONFIG_SIZE];
    for (int index = 0; index < this.isDrags.Length; ++index)
      this.isDrags[index] = false;
    if (this.isInit)
      return;
    if (!Object.op_Implicit((Object) this.targetObj))
      this.CamDat.Pos = Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(((Component) this).get_transform().TransformDirection(Vector3.get_forward()), this.noneTargetDir));
    this.TargetSet(this.targetObj, true);
  }

  protected void LateUpdate()
  {
    this.isControlNow = false;
    this.SetCtrlSpeed();
    if (!this.isControlNow)
    {
      BaseCameraControl_Ver2.NoCtrlFunc zoomCondition = this.ZoomCondition;
      bool flag = true;
      if (zoomCondition != null)
        flag = zoomCondition();
      BaseCameraControl_Ver2 cameraControlVer2 = this;
      cameraControlVer2.isControlNow = ((cameraControlVer2.isControlNow ? 1 : 0) | (!flag ? 0 : (this.InputMouseWheelZoomProc() ? 1 : 0))) != 0;
    }
    if (!this.isControlNow)
    {
      BaseCameraControl_Ver2.NoCtrlFunc noCtrlCondition = this.NoCtrlCondition;
      bool flag = false;
      if (noCtrlCondition != null)
        flag = noCtrlCondition();
      if (!flag)
      {
        if (this.InputTouchProc())
          this.isControlNow = true;
        else if (this.InputMouseProc())
          this.isControlNow = true;
      }
    }
    if (!this.isControlNow)
    {
      BaseCameraControl_Ver2.NoCtrlFunc keyCondition = this.KeyCondition;
      bool flag = true;
      if (keyCondition != null)
        flag = keyCondition();
      BaseCameraControl_Ver2 cameraControlVer2 = this;
      cameraControlVer2.isControlNow = ((cameraControlVer2.isControlNow ? 1 : 0) | (!flag ? 0 : (this.InputKeyProc() ? 1 : 0))) != 0;
    }
    this.CameraUpdate();
  }

  protected void CameraUpdate()
  {
    if (this.isLimitDir)
      this.CamDat.Dir.z = (__Null) (double) Mathf.Clamp((float) this.CamDat.Dir.z, -this.limitDir, 0.0f);
    if (this.isLimitPos)
      this.CamDat.Pos = Vector3.ClampMagnitude(this.CamDat.Pos, this.limitPos);
    if (Object.op_Inequality((Object) this.transBase, (Object) null))
    {
      ((Component) this).get_transform().set_rotation(Quaternion.op_Multiply(this.transBase.get_rotation(), Quaternion.Euler(this.CamDat.Rot)));
      ((Component) this).get_transform().set_position(Vector3.op_Addition(Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), this.CamDat.Dir), this.transBase.TransformPoint(this.CamDat.Pos)));
    }
    else
    {
      ((Component) this).get_transform().set_rotation(Quaternion.Euler(this.CamDat.Rot));
      ((Component) this).get_transform().set_position(Vector3.op_Addition(Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), this.CamDat.Dir), this.CamDat.Pos));
    }
    this.viewCollider.set_height((float) this.CamDat.Dir.z);
    this.viewCollider.set_center(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.get_forward()), (float) this.CamDat.Dir.z), 0.5f));
  }

  public void TargetSet(Transform target, bool isReset)
  {
    if (Object.op_Implicit((Object) target))
      this.targetObj = target;
    if (Object.op_Implicit((Object) this.targetObj))
      this.CamDat.Pos = this.targetObj.get_position();
    Transform transform = ((Component) this).get_transform();
    this.CamDat.Dir = Vector3.get_zero();
    this.CamDat.Dir.z = (__Null) -(double) Vector3.Distance(this.CamDat.Pos, transform.get_position());
    transform.LookAt(this.CamDat.Pos);
    ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
    Quaternion rotation = ((Component) this).get_transform().get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
    local.Rot = eulerAngles;
    if (!isReset)
      return;
    this.CamReset.Copy(this.CamDat, ((Component) this).get_transform().get_rotation());
  }

  public void FrontTarget(Transform target, bool isReset, float dir = -3.402823E+38f)
  {
    if (Object.op_Implicit((Object) target))
      this.targetObj = target;
    if (Object.op_Implicit((Object) this.targetObj))
    {
      target = this.targetObj;
      this.CamDat.Pos = target.get_position();
    }
    if (!Object.op_Implicit((Object) target))
      return;
    if ((double) dir != -3.40282346638529E+38)
    {
      this.CamDat.Dir = Vector3.get_zero();
      this.CamDat.Dir.z = (__Null) -(double) dir;
    }
    Transform transform = ((Component) this).get_transform();
    transform.set_position(target.get_position());
    Quaternion rotation1 = transform.get_rotation();
    Vector3 eulerAngles1 = ((Quaternion) ref rotation1).get_eulerAngles();
    ((Vector3) ref eulerAngles1).Set((float) this.CamDat.Rot.x, (float) this.CamDat.Rot.y, (float) this.CamDat.Rot.z);
    transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(transform.get_forward(), (float) this.CamDat.Dir.z)));
    transform.LookAt(this.CamDat.Pos);
    ref BaseCameraControl_Ver2.CameraData local = ref this.CamDat;
    Quaternion rotation2 = ((Component) this).get_transform().get_rotation();
    Vector3 eulerAngles2 = ((Quaternion) ref rotation2).get_eulerAngles();
    local.Rot = eulerAngles2;
    if (!isReset)
      return;
    this.CamReset.Copy(this.CamDat, ((Component) this).get_transform().get_rotation());
  }

  public void SetCamera(BaseCameraControl_Ver2 src)
  {
    ((Component) this).get_transform().set_position(((Component) src).get_transform().get_position());
    ((Component) this).get_transform().set_rotation(((Component) src).get_transform().get_rotation());
    this.CamDat = src.CamDat;
    this.CamDat.Pos = Vector3.op_UnaryNegation(Vector3.op_Subtraction(Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), this.CamDat.Dir), ((Component) this).get_transform().get_position()));
    this.CamReset.Copy(this.CamDat, ((Component) this).get_transform().get_rotation());
    if (!Object.op_Inequality((Object) this.thisCamera, (Object) null) || !Object.op_Inequality((Object) src.thisCamera, (Object) null))
      return;
    this.thisCamera.CopyFrom(src.thisCamera);
  }

  public void SetCamera(Vector3 pos, Vector3 angle, Quaternion rot, Vector3 dir)
  {
    ((Component) this).get_transform().set_localPosition(pos);
    ((Component) this).get_transform().set_localRotation(rot);
    this.CamDat.Rot = angle;
    this.CamDat.Dir = dir;
    this.CamDat.Pos = Vector3.op_UnaryNegation(Vector3.op_Subtraction(Quaternion.op_Multiply(((Component) this).get_transform().get_localRotation(), this.CamDat.Dir), ((Component) this).get_transform().get_localPosition()));
    this.CamReset.Copy(this.CamDat, ((Component) this).get_transform().get_rotation());
  }

  public void CopyCamera(BaseCameraControl_Ver2 dest)
  {
    ((Component) dest).get_transform().set_position(((Component) this).get_transform().get_position());
    ((Component) dest).get_transform().set_rotation(((Component) this).get_transform().get_rotation());
    dest.CamDat = this.CamDat;
    dest.CamDat.Pos = Vector3.op_UnaryNegation(Vector3.op_Subtraction(Quaternion.op_Multiply(((Component) dest).get_transform().get_rotation(), dest.CamDat.Dir), ((Component) dest).get_transform().get_position()));
  }

  public void CopyInstance(BaseCameraControl_Ver2 src)
  {
    this.isInit = true;
    this.targetObj = src.targetObj;
    this.xRotSpeed = src.xRotSpeed;
    this.yRotSpeed = src.yRotSpeed;
    this.zoomSpeed = src.zoomSpeed;
    this.moveSpeed = src.moveSpeed;
    this.keySpeed = src.keySpeed;
    this.noneTargetDir = src.noneTargetDir;
    this.NoCtrlCondition = src.NoCtrlCondition;
    this.ZoomCondition = src.ZoomCondition;
    this.KeyCondition = src.KeyCondition;
    if (!Object.op_Inequality((Object) this.thisCamera, (Object) null) || !Object.op_Inequality((Object) src.thisCamera, (Object) null))
      return;
    this.thisCamera.CopyFrom(src.thisCamera);
  }

  private void OnDrawGizmos()
  {
    Gizmos.set_color(this.CamDat.Dir.z <= 0.0 ? Color.get_blue() : Color.get_red());
    Gizmos.DrawRay(((Component) this).get_transform().get_position(), !Object.op_Inequality((Object) this.transBase, (Object) null) ? Vector3.op_Subtraction(this.CamDat.Pos, ((Component) this).get_transform().get_position()) : Vector3.op_Subtraction(this.transBase.TransformPoint(this.CamDat.Pos), ((Component) this).get_transform().get_position()));
  }

  public bool SetBase(Transform _trans)
  {
    if (Object.op_Equality((Object) this.transBase, (Object) null))
      return false;
    ((Component) this.transBase).get_transform().set_position(_trans.get_position());
    ((Component) this.transBase).get_transform().set_rotation(_trans.get_rotation());
    return true;
  }

  public bool SetCtrlSpeed()
  {
    if (!Singleton<Manager.Config>.IsInstance())
      ;
    return true;
  }

  public delegate bool NoCtrlFunc();

  [Serializable]
  protected struct CameraData
  {
    public Vector3 Pos;
    public Vector3 Dir;
    public Vector3 Rot;
    public float Fov;

    public void Copy(BaseCameraControl_Ver2.ResetData copy)
    {
      this.Pos = copy.Pos;
      this.Dir = copy.Dir;
      this.Rot = copy.Rot;
      this.Fov = copy.Fov;
    }

    public void Copy(BaseCameraControl_Ver2.CameraData _src)
    {
      this.Pos = _src.Pos;
      this.Dir = _src.Dir;
      this.Rot = _src.Rot;
      this.Fov = _src.Fov;
    }
  }

  protected struct ResetData
  {
    public Vector3 Pos;
    public Vector3 Dir;
    public Vector3 Rot;
    public Quaternion RotQ;
    public float Fov;

    public void Copy(BaseCameraControl_Ver2.CameraData copy, Quaternion rot)
    {
      this.Pos = copy.Pos;
      this.Dir = copy.Dir;
      this.Rot = copy.Rot;
      this.RotQ = rot;
      this.Fov = copy.Fov;
    }
  }

  public enum Config
  {
    MoveXZ,
    Rotation,
    Translation,
    MoveXY,
  }
}
