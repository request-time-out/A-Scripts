// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine;

namespace AIChara
{
  [MessagePackObject(true)]
  public class ChaFileBody
  {
    public ChaFileBody()
    {
      this.MemberInit();
    }

    public Version version { get; set; }

    public float[] shapeValueBody { get; set; }

    public float bustSoftness { get; set; }

    public float bustWeight { get; set; }

    public int skinId { get; set; }

    public int detailId { get; set; }

    public float detailPower { get; set; }

    public Color skinColor { get; set; }

    public float skinGlossPower { get; set; }

    public float skinMetallicPower { get; set; }

    public int sunburnId { get; set; }

    public Color sunburnColor { get; set; }

    public PaintInfo[] paintInfo { get; set; }

    public int nipId { get; set; }

    public Color nipColor { get; set; }

    public float nipGlossPower { get; set; }

    public float areolaSize { get; set; }

    public int underhairId { get; set; }

    public Color underhairColor { get; set; }

    public Color nailColor { get; set; }

    public float nailGlossPower { get; set; }

    public void MemberInit()
    {
      this.version = ChaFileDefine.ChaFileBodyVersion;
      this.shapeValueBody = new float[ChaFileDefine.cf_bodyshapename.Length];
      for (int index = 0; index < this.shapeValueBody.Length; ++index)
        this.shapeValueBody[index] = ChaFileDefine.cf_bodyInitValue[index];
      this.bustSoftness = 0.5f;
      this.bustWeight = 0.5f;
      this.skinId = 0;
      this.detailId = 0;
      this.detailPower = 0.5f;
      this.skinColor = new Color(0.8f, 0.7f, 0.64f);
      this.skinGlossPower = 0.7f;
      this.skinMetallicPower = 0.0f;
      this.sunburnId = 0;
      this.sunburnColor = Color.get_white();
      this.paintInfo = new PaintInfo[2];
      for (int index = 0; index < 2; ++index)
        this.paintInfo[index] = new PaintInfo();
      this.nipId = 0;
      this.nipColor = new Color(0.76f, 0.52f, 0.52f);
      this.nipGlossPower = 0.6f;
      this.areolaSize = 0.7f;
      this.underhairId = 0;
      this.underhairColor = new Color(0.05f, 0.05f, 0.05f);
      this.nailColor = new Color(1f, 0.92f, 0.92f);
      this.nailGlossPower = 0.6f;
    }

    public void ComplementWithVersion()
    {
      if (this.version < new Version("0.0.1"))
        this.bustWeight *= 0.1f;
      this.version = ChaFileDefine.ChaFileBodyVersion;
    }
  }
}
