// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.LoadingScreen.LoadingScreenTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace PlayfulSystems.LoadingScreen
{
  public class LoadingScreenTrigger : MonoBehaviour
  {
    public bool loadOnStart;
    public LoadingScreenTrigger.SceneReference loadSceneFrom;
    public int sceneNumber;
    public string sceneName;

    public LoadingScreenTrigger()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (!this.loadOnStart)
        return;
      this.TriggerLoadScene();
    }

    public void TriggerLoadScene()
    {
      if (this.loadSceneFrom == LoadingScreenTrigger.SceneReference.Number)
        LoadingScreenProBase.LoadScene(this.sceneNumber);
      else
        LoadingScreenProBase.LoadScene(this.sceneName);
    }

    public void LoadScene(int number)
    {
      LoadingScreenProBase.LoadScene(number);
    }

    public void LoadScene(string name)
    {
      LoadingScreenProBase.LoadScene(name);
    }

    public enum SceneReference
    {
      Number,
      Name,
    }
  }
}
