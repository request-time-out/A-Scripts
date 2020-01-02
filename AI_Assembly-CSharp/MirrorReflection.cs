// Decompiled with JetBrains decompiler
// Type: MirrorReflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class MirrorReflection : MonoBehaviour
{
  public bool m_DisablePixelLights;
  public int m_TextureSize;
  public float m_ClipPlaneOffset;
  public LayerMask m_ReflectLayers;
  private Hashtable m_ReflectionCameras;
  private RenderTexture m_ReflectionTexture;
  private int m_OldReflectionTextureSize;
  private static bool s_InsideRendering;
  private int _ReflectionTex;
  private int _ProjMatrix;

  public MirrorReflection()
  {
    base.\u002Ector();
  }

  public void Awake()
  {
    this._ReflectionTex = Shader.PropertyToID("_ReflectionTex");
    this._ProjMatrix = Shader.PropertyToID("_ProjMatrix");
  }

  public void OnWillRenderObject()
  {
    if (!((Behaviour) this).get_enabled() || !Object.op_Implicit((Object) ((Component) this).GetComponent<Renderer>()) || (!Object.op_Implicit((Object) ((Renderer) ((Component) this).GetComponent<Renderer>()).get_sharedMaterial()) || !((Renderer) ((Component) this).GetComponent<Renderer>()).get_enabled()))
      return;
    Camera current = Camera.get_current();
    if (!Object.op_Implicit((Object) current) || MirrorReflection.s_InsideRendering)
      return;
    MirrorReflection.s_InsideRendering = true;
    Camera reflectionCamera;
    this.CreateMirrorObjects(current, out reflectionCamera);
    Vector3 position1 = ((Component) this).get_transform().get_position();
    Vector3 forward = ((Component) this).get_transform().get_forward();
    int pixelLightCount = QualitySettings.get_pixelLightCount();
    if (this.m_DisablePixelLights)
      QualitySettings.set_pixelLightCount(0);
    this.UpdateCameraModes(current, reflectionCamera);
    float num = -Vector3.Dot(forward, position1) - this.m_ClipPlaneOffset;
    Vector4 plane;
    ((Vector4) ref plane).\u002Ector((float) forward.x, (float) forward.y, (float) forward.z, num);
    Matrix4x4 zero = Matrix4x4.get_zero();
    MirrorReflection.CalculateReflectionMatrix(ref zero, plane);
    Vector3 position2 = ((Component) current).get_transform().get_position();
    Vector3 vector3 = ((Matrix4x4) ref zero).MultiplyPoint(position2);
    reflectionCamera.set_worldToCameraMatrix(Matrix4x4.op_Multiply(current.get_worldToCameraMatrix(), zero));
    Vector4 clipPlane = this.CameraSpacePlane(reflectionCamera, position1, forward, 1f);
    Matrix4x4 projectionMatrix = current.get_projectionMatrix();
    MirrorReflection.CalculateObliqueMatrix(ref projectionMatrix, clipPlane);
    reflectionCamera.set_projectionMatrix(projectionMatrix);
    reflectionCamera.set_cullingMask(-17 & ((LayerMask) ref this.m_ReflectLayers).get_value());
    reflectionCamera.set_targetTexture(this.m_ReflectionTexture);
    GL.set_invertCulling(true);
    ((Component) reflectionCamera).get_transform().set_position(vector3);
    Vector3 eulerAngles = ((Component) current).get_transform().get_eulerAngles();
    ((Component) reflectionCamera).get_transform().set_eulerAngles(new Vector3(0.0f, (float) eulerAngles.y, (float) eulerAngles.z));
    reflectionCamera.Render();
    ((Component) reflectionCamera).get_transform().set_position(position2);
    GL.set_invertCulling(false);
    Material[] sharedMaterials = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_sharedMaterials();
    foreach (Material material in sharedMaterials)
    {
      if (material.HasProperty(this._ReflectionTex))
        material.SetTexture(this._ReflectionTex, (Texture) this.m_ReflectionTexture);
    }
    Matrix4x4 matrix4x4_1 = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.get_identity(), new Vector3(0.5f, 0.5f, 0.5f));
    Vector3 lossyScale = ((Component) this).get_transform().get_lossyScale();
    Matrix4x4 matrix4x4_2 = Matrix4x4.op_Multiply(((Component) this).get_transform().get_localToWorldMatrix(), Matrix4x4.Scale(new Vector3((float) (1.0 / lossyScale.x), (float) (1.0 / lossyScale.y), (float) (1.0 / lossyScale.z))));
    Matrix4x4 matrix4x4_3 = Matrix4x4.op_Multiply(Matrix4x4.op_Multiply(Matrix4x4.op_Multiply(matrix4x4_1, current.get_projectionMatrix()), current.get_worldToCameraMatrix()), matrix4x4_2);
    foreach (Material material in sharedMaterials)
      material.SetMatrix(this._ProjMatrix, matrix4x4_3);
    if (this.m_DisablePixelLights)
      QualitySettings.set_pixelLightCount(pixelLightCount);
    MirrorReflection.s_InsideRendering = false;
  }

  private void OnDisable()
  {
    if (Object.op_Implicit((Object) this.m_ReflectionTexture))
    {
      Object.DestroyImmediate((Object) this.m_ReflectionTexture);
      this.m_ReflectionTexture = (RenderTexture) null;
    }
    foreach (DictionaryEntry reflectionCamera in this.m_ReflectionCameras)
      Object.DestroyImmediate((Object) ((Component) reflectionCamera.Value).get_gameObject());
    this.m_ReflectionCameras.Clear();
  }

  private void UpdateCameraModes(Camera src, Camera dest)
  {
    if (Object.op_Equality((Object) dest, (Object) null))
      return;
    dest.set_clearFlags(src.get_clearFlags());
    dest.set_backgroundColor(src.get_backgroundColor());
    if (src.get_clearFlags() == 1)
    {
      Skybox component1 = ((Component) src).GetComponent(typeof (Skybox)) as Skybox;
      Skybox component2 = ((Component) dest).GetComponent(typeof (Skybox)) as Skybox;
      if (!Object.op_Implicit((Object) component1) || !Object.op_Implicit((Object) component1.get_material()))
      {
        ((Behaviour) component2).set_enabled(false);
      }
      else
      {
        ((Behaviour) component2).set_enabled(true);
        component2.set_material(component1.get_material());
      }
    }
    dest.set_farClipPlane(src.get_farClipPlane());
    dest.set_nearClipPlane(src.get_nearClipPlane());
    dest.set_orthographic(src.get_orthographic());
    dest.set_fieldOfView(src.get_fieldOfView());
    dest.set_aspect(src.get_aspect());
    dest.set_orthographicSize(src.get_orthographicSize());
  }

  private void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
  {
    reflectionCamera = (Camera) null;
    if (!Object.op_Implicit((Object) this.m_ReflectionTexture) || this.m_OldReflectionTextureSize != this.m_TextureSize)
    {
      if (Object.op_Implicit((Object) this.m_ReflectionTexture))
        Object.DestroyImmediate((Object) this.m_ReflectionTexture);
      this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
      ((Object) this.m_ReflectionTexture).set_name("__MirrorReflection" + (object) ((Object) this).GetInstanceID());
      this.m_ReflectionTexture.set_isPowerOfTwo(true);
      ((Object) this.m_ReflectionTexture).set_hideFlags((HideFlags) 52);
      this.m_OldReflectionTextureSize = this.m_TextureSize;
    }
    reflectionCamera = this.m_ReflectionCameras[(object) currentCamera] as Camera;
    if (Object.op_Implicit((Object) reflectionCamera))
      return;
    GameObject gameObject = new GameObject("Mirror Refl Camera id" + (object) ((Object) this).GetInstanceID() + " for " + (object) ((Object) currentCamera).GetInstanceID(), new System.Type[2]
    {
      typeof (Camera),
      typeof (Skybox)
    });
    reflectionCamera = (Camera) gameObject.GetComponent<Camera>();
    ((Behaviour) reflectionCamera).set_enabled(false);
    ((Component) reflectionCamera).get_transform().set_position(((Component) this).get_transform().get_position());
    ((Component) reflectionCamera).get_transform().set_rotation(((Component) this).get_transform().get_rotation());
    ((Component) reflectionCamera).get_gameObject().AddComponent<FlareLayer>();
    ((Object) gameObject).set_hideFlags((HideFlags) 61);
    this.m_ReflectionCameras[(object) currentCamera] = (object) reflectionCamera;
  }

  private static float sgn(float a)
  {
    if ((double) a > 0.0)
      return 1f;
    return (double) a < 0.0 ? -1f : 0.0f;
  }

  private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
  {
    Vector3 vector3_1 = Vector3.op_Addition(pos, Vector3.op_Multiply(normal, this.m_ClipPlaneOffset));
    Matrix4x4 worldToCameraMatrix = cam.get_worldToCameraMatrix();
    Vector3 vector3_2 = ((Matrix4x4) ref worldToCameraMatrix).MultiplyPoint(vector3_1);
    Vector3 vector3_3 = ((Matrix4x4) ref worldToCameraMatrix).MultiplyVector(normal);
    Vector3 vector3_4 = Vector3.op_Multiply(((Vector3) ref vector3_3).get_normalized(), sideSign);
    return new Vector4((float) vector3_4.x, (float) vector3_4.y, (float) vector3_4.z, -Vector3.Dot(vector3_2, vector3_4));
  }

  private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
  {
    Vector4 vector4_1 = Matrix4x4.op_Multiply(((Matrix4x4) ref projection).get_inverse(), new Vector4(MirrorReflection.sgn((float) clipPlane.x), MirrorReflection.sgn((float) clipPlane.y), 1f, 1f));
    Vector4 vector4_2 = Vector4.op_Multiply(clipPlane, 2f / Vector4.Dot(clipPlane, vector4_1));
    ((Matrix4x4) ref projection).set_Item(2, (float) vector4_2.x - ((Matrix4x4) ref projection).get_Item(3));
    ((Matrix4x4) ref projection).set_Item(6, (float) vector4_2.y - ((Matrix4x4) ref projection).get_Item(7));
    ((Matrix4x4) ref projection).set_Item(10, (float) vector4_2.z - ((Matrix4x4) ref projection).get_Item(11));
    ((Matrix4x4) ref projection).set_Item(14, (float) vector4_2.w - ((Matrix4x4) ref projection).get_Item(15));
  }

  private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
  {
    reflectionMat.m00 = (__Null) (1.0 - 2.0 * (double) ((Vector4) ref plane).get_Item(0) * (double) ((Vector4) ref plane).get_Item(0));
    reflectionMat.m01 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(0) * (double) ((Vector4) ref plane).get_Item(1));
    reflectionMat.m02 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(0) * (double) ((Vector4) ref plane).get_Item(2));
    reflectionMat.m03 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(3) * (double) ((Vector4) ref plane).get_Item(0));
    reflectionMat.m10 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(1) * (double) ((Vector4) ref plane).get_Item(0));
    reflectionMat.m11 = (__Null) (1.0 - 2.0 * (double) ((Vector4) ref plane).get_Item(1) * (double) ((Vector4) ref plane).get_Item(1));
    reflectionMat.m12 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(1) * (double) ((Vector4) ref plane).get_Item(2));
    reflectionMat.m13 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(3) * (double) ((Vector4) ref plane).get_Item(1));
    reflectionMat.m20 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(2) * (double) ((Vector4) ref plane).get_Item(0));
    reflectionMat.m21 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(2) * (double) ((Vector4) ref plane).get_Item(1));
    reflectionMat.m22 = (__Null) (1.0 - 2.0 * (double) ((Vector4) ref plane).get_Item(2) * (double) ((Vector4) ref plane).get_Item(2));
    reflectionMat.m23 = (__Null) (-2.0 * (double) ((Vector4) ref plane).get_Item(3) * (double) ((Vector4) ref plane).get_Item(2));
    reflectionMat.m30 = (__Null) 0.0;
    reflectionMat.m31 = (__Null) 0.0;
    reflectionMat.m32 = (__Null) 0.0;
    reflectionMat.m33 = (__Null) 1.0;
  }
}
