// Decompiled with JetBrains decompiler
// Type: SerialCaptureEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class SerialCaptureEx
{
  public string Name = "1000/FrameRateして整数にならないとダメ";
  public int FrameRate = 25;
  public int EndCount = -1;
  private string captureDirectory = string.Empty;
  private string serialId = string.Empty;
  private int frameCount = -1;
  public int SuperSize;
  private bool recording;

  public SerialCaptureEx()
  {
    this.captureDirectory = UserData.Create("SerialCapture");
  }

  public void StartRecording()
  {
    this.serialId = DateTime.Now.Minute.ToString("00");
    this.serialId += DateTime.Now.Second.ToString("00");
    this.serialId += "_";
    Time.set_captureFramerate(1000 / this.FrameRate);
    this.frameCount = -1;
    this.recording = true;
    Debug.Log((object) "連続キャプチャー開始");
  }

  public void EndRecording()
  {
    Time.set_captureFramerate(0);
    this.recording = false;
    Debug.Log((object) "連続キャプチャー終了");
  }

  public void Update()
  {
    if (!this.recording)
      return;
    if (0 < this.frameCount)
      ScreenCapture.CaptureScreenshot(this.captureDirectory + "/" + this.serialId + this.frameCount.ToString("0000") + ".png", this.SuperSize);
    ++this.frameCount;
    if (0 < this.frameCount && this.frameCount % this.FrameRate == 0)
    {
      int num = this.frameCount / this.FrameRate;
      Debug.Log((object) ("経過時間" + (num / 60).ToString("D2") + "分" + (num % 60).ToString("D2") + "秒"));
    }
    if (this.EndCount == -1 || this.frameCount <= this.EndCount)
      return;
    this.EndRecording();
  }
}
