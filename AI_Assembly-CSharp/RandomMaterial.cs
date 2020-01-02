// Decompiled with JetBrains decompiler
// Type: RandomMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
  public Renderer targetRenderer;
  public Material[] materials;

  public RandomMaterial()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    this.ChangeMaterial();
  }

  public void ChangeMaterial()
  {
    this.targetRenderer.set_sharedMaterial(this.materials[Random.Range(0, this.materials.Length)]);
  }
}
