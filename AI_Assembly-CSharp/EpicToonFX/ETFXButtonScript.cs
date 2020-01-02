// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXButtonScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
  public class ETFXButtonScript : MonoBehaviour
  {
    public GameObject Button;
    private Text MyButtonText;
    private string projectileParticleName;
    private ETFXFireProjectile effectScript;
    private ETFXProjectileScript projectileScript;
    public float buttonsX;
    public float buttonsY;
    public float buttonsSizeX;
    public float buttonsSizeY;
    public float buttonsDistance;

    public ETFXButtonScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.effectScript = (ETFXFireProjectile) GameObject.Find("ETFXFireProjectile").GetComponent<ETFXFireProjectile>();
      this.getProjectileNames();
      this.MyButtonText = (Text) ((Component) this.Button.get_transform().Find("Text")).GetComponent<Text>();
      this.MyButtonText.set_text(this.projectileParticleName);
    }

    private void Update()
    {
      this.MyButtonText.set_text(this.projectileParticleName);
    }

    public void getProjectileNames()
    {
      this.projectileScript = (ETFXProjectileScript) this.effectScript.projectiles[this.effectScript.currentProjectile].GetComponent<ETFXProjectileScript>();
      this.projectileParticleName = ((Object) this.projectileScript.projectileParticle).get_name();
    }

    public bool overButton()
    {
      Rect rect1;
      ((Rect) ref rect1).\u002Ector(this.buttonsX, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
      Rect rect2;
      ((Rect) ref rect2).\u002Ector(this.buttonsX + this.buttonsDistance, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
      return ((Rect) ref rect1).Contains(new Vector2((float) Input.get_mousePosition().x, (float) Screen.get_height() - (float) Input.get_mousePosition().y)) || ((Rect) ref rect2).Contains(new Vector2((float) Input.get_mousePosition().x, (float) Screen.get_height() - (float) Input.get_mousePosition().y));
    }
  }
}
