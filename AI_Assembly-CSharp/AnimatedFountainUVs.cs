// Decompiled with JetBrains decompiler
// Type: AnimatedFountainUVs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class AnimatedFountainUVs : MonoBehaviour
{
  public int _uvTieX;
  public int _uvTieY;
  public int _fps;
  private Vector2 _size;
  private Renderer _myRenderer;
  private int _lastIndex;

  public AnimatedFountainUVs()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this._size = new Vector2(1f / (float) this._uvTieX, 1f / (float) this._uvTieY);
    this._myRenderer = (Renderer) ((Component) this).GetComponent<Renderer>();
    if (!Object.op_Equality((Object) this._myRenderer, (Object) null))
      return;
    ((Behaviour) this).set_enabled(false);
  }

  private void Update()
  {
    int num1 = (int) ((double) Time.get_timeSinceLevelLoad() * (double) this._fps) % (this._uvTieX * this._uvTieY);
    if (num1 == this._lastIndex)
      return;
    int num2 = num1 % this._uvTieX;
    int num3 = num1 / this._uvTieY;
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector((float) num2 * (float) this._size.x, (float) (1.0 - this._size.y - (double) num3 * this._size.y));
    this._myRenderer.get_material().SetTextureOffset("_MainTex", vector2);
    this._myRenderer.get_material().SetTextureScale("_MainTex", this._size);
    this._lastIndex = num1;
  }
}
