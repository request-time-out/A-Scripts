// Decompiled with JetBrains decompiler
// Type: EyeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EyeObject
{
  public Transform eyeTransform;
  public EYE_LR eyeLR;
  internal float angleH;
  internal float angleV;
  internal Vector3 dirUp;
  internal Vector3 referenceLookDir;
  internal Vector3 referenceUpDir;
  internal Quaternion origRotation;
}
