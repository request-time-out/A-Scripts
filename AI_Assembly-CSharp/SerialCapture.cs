// Decompiled with JetBrains decompiler
// Type: SerialCapture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class SerialCapture : MonoBehaviour
{
  public string Name;
  public int FrameRate;
  public int SuperSize;
  public int EndCount;
  public bool AutoRecord;
  public KeyCode ExitKey;
  public Vector2 posCapBtn;
  private string captureDirectory;
  private string serialId;
  private int frameCount;
  private bool recording;

  public SerialCapture()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.captureDirectory = UserData.Create(nameof (SerialCapture));
  }

  private void Start()
  {
    if (!this.AutoRecord)
      return;
    this.StartRecording();
  }

  public bool GetRecording()
  {
    return this.recording;
  }

  private void StartRecording()
  {
    this.serialId = DateTime.Now.Minute.ToString("00");
    this.serialId += DateTime.Now.Second.ToString("00");
    this.serialId += "_";
    Time.set_captureFramerate(1000 / this.FrameRate);
    this.frameCount = -1;
    this.recording = true;
    Debug.Log((object) "連続キャプチャー開始");
  }

  private void EndRecording()
  {
    Time.set_captureFramerate(0);
    this.recording = false;
    Debug.Log((object) "連続キャプチャー終了");
  }

  private void Update()
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

  private void OnGUI()
  {
    if (this.recording)
    {
      if (!Input.GetKeyDown(this.ExitKey))
        return;
      this.EndRecording();
    }
    else
    {
      if (!GUI.Button(new Rect((float) this.posCapBtn.x, (float) this.posCapBtn.y, 200f, 30f), "Start SerialCapture"))
        return;
      this.StartRecording();
    }
  }
}
