// Decompiled with JetBrains decompiler
// Type: ADV.Program
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ADV
{
  public static class Program
  {
    public const string BASE_FOV = "23";
    public const string AdvScenarioPath = "scenario/";

    public static string ScenarioBundle(string file)
    {
      return "scenario/" + file + ".unity3d";
    }

    public static string FindADVBundleFilePath(string path, string asset = null)
    {
      return Program.FindADVBundleFilePath(path, asset, out ScenarioData _);
    }

    public static string FindADVBundleFilePath(string path, string asset, out ScenarioData data)
    {
      data = (ScenarioData) null;
      string str = (string) null;
      foreach (string assetBundleName in (IEnumerable<string>) CommonLib.GetAssetBundleNameListFromPath(string.Format("adv/scenario/{0}/ ", (object) path), true).OrderByDescending<string, string>((Func<string, string>) (bundle => bundle)))
      {
        if (asset == null)
        {
          str = assetBundleName;
        }
        else
        {
          foreach (ScenarioData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ScenarioData), (string) null).GetAllAssets<ScenarioData>())
          {
            if (((Object) allAsset).get_name() == asset)
            {
              str = assetBundleName;
              data = allAsset;
              break;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        if (str != null)
          break;
      }
      return str;
    }

    public static string FindADVBundleFilePath(int id, int category, string asset = null)
    {
      return Program.FindADVBundleFilePath(id, category, asset, out ScenarioData _);
    }

    public static string FindADVBundleFilePath(
      int id,
      int category,
      string asset,
      out ScenarioData data)
    {
      data = (ScenarioData) null;
      string str1 = (string) null;
      string str2 = string.Format("{0:00}", (object) category);
      foreach (string str3 in (IEnumerable<string>) CommonLib.GetAssetBundleNameListFromPath(string.Format("adv/scenario/c{0:00}/", (object) id), true).OrderByDescending<string, string>((Func<string, string>) (bundle => bundle)))
      {
        if (!(Path.GetFileNameWithoutExtension(str3) != str2))
        {
          if (asset == null)
          {
            str1 = str3;
          }
          else
          {
            foreach (ScenarioData allAsset in AssetBundleManager.LoadAllAsset(str3, typeof (ScenarioData), (string) null).GetAllAssets<ScenarioData>())
            {
              if (((Object) allAsset).get_name() == asset)
              {
                str1 = str3;
                data = allAsset;
                break;
              }
            }
            AssetBundleManager.UnloadAssetBundle(str3, false, (string) null, false);
          }
          if (str1 != null)
            break;
        }
      }
      return str1;
    }

    public static string FindMessageADVBundleFilePath(string path, string asset = null)
    {
      return Program.FindMessageADVBundleFilePath(path, asset, out ScenarioData _);
    }

    public static string FindMessageADVBundleFilePath(
      string path,
      string asset,
      out ScenarioData data)
    {
      data = (ScenarioData) null;
      string str = (string) null;
      foreach (string assetBundleName in (IEnumerable<string>) CommonLib.GetAssetBundleNameListFromPath(string.Format("adv/message/{0}/", (object) path), true).OrderByDescending<string, string>((Func<string, string>) (bundle => bundle)))
      {
        if (asset == null)
        {
          str = assetBundleName;
        }
        else
        {
          foreach (ScenarioData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ScenarioData), (string) null).GetAllAssets<ScenarioData>())
          {
            if (((Object) allAsset).get_name() == asset)
            {
              str = assetBundleName;
              data = allAsset;
              break;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        if (str != null)
          break;
      }
      return str;
    }

    [DebuggerHidden]
    public static IEnumerator Open(
      IData scene,
      IData openData,
      Program.OpenDataProc proc = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Program.\u003COpen\u003Ec__Iterator0()
      {
        scene = scene
      };
    }

    public static bool isADVActionActive
    {
      get
      {
        return true;
      }
    }

    public static bool isADVScene
    {
      get
      {
        return Singleton<Scene>.Instance.NowSceneNames.Contains("ADV");
      }
    }

    public static bool isADVProcessing
    {
      get
      {
        return Program.isADVActionActive || Program.isADVScene;
      }
    }

    [DebuggerHidden]
    public static IEnumerator ADVProcessingCheck()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Program.\u003CADVProcessingCheck\u003Ec__Iterator1 processingCheckCIterator1 = new Program.\u003CADVProcessingCheck\u003Ec__Iterator1();
      return (IEnumerator) processingCheckCIterator1;
    }

    [DebuggerHidden]
    public static IEnumerator Wait(string addSceneName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Program.\u003CWait\u003Ec__Iterator2()
      {
        addSceneName = addSceneName
      };
    }

    [Serializable]
    public class Transfer
    {
      public Transfer(ScenarioData.Param param)
      {
        this.line = -1;
        this.param = param;
      }

      public int line { get; private set; }

      public ScenarioData.Param param { get; private set; }

      public static List<Program.Transfer> NewList(bool multi = true, bool isSceneRegulate = false)
      {
        return new List<Program.Transfer>()
        {
          Program.Transfer.Create((multi ? 1 : 0) != 0, Command.SceneFadeRegulate, isSceneRegulate.ToString())
        };
      }

      public static Program.Transfer Create(
        bool multi,
        Command command,
        params string[] args)
      {
        return new Program.Transfer(new ScenarioData.Param(multi, command, args));
      }

      public static Program.Transfer VAR(params string[] args)
      {
        return Program.Transfer.Create(true, Command.VAR, args);
      }

      public static Program.Transfer Open(params string[] args)
      {
        return Program.Transfer.Create(false, Command.Open, args);
      }

      public static Program.Transfer Close()
      {
        return Program.Transfer.Create(false, Command.Close, (string[]) null);
      }

      public static Program.Transfer Text(params string[] args)
      {
        return Program.Transfer.Create(false, Command.Text, args);
      }

      public static Program.Transfer Voice(params string[] args)
      {
        return Program.Transfer.Create(true, Command.Voice, args);
      }

      public static Program.Transfer Motion(params string[] args)
      {
        return Program.Transfer.Create(true, Command.Motion, args);
      }

      public static Program.Transfer Expression(params string[] args)
      {
        return Program.Transfer.Create(true, Command.Expression, args);
      }

      public static Program.Transfer ExpressionIcon(params string[] args)
      {
        return Program.Transfer.Create(true, Command.ExpressionIcon, args);
      }
    }

    public class OpenDataProc
    {
      public Action onLoad { get; set; }

      public Func<IEnumerator> onFadeIn { get; set; }

      public Func<IEnumerator> onFadeOut { get; set; }

      public Scene.Data.FadeType fadeType { get; set; }
    }
  }
}
