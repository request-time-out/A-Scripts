// Decompiled with JetBrains decompiler
// Type: LoadSceneButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using PlayfulSystems.LoadingScreen;
using System;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
  public string targetSceneName;

  public LoadSceneButton()
  {
    base.\u002Ector();
  }

  public void SetLoadingScene(string sceneName)
  {
    LoadingScreenConfig.loadingSceneName = sceneName;
    CameraFade cameraFade = (CameraFade) ((Component) this).get_gameObject().AddComponent<CameraFade>();
    cameraFade.Init();
    cameraFade.StartFadeTo(Color.get_black(), 1f, new Action(this.LoadTargetScene));
  }

  private void LoadTargetScene()
  {
    LoadingScreenProBase.LoadScene(this.targetSceneName);
  }
}
