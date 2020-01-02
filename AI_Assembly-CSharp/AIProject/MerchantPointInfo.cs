// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantPointInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using System;

namespace AIProject
{
  [Serializable]
  public struct MerchantPointInfo
  {
    public string actionName;
    public int pointID;
    public int eventID;
    public Merchant.EventType eventTypeMask;
    public int poseID;
    public bool isTalkable;
    public bool isLooking;
    public string baseNullName;
    public string recoveryNullName;
  }
}
