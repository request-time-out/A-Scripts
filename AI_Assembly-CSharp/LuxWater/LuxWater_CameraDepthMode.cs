// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_CameraDepthMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LuxWater
{
  [ExecuteInEditMode]
  [RequireComponent(typeof (Camera))]
  public class LuxWater_CameraDepthMode : MonoBehaviour
  {
    public bool GrabDepthTexture;
    private Camera cam;
    private Material CopyDepthMat;
    private RenderTextureFormat format;
    private Dictionary<Camera, CommandBuffer> m_cmdBuffer;
    private bool CamCallBackAdded;
    [HideInInspector]
    public bool ShowShaderWarning;

    public LuxWater_CameraDepthMode()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      this.cam = (Camera) ((Component) this).GetComponent<Camera>();
      Camera cam = this.cam;
      cam.set_depthTextureMode((DepthTextureMode) (cam.get_depthTextureMode() | 1));
      if (SystemInfo.get_graphicsDeviceType() != 16)
        return;
      // ISSUE: method pointer
      Camera.onPreCull = (__Null) Delegate.Combine((Delegate) Camera.onPreCull, (Delegate) new Camera.CameraCallback((object) this, __methodptr(OnPrecull)));
      this.CamCallBackAdded = true;
      this.CopyDepthMat = new Material(Shader.Find("Hidden/Lux Water/CopyDepth"));
      this.format = (RenderTextureFormat) 14;
      if (!SystemInfo.SupportsRenderTextureFormat(this.format))
        this.format = (RenderTextureFormat) 15;
      if (SystemInfo.SupportsRenderTextureFormat(this.format))
        return;
      this.format = (RenderTextureFormat) 2;
    }

    private void OnDisable()
    {
      if (this.CamCallBackAdded)
      {
        // ISSUE: method pointer
        Camera.onPreCull = (__Null) Delegate.Remove((Delegate) Camera.onPreCull, (Delegate) new Camera.CameraCallback((object) this, __methodptr(OnPrecull)));
        using (Dictionary<Camera, CommandBuffer>.Enumerator enumerator = this.m_cmdBuffer.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<Camera, CommandBuffer> current = enumerator.Current;
            if (Object.op_Inequality((Object) current.Key, (Object) null))
              current.Key.RemoveCommandBuffer((CameraEvent) 7, current.Value);
          }
        }
        this.m_cmdBuffer.Clear();
      }
      this.ShowShaderWarning = true;
    }

    private void OnPrecull(Camera camera)
    {
      if (!this.GrabDepthTexture)
        return;
      if (this.cam.get_actualRenderingPath() == 3 && SystemInfo.get_graphicsDeviceType() == 16)
      {
        CommandBuffer commandBuffer;
        if (!this.m_cmdBuffer.TryGetValue(camera, out commandBuffer))
        {
          commandBuffer = new CommandBuffer();
          commandBuffer.set_name("Lux Water Grab Depth");
          camera.AddCommandBuffer((CameraEvent) 14, commandBuffer);
          this.m_cmdBuffer[camera] = commandBuffer;
        }
        commandBuffer.Clear();
        int pixelWidth = camera.get_pixelWidth();
        int pixelHeight = camera.get_pixelHeight();
        int id = Shader.PropertyToID("_Lux_GrabbedDepth");
        commandBuffer.GetTemporaryRT(id, pixelWidth, pixelHeight, 0, (FilterMode) 0, this.format, (RenderTextureReadWrite) 1);
        commandBuffer.Blit(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 1), RenderTargetIdentifier.op_Implicit(id), this.CopyDepthMat, 0);
        commandBuffer.ReleaseTemporaryRT(id);
      }
      else
      {
        this.GrabDepthTexture = false;
        using (Dictionary<Camera, CommandBuffer>.Enumerator enumerator = this.m_cmdBuffer.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<Camera, CommandBuffer> current = enumerator.Current;
            if (Object.op_Inequality((Object) current.Key, (Object) null))
              current.Key.RemoveCommandBuffer((CameraEvent) 7, current.Value);
          }
        }
        this.m_cmdBuffer.Clear();
        this.ShowShaderWarning = true;
      }
    }
  }
}
