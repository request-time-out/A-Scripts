// Decompiled with JetBrains decompiler
// Type: Studio.CameraControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Cinemachine;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class CameraControl : CinemachineVirtualCameraBase
  {
    private int m_MapLayer;
    public Transform transBase;
    public Transform targetObj;
    public float xRotSpeed;
    public float yRotSpeed;
    public float zoomSpeed;
    public float moveSpeed;
    public float noneTargetDir;
    public bool isLimitPos;
    public float limitPos;
    public bool isLimitDir;
    public float limitDir;
    public float limitFov;
    [SerializeField]
    private Camera m_SubCamera;
    public CameraControl.NoCtrlFunc noCtrlCondition;
    public CameraControl.NoCtrlFunc zoomCondition;
    public CameraControl.NoCtrlFunc keyCondition;
    public readonly int CONFIG_SIZE;
    [SerializeField]
    protected CameraControl.CameraData cameraData;
    protected CameraControl.CameraData cameraReset;
    protected bool isInit;
    private const float INIT_FOV = 23f;
    protected CapsuleCollider viewCollider;
    protected float rateAddSpeed;
    private bool dragging;
    private bool m_ConfigVanish;
    [SerializeField]
    private Transform m_TargetTex;
    [SerializeField]
    private Renderer m_TargetRender;
    [SerializeField]
    private GameObject objRoot;
    private List<CameraControl.VisibleObject> lstMapVanish;
    private List<Collider> listCollider;
    public bool isFlashVisible;
    [SerializeField]
    private LensSettings lensSettings;
    private CameraState cameraState;

    public CameraControl()
    {
      base.\u002Ector();
      this.cameraData.parse = 23f;
      this.cameraReset.parse = 23f;
    }

    private int mapLayer
    {
      get
      {
        if (this.m_MapLayer == -1)
          this.m_MapLayer = LayerMask.GetMask(new string[2]
          {
            "Map",
            "MapNoShadow"
          });
        return this.m_MapLayer;
      }
    }

    public Camera mainCmaera { get; protected set; }

    public Camera subCamera
    {
      get
      {
        return this.m_SubCamera;
      }
    }

    public bool isControlNow { get; protected set; }

    public bool isOutsideTargetTex { get; set; }

    public bool isCursorLock { get; set; }

    public bool isConfigTargetTex { get; set; }

    public bool isConfigVanish
    {
      get
      {
        return this.m_ConfigVanish;
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_ConfigVanish, value))
          return;
        this.VisibleFroceVanish(true);
      }
    }

    public Transform targetTex
    {
      get
      {
        return this.m_TargetTex;
      }
    }

    public bool active
    {
      get
      {
        return this.objRoot.get_activeSelf();
      }
      set
      {
        this.objRoot.SetActive(value);
      }
    }

    public bool IsOutsideSetting { get; set; }

    public CameraControl.CameraData Export()
    {
      return new CameraControl.CameraData(this.cameraData);
    }

    public CameraControl.CameraData ExportResetData()
    {
      return new CameraControl.CameraData(this.cameraReset);
    }

    public void Import(CameraControl.CameraData _src)
    {
      if (_src == null)
        return;
      this.cameraData.Copy(_src);
      this.fieldOfView = this.cameraData.parse;
    }

    public bool LoadVanish(string _assetbundle, string _file, GameObject _objMap)
    {
      this.lstMapVanish.Clear();
      return false;
    }

    public void CloerListCollider()
    {
      this.listCollider.Clear();
      this.lstMapVanish.Clear();
    }

    public void VisibleFroceVanish(bool _visible)
    {
      foreach (CameraControl.VisibleObject visibleObject in this.lstMapVanish)
      {
        using (List<MeshRenderer>.Enumerator enumerator = visibleObject.listRender.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            MeshRenderer current = enumerator.Current;
            if (Object.op_Implicit((Object) current))
              ((Renderer) current).set_enabled(_visible);
          }
        }
        visibleObject.isVisible = _visible;
        visibleObject.delay = !_visible ? 0.0f : 0.3f;
      }
    }

    private void VisibleFroceVanish(CameraControl.VisibleObject _obj, bool _visible)
    {
      if (_obj == null || _obj.listRender == null)
        return;
      using (List<MeshRenderer>.Enumerator enumerator = _obj.listRender.GetEnumerator())
      {
        while (enumerator.MoveNext())
          ((Renderer) enumerator.Current).set_enabled(_visible);
      }
      _obj.delay = !_visible ? 0.0f : 0.3f;
      _obj.isVisible = _visible;
    }

    private void VanishProc()
    {
      if (!this.isConfigVanish)
        return;
      int count = this.lstMapVanish.Count;
      for (int i = 0; i < count; ++i)
      {
        if (Object.op_Equality((Object) this.listCollider.Find((Predicate<Collider>) (x => this.lstMapVanish[i].nameCollider == ((Object) x).get_name())), (Object) null))
          this.VanishDelayVisible(this.lstMapVanish[i]);
        else if (this.lstMapVanish[i].isVisible)
          this.VisibleFroceVanish(this.lstMapVanish[i], false);
      }
    }

    private void VanishDelayVisible(CameraControl.VisibleObject _visible)
    {
      if (_visible.isVisible)
        return;
      if (!this.isFlashVisible)
      {
        _visible.delay += Time.get_deltaTime();
        if ((double) _visible.delay < 0.300000011920929)
          return;
        this.VisibleFroceVanish(_visible, true);
      }
      else
        this.VisibleFroceVanish(_visible, true);
    }

    public Vector3 targetPos
    {
      get
      {
        return this.cameraData.pos;
      }
      set
      {
        this.cameraData.pos = value;
      }
    }

    public Vector3 cameraAngle
    {
      get
      {
        return this.cameraData.rotate;
      }
      set
      {
        ((Component) this).get_transform().set_rotation(Quaternion.Euler(value));
        this.cameraData.rotate = value;
      }
    }

    public float fieldOfView
    {
      get
      {
        return this.cameraData.parse;
      }
      set
      {
        this.cameraData.parse = value;
        if (Object.op_Inequality((Object) this.mainCmaera, (Object) null))
          this.mainCmaera.set_fieldOfView(value);
        if (Object.op_Inequality((Object) this.subCamera, (Object) null))
          this.subCamera.set_fieldOfView(value);
        this.lensSettings.FieldOfView = (__Null) (double) value;
        ((CameraState) ref this.cameraState).set_Lens(this.lensSettings);
      }
    }

    public virtual CameraState State
    {
      get
      {
        return this.cameraState;
      }
    }

    public virtual Transform LookAt
    {
      get
      {
        return this.targetObj;
      }
      set
      {
        this.targetObj = value;
      }
    }

    public virtual Transform Follow
    {
      get
      {
        return this.transBase;
      }
      set
      {
        this.transBase = value;
      }
    }

    public void Reset(int _mode)
    {
      switch (_mode)
      {
        case 0:
          this.cameraData.Copy(this.cameraReset);
          this.fieldOfView = this.cameraData.parse;
          break;
        case 1:
          this.cameraData.pos = this.cameraReset.pos;
          break;
        case 2:
          ((Component) this).get_transform().set_rotation(this.cameraReset.rotation);
          break;
        case 3:
          this.cameraData.distance = this.cameraReset.distance;
          break;
      }
    }

    protected virtual bool InputMouseWheelZoomProc()
    {
      float num = Input.GetAxis("Mouse ScrollWheel") * this.zoomSpeed;
      if ((double) num == 0.0)
        return false;
      ref Vector3 local = ref this.cameraData.distance;
      local.z = (__Null) (local.z + (double) num);
      this.cameraData.distance.z = (__Null) (double) Mathf.Min(0.0f, (float) this.cameraData.distance.z);
      return true;
    }

    protected virtual bool InputMouseProc()
    {
      bool flag = false;
      float axis1 = Input.GetAxis("Mouse X");
      float axis2 = Input.GetAxis("Mouse Y");
      if ((!Object.op_Implicit((Object) EventSystem.get_current()) || !EventSystem.get_current().IsPointerOverGameObject()) && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        this.dragging = true;
      else if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        this.dragging = false;
      if (!this.dragging)
        return false;
      if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
      {
        Vector3 zero = Vector3.get_zero();
        zero.x = (__Null) ((double) axis1 * (double) this.moveSpeed * (double) this.rateAddSpeed);
        zero.z = (__Null) ((double) axis2 * (double) this.moveSpeed * (double) this.rateAddSpeed);
        if (Object.op_Inequality((Object) this.transBase, (Object) null))
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(zero)));
        }
        else
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, ((Component) this).get_transform().TransformDirection(zero));
        }
        flag = true;
      }
      else if (Input.GetMouseButton(0))
      {
        Vector3 zero = Vector3.get_zero();
        ref Vector3 local1 = ref zero;
        local1.y = (__Null) (local1.y + (double) axis1 * (double) this.xRotSpeed * (double) this.rateAddSpeed);
        ref Vector3 local2 = ref zero;
        local2.x = (__Null) (local2.x - (double) axis2 * (double) this.yRotSpeed * (double) this.rateAddSpeed);
        this.cameraData.rotate.y = (__Null) ((this.cameraData.rotate.y + zero.y) % 360.0);
        this.cameraData.rotate.x = (__Null) ((this.cameraData.rotate.x + zero.x) % 360.0);
        flag = true;
      }
      else if (Input.GetMouseButton(1))
      {
        ref Vector3 local1 = ref this.cameraData.pos;
        local1.y = (__Null) (local1.y + (double) axis2 * (double) this.moveSpeed * (double) this.rateAddSpeed);
        ref Vector3 local2 = ref this.cameraData.distance;
        local2.z = (__Null) (local2.z - (double) axis1 * (double) this.moveSpeed * (double) this.rateAddSpeed);
        this.cameraData.distance.z = (__Null) (double) Mathf.Min(0.0f, (float) this.cameraData.distance.z);
        flag = true;
      }
      else if (Input.GetMouseButton(2))
      {
        Vector3 zero = Vector3.get_zero();
        zero.x = (__Null) ((double) axis1 * (double) this.moveSpeed * (double) this.rateAddSpeed);
        zero.y = (__Null) ((double) axis2 * (double) this.moveSpeed * (double) this.rateAddSpeed);
        if (Object.op_Inequality((Object) this.transBase, (Object) null))
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(zero)));
        }
        else
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, ((Component) this).get_transform().TransformDirection(zero));
        }
        flag = true;
      }
      return flag;
    }

    protected virtual bool InputKeyProc()
    {
      bool flag = false;
      if (Input.GetKeyDown((KeyCode) 97))
        this.Reset(0);
      else if (Input.GetKeyDown((KeyCode) 261))
      {
        this.cameraData.rotate.x = this.cameraReset.rotate.x;
        this.cameraData.rotate.y = this.cameraReset.rotate.y;
      }
      else if (Input.GetKeyDown((KeyCode) 47))
        this.cameraData.rotate.z = (__Null) 0.0;
      else if (Input.GetKeyDown((KeyCode) 59))
        this.fieldOfView = this.cameraReset.parse;
      float deltaTime1 = Time.get_deltaTime();
      if (Input.GetKey((KeyCode) 278))
      {
        flag = true;
        ref Vector3 local = ref this.cameraData.distance;
        local.z = (__Null) (local.z + (double) deltaTime1);
        this.cameraData.distance.z = (__Null) (double) Mathf.Min(0.0f, (float) this.cameraData.distance.z);
      }
      else if (Input.GetKey((KeyCode) 279))
      {
        flag = true;
        ref Vector3 local = ref this.cameraData.distance;
        local.z = (__Null) (local.z - (double) deltaTime1);
      }
      if (Input.GetKey((KeyCode) 275))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.transBase, (Object) null))
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(deltaTime1, 0.0f, 0.0f))));
        }
        else
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, ((Component) this).get_transform().TransformDirection(new Vector3(deltaTime1, 0.0f, 0.0f)));
        }
      }
      else if (Input.GetKey((KeyCode) 276))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.transBase, (Object) null))
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(-deltaTime1, 0.0f, 0.0f))));
        }
        else
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, ((Component) this).get_transform().TransformDirection(new Vector3(-deltaTime1, 0.0f, 0.0f)));
        }
      }
      if (Input.GetKey((KeyCode) 273))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.transBase, (Object) null))
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, deltaTime1))));
        }
        else
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, ((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, deltaTime1)));
        }
      }
      else if (Input.GetKey((KeyCode) 274))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.transBase, (Object) null))
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, this.transBase.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, -deltaTime1))));
        }
        else
        {
          CameraControl.CameraData cameraData = this.cameraData;
          cameraData.pos = Vector3.op_Addition(cameraData.pos, ((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, -deltaTime1)));
        }
      }
      if (Input.GetKey((KeyCode) 280))
      {
        flag = true;
        ref Vector3 local = ref this.cameraData.pos;
        local.y = (__Null) (local.y + (double) deltaTime1);
      }
      else if (Input.GetKey((KeyCode) 281))
      {
        flag = true;
        ref Vector3 local = ref this.cameraData.pos;
        local.y = (__Null) (local.y - (double) deltaTime1);
      }
      float num = 10f * Time.get_deltaTime();
      Vector3 zero = Vector3.get_zero();
      if (Input.GetKey((KeyCode) 46))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.z = (__Null) (local.z + (double) num);
      }
      else if (Input.GetKey((KeyCode) 92))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.z = (__Null) (local.z - (double) num);
      }
      if (Input.GetKey((KeyCode) 258))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.x = (__Null) (local.x - (double) num * (double) this.yRotSpeed);
      }
      else if (Input.GetKey((KeyCode) 264))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.x = (__Null) (local.x + (double) num * (double) this.yRotSpeed);
      }
      if (Input.GetKey((KeyCode) 260))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.y = (__Null) (local.y + (double) num * (double) this.xRotSpeed);
      }
      else if (Input.GetKey((KeyCode) 262))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.y = (__Null) (local.y - (double) num * (double) this.xRotSpeed);
      }
      if (flag)
      {
        this.cameraData.rotate.y = (__Null) ((this.cameraData.rotate.y + zero.y) % 360.0);
        this.cameraData.rotate.x = (__Null) ((this.cameraData.rotate.x + zero.x) % 360.0);
        this.cameraData.rotate.z = (__Null) ((this.cameraData.rotate.z + zero.z) % 360.0);
      }
      float deltaTime2 = Time.get_deltaTime();
      if (Input.GetKey((KeyCode) 61))
      {
        flag = true;
        this.fieldOfView = Mathf.Max(this.cameraData.parse - deltaTime2 * 15f, 1f);
      }
      else if (Input.GetKey((KeyCode) 93))
      {
        flag = true;
        this.fieldOfView = Mathf.Min(this.cameraData.parse + deltaTime2 * 15f, this.limitFov);
      }
      return flag;
    }

    public void TargetSet(Transform target, bool isReset)
    {
      if (Object.op_Implicit((Object) target))
        this.targetObj = target;
      if (Object.op_Implicit((Object) this.targetObj))
        this.cameraData.pos = this.targetObj.get_position();
      Transform transform = ((Component) this).get_transform();
      this.cameraData.distance = Vector3.get_zero();
      this.cameraData.distance.z = (__Null) -(double) Vector3.Distance(this.cameraData.pos, transform.get_position());
      transform.LookAt(this.cameraData.pos);
      CameraControl.CameraData cameraData = this.cameraData;
      Quaternion rotation = ((Component) this).get_transform().get_rotation();
      Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
      cameraData.rotate = eulerAngles;
      if (!isReset)
        return;
      this.cameraReset.Copy(this.cameraData);
    }

    public void FrontTarget(Transform target, bool isReset, float dir = -3.402823E+38f)
    {
      if (Object.op_Implicit((Object) target))
        this.targetObj = target;
      if (Object.op_Implicit((Object) this.targetObj))
      {
        target = this.targetObj;
        this.cameraData.pos = target.get_position();
      }
      if (!Object.op_Implicit((Object) target))
        return;
      if ((double) dir != -3.40282346638529E+38)
      {
        this.cameraData.distance = Vector3.get_zero();
        this.cameraData.distance.z = (__Null) -(double) dir;
      }
      Transform transform = ((Component) this).get_transform();
      transform.set_position(target.get_position());
      Quaternion rotation1 = transform.get_rotation();
      Vector3 eulerAngles1 = ((Quaternion) ref rotation1).get_eulerAngles();
      ((Vector3) ref eulerAngles1).Set((float) this.cameraData.rotate.x, (float) this.cameraData.rotate.y, (float) this.cameraData.rotate.z);
      transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(transform.get_forward(), (float) this.cameraData.distance.z)));
      transform.LookAt(this.cameraData.pos);
      CameraControl.CameraData cameraData = this.cameraData;
      Quaternion rotation2 = ((Component) this).get_transform().get_rotation();
      Vector3 eulerAngles2 = ((Quaternion) ref rotation2).get_eulerAngles();
      cameraData.rotate = eulerAngles2;
      if (!isReset)
        return;
      this.cameraReset.Copy(this.cameraData);
    }

    public void SetCamera(Vector3 pos, Vector3 angle, Quaternion rot, Vector3 dir)
    {
      ((Component) this).get_transform().set_localPosition(pos);
      ((Component) this).get_transform().set_localRotation(rot);
      this.cameraData.rotate = angle;
      this.cameraData.distance = dir;
      this.cameraData.pos = Vector3.op_UnaryNegation(Vector3.op_Subtraction(Quaternion.op_Multiply(((Component) this).get_transform().get_localRotation(), this.cameraData.distance), ((Component) this).get_transform().get_localPosition()));
      this.cameraReset.Copy(this.cameraData);
    }

    public void SetCamera(Vector3 _pos, Quaternion _rot, float _dis, bool _update = true, bool _reset = true)
    {
      this.cameraData.pos = _pos;
      this.cameraData.rotation = _rot;
      this.cameraData.distance = new Vector3(0.0f, 0.0f, -_dis);
      if (_reset)
        this.cameraReset.Copy(this.cameraData);
      if (!_update)
        return;
      base.InternalUpdateCameraState(Vector3.get_zero(), 0.0f);
    }

    public void SetBase(Transform _trans)
    {
      if (Object.op_Equality((Object) this.transBase, (Object) null))
        return;
      ((Component) this.transBase).get_transform().set_position(_trans.get_position());
      ((Component) this.transBase).get_transform().set_rotation(_trans.get_rotation());
    }

    public void ReflectOption()
    {
      this.rateAddSpeed = Studio.Studio.optionSystem.cameraSpeed;
      this.xRotSpeed = Studio.Studio.optionSystem.cameraSpeedX;
      this.yRotSpeed = Studio.Studio.optionSystem.cameraSpeedY;
      List<string> stringList = new List<string>();
      if (Singleton<Studio.Studio>.Instance.workInfo.visibleAxis)
      {
        if (Studio.Studio.optionSystem.selectedState == 0)
          stringList.Add("Studio/Col");
        stringList.Add("Studio/Select");
      }
      stringList.Add("Studio/Route");
      this.m_SubCamera.set_cullingMask(LayerMask.GetMask(stringList.ToArray()));
    }

    private void Awake()
    {
      this.m_MapLayer = -1;
      this.mainCmaera = (Camera) ((Component) this).GetComponent<Camera>();
      this.fieldOfView = this.cameraReset.parse;
      this.zoomCondition = (CameraControl.NoCtrlFunc) (() => false);
      this.isControlNow = false;
      if (!Object.op_Implicit((Object) this.targetObj))
        this.cameraData.pos = Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(((Component) this).get_transform().TransformDirection(Vector3.get_forward()), this.noneTargetDir));
      this.TargetSet(this.targetObj, true);
      this.isOutsideTargetTex = true;
      this.isConfigTargetTex = true;
      this.isCursorLock = true;
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CameraControl.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void LateUpdate()
    {
      if (Singleton<Scene>.Instance.AddSceneName != string.Empty || Singleton<Scene>.Instance.IsNowLoadingFade || this.IsOutsideSetting || !this.isControlNow && Input.GetKey((KeyCode) 98))
        return;
      this.isControlNow = false;
      this.xRotSpeed = Studio.Studio.optionSystem.cameraSpeedX;
      this.yRotSpeed = Studio.Studio.optionSystem.cameraSpeedY;
      if (!this.isControlNow)
      {
        CameraControl cameraControl = this;
        cameraControl.isControlNow = ((cameraControl.isControlNow ? 1 : 0) | ((this.zoomCondition == null ? 1 : (this.zoomCondition() ? 1 : 0)) == 0 ? 0 : (this.InputMouseWheelZoomProc() ? 1 : 0))) != 0;
      }
      if (!this.isControlNow && ((this.noCtrlCondition == null ? 0 : (this.noCtrlCondition() ? 1 : 0)) == 0 && this.InputMouseProc()))
        this.isControlNow = true;
      if (this.isControlNow)
        return;
      CameraControl cameraControl1 = this;
      cameraControl1.isControlNow = ((cameraControl1.isControlNow ? 1 : 0) | ((this.keyCondition == null ? 1 : (this.keyCondition() ? 1 : 0)) == 0 ? 0 : (this.InputKeyProc() ? 1 : 0))) != 0;
    }

    protected void OnTriggerEnter(Collider other)
    {
      if (Object.op_Equality((Object) other, (Object) null) || (this.mapLayer & 1 << ((Component) other).get_gameObject().get_layer()) == 0 || !Object.op_Equality((Object) this.listCollider.Find((Predicate<Collider>) (x => ((Object) other).get_name() == ((Object) x).get_name())), (Object) null))
        return;
      this.listCollider.Add(other);
    }

    protected void OnTriggerStay(Collider other)
    {
      if (Object.op_Equality((Object) other, (Object) null) || (this.mapLayer & 1 << ((Component) other).get_gameObject().get_layer()) == 0 || !Object.op_Equality((Object) this.listCollider.Find((Predicate<Collider>) (x => ((Object) other).get_name() == ((Object) x).get_name())), (Object) null))
        return;
      this.listCollider.Add(other);
    }

    protected void OnTriggerExit(Collider other)
    {
      this.listCollider.Clear();
    }

    private void OnDrawGizmos()
    {
      Gizmos.set_color(this.cameraData.distance.z <= 0.0 ? Color.get_blue() : Color.get_red());
      Gizmos.DrawRay(((Component) this).get_transform().get_position(), !Object.op_Inequality((Object) this.transBase, (Object) null) ? Vector3.op_Subtraction(this.cameraData.pos, ((Component) this).get_transform().get_position()) : Vector3.op_Subtraction(this.transBase.TransformPoint(this.cameraData.pos), ((Component) this).get_transform().get_position()));
    }

    public virtual void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
    {
      if (!((Behaviour) this).get_enabled() || this.IsOutsideSetting)
        return;
      if (this.isLimitDir)
        this.cameraData.distance.z = (__Null) (double) Mathf.Clamp((float) this.cameraData.distance.z, -this.limitDir, 0.0f);
      if (this.isLimitPos)
        this.cameraData.pos = Vector3.ClampMagnitude(this.cameraData.pos, this.limitPos);
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
      {
        ((CameraState) ref this.cameraState).set_RawOrientation(Quaternion.op_Multiply(this.transBase.get_rotation(), Quaternion.Euler(this.cameraData.rotate)));
        ((CameraState) ref this.cameraState).set_RawPosition(Vector3.op_Addition(Quaternion.op_Multiply(((CameraState) ref this.cameraState).get_RawOrientation(), this.cameraData.distance), this.transBase.TransformPoint(this.cameraData.pos)));
      }
      else
      {
        ((CameraState) ref this.cameraState).set_RawOrientation(Quaternion.Euler(this.cameraData.rotate));
        ((CameraState) ref this.cameraState).set_RawPosition(Vector3.op_Addition(Quaternion.op_Multiply(((CameraState) ref this.cameraState).get_RawOrientation(), this.cameraData.distance), this.cameraData.pos));
      }
      ((Component) this).get_transform().set_position(((CameraState) ref this.cameraState).get_RawPosition());
      ((Component) this).get_transform().set_rotation(((CameraState) ref this.cameraState).get_RawOrientation());
      if (Object.op_Implicit((Object) this.targetTex))
      {
        if (Object.op_Inequality((Object) this.transBase, (Object) null))
          this.targetTex.set_position(this.transBase.TransformPoint(this.cameraData.pos));
        else
          this.targetTex.set_position(this.cameraData.pos);
        Vector3 position = ((Component) this).get_transform().get_position();
        position.y = this.targetTex.get_position().y;
        ((Component) this.targetTex).get_transform().LookAt(position);
        this.targetTex.Rotate(90f, 0.0f, 0.0f);
        if (Object.op_Implicit((Object) this.m_TargetRender))
          this.m_TargetRender.set_enabled(this.isControlNow & this.isOutsideTargetTex & this.isConfigTargetTex);
        if (Singleton<GameCursor>.IsInstance() && this.isCursorLock)
          Singleton<GameCursor>.Instance.SetCursorLock(this.isControlNow & this.isOutsideTargetTex);
      }
      if (!Object.op_Inequality((Object) this.viewCollider, (Object) null))
        return;
      this.viewCollider.set_height((float) this.cameraData.distance.z);
      this.viewCollider.set_center(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.get_forward()), (float) this.cameraData.distance.z), 0.5f));
      this.VanishProc();
    }

    public void SetPositionAndRotation(Vector3 _position, Quaternion _orientation)
    {
      ((CameraState) ref this.cameraState).set_RawPosition(_position);
      ((CameraState) ref this.cameraState).set_RawOrientation(_orientation);
    }

    public delegate bool NoCtrlFunc();

    [Serializable]
    public class CameraData
    {
      public Vector3 pos = Vector3.get_zero();
      public Vector3 rotate = Vector3.get_zero();
      public Vector3 distance = Vector3.get_zero();
      public float parse = 23f;
      private const int ver = 2;

      public CameraData()
      {
      }

      public CameraData(CameraControl.CameraData _src)
      {
        this.Copy(_src);
      }

      public Quaternion rotation
      {
        get
        {
          return Quaternion.Euler(this.rotate);
        }
        set
        {
          this.rotate = ((Quaternion) ref value).get_eulerAngles();
        }
      }

      public void Set(Vector3 _pos, Vector3 _rotate, Vector3 _distance, float _parse)
      {
        this.pos = _pos;
        this.rotate = _rotate;
        this.distance = _distance;
        this.parse = _parse;
      }

      public void Save(BinaryWriter _writer)
      {
        _writer.Write(2);
        _writer.Write((float) this.pos.x);
        _writer.Write((float) this.pos.y);
        _writer.Write((float) this.pos.z);
        _writer.Write((float) this.rotate.x);
        _writer.Write((float) this.rotate.y);
        _writer.Write((float) this.rotate.z);
        _writer.Write((float) this.distance.x);
        _writer.Write((float) this.distance.y);
        _writer.Write((float) this.distance.z);
        _writer.Write(this.parse);
      }

      public void Load(BinaryReader _reader)
      {
        int num1 = _reader.ReadInt32();
        this.pos.x = (__Null) (double) _reader.ReadSingle();
        this.pos.y = (__Null) (double) _reader.ReadSingle();
        this.pos.z = (__Null) (double) _reader.ReadSingle();
        this.rotate.x = (__Null) (double) _reader.ReadSingle();
        this.rotate.y = (__Null) (double) _reader.ReadSingle();
        this.rotate.z = (__Null) (double) _reader.ReadSingle();
        if (num1 == 1)
        {
          double num2 = (double) _reader.ReadSingle();
        }
        else
        {
          this.distance.x = (__Null) (double) _reader.ReadSingle();
          this.distance.y = (__Null) (double) _reader.ReadSingle();
          this.distance.z = (__Null) (double) _reader.ReadSingle();
        }
        this.parse = _reader.ReadSingle();
      }

      public void Copy(CameraControl.CameraData _src)
      {
        this.pos = _src.pos;
        this.rotate = _src.rotate;
        this.distance = _src.distance;
        this.parse = _src.parse;
      }
    }

    public enum Config
    {
      MoveXZ,
      Rotation,
      Translation,
      MoveXY,
    }

    public class VisibleObject
    {
      public bool isVisible = true;
      public List<MeshRenderer> listRender = new List<MeshRenderer>();
      public string nameCollider;
      public float delay;
    }
  }
}
