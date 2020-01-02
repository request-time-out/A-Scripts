// Decompiled with JetBrains decompiler
// Type: LogCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class LogCallback : MonoBehaviour
{
  [SerializeField]
  private bool isDraw;
  [SerializeField]
  private bool isLeft;
  [SerializeField]
  private bool isUp;
  private bool isGuiArea;
  private LogType level;

  public LogCallback()
  {
    base.\u002Ector();
  }
}
