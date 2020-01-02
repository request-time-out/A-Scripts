// Decompiled with JetBrains decompiler
// Type: EffectsParticleSystemScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class EffectsParticleSystemScaler : MonoBehaviour
{
  public float particlesScale;
  private float oldScale;

  public EffectsParticleSystemScaler()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.oldScale = this.particlesScale;
  }

  private void Update()
  {
  }
}
