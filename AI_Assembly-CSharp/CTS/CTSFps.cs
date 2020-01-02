// Decompiled with JetBrains decompiler
// Type: CTS.CTSFps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace CTS
{
  public class CTSFps : MonoBehaviour
  {
    private const string cFormat = "FPS {0}, MS {1:0.00}";
    private const float cMeasurePeriod = 1f;
    private float m_currentFps;
    private float m_currentMs;
    private float m_fpsAccumulator;
    private float m_fpsNextPeriod;
    public string m_CTSVersion;
    public string m_OS;
    public string m_deviceName;
    public string m_deviceType;
    public string m_deviceModel;
    public string m_platform;
    public string m_processor;
    public string m_ram;
    public string m_gpu;
    public string m_gpuDevice;
    public string m_gpuVendor;
    public string m_gpuSpec;
    public string m_gpuRam;
    public string m_gpuCapabilities;
    public string m_screenInfo;
    public string m_quality;
    public Text m_fpsText;
    public Text m_CTSVersionText;
    public Text m_OSText;
    public Text m_deviceText;
    public Text m_systemText;
    public Text m_gpuText;
    public Text m_gpuCapabilitiesText;
    public Text m_screenInfoText;

    public CTSFps()
    {
      base.\u002Ector();
    }

    public int FPS
    {
      get
      {
        return (int) this.m_currentFps;
      }
    }

    private void Start()
    {
      this.m_fpsNextPeriod = Time.get_realtimeSinceStartup() + 1f;
      try
      {
        this.m_CTSVersion = "CTS v" + (object) CTSConstants.MajorVersion + "." + (object) CTSConstants.MinorVersion + ", Unity v" + Application.get_unityVersion();
        this.m_deviceName = SystemInfo.get_deviceName();
        this.m_deviceType = SystemInfo.get_deviceType().ToString();
        this.m_OS = SystemInfo.get_operatingSystem();
        this.m_platform = Application.get_platform().ToString();
        this.m_processor = SystemInfo.get_processorType() + " - " + (object) SystemInfo.get_processorCount() + " cores";
        this.m_gpu = SystemInfo.get_graphicsDeviceName();
        this.m_gpuDevice = SystemInfo.get_graphicsDeviceType().ToString() + " - " + SystemInfo.get_graphicsDeviceVersion();
        this.m_gpuVendor = SystemInfo.get_graphicsDeviceVendor();
        this.m_gpuRam = SystemInfo.get_graphicsMemorySize().ToString();
        CTSFps ctsFps1 = this;
        ctsFps1.m_gpuCapabilities = ctsFps1.m_gpuCapabilities + "TA: " + SystemInfo.get_supports2DArrayTextures().ToString();
        CTSFps ctsFps2 = this;
        ctsFps2.m_gpuCapabilities = ctsFps2.m_gpuCapabilities + ", MT: " + SystemInfo.get_maxTextureSize().ToString();
        CTSFps ctsFps3 = this;
        ctsFps3.m_gpuCapabilities = ctsFps3.m_gpuCapabilities + ", NPOT: " + SystemInfo.get_npotSupport().ToString();
        CTSFps ctsFps4 = this;
        ctsFps4.m_gpuCapabilities = ctsFps4.m_gpuCapabilities + ", RTC: " + SystemInfo.get_supportedRenderTargetCount().ToString();
        CTSFps ctsFps5 = this;
        ctsFps5.m_gpuCapabilities = ctsFps5.m_gpuCapabilities + ", CT: " + SystemInfo.get_copyTextureSupport().ToString();
        int graphicsShaderLevel = SystemInfo.get_graphicsShaderLevel();
        if (graphicsShaderLevel >= 10 && graphicsShaderLevel <= 99)
        {
          int num;
          this.m_gpuSpec = "SM: " + (object) (num = graphicsShaderLevel / 10) + (object) '.' + (object) (num / 10);
        }
        else
          this.m_gpuSpec = "SM: N/A";
        int graphicsMemorySize = SystemInfo.get_graphicsMemorySize();
        if (graphicsMemorySize > 0)
        {
          CTSFps ctsFps6 = this;
          ctsFps6.m_gpuSpec = ctsFps6.m_gpuSpec + ", VRAM: " + (object) graphicsMemorySize + " MB";
        }
        else
        {
          CTSFps ctsFps6 = this;
          ctsFps6.m_gpuSpec = ctsFps6.m_gpuSpec + ", VRAM: " + (object) graphicsMemorySize + " N/A";
        }
        int systemMemorySize = SystemInfo.get_systemMemorySize();
        this.m_ram = systemMemorySize <= 0 ? "N/A" : systemMemorySize.ToString();
        Resolution currentResolution = Screen.get_currentResolution();
        this.m_screenInfo = ((Resolution) ref currentResolution).get_width().ToString() + "x" + (object) ((Resolution) ref currentResolution).get_height() + " @" + (object) ((Resolution) ref currentResolution).get_refreshRate() + " Hz [window size: " + (object) Screen.get_width() + "x" + (object) Screen.get_height();
        float dpi = Screen.get_dpi();
        if ((double) dpi > 0.0)
        {
          CTSFps ctsFps6 = this;
          ctsFps6.m_screenInfo = ctsFps6.m_screenInfo + ", DPI: " + (object) dpi + "]";
        }
        else
          this.m_screenInfo += "]";
        this.m_deviceModel = SystemInfo.get_deviceModel();
        this.m_quality = QualitySettings.GetQualityLevel().ToString();
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Problem getting system metrics : " + ex.Message));
      }
      if (Object.op_Inequality((Object) this.m_CTSVersionText, (Object) null))
        this.m_CTSVersionText.set_text(this.m_CTSVersion);
      if (Object.op_Inequality((Object) this.m_OSText, (Object) null))
        this.m_OSText.set_text(this.m_OS);
      if (Object.op_Inequality((Object) this.m_deviceText, (Object) null))
        this.m_deviceText.set_text(this.m_deviceName + ", " + this.m_platform + ", " + this.m_deviceType);
      if (Object.op_Inequality((Object) this.m_systemText, (Object) null))
        this.m_systemText.set_text(this.m_deviceModel + ", " + this.m_processor + ", " + this.m_ram + " GB");
      if (Object.op_Inequality((Object) this.m_gpuText, (Object) null))
        this.m_gpuText.set_text(this.m_gpu + ", " + this.m_gpuSpec + ", QUAL: " + this.m_quality);
      if (Object.op_Inequality((Object) this.m_gpuCapabilitiesText, (Object) null))
        this.m_gpuCapabilitiesText.set_text(this.m_gpuDevice + ", " + this.m_gpuCapabilities);
      if (!Object.op_Inequality((Object) this.m_screenInfoText, (Object) null))
        return;
      this.m_screenInfoText.set_text(this.m_screenInfo);
    }

    private void Update()
    {
      ++this.m_fpsAccumulator;
      if ((double) Time.get_realtimeSinceStartup() <= (double) this.m_fpsNextPeriod)
        return;
      this.m_currentFps = this.m_fpsAccumulator / 1f;
      this.m_currentMs = 1000f / this.m_currentFps;
      this.m_fpsAccumulator = 0.0f;
      this.m_fpsNextPeriod = Time.get_realtimeSinceStartup() + 1f;
      if (!Object.op_Inequality((Object) this.m_fpsText, (Object) null))
        return;
      this.m_fpsText.set_text(string.Format("FPS {0}, MS {1:0.00}", (object) this.m_currentFps, (object) this.m_currentMs));
    }
  }
}
