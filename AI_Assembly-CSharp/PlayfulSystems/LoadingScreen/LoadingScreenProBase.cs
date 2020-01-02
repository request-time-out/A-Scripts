// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.LoadingScreen.LoadingScreenProBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayfulSystems.LoadingScreen
{
  public abstract class LoadingScreenProBase : MonoBehaviour
  {
    private static int targetSceneIndex = -1;
    private static string targetSceneName;
    [Tooltip("Central Config asset.")]
    public LoadingScreenConfig config;
    [Tooltip("If loading additively, set reference to this scene's camera's audio listener, to avoid multiple active audio listeners at any one time. The script will try to auto set this for your convenience.")]
    public AudioListener audioListener;
    [Header("Timing Settings")]
    public LoadingScreenProBase.BehaviorAfterLoad behaviorAfterLoad;
    [Tooltip("After finishing Loading, wait this much before showing the completion visuals.")]
    public float timeToAutoContinue;
    private AsyncOperation operation;
    private float previousTimescale;
    private float currentProgress;
    private Scene currentScene;

    protected LoadingScreenProBase()
    {
      base.\u002Ector();
    }

    public static void LoadScene(int levelNum)
    {
      if (!LoadingScreenProBase.IsLegalLevelIndex(levelNum))
      {
        Debug.LogWarning((object) ("No Scene with Buildindex " + (object) levelNum + " found."));
        LoadingScreenProBase.targetSceneName = (string) null;
      }
      else
      {
        LoadingScreenProBase.targetSceneIndex = levelNum;
        LoadingScreenProBase.targetSceneName = (string) null;
        LoadingScreenProBase.LoadLoadingScene();
      }
    }

    private static bool IsLegalLevelIndex(int levelNum)
    {
      return levelNum >= 0 || levelNum < SceneManager.get_sceneCountInBuildSettings();
    }

    public static void LoadScene(string levelName)
    {
      LoadingScreenProBase.targetSceneIndex = -1;
      LoadingScreenProBase.targetSceneName = levelName;
      LoadingScreenProBase.LoadLoadingScene();
    }

    private static void LoadLoadingScene()
    {
      Application.set_backgroundLoadingPriority((ThreadPriority) 4);
      SceneManager.LoadScene(LoadingScreenConfig.loadingSceneName);
    }

    protected virtual void Start()
    {
      if (LoadingScreenProBase.targetSceneName == null && LoadingScreenProBase.targetSceneIndex == -1)
        Debug.LogWarning((object) "[LoadingScreenPro] Directly loaded a scene with Loading Screen Pro without setting target Scene index or name.\nUse static method LoadingScreenPro.LoadScene(int levelNum) from your scripts to use the loading screen.");
      else if (Object.op_Equality((Object) this.config, (Object) null))
      {
        Debug.LogWarning((object) "[LoadingScreenPro] Config is not set. Please open your Loading Scene and add a reference to a config file to the LoadingScreenPro and save the scene.");
      }
      else
      {
        this.previousTimescale = Time.get_timeScale();
        Time.set_timeScale(1f);
        this.currentScene = SceneManager.GetActiveScene();
        this.Init();
        Application.set_backgroundLoadingPriority(this.config.loadThreadPriority);
        this.StartCoroutine(this.LoadAsync(LoadingScreenProBase.targetSceneIndex, LoadingScreenProBase.targetSceneName));
      }
    }

    protected virtual void Init()
    {
    }

    [DebuggerHidden]
    private IEnumerator LoadAsync(int levelNum, string levelName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new LoadingScreenProBase.\u003CLoadAsync\u003Ec__Iterator0()
      {
        levelNum = levelNum,
        levelName = levelName,
        \u0024this = this
      };
    }

    private void SetProgress(float progress)
    {
      if ((double) progress <= (double) this.currentProgress)
        return;
      this.ShowProgressVisuals(progress);
      this.currentProgress = progress;
    }

    private AsyncOperation StartOperation(int levelNum, string levelName)
    {
      LoadSceneMode loadSceneMode = !this.CanLoadAdditively() ? (LoadSceneMode) 0 : (LoadSceneMode) 1;
      return string.IsNullOrEmpty(levelName) ? SceneManager.LoadSceneAsync(levelNum, loadSceneMode) : SceneManager.LoadSceneAsync(levelName, loadSceneMode);
    }

    private bool CanLoadAdditively()
    {
      return this.config.loadAdditively;
    }

    protected bool CanLoadAsynchronously()
    {
      return true;
    }

    private bool IsDoneLoading()
    {
      return this.CanLoadAdditively() ? this.operation.get_isDone() : (double) this.operation.get_progress() >= 0.899999976158142;
    }

    private void ActivateLoadedScene()
    {
      LoadingScreenProBase.targetSceneIndex = -1;
      LoadingScreenProBase.targetSceneName = (string) null;
      if (this.CanLoadAdditively())
        SceneManager.UnloadSceneAsync(this.currentScene);
      this.operation.set_allowSceneActivation(true);
      Resources.UnloadUnusedAssets();
      Time.set_timeScale(this.previousTimescale);
    }

    private void ShowSceneInfos()
    {
      if (!this.config.showSceneInfos || this.config.sceneInfos == null || this.config.sceneInfos.Length == 0)
        this.DisplaySceneInfo((SceneInfo) null);
      else
        this.DisplaySceneInfo(this.config.GetSceneInfo(LoadingScreenProBase.targetSceneName));
    }

    protected virtual void DisplaySceneInfo(SceneInfo info)
    {
    }

    private void ShowTips()
    {
      if (!this.config.showRandomTip || this.config.gameTips == null || this.config.gameTips.Length == 0)
        this.DisplayGameTip((LoadingTip) null);
      else
        this.DisplayGameTip(this.config.GetGameTip());
    }

    protected virtual void DisplayGameTip(LoadingTip tip)
    {
    }

    protected virtual void ShowStartingVisuals()
    {
    }

    protected virtual void ShowProgressVisuals(float progress)
    {
    }

    protected virtual void ShowLoadingDoneVisuals()
    {
    }

    protected virtual void ShowEndingVisuals()
    {
    }

    protected virtual bool CanShowConfirmation()
    {
      return true;
    }

    protected virtual bool CanActivateTargetScene()
    {
      return true;
    }

    public enum BehaviorAfterLoad
    {
      WaitForPlayerInput,
      ContinueAutomatically,
    }
  }
}
