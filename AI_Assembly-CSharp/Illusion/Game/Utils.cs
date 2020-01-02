// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Utils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Illusion.Game
{
  public static class Utils
  {
    public static class IKLoader
    {
      public static void Execute(FullBodyBipedIK ik, List<List<string>> dataList)
      {
        Transform[] componentsInChildren = (Transform[]) ((Component) ik).GetComponentsInChildren<Transform>(true);
        int num1 = 0;
        List<List<string>> stringListList = dataList;
        int index1 = num1;
        int count1 = index1 + 1;
        List<string> stringList1 = stringListList[index1];
        int num2 = 0;
        // ISSUE: variable of the null type
        __Null solver1 = ik.solver;
        List<string> stringList2 = stringList1;
        int index2 = num2;
        int num3 = index2 + 1;
        double num4 = (double) float.Parse(stringList2[index2]);
        ((IKSolver) solver1).IKPositionWeight = (__Null) num4;
        // ISSUE: variable of the null type
        __Null solver2 = ik.solver;
        List<string> stringList3 = stringList1;
        int index3 = num3;
        int num5 = index3 + 1;
        int num6 = int.Parse(stringList3[index3]);
        ((IKSolverFullBody) solver2).iterations = (__Null) num6;
        int num7 = 0;
        foreach (List<string> stringList4 in dataList.Skip<List<string>>(count1))
        {
          if (((IKSolverFullBody) ik.solver).effectors.Length > num7)
          {
            ++num7;
            int num8 = 0;
            // ISSUE: variable of the null type
            __Null solver3 = ik.solver;
            List<string> stringList5 = stringList4;
            int index4 = num8;
            int num9 = index4 + 1;
            FullBodyBipedEffector bodyBipedEffector = Illusion.Utils.Enum<FullBodyBipedEffector>.Cast(stringList5[index4]);
            IKEffector eff = ((IKSolverFullBodyBiped) solver3).GetEffector(bodyBipedEffector);
            if (eff != null)
            {
              IKEffector ikEffector1 = eff;
              List<string> stringList6 = stringList4;
              int index5 = num9;
              int num10 = index5 + 1;
              double num11 = (double) float.Parse(stringList6[index5]);
              ikEffector1.positionWeight = (__Null) num11;
              IKEffector ikEffector2 = eff;
              List<string> stringList7 = stringList4;
              int index6 = num10;
              int num12 = index6 + 1;
              double num13 = (double) float.Parse(stringList7[index6]);
              ikEffector2.rotationWeight = (__Null) num13;
              List<string> stringList8 = stringList4;
              int index7 = num12;
              int num14 = index7 + 1;
              string findFrame = stringList8[index7];
              if (findFrame == "null")
                eff.target = null;
              else
                ((IEnumerable<Transform>) componentsInChildren).FirstOrDefault<Transform>((Func<Transform, bool>) (p => ((Object) p).get_name() == findFrame)).SafeProc<Transform>((Action<Transform>) (frame => eff.target = (__Null) frame));
              if (Object.op_Inequality((Object) eff.target, (Object) null))
              {
                // ISSUE: variable of the null type
                __Null target1 = eff.target;
                List<string> stringList9 = stringList4;
                int index8 = num14;
                int num15 = index8 + 1;
                Vector3 vector3_1 = stringList9[index8].GetVector3();
                ((Transform) target1).set_localPosition(vector3_1);
                // ISSUE: variable of the null type
                __Null target2 = eff.target;
                List<string> stringList10 = stringList4;
                int index9 = num15;
                int num16 = index9 + 1;
                Vector3 vector3_2 = stringList10[index9].GetVector3();
                ((Transform) target2).set_localEulerAngles(vector3_2);
              }
            }
          }
          else
            break;
        }
        int count2 = count1 + num7;
        int num17 = 0;
        foreach (List<string> stringList4 in dataList.Skip<List<string>>(count2))
        {
          if (((IKSolverFullBody) ik.solver).chain.Length <= num17)
            break;
          FBIKChain fbikChain1 = (FBIKChain) ((IKSolverFullBody) ik.solver).chain[num17++];
          int num8 = 0;
          FBIKChain fbikChain2 = fbikChain1;
          List<string> stringList5 = stringList4;
          int index4 = num8;
          int num9 = index4 + 1;
          double num10 = (double) float.Parse(stringList5[index4]);
          fbikChain2.pull = (__Null) num10;
          FBIKChain fbikChain3 = fbikChain1;
          List<string> stringList6 = stringList4;
          int index5 = num9;
          int num11 = index5 + 1;
          double num12 = (double) float.Parse(stringList6[index5]);
          fbikChain3.reach = (__Null) num12;
          FBIKChain fbikChain4 = fbikChain1;
          List<string> stringList7 = stringList4;
          int index6 = num11;
          int num13 = index6 + 1;
          double num14 = (double) float.Parse(stringList7[index6]);
          fbikChain4.push = (__Null) num14;
          FBIKChain fbikChain5 = fbikChain1;
          List<string> stringList8 = stringList4;
          int index7 = num13;
          int num15 = index7 + 1;
          double num16 = (double) float.Parse(stringList8[index7]);
          fbikChain5.pushParent = (__Null) num16;
          FBIKChain fbikChain6 = fbikChain1;
          List<string> stringList9 = stringList4;
          int index8 = num15;
          int num18 = index8 + 1;
          FBIKChain.Smoothing smoothing1 = Illusion.Utils.Enum<FBIKChain.Smoothing>.Cast(stringList9[index8]);
          fbikChain6.reachSmoothing = (__Null) smoothing1;
          FBIKChain fbikChain7 = fbikChain1;
          List<string> stringList10 = stringList4;
          int index9 = num18;
          int num19 = index9 + 1;
          FBIKChain.Smoothing smoothing2 = Illusion.Utils.Enum<FBIKChain.Smoothing>.Cast(stringList10[index9]);
          fbikChain7.pushSmoothing = (__Null) smoothing2;
          // ISSUE: variable of the null type
          __Null bendConstraint = fbikChain1.bendConstraint;
          List<string> stringList11 = stringList4;
          int index10 = num19;
          int num20 = index10 + 1;
          double num21 = (double) float.Parse(stringList11[index10]);
          ((IKConstraintBend) bendConstraint).weight = (__Null) num21;
          List<string> stringList12 = stringList4;
          int index11 = num20;
          int num22 = index11 + 1;
          string findFrame = stringList12[index11];
          if (findFrame == "null")
            ((IKConstraintBend) fbikChain1.bendConstraint).bendGoal = null;
          else
            ((IKConstraintBend) fbikChain1.bendConstraint).bendGoal = (__Null) ((IEnumerable<Transform>) componentsInChildren).FirstOrDefault<Transform>((Func<Transform, bool>) (p => ((Object) p).get_name() == findFrame));
          if (Object.op_Inequality((Object) ((IKConstraintBend) fbikChain1.bendConstraint).bendGoal, (Object) null))
          {
            // ISSUE: variable of the null type
            __Null bendGoal1 = ((IKConstraintBend) fbikChain1.bendConstraint).bendGoal;
            List<string> stringList13 = stringList4;
            int index12 = num22;
            int num23 = index12 + 1;
            Vector3 vector3_1 = stringList13[index12].GetVector3();
            ((Transform) bendGoal1).set_localPosition(vector3_1);
            // ISSUE: variable of the null type
            __Null bendGoal2 = ((IKConstraintBend) fbikChain1.bendConstraint).bendGoal;
            List<string> stringList14 = stringList4;
            int index13 = num23;
            int num24 = index13 + 1;
            Vector3 vector3_2 = stringList14[index13].GetVector3();
            ((Transform) bendGoal2).set_localEulerAngles(vector3_2);
          }
        }
      }
    }

    public static class UniRx
    {
      public static void FixPerspectiveObject<T>(T o, Camera camera) where T : Component
      {
        Transform transform = o.get_transform();
        Func<float> distance = (Func<float>) (() =>
        {
          Vector3 vector3 = Vector3.op_Subtraction(transform.get_position(), ((Component) camera).get_transform().get_position());
          return ((Vector3) ref vector3).get_magnitude();
        });
        Vector3 baseScale = Vector3.op_Division(transform.get_localScale(), distance());
        ObservableExtensions.Subscribe<Vector3>((IObservable<M0>) Observable.Select<Unit, Vector3>(Observable.TakeWhile<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) (object) o), (Func<M0, bool>) (_ => Object.op_Inequality((Object) camera, (Object) null))), (Func<M0, M1>) (_ => Vector3.op_Multiply(baseScale, distance()))), (Action<M0>) (scale => transform.set_localScale(scale)));
      }

      public static class FPSCounter
      {
        private const int BufferSize = 5;

        static FPSCounter()
        {
          Utils.UniRx.FPSCounter.Current = (IReadOnlyReactiveProperty<float>) ReactivePropertyExtensions.ToReadOnlyReactiveProperty<float>((IObservable<M0>) Observable.Select<IList<float>, float>((IObservable<M0>) Observable.Buffer<float>((IObservable<M0>) Observable.Select<long, float>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, M1>) (_ => Time.get_deltaTime())), 5, 1), (Func<M0, M1>) (x => 1f / x.Average())));
        }

        public static IReadOnlyReactiveProperty<float> Current { get; private set; }
      }
    }

    public static class Bundle
    {
      public static void LoadSprite(
        string assetBundleName,
        string assetName,
        Image image,
        bool isTexSize,
        string spAnimeName = null,
        string manifest = null,
        string spAnimeManifest = null)
      {
        AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (Sprite), manifest);
        Sprite asset1 = loadAssetOperation.GetAsset<Sprite>();
        if (Object.op_Equality((Object) asset1, (Object) null))
        {
          Texture2D asset2 = loadAssetOperation.GetAsset<Texture2D>();
          asset1 = Sprite.Create(asset2, new Rect(0.0f, 0.0f, (float) ((Texture) asset2).get_width(), (float) ((Texture) asset2).get_height()), Vector2.get_zero());
        }
        image.set_sprite(asset1);
        RectTransform rectTransform = ((Graphic) image).get_rectTransform();
        Vector2 vector2_1;
        if (isTexSize)
        {
          Rect rect1 = asset1.get_rect();
          double width = (double) ((Rect) ref rect1).get_width();
          Rect rect2 = asset1.get_rect();
          double height = (double) ((Rect) ref rect2).get_height();
          vector2_1 = new Vector2((float) width, (float) height);
        }
        else
          vector2_1 = rectTransform.get_sizeDelta();
        Vector2 vector2_2 = vector2_1;
        if (!spAnimeName.IsNullOrEmpty())
        {
          Animator component = (Animator) ((Component) image).GetComponent<Animator>();
          ((Behaviour) component).set_enabled(true);
          component.set_runtimeAnimatorController(AssetBundleManager.LoadAsset(assetBundleName, spAnimeName, typeof (RuntimeAnimatorController), (string) null).GetAsset<RuntimeAnimatorController>());
          Func<float, float, float> func1 = (Func<float, float, float>) ((x, y) => x / y);
          Func<float, float, bool> func2 = (Func<float, float, bool>) ((a, b) => (double) a > (double) b && Mathf.FloorToInt(a - 1f) > 0);
          float num1 = func1((float) vector2_2.x, (float) vector2_2.y);
          float num2 = func1((float) vector2_2.y, (float) vector2_2.x);
          if (func2(num1, num2))
            rectTransform.set_sizeDelta(new Vector2((float) vector2_2.y, (float) vector2_2.y));
          else if (func2(num2, num1))
            rectTransform.set_sizeDelta(new Vector2((float) vector2_2.x, (float) vector2_2.x));
          else
            rectTransform.set_sizeDelta(new Vector2((float) vector2_2.x, (float) vector2_2.y));
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, spAnimeManifest, false);
        }
        else
          rectTransform.set_sizeDelta(new Vector2((float) vector2_2.x, (float) vector2_2.y));
        AssetBundleManager.UnloadAssetBundle(assetBundleName, false, manifest, false);
      }
    }

    public static class Layout
    {
      public class EnabledScope : GUI.Scope
      {
        private readonly bool enabled;

        public EnabledScope()
        {
          base.\u002Ector();
          this.enabled = GUI.get_enabled();
        }

        public EnabledScope(bool enabled)
        {
          base.\u002Ector();
          this.enabled = GUI.get_enabled();
          GUI.set_enabled(enabled);
        }

        protected virtual void CloseScope()
        {
          GUI.set_enabled(this.enabled);
        }
      }

      public class ColorScope : GUI.Scope
      {
        private readonly Color[] colors;

        public ColorScope()
        {
          base.\u002Ector();
          this.colors = new Color[3]
          {
            GUI.get_color(),
            GUI.get_backgroundColor(),
            GUI.get_contentColor()
          };
        }

        public ColorScope(params Color[] colors)
        {
          base.\u002Ector();
          this.colors = new Color[3]
          {
            GUI.get_color(),
            GUI.get_backgroundColor(),
            GUI.get_contentColor()
          };
          // ISSUE: object of a compiler-generated type is created
          using (IEnumerator<\u003C\u003E__AnonType4<Color, int>> enumerator = ((IEnumerable<Color>) colors).Take<Color>(this.colors.Length).Select<Color, \u003C\u003E__AnonType4<Color, int>>((Func<Color, int, \u003C\u003E__AnonType4<Color, int>>) ((color, index) => new \u003C\u003E__AnonType4<Color, int>(color, index))).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              // ISSUE: variable of a compiler-generated type
              \u003C\u003E__AnonType4<Color, int> current = enumerator.Current;
              switch (current.index)
              {
                case 0:
                  GUI.set_color(current.color);
                  continue;
                case 1:
                  GUI.set_backgroundColor(current.color);
                  continue;
                case 2:
                  GUI.set_contentColor(current.color);
                  continue;
                default:
                  continue;
              }
            }
          }
        }

        public ColorScope(Colors colors)
        {
          base.\u002Ector();
          this.colors = new Color[3]
          {
            GUI.get_color(),
            GUI.get_backgroundColor(),
            GUI.get_contentColor()
          };
          if (colors.color.HasValue)
            GUI.set_color(colors.color.Value);
          if (colors.backgroundColor.HasValue)
            GUI.set_backgroundColor(colors.backgroundColor.Value);
          if (!colors.contentColor.HasValue)
            return;
          GUI.set_contentColor(colors.contentColor.Value);
        }

        protected virtual void CloseScope()
        {
          int num1 = 0;
          Color[] colors1 = this.colors;
          int index1 = num1;
          int num2 = index1 + 1;
          GUI.set_color(colors1[index1]);
          Color[] colors2 = this.colors;
          int index2 = num2;
          int num3 = index2 + 1;
          GUI.set_backgroundColor(colors2[index2]);
          Color[] colors3 = this.colors;
          int index3 = num3;
          int num4 = index3 + 1;
          GUI.set_contentColor(colors3[index3]);
        }
      }
    }

    public static class ScreenShot
    {
      public static string Path
      {
        get
        {
          StringBuilder stringBuilder = new StringBuilder(256);
          stringBuilder.Append(UserData.Create("cap"));
          DateTime now = DateTime.Now;
          stringBuilder.Append(now.Year.ToString("0000"));
          stringBuilder.Append(now.Month.ToString("00"));
          stringBuilder.Append(now.Day.ToString("00"));
          stringBuilder.Append(now.Hour.ToString("00"));
          stringBuilder.Append(now.Minute.ToString("00"));
          stringBuilder.Append(now.Second.ToString("00"));
          stringBuilder.Append(now.Millisecond.ToString("000"));
          stringBuilder.Append(".png");
          return stringBuilder.ToString();
        }
      }

      [DebuggerHidden]
      public static IEnumerator CaptureGSS(
        List<ScreenShotCamera> ssCamList,
        string path,
        Texture capMark,
        int capRate = 1)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Utils.ScreenShot.\u003CCaptureGSS\u003Ec__Iterator0()
        {
          ssCamList = ssCamList,
          path = path,
          capMark = capMark,
          capRate = capRate
        };
      }

      [DebuggerHidden]
      public static IEnumerator CaptureCameras(
        List<Camera> cameraList,
        string path,
        Texture capMark,
        int capRate = 1)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Utils.ScreenShot.\u003CCaptureCameras\u003Ec__Iterator1()
        {
          cameraList = cameraList,
          capMark = capMark,
          path = path,
          capRate = capRate
        };
      }

      public static void Capture(Action<RenderTexture> proc, string path, int capRate = 1)
      {
        int num = capRate != 0 ? capRate : 1;
        Texture2D texture2D = new Texture2D(Screen.get_width() * num, Screen.get_height() * num, (TextureFormat) 3, false);
        RenderTexture temporary = RenderTexture.GetTemporary(((Texture) texture2D).get_width(), ((Texture) texture2D).get_height(), 24, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, QualitySettings.get_antiAliasing() != 0 ? QualitySettings.get_antiAliasing() : 1);
        proc(temporary);
        RenderTexture.set_active(temporary);
        texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).get_width(), (float) ((Texture) texture2D).get_height()), 0, 0);
        texture2D.Apply();
        RenderTexture.set_active((RenderTexture) null);
        byte[] png = ImageConversion.EncodeToPNG(texture2D);
        RenderTexture.ReleaseTemporary(temporary);
        Object.Destroy((Object) texture2D);
        System.IO.File.WriteAllBytes(path, png);
      }

      public static void DrawCapMark(Texture tex, Vector2? pos)
      {
        float num = (float) Screen.get_width() / 1280f;
        if (!pos.HasValue)
          pos = new Vector2?(new Vector2(1152f, 688f));
        Graphics.DrawTexture(new Rect((float) pos.Value.x * num, (float) pos.Value.y * num, (float) tex.get_width() * num, (float) tex.get_height() * num), tex);
      }
    }

    public static class Sound
    {
      public static readonly Dictionary<SystemSE, string> SystemSECast = new Dictionary<SystemSE, string>((IEqualityComparer<SystemSE>) new Utils.Sound.SystemSEComparer())
      {
        {
          SystemSE.sel,
          "sse_00_01"
        },
        {
          SystemSE.ok_s,
          "sse_00_02"
        },
        {
          SystemSE.ok_l,
          "sse_00_03"
        },
        {
          SystemSE.cancel,
          "sse_00_04"
        },
        {
          SystemSE.photo,
          "sse_00_05"
        },
        {
          SystemSE.title,
          "se_06_title"
        },
        {
          SystemSE.ok_s2,
          "se_07_button_A"
        },
        {
          SystemSE.window_o,
          "se_08_window_B"
        },
        {
          SystemSE.save,
          "se_09_save_A"
        },
        {
          SystemSE.result_single,
          "result_00"
        },
        {
          SystemSE.result_gauge,
          "result_01"
        },
        {
          SystemSE.result_end,
          "result_02"
        }
      };
      public static readonly Dictionary<Manager.Sound.Type, string> SoundBasePath = new Dictionary<Manager.Sound.Type, string>((IEqualityComparer<Manager.Sound.Type>) new Utils.Sound.SoundTypeComparer())
      {
        {
          Manager.Sound.Type.BGM,
          "sound/data/bgm/00.unity3d"
        },
        {
          Manager.Sound.Type.ENV,
          "sound/data/env/00.unity3d"
        },
        {
          Manager.Sound.Type.GameSE2D,
          "sound/data/se/00.unity3d"
        },
        {
          Manager.Sound.Type.GameSE3D,
          "sound/data/se/00.unity3d"
        },
        {
          Manager.Sound.Type.SystemSE,
          "sound/data/systemse/00.unity3d"
        }
      };

      public static LoadSound GetBGM()
      {
        return Singleton<Manager.Sound>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Sound>.Instance.currentBGM, (Object) null) ? (LoadSound) Singleton<Manager.Sound>.Instance.currentBGM.GetComponentInChildren<LoadSound>() : (LoadSound) null;
      }

      public static AudioSource Get(Manager.Sound.Type type, AssetBundleData data)
      {
        return !Singleton<Manager.Sound>.IsInstance() ? (AudioSource) null : Singleton<Manager.Sound>.Instance.CreateCache(type, data);
      }

      public static AudioSource Get(Manager.Sound.Type type, AssetBundleManifestData data)
      {
        return !Singleton<Manager.Sound>.IsInstance() ? (AudioSource) null : Singleton<Manager.Sound>.Instance.CreateCache(type, data);
      }

      public static AudioSource Get(
        Manager.Sound.Type type,
        string bundle,
        string asset,
        string manifest = null)
      {
        return !Singleton<Manager.Sound>.IsInstance() ? (AudioSource) null : Singleton<Manager.Sound>.Instance.CreateCache(type, bundle, asset, manifest);
      }

      public static void Remove(Manager.Sound.Type type, string bundle, string asset, string manifest = null)
      {
        if (!Singleton<Manager.Sound>.IsInstance())
          return;
        Singleton<Manager.Sound>.Instance.ReleaseCache(type, bundle, asset, manifest);
      }

      public static AudioSource Get(SystemSE se)
      {
        return Utils.Sound.Get(Manager.Sound.Type.SystemSE, Utils.Sound.SoundBasePath[Manager.Sound.Type.SystemSE], Utils.Sound.SystemSECast[se], (string) null);
      }

      public static void Remove(SystemSE se)
      {
        Utils.Sound.Remove(Manager.Sound.Type.SystemSE, Utils.Sound.SoundBasePath[Manager.Sound.Type.SystemSE], Utils.Sound.SystemSECast[se], (string) null);
      }

      public static void Play(SystemSE se)
      {
        AudioSource audioSource = Utils.Sound.Get(se);
        if (Object.op_Equality((Object) audioSource, (Object) null))
          return;
        audioSource.Play();
      }

      public static AudioSource Play(Manager.Sound.Type type, AudioClip clip, float fadeTime = 0.0f)
      {
        AudioSource audio = Singleton<Manager.Sound>.Instance.Play(type, clip, fadeTime);
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) audio), (Func<M0, bool>) (__ => !audio.get_isPlaying())), 1), (Action<M0>) (__ => Object.Destroy((Object) ((Component) audio).get_gameObject())));
        return audio;
      }

      [DebuggerHidden]
      public static IEnumerator GetBGMandVolume(Action<string, float> bgmAndVolume)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Utils.Sound.\u003CGetBGMandVolume\u003Ec__Iterator0()
        {
          bgmAndVolume = bgmAndVolume
        };
      }

      [DebuggerHidden]
      public static IEnumerator GetFadePlayerWhileNull(string bgm, float volume)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Utils.Sound.\u003CGetFadePlayerWhileNull\u003Ec__Iterator1()
        {
          bgm = bgm,
          volume = volume
        };
      }

      public static bool isPlay(SystemSE se)
      {
        return Singleton<Manager.Sound>.IsInstance() && Singleton<Manager.Sound>.Instance.IsPlay(Manager.Sound.Type.SystemSE, Utils.Sound.SystemSECast[se]);
      }

      public static Transform Play(Utils.Sound.Setting s)
      {
        return !Singleton<Manager.Sound>.IsInstance() ? (Transform) null : Singleton<Manager.Sound>.Instance.Play(s.type, s.assetBundleName, s.assetName, s.delayTime, s.fadeTime, s.isAssetEqualPlay, s.isAsync, s.settingNo, s.isBundleUnload);
      }

      public class SettingBGM : Utils.Sound.Setting
      {
        private string _assetBundleName;
        private string _assetName;

        public SettingBGM()
        {
          this.Initialize();
        }

        public SettingBGM(int bgmNo)
        {
          this.Setting(this.Convert(bgmNo));
        }

        public SettingBGM(BGM bgm)
        {
          this.Setting(this.Convert((int) bgm));
        }

        public SettingBGM(string assetBundleName)
        {
          this.Setting(assetBundleName);
        }

        public override string assetBundleName
        {
          get
          {
            return this._assetBundleName;
          }
          set
          {
            this._assetBundleName = value;
            this._assetName = Path.GetFileNameWithoutExtension(value);
          }
        }

        public override string assetName
        {
          get
          {
            return this._assetName;
          }
          set
          {
            this._assetName = value;
          }
        }

        private void Setting(string assetBundleName)
        {
          this.assetBundleName = assetBundleName;
          this.Initialize();
        }

        private string Convert(int bgmNo)
        {
          return string.Format("sound/data/bgm/bgm_{0:00}{1}", (object) bgmNo, (object) ".unity3d");
        }

        private void Initialize()
        {
          this.type = Manager.Sound.Type.BGM;
          this.fadeTime = 0.8f;
          this.isAssetEqualPlay = false;
          this.isBundleUnload = true;
        }
      }

      public class Setting
      {
        public bool isAssetEqualPlay = true;
        public bool isAsync = true;
        public int settingNo = -1;
        public Manager.Sound.Type type;
        public float delayTime;
        public float fadeTime;
        public bool isBundleUnload;

        public Setting()
        {
        }

        public Setting(SystemSE se)
        {
          this.Cast(Manager.Sound.Type.SystemSE);
          this.assetName = Utils.Sound.SystemSECast[se];
        }

        public Setting(Manager.Sound.Type type)
        {
          this.Cast(type);
        }

        public virtual string assetBundleName { get; set; }

        public virtual string assetName { get; set; }

        public void Cast(Manager.Sound.Type type)
        {
          this.type = type;
          this.assetBundleName = Utils.Sound.SoundBasePath[this.type];
        }
      }

      private class SystemSEComparer : IEqualityComparer<SystemSE>
      {
        public bool Equals(SystemSE x, SystemSE y)
        {
          return x == y;
        }

        public int GetHashCode(SystemSE obj)
        {
          return (int) obj;
        }
      }

      private class SoundTypeComparer : IEqualityComparer<Manager.Sound.Type>
      {
        public bool Equals(Manager.Sound.Type x, Manager.Sound.Type y)
        {
          return x == y;
        }

        public int GetHashCode(Manager.Sound.Type obj)
        {
          return (int) obj;
        }
      }
    }

    public static class Voice
    {
      public static AudioSource Get(int voiceNo, AssetBundleData data)
      {
        return !Singleton<Manager.Voice>.IsInstance() ? (AudioSource) null : Singleton<Manager.Voice>.Instance.CreateCache(voiceNo, data);
      }

      public static AudioSource Get(int voiceNo, AssetBundleManifestData data)
      {
        return !Singleton<Manager.Voice>.IsInstance() ? (AudioSource) null : Singleton<Manager.Voice>.Instance.CreateCache(voiceNo, data);
      }

      public static AudioSource Get(
        int voiceNo,
        string bundle,
        string asset,
        string manifest = null)
      {
        return !Singleton<Manager.Voice>.IsInstance() ? (AudioSource) null : Singleton<Manager.Voice>.Instance.CreateCache(voiceNo, bundle, asset, manifest);
      }

      public static void Remove(int voiceNo, string bundle, string asset, string manifest = null)
      {
        if (!Singleton<Manager.Voice>.IsInstance())
          return;
        Singleton<Manager.Voice>.Instance.ReleaseCache(voiceNo, bundle, asset, manifest);
      }

      public static Transform Play(Utils.Voice.Setting s)
      {
        return !Singleton<Manager.Voice>.IsInstance() ? (Transform) null : Singleton<Manager.Voice>.Instance.Play(s.no, s.assetBundleName, s.assetName, s.pitch, s.delayTime, s.fadeTime, s.isAsync, s.voiceTrans, s.type, s.settingNo, s.isPlayEndDelete, s.isBundleUnload, s.is2D);
      }

      public static Transform OnecePlay(Utils.Voice.Setting s)
      {
        return !Singleton<Manager.Voice>.IsInstance() ? (Transform) null : Singleton<Manager.Voice>.Instance.OnecePlay(s.no, s.assetBundleName, s.assetName, s.pitch, s.delayTime, s.fadeTime, s.isAsync, s.voiceTrans, s.type, s.settingNo, s.isPlayEndDelete, s.isBundleUnload, s.is2D);
      }

      public static Transform OnecePlayChara(Utils.Voice.Setting s)
      {
        return !Singleton<Manager.Voice>.IsInstance() ? (Transform) null : Singleton<Manager.Voice>.Instance.OnecePlayChara(s.no, s.assetBundleName, s.assetName, s.pitch, s.delayTime, s.fadeTime, s.isAsync, s.voiceTrans, s.type, s.settingNo, s.isPlayEndDelete, s.isBundleUnload, s.is2D);
      }

      public class Setting
      {
        public float pitch = 1f;
        public bool isAsync = true;
        public int settingNo = -1;
        public bool isPlayEndDelete = true;
        public string assetBundleName;
        public string assetName;
        public Manager.Voice.Type type;
        public int no;
        public Transform voiceTrans;
        public float delayTime;
        public float fadeTime;
        public bool isBundleUnload;
        public bool is2D;
      }
    }
  }
}
