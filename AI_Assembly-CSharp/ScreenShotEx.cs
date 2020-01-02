// Decompiled with JetBrains decompiler
// Type: ScreenShotEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ScreenShotEx : MonoBehaviour
{
  private bool capFlag;
  private ScreenShotEx.SSInfo ssinfo;

  public ScreenShotEx()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (!this.capFlag)
      return;
    this.capFlag = false;
    this.StartCoroutine(this.CaptureProc(this.ssinfo));
  }

  [DebuggerHidden]
  private IEnumerator CaptureProc(ScreenShotEx.SSInfo ssinfo)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ScreenShotEx.\u003CCaptureProc\u003Ec__Iterator0()
    {
      ssinfo = ssinfo,
      \u0024this = this
    };
  }

  public static void Capture(string _path, bool _alpha, int _width, int _height, int _rate)
  {
    GameObject gameObject = new GameObject("CapExObj");
    if (!Object.op_Implicit((Object) gameObject))
      return;
    ScreenShotEx screenShotEx = (ScreenShotEx) gameObject.AddComponent<ScreenShotEx>();
    screenShotEx.ssinfo.Set(_path, _alpha, _width, _height, _rate);
    screenShotEx.capFlag = true;
  }

  public class SSInfo
  {
    public string path = string.Empty;
    public int rate = 1;
    public bool alpha;
    public int width;
    public int height;

    public void Set(string _path, bool _alpha, int _width, int _height, int _rate)
    {
      this.path = _path;
      this.alpha = _alpha;
      this.width = _width;
      this.height = _height;
      this.rate = _rate;
    }
  }
}
