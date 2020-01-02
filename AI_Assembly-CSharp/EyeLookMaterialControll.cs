// Decompiled with JetBrains decompiler
// Type: EyeLookMaterialControll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class EyeLookMaterialControll : MonoBehaviour
{
  public EyeLookCalc script;
  public int InsideWait;
  public int OutsideWait;
  public int UpWait;
  public int DownWait;
  public float InsideLimit;
  public float OutsideLimit;
  public float UpLimit;
  public float DownLimit;
  public EYE_LR eyeLR;
  public Rect Limit;
  public Rect rect;
  private Material _material;
  private int textureWidth;
  private int textureHeight;
  private Vector2 offset;
  private Vector2 scale;

  public EyeLookMaterialControll()
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
    this.offset = (Vector2) null;
    this.scale = (Vector2) null;
    Debug.Log((object) (this.textureWidth.ToString() + "/" + (object) this.textureHeight));
  }

  private void Update()
  {
    ((Rect) ref this.rect).set_x(Mathf.Lerp((float) this.InsideWait, (float) this.OutsideWait, Mathf.InverseLerp(-1f, 1f, this.script.GetAngleHRate(this.eyeLR))));
    ((Rect) ref this.rect).set_y(Mathf.Lerp((float) this.DownWait, (float) this.UpWait, Mathf.InverseLerp(-1f, 1f, this.script.GetAngleVRate())));
    this.offset = new Vector2(Mathf.Clamp(((Rect) ref this.rect).get_x() / (float) this.textureWidth, this.InsideLimit, this.OutsideLimit), Mathf.Clamp(((Rect) ref this.rect).get_y() / (float) this.textureHeight, this.UpLimit, this.DownLimit));
    this.scale = new Vector2(((Rect) ref this.rect).get_width() / (float) this.textureWidth, ((Rect) ref this.rect).get_height() / (float) this.textureHeight);
    this._material.SetTextureOffset("_MainTex", this.offset);
    this._material.SetTextureScale("_MainTex", this.scale);
  }

  public Vector2 GetEyeTexOffset()
  {
    return this.offset;
  }

  public Vector2 GetEyeTexScale()
  {
    return this.scale;
  }
}
