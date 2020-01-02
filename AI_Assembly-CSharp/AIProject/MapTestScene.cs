// Decompiled with JetBrains decompiler
// Type: AIProject.MapTestScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.UI;
using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace AIProject
{
  public class MapTestScene : MonoBehaviour, ISystemCommand
  {
    [SerializeField]
    private Transform _mapRoot;
    [SerializeField]
    private Transform _charaRoot;
    [SerializeField]
    private GameObject _charaPrefab;
    [SerializeField]
    private Transform _playerStartPoint;
    [SerializeField]
    private Transform _warpPoint;
    [SerializeField]
    private string _sceneAssetBundleName;
    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private bool _isTrial;
    private TextMeshProUGUI _fpsText;
    private List<ICommandData> _systemCommands;

    public MapTestScene()
    {
      base.\u002Ector();
    }

    public bool EnabledInput { get; set; }

    public void OnUpdateInput()
    {
      Input instance = Singleton<Input>.Instance;
      foreach (ICommandData systemCommand in this._systemCommands)
        systemCommand.Invoke(instance);
    }

    private void Awake()
    {
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MapTestScene.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public bool IsCursorLock { get; set; }
  }
}
