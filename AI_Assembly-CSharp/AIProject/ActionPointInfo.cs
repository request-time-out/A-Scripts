// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIProject
{
  [Serializable]
  public struct ActionPointInfo
  {
    public string actionName;
    public int pointID;
    public int eventID;
    public EventType eventTypeMask;
    public int iconID;
    public int poseID;
    public int datePoseID;
    public bool isTalkable;
    public int cameraID;
    public string baseNullName;
    public string recoveryNullName;
    public string labelNullName;
    public int searchAreaID;
    public int gradeValue;
    public int doorOpenType;
  }
}
