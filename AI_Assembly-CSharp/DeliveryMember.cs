// Decompiled with JetBrains decompiler
// Type: DeliveryMember
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryMember
{
  public HSceneFlagCtrl ctrlFlag;
  public ChaControl[] chaFemales;
  public ChaControl[] chaMales;
  public CrossFade fade;
  public MetaballCtrl ctrlMeta;
  public HSceneSprite sprite;
  public HItemCtrl item;
  public FeelHit feelHit;
  public HAutoCtrl auto;
  public HVoiceCtrl voice;
  public HParticleCtrl particle;
  public ParticleSystem AtariEffect;
  public ParticleSystem FeelHitEffect3D;
  public HSeCtrl se;
  public List<Tuple<int, int, MotionIK>> lstMotionIK;
}
