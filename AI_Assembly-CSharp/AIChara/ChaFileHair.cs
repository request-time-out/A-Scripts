// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileHair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIChara
{
  [MessagePackObject(true)]
  public class ChaFileHair
  {
    public ChaFileHair()
    {
      this.MemberInit();
    }

    public Version version { get; set; }

    public bool sameSetting { get; set; }

    public bool autoSetting { get; set; }

    public bool ctrlTogether { get; set; }

    public ChaFileHair.PartsInfo[] parts { get; set; }

    public int kind { get; set; }

    public int shaderType { get; set; }

    public void MemberInit()
    {
      this.version = ChaFileDefine.ChaFileHairVersion;
      this.sameSetting = true;
      this.autoSetting = true;
      this.ctrlTogether = false;
      this.parts = new ChaFileHair.PartsInfo[Enum.GetValues(typeof (ChaFileDefine.HairKind)).Length];
      for (int index = 0; index < this.parts.Length; ++index)
        this.parts[index] = new ChaFileHair.PartsInfo();
      this.kind = 0;
      this.shaderType = 0;
    }

    public void ComplementWithVersion()
    {
      if (this.version < new Version("0.0.1"))
      {
        for (int index1 = 0; index1 < this.parts.Length; ++index1)
        {
          this.parts[index1].acsColorInfo = new ChaFileHair.PartsInfo.ColorInfo[4];
          for (int index2 = 0; index2 < this.parts[index1].acsColorInfo.Length; ++index2)
            this.parts[index1].acsColorInfo[index2] = new ChaFileHair.PartsInfo.ColorInfo();
        }
      }
      if (this.version < new Version("0.0.2"))
      {
        this.sameSetting = true;
        this.autoSetting = true;
        this.ctrlTogether = false;
      }
      if (this.version < new Version("0.0.3"))
      {
        ChaFileHair.PartsInfo.BundleInfo bundleInfo;
        if (this.parts[0].id == 4)
        {
          if (this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
            bundleInfo.rotRate = new Vector3((float) bundleInfo.rotRate.x, (float) (1.0 - bundleInfo.rotRate.y), (float) bundleInfo.rotRate.z);
        }
        else if (this.parts[0].id == 5)
        {
          if (this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
          {
            float num = Mathf.InverseLerp(-30f, 30f, Mathf.Lerp(3f, 30f, (float) bundleInfo.rotRate.z));
            bundleInfo.rotRate = new Vector3((float) bundleInfo.rotRate.x, (float) bundleInfo.rotRate.y, num);
          }
          if (this.parts[0].dictBundle.TryGetValue(1, out bundleInfo))
          {
            float num = Mathf.InverseLerp(0.1f, -0.4f, Mathf.Lerp(0.1f, -0.1f, (float) bundleInfo.moveRate.z));
            bundleInfo.moveRate = new Vector3((float) bundleInfo.moveRate.x, (float) bundleInfo.moveRate.y, num);
          }
          if (this.parts[0].dictBundle.TryGetValue(2, out bundleInfo))
          {
            float num = Mathf.InverseLerp(-25f, 50f, Mathf.Lerp(-25f, 45f, (float) bundleInfo.rotRate.x));
            bundleInfo.rotRate = new Vector3(num, (float) bundleInfo.rotRate.y, (float) bundleInfo.rotRate.z);
          }
          if (this.parts[0].dictBundle.TryGetValue(3, out bundleInfo))
          {
            float num1 = Mathf.InverseLerp(-0.1f, 0.4f, Mathf.Lerp(-0.1f, -0.4f, (float) bundleInfo.moveRate.z));
            bundleInfo.moveRate = new Vector3((float) bundleInfo.moveRate.x, (float) bundleInfo.moveRate.y, num1);
            float num2 = Mathf.InverseLerp(45f, -22.5f, Mathf.Lerp(-22.5f, 45f, (float) bundleInfo.rotRate.x));
            bundleInfo.rotRate = new Vector3(num2, (float) bundleInfo.rotRate.y, (float) bundleInfo.rotRate.z);
          }
          if (this.parts[0].dictBundle.TryGetValue(4, out bundleInfo))
          {
            float num = Mathf.InverseLerp(45f, -22.5f, Mathf.Lerp(-22.5f, 45f, (float) bundleInfo.rotRate.x));
            bundleInfo.rotRate = new Vector3(num, (float) bundleInfo.rotRate.y, (float) bundleInfo.rotRate.z);
          }
        }
        else if (this.parts[0].id == 8 && this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
          bundleInfo.rotRate = new Vector3((float) bundleInfo.rotRate.x, (float) (1.0 - bundleInfo.rotRate.y), (float) bundleInfo.rotRate.z);
        if (this.parts[1].id == 7 && this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
        {
          float num = Mathf.InverseLerp(70f, -35f, Mathf.Lerp(-70f, 35f, (float) bundleInfo.rotRate.y));
          bundleInfo.rotRate = new Vector3((float) bundleInfo.rotRate.x, num, (float) bundleInfo.rotRate.z);
        }
      }
      this.version = ChaFileDefine.ChaFileHairVersion;
    }

    [MessagePackObject(true)]
    public class PartsInfo
    {
      public PartsInfo()
      {
        this.MemberInit();
      }

      public int id { get; set; }

      public Color baseColor { get; set; }

      public Color topColor { get; set; }

      public Color underColor { get; set; }

      public Color specular { get; set; }

      public float metallic { get; set; }

      public float smoothness { get; set; }

      public ChaFileHair.PartsInfo.ColorInfo[] acsColorInfo { get; set; }

      public int bundleId { get; set; }

      public Dictionary<int, ChaFileHair.PartsInfo.BundleInfo> dictBundle { get; set; }

      public void MemberInit()
      {
        this.id = 0;
        this.baseColor = new Color(0.2f, 0.2f, 0.2f);
        this.topColor = new Color(0.039f, 0.039f, 0.039f);
        this.underColor = new Color(0.565f, 0.565f, 0.565f);
        this.specular = new Color(0.3f, 0.3f, 0.3f);
        this.metallic = 0.0f;
        this.smoothness = 0.0f;
        this.acsColorInfo = new ChaFileHair.PartsInfo.ColorInfo[4];
        for (int index = 0; index < this.acsColorInfo.Length; ++index)
          this.acsColorInfo[index] = new ChaFileHair.PartsInfo.ColorInfo();
        this.bundleId = -1;
        this.dictBundle = new Dictionary<int, ChaFileHair.PartsInfo.BundleInfo>();
      }

      [MessagePackObject(true)]
      public class BundleInfo
      {
        public BundleInfo()
        {
          this.MemberInit();
        }

        public Vector3 moveRate { get; set; }

        public Vector3 rotRate { get; set; }

        public bool noShake { get; set; }

        public void MemberInit()
        {
          this.moveRate = Vector3.get_zero();
          this.rotRate = Vector3.get_zero();
          this.noShake = false;
        }
      }

      [MessagePackObject(true)]
      public class ColorInfo
      {
        public ColorInfo()
        {
          this.MemberInit();
        }

        public Color color { get; set; }

        public void MemberInit()
        {
          this.color = Color.get_white();
        }
      }
    }
  }
}
