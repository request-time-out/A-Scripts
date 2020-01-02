// Decompiled with JetBrains decompiler
// Type: ScreenShot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Text;
using UnityEngine;

public static class ScreenShot
{
  public static void Capture(string path = "", int superSize = 0)
  {
    if (string.Empty == path)
    {
      StringBuilder stringBuilder = new StringBuilder(256);
      string str = UserData.Create("cap");
      stringBuilder.Append(str);
      stringBuilder.Append(DateTime.Now.Year.ToString("0000"));
      stringBuilder.Append(DateTime.Now.Month.ToString("00"));
      stringBuilder.Append(DateTime.Now.Day.ToString("00"));
      stringBuilder.Append(DateTime.Now.Hour.ToString("00"));
      stringBuilder.Append(DateTime.Now.Minute.ToString("00"));
      stringBuilder.Append(DateTime.Now.Second.ToString("00"));
      stringBuilder.Append(DateTime.Now.Millisecond.ToString("000"));
      stringBuilder.Append(".png");
      path = stringBuilder.ToString();
    }
    ScreenCapture.CaptureScreenshot(path, superSize);
    Debug.Log((object) (path + " ScreenShot!"));
  }
}
