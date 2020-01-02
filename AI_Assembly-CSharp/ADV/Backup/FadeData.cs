// Decompiled with JetBrains decompiler
// Type: ADV.Backup.FadeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using UnityEngine;

namespace ADV.Backup
{
  internal class FadeData
  {
    private Color color;
    private float time;
    private Texture2D texture;

    public FadeData(SimpleFade fade)
    {
      this.color = fade._Color;
      this.time = fade._Time;
      this.texture = fade._Texture;
    }

    public void Load(SimpleFade fade)
    {
      fade._Color = fade._Color.Get(this.color, false, true, true, true);
      fade._Time = this.time;
      fade._Texture = this.texture;
    }
  }
}
