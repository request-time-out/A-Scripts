// Decompiled with JetBrains decompiler
// Type: DistortionMobileCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DistortionMobileCamera : MonoBehaviour
{
  public float TextureScale;
  public RenderTextureFormat RenderTextureFormat;
  public FilterMode FilterMode;
  public LayerMask CullingMask;
  public RenderingPath RenderingPath;
  public int FPSWhenMoveCamera;
  public int FPSWhenStaticCamera;
  public bool UseRealTime;
  private RenderTexture renderTexture;
  private Camera cameraInstance;
  private GameObject goCamera;
  private Vector3 oldPosition;
  private Quaternion oldRotation;
  private Transform instanceCameraTransform;
  private bool canUpdateCamera;
  private bool isStaticUpdate;
  private WaitForSeconds fpsMove;
  private WaitForSeconds fpsStatic;
  private const int DropedFrames = 50;
  private int frameCountWhenCameraIsStatic;

  public DistortionMobileCamera()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (this.UseRealTime)
    {
      this.Initialize();
    }
    else
    {
      this.fpsMove = new WaitForSeconds(1f / (float) this.FPSWhenMoveCamera);
      this.fpsStatic = new WaitForSeconds(1f / (float) this.FPSWhenStaticCamera);
      this.canUpdateCamera = true;
      if (this.FPSWhenMoveCamera > 0)
        this.StartCoroutine(this.RepeatCameraMove());
      if (this.FPSWhenStaticCamera > 0)
        this.StartCoroutine(this.RepeatCameraStatic());
      this.Initialize();
    }
  }

  private void Update()
  {
    if (this.UseRealTime || Object.op_Equality((Object) this.cameraInstance, (Object) null))
      return;
    if ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(this.instanceCameraTransform.get_position(), this.oldPosition)) <= 9.99999974737875E-06 && Quaternion.op_Equality(this.instanceCameraTransform.get_rotation(), this.oldRotation))
    {
      ++this.frameCountWhenCameraIsStatic;
      if (this.frameCountWhenCameraIsStatic >= 50)
        this.isStaticUpdate = true;
    }
    else
    {
      this.frameCountWhenCameraIsStatic = 0;
      this.isStaticUpdate = false;
    }
    this.oldPosition = this.instanceCameraTransform.get_position();
    this.oldRotation = this.instanceCameraTransform.get_rotation();
    if (this.canUpdateCamera)
    {
      if (!((Behaviour) this.cameraInstance).get_enabled())
        ((Behaviour) this.cameraInstance).set_enabled(true);
      if (this.FPSWhenMoveCamera <= 0)
        return;
      this.canUpdateCamera = false;
    }
    else
    {
      if (!((Behaviour) this.cameraInstance).get_enabled())
        return;
      ((Behaviour) this.cameraInstance).set_enabled(false);
    }
  }

  [DebuggerHidden]
  private IEnumerator RepeatCameraMove()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DistortionMobileCamera.\u003CRepeatCameraMove\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator RepeatCameraStatic()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DistortionMobileCamera.\u003CRepeatCameraStatic\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  private void OnBecameVisible()
  {
    if (!Object.op_Inequality((Object) this.goCamera, (Object) null))
      return;
    this.goCamera.SetActive(true);
  }

  private void OnBecameInvisible()
  {
    if (!Object.op_Inequality((Object) this.goCamera, (Object) null))
      return;
    this.goCamera.SetActive(false);
  }

  private void Initialize()
  {
    this.goCamera = new GameObject("RenderTextureCamera");
    this.cameraInstance = (Camera) this.goCamera.AddComponent<Camera>();
    Camera main = Camera.get_main();
    this.cameraInstance.CopyFrom(main);
    Camera cameraInstance = this.cameraInstance;
    cameraInstance.set_depth(cameraInstance.get_depth() + 1f);
    this.cameraInstance.set_cullingMask(LayerMask.op_Implicit(this.CullingMask));
    this.cameraInstance.set_renderingPath(this.RenderingPath);
    this.goCamera.get_transform().set_parent(((Component) main).get_transform());
    this.renderTexture = new RenderTexture(Mathf.RoundToInt((float) Screen.get_width() * this.TextureScale), Mathf.RoundToInt((float) Screen.get_height() * this.TextureScale), 16, this.RenderTextureFormat);
    this.renderTexture.DiscardContents();
    ((Texture) this.renderTexture).set_filterMode(this.FilterMode);
    this.cameraInstance.set_targetTexture(this.renderTexture);
    this.instanceCameraTransform = ((Component) this.cameraInstance).get_transform();
    this.oldPosition = this.instanceCameraTransform.get_position();
    Shader.SetGlobalTexture("_GrabTextureMobile", (Texture) this.renderTexture);
  }

  private void OnDisable()
  {
    if (Object.op_Implicit((Object) this.goCamera))
    {
      Object.DestroyImmediate((Object) this.goCamera);
      this.goCamera = (GameObject) null;
    }
    if (!Object.op_Implicit((Object) this.renderTexture))
      return;
    Object.DestroyImmediate((Object) this.renderTexture);
    this.renderTexture = (RenderTexture) null;
  }
}
