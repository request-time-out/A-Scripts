// Decompiled with JetBrains decompiler
// Type: Rewired.Internal.ControllerTemplateFactory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace Rewired.Internal
{
  public static class ControllerTemplateFactory
  {
    private static readonly Type[] _defaultTemplateTypes = new Type[6]
    {
      typeof (GamepadTemplate),
      typeof (RacingWheelTemplate),
      typeof (HOTASTemplate),
      typeof (FlightYokeTemplate),
      typeof (FlightPedalsTemplate),
      typeof (SixDofControllerTemplate)
    };
    private static readonly Type[] _defaultTemplateInterfaceTypes = new Type[6]
    {
      typeof (IGamepadTemplate),
      typeof (IRacingWheelTemplate),
      typeof (IHOTASTemplate),
      typeof (IFlightYokeTemplate),
      typeof (IFlightPedalsTemplate),
      typeof (ISixDofControllerTemplate)
    };

    public static Type[] templateTypes
    {
      get
      {
        return ControllerTemplateFactory._defaultTemplateTypes;
      }
    }

    public static Type[] templateInterfaceTypes
    {
      get
      {
        return ControllerTemplateFactory._defaultTemplateInterfaceTypes;
      }
    }

    public static IControllerTemplate Create(Guid typeGuid, object payload)
    {
      if (typeGuid == GamepadTemplate.typeGuid)
        return (IControllerTemplate) new GamepadTemplate(payload);
      if (typeGuid == RacingWheelTemplate.typeGuid)
        return (IControllerTemplate) new RacingWheelTemplate(payload);
      if (typeGuid == HOTASTemplate.typeGuid)
        return (IControllerTemplate) new HOTASTemplate(payload);
      if (typeGuid == FlightYokeTemplate.typeGuid)
        return (IControllerTemplate) new FlightYokeTemplate(payload);
      if (typeGuid == FlightPedalsTemplate.typeGuid)
        return (IControllerTemplate) new FlightPedalsTemplate(payload);
      return typeGuid == SixDofControllerTemplate.typeGuid ? (IControllerTemplate) new SixDofControllerTemplate(payload) : (IControllerTemplate) null;
    }
  }
}
