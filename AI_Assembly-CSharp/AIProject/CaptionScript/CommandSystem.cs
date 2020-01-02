// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.CommandSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.CaptionScript
{
  public class CommandSystem : MonoBehaviour
  {
    public static readonly string[] ReplaceStrings = new string[2]
    {
      "'@",
      "@"
    };
    public const string TagKey = "Tag";
    public const string NameKey = "name";
    public const string AssetKey = "asset";
    public const string FileKey = "file";
    public const string NumKey = "num";
    public const string AnimationNameKey = "AnimeName";
    public const string CrossFadeKey = "Cross Fade";
    public const string ConditionsKey = "conditions";
    public const string TrueKey = "true";
    public const string FalseKey = "false";
    public readonly Dictionary<string, IScriptCommand> CommandTable;
    public readonly Dictionary<string, IScriptCommand> MerchantCommandTable;
    private List<Dictionary<string, string>> _scenarioTableList;
    private int _currentLine;

    public CommandSystem()
    {
      base.\u002Ector();
    }

    public Dictionary<string, IScriptCommand> ActorCommandTable { get; set; }

    public bool CompletedCommand { get; set; }

    public bool CompletedScenario
    {
      get
      {
        return this._currentLine >= this._scenarioTableList.Count && this.CompletedCommand;
      }
    }

    public RawImage RawImage { get; set; }

    public ActorCameraControl CameraSystem { get; set; }

    public GameObject ObjBack { get; private set; }

    public Transform TransformDepth { get; private set; }

    private void Awake()
    {
      this.CompletedCommand = false;
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, bool>) (_ => !this.CompletedScenario)), (Func<M0, bool>) (_ => this.CompletedCommand)), (Action<M0>) (_ => this.ExecuteNextLine()));
    }

    public bool LoadScenario(string assetbundle, string file)
    {
      this._scenarioTableList.Clear();
      this._currentLine = -1;
      ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundle, file, string.Empty);
      if (Object.op_Equality((Object) excelData, (Object) null))
      {
        Debug.LogError((object) string.Format("読み込みエラー: 要チェック{{[{0}][{1}]}}", (object) assetbundle, (object) file));
        return false;
      }
      int maxCell = excelData.MaxCell;
      for (int index = 0; index < maxCell; ++index)
      {
        ExcelData.Param obj = excelData.list[index];
        if (obj.list.Count != 0)
        {
          string str = obj.list[0];
          if (str.Length != 0 && MathfEx.RangeEqualOn<int>(0, str.IndexOf("@"), 1))
          {
            string key = obj.list[0].Replace("'@", string.Empty).Replace("@", string.Empty);
            IScriptCommand scriptCommand;
            if (!this.ActorCommandTable.TryGetValue(key, out scriptCommand))
              Debug.LogWarning((object) string.Format("未実装のコマンド: {0}", (object) key));
            else if (scriptCommand.IsBefore)
              scriptCommand.Execute(scriptCommand.Analysis(obj.list), -1);
            else
              this._scenarioTableList.Add(scriptCommand.Analysis(obj.list));
          }
        }
      }
      this._currentLine = -1;
      return this._scenarioTableList.Count != 0;
    }

    public void GotoTag(string tag)
    {
      string str1;
      string str2;
      int index = this._scenarioTableList.FindIndex((Predicate<Dictionary<string, string>>) (d => d.TryGetValue("Tag", out str1) && d.TryGetValue("name", out str2) && str1 == nameof (tag) && str2 == tag));
      if (index >= 0)
      {
        this._currentLine = index;
      }
      else
      {
        Debug.LogError((object) string.Format("タグ指定エラー: tag=[{0}]", (object) tag));
        this._scenarioTableList.Clear();
      }
      this.CompletedCommand = true;
    }

    public void ExecuteNextLine()
    {
      bool flag = true;
      while (flag)
      {
        ++this._currentLine;
        if (this._currentLine < this._scenarioTableList.Count)
        {
          string key;
          IScriptCommand scriptCommand;
          if (this._scenarioTableList[this._currentLine].TryGetValue("Tag", out key) && this.ActorCommandTable.TryGetValue(key, out scriptCommand))
            flag = scriptCommand.Execute(this._scenarioTableList[this._currentLine], this._currentLine);
          if (!flag)
            break;
        }
        else
          break;
      }
      if (this._scenarioTableList.Count == 0)
        return;
      this.CompletedCommand = false;
    }

    public void Clear()
    {
      this._scenarioTableList.Clear();
      this._currentLine = -1;
      this.CompletedCommand = true;
    }

    public void WaitForFading()
    {
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.WaitWhileFading()), false), ((Component) this).get_gameObject()), (Action<M0>) (_ => {}));
    }

    [DebuggerHidden]
    private IEnumerator WaitWhileFading()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommandSystem.\u003CWaitWhileFading\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void WaitSeconds(float duration, bool acceptInput = false)
    {
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.WaitSecondsCoroutine(duration, acceptInput)), false), ((Component) this).get_gameObject()), (Action<M0>) (_ => this.CompletedCommand = true));
    }

    [DebuggerHidden]
    private IEnumerator WaitSecondsCoroutine(float duration, bool acceptInput)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommandSystem.\u003CWaitSecondsCoroutine\u003Ec__Iterator1()
      {
        duration = duration,
        acceptInput = acceptInput
      };
    }
  }
}
