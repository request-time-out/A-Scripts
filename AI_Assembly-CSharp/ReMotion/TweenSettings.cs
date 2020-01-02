// Decompiled with JetBrains decompiler
// Type: ReMotion.TweenSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ReMotion
{
  public sealed class TweenSettings
  {
    public static TweenSettings Default = new TweenSettings(LoopType.None, false, (EasingFunction) null);
    public static readonly TweenSettings Cycle = new TweenSettings(LoopType.None, false, (EasingFunction) null)
    {
      LoopType = LoopType.Cycle
    };
    public static readonly TweenSettings Restart = new TweenSettings(LoopType.None, false, (EasingFunction) null)
    {
      LoopType = LoopType.Restart
    };
    public static readonly TweenSettings CycleOnce = new TweenSettings(LoopType.None, false, (EasingFunction) null)
    {
      LoopType = LoopType.CycleOnce
    };
    public static readonly TweenSettings IgnoreTimeScale = new TweenSettings(LoopType.None, false, (EasingFunction) null)
    {
      IsIgnoreTimeScale = true
    };
    public static readonly TweenSettings IgnoreTimeScaleCycle = new TweenSettings(LoopType.None, false, (EasingFunction) null)
    {
      IsIgnoreTimeScale = true,
      LoopType = LoopType.Cycle
    };
    public static readonly TweenSettings IgnoreTimeScaleRestart = new TweenSettings(LoopType.None, false, (EasingFunction) null)
    {
      IsIgnoreTimeScale = true,
      LoopType = LoopType.Restart
    };
    public static readonly TweenSettings IgnoreTimeScaleCycleOnce = new TweenSettings(LoopType.None, false, (EasingFunction) null)
    {
      IsIgnoreTimeScale = true,
      LoopType = LoopType.CycleOnce
    };

    public TweenSettings(LoopType loopType = LoopType.None, bool ignoreTimeScale = false, EasingFunction defaultEasing = null)
    {
      this.LoopType = loopType;
      this.IsIgnoreTimeScale = ignoreTimeScale;
      this.DefaultEasing = defaultEasing ?? EasingFunctions.EaseOutQuad;
    }

    public static void SetDefault(TweenSettings settings)
    {
      TweenSettings.Default = settings;
    }

    public LoopType LoopType { get; private set; }

    public bool IsIgnoreTimeScale { get; private set; }

    public EasingFunction DefaultEasing { get; private set; }
  }
}
