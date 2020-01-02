// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Tween
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AIProject.Definitions
{
  public static class Tween
  {
    public static ReadOnlyDictionary<MotionType, EasingFunction> MotionFunctionTable { get; } = new ReadOnlyDictionary<MotionType, EasingFunction>((IDictionary<MotionType, EasingFunction>) new Dictionary<MotionType, EasingFunction>()
    {
      [MotionType.Linear] = EasingFunctions.Linear,
      [MotionType.EaseInSine] = EasingFunctions.EaseInSine,
      [MotionType.EaseOutSine] = EasingFunctions.EaseOutSine,
      [MotionType.EaseInOutSine] = EasingFunctions.EaseInOutSine,
      [MotionType.EaseInQuad] = EasingFunctions.EaseInQuad,
      [MotionType.EaseOutQuad] = EasingFunctions.EaseOutQuad,
      [MotionType.EaseInOutQuad] = EasingFunctions.EaseInOutQuad,
      [MotionType.EaseInCubic] = EasingFunctions.EaseInCubic,
      [MotionType.EaseOutCubic] = EasingFunctions.EaseOutCubic,
      [MotionType.EaseInOutCubic] = EasingFunctions.EaseInOutCubic,
      [MotionType.EaseInQuart] = EasingFunctions.EaseInQuart,
      [MotionType.EaseOutQuart] = EasingFunctions.EaseOutQuart,
      [MotionType.EaseInOutQuart] = EasingFunctions.EaseInOutQuart,
      [MotionType.EaseInQuint] = EasingFunctions.EaseInQuint,
      [MotionType.EaseOutQuint] = EasingFunctions.EaseOutQuint,
      [MotionType.EaseInOutQuint] = EasingFunctions.EaseInOutQuint,
      [MotionType.EaseInExpo] = EasingFunctions.EaseInExpo,
      [MotionType.EaseOutExpo] = EasingFunctions.EaseOutExpo,
      [MotionType.EaseInOutExpo] = EasingFunctions.EaseInOutExpo,
      [MotionType.EaseInCirc] = EasingFunctions.EaseInCirc,
      [MotionType.EaseOutCirc] = EasingFunctions.EaseOutCirc,
      [MotionType.EaseInOutCirc] = EasingFunctions.EaseInOutCirc,
      [MotionType.EaseInBack] = EasingFunctions.EaseInBack(1.70158f),
      [MotionType.EaseOutBack] = EasingFunctions.EaseOutBack(1.70158f),
      [MotionType.EaseInOutBack] = EasingFunctions.EaseInOutBack(1.70158f),
      [MotionType.EaseInElastic] = EasingFunctions.EaseInElastic(1.70158f, 0.0f),
      [MotionType.EaseOutElastic] = EasingFunctions.EaseOutElastic(1.70158f, 0.0f),
      [MotionType.EaseInOutElastic] = EasingFunctions.EaseInOutElastic(1.70158f, 0.0f),
      [MotionType.EaseInBounce] = EasingFunctions.EaseInBounce,
      [MotionType.EaseOutBounce] = EasingFunctions.EaseOutBounce,
      [MotionType.EaseInOutBounce] = EasingFunctions.EaseInOutBounce
    });
  }
}
