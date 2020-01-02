// Decompiled with JetBrains decompiler
// Type: UVScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class UVScroll : MonoBehaviour
{
  [SerializeField]
  private float scrollSpeedX;
  [SerializeField]
  private float scrollSpeedY;

  public UVScroll()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    ((Renderer) ((Component) this).GetComponent<Renderer>()).get_sharedMaterial().SetTextureOffset("_MainTex", Vector2.get_zero());
  }

  private void Update()
  {
    float num1 = Mathf.Repeat(Time.get_time() * this.scrollSpeedX, 1f);
    float num2 = Mathf.Repeat(Time.get_time() * this.scrollSpeedY, 1f);
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector(num1, num2);
    ((Renderer) ((Component) this).GetComponent<Renderer>()).get_sharedMaterial().SetTextureOffset("_MainTex", vector2);
  }
}
