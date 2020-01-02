// Decompiled with JetBrains decompiler
// Type: AIChara.PaintInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using UnityEngine;

namespace AIChara
{
  [MessagePackObject(true)]
  public class PaintInfo
  {
    public PaintInfo()
    {
      this.MemberInit();
    }

    public int id { get; set; }

    public Color color { get; set; }

    public float glossPower { get; set; }

    public float metallicPower { get; set; }

    public int layoutId { get; set; }

    public Vector4 layout { get; set; }

    public float rotation { get; set; }

    public void MemberInit()
    {
      this.id = 0;
      this.color = Color.get_red();
      this.glossPower = 0.5f;
      this.metallicPower = 0.5f;
      this.layoutId = 0;
      this.layout = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
      this.rotation = 0.5f;
    }

    public void Copy(PaintInfo src)
    {
      this.id = src.id;
      this.color = src.color;
      this.glossPower = src.glossPower;
      this.metallicPower = src.metallicPower;
      this.layoutId = src.layoutId;
      this.layout = src.layout;
      this.rotation = src.rotation;
    }
  }
}
