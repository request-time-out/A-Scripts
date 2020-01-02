// Decompiled with JetBrains decompiler
// Type: MiniPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using SpriteToParticlesAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniPanel : MonoBehaviour
{
  public List<SpriteToParticles> PlayableFXs;
  public Button PlayButton;
  public Button PauseButton;
  public Toggle WindButton;
  private int SceneCount;
  public WindZone wind;
  private int currentScene;

  public MiniPanel()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (this.PlayableFXs == null || this.PlayableFXs.Count <= 0)
      this.PlayableFXs = ((IEnumerable<SpriteToParticles>) Object.FindObjectsOfType<SpriteToParticles>()).ToList<SpriteToParticles>();
    if (this.PlayableFXs == null || this.PlayableFXs.Count <= 0)
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
    {
      if (!Object.op_Implicit((Object) this.wind))
        this.wind = (WindZone) Object.FindObjectOfType<WindZone>();
      if (!Object.op_Implicit((Object) this.wind))
        ((Component) this.WindButton).get_gameObject().SetActive(false);
      foreach (SpriteToParticles playableFx in this.PlayableFXs)
      {
        if (Object.op_Implicit((Object) playableFx))
          playableFx.OnAvailableToPlay += new SimpleEvent(this.BecameAvailableToPlay);
      }
      this.RefreshButtons();
    }
  }

  public void ReloadScene()
  {
    Scene activeScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(((Scene) ref activeScene).get_name());
  }

  public void TogglePlay()
  {
    if (this.PlayableFXs.TrueForAll((Predicate<SpriteToParticles>) (x => x.IsPlaying())))
    {
      foreach (SpriteToParticles playableFx in this.PlayableFXs)
        playableFx.Pause();
    }
    else
    {
      foreach (SpriteToParticles playableFx in this.PlayableFXs)
        playableFx.Play();
    }
    this.RefreshButtons();
  }

  public void Stop()
  {
    foreach (SpriteToParticles playableFx in this.PlayableFXs)
      playableFx.Stop();
    this.RefreshButtons();
  }

  public void BecameAvailableToPlay()
  {
    this.RefreshButtons();
  }

  public void RefreshButtons()
  {
    bool flag = this.PlayableFXs.TrueForAll((Predicate<SpriteToParticles>) (x => x.IsPlaying()));
    ((Component) this.PlayButton).get_gameObject().SetActive(!flag);
    ((Component) this.PauseButton).get_gameObject().SetActive(flag);
    ((Selectable) this.PlayButton).set_interactable(this.PlayableFXs.TrueForAll((Predicate<SpriteToParticles>) (x => x.IsAvailableToPlay())));
  }

  public void ToggleWind()
  {
    if (!Object.op_Implicit((Object) this.wind))
      return;
    ((Component) this.wind).get_gameObject().SetActive(!((Component) this.wind).get_gameObject().get_activeInHierarchy());
  }

  public void NextScene()
  {
    Scene activeScene = SceneManager.GetActiveScene();
    this.currentScene = ((Scene) ref activeScene).get_buildIndex();
    this.currentScene = (this.currentScene + 1) % this.SceneCount;
    this.UnloadCurrentScene();
    this.Invoke("LoadNextScene", 0.1f);
  }

  public void PreviousScene()
  {
    Scene activeScene = SceneManager.GetActiveScene();
    this.currentScene = ((Scene) ref activeScene).get_buildIndex();
    this.currentScene = (this.currentScene - 1 + this.SceneCount) % this.SceneCount;
    this.UnloadCurrentScene();
    this.Invoke("LoadNextScene", 0.1f);
  }

  private void UnloadCurrentScene()
  {
    foreach (Component playableFx in this.PlayableFXs)
      Object.DestroyImmediate((Object) playableFx.get_gameObject());
    GC.Collect();
    Resources.UnloadUnusedAssets();
    GC.Collect();
  }

  private void LoadNextScene()
  {
    GC.Collect();
    SceneManager.LoadScene(this.currentScene);
  }
}
