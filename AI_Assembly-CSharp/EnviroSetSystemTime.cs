// Decompiled with JetBrains decompiler
// Type: EnviroSetSystemTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class EnviroSetSystemTime : MonoBehaviour
{
  public EnviroSetSystemTime()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!Object.op_Inequality((Object) EnviroSky.instance, (Object) null))
      return;
    EnviroSky.instance.SetTime(DateTime.Now);
  }
}
