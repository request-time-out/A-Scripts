// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.LoadingScreen.LoadingScreenConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace PlayfulSystems.LoadingScreen
{
  public class LoadingScreenConfig : ScriptableObject
  {
    public static string loadingSceneName = "PS-LoadingScene_5";
    [Header("Loading Behavior")]
    [Tooltip("Loading additively means that the scene is loaded in the background in addition to the loading scene and is then turned off as the loading scene is unloaded.")]
    public bool loadAdditively;
    [Tooltip("Lower priority means a background operation will run less often and will take up less time, but will progress more slowly.")]
    public ThreadPriority loadThreadPriority;
    [Header("Scene Infos")]
    public bool showSceneInfos;
    public SceneInfo[] sceneInfos;
    [Header("Game Tips")]
    public bool showRandomTip;
    public LoadingTip[] gameTips;

    public LoadingScreenConfig()
    {
      base.\u002Ector();
    }

    public virtual SceneInfo GetSceneInfo(string targetSceneName)
    {
      for (int index = 0; index < this.sceneInfos.Length; ++index)
      {
        if (this.sceneInfos[index].sceneName == targetSceneName)
          return this.sceneInfos[index];
      }
      return (SceneInfo) null;
    }

    public virtual LoadingTip GetGameTip()
    {
      return this.gameTips[Random.Range(0, this.gameTips.Length)];
    }
  }
}
