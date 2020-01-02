// Decompiled with JetBrains decompiler
// Type: RawImageUVScrool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class RawImageUVScrool : MonoBehaviour
{
  public RawImage image;
  public float scrollSpeed;
  private Rect uvRect;

  public RawImageUVScrool()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.uvRect = this.image.get_uvRect();
  }

  private void Update()
  {
    this.uvRect = this.image.get_uvRect();
    ref Rect local = ref this.uvRect;
    ((Rect) ref local).set_x(((Rect) ref local).get_x() + Time.get_deltaTime() * this.scrollSpeed);
    this.image.set_uvRect(this.uvRect);
  }
}
