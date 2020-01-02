// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_PlanarReflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuxWater
{
  [ExecuteInEditMode]
  public class LuxWater_PlanarReflection : MonoBehaviour
  {
    [Space(6f)]
    [LuxWater_HelpBtn("h.5c3jy4qfh163")]
    public bool UpdateSceneView;
    [Space(5f)]
    public bool isMaster;
    public Material[] WaterMaterials;
    [Space(5f)]
    public LayerMask reflectionMask;
    public LuxWater_PlanarReflection.ReflectionResolution Resolution;
    public Color clearColor;
    public bool reflectSkybox;
    [Space(5f)]
    public bool disablePixelLights;
    [Space(5f)]
    public bool renderShadows;
    public float shadowDistance;
    public LuxWater_PlanarReflection.NumberOfShadowCascades ShadowCascades;
    [Space(5f)]
    public float WaterSurfaceOffset;
    public float clipPlaneOffset;
    private string reflectionSampler;
    private Vector3 m_Oldpos;
    private Camera m_ReflectionCamera;
    private Material m_SharedMaterial;
    private Dictionary<Camera, bool> m_HelperCameras;
    private RenderTexture m_reflectionMap;

    public LuxWater_PlanarReflection()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      ((Component) this).get_gameObject().set_layer(LayerMask.NameToLayer("Water"));
      if (!Object.op_Inequality((Object) ((Component) this).GetComponent<Renderer>(), (Object) null))
        return;
      this.m_SharedMaterial = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_sharedMaterial();
    }

    private void OnDisable()
    {
      if (Object.op_Inequality((Object) this.m_ReflectionCamera, (Object) null))
      {
        Object.DestroyImmediate((Object) this.m_ReflectionCamera.get_targetTexture());
        Object.DestroyImmediate((Object) this.m_ReflectionCamera);
      }
      if (this.m_HelperCameras == null)
        return;
      this.m_HelperCameras.Clear();
    }

    private Camera CreateReflectionCameraFor(Camera cam)
    {
      string str = ((Object) ((Component) this).get_gameObject()).get_name() + "Reflection" + ((Object) cam).get_name();
      GameObject gameObject = GameObject.Find(str);
      if (!Object.op_Implicit((Object) gameObject))
      {
        gameObject = new GameObject(str, new Type[1]
        {
          typeof (Camera)
        });
        ((Object) gameObject).set_hideFlags((HideFlags) 61);
      }
      if (!Object.op_Implicit((Object) gameObject.GetComponent(typeof (Camera))))
        gameObject.AddComponent(typeof (Camera));
      Camera component = (Camera) gameObject.GetComponent<Camera>();
      component.set_backgroundColor(this.clearColor);
      component.set_clearFlags(!this.reflectSkybox ? (CameraClearFlags) 2 : (CameraClearFlags) 1);
      this.SetStandardCameraParameter(component, this.reflectionMask);
      if (!Object.op_Implicit((Object) component.get_targetTexture()))
        component.set_targetTexture(this.CreateTextureFor(cam));
      return component;
    }

    private void SetStandardCameraParameter(Camera cam, LayerMask mask)
    {
      cam.set_cullingMask(LayerMask.op_Implicit(mask) & ~(1 << LayerMask.NameToLayer("Water")));
      cam.set_backgroundColor(Color.get_black());
      ((Behaviour) cam).set_enabled(false);
    }

    private RenderTexture CreateTextureFor(Camera cam)
    {
      RenderTexture renderTexture = new RenderTexture(Mathf.FloorToInt((float) (cam.get_pixelWidth() / (int) this.Resolution)), Mathf.FloorToInt((float) (cam.get_pixelHeight() / (int) this.Resolution)), 24);
      ((Object) renderTexture).set_hideFlags((HideFlags) 52);
      return renderTexture;
    }

    public void RenderHelpCameras(Camera currentCam)
    {
      if (this.m_HelperCameras == null)
        this.m_HelperCameras = new Dictionary<Camera, bool>();
      if (!this.m_HelperCameras.ContainsKey(currentCam))
        this.m_HelperCameras.Add(currentCam, false);
      if (this.m_HelperCameras[currentCam] || ((Object) currentCam).get_name().Contains("Reflection Probes"))
        return;
      if (!Object.op_Implicit((Object) this.m_ReflectionCamera))
        this.m_ReflectionCamera = this.CreateReflectionCameraFor(currentCam);
      this.RenderReflectionFor(currentCam, this.m_ReflectionCamera);
      this.m_HelperCameras[currentCam] = true;
    }

    public void LateUpdate()
    {
      if (this.m_HelperCameras == null)
        return;
      this.m_HelperCameras.Clear();
    }

    public void WaterTileBeingRendered(Transform tr, Camera currentCam)
    {
      this.RenderHelpCameras(currentCam);
      if (!Object.op_Implicit((Object) this.m_ReflectionCamera) || !Object.op_Implicit((Object) this.m_SharedMaterial))
        return;
      this.m_SharedMaterial.SetTexture(this.reflectionSampler, (Texture) this.m_ReflectionCamera.get_targetTexture());
    }

    public void OnWillRenderObject()
    {
      this.WaterTileBeingRendered(((Component) this).get_transform(), Camera.get_current());
    }

    private void RenderReflectionFor(Camera cam, Camera reflectCamera)
    {
      if (!Object.op_Implicit((Object) reflectCamera) || Object.op_Implicit((Object) this.m_SharedMaterial) && !this.m_SharedMaterial.HasProperty(this.reflectionSampler))
        return;
      reflectCamera.set_cullingMask(LayerMask.op_Implicit(this.reflectionMask) & ~(1 << LayerMask.NameToLayer("Water")));
      this.SaneCameraSettings(reflectCamera);
      reflectCamera.set_backgroundColor(this.clearColor);
      reflectCamera.set_clearFlags(!this.reflectSkybox ? (CameraClearFlags) 2 : (CameraClearFlags) 1);
      GL.set_invertCulling(true);
      Transform transform = ((Component) this).get_transform();
      Vector3 eulerAngles1 = ((Component) cam).get_transform().get_eulerAngles();
      ((Component) reflectCamera).get_transform().set_eulerAngles(new Vector3((float) -eulerAngles1.x, (float) eulerAngles1.y, (float) eulerAngles1.z));
      ((Component) reflectCamera).get_transform().set_position(((Component) cam).get_transform().get_position());
      reflectCamera.set_orthographic(cam.get_orthographic());
      reflectCamera.set_orthographicSize(cam.get_orthographicSize());
      Vector3 position = ((Component) transform).get_transform().get_position();
      position.y = (__Null) (transform.get_position().y + (double) this.WaterSurfaceOffset);
      Vector3 up = ((Component) transform).get_transform().get_up();
      float num = -Vector3.Dot(up, position) - this.clipPlaneOffset;
      Vector4 plane;
      ((Vector4) ref plane).\u002Ector((float) up.x, (float) up.y, (float) up.z, num);
      Matrix4x4 reflectionMatrix = LuxWater_PlanarReflection.CalculateReflectionMatrix(Matrix4x4.get_zero(), plane);
      this.m_Oldpos = ((Component) cam).get_transform().get_position();
      Vector3 vector3 = ((Matrix4x4) ref reflectionMatrix).MultiplyPoint(this.m_Oldpos);
      reflectCamera.set_worldToCameraMatrix(Matrix4x4.op_Multiply(cam.get_worldToCameraMatrix(), reflectionMatrix));
      Vector4 clipPlane = this.CameraSpacePlane(reflectCamera, position, up, 1f);
      Matrix4x4 obliqueMatrix = LuxWater_PlanarReflection.CalculateObliqueMatrix(cam.get_projectionMatrix(), clipPlane);
      reflectCamera.set_projectionMatrix(obliqueMatrix);
      ((Component) reflectCamera).get_transform().set_position(vector3);
      Vector3 eulerAngles2 = ((Component) cam).get_transform().get_eulerAngles();
      ((Component) reflectCamera).get_transform().set_eulerAngles(new Vector3((float) -eulerAngles2.x, (float) eulerAngles2.y, (float) eulerAngles2.z));
      int pixelLightCount = QualitySettings.get_pixelLightCount();
      if (this.disablePixelLights)
        QualitySettings.set_pixelLightCount(0);
      float shadowDistance = QualitySettings.get_shadowDistance();
      int shadowCascades = QualitySettings.get_shadowCascades();
      if (!this.renderShadows)
        QualitySettings.set_shadowDistance(0.0f);
      else if ((double) this.shadowDistance > 0.0)
        QualitySettings.set_shadowDistance(this.shadowDistance);
      QualitySettings.set_shadowCascades((int) this.ShadowCascades);
      reflectCamera.Render();
      GL.set_invertCulling(false);
      if (this.disablePixelLights)
        QualitySettings.set_pixelLightCount(pixelLightCount);
      if (!this.renderShadows || (double) this.shadowDistance > 0.0)
        QualitySettings.set_shadowDistance(shadowDistance);
      QualitySettings.set_shadowCascades(shadowCascades);
      if (!this.isMaster)
        return;
      for (int index = 0; index < this.WaterMaterials.Length; ++index)
        this.WaterMaterials[index].SetTexture(this.reflectionSampler, (Texture) reflectCamera.get_targetTexture());
    }

    private void SaneCameraSettings(Camera helperCam)
    {
      helperCam.set_depthTextureMode((DepthTextureMode) 0);
      helperCam.set_backgroundColor(Color.get_black());
      helperCam.set_clearFlags((CameraClearFlags) 2);
      helperCam.set_renderingPath((RenderingPath) 1);
    }

    private static Matrix4x4 CalculateObliqueMatrix(
      Matrix4x4 projection,
      Vector4 clipPlane)
    {
      Vector4 vector4_1 = Matrix4x4.op_Multiply(((Matrix4x4) ref projection).get_inverse(), new Vector4(LuxWater_PlanarReflection.Sgn((float) clipPlane.x), LuxWater_PlanarReflection.Sgn((float) clipPlane.y), 1f, 1f));
      Vector4 vector4_2 = Vector4.op_Multiply(clipPlane, 2f / Vector4.Dot(clipPlane, vector4_1));
      ((Matrix4x4) ref projection).set_Item(2, (float) vector4_2.x - ((Matrix4x4) ref projection).get_Item(3));
      ((Matrix4x4) ref projection).set_Item(6, (float) vector4_2.y - ((Matrix4x4) ref projection).get_Item(7));
      ((Matrix4x4) ref projection).set_Item(10, (float) vector4_2.z - ((Matrix4x4) ref projection).get_Item(11));
      ((Matrix4x4) ref projection).set_Item(14, (float) vector4_2.w - ((Matrix4x4) ref projection).get_Item(15));
      return projection;
    }

    private static Matrix4x4 CalculateReflectionMatrix(
      Matrix4x4 reflectionMat,
      Vector4 plane)
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
      return reflectionMat;
    }

    private static float Sgn(float a)
    {
      if ((double) a > 0.0)
        return 1f;
      return (double) a < 0.0 ? -1f : 0.0f;
    }

    private Vector4 CameraSpacePlane(
      Camera cam,
      Vector3 pos,
      Vector3 normal,
      float sideSign)
    {
      Vector3 vector3_1 = Vector3.op_Addition(pos, Vector3.op_Multiply(normal, this.clipPlaneOffset));
      Matrix4x4 worldToCameraMatrix = cam.get_worldToCameraMatrix();
      Vector3 vector3_2 = ((Matrix4x4) ref worldToCameraMatrix).MultiplyPoint(vector3_1);
      Vector3 vector3_3 = ((Matrix4x4) ref worldToCameraMatrix).MultiplyVector(normal);
      Vector3 vector3_4 = Vector3.op_Multiply(((Vector3) ref vector3_3).get_normalized(), sideSign);
      return new Vector4((float) vector3_4.x, (float) vector3_4.y, (float) vector3_4.z, -Vector3.Dot(vector3_2, vector3_4));
    }

    public enum ReflectionResolution
    {
      Full = 1,
      Half = 2,
      Quarter = 4,
      Eighth = 8,
    }

    public enum NumberOfShadowCascades
    {
      One = 1,
      Two = 2,
      Four = 4,
    }
  }
}
