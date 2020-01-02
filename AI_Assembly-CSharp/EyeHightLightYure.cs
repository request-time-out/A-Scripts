// Decompiled with JetBrains decompiler
// Type: EyeHightLightYure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class EyeHightLightYure : MonoBehaviour
{
  public EyeLookMaterialControll eyeLookMaterialCtrl;
  public int Inside;
  public int Outside;
  public int Up;
  public int Down;
  public Rect rect;
  private Material _material;
  private int textureWidth;
  private int textureHeight;
  private Vector2 offset;
  private Vector2 scale;

  public EyeHightLightYure()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this._material = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_material();
    Texture mainTexture = this._material.get_mainTexture();
    this.textureWidth = mainTexture.get_width();
    this.textureHeight = mainTexture.get_height();
    ref Rect local = ref this.rect;
    float num1 = 0.0f;
    ((Rect) ref this.rect).set_y(num1);
    double num2 = (double) num1;
    ((Rect) ref local).set_x((float) num2);
    ((Rect) ref this.rect).set_width((float) this.textureWidth);
    ((Rect) ref this.rect).set_height((float) this.textureHeight);
    this.offset = new Vector2(0.0f, 0.0f);
    this.scale = new Vector2(1f, 1f);
    Debug.Log((object) (this.textureWidth.ToString() + "/" + (object) this.textureHeight));
  }

  private void FixedUpdate()
  {
    if (Object.op_Inequality((Object) this.eyeLookMaterialCtrl, (Object) null))
      this.offset = this.eyeLookMaterialCtrl.GetEyeTexOffset();
    ((Rect) ref this.rect).set_x((float) Random.Range(this.Inside, this.Outside));
    ((Rect) ref this.rect).set_y((float) Random.Range(this.Up, this.Down));
    EyeHightLightYure eyeHightLightYure = this;
    eyeHightLightYure.offset = Vector2.op_Addition(eyeHightLightYure.offset, new Vector2(((Rect) ref this.rect).get_x() / (float) this.textureWidth, ((Rect) ref this.rect).get_y() / (float) this.textureHeight));
    this.scale = new Vector2(((Rect) ref this.rect).get_width() / (float) this.textureWidth, ((Rect) ref this.rect).get_height() / (float) this.textureHeight);
    this._material.SetTextureOffset("_MainTex", this.offset);
    this._material.SetTextureScale("_MainTex", this.scale);
  }
}
