// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileAccessory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine;

namespace AIChara
{
  [MessagePackObject(true)]
  public class ChaFileAccessory
  {
    public ChaFileAccessory()
    {
      this.MemberInit();
    }

    public Version version { get; set; }

    public ChaFileAccessory.PartsInfo[] parts { get; set; }

    public void MemberInit()
    {
      this.version = ChaFileDefine.ChaFileAccessoryVersion;
      this.parts = new ChaFileAccessory.PartsInfo[20];
      for (int index = 0; index < this.parts.Length; ++index)
        this.parts[index] = new ChaFileAccessory.PartsInfo();
    }

    public void ComplementWithVersion()
    {
      this.version = ChaFileDefine.ChaFileAccessoryVersion;
    }

    [MessagePackObject(true)]
    public class PartsInfo
    {
      public PartsInfo()
      {
        this.MemberInit();
      }

      public int type { get; set; }

      public int id { get; set; }

      public string parentKey { get; set; }

      public Vector3[,] addMove { get; set; }

      public ChaFileAccessory.PartsInfo.ColorInfo[] colorInfo { get; set; }

      public int hideCategory { get; set; }

      public int hideTiming { get; set; }

      public bool noShake { get; set; }

      [IgnoreMember]
      public bool partsOfHead { get; set; }

      public void MemberInit()
      {
        this.type = 120;
        this.id = 0;
        this.parentKey = string.Empty;
        this.addMove = new Vector3[2, 3];
        for (int index = 0; index < 2; ++index)
        {
          this.addMove[index, 0] = Vector3.get_zero();
          this.addMove[index, 1] = Vector3.get_zero();
          this.addMove[index, 2] = Vector3.get_one();
        }
        this.colorInfo = new ChaFileAccessory.PartsInfo.ColorInfo[4];
        for (int index = 0; index < this.colorInfo.Length; ++index)
          this.colorInfo[index] = new ChaFileAccessory.PartsInfo.ColorInfo();
        this.hideCategory = 0;
        this.hideTiming = 1;
        this.partsOfHead = false;
        this.noShake = false;
      }

      [MessagePackObject(true)]
      public class ColorInfo
      {
        public ColorInfo()
        {
          this.MemberInit();
        }

        public Color color { get; set; }

        public float glossPower { get; set; }

        public float metallicPower { get; set; }

        public float smoothnessPower { get; set; }

        public void MemberInit()
        {
          this.color = Color.get_white();
          this.glossPower = 0.5f;
          this.metallicPower = 0.5f;
          this.smoothnessPower = 0.5f;
        }
      }
    }
  }
}
