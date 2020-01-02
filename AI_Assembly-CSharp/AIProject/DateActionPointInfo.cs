// Decompiled with JetBrains decompiler
// Type: AIProject.DateActionPointInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace AIProject
{
  [Serializable]
  public struct DateActionPointInfo
  {
    public string actionName;
    public int pointID;
    public int eventID;
    public EventType eventTypeMask;
    public int iconID;
    public int poseIDA;
    public int poseIDB;
    public bool isTalkable;
    public int cameraID;
    public string baseNullNameA;
    public string baseNullNameB;
    public string recoveryNullNameA;
    public string recoveryNullNameB;
    public string labelNullName;
    public int searchAreaID;
    public int gradeValue;
    public int doorOpenType;
  }
}
