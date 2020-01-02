// Decompiled with JetBrains decompiler
// Type: ETFXSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class ETFXSceneManager : MonoBehaviour
{
  public bool GUIHide;
  public bool GUIHide2;
  public bool GUIHide3;

  public ETFXSceneManager()
  {
    base.\u002Ector();
  }

  public void LoadScene1()
  {
    SceneManager.LoadScene("etfx_explosions");
  }

  public void LoadScene2()
  {
    SceneManager.LoadScene("etfx_explosions2");
  }

  public void LoadScene3()
  {
    SceneManager.LoadScene("etfx_portals");
  }

  public void LoadScene4()
  {
    SceneManager.LoadScene("etfx_magic");
  }

  public void LoadScene5()
  {
    SceneManager.LoadScene("etfx_emojis");
  }

  public void LoadScene6()
  {
    SceneManager.LoadScene("etfx_sparkles");
  }

  public void LoadScene7()
  {
    SceneManager.LoadScene("etfx_fireworks");
  }

  public void LoadScene8()
  {
    SceneManager.LoadScene("etfx_powerups");
  }

  public void LoadScene9()
  {
    SceneManager.LoadScene("etfx_swordcombat");
  }

  public void LoadScene10()
  {
    SceneManager.LoadScene("etfx_maindemo");
  }

  public void LoadScene11()
  {
    SceneManager.LoadScene("etfx_combat");
  }

  public void LoadScene12()
  {
    SceneManager.LoadScene("etfx_2ddemo");
  }

  public void LoadScene13()
  {
    SceneManager.LoadScene("etfx_missiles");
  }

  private void Update()
  {
    if (Input.GetKeyDown((KeyCode) 108))
    {
      this.GUIHide = !this.GUIHide;
      if (this.GUIHide)
        ((Behaviour) GameObject.Find("CanvasSceneSelect").GetComponent<Canvas>()).set_enabled(false);
      else
        ((Behaviour) GameObject.Find("CanvasSceneSelect").GetComponent<Canvas>()).set_enabled(true);
    }
    if (Input.GetKeyDown((KeyCode) 106))
    {
      this.GUIHide2 = !this.GUIHide2;
      if (this.GUIHide2)
        ((Behaviour) GameObject.Find("Canvas").GetComponent<Canvas>()).set_enabled(false);
      else
        ((Behaviour) GameObject.Find("Canvas").GetComponent<Canvas>()).set_enabled(true);
    }
    if (!Input.GetKeyDown((KeyCode) 104))
      return;
    this.GUIHide3 = !this.GUIHide3;
    if (this.GUIHide3)
      ((Behaviour) GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas>()).set_enabled(false);
    else
      ((Behaviour) GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas>()).set_enabled(true);
  }
}
