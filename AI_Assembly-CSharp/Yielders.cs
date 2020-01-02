// Decompiled with JetBrains decompiler
// Type: Yielders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public static class Yielders
{
  public static bool Enabled = true;
  public static int _internalCounter = 0;
  private static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
  private static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
  private static Dictionary<float, WaitForSeconds> _waitForSecondsYielders = new Dictionary<float, WaitForSeconds>(100, (IEqualityComparer<float>) new FloatComparer());

  public static WaitForEndOfFrame EndOfFrame
  {
    get
    {
      ++Yielders._internalCounter;
      return Yielders.Enabled ? Yielders._endOfFrame : new WaitForEndOfFrame();
    }
  }

  public static WaitForFixedUpdate FixedUpdate
  {
    get
    {
      ++Yielders._internalCounter;
      return Yielders.Enabled ? Yielders._fixedUpdate : new WaitForFixedUpdate();
    }
  }

  public static WaitForSeconds GetWaitForSeconds(float seconds)
  {
    ++Yielders._internalCounter;
    if (!Yielders.Enabled)
      return new WaitForSeconds(seconds);
    WaitForSeconds waitForSeconds;
    if (!Yielders._waitForSecondsYielders.TryGetValue(seconds, out waitForSeconds))
      Yielders._waitForSecondsYielders.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
    return waitForSeconds;
  }

  public static void ClearWaitForSeconds()
  {
    Yielders._waitForSecondsYielders.Clear();
  }
}
