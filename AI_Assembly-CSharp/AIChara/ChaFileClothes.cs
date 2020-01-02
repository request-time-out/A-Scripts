// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileClothes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine;

namespace AIChara
{
  [MessagePackObject(true)]
  public class ChaFileClothes
  {
    public ChaFileClothes()
    {
      this.MemberInit();
    }

    public Version version { get; set; }

    public ChaFileClothes.PartsInfo[] parts { get; set; }

    public void MemberInit()
    {
      this.version = ChaFileDefine.ChaFileClothesVersion;
      this.parts = new ChaFileClothes.PartsInfo[Enum.GetValues(typeof (ChaFileDefine.ClothesKind)).Length];
      for (int index = 0; index < this.parts.Length; ++index)
        this.parts[index] = new ChaFileClothes.PartsInfo();
    }

    public void ComplementWithVersion()
    {
      this.version = ChaFileDefine.ChaFileClothesVersion;
    }

    [MessagePackObject(true)]
    public class PartsInfo
    {
      public PartsInfo()
      {
        this.MemberInit();
      }

      public int id { get; set; }

      public ChaFileClothes.PartsInfo.ColorInfo[] colorInfo { get; set; }

      public float breakRate { get; set; }

      public bool[] hideOpt { get; set; }

      public void MemberInit()
      {
        this.id = 0;
        this.colorInfo = new ChaFileClothes.PartsInfo.ColorInfo[3];
        for (int index = 0; index < this.colorInfo.Length; ++index)
          this.colorInfo[index] = new ChaFileClothes.PartsInfo.ColorInfo();
        this.breakRate = 0.0f;
        this.hideOpt = new bool[2];
      }

      [MessagePackObject(true)]
      public class ColorInfo
      {
        public ColorInfo()
        {
          this.MemberInit();
        }

        public Color baseColor { get; set; }

        public int pattern { get; set; }

        public Vector4 layout { get; set; }

        public float rotation { get; set; }

        public Color patternColor { get; set; }

        public float glossPower { get; set; }

        public float metallicPower { get; set; }

        public void MemberInit()
        {
          this.baseColor = Color.get_white();
          this.pattern = 0;
          this.layout = new Vector4(1f, 1f, 0.0f, 0.0f);
          this.rotation = 0.5f;
          this.patternColor = Color.get_white();
          this.glossPower = 0.5f;
          this.metallicPower = 0.0f;
        }
      }
    }
  }
}
