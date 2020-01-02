// Decompiled with JetBrains decompiler
// Type: Illusion.Component.ScreenShot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Illusion.Component
{
  public class ScreenShot : MonoBehaviour
  {
    [Button("Capture", "キャプチャー", new object[] {""})]
    public int excuteCapture;
    public int capRate;
    public Texture texCapMark;
    public List<ScreenShotCamera> ssCamList;

    public ScreenShot()
    {
      base.\u002Ector();
    }

    public void Capture(string path = null)
    {
      if (path.IsNullOrEmpty())
        path = Illusion.Game.Utils.ScreenShot.Path;
      this.StartCoroutine(Illusion.Game.Utils.ScreenShot.CaptureGSS(this.ssCamList, path, this.texCapMark, this.capRate));
    }
  }
}
