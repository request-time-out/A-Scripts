// Decompiled with JetBrains decompiler
// Type: CameraControl_Ver2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CameraControl_Ver2 : BaseCameraControl_Ver2
{
  private bool isConfigVanish = true;
  private List<CameraControl_Ver2.VisibleObject> lstMapVanish = new List<CameraControl_Ver2.VisibleObject>();
  private List<Collider> listCollider = new List<Collider>();
  private Renderer targetRender;

  public bool isOutsideTargetTex { get; set; }

  public bool isCursorLock { get; set; }

  public bool isConfigTargetTex { get; set; }

  public bool ConfigVanish
  {
    get
    {
      return this.isConfigVanish;
    }
    set
    {
      if (this.isConfigVanish == value)
        return;
      this.isConfigVanish = value;
      this.visibleFroceVanish(true);
    }
  }

  public Transform targetTex { get; private set; }

  protected new void Start()
  {
    base.Start();
    this.targetTex = ((Component) this).get_transform().Find("CameraTarget");
    if (Object.op_Implicit((Object) this.targetTex))
    {
      this.targetTex.set_localScale(Vector3.op_Multiply(Vector3.get_one(), 0.1f));
      this.targetRender = (Renderer) ((Component) this.targetTex).GetComponent<Renderer>();
    }
    this.isOutsideTargetTex = true;
    this.isConfigTargetTex = true;
    this.isConfigVanish = true;
    this.isCursorLock = true;
    this.viewCollider = (CapsuleCollider) ((Component) this).get_gameObject().AddComponent<CapsuleCollider>();
    this.viewCollider.set_radius(0.05f);
    ((Collider) this.viewCollider).set_isTrigger(true);
    this.viewCollider.set_direction(2);
    Rigidbody orAddComponent = ((Component) this).get_gameObject().GetOrAddComponent<Rigidbody>();
    orAddComponent.set_useGravity(false);
    orAddComponent.set_isKinematic(true);
    this.isInit = true;
    this.listCollider.Clear();
  }

  protected new void LateUpdate()
  {
    if (Singleton<Scene>.IsInstance())
    {
      if (Singleton<Scene>.Instance.NowSceneNames.Any<string>((Func<string, bool>) (s => s == "Config")))
        return;
      if (!Singleton<Scene>.Instance.IsNowLoading && !Singleton<Scene>.Instance.IsNowLoadingFade)
        base.LateUpdate();
    }
    if (Object.op_Implicit((Object) this.targetTex))
    {
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
        this.targetTex.set_position(this.transBase.TransformPoint(this.CamDat.Pos));
      else
        this.targetTex.set_position(this.CamDat.Pos);
      Vector3 position = ((Component) this).get_transform().get_position();
      position.y = this.targetTex.get_position().y;
      ((Component) this.targetTex).get_transform().LookAt(position);
      this.targetTex.Rotate(new Vector3(90f, 0.0f, 0.0f));
      if (Object.op_Inequality((Object) null, (Object) this.targetRender))
        this.targetRender.set_enabled(this.isControlNow & this.isOutsideTargetTex & this.isConfigTargetTex);
      if (Singleton<GameCursor>.IsInstance() && this.isCursorLock)
        Singleton<GameCursor>.Instance.SetCursorLock(this.isControlNow & this.isOutsideTargetTex);
    }
    this.VanishProc();
  }

  private void OnDisable()
  {
    this.visibleFroceVanish(true);
  }

  public void ClearListCollider()
  {
    this.listCollider.Clear();
  }

  protected void OnTriggerEnter(Collider other)
  {
    if (!Object.op_Equality((Object) this.listCollider.Find((Predicate<Collider>) (x => ((Object) other).get_name() == ((Object) x).get_name())), (Object) null))
      return;
    this.listCollider.Add(other);
  }

  protected void OnTriggerStay(Collider other)
  {
    if (!Object.op_Equality((Object) this.listCollider.Find((Predicate<Collider>) (x => ((Object) other).get_name() == ((Object) x).get_name())), (Object) null))
      return;
    this.listCollider.Add(other);
  }

  protected void OnTriggerExit(Collider other)
  {
    this.listCollider.Clear();
  }

  public void autoCamera(float _fSpeed)
  {
    this.CamDat.Rot.y = (__Null) ((this.CamDat.Rot.y + (double) _fSpeed * (double) Time.get_deltaTime()) % 360.0);
  }

  public void CameraDataSave(string _strCreateAssetPath, string _strFile)
  {
    string path = new FileData(string.Empty).Create(_strCreateAssetPath) + _strFile + ".txt";
    Debug.Log((object) path);
    using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
    {
      streamWriter.Write((float) this.CamDat.Pos.x);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Pos.y);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Pos.z);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Dir.x);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Dir.y);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Dir.z);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Rot.x);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Rot.y);
      streamWriter.Write('\n');
      streamWriter.Write((float) this.CamDat.Rot.z);
      streamWriter.Write('\n');
      streamWriter.Write(this.CamDat.Fov);
      streamWriter.Write('\n');
    }
  }

  public void CameraDataSaveBinary(BinaryWriter bw)
  {
    bw.Write((float) this.CamDat.Pos.x);
    bw.Write((float) this.CamDat.Pos.y);
    bw.Write((float) this.CamDat.Pos.z);
    bw.Write((float) this.CamDat.Dir.x);
    bw.Write((float) this.CamDat.Dir.y);
    bw.Write((float) this.CamDat.Dir.z);
    bw.Write((float) this.CamDat.Rot.x);
    bw.Write((float) this.CamDat.Rot.y);
    bw.Write((float) this.CamDat.Rot.z);
    bw.Write(this.CamDat.Fov);
  }

  public bool CameraDataLoadBinary(BinaryReader br, bool isUpdate)
  {
    BaseCameraControl_Ver2.CameraData cameraData = new BaseCameraControl_Ver2.CameraData();
    cameraData.Pos.x = (__Null) (double) br.ReadSingle();
    cameraData.Pos.y = (__Null) (double) br.ReadSingle();
    cameraData.Pos.z = (__Null) (double) br.ReadSingle();
    cameraData.Dir.x = (__Null) (double) br.ReadSingle();
    cameraData.Dir.y = (__Null) (double) br.ReadSingle();
    cameraData.Dir.z = (__Null) (double) br.ReadSingle();
    cameraData.Rot.x = (__Null) (double) br.ReadSingle();
    cameraData.Rot.y = (__Null) (double) br.ReadSingle();
    cameraData.Rot.z = (__Null) (double) br.ReadSingle();
    cameraData.Fov = br.ReadSingle();
    this.CamReset.Copy(cameraData, Quaternion.get_identity());
    if (isUpdate)
    {
      this.CamDat.Copy(cameraData);
      if (Object.op_Inequality((Object) this.thisCamera, (Object) null))
        this.thisCamera.set_fieldOfView(cameraData.Fov);
      this.CameraUpdate();
      if (!this.isInit)
        this.isInit = true;
    }
    return true;
  }

  public void visibleFroceVanish(bool _visible)
  {
    foreach (CameraControl_Ver2.VisibleObject visibleObject in this.lstMapVanish)
    {
      using (List<GameObject>.Enumerator enumerator = visibleObject.listObj.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (Object.op_Implicit((Object) current))
            current.SetActive(_visible);
        }
      }
      visibleObject.isVisible = _visible;
      visibleObject.delay = !_visible ? 0.0f : 0.3f;
    }
  }

  private void visibleFroceVanish(CameraControl_Ver2.VisibleObject _obj, bool _visible)
  {
    if (_obj == null || _obj.listObj == null)
      return;
    for (int index = 0; index < _obj.listObj.Count; ++index)
      _obj.listObj[index].SetActive(_visible);
    _obj.delay = !_visible ? 0.0f : 0.3f;
    _obj.isVisible = _visible;
  }

  private bool VanishProc()
  {
    if (!this.isConfigVanish)
      return false;
    for (int i = 0; i < this.lstMapVanish.Count; ++i)
    {
      if (Object.op_Equality((Object) this.listCollider.Find((Predicate<Collider>) (x => this.lstMapVanish[i].nameCollider == ((Object) x).get_name())), (Object) null))
        this.VanishDelayVisible(this.lstMapVanish[i]);
      else if (this.lstMapVanish[i].isVisible)
        this.visibleFroceVanish(this.lstMapVanish[i], false);
    }
    return true;
  }

  private bool VanishDelayVisible(CameraControl_Ver2.VisibleObject _visible)
  {
    if (_visible.isVisible)
      return false;
    _visible.delay += Time.get_deltaTime();
    if ((double) _visible.delay >= 0.300000011920929)
      this.visibleFroceVanish(_visible, true);
    return true;
  }

  public class VisibleObject
  {
    public bool isVisible = true;
    public List<GameObject> listObj = new List<GameObject>();
    public string nameCollider;
    public float delay;
  }
}
