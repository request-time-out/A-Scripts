// Decompiled with JetBrains decompiler
// Type: Studio.GameScreenShot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  [DefaultExecutionOrder(13000)]
  public class GameScreenShot : MonoBehaviour
  {
    [Button("Capture", "キャプチャー", new object[] {""})]
    public int excuteCapture;
    [Button("UnityCapture", "Unityキャプチャー", new object[] {""})]
    public int excuteCaptureEx;
    [SerializeField]
    private bool _modeARGB;
    [SerializeField]
    private int _capRate;
    [SerializeField]
    private Camera[] renderCam;
    [SerializeField]
    private Image imageCap;
    [SerializeField]
    private bool _capMark;
    public Action captureBeforeFunc;
    public Action captureAfterFunc;
    private string savePath;
    private IDisposable captureDisposable;

    public GameScreenShot()
    {
      base.\u002Ector();
    }

    public bool modeARGB
    {
      get
      {
        return this._modeARGB;
      }
      set
      {
        this._modeARGB = value;
      }
    }

    public int capRate
    {
      get
      {
        return this._capRate;
      }
      set
      {
        this._capRate = value;
      }
    }

    public bool capMark
    {
      get
      {
        return this._capMark;
      }
      set
      {
        this._capMark = value;
      }
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
      this.savePath = path;
      if (this.savePath == string.Empty)
        this.savePath = this.CreateCaptureFileName();
      this.captureDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine(new Func<IEnumerator>(this.CaptureFunc), false), (Action<M0>) (_ => Utility.PlaySE(SoundPack.SystemSE.Photo)), (Action) (() => this.captureDisposable = (IDisposable) null));
    }

    public void UnityCapture(string path = "")
    {
      this.savePath = path;
      if (!(string.Empty == this.savePath))
        return;
      this.savePath = this.CreateCaptureFileName();
    }

    public byte[] CreatePngScreen(int _width, int _height, bool _ARGB = false, bool _cap = false)
    {
      Texture2D texture2D = new Texture2D(_width, _height, !_ARGB ? (TextureFormat) 3 : (TextureFormat) 5, false);
      int num = QualitySettings.get_antiAliasing() != 0 ? QualitySettings.get_antiAliasing() : 1;
      RenderTexture temporary = RenderTexture.GetTemporary(((Texture) texture2D).get_width(), ((Texture) texture2D).get_height(), 24, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, num);
      if (_cap)
        ((Behaviour) this.imageCap).set_enabled(true);
      Graphics.SetRenderTarget(temporary);
      GL.Clear(true, true, Color.get_black());
      Graphics.SetRenderTarget((RenderTexture) null);
      bool sRgbWrite = GL.get_sRGBWrite();
      GL.set_sRGBWrite(true);
      foreach (Camera camera1 in this.renderCam)
      {
        if (!Object.op_Equality((Object) null, (Object) camera1))
        {
          int cullingMask = camera1.get_cullingMask();
          Camera camera2 = camera1;
          camera2.set_cullingMask(camera2.get_cullingMask() & ~(1 << LayerMask.NameToLayer("Studio/Camera")));
          bool enabled = ((Behaviour) camera1).get_enabled();
          RenderTexture targetTexture = camera1.get_targetTexture();
          Rect rect = camera1.get_rect();
          ((Behaviour) camera1).set_enabled(true);
          camera1.set_targetTexture(temporary);
          camera1.Render();
          camera1.set_targetTexture(targetTexture);
          camera1.set_rect(rect);
          ((Behaviour) camera1).set_enabled(enabled);
          camera1.set_cullingMask(cullingMask);
        }
      }
      if (_cap)
        ((Behaviour) this.imageCap).set_enabled(false);
      GL.set_sRGBWrite(sRgbWrite);
      RenderTexture.set_active(temporary);
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).get_width(), (float) ((Texture) texture2D).get_height()), 0, 0);
      texture2D.Apply();
      RenderTexture.set_active((RenderTexture) null);
      byte[] png = ImageConversion.EncodeToPNG(texture2D);
      RenderTexture.ReleaseTemporary(temporary);
      Object.Destroy((Object) texture2D);
      Resources.UnloadUnusedAssets();
      return png;
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
}
