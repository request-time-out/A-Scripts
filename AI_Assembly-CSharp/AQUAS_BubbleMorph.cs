// Decompiled with JetBrains decompiler
// Type: AQUAS_BubbleMorph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class AQUAS_BubbleMorph : MonoBehaviour
{
  private float t;
  private float t2;
  [Space(5f)]
  [Header("Duration of a full morphing cycle")]
  public float tTarget;
  private SkinnedMeshRenderer skinnedMeshRenderer;

  public AQUAS_BubbleMorph()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.skinnedMeshRenderer = (SkinnedMeshRenderer) ((Component) this).GetComponent<SkinnedMeshRenderer>();
  }

  private void Update()
  {
    this.t += Time.get_deltaTime();
    this.t2 += Time.get_deltaTime();
    if ((double) this.t < (double) this.tTarget / 2.0)
    {
      this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0.0f, 50f, this.t / (this.tTarget / 2f)));
      this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(50f, 0.0f, this.t / (this.tTarget / 2f)));
    }
    else if ((double) this.t >= (double) this.tTarget / 2.0 && (double) this.t < (double) this.tTarget)
    {
      this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(50f, 100f, this.t / this.tTarget));
      this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(0.0f, 50f, this.t / this.tTarget));
    }
    else if ((double) this.t >= (double) this.tTarget && (double) this.t < (double) this.tTarget * 1.5)
    {
      this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(100f, 50f, this.t / (this.tTarget * 1.5f)));
      this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(50f, 100f, this.t / (this.tTarget * 1.5f)));
    }
    else if ((double) this.t >= (double) this.tTarget * 1.5 && (double) this.t < (double) this.tTarget * 2.0)
    {
      this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(50f, 0.0f, this.t / (this.tTarget * 2f)));
      this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(100f, 50f, this.t / (this.tTarget * 2f)));
    }
    else
      this.t = 0.0f;
  }
}
