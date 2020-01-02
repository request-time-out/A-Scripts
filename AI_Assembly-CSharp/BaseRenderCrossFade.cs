// Decompiled with JetBrains decompiler
// Type: BaseRenderCrossFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class BaseRenderCrossFade : MonoBehaviour
{
  public Camera uiCamera;
  public Camera targetCamera;
  protected float maxTime;
  protected float timer;
  protected bool isAlphaAdd;
  public RenderTexture texture;
  protected bool isInitRenderSetting;

  public BaseRenderCrossFade()
  {
    base.\u002Ector();
  }

  public bool isDrawGUI { get; protected set; }

  public float alpha { get; protected set; }

  public float MaxTime
  {
    set
    {
      this.maxTime = value;
      this.timer = 0.0f;
    }
  }

  protected void AlphaCalc()
  {
    this.alpha = Mathf.InverseLerp(0.0f, this.maxTime, this.timer);
    if (this.isAlphaAdd)
      return;
    this.alpha = Mathf.Lerp(1f, 0.0f, this.alpha);
  }

  public void Capture()
  {
    if (Object.op_Inequality((Object) this.texture, (Object) null) && (((Texture) this.texture).get_width() != Screen.get_width() || ((Texture) this.texture).get_height() != Screen.get_height()))
      this.CreateRenderTexture();
    if (!this.isInitRenderSetting)
      this.RenderTargetSetting();
    if (Object.op_Inequality((Object) this.targetCamera, (Object) null))
    {
      RenderTexture targetTexture = this.targetCamera.get_targetTexture();
      Rect rect = this.targetCamera.get_rect();
      this.targetCamera.set_targetTexture(this.texture);
      this.targetCamera.Render();
      this.targetCamera.set_targetTexture(targetTexture);
      this.targetCamera.set_rect(rect);
    }
    if (Object.op_Inequality((Object) this.uiCamera, (Object) null))
    {
      RenderTexture targetTexture = this.uiCamera.get_targetTexture();
      Rect rect = this.uiCamera.get_rect();
      this.uiCamera.set_targetTexture(this.texture);
      this.uiCamera.Render();
      this.uiCamera.set_targetTexture(targetTexture);
      this.uiCamera.set_rect(rect);
    }
    this.timer = 0.0f;
    this.isDrawGUI = false;
    this.AlphaCalc();
  }

  public virtual void End()
  {
    this.timer = this.maxTime;
    this.AlphaCalc();
  }

  public void Destroy()
  {
    this.ReleaseRenderTexture();
  }

  protected virtual void Awake()
  {
    this.CreateRenderTexture();
    this.RenderTargetSetting();
    this.isDrawGUI = false;
  }

  protected virtual void Update()
  {
    this.timer += Time.get_deltaTime();
    this.timer = Mathf.Min(this.timer, this.maxTime);
    this.AlphaCalc();
  }

  protected virtual void OnGUI()
  {
    GUI.set_depth(10);
    GUI.set_color(new Color(1f, 1f, 1f, this.alpha));
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.get_width(), (float) Screen.get_height()), (Texture) this.texture);
    this.isDrawGUI = true;
  }

  private void CreateRenderTexture()
  {
    this.ReleaseRenderTexture();
    this.texture = new RenderTexture(Screen.get_width(), Screen.get_height(), 24, (RenderTextureFormat) 4);
    this.texture.set_antiAliasing(QualitySettings.get_antiAliasing() != 0 ? QualitySettings.get_antiAliasing() : 1);
    this.texture.set_enableRandomWrite(false);
  }

  private void ReleaseRenderTexture()
  {
    if (!Object.op_Inequality((Object) this.texture, (Object) null))
      return;
    this.texture.Release();
    Object.Destroy((Object) this.texture);
    this.texture = (RenderTexture) null;
  }

  private void RenderTargetSetting()
  {
    if (Object.op_Equality((Object) this.uiCamera, (Object) null))
    {
      GameObject gameObject = GameObject.Find("SpDef");
      if (Object.op_Implicit((Object) gameObject))
        this.uiCamera = (Camera) gameObject.GetComponent<Camera>();
    }
    if (!Object.op_Equality((Object) this.targetCamera, (Object) null))
      return;
    this.targetCamera = Camera.get_main();
  }
}
