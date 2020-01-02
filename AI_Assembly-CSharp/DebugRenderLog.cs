// Decompiled with JetBrains decompiler
// Type: DebugRenderLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DebugRenderLog : MonoBehaviour
{
  public Color color;
  private Queue<string> logQueue;
  private const uint LOG_MAX = 20;

  public DebugRenderLog()
  {
    base.\u002Ector();
  }
}
