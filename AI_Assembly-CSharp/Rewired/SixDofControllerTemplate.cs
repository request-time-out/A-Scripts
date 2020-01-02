﻿// Decompiled with JetBrains decompiler
// Type: Rewired.SixDofControllerTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace Rewired
{
  public sealed class SixDofControllerTemplate : ControllerTemplate, ISixDofControllerTemplate, IControllerTemplate
  {
    public static readonly Guid typeGuid = new Guid("2599beb3-522b-43dd-a4ef-93fd60e5eafa");
    public const int elementId_positionX = 1;
    public const int elementId_positionY = 2;
    public const int elementId_positionZ = 0;
    public const int elementId_rotationX = 3;
    public const int elementId_rotationY = 5;
    public const int elementId_rotationZ = 4;
    public const int elementId_throttle1Axis = 6;
    public const int elementId_throttle1MinDetent = 50;
    public const int elementId_throttle2Axis = 7;
    public const int elementId_throttle2MinDetent = 51;
    public const int elementId_extraAxis1 = 8;
    public const int elementId_extraAxis2 = 9;
    public const int elementId_extraAxis3 = 10;
    public const int elementId_extraAxis4 = 11;
    public const int elementId_button1 = 12;
    public const int elementId_button2 = 13;
    public const int elementId_button3 = 14;
    public const int elementId_button4 = 15;
    public const int elementId_button5 = 16;
    public const int elementId_button6 = 17;
    public const int elementId_button7 = 18;
    public const int elementId_button8 = 19;
    public const int elementId_button9 = 20;
    public const int elementId_button10 = 21;
    public const int elementId_button11 = 22;
    public const int elementId_button12 = 23;
    public const int elementId_button13 = 24;
    public const int elementId_button14 = 25;
    public const int elementId_button15 = 26;
    public const int elementId_button16 = 27;
    public const int elementId_button17 = 28;
    public const int elementId_button18 = 29;
    public const int elementId_button19 = 30;
    public const int elementId_button20 = 31;
    public const int elementId_button21 = 55;
    public const int elementId_button22 = 56;
    public const int elementId_button23 = 57;
    public const int elementId_button24 = 58;
    public const int elementId_button25 = 59;
    public const int elementId_button26 = 60;
    public const int elementId_button27 = 61;
    public const int elementId_button28 = 62;
    public const int elementId_button29 = 63;
    public const int elementId_button30 = 64;
    public const int elementId_button31 = 65;
    public const int elementId_button32 = 66;
    public const int elementId_hat1Up = 32;
    public const int elementId_hat1UpRight = 33;
    public const int elementId_hat1Right = 34;
    public const int elementId_hat1DownRight = 35;
    public const int elementId_hat1Down = 36;
    public const int elementId_hat1DownLeft = 37;
    public const int elementId_hat1Left = 38;
    public const int elementId_hat1UpLeft = 39;
    public const int elementId_hat2Up = 40;
    public const int elementId_hat2UpRight = 41;
    public const int elementId_hat2Right = 42;
    public const int elementId_hat2DownRight = 43;
    public const int elementId_hat2Down = 44;
    public const int elementId_hat2DownLeft = 45;
    public const int elementId_hat2Left = 46;
    public const int elementId_hat2UpLeft = 47;
    public const int elementId_hat1 = 48;
    public const int elementId_hat2 = 49;
    public const int elementId_throttle1 = 52;
    public const int elementId_throttle2 = 53;
    public const int elementId_stick = 54;

    public SixDofControllerTemplate(object payload)
    {
      base.\u002Ector(payload);
    }

    IControllerTemplateAxis ISixDofControllerTemplate.extraAxis1
    {
      get
      {
        return (IControllerTemplateAxis) this.GetElement<IControllerTemplateAxis>(8);
      }
    }

    IControllerTemplateAxis ISixDofControllerTemplate.extraAxis2
    {
      get
      {
        return (IControllerTemplateAxis) this.GetElement<IControllerTemplateAxis>(9);
      }
    }

    IControllerTemplateAxis ISixDofControllerTemplate.extraAxis3
    {
      get
      {
        return (IControllerTemplateAxis) this.GetElement<IControllerTemplateAxis>(10);
      }
    }

    IControllerTemplateAxis ISixDofControllerTemplate.extraAxis4
    {
      get
      {
        return (IControllerTemplateAxis) this.GetElement<IControllerTemplateAxis>(11);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button1
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(12);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button2
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(13);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button3
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(14);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button4
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(15);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button5
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(16);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button6
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(17);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button7
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(18);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button8
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(19);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button9
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(20);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button10
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(21);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button11
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(22);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button12
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(23);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button13
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(24);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button14
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(25);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button15
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(26);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button16
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(27);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button17
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(28);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button18
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(29);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button19
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(30);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button20
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(31);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button21
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(55);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button22
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(56);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button23
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(57);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button24
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(58);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button25
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(59);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button26
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(60);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button27
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(61);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button28
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(62);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button29
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(63);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button30
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(64);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button31
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(65);
      }
    }

    IControllerTemplateButton ISixDofControllerTemplate.button32
    {
      get
      {
        return (IControllerTemplateButton) this.GetElement<IControllerTemplateButton>(66);
      }
    }

    IControllerTemplateHat ISixDofControllerTemplate.hat1
    {
      get
      {
        return (IControllerTemplateHat) this.GetElement<IControllerTemplateHat>(48);
      }
    }

    IControllerTemplateHat ISixDofControllerTemplate.hat2
    {
      get
      {
        return (IControllerTemplateHat) this.GetElement<IControllerTemplateHat>(49);
      }
    }

    IControllerTemplateThrottle ISixDofControllerTemplate.throttle1
    {
      get
      {
        return (IControllerTemplateThrottle) this.GetElement<IControllerTemplateThrottle>(52);
      }
    }

    IControllerTemplateThrottle ISixDofControllerTemplate.throttle2
    {
      get
      {
        return (IControllerTemplateThrottle) this.GetElement<IControllerTemplateThrottle>(53);
      }
    }

    IControllerTemplateStick6D ISixDofControllerTemplate.stick
    {
      get
      {
        return (IControllerTemplateStick6D) this.GetElement<IControllerTemplateStick6D>(54);
      }
    }
  }
}
