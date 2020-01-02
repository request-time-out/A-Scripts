// Decompiled with JetBrains decompiler
// Type: AIChara.ChaControlDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  public static class ChaControlDefine
  {
    public static readonly Bounds bounds = new Bounds(new Vector3(0.0f, -2f, 0.0f), new Vector3(20f, 20f, 20f));
    public static readonly string[] extraAcsNames = new string[4]
    {
      "mapAcsHead",
      "mapAcsBack",
      "mapAcsNeck",
      "mapAcsWaist"
    };
    public const string headBoneName = "cf_J_FaceRoot";
    public const string bodyBoneName = "cf_J_Root";
    public const string bodyTopName = "BodyTop";
    public const string AnimeMannequinState = "mannequin";
    public const string AnimeMannequinState02 = "mannequin02";
    public const string objHeadName = "ct_head";
    public const int FaceTexSize = 2048;
    public const int BodyTexSize = 4096;

    public enum ExtraAccessoryParts
    {
      Head,
      Back,
      Neck,
      Waist,
    }

    public enum DynamicBoneKind
    {
      BreastL,
      BreastR,
      HipL,
      HipR,
    }
  }
}
