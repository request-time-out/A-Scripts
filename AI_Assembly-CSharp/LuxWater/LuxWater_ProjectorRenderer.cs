// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_ProjectorRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

namespace LuxWater
{
  [RequireComponent(typeof (Camera))]
  [ExecuteInEditMode]
  public class LuxWater_ProjectorRenderer : MonoBehaviour
  {
    [Space(8f)]
    public LuxWater_ProjectorRenderer.BufferResolution FoamBufferResolution;
    public LuxWater_ProjectorRenderer.BufferResolution NormalBufferResolution;
    [Space(2f)]
    [Header("Debug")]
    [Space(4f)]
    public bool DebugFoamBuffer;
    public bool DebugNormalBuffer;
    public bool DebugStats;
    private int drawnFoamProjectors;
    private int drawnNormalProjectors;
    private static CommandBuffer cb_Foam;
    private static CommandBuffer cb_Normals;
    private Camera cam;
    private Transform camTransform;
    private RenderTexture ProjectedFoam;
    private RenderTexture ProjectedNormals;
    private Texture2D defaultBump;
    private Bounds tempBounds;
    private int _LuxWater_FoamOverlayPID;
    private int _LuxWater_NormalOverlayPID;
    private Plane[] frustumPlanes;
    private Material DebugMat;
    private Material DebugNormalMat;

    public LuxWater_ProjectorRenderer()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      this._LuxWater_FoamOverlayPID = Shader.PropertyToID("_LuxWater_FoamOverlay");
      this._LuxWater_NormalOverlayPID = Shader.PropertyToID("_LuxWater_NormalOverlay");
      LuxWater_ProjectorRenderer.cb_Foam = new CommandBuffer();
      LuxWater_ProjectorRenderer.cb_Foam.set_name("Lux Water: Foam Overlay Buffer");
      LuxWater_ProjectorRenderer.cb_Normals = new CommandBuffer();
      LuxWater_ProjectorRenderer.cb_Normals.set_name("Lux Water: Normal Overlay Buffer");
    }

    private void OnDisable()
    {
      if (Object.op_Inequality((Object) this.ProjectedFoam, (Object) null))
        Object.DestroyImmediate((Object) this.ProjectedFoam);
      if (Object.op_Inequality((Object) this.ProjectedNormals, (Object) null))
        Object.DestroyImmediate((Object) this.ProjectedNormals);
      if (Object.op_Inequality((Object) this.defaultBump, (Object) null))
        Object.DestroyImmediate((Object) this.defaultBump);
      if (Object.op_Inequality((Object) this.DebugMat, (Object) null))
        Object.DestroyImmediate((Object) this.DebugMat);
      if (LuxWater_ProjectorRenderer.cb_Foam != null && LuxWater_ProjectorRenderer.cb_Foam.get_sizeInBytes() > 0)
      {
        LuxWater_ProjectorRenderer.cb_Foam.Clear();
        LuxWater_ProjectorRenderer.cb_Foam.Dispose();
      }
      if (LuxWater_ProjectorRenderer.cb_Normals != null && LuxWater_ProjectorRenderer.cb_Normals.get_sizeInBytes() > 0)
      {
        LuxWater_ProjectorRenderer.cb_Normals.Clear();
        LuxWater_ProjectorRenderer.cb_Normals.Dispose();
      }
      Shader.DisableKeyword("USINGWATERPROJECTORS");
    }

    private void OnPreCull()
    {
      this.cam = (Camera) ((Component) this).GetComponent<Camera>();
      int count1 = LuxWater_Projector.FoamProjectors.Count;
      int count2 = LuxWater_Projector.NormalProjectors.Count;
      if (count1 + count2 == 0)
      {
        if (LuxWater_ProjectorRenderer.cb_Foam != null)
          LuxWater_ProjectorRenderer.cb_Foam.Clear();
        if (LuxWater_ProjectorRenderer.cb_Normals != null)
          LuxWater_ProjectorRenderer.cb_Normals.Clear();
        Shader.DisableKeyword("USINGWATERPROJECTORS");
      }
      else
      {
        Shader.EnableKeyword("USINGWATERPROJECTORS");
        Matrix4x4 projectionMatrix = this.cam.get_projectionMatrix();
        Matrix4x4 worldToCameraMatrix = this.cam.get_worldToCameraMatrix();
        Matrix4x4 worldToProjectMatrix = Matrix4x4.op_Multiply(projectionMatrix, worldToCameraMatrix);
        int pixelWidth = this.cam.get_pixelWidth();
        int pixelHeight = this.cam.get_pixelHeight();
        GeomUtil.CalculateFrustumPlanes(this.frustumPlanes, worldToProjectMatrix);
        int num1 = Mathf.FloorToInt((float) (pixelWidth / (int) this.FoamBufferResolution));
        int num2 = Mathf.FloorToInt((float) (pixelHeight / (int) this.FoamBufferResolution));
        if (!Object.op_Implicit((Object) this.ProjectedFoam))
          this.ProjectedFoam = new RenderTexture(num1, num2, 0, (RenderTextureFormat) 0, (RenderTextureReadWrite) 1);
        else if (((Texture) this.ProjectedFoam).get_width() != num1)
        {
          Object.DestroyImmediate((Object) this.ProjectedFoam);
          this.ProjectedFoam = new RenderTexture(num1, num2, 0, (RenderTextureFormat) 0, (RenderTextureReadWrite) 1);
        }
        GL.PushMatrix();
        GL.set_modelview(worldToCameraMatrix);
        GL.LoadProjectionMatrix(projectionMatrix);
        LuxWater_ProjectorRenderer.cb_Foam.Clear();
        LuxWater_ProjectorRenderer.cb_Foam.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this.ProjectedFoam));
        LuxWater_ProjectorRenderer.cb_Foam.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f), 1f);
        this.drawnFoamProjectors = 0;
        for (int index = 0; index < count1; ++index)
        {
          LuxWater_Projector foamProjector = LuxWater_Projector.FoamProjectors[index];
          this.tempBounds = foamProjector.m_Rend.get_bounds();
          if (GeometryUtility.TestPlanesAABB(this.frustumPlanes, this.tempBounds))
          {
            LuxWater_ProjectorRenderer.cb_Foam.DrawRenderer(foamProjector.m_Rend, foamProjector.m_Mat);
            ++this.drawnFoamProjectors;
          }
        }
        Graphics.ExecuteCommandBuffer(LuxWater_ProjectorRenderer.cb_Foam);
        Shader.SetGlobalTexture(this._LuxWater_FoamOverlayPID, (Texture) this.ProjectedFoam);
        int num3 = Mathf.FloorToInt((float) (pixelWidth / (int) this.NormalBufferResolution));
        int num4 = Mathf.FloorToInt((float) (pixelHeight / (int) this.NormalBufferResolution));
        if (!Object.op_Implicit((Object) this.ProjectedNormals))
          this.ProjectedNormals = new RenderTexture(num3, num4, 0, (RenderTextureFormat) 2, (RenderTextureReadWrite) 1);
        else if (((Texture) this.ProjectedNormals).get_width() != num3)
        {
          Object.DestroyImmediate((Object) this.ProjectedNormals);
          this.ProjectedNormals = new RenderTexture(num3, num4, 0, (RenderTextureFormat) 2, (RenderTextureReadWrite) 1);
        }
        LuxWater_ProjectorRenderer.cb_Normals.Clear();
        LuxWater_ProjectorRenderer.cb_Normals.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this.ProjectedNormals));
        LuxWater_ProjectorRenderer.cb_Normals.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f), 1f);
        this.drawnNormalProjectors = 0;
        for (int index = 0; index < count2; ++index)
        {
          LuxWater_Projector normalProjector = LuxWater_Projector.NormalProjectors[index];
          this.tempBounds = normalProjector.m_Rend.get_bounds();
          if (GeometryUtility.TestPlanesAABB(this.frustumPlanes, this.tempBounds))
          {
            LuxWater_ProjectorRenderer.cb_Normals.DrawRenderer(normalProjector.m_Rend, normalProjector.m_Mat);
            ++this.drawnNormalProjectors;
          }
        }
        Graphics.ExecuteCommandBuffer(LuxWater_ProjectorRenderer.cb_Normals);
        Shader.SetGlobalTexture(this._LuxWater_NormalOverlayPID, (Texture) this.ProjectedNormals);
        GL.PopMatrix();
      }
    }

    private void OnDrawGizmos()
    {
      Camera component = (Camera) ((Component) this).GetComponent<Camera>();
      int num1 = 0;
      int num2 = (int) ((double) component.get_aspect() * 128.0);
      if (Object.op_Equality((Object) this.DebugMat, (Object) null))
        this.DebugMat = new Material(Shader.Find("Hidden/LuxWater_Debug"));
      if (Object.op_Equality((Object) this.DebugNormalMat, (Object) null))
        this.DebugNormalMat = new Material(Shader.Find("Hidden/LuxWater_DebugNormals"));
      if (this.DebugFoamBuffer)
      {
        if (Object.op_Equality((Object) this.ProjectedFoam, (Object) null))
          return;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0.0f, (float) Screen.get_width(), (float) Screen.get_height(), 0.0f);
        Graphics.DrawTexture(new Rect((float) num1, 0.0f, (float) num2, 128f), (Texture) this.ProjectedFoam, this.DebugMat);
        GL.PopMatrix();
        num1 = num2;
      }
      if (!this.DebugNormalBuffer || Object.op_Equality((Object) this.ProjectedNormals, (Object) null))
        return;
      GL.PushMatrix();
      GL.LoadPixelMatrix(0.0f, (float) Screen.get_width(), (float) Screen.get_height(), 0.0f);
      Graphics.DrawTexture(new Rect((float) num1, 0.0f, (float) num2, 128f), (Texture) this.ProjectedNormals, this.DebugNormalMat);
      GL.PopMatrix();
    }

    private void OnGUI()
    {
      if (!this.DebugStats)
        return;
      int count1 = LuxWater_Projector.FoamProjectors.Count;
      int count2 = LuxWater_Projector.NormalProjectors.Count;
      TextAnchor alignment = GUI.get_skin().get_label().get_alignment();
      GUI.get_skin().get_label().set_alignment((TextAnchor) 3);
      GUI.Label(new Rect(10f, 0.0f, 300f, 40f), "Foam Projectors   [Registered] " + (object) count1 + "  [Drawn] " + (object) this.drawnFoamProjectors);
      GUI.Label(new Rect(10f, 18f, 300f, 40f), "Normal Projectors [Registered] " + (object) count2 + "  [Drawn] " + (object) this.drawnNormalProjectors);
      GUI.get_skin().get_label().set_alignment(alignment);
    }

    public enum BufferResolution
    {
      Full = 1,
      Half = 2,
      Quarter = 4,
      Eighth = 8,
    }
  }
}
