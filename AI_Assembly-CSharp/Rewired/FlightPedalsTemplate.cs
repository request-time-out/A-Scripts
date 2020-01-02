// Decompiled with JetBrains decompiler
// Type: Rewired.FlightPedalsTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace Rewired
{
  public sealed class FlightPedalsTemplate : ControllerTemplate, IFlightPedalsTemplate, IControllerTemplate
  {
    public static readonly Guid typeGuid = new Guid("f6fe76f8-be2a-4db2-b853-9e3652075913");
    public const int elementId_leftPedal = 0;
    public const int elementId_rightPedal = 1;
    public const int elementId_slide = 2;

    public FlightPedalsTemplate(object payload)
    {
      base.\u002Ector(payload);
    }

    IControllerTemplateAxis IFlightPedalsTemplate.leftPedal
    {
      get
      {
        return (IControllerTemplateAxis) this.GetElement<IControllerTemplateAxis>(0);
      }
    }

    IControllerTemplateAxis IFlightPedalsTemplate.rightPedal
    {
      get
      {
        return (IControllerTemplateAxis) this.GetElement<IControllerTemplateAxis>(1);
      }
    }

    IControllerTemplateAxis IFlightPedalsTemplate.slide
    {
      get
      {
        return (IControllerTemplateAxis) this.GetElement<IControllerTemplateAxis>(2);
      }
    }
  }
}
