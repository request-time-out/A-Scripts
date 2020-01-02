// Decompiled with JetBrains decompiler
// Type: CrossFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.CustomAttributes;
using UnityEngine;

public class CrossFade : MonoBehaviour
{
  [Label("CrossFadeマテリアル")]
  public Material materiaEffect;
  [Label("フェード時間")]
  public float time;
  [Header("Debug表示")]
  [SerializeField]
  private RenderTexture texBase;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float alpha;
  [SerializeField]
  [NotEditable]
  private float timer;
  private float timeCalc;
  private int _FadeTex;
  private int _Alpha;
  private bool isProcess;
  [Button("FadeStart", "FadeStart", new object[] {-1})]
  public int FadeStartButton;

  public CrossFade()
  {
    base.\u002Ector();
  }

  public bool isEnd { get; private set; }

  public void FadeStart(float time = -1f)
  {
    if (Object.op_Equality((Object) this.texBase, (Object) null))
      return;
    this.timeCalc = (double) time >= 0.0 ? time : this.time;
    if (Mathf.Approximately(this.timeCalc, 0.0f))
      return;
    this.timer = 0.0f;
    this.alpha = 0.0f;
    this.isProcess = true;
    this.isEnd = false;
    Debug.LogFormat("CrossFadeStart\nTime : {0}", new object[1]
    {
      (object) this.timeCalc
    });
  }

  public void End()
  {
    this.timer = this.timeCalc;
    this.isEnd = true;
    this.alpha = 1f;
  }

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.texBase, (Object) null))
      return;
    this.texBase.Release();
  }

  private void Start()
  {
    this._FadeTex = Shader.PropertyToID("_FadeTex");
    this._Alpha = Shader.PropertyToID("_Alpha");
    this.isProcess = false;
    this.isEnd = true;
    this.texBase = new RenderTexture(Screen.get_width(), Screen.get_height(), 24, (RenderTextureFormat) 0, (RenderTextureReadWrite) 0);
  }

  private void Update()
  {
    if (!this.isProcess)
      return;
    this.timer = Mathf.Clamp(this.timer + Time.get_smoothDeltaTime(), 0.0f, this.timeCalc);
    this.isEnd = (double) this.timer >= (double) this.timeCalc;
    if (this.isEnd)
      this.alpha = 1f;
    else
      this.alpha = this.timer / this.timeCalc;
  }

  private void OnRenderImage(RenderTexture src, RenderTexture dst)
  {
    if (Object.op_Equality((Object) this.texBase, (Object) null))
      Graphics.Blit((Texture) src, dst);
    else if (!this.isProcess)
    {
      Graphics.Blit((Texture) src, this.texBase);
      Graphics.Blit((Texture) src, dst);
    }
    else
    {
      this.materiaEffect.SetTexture(this._FadeTex, (Texture) this.texBase);
      this.materiaEffect.SetFloat(this._Alpha, this.alpha);
      Graphics.Blit((Texture) src, dst, this.materiaEffect);
      this.isProcess = !this.isEnd;
    }
  }
}
