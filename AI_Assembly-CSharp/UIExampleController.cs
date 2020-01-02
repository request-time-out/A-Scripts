// Decompiled with JetBrains decompiler
// Type: UIExampleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExampleController : MonoBehaviour
{
  public Toggle maskToggle;
  public Image maskBorder;
  public List<Image> maskImages;
  public Sprite[] windowSprites;
  public Image window;
  public Image health;
  public Image mana;
  public Slider healthSlider;
  public Slider manaSlider;

  public UIExampleController()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.maskImages = new List<Image>();
    foreach (Component component in (Mask[]) Object.FindObjectsOfType<Mask>())
      this.maskImages.Add((Image) component.GetComponent<Image>());
  }

  private void Update()
  {
  }

  public void ToggleMask()
  {
    using (List<Image>.Enumerator enumerator = this.maskImages.GetEnumerator())
    {
      while (enumerator.MoveNext())
        ((Behaviour) enumerator.Current).set_enabled(this.maskToggle.get_isOn());
    }
    ((Behaviour) this.maskBorder).set_enabled(this.maskToggle.get_isOn());
  }

  public void ChangeWindowType(int i)
  {
    this.window.set_sprite(this.windowSprites[i]);
  }

  public void OnSlidersChanged()
  {
    ((Graphic) this.mana).get_rectTransform().set_sizeDelta(new Vector2((float) ((Graphic) this.mana).get_rectTransform().get_sizeDelta().x, this.manaSlider.get_value() * 240f));
    ((Graphic) this.health).get_rectTransform().set_sizeDelta(new Vector2((float) ((Graphic) this.health).get_rectTransform().get_sizeDelta().x, this.healthSlider.get_value() * 240f));
  }
}
