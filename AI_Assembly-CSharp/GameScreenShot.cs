// Decompiled with JetBrains decompiler
// Type: GameScreenShot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UniRx;
using UnityEngine;

public class GameScreenShot : MonoBehaviour
{
  [Button("Capture", "キャプチャー", new object[] {""})]
  public int excuteCapture;
  [Button("UnityCapture", "Unityキャプチャー", new object[] {""})]
  public int excuteCaptureEx;
  public bool capExMode;
  public bool modeARGB;
  public Camera[] renderCam;
  public GameScreenShotAssist[] scriptGssAssist;
  public Texture texCapMark;
  public Vector2 texPosition;
  public int capRate;
  private string savePath;
  private IDisposable captureDisposable;

  public GameScreenShot()
  {
    base.\u002Ector();
  }

  public string CreateCaptureFileName()
  {
    StringBuilder stringBuilder = new StringBuilder(256);
    stringBuilder.Append(UserData.Create("cap"));
    DateTime now = DateTime.Now;
    stringBuilder.Append(now.Year.ToString("0000"));
    stringBuilder.Append(now.Month.ToString("00"));
    stringBuilder.Append(now.Day.ToString("00"));
    stringBuilder.Append(now.Hour.ToString("00"));
    stringBuilder.Append(now.Minute.ToString("00"));
    stringBuilder.Append(now.Second.ToString("00"));
    stringBuilder.Append(now.Millisecond.ToString("000"));
    stringBuilder.Append(".png");
    return stringBuilder.ToString();
  }

  public void Capture(string path = "")
  {
    if (this.captureDisposable != null || Singleton<Scene>.IsInstance() && Singleton<Scene>.Instance.IsNowLoadingFade)
      return;
    bool isRenderSetCam = false;
    if (!this.capExMode)
    {
      if (((IList<Camera>) this.renderCam).IsNullOrEmpty<Camera>())
      {
        if (Object.op_Equality((Object) Camera.get_main(), (Object) null))
          return;
        isRenderSetCam = true;
        this.renderCam = new Camera[1]{ Camera.get_main() };
      }
    }
    else if (this.scriptGssAssist.Length == 0)
      return;
    this.savePath = path;
    if (this.savePath == string.Empty)
      this.savePath = this.CreateCaptureFileName();
    this.captureDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine(new Func<IEnumerator>(this.CaptureFunc), false), (Action<M0>) (_ =>
    {
      if (isRenderSetCam)
        this.renderCam = (Camera[]) null;
      this.captureDisposable = (IDisposable) null;
    }));
  }

  public void UnityCapture(string path = "")
  {
    this.savePath = path;
    if (string.Empty == this.savePath)
      this.savePath = this.CreateCaptureFileName();
    ScreenCapture.CaptureScreenshot(this.savePath, this.capRate);
  }

  [DebuggerHidden]
  private IEnumerator CaptureFunc()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new GameScreenShot.\u003CCaptureFunc\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
