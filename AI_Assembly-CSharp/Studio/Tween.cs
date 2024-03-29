﻿// Decompiled with JetBrains decompiler
// Type: Studio.Tween
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Studio
{
  public class Tween : MonoBehaviour
  {
    public static ArrayList tweens = new ArrayList();
    private static GameObject cameraFade;
    public string id;
    public string type;
    public string method;
    public Tween.EaseType easeType;
    public float time;
    public float delay;
    public Tween.LoopType loopType;
    public bool isRunning;
    public bool isPaused;
    public string _name;
    private float runningTime;
    private float _percentage;
    private float delayStarted;
    private bool kinematic;
    private bool isLocal;
    private bool loop;
    private bool reverse;
    private bool wasPaused;
    private bool physics;
    private Hashtable tweenArguments;
    private Space space;
    private Tween.EasingFunction ease;
    private Tween.ApplyTween apply;
    private AudioSource audioSource;
    private Vector3[] vector3s;
    private Vector2[] vector2s;
    private Color[,] colors;
    private float[] floats;
    private Rect[] rects;
    private Tween.CRSpline path;
    private Vector3 preUpdate;
    private Vector3 postUpdate;
    private Tween.NamedValueColor namedcolorvalue;
    private float lastRealTime;
    private bool useRealTime;
    public Action<float> onUpdate;
    public Tween.CompleteFunction onComplete;

    public Tween()
    {
      base.\u002Ector();
    }

    public float percentage
    {
      get
      {
        return this._percentage;
      }
    }

    public static void Init(GameObject target)
    {
      Tween.MoveBy(target, Vector3.get_zero(), 0.0f);
    }

    public static void CameraFadeFrom(float amount, float time)
    {
      if (Object.op_Implicit((Object) Tween.cameraFade))
        Tween.CameraFadeFrom(Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
      else
        Debug.LogError((object) "iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void CameraFadeFrom(Hashtable args)
    {
      if (Object.op_Implicit((Object) Tween.cameraFade))
        Tween.ColorFrom(Tween.cameraFade, args);
      else
        Debug.LogError((object) "iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void CameraFadeTo(float amount, float time)
    {
      if (Object.op_Implicit((Object) Tween.cameraFade))
        Tween.CameraFadeTo(Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
      else
        Debug.LogError((object) "iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void CameraFadeTo(Hashtable args)
    {
      if (Object.op_Implicit((Object) Tween.cameraFade))
        Tween.ColorTo(Tween.cameraFade, args);
      else
        Debug.LogError((object) "iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void ValueTo(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      if (!args.Contains((object) "onupdate") || !args.Contains((object) "from") || !args.Contains((object) "to"))
      {
        Debug.LogError((object) "iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
      }
      else
      {
        args[(object) "type"] = (object) "value";
        if (args[(object) "from"].GetType() == typeof (Vector2))
          args[(object) "method"] = (object) "vector2";
        else if (args[(object) "from"].GetType() == typeof (Vector3))
          args[(object) "method"] = (object) "vector3";
        else if (args[(object) "from"].GetType() == typeof (Rect))
          args[(object) "method"] = (object) "rect";
        else if (args[(object) "from"].GetType() == typeof (float))
          args[(object) "method"] = (object) "float";
        else if (args[(object) "from"].GetType() == typeof (Color))
        {
          args[(object) "method"] = (object) "color";
        }
        else
        {
          Debug.LogError((object) "iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
          return;
        }
        if (!args.Contains((object) "easetype"))
          args.Add((object) "easetype", (object) Tween.EaseType.linear);
        Tween.Launch(target, args);
      }
    }

    public static void FadeFrom(GameObject target, float alpha, float time)
    {
      Tween.FadeFrom(target, Tween.Hash((object) nameof (alpha), (object) alpha, (object) nameof (time), (object) time));
    }

    public static void FadeFrom(GameObject target, Hashtable args)
    {
      Tween.ColorFrom(target, args);
    }

    public static void FadeTo(GameObject target, float alpha, float time)
    {
      Tween.FadeTo(target, Tween.Hash((object) nameof (alpha), (object) alpha, (object) nameof (time), (object) time));
    }

    public static void FadeTo(GameObject target, Hashtable args)
    {
      Tween.ColorTo(target, args);
    }

    public static void ColorFrom(GameObject target, Color color, float time)
    {
      Tween.ColorFrom(target, Tween.Hash((object) nameof (color), (object) color, (object) nameof (time), (object) time));
    }

    public static void ColorFrom(GameObject target, Hashtable args)
    {
      Color color1 = (Color) null;
      Color color2 = (Color) null;
      args = Tween.CleanArgs(args);
      if (!args.Contains((object) "includechildren") || (bool) args[(object) "includechildren"])
      {
        IEnumerator enumerator = target.get_transform().GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current = (Transform) enumerator.Current;
            Hashtable args1 = (Hashtable) args.Clone();
            args1[(object) "ischild"] = (object) true;
            Tween.ColorFrom(((Component) current).get_gameObject(), args1);
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      if (!args.Contains((object) "easetype"))
        args.Add((object) "easetype", (object) Tween.EaseType.linear);
      if (Object.op_Implicit((Object) target.GetComponent(typeof (GUITexture))))
        color2 = color1 = ((GUITexture) target.GetComponent<GUITexture>()).get_color();
      else if (Object.op_Implicit((Object) target.GetComponent(typeof (GUIText))))
        color2 = color1 = ((GUIText) target.GetComponent<GUIText>()).get_material().get_color();
      else if (Object.op_Implicit((Object) target.GetComponent<Renderer>()))
        color2 = color1 = ((Renderer) target.GetComponent<Renderer>()).get_material().get_color();
      else if (Object.op_Implicit((Object) target.GetComponent<Light>()))
        color2 = color1 = ((Light) target.GetComponent<Light>()).get_color();
      if (args.Contains((object) "color"))
      {
        color1 = (Color) args[(object) "color"];
      }
      else
      {
        if (args.Contains((object) "r"))
          color1.r = (__Null) (double) (float) args[(object) "r"];
        if (args.Contains((object) "g"))
          color1.g = (__Null) (double) (float) args[(object) "g"];
        if (args.Contains((object) "b"))
          color1.b = (__Null) (double) (float) args[(object) "b"];
        if (args.Contains((object) "a"))
          color1.a = (__Null) (double) (float) args[(object) "a"];
      }
      if (args.Contains((object) "amount"))
      {
        color1.a = (__Null) (double) (float) args[(object) "amount"];
        args.Remove((object) "amount");
      }
      else if (args.Contains((object) "alpha"))
      {
        color1.a = (__Null) (double) (float) args[(object) "alpha"];
        args.Remove((object) "alpha");
      }
      if (Object.op_Implicit((Object) target.GetComponent(typeof (GUITexture))))
        ((GUITexture) target.GetComponent<GUITexture>()).set_color(color1);
      else if (Object.op_Implicit((Object) target.GetComponent(typeof (GUIText))))
        ((GUIText) target.GetComponent<GUIText>()).get_material().set_color(color1);
      else if (Object.op_Implicit((Object) target.GetComponent<Renderer>()))
        ((Renderer) target.GetComponent<Renderer>()).get_material().set_color(color1);
      else if (Object.op_Implicit((Object) target.GetComponent<Light>()))
        ((Light) target.GetComponent<Light>()).set_color(color1);
      args[(object) "color"] = (object) color2;
      args[(object) "type"] = (object) "color";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void ColorTo(GameObject target, Color color, float time)
    {
      Tween.ColorTo(target, Tween.Hash((object) nameof (color), (object) color, (object) nameof (time), (object) time));
    }

    public static void ColorTo(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      if (!args.Contains((object) "includechildren") || (bool) args[(object) "includechildren"])
      {
        IEnumerator enumerator = target.get_transform().GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current = (Transform) enumerator.Current;
            Hashtable args1 = (Hashtable) args.Clone();
            args1[(object) "ischild"] = (object) true;
            Tween.ColorTo(((Component) current).get_gameObject(), args1);
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      if (!args.Contains((object) "easetype"))
        args.Add((object) "easetype", (object) Tween.EaseType.linear);
      args[(object) "type"] = (object) "color";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void AudioFrom(GameObject target, float volume, float pitch, float time)
    {
      Tween.AudioFrom(target, Tween.Hash((object) nameof (volume), (object) volume, (object) nameof (pitch), (object) pitch, (object) nameof (time), (object) time));
    }

    public static void AudioFrom(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      AudioSource component;
      if (args.Contains((object) "audiosource"))
        component = (AudioSource) args[(object) "audiosource"];
      else if (Object.op_Implicit((Object) target.GetComponent(typeof (AudioSource))))
      {
        component = (AudioSource) target.GetComponent<AudioSource>();
      }
      else
      {
        Debug.LogError((object) "iTween Error: AudioFrom requires an AudioSource.");
        return;
      }
      Vector2 vector2_1;
      Vector2 vector2_2;
      vector2_1.x = (__Null) (double) (vector2_2.x = (__Null) component.get_volume());
      vector2_1.y = (__Null) (double) (vector2_2.y = (__Null) component.get_pitch());
      if (args.Contains((object) "volume"))
        vector2_2.x = (__Null) (double) (float) args[(object) "volume"];
      if (args.Contains((object) "pitch"))
        vector2_2.y = (__Null) (double) (float) args[(object) "pitch"];
      component.set_volume((float) vector2_2.x);
      component.set_pitch((float) vector2_2.y);
      args[(object) "volume"] = (object) (float) vector2_1.x;
      args[(object) "pitch"] = (object) (float) vector2_1.y;
      if (!args.Contains((object) "easetype"))
        args.Add((object) "easetype", (object) Tween.EaseType.linear);
      args[(object) "type"] = (object) "audio";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void AudioTo(GameObject target, float volume, float pitch, float time)
    {
      Tween.AudioTo(target, Tween.Hash((object) nameof (volume), (object) volume, (object) nameof (pitch), (object) pitch, (object) nameof (time), (object) time));
    }

    public static void AudioTo(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      if (!args.Contains((object) "easetype"))
        args.Add((object) "easetype", (object) Tween.EaseType.linear);
      args[(object) "type"] = (object) "audio";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void Stab(GameObject target, AudioClip audioclip, float delay)
    {
      Tween.Stab(target, Tween.Hash((object) nameof (audioclip), (object) audioclip, (object) nameof (delay), (object) delay));
    }

    public static void Stab(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "stab";
      Tween.Launch(target, args);
    }

    public static void LookFrom(GameObject target, Vector3 looktarget, float time)
    {
      Tween.LookFrom(target, Tween.Hash((object) nameof (looktarget), (object) looktarget, (object) nameof (time), (object) time));
    }

    public static void LookFrom(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      Vector3 eulerAngles1 = target.get_transform().get_eulerAngles();
      if (args[(object) "looktarget"].GetType() == typeof (Transform))
      {
        Transform transform1 = target.get_transform();
        Transform transform2 = (Transform) args[(object) "looktarget"];
        Vector3? nullable = (Vector3?) args[(object) "up"];
        Vector3 vector3 = !nullable.HasValue ? Tween.Defaults.up : nullable.Value;
        transform1.LookAt(transform2, vector3);
      }
      else if (args[(object) "looktarget"].GetType() == typeof (Vector3))
      {
        Transform transform = target.get_transform();
        Vector3 vector3_1 = (Vector3) args[(object) "looktarget"];
        Vector3? nullable = (Vector3?) args[(object) "up"];
        Vector3 vector3_2 = !nullable.HasValue ? Tween.Defaults.up : nullable.Value;
        transform.LookAt(vector3_1, vector3_2);
      }
      if (args.Contains((object) "axis"))
      {
        Vector3 eulerAngles2 = target.get_transform().get_eulerAngles();
        switch ((string) args[(object) "axis"])
        {
          case "x":
            eulerAngles2.y = eulerAngles1.y;
            eulerAngles2.z = eulerAngles1.z;
            break;
          case "y":
            eulerAngles2.x = eulerAngles1.x;
            eulerAngles2.z = eulerAngles1.z;
            break;
          case "z":
            eulerAngles2.x = eulerAngles1.x;
            eulerAngles2.y = eulerAngles1.y;
            break;
        }
        target.get_transform().set_eulerAngles(eulerAngles2);
      }
      args[(object) "rotation"] = (object) eulerAngles1;
      args[(object) "type"] = (object) "rotate";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void LookTo(GameObject target, Vector3 looktarget, float time)
    {
      Tween.LookTo(target, Tween.Hash((object) nameof (looktarget), (object) looktarget, (object) nameof (time), (object) time));
    }

    public static void LookTo(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      if (args.Contains((object) "looktarget") && args[(object) "looktarget"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args[(object) "looktarget"];
        args[(object) "position"] = (object) new Vector3((float) transform.get_position().x, (float) transform.get_position().y, (float) transform.get_position().z);
        args[(object) "rotation"] = (object) new Vector3((float) transform.get_eulerAngles().x, (float) transform.get_eulerAngles().y, (float) transform.get_eulerAngles().z);
      }
      args[(object) "type"] = (object) "look";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void MoveTo(GameObject target, Vector3 position, float time)
    {
      Tween.MoveTo(target, Tween.Hash((object) nameof (position), (object) position, (object) nameof (time), (object) time));
    }

    public static Tween MoveTo(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      if (args.Contains((object) "position") && args[(object) "position"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args[(object) "position"];
        args[(object) "position"] = (object) new Vector3((float) transform.get_position().x, (float) transform.get_position().y, (float) transform.get_position().z);
        args[(object) "rotation"] = (object) new Vector3((float) transform.get_eulerAngles().x, (float) transform.get_eulerAngles().y, (float) transform.get_eulerAngles().z);
        args[(object) "scale"] = (object) new Vector3((float) transform.get_localScale().x, (float) transform.get_localScale().y, (float) transform.get_localScale().z);
      }
      args[(object) "type"] = (object) "move";
      args[(object) "method"] = (object) "to";
      return Tween.Launch(target, args);
    }

    public static void MoveFrom(GameObject target, Vector3 position, float time)
    {
      Tween.MoveFrom(target, Tween.Hash((object) nameof (position), (object) position, (object) nameof (time), (object) time));
    }

    public static void MoveFrom(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      bool flag = !args.Contains((object) "islocal") ? Tween.Defaults.isLocal : (bool) args[(object) "islocal"];
      if (args.Contains((object) "path"))
      {
        Vector3[] vector3Array1;
        if (args[(object) "path"].GetType() == typeof (Vector3[]))
        {
          Vector3[] vector3Array2 = (Vector3[]) args[(object) "path"];
          vector3Array1 = new Vector3[vector3Array2.Length];
          Array.Copy((Array) vector3Array2, (Array) vector3Array1, vector3Array2.Length);
        }
        else
        {
          Transform[] transformArray = (Transform[]) args[(object) "path"];
          vector3Array1 = new Vector3[transformArray.Length];
          for (int index = 0; index < transformArray.Length; ++index)
            vector3Array1[index] = transformArray[index].get_position();
        }
        if (Vector3.op_Inequality(vector3Array1[vector3Array1.Length - 1], target.get_transform().get_position()))
        {
          Vector3[] vector3Array2 = new Vector3[vector3Array1.Length + 1];
          Array.Copy((Array) vector3Array1, (Array) vector3Array2, vector3Array1.Length);
          if (flag)
          {
            vector3Array2[vector3Array2.Length - 1] = target.get_transform().get_localPosition();
            target.get_transform().set_localPosition(vector3Array2[0]);
          }
          else
          {
            vector3Array2[vector3Array2.Length - 1] = target.get_transform().get_position();
            target.get_transform().set_position(vector3Array2[0]);
          }
          args[(object) "path"] = (object) vector3Array2;
        }
        else
        {
          if (flag)
            target.get_transform().set_localPosition(vector3Array1[0]);
          else
            target.get_transform().set_position(vector3Array1[0]);
          args[(object) "path"] = (object) vector3Array1;
        }
      }
      else
      {
        Vector3 vector3_1;
        Vector3 vector3_2 = !flag ? (vector3_1 = target.get_transform().get_position()) : (vector3_1 = target.get_transform().get_localPosition());
        if (args.Contains((object) "position"))
        {
          if (args[(object) "position"].GetType() == typeof (Transform))
            vector3_1 = ((Transform) args[(object) "position"]).get_position();
          else if (args[(object) "position"].GetType() == typeof (Vector3))
            vector3_1 = (Vector3) args[(object) "position"];
        }
        else
        {
          if (args.Contains((object) "x"))
            vector3_1.x = (__Null) (double) (float) args[(object) "x"];
          if (args.Contains((object) "y"))
            vector3_1.y = (__Null) (double) (float) args[(object) "y"];
          if (args.Contains((object) "z"))
            vector3_1.z = (__Null) (double) (float) args[(object) "z"];
        }
        if (flag)
          target.get_transform().set_localPosition(vector3_1);
        else
          target.get_transform().set_position(vector3_1);
        args[(object) "position"] = (object) vector3_2;
      }
      args[(object) "type"] = (object) "move";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void MoveAdd(GameObject target, Vector3 amount, float time)
    {
      Tween.MoveAdd(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void MoveAdd(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "move";
      args[(object) "method"] = (object) "add";
      Tween.Launch(target, args);
    }

    public static void MoveBy(GameObject target, Vector3 amount, float time)
    {
      Tween.MoveBy(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void MoveBy(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "move";
      args[(object) "method"] = (object) "by";
      Tween.Launch(target, args);
    }

    public static void ScaleTo(GameObject target, Vector3 scale, float time)
    {
      Tween.ScaleTo(target, Tween.Hash((object) nameof (scale), (object) scale, (object) nameof (time), (object) time));
    }

    public static void ScaleTo(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      if (args.Contains((object) "scale") && args[(object) "scale"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args[(object) "scale"];
        args[(object) "position"] = (object) new Vector3((float) transform.get_position().x, (float) transform.get_position().y, (float) transform.get_position().z);
        args[(object) "rotation"] = (object) new Vector3((float) transform.get_eulerAngles().x, (float) transform.get_eulerAngles().y, (float) transform.get_eulerAngles().z);
        args[(object) "scale"] = (object) new Vector3((float) transform.get_localScale().x, (float) transform.get_localScale().y, (float) transform.get_localScale().z);
      }
      args[(object) "type"] = (object) "scale";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void ScaleFrom(GameObject target, Vector3 scale, float time)
    {
      Tween.ScaleFrom(target, Tween.Hash((object) nameof (scale), (object) scale, (object) nameof (time), (object) time));
    }

    public static void ScaleFrom(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      Vector3 localScale;
      Vector3 vector3 = localScale = target.get_transform().get_localScale();
      if (args.Contains((object) "scale"))
      {
        if (args[(object) "scale"].GetType() == typeof (Transform))
          localScale = ((Transform) args[(object) "scale"]).get_localScale();
        else if (args[(object) "scale"].GetType() == typeof (Vector3))
          localScale = (Vector3) args[(object) "scale"];
      }
      else
      {
        if (args.Contains((object) "x"))
          localScale.x = (__Null) (double) (float) args[(object) "x"];
        if (args.Contains((object) "y"))
          localScale.y = (__Null) (double) (float) args[(object) "y"];
        if (args.Contains((object) "z"))
          localScale.z = (__Null) (double) (float) args[(object) "z"];
      }
      target.get_transform().set_localScale(localScale);
      args[(object) "scale"] = (object) vector3;
      args[(object) "type"] = (object) "scale";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void ScaleAdd(GameObject target, Vector3 amount, float time)
    {
      Tween.ScaleAdd(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ScaleAdd(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "scale";
      args[(object) "method"] = (object) "add";
      Tween.Launch(target, args);
    }

    public static void ScaleBy(GameObject target, Vector3 amount, float time)
    {
      Tween.ScaleBy(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ScaleBy(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "scale";
      args[(object) "method"] = (object) "by";
      Tween.Launch(target, args);
    }

    public static void RotateTo(GameObject target, Vector3 rotation, float time)
    {
      Tween.RotateTo(target, Tween.Hash((object) nameof (rotation), (object) rotation, (object) nameof (time), (object) time));
    }

    public static void RotateTo(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      if (args.Contains((object) "rotation") && args[(object) "rotation"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args[(object) "rotation"];
        args[(object) "position"] = (object) new Vector3((float) transform.get_position().x, (float) transform.get_position().y, (float) transform.get_position().z);
        args[(object) "rotation"] = (object) new Vector3((float) transform.get_eulerAngles().x, (float) transform.get_eulerAngles().y, (float) transform.get_eulerAngles().z);
        args[(object) "scale"] = (object) new Vector3((float) transform.get_localScale().x, (float) transform.get_localScale().y, (float) transform.get_localScale().z);
      }
      args[(object) "type"] = (object) "rotate";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void RotateFrom(GameObject target, Vector3 rotation, float time)
    {
      Tween.RotateFrom(target, Tween.Hash((object) nameof (rotation), (object) rotation, (object) nameof (time), (object) time));
    }

    public static void RotateFrom(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      bool flag = !args.Contains((object) "islocal") ? Tween.Defaults.isLocal : (bool) args[(object) "islocal"];
      Vector3 vector3_1;
      Vector3 vector3_2 = !flag ? (vector3_1 = target.get_transform().get_eulerAngles()) : (vector3_1 = target.get_transform().get_localEulerAngles());
      if (args.Contains((object) "rotation"))
      {
        if (args[(object) "rotation"].GetType() == typeof (Transform))
          vector3_1 = ((Transform) args[(object) "rotation"]).get_eulerAngles();
        else if (args[(object) "rotation"].GetType() == typeof (Vector3))
          vector3_1 = (Vector3) args[(object) "rotation"];
      }
      else
      {
        if (args.Contains((object) "x"))
          vector3_1.x = (__Null) (double) (float) args[(object) "x"];
        if (args.Contains((object) "y"))
          vector3_1.y = (__Null) (double) (float) args[(object) "y"];
        if (args.Contains((object) "z"))
          vector3_1.z = (__Null) (double) (float) args[(object) "z"];
      }
      if (flag)
        target.get_transform().set_localEulerAngles(vector3_1);
      else
        target.get_transform().set_eulerAngles(vector3_1);
      args[(object) "rotation"] = (object) vector3_2;
      args[(object) "type"] = (object) "rotate";
      args[(object) "method"] = (object) "to";
      Tween.Launch(target, args);
    }

    public static void RotateAdd(GameObject target, Vector3 amount, float time)
    {
      Tween.RotateAdd(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void RotateAdd(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "rotate";
      args[(object) "method"] = (object) "add";
      Tween.Launch(target, args);
    }

    public static void RotateBy(GameObject target, Vector3 amount, float time)
    {
      Tween.RotateBy(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void RotateBy(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "rotate";
      args[(object) "method"] = (object) "by";
      Tween.Launch(target, args);
    }

    public static void ShakePosition(GameObject target, Vector3 amount, float time)
    {
      Tween.ShakePosition(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ShakePosition(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "shake";
      args[(object) "method"] = (object) "position";
      Tween.Launch(target, args);
    }

    public static void ShakeScale(GameObject target, Vector3 amount, float time)
    {
      Tween.ShakeScale(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ShakeScale(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "shake";
      args[(object) "method"] = (object) "scale";
      Tween.Launch(target, args);
    }

    public static void ShakeRotation(GameObject target, Vector3 amount, float time)
    {
      Tween.ShakeRotation(target, Tween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ShakeRotation(GameObject target, Hashtable args)
    {
      args = Tween.CleanArgs(args);
      args[(object) "type"] = (object) "shake";
      args[(object) "method"] = (object) "rotation";
      Tween.Launch(target, args);
    }

    private void GenerateTargets()
    {
      string type = this.type;
      if (type == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Tween.\u003C\u003Ef__switch\u0024mapD == null)
      {
        // ISSUE: reference to a compiler-generated field
        Tween.\u003C\u003Ef__switch\u0024mapD = new Dictionary<string, int>(10)
        {
          {
            "value",
            0
          },
          {
            "color",
            1
          },
          {
            "audio",
            2
          },
          {
            "move",
            3
          },
          {
            "scale",
            4
          },
          {
            "rotate",
            5
          },
          {
            "shake",
            6
          },
          {
            "punch",
            7
          },
          {
            "look",
            8
          },
          {
            "stab",
            9
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!Tween.\u003C\u003Ef__switch\u0024mapD.TryGetValue(type, out num))
        return;
      switch (num)
      {
        case 0:
          switch (this.method)
          {
            case "float":
              this.GenerateFloatTargets();
              this.apply = new Tween.ApplyTween(this.ApplyFloatTargets);
              return;
            case "vector2":
              this.GenerateVector2Targets();
              this.apply = new Tween.ApplyTween(this.ApplyVector2Targets);
              return;
            case "vector3":
              this.GenerateVector3Targets();
              this.apply = new Tween.ApplyTween(this.ApplyVector3Targets);
              return;
            case "color":
              this.GenerateColorTargets();
              this.apply = new Tween.ApplyTween(this.ApplyColorTargets);
              return;
            case "rect":
              this.GenerateRectTargets();
              this.apply = new Tween.ApplyTween(this.ApplyRectTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 1:
          switch (this.method)
          {
            case "to":
              this.GenerateColorToTargets();
              this.apply = new Tween.ApplyTween(this.ApplyColorToTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 2:
          switch (this.method)
          {
            case "to":
              this.GenerateAudioToTargets();
              this.apply = new Tween.ApplyTween(this.ApplyAudioToTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 3:
          switch (this.method)
          {
            case "to":
              if (this.tweenArguments.Contains((object) "path"))
              {
                this.GenerateMoveToPathTargets();
                this.apply = new Tween.ApplyTween(this.ApplyMoveToPathTargets);
                return;
              }
              this.GenerateMoveToTargets();
              this.apply = new Tween.ApplyTween(this.ApplyMoveToTargets);
              return;
            case "by":
            case "add":
              this.GenerateMoveByTargets();
              this.apply = new Tween.ApplyTween(this.ApplyMoveByTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 4:
          switch (this.method)
          {
            case "to":
              this.GenerateScaleToTargets();
              this.apply = new Tween.ApplyTween(this.ApplyScaleToTargets);
              return;
            case "by":
              this.GenerateScaleByTargets();
              this.apply = new Tween.ApplyTween(this.ApplyScaleToTargets);
              return;
            case "add":
              this.GenerateScaleAddTargets();
              this.apply = new Tween.ApplyTween(this.ApplyScaleToTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 5:
          switch (this.method)
          {
            case "to":
              this.GenerateRotateToTargets();
              this.apply = new Tween.ApplyTween(this.ApplyRotateToTargets);
              return;
            case "add":
              this.GenerateRotateAddTargets();
              this.apply = new Tween.ApplyTween(this.ApplyRotateAddTargets);
              return;
            case "by":
              this.GenerateRotateByTargets();
              this.apply = new Tween.ApplyTween(this.ApplyRotateAddTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 6:
          switch (this.method)
          {
            case "position":
              this.GenerateShakePositionTargets();
              this.apply = new Tween.ApplyTween(this.ApplyShakePositionTargets);
              return;
            case "scale":
              this.GenerateShakeScaleTargets();
              this.apply = new Tween.ApplyTween(this.ApplyShakeScaleTargets);
              return;
            case "rotation":
              this.GenerateShakeRotationTargets();
              this.apply = new Tween.ApplyTween(this.ApplyShakeRotationTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 7:
          switch (this.method)
          {
            case "position":
              this.GeneratePunchPositionTargets();
              this.apply = new Tween.ApplyTween(this.ApplyPunchPositionTargets);
              return;
            case "rotation":
              this.GeneratePunchRotationTargets();
              this.apply = new Tween.ApplyTween(this.ApplyPunchRotationTargets);
              return;
            case "scale":
              this.GeneratePunchScaleTargets();
              this.apply = new Tween.ApplyTween(this.ApplyPunchScaleTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 8:
          switch (this.method)
          {
            case "to":
              this.GenerateLookToTargets();
              this.apply = new Tween.ApplyTween(this.ApplyLookToTargets);
              return;
            case null:
              return;
            default:
              return;
          }
        case 9:
          this.GenerateStabTargets();
          this.apply = new Tween.ApplyTween(this.ApplyStabTargets);
          break;
      }
    }

    private void GenerateRectTargets()
    {
      this.rects = new Rect[3];
      this.rects[0] = (Rect) this.tweenArguments[(object) "from"];
      this.rects[1] = (Rect) this.tweenArguments[(object) "to"];
    }

    private void GenerateColorTargets()
    {
      this.colors = new Color[1, 3];
      this.colors[0, 0] = (Color) this.tweenArguments[(object) "from"];
      this.colors[0, 1] = (Color) this.tweenArguments[(object) "to"];
    }

    private void GenerateVector3Targets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = (Vector3) this.tweenArguments[(object) "from"];
      this.vector3s[1] = (Vector3) this.tweenArguments[(object) "to"];
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateVector2Targets()
    {
      this.vector2s = new Vector2[3];
      this.vector2s[0] = (Vector2) this.tweenArguments[(object) "from"];
      this.vector2s[1] = (Vector2) this.tweenArguments[(object) "to"];
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector((float) this.vector2s[0].x, (float) this.vector2s[0].y, 0.0f);
      Vector3 vector3_2;
      ((Vector3) ref vector3_2).\u002Ector((float) this.vector2s[1].x, (float) this.vector2s[1].y, 0.0f);
      this.time = Math.Abs(Vector3.Distance(vector3_1, vector3_2)) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateFloatTargets()
    {
      this.floats = new float[3];
      this.floats[0] = (float) this.tweenArguments[(object) "from"];
      this.floats[1] = (float) this.tweenArguments[(object) "to"];
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(this.floats[0] - this.floats[1]) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateColorToTargets()
    {
      if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (GUITexture))))
      {
        this.colors = new Color[1, 3];
        this.colors[0, 0] = this.colors[0, 1] = ((GUITexture) ((Component) this).GetComponent<GUITexture>()).get_color();
      }
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (GUIText))))
      {
        this.colors = new Color[1, 3];
        this.colors[0, 0] = this.colors[0, 1] = ((GUIText) ((Component) this).GetComponent<GUIText>()).get_material().get_color();
      }
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent<Renderer>()))
      {
        this.colors = new Color[((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials().Length, 3];
        for (int index = 0; index < ((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials().Length; ++index)
        {
          this.colors[index, 0] = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials()[index].GetColor(this.namedcolorvalue.ToString());
          this.colors[index, 1] = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials()[index].GetColor(this.namedcolorvalue.ToString());
        }
      }
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent<Light>()))
      {
        this.colors = new Color[1, 3];
        this.colors[0, 0] = this.colors[0, 1] = ((Light) ((Component) this).GetComponent<Light>()).get_color();
      }
      else
        this.colors = new Color[1, 3];
      if (this.tweenArguments.Contains((object) "color"))
      {
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          this.colors[index, 1] = (Color) this.tweenArguments[(object) "color"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "r"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].r = (__Null) (double) (float) this.tweenArguments[(object) "r"];
        }
        if (this.tweenArguments.Contains((object) "g"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].g = (__Null) (double) (float) this.tweenArguments[(object) "g"];
        }
        if (this.tweenArguments.Contains((object) "b"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].b = (__Null) (double) (float) this.tweenArguments[(object) "b"];
        }
        if (this.tweenArguments.Contains((object) "a"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].a = (__Null) (double) (float) this.tweenArguments[(object) "a"];
        }
      }
      if (this.tweenArguments.Contains((object) "amount"))
      {
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          this.colors[index, 1].a = (__Null) (double) (float) this.tweenArguments[(object) "amount"];
      }
      else
      {
        if (!this.tweenArguments.Contains((object) "alpha"))
          return;
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          this.colors[index, 1].a = (__Null) (double) (float) this.tweenArguments[(object) "alpha"];
      }
    }

    private void GenerateAudioToTargets()
    {
      this.vector2s = new Vector2[3];
      if (this.tweenArguments.Contains((object) "audiosource"))
        this.audioSource = (AudioSource) this.tweenArguments[(object) "audiosource"];
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (AudioSource))))
      {
        this.audioSource = (AudioSource) ((Component) this).GetComponent<AudioSource>();
      }
      else
      {
        Debug.LogError((object) "iTween Error: AudioTo requires an AudioSource.");
        this.Dispose();
      }
      this.vector2s[0] = this.vector2s[1] = new Vector2(this.audioSource.get_volume(), this.audioSource.get_pitch());
      if (this.tweenArguments.Contains((object) "volume"))
        this.vector2s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "volume"];
      if (!this.tweenArguments.Contains((object) "pitch"))
        return;
      this.vector2s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "pitch"];
    }

    private void GenerateStabTargets()
    {
      if (this.tweenArguments.Contains((object) "audiosource"))
        this.audioSource = (AudioSource) this.tweenArguments[(object) "audiosource"];
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (AudioSource))))
      {
        this.audioSource = (AudioSource) ((Component) this).GetComponent<AudioSource>();
      }
      else
      {
        ((Component) this).get_gameObject().AddComponent(typeof (AudioSource));
        this.audioSource = (AudioSource) ((Component) this).GetComponent<AudioSource>();
        this.audioSource.set_playOnAwake(false);
      }
      this.audioSource.set_clip((AudioClip) this.tweenArguments[(object) "audioclip"]);
      if (this.tweenArguments.Contains((object) "pitch"))
        this.audioSource.set_pitch((float) this.tweenArguments[(object) "pitch"]);
      if (this.tweenArguments.Contains((object) "volume"))
        this.audioSource.set_volume((float) this.tweenArguments[(object) "volume"]);
      this.time = this.audioSource.get_clip().get_length() / this.audioSource.get_pitch();
    }

    private void GenerateLookToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = ((Component) this).get_transform().get_eulerAngles();
      if (this.tweenArguments.Contains((object) "looktarget"))
      {
        if (this.tweenArguments[(object) "looktarget"].GetType() == typeof (Transform))
        {
          Transform transform = ((Component) this).get_transform();
          Transform tweenArgument1 = (Transform) this.tweenArguments[(object) "looktarget"];
          Vector3? tweenArgument2 = (Vector3?) this.tweenArguments[(object) "up"];
          Vector3 vector3 = !tweenArgument2.HasValue ? Tween.Defaults.up : tweenArgument2.Value;
          transform.LookAt(tweenArgument1, vector3);
        }
        else if (this.tweenArguments[(object) "looktarget"].GetType() == typeof (Vector3))
        {
          Transform transform = ((Component) this).get_transform();
          Vector3 tweenArgument1 = (Vector3) this.tweenArguments[(object) "looktarget"];
          Vector3? tweenArgument2 = (Vector3?) this.tweenArguments[(object) "up"];
          Vector3 vector3 = !tweenArgument2.HasValue ? Tween.Defaults.up : tweenArgument2.Value;
          transform.LookAt(tweenArgument1, vector3);
        }
      }
      else
      {
        Debug.LogError((object) "iTween Error: LookTo needs a 'looktarget' property!");
        this.Dispose();
      }
      this.vector3s[1] = ((Component) this).get_transform().get_eulerAngles();
      ((Component) this).get_transform().set_eulerAngles(this.vector3s[0]);
      if (this.tweenArguments.Contains((object) "axis"))
      {
        switch ((string) this.tweenArguments[(object) "axis"])
        {
          case "x":
            this.vector3s[1].y = this.vector3s[0].y;
            this.vector3s[1].z = this.vector3s[0].z;
            break;
          case "y":
            this.vector3s[1].x = this.vector3s[0].x;
            this.vector3s[1].z = this.vector3s[0].z;
            break;
          case "z":
            this.vector3s[1].x = this.vector3s[0].x;
            this.vector3s[1].y = this.vector3s[0].y;
            break;
        }
      }
      this.vector3s[1] = new Vector3(this.clerp((float) this.vector3s[0].x, (float) this.vector3s[1].x, 1f), this.clerp((float) this.vector3s[0].y, (float) this.vector3s[1].y, 1f), this.clerp((float) this.vector3s[0].z, (float) this.vector3s[1].z, 1f));
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateMoveToPathTargets()
    {
      Vector3[] vector3Array1;
      if (this.tweenArguments[(object) "path"].GetType() == typeof (Vector3[]))
      {
        Vector3[] tweenArgument = (Vector3[]) this.tweenArguments[(object) "path"];
        if (tweenArgument.Length == 1)
        {
          Debug.LogError((object) "iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
          this.Dispose();
        }
        vector3Array1 = new Vector3[tweenArgument.Length];
        Array.Copy((Array) tweenArgument, (Array) vector3Array1, tweenArgument.Length);
      }
      else
      {
        Transform[] tweenArgument = (Transform[]) this.tweenArguments[(object) "path"];
        if (tweenArgument.Length == 1)
        {
          Debug.LogError((object) "iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
          this.Dispose();
        }
        vector3Array1 = new Vector3[tweenArgument.Length];
        for (int index = 0; index < tweenArgument.Length; ++index)
          vector3Array1[index] = tweenArgument[index].get_position();
      }
      bool flag;
      int num;
      if (Vector3.op_Inequality(((Component) this).get_transform().get_position(), vector3Array1[0]))
      {
        if (!this.tweenArguments.Contains((object) "movetopath") || (bool) this.tweenArguments[(object) "movetopath"])
        {
          flag = true;
          num = 3;
        }
        else
        {
          flag = false;
          num = 2;
        }
      }
      else
      {
        flag = false;
        num = 2;
      }
      this.vector3s = new Vector3[vector3Array1.Length + num];
      int destinationIndex;
      if (flag)
      {
        this.vector3s[1] = ((Component) this).get_transform().get_position();
        destinationIndex = 2;
      }
      else
        destinationIndex = 1;
      Array.Copy((Array) vector3Array1, 0, (Array) this.vector3s, destinationIndex, vector3Array1.Length);
      this.vector3s[0] = Vector3.op_Addition(this.vector3s[1], Vector3.op_Subtraction(this.vector3s[1], this.vector3s[2]));
      this.vector3s[this.vector3s.Length - 1] = Vector3.op_Addition(this.vector3s[this.vector3s.Length - 2], Vector3.op_Subtraction(this.vector3s[this.vector3s.Length - 2], this.vector3s[this.vector3s.Length - 3]));
      if (Vector3.op_Equality(this.vector3s[1], this.vector3s[this.vector3s.Length - 2]))
      {
        Vector3[] vector3Array2 = new Vector3[this.vector3s.Length];
        Array.Copy((Array) this.vector3s, (Array) vector3Array2, this.vector3s.Length);
        vector3Array2[0] = vector3Array2[vector3Array2.Length - 3];
        vector3Array2[vector3Array2.Length - 1] = vector3Array2[2];
        this.vector3s = new Vector3[vector3Array2.Length];
        Array.Copy((Array) vector3Array2, (Array) this.vector3s, vector3Array2.Length);
      }
      this.path = new Tween.CRSpline(this.vector3s);
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Tween.PathLength(this.vector3s) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateMoveToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = !this.isLocal ? (this.vector3s[1] = ((Component) this).get_transform().get_position()) : (this.vector3s[1] = ((Component) this).get_transform().get_localPosition());
      if (this.tweenArguments.Contains((object) "position"))
      {
        if (this.tweenArguments[(object) "position"].GetType() == typeof (Transform))
          this.vector3s[1] = ((Transform) this.tweenArguments[(object) "position"]).get_position();
        else if (this.tweenArguments[(object) "position"].GetType() == typeof (Vector3))
          this.vector3s[1] = (Vector3) this.tweenArguments[(object) "position"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (this.tweenArguments.Contains((object) "z"))
          this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
      if (this.tweenArguments.Contains((object) "orienttopath") && (bool) this.tweenArguments[(object) "orienttopath"])
        this.tweenArguments[(object) "looktarget"] = (object) this.vector3s[1];
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateMoveByTargets()
    {
      this.vector3s = new Vector3[6];
      this.vector3s[4] = ((Component) this).get_transform().get_eulerAngles();
      this.vector3s[0] = this.vector3s[1] = this.vector3s[3] = ((Component) this).get_transform().get_position();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = Vector3.op_Addition(this.vector3s[0], (Vector3) this.tweenArguments[(object) "amount"]);
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (this.vector3s[0].x + (double) (float) this.tweenArguments[(object) "x"]);
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (this.vector3s[0].y + (double) (float) this.tweenArguments[(object) "y"]);
        if (this.tweenArguments.Contains((object) "z"))
          this.vector3s[1].z = (__Null) (this.vector3s[0].z + (double) (float) this.tweenArguments[(object) "z"]);
      }
      ((Component) this).get_transform().Translate(this.vector3s[1], this.space);
      this.vector3s[5] = ((Component) this).get_transform().get_position();
      ((Component) this).get_transform().set_position(this.vector3s[0]);
      if (this.tweenArguments.Contains((object) "orienttopath") && (bool) this.tweenArguments[(object) "orienttopath"])
        this.tweenArguments[(object) "looktarget"] = (object) this.vector3s[1];
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateScaleToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.vector3s[1] = ((Component) this).get_transform().get_localScale();
      if (this.tweenArguments.Contains((object) "scale"))
      {
        if (this.tweenArguments[(object) "scale"].GetType() == typeof (Transform))
          this.vector3s[1] = ((Transform) this.tweenArguments[(object) "scale"]).get_localScale();
        else if (this.tweenArguments[(object) "scale"].GetType() == typeof (Vector3))
          this.vector3s[1] = (Vector3) this.tweenArguments[(object) "scale"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (this.tweenArguments.Contains((object) "z"))
          this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateScaleByTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.vector3s[1] = ((Component) this).get_transform().get_localScale();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3) this.tweenArguments[(object) "amount"]);
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.x = (__Null) (local.x * (double) (float) this.tweenArguments[(object) "x"]);
        }
        if (this.tweenArguments.Contains((object) "y"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.y = (__Null) (local.y * (double) (float) this.tweenArguments[(object) "y"]);
        }
        if (this.tweenArguments.Contains((object) "z"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.z = (__Null) (local.z * (double) (float) this.tweenArguments[(object) "z"]);
        }
      }
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateScaleAddTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.vector3s[1] = ((Component) this).get_transform().get_localScale();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        ref Vector3 local = ref this.vector3s[1];
        local = Vector3.op_Addition(local, (Vector3) this.tweenArguments[(object) "amount"]);
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.x = (__Null) (local.x + (double) (float) this.tweenArguments[(object) "x"]);
        }
        if (this.tweenArguments.Contains((object) "y"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.y = (__Null) (local.y + (double) (float) this.tweenArguments[(object) "y"]);
        }
        if (this.tweenArguments.Contains((object) "z"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.z = (__Null) (local.z + (double) (float) this.tweenArguments[(object) "z"]);
        }
      }
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateRotateToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = !this.isLocal ? (this.vector3s[1] = ((Component) this).get_transform().get_eulerAngles()) : (this.vector3s[1] = ((Component) this).get_transform().get_localEulerAngles());
      if (this.tweenArguments.Contains((object) "rotation"))
      {
        if (this.tweenArguments[(object) "rotation"].GetType() == typeof (Transform))
          this.vector3s[1] = ((Transform) this.tweenArguments[(object) "rotation"]).get_eulerAngles();
        else if (this.tweenArguments[(object) "rotation"].GetType() == typeof (Vector3))
          this.vector3s[1] = (Vector3) this.tweenArguments[(object) "rotation"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (this.tweenArguments.Contains((object) "z"))
          this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
      this.vector3s[1] = new Vector3(this.clerp((float) this.vector3s[0].x, (float) this.vector3s[1].x, 1f), this.clerp((float) this.vector3s[0].y, (float) this.vector3s[1].y, 1f), this.clerp((float) this.vector3s[0].z, (float) this.vector3s[1].z, 1f));
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateRotateAddTargets()
    {
      this.vector3s = new Vector3[5];
      this.vector3s[0] = this.vector3s[1] = this.vector3s[3] = ((Component) this).get_transform().get_eulerAngles();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        ref Vector3 local = ref this.vector3s[1];
        local = Vector3.op_Addition(local, (Vector3) this.tweenArguments[(object) "amount"]);
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.x = (__Null) (local.x + (double) (float) this.tweenArguments[(object) "x"]);
        }
        if (this.tweenArguments.Contains((object) "y"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.y = (__Null) (local.y + (double) (float) this.tweenArguments[(object) "y"]);
        }
        if (this.tweenArguments.Contains((object) "z"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.z = (__Null) (local.z + (double) (float) this.tweenArguments[(object) "z"]);
        }
      }
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateRotateByTargets()
    {
      this.vector3s = new Vector3[4];
      this.vector3s[0] = this.vector3s[1] = this.vector3s[3] = ((Component) this).get_transform().get_eulerAngles();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        ref Vector3 local = ref this.vector3s[1];
        local = Vector3.op_Addition(local, Vector3.Scale((Vector3) this.tweenArguments[(object) "amount"], new Vector3(360f, 360f, 360f)));
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.x = (__Null) (local.x + 360.0 * (double) (float) this.tweenArguments[(object) "x"]);
        }
        if (this.tweenArguments.Contains((object) "y"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.y = (__Null) (local.y + 360.0 * (double) (float) this.tweenArguments[(object) "y"]);
        }
        if (this.tweenArguments.Contains((object) "z"))
        {
          ref Vector3 local = ref this.vector3s[1];
          local.z = (__Null) (local.z + 360.0 * (double) (float) this.tweenArguments[(object) "z"]);
        }
      }
      if (!this.tweenArguments.Contains((object) "speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments[(object) "speed"];
    }

    private void GenerateShakePositionTargets()
    {
      this.vector3s = new Vector3[4];
      this.vector3s[3] = ((Component) this).get_transform().get_eulerAngles();
      this.vector3s[0] = ((Component) this).get_transform().get_position();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments[(object) "amount"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (!this.tweenArguments.Contains((object) "z"))
          return;
        this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
    }

    private void GenerateShakeScaleTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = ((Component) this).get_transform().get_localScale();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments[(object) "amount"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (!this.tweenArguments.Contains((object) "z"))
          return;
        this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
    }

    private void GenerateShakeRotationTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = ((Component) this).get_transform().get_eulerAngles();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments[(object) "amount"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (!this.tweenArguments.Contains((object) "z"))
          return;
        this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
    }

    private void GeneratePunchPositionTargets()
    {
      this.vector3s = new Vector3[5];
      this.vector3s[4] = ((Component) this).get_transform().get_eulerAngles();
      this.vector3s[0] = ((Component) this).get_transform().get_position();
      this.vector3s[1] = this.vector3s[3] = Vector3.get_zero();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments[(object) "amount"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (!this.tweenArguments.Contains((object) "z"))
          return;
        this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
    }

    private void GeneratePunchRotationTargets()
    {
      this.vector3s = new Vector3[4];
      this.vector3s[0] = ((Component) this).get_transform().get_eulerAngles();
      this.vector3s[1] = this.vector3s[3] = Vector3.get_zero();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments[(object) "amount"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (!this.tweenArguments.Contains((object) "z"))
          return;
        this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
    }

    private void GeneratePunchScaleTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = ((Component) this).get_transform().get_localScale();
      this.vector3s[1] = Vector3.get_zero();
      if (this.tweenArguments.Contains((object) "amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments[(object) "amount"];
      }
      else
      {
        if (this.tweenArguments.Contains((object) "x"))
          this.vector3s[1].x = (__Null) (double) (float) this.tweenArguments[(object) "x"];
        if (this.tweenArguments.Contains((object) "y"))
          this.vector3s[1].y = (__Null) (double) (float) this.tweenArguments[(object) "y"];
        if (!this.tweenArguments.Contains((object) "z"))
          return;
        this.vector3s[1].z = (__Null) (double) (float) this.tweenArguments[(object) "z"];
      }
    }

    private void ApplyRectTargets()
    {
      ((Rect) ref this.rects[2]).set_x(this.ease(((Rect) ref this.rects[0]).get_x(), ((Rect) ref this.rects[1]).get_x(), this._percentage));
      ((Rect) ref this.rects[2]).set_y(this.ease(((Rect) ref this.rects[0]).get_y(), ((Rect) ref this.rects[1]).get_y(), this._percentage));
      ((Rect) ref this.rects[2]).set_width(this.ease(((Rect) ref this.rects[0]).get_width(), ((Rect) ref this.rects[1]).get_width(), this._percentage));
      ((Rect) ref this.rects[2]).set_height(this.ease(((Rect) ref this.rects[0]).get_height(), ((Rect) ref this.rects[1]).get_height(), this._percentage));
      this.tweenArguments[(object) "onupdateparams"] = (object) this.rects[2];
      if ((double) this._percentage != 1.0)
        return;
      this.tweenArguments[(object) "onupdateparams"] = (object) this.rects[1];
    }

    private void ApplyColorTargets()
    {
      this.colors[0, 2].r = (__Null) (double) this.ease((float) this.colors[0, 0].r, (float) this.colors[0, 1].r, this._percentage);
      this.colors[0, 2].g = (__Null) (double) this.ease((float) this.colors[0, 0].g, (float) this.colors[0, 1].g, this._percentage);
      this.colors[0, 2].b = (__Null) (double) this.ease((float) this.colors[0, 0].b, (float) this.colors[0, 1].b, this._percentage);
      this.colors[0, 2].a = (__Null) (double) this.ease((float) this.colors[0, 0].a, (float) this.colors[0, 1].a, this._percentage);
      this.tweenArguments[(object) "onupdateparams"] = (object) this.colors[0, 2];
      if ((double) this._percentage != 1.0)
        return;
      this.tweenArguments[(object) "onupdateparams"] = (object) this.colors[0, 1];
    }

    private void ApplyVector3Targets()
    {
      this.vector3s[2].x = (__Null) (double) this.ease((float) this.vector3s[0].x, (float) this.vector3s[1].x, this._percentage);
      this.vector3s[2].y = (__Null) (double) this.ease((float) this.vector3s[0].y, (float) this.vector3s[1].y, this._percentage);
      this.vector3s[2].z = (__Null) (double) this.ease((float) this.vector3s[0].z, (float) this.vector3s[1].z, this._percentage);
      this.tweenArguments[(object) "onupdateparams"] = (object) this.vector3s[2];
      if ((double) this._percentage != 1.0)
        return;
      this.tweenArguments[(object) "onupdateparams"] = (object) this.vector3s[1];
    }

    private void ApplyVector2Targets()
    {
      this.vector2s[2].x = (__Null) (double) this.ease((float) this.vector2s[0].x, (float) this.vector2s[1].x, this._percentage);
      this.vector2s[2].y = (__Null) (double) this.ease((float) this.vector2s[0].y, (float) this.vector2s[1].y, this._percentage);
      this.tweenArguments[(object) "onupdateparams"] = (object) this.vector2s[2];
      if ((double) this._percentage != 1.0)
        return;
      this.tweenArguments[(object) "onupdateparams"] = (object) this.vector2s[1];
    }

    private void ApplyFloatTargets()
    {
      this.floats[2] = this.ease(this.floats[0], this.floats[1], this._percentage);
      this.tweenArguments[(object) "onupdateparams"] = (object) this.floats[2];
      if ((double) this._percentage != 1.0)
        return;
      this.tweenArguments[(object) "onupdateparams"] = (object) this.floats[1];
    }

    private void ApplyColorToTargets()
    {
      for (int index = 0; index < this.colors.GetLength(0); ++index)
      {
        this.colors[index, 2].r = (__Null) (double) this.ease((float) this.colors[index, 0].r, (float) this.colors[index, 1].r, this._percentage);
        this.colors[index, 2].g = (__Null) (double) this.ease((float) this.colors[index, 0].g, (float) this.colors[index, 1].g, this._percentage);
        this.colors[index, 2].b = (__Null) (double) this.ease((float) this.colors[index, 0].b, (float) this.colors[index, 1].b, this._percentage);
        this.colors[index, 2].a = (__Null) (double) this.ease((float) this.colors[index, 0].a, (float) this.colors[index, 1].a, this._percentage);
      }
      if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (GUITexture))))
        ((GUITexture) ((Component) this).GetComponent<GUITexture>()).set_color(this.colors[0, 2]);
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (GUIText))))
        ((GUIText) ((Component) this).GetComponent<GUIText>()).get_material().set_color(this.colors[0, 2]);
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent<Renderer>()))
      {
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          ((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials()[index].SetColor(this.namedcolorvalue.ToString(), this.colors[index, 2]);
      }
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent<Light>()))
        ((Light) ((Component) this).GetComponent<Light>()).set_color(this.colors[0, 2]);
      if ((double) this._percentage != 1.0)
        return;
      if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (GUITexture))))
        ((GUITexture) ((Component) this).GetComponent<GUITexture>()).set_color(this.colors[0, 1]);
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent(typeof (GUIText))))
        ((GUIText) ((Component) this).GetComponent<GUIText>()).get_material().set_color(this.colors[0, 1]);
      else if (Object.op_Implicit((Object) ((Component) this).GetComponent<Renderer>()))
      {
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          ((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials()[index].SetColor(this.namedcolorvalue.ToString(), this.colors[index, 1]);
      }
      else
      {
        if (!Object.op_Implicit((Object) ((Component) this).GetComponent<Light>()))
          return;
        ((Light) ((Component) this).GetComponent<Light>()).set_color(this.colors[0, 1]);
      }
    }

    private void ApplyAudioToTargets()
    {
      this.vector2s[2].x = (__Null) (double) this.ease((float) this.vector2s[0].x, (float) this.vector2s[1].x, this._percentage);
      this.vector2s[2].y = (__Null) (double) this.ease((float) this.vector2s[0].y, (float) this.vector2s[1].y, this._percentage);
      this.audioSource.set_volume((float) this.vector2s[2].x);
      this.audioSource.set_pitch((float) this.vector2s[2].y);
      if ((double) this._percentage != 1.0)
        return;
      this.audioSource.set_volume((float) this.vector2s[1].x);
      this.audioSource.set_pitch((float) this.vector2s[1].y);
    }

    private void ApplyStabTargets()
    {
    }

    private void ApplyMoveToPathTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_position();
      float num = this.ease(0.0f, 1f, this._percentage);
      if (this.isLocal)
        ((Component) this).get_transform().set_localPosition(this.path.Interp(Mathf.Clamp(num, 0.0f, 1f)));
      else
        ((Component) this).get_transform().set_position(this.path.Interp(Mathf.Clamp(num, 0.0f, 1f)));
      if (this.tweenArguments.Contains((object) "orienttopath") && (bool) this.tweenArguments[(object) "orienttopath"])
        this.tweenArguments[(object) "looktarget"] = (object) this.path.Interp(Mathf.Clamp(this.ease(0.0f, 1f, Mathf.Min(1f, this._percentage + (!this.tweenArguments.Contains((object) "lookahead") ? Tween.Defaults.lookAhead : (float) this.tweenArguments[(object) "lookahead"]))), 0.0f, 1f));
      this.postUpdate = ((Component) this).get_transform().get_position();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_position(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MovePosition(this.postUpdate);
    }

    private void ApplyMoveToTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_position();
      this.vector3s[2].x = (__Null) (double) this.ease((float) this.vector3s[0].x, (float) this.vector3s[1].x, this._percentage);
      this.vector3s[2].y = (__Null) (double) this.ease((float) this.vector3s[0].y, (float) this.vector3s[1].y, this._percentage);
      this.vector3s[2].z = (__Null) (double) this.ease((float) this.vector3s[0].z, (float) this.vector3s[1].z, this._percentage);
      if (this.isLocal)
        ((Component) this).get_transform().set_localPosition(this.vector3s[2]);
      else
        ((Component) this).get_transform().set_position(this.vector3s[2]);
      if ((double) this._percentage == 1.0)
      {
        if (this.isLocal)
          ((Component) this).get_transform().set_localPosition(this.vector3s[1]);
        else
          ((Component) this).get_transform().set_position(this.vector3s[1]);
      }
      this.postUpdate = ((Component) this).get_transform().get_position();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_position(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MovePosition(this.postUpdate);
    }

    private void ApplyMoveByTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_position();
      Vector3 vector3 = (Vector3) null;
      if (this.tweenArguments.Contains((object) "looktarget"))
      {
        vector3 = ((Component) this).get_transform().get_eulerAngles();
        ((Component) this).get_transform().set_eulerAngles(this.vector3s[4]);
      }
      this.vector3s[2].x = (__Null) (double) this.ease((float) this.vector3s[0].x, (float) this.vector3s[1].x, this._percentage);
      this.vector3s[2].y = (__Null) (double) this.ease((float) this.vector3s[0].y, (float) this.vector3s[1].y, this._percentage);
      this.vector3s[2].z = (__Null) (double) this.ease((float) this.vector3s[0].z, (float) this.vector3s[1].z, this._percentage);
      ((Component) this).get_transform().Translate(Vector3.op_Subtraction(this.vector3s[2], this.vector3s[3]), this.space);
      this.vector3s[3] = this.vector3s[2];
      if (this.tweenArguments.Contains((object) "looktarget"))
        ((Component) this).get_transform().set_eulerAngles(vector3);
      this.postUpdate = ((Component) this).get_transform().get_position();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_position(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MovePosition(this.postUpdate);
    }

    private void ApplyScaleToTargets()
    {
      this.vector3s[2].x = (__Null) (double) this.ease((float) this.vector3s[0].x, (float) this.vector3s[1].x, this._percentage);
      this.vector3s[2].y = (__Null) (double) this.ease((float) this.vector3s[0].y, (float) this.vector3s[1].y, this._percentage);
      this.vector3s[2].z = (__Null) (double) this.ease((float) this.vector3s[0].z, (float) this.vector3s[1].z, this._percentage);
      ((Component) this).get_transform().set_localScale(this.vector3s[2]);
      if ((double) this._percentage != 1.0)
        return;
      ((Component) this).get_transform().set_localScale(this.vector3s[1]);
    }

    private void ApplyLookToTargets()
    {
      this.vector3s[2].x = (__Null) (double) this.ease((float) this.vector3s[0].x, (float) this.vector3s[1].x, this._percentage);
      this.vector3s[2].y = (__Null) (double) this.ease((float) this.vector3s[0].y, (float) this.vector3s[1].y, this._percentage);
      this.vector3s[2].z = (__Null) (double) this.ease((float) this.vector3s[0].z, (float) this.vector3s[1].z, this._percentage);
      if (this.isLocal)
        ((Component) this).get_transform().set_localRotation(Quaternion.Euler(this.vector3s[2]));
      else
        ((Component) this).get_transform().set_rotation(Quaternion.Euler(this.vector3s[2]));
    }

    private void ApplyRotateToTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_eulerAngles();
      this.vector3s[2].x = (__Null) (double) this.ease((float) this.vector3s[0].x, (float) this.vector3s[1].x, this._percentage);
      this.vector3s[2].y = (__Null) (double) this.ease((float) this.vector3s[0].y, (float) this.vector3s[1].y, this._percentage);
      this.vector3s[2].z = (__Null) (double) this.ease((float) this.vector3s[0].z, (float) this.vector3s[1].z, this._percentage);
      if (this.isLocal)
        ((Component) this).get_transform().set_localRotation(Quaternion.Euler(this.vector3s[2]));
      else
        ((Component) this).get_transform().set_rotation(Quaternion.Euler(this.vector3s[2]));
      if ((double) this._percentage == 1.0)
      {
        if (this.isLocal)
          ((Component) this).get_transform().set_localRotation(Quaternion.Euler(this.vector3s[1]));
        else
          ((Component) this).get_transform().set_rotation(Quaternion.Euler(this.vector3s[1]));
      }
      this.postUpdate = ((Component) this).get_transform().get_eulerAngles();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_eulerAngles(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyRotateAddTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_eulerAngles();
      this.vector3s[2].x = (__Null) (double) this.ease((float) this.vector3s[0].x, (float) this.vector3s[1].x, this._percentage);
      this.vector3s[2].y = (__Null) (double) this.ease((float) this.vector3s[0].y, (float) this.vector3s[1].y, this._percentage);
      this.vector3s[2].z = (__Null) (double) this.ease((float) this.vector3s[0].z, (float) this.vector3s[1].z, this._percentage);
      ((Component) this).get_transform().Rotate(Vector3.op_Subtraction(this.vector3s[2], this.vector3s[3]), this.space);
      this.vector3s[3] = this.vector3s[2];
      this.postUpdate = ((Component) this).get_transform().get_eulerAngles();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_eulerAngles(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyShakePositionTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_position();
      Vector3 vector3 = (Vector3) null;
      if (this.tweenArguments.Contains((object) "looktarget"))
      {
        vector3 = ((Component) this).get_transform().get_eulerAngles();
        ((Component) this).get_transform().set_eulerAngles(this.vector3s[3]);
      }
      if ((double) this._percentage == 0.0)
        ((Component) this).get_transform().Translate(this.vector3s[1], this.space);
      ((Component) this).get_transform().set_position(this.vector3s[0]);
      float num = 1f - this._percentage;
      this.vector3s[2].x = (__Null) (double) Random.Range((float) -this.vector3s[1].x * num, (float) this.vector3s[1].x * num);
      this.vector3s[2].y = (__Null) (double) Random.Range((float) -this.vector3s[1].y * num, (float) this.vector3s[1].y * num);
      this.vector3s[2].z = (__Null) (double) Random.Range((float) -this.vector3s[1].z * num, (float) this.vector3s[1].z * num);
      ((Component) this).get_transform().Translate(this.vector3s[2], this.space);
      if (this.tweenArguments.Contains((object) "looktarget"))
        ((Component) this).get_transform().set_eulerAngles(vector3);
      this.postUpdate = ((Component) this).get_transform().get_position();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_position(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MovePosition(this.postUpdate);
    }

    private void ApplyShakeScaleTargets()
    {
      if ((double) this._percentage == 0.0)
        ((Component) this).get_transform().set_localScale(this.vector3s[1]);
      ((Component) this).get_transform().set_localScale(this.vector3s[0]);
      float num = 1f - this._percentage;
      this.vector3s[2].x = (__Null) (double) Random.Range((float) -this.vector3s[1].x * num, (float) this.vector3s[1].x * num);
      this.vector3s[2].y = (__Null) (double) Random.Range((float) -this.vector3s[1].y * num, (float) this.vector3s[1].y * num);
      this.vector3s[2].z = (__Null) (double) Random.Range((float) -this.vector3s[1].z * num, (float) this.vector3s[1].z * num);
      Transform transform = ((Component) this).get_transform();
      transform.set_localScale(Vector3.op_Addition(transform.get_localScale(), this.vector3s[2]));
    }

    private void ApplyShakeRotationTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_eulerAngles();
      if ((double) this._percentage == 0.0)
        ((Component) this).get_transform().Rotate(this.vector3s[1], this.space);
      ((Component) this).get_transform().set_eulerAngles(this.vector3s[0]);
      float num = 1f - this._percentage;
      this.vector3s[2].x = (__Null) (double) Random.Range((float) -this.vector3s[1].x * num, (float) this.vector3s[1].x * num);
      this.vector3s[2].y = (__Null) (double) Random.Range((float) -this.vector3s[1].y * num, (float) this.vector3s[1].y * num);
      this.vector3s[2].z = (__Null) (double) Random.Range((float) -this.vector3s[1].z * num, (float) this.vector3s[1].z * num);
      ((Component) this).get_transform().Rotate(this.vector3s[2], this.space);
      this.postUpdate = ((Component) this).get_transform().get_eulerAngles();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_eulerAngles(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyPunchPositionTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_position();
      Vector3 vector3 = (Vector3) null;
      if (this.tweenArguments.Contains((object) "looktarget"))
      {
        vector3 = ((Component) this).get_transform().get_eulerAngles();
        ((Component) this).get_transform().set_eulerAngles(this.vector3s[4]);
      }
      if (this.vector3s[1].x > 0.0)
        this.vector3s[2].x = (__Null) (double) this.punch((float) this.vector3s[1].x, this._percentage);
      else if (this.vector3s[1].x < 0.0)
        this.vector3s[2].x = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].x), this._percentage);
      if (this.vector3s[1].y > 0.0)
        this.vector3s[2].y = (__Null) (double) this.punch((float) this.vector3s[1].y, this._percentage);
      else if (this.vector3s[1].y < 0.0)
        this.vector3s[2].y = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].y), this._percentage);
      if (this.vector3s[1].z > 0.0)
        this.vector3s[2].z = (__Null) (double) this.punch((float) this.vector3s[1].z, this._percentage);
      else if (this.vector3s[1].z < 0.0)
        this.vector3s[2].z = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].z), this._percentage);
      ((Component) this).get_transform().Translate(Vector3.op_Subtraction(this.vector3s[2], this.vector3s[3]), this.space);
      this.vector3s[3] = this.vector3s[2];
      if (this.tweenArguments.Contains((object) "looktarget"))
        ((Component) this).get_transform().set_eulerAngles(vector3);
      this.postUpdate = ((Component) this).get_transform().get_position();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_position(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MovePosition(this.postUpdate);
    }

    private void ApplyPunchRotationTargets()
    {
      this.preUpdate = ((Component) this).get_transform().get_eulerAngles();
      if (this.vector3s[1].x > 0.0)
        this.vector3s[2].x = (__Null) (double) this.punch((float) this.vector3s[1].x, this._percentage);
      else if (this.vector3s[1].x < 0.0)
        this.vector3s[2].x = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].x), this._percentage);
      if (this.vector3s[1].y > 0.0)
        this.vector3s[2].y = (__Null) (double) this.punch((float) this.vector3s[1].y, this._percentage);
      else if (this.vector3s[1].y < 0.0)
        this.vector3s[2].y = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].y), this._percentage);
      if (this.vector3s[1].z > 0.0)
        this.vector3s[2].z = (__Null) (double) this.punch((float) this.vector3s[1].z, this._percentage);
      else if (this.vector3s[1].z < 0.0)
        this.vector3s[2].z = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].z), this._percentage);
      ((Component) this).get_transform().Rotate(Vector3.op_Subtraction(this.vector3s[2], this.vector3s[3]), this.space);
      this.vector3s[3] = this.vector3s[2];
      this.postUpdate = ((Component) this).get_transform().get_eulerAngles();
      if (!this.physics)
        return;
      ((Component) this).get_transform().set_eulerAngles(this.preUpdate);
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyPunchScaleTargets()
    {
      if (this.vector3s[1].x > 0.0)
        this.vector3s[2].x = (__Null) (double) this.punch((float) this.vector3s[1].x, this._percentage);
      else if (this.vector3s[1].x < 0.0)
        this.vector3s[2].x = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].x), this._percentage);
      if (this.vector3s[1].y > 0.0)
        this.vector3s[2].y = (__Null) (double) this.punch((float) this.vector3s[1].y, this._percentage);
      else if (this.vector3s[1].y < 0.0)
        this.vector3s[2].y = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].y), this._percentage);
      if (this.vector3s[1].z > 0.0)
        this.vector3s[2].z = (__Null) (double) this.punch((float) this.vector3s[1].z, this._percentage);
      else if (this.vector3s[1].z < 0.0)
        this.vector3s[2].z = (__Null) -(double) this.punch(Mathf.Abs((float) this.vector3s[1].z), this._percentage);
      ((Component) this).get_transform().set_localScale(Vector3.op_Addition(this.vector3s[0], this.vector3s[2]));
    }

    [DebuggerHidden]
    private IEnumerator TweenDelay()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Tween.\u003CTweenDelay\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void TweenStart()
    {
      this.CallBack("onstart");
      if (!this.loop)
      {
        this.ConflictCheck();
        this.GenerateTargets();
      }
      if (this.type == "stab")
        this.audioSource.PlayOneShot(this.audioSource.get_clip());
      if (this.type == "move" || this.type == "scale" || (this.type == "rotate" || this.type == "punch") || (this.type == "shake" || this.type == "curve" || this.type == "look"))
        this.EnableKinematic();
      this.isRunning = true;
    }

    [DebuggerHidden]
    private IEnumerator TweenRestart()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Tween.\u003CTweenRestart\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void TweenUpdate()
    {
      this.apply();
      this.CallBack("onupdate");
      if (this.onUpdate != null)
        this.onUpdate(this._percentage);
      this.UpdatePercentage();
    }

    private void TweenComplete()
    {
      this.isRunning = false;
      this._percentage = (double) this._percentage <= 0.5 ? 0.0f : 1f;
      this.apply();
      if (this.type == "value")
      {
        this.CallBack("onupdate");
        if (this.onUpdate != null)
          this.onUpdate(this._percentage);
      }
      if (this.loopType == Tween.LoopType.none)
        this.Dispose();
      else
        this.TweenLoop();
      this.CallBack("oncomplete");
      if (this.onComplete == null)
        return;
      int num = this.onComplete() ? 1 : 0;
    }

    private void TweenLoop()
    {
      this.DisableKinematic();
      switch (this.loopType)
      {
        case Tween.LoopType.loop:
          this._percentage = 0.0f;
          this.runningTime = 0.0f;
          this.apply();
          this.StartCoroutine("TweenRestart");
          break;
        case Tween.LoopType.pingPong:
          this.reverse = !this.reverse;
          this.runningTime = 0.0f;
          this.StartCoroutine("TweenRestart");
          break;
      }
    }

    public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
    {
      Rect rect;
      ((Rect) ref rect).\u002Ector(Tween.FloatUpdate(((Rect) ref currentValue).get_x(), ((Rect) ref targetValue).get_x(), speed), Tween.FloatUpdate(((Rect) ref currentValue).get_y(), ((Rect) ref targetValue).get_y(), speed), Tween.FloatUpdate(((Rect) ref currentValue).get_width(), ((Rect) ref targetValue).get_width(), speed), Tween.FloatUpdate(((Rect) ref currentValue).get_height(), ((Rect) ref targetValue).get_height(), speed));
      return rect;
    }

    public static Vector3 Vector3Update(
      Vector3 currentValue,
      Vector3 targetValue,
      float speed)
    {
      Vector3 vector3 = Vector3.op_Subtraction(targetValue, currentValue);
      currentValue = Vector3.op_Addition(currentValue, Vector3.op_Multiply(Vector3.op_Multiply(vector3, speed), Time.get_deltaTime()));
      return currentValue;
    }

    public static Vector2 Vector2Update(
      Vector2 currentValue,
      Vector2 targetValue,
      float speed)
    {
      Vector2 vector2 = Vector2.op_Subtraction(targetValue, currentValue);
      currentValue = Vector2.op_Addition(currentValue, Vector2.op_Multiply(Vector2.op_Multiply(vector2, speed), Time.get_deltaTime()));
      return currentValue;
    }

    public static float FloatUpdate(float currentValue, float targetValue, float speed)
    {
      float num = targetValue - currentValue;
      currentValue += num * speed * Time.get_deltaTime();
      return currentValue;
    }

    public static void FadeUpdate(GameObject target, Hashtable args)
    {
      args[(object) "a"] = args[(object) "alpha"];
      Tween.ColorUpdate(target, args);
    }

    public static void FadeUpdate(GameObject target, float alpha, float time)
    {
      Tween.FadeUpdate(target, Tween.Hash((object) nameof (alpha), (object) alpha, (object) nameof (time), (object) time));
    }

    public static void ColorUpdate(GameObject target, Hashtable args)
    {
      Tween.CleanArgs(args);
      Color[] colorArray = new Color[4];
      if (!args.Contains((object) "includechildren") || (bool) args[(object) "includechildren"])
      {
        IEnumerator enumerator = target.get_transform().GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
            Tween.ColorUpdate(((Component) enumerator.Current).get_gameObject(), args);
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      float num = !args.Contains((object) "time") ? Tween.Defaults.updateTime : (float) args[(object) "time"] * Tween.Defaults.updateTimePercentage;
      if (Object.op_Implicit((Object) target.GetComponent(typeof (GUITexture))))
        colorArray[0] = colorArray[1] = ((GUITexture) target.GetComponent<GUITexture>()).get_color();
      else if (Object.op_Implicit((Object) target.GetComponent(typeof (GUIText))))
        colorArray[0] = colorArray[1] = ((GUIText) target.GetComponent<GUIText>()).get_material().get_color();
      else if (Object.op_Implicit((Object) target.GetComponent<Renderer>()))
        colorArray[0] = colorArray[1] = ((Renderer) target.GetComponent<Renderer>()).get_material().get_color();
      else if (Object.op_Implicit((Object) target.GetComponent<Light>()))
        colorArray[0] = colorArray[1] = ((Light) target.GetComponent<Light>()).get_color();
      if (args.Contains((object) "color"))
      {
        colorArray[1] = (Color) args[(object) "color"];
      }
      else
      {
        if (args.Contains((object) "r"))
          colorArray[1].r = (__Null) (double) (float) args[(object) "r"];
        if (args.Contains((object) "g"))
          colorArray[1].g = (__Null) (double) (float) args[(object) "g"];
        if (args.Contains((object) "b"))
          colorArray[1].b = (__Null) (double) (float) args[(object) "b"];
        if (args.Contains((object) "a"))
          colorArray[1].a = (__Null) (double) (float) args[(object) "a"];
      }
      // ISSUE: cast to a reference type
      colorArray[3].r = (__Null) (double) Mathf.SmoothDamp((float) colorArray[0].r, (float) colorArray[1].r, (float&) ref colorArray[2].r, num);
      // ISSUE: cast to a reference type
      colorArray[3].g = (__Null) (double) Mathf.SmoothDamp((float) colorArray[0].g, (float) colorArray[1].g, (float&) ref colorArray[2].g, num);
      // ISSUE: cast to a reference type
      colorArray[3].b = (__Null) (double) Mathf.SmoothDamp((float) colorArray[0].b, (float) colorArray[1].b, (float&) ref colorArray[2].b, num);
      // ISSUE: cast to a reference type
      colorArray[3].a = (__Null) (double) Mathf.SmoothDamp((float) colorArray[0].a, (float) colorArray[1].a, (float&) ref colorArray[2].a, num);
      if (Object.op_Implicit((Object) target.GetComponent(typeof (GUITexture))))
        ((GUITexture) target.GetComponent<GUITexture>()).set_color(colorArray[3]);
      else if (Object.op_Implicit((Object) target.GetComponent(typeof (GUIText))))
        ((GUIText) target.GetComponent<GUIText>()).get_material().set_color(colorArray[3]);
      else if (Object.op_Implicit((Object) target.GetComponent<Renderer>()))
      {
        ((Renderer) target.GetComponent<Renderer>()).get_material().set_color(colorArray[3]);
      }
      else
      {
        if (!Object.op_Implicit((Object) target.GetComponent<Light>()))
          return;
        ((Light) target.GetComponent<Light>()).set_color(colorArray[3]);
      }
    }

    public static void ColorUpdate(GameObject target, Color color, float time)
    {
      Tween.ColorUpdate(target, Tween.Hash((object) nameof (color), (object) color, (object) nameof (time), (object) time));
    }

    public static void AudioUpdate(GameObject target, Hashtable args)
    {
      Tween.CleanArgs(args);
      Vector2[] vector2Array = new Vector2[4];
      float num = !args.Contains((object) "time") ? Tween.Defaults.updateTime : (float) args[(object) "time"] * Tween.Defaults.updateTimePercentage;
      AudioSource component;
      if (args.Contains((object) "audiosource"))
        component = (AudioSource) args[(object) "audiosource"];
      else if (Object.op_Implicit((Object) target.GetComponent(typeof (AudioSource))))
      {
        component = (AudioSource) target.GetComponent<AudioSource>();
      }
      else
      {
        Debug.LogError((object) "iTween Error: AudioUpdate requires an AudioSource.");
        return;
      }
      vector2Array[0] = vector2Array[1] = new Vector2(component.get_volume(), component.get_pitch());
      if (args.Contains((object) "volume"))
        vector2Array[1].x = (__Null) (double) (float) args[(object) "volume"];
      if (args.Contains((object) "pitch"))
        vector2Array[1].y = (__Null) (double) (float) args[(object) "pitch"];
      // ISSUE: cast to a reference type
      vector2Array[3].x = (__Null) (double) Mathf.SmoothDampAngle((float) vector2Array[0].x, (float) vector2Array[1].x, (float&) ref vector2Array[2].x, num);
      // ISSUE: cast to a reference type
      vector2Array[3].y = (__Null) (double) Mathf.SmoothDampAngle((float) vector2Array[0].y, (float) vector2Array[1].y, (float&) ref vector2Array[2].y, num);
      component.set_volume((float) vector2Array[3].x);
      component.set_pitch((float) vector2Array[3].y);
    }

    public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
    {
      Tween.AudioUpdate(target, Tween.Hash((object) nameof (volume), (object) volume, (object) nameof (pitch), (object) pitch, (object) nameof (time), (object) time));
    }

    public static void RotateUpdate(GameObject target, Hashtable args)
    {
      Tween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[4];
      Vector3 eulerAngles1 = target.get_transform().get_eulerAngles();
      float num = !args.Contains((object) "time") ? Tween.Defaults.updateTime : (float) args[(object) "time"] * Tween.Defaults.updateTimePercentage;
      bool flag = !args.Contains((object) "islocal") ? Tween.Defaults.isLocal : (bool) args[(object) "islocal"];
      vector3Array[0] = !flag ? target.get_transform().get_eulerAngles() : target.get_transform().get_localEulerAngles();
      if (args.Contains((object) "rotation"))
      {
        if (args[(object) "rotation"].GetType() == typeof (Transform))
        {
          Transform transform = (Transform) args[(object) "rotation"];
          vector3Array[1] = transform.get_eulerAngles();
        }
        else if (args[(object) "rotation"].GetType() == typeof (Vector3))
          vector3Array[1] = (Vector3) args[(object) "rotation"];
      }
      // ISSUE: cast to a reference type
      vector3Array[3].x = (__Null) (double) Mathf.SmoothDampAngle((float) vector3Array[0].x, (float) vector3Array[1].x, (float&) ref vector3Array[2].x, num);
      // ISSUE: cast to a reference type
      vector3Array[3].y = (__Null) (double) Mathf.SmoothDampAngle((float) vector3Array[0].y, (float) vector3Array[1].y, (float&) ref vector3Array[2].y, num);
      // ISSUE: cast to a reference type
      vector3Array[3].z = (__Null) (double) Mathf.SmoothDampAngle((float) vector3Array[0].z, (float) vector3Array[1].z, (float&) ref vector3Array[2].z, num);
      if (flag)
        target.get_transform().set_localEulerAngles(vector3Array[3]);
      else
        target.get_transform().set_eulerAngles(vector3Array[3]);
      if (!Object.op_Inequality((Object) target.GetComponent<Rigidbody>(), (Object) null))
        return;
      Vector3 eulerAngles2 = target.get_transform().get_eulerAngles();
      target.get_transform().set_eulerAngles(eulerAngles1);
      ((Rigidbody) target.GetComponent<Rigidbody>()).MoveRotation(Quaternion.Euler(eulerAngles2));
    }

    public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
    {
      Tween.RotateUpdate(target, Tween.Hash((object) nameof (rotation), (object) rotation, (object) nameof (time), (object) time));
    }

    public static void ScaleUpdate(GameObject target, Hashtable args)
    {
      Tween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[4];
      float num = !args.Contains((object) "time") ? Tween.Defaults.updateTime : (float) args[(object) "time"] * Tween.Defaults.updateTimePercentage;
      vector3Array[0] = vector3Array[1] = target.get_transform().get_localScale();
      if (args.Contains((object) "scale"))
      {
        if (args[(object) "scale"].GetType() == typeof (Transform))
        {
          Transform transform = (Transform) args[(object) "scale"];
          vector3Array[1] = transform.get_localScale();
        }
        else if (args[(object) "scale"].GetType() == typeof (Vector3))
          vector3Array[1] = (Vector3) args[(object) "scale"];
      }
      else
      {
        if (args.Contains((object) "x"))
          vector3Array[1].x = (__Null) (double) (float) args[(object) "x"];
        if (args.Contains((object) "y"))
          vector3Array[1].y = (__Null) (double) (float) args[(object) "y"];
        if (args.Contains((object) "z"))
          vector3Array[1].z = (__Null) (double) (float) args[(object) "z"];
      }
      // ISSUE: cast to a reference type
      vector3Array[3].x = (__Null) (double) Mathf.SmoothDamp((float) vector3Array[0].x, (float) vector3Array[1].x, (float&) ref vector3Array[2].x, num);
      // ISSUE: cast to a reference type
      vector3Array[3].y = (__Null) (double) Mathf.SmoothDamp((float) vector3Array[0].y, (float) vector3Array[1].y, (float&) ref vector3Array[2].y, num);
      // ISSUE: cast to a reference type
      vector3Array[3].z = (__Null) (double) Mathf.SmoothDamp((float) vector3Array[0].z, (float) vector3Array[1].z, (float&) ref vector3Array[2].z, num);
      target.get_transform().set_localScale(vector3Array[3]);
    }

    public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
    {
      Tween.ScaleUpdate(target, Tween.Hash((object) nameof (scale), (object) scale, (object) nameof (time), (object) time));
    }

    public static void MoveUpdate(GameObject target, Hashtable args)
    {
      Tween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[4];
      Vector3 position1 = target.get_transform().get_position();
      float num = !args.Contains((object) "time") ? Tween.Defaults.updateTime : (float) args[(object) "time"] * Tween.Defaults.updateTimePercentage;
      bool flag = !args.Contains((object) "islocal") ? Tween.Defaults.isLocal : (bool) args[(object) "islocal"];
      vector3Array[0] = !flag ? (vector3Array[1] = target.get_transform().get_position()) : (vector3Array[1] = target.get_transform().get_localPosition());
      if (args.Contains((object) "position"))
      {
        if (args[(object) "position"].GetType() == typeof (Transform))
        {
          Transform transform = (Transform) args[(object) "position"];
          vector3Array[1] = transform.get_position();
        }
        else if (args[(object) "position"].GetType() == typeof (Vector3))
          vector3Array[1] = (Vector3) args[(object) "position"];
      }
      else
      {
        if (args.Contains((object) "x"))
          vector3Array[1].x = (__Null) (double) (float) args[(object) "x"];
        if (args.Contains((object) "y"))
          vector3Array[1].y = (__Null) (double) (float) args[(object) "y"];
        if (args.Contains((object) "z"))
          vector3Array[1].z = (__Null) (double) (float) args[(object) "z"];
      }
      // ISSUE: cast to a reference type
      vector3Array[3].x = (__Null) (double) Mathf.SmoothDamp((float) vector3Array[0].x, (float) vector3Array[1].x, (float&) ref vector3Array[2].x, num);
      // ISSUE: cast to a reference type
      vector3Array[3].y = (__Null) (double) Mathf.SmoothDamp((float) vector3Array[0].y, (float) vector3Array[1].y, (float&) ref vector3Array[2].y, num);
      // ISSUE: cast to a reference type
      vector3Array[3].z = (__Null) (double) Mathf.SmoothDamp((float) vector3Array[0].z, (float) vector3Array[1].z, (float&) ref vector3Array[2].z, num);
      if (args.Contains((object) "orienttopath") && (bool) args[(object) "orienttopath"])
        args[(object) "looktarget"] = (object) vector3Array[3];
      if (args.Contains((object) "looktarget"))
        Tween.LookUpdate(target, args);
      if (flag)
        target.get_transform().set_localPosition(vector3Array[3]);
      else
        target.get_transform().set_position(vector3Array[3]);
      if (!Object.op_Inequality((Object) target.GetComponent<Rigidbody>(), (Object) null))
        return;
      Vector3 position2 = target.get_transform().get_position();
      target.get_transform().set_position(position1);
      ((Rigidbody) target.GetComponent<Rigidbody>()).MovePosition(position2);
    }

    public static void MoveUpdate(GameObject target, Vector3 position, float time)
    {
      Tween.MoveUpdate(target, Tween.Hash((object) nameof (position), (object) position, (object) nameof (time), (object) time));
    }

    public static void LookUpdate(GameObject target, Hashtable args)
    {
      Tween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[5];
      float num = !args.Contains((object) "looktime") ? (!args.Contains((object) "time") ? Tween.Defaults.updateTime : (float) args[(object) "time"] * 0.15f * Tween.Defaults.updateTimePercentage) : (float) args[(object) "looktime"] * Tween.Defaults.updateTimePercentage;
      vector3Array[0] = target.get_transform().get_eulerAngles();
      if (args.Contains((object) "looktarget"))
      {
        if (args[(object) "looktarget"].GetType() == typeof (Transform))
        {
          Transform transform1 = target.get_transform();
          Transform transform2 = (Transform) args[(object) "looktarget"];
          Vector3? nullable = (Vector3?) args[(object) "up"];
          Vector3 vector3 = !nullable.HasValue ? Tween.Defaults.up : nullable.Value;
          transform1.LookAt(transform2, vector3);
        }
        else if (args[(object) "looktarget"].GetType() == typeof (Vector3))
        {
          Transform transform = target.get_transform();
          Vector3 vector3_1 = (Vector3) args[(object) "looktarget"];
          Vector3? nullable = (Vector3?) args[(object) "up"];
          Vector3 vector3_2 = !nullable.HasValue ? Tween.Defaults.up : nullable.Value;
          transform.LookAt(vector3_1, vector3_2);
        }
        vector3Array[1] = target.get_transform().get_eulerAngles();
        target.get_transform().set_eulerAngles(vector3Array[0]);
        // ISSUE: cast to a reference type
        vector3Array[3].x = (__Null) (double) Mathf.SmoothDampAngle((float) vector3Array[0].x, (float) vector3Array[1].x, (float&) ref vector3Array[2].x, num);
        // ISSUE: cast to a reference type
        vector3Array[3].y = (__Null) (double) Mathf.SmoothDampAngle((float) vector3Array[0].y, (float) vector3Array[1].y, (float&) ref vector3Array[2].y, num);
        // ISSUE: cast to a reference type
        vector3Array[3].z = (__Null) (double) Mathf.SmoothDampAngle((float) vector3Array[0].z, (float) vector3Array[1].z, (float&) ref vector3Array[2].z, num);
        target.get_transform().set_eulerAngles(vector3Array[3]);
        if (!args.Contains((object) "axis"))
          return;
        vector3Array[4] = target.get_transform().get_eulerAngles();
        switch ((string) args[(object) "axis"])
        {
          case "x":
            vector3Array[4].y = vector3Array[0].y;
            vector3Array[4].z = vector3Array[0].z;
            break;
          case "y":
            vector3Array[4].x = vector3Array[0].x;
            vector3Array[4].z = vector3Array[0].z;
            break;
          case "z":
            vector3Array[4].x = vector3Array[0].x;
            vector3Array[4].y = vector3Array[0].y;
            break;
        }
        target.get_transform().set_eulerAngles(vector3Array[4]);
      }
      else
        Debug.LogError((object) "iTween Error: LookUpdate needs a 'looktarget' property!");
    }

    public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
    {
      Tween.LookUpdate(target, Tween.Hash((object) nameof (looktarget), (object) looktarget, (object) nameof (time), (object) time));
    }

    public static float PathLength(Transform[] path)
    {
      Vector3[] path1 = new Vector3[path.Length];
      float num1 = 0.0f;
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      Vector3[] pts = Tween.PathControlPointGenerator(path1);
      Vector3 vector3_1 = Tween.Interp(pts, 0.0f);
      int num2 = path.Length * 20;
      for (int index = 1; index <= num2; ++index)
      {
        float t = (float) index / (float) num2;
        Vector3 vector3_2 = Tween.Interp(pts, t);
        num1 += Vector3.Distance(vector3_1, vector3_2);
        vector3_1 = vector3_2;
      }
      return num1;
    }

    public static float PathLength(Vector3[] path)
    {
      float num1 = 0.0f;
      Vector3[] pts = Tween.PathControlPointGenerator(path);
      Vector3 vector3_1 = Tween.Interp(pts, 0.0f);
      int num2 = path.Length * 20;
      for (int index = 1; index <= num2; ++index)
      {
        float t = (float) index / (float) num2;
        Vector3 vector3_2 = Tween.Interp(pts, t);
        num1 += Vector3.Distance(vector3_1, vector3_2);
        vector3_1 = vector3_2;
      }
      return num1;
    }

    public static Texture2D CameraTexture(Color color)
    {
      Texture2D texture2D = new Texture2D(Screen.get_width(), Screen.get_height(), (TextureFormat) 5, false);
      Color[] colorArray = new Color[Screen.get_width() * Screen.get_height()];
      for (int index = 0; index < colorArray.Length; ++index)
        colorArray[index] = color;
      texture2D.SetPixels(colorArray);
      texture2D.Apply();
      return texture2D;
    }

    public static void PutOnPath(GameObject target, Vector3[] path, float percent)
    {
      target.get_transform().set_position(Tween.Interp(Tween.PathControlPointGenerator(path), percent));
    }

    public static void PutOnPath(Transform target, Vector3[] path, float percent)
    {
      target.set_position(Tween.Interp(Tween.PathControlPointGenerator(path), percent));
    }

    public static void PutOnPath(GameObject target, Transform[] path, float percent)
    {
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      target.get_transform().set_position(Tween.Interp(Tween.PathControlPointGenerator(path1), percent));
    }

    public static void PutOnPath(Transform target, Transform[] path, float percent)
    {
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      target.set_position(Tween.Interp(Tween.PathControlPointGenerator(path1), percent));
    }

    public static Vector3 PointOnPath(Transform[] path, float percent)
    {
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      return Tween.Interp(Tween.PathControlPointGenerator(path1), percent);
    }

    public static void DrawLine(Vector3[] line)
    {
      if (line.Length <= 0)
        return;
      Tween.DrawLineHelper(line, Tween.Defaults.color, "gizmos");
    }

    public static void DrawLine(Vector3[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Tween.DrawLineHelper(line, color, "gizmos");
    }

    public static void DrawLine(Transform[] line)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].get_position();
      Tween.DrawLineHelper(line1, Tween.Defaults.color, "gizmos");
    }

    public static void DrawLine(Transform[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].get_position();
      Tween.DrawLineHelper(line1, color, "gizmos");
    }

    public static void DrawLineGizmos(Vector3[] line)
    {
      if (line.Length <= 0)
        return;
      Tween.DrawLineHelper(line, Tween.Defaults.color, "gizmos");
    }

    public static void DrawLineGizmos(Vector3[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Tween.DrawLineHelper(line, color, "gizmos");
    }

    public static void DrawLineGizmos(Transform[] line)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].get_position();
      Tween.DrawLineHelper(line1, Tween.Defaults.color, "gizmos");
    }

    public static void DrawLineGizmos(Transform[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].get_position();
      Tween.DrawLineHelper(line1, color, "gizmos");
    }

    public static void DrawLineHandles(Vector3[] line)
    {
      if (line.Length <= 0)
        return;
      Tween.DrawLineHelper(line, Tween.Defaults.color, "handles");
    }

    public static void DrawLineHandles(Vector3[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Tween.DrawLineHelper(line, color, "handles");
    }

    public static void DrawLineHandles(Transform[] line)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].get_position();
      Tween.DrawLineHelper(line1, Tween.Defaults.color, "handles");
    }

    public static void DrawLineHandles(Transform[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].get_position();
      Tween.DrawLineHelper(line1, color, "handles");
    }

    public static Vector3 PointOnPath(Vector3[] path, float percent)
    {
      return Tween.Interp(Tween.PathControlPointGenerator(path), percent);
    }

    public static void DrawPath(Vector3[] path)
    {
      if (path.Length <= 0)
        return;
      Tween.DrawPathHelper(path, Tween.Defaults.color, "gizmos");
    }

    public static void DrawPath(Vector3[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Tween.DrawPathHelper(path, color, "gizmos");
    }

    public static void DrawPath(Transform[] path)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      Tween.DrawPathHelper(path1, Tween.Defaults.color, "gizmos");
    }

    public static void DrawPath(Transform[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      Tween.DrawPathHelper(path1, color, "gizmos");
    }

    public static void DrawPathGizmos(Vector3[] path)
    {
      if (path.Length <= 0)
        return;
      Tween.DrawPathHelper(path, Tween.Defaults.color, "gizmos");
    }

    public static void DrawPathGizmos(Vector3[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Tween.DrawPathHelper(path, color, "gizmos");
    }

    public static void DrawPathGizmos(Transform[] path)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      Tween.DrawPathHelper(path1, Tween.Defaults.color, "gizmos");
    }

    public static void DrawPathGizmos(Transform[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      Tween.DrawPathHelper(path1, color, "gizmos");
    }

    public static void DrawPathHandles(Vector3[] path)
    {
      if (path.Length <= 0)
        return;
      Tween.DrawPathHelper(path, Tween.Defaults.color, "handles");
    }

    public static void DrawPathHandles(Vector3[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Tween.DrawPathHelper(path, color, "handles");
    }

    public static void DrawPathHandles(Transform[] path)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      Tween.DrawPathHelper(path1, Tween.Defaults.color, "handles");
    }

    public static void DrawPathHandles(Transform[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].get_position();
      Tween.DrawPathHelper(path1, color, "handles");
    }

    public static void CameraFadeDepth(int depth)
    {
      if (!Object.op_Implicit((Object) Tween.cameraFade))
        return;
      Tween.cameraFade.get_transform().set_position(new Vector3((float) Tween.cameraFade.get_transform().get_position().x, (float) Tween.cameraFade.get_transform().get_position().y, (float) depth));
    }

    public static void CameraFadeDestroy()
    {
      if (!Object.op_Implicit((Object) Tween.cameraFade))
        return;
      Object.Destroy((Object) Tween.cameraFade);
    }

    public static void CameraFadeSwap(Texture2D texture)
    {
      if (!Object.op_Implicit((Object) Tween.cameraFade))
        return;
      ((GUITexture) Tween.cameraFade.GetComponent<GUITexture>()).set_texture((Texture) texture);
    }

    public static GameObject CameraFadeAdd(Texture2D texture, int depth)
    {
      if (Object.op_Implicit((Object) Tween.cameraFade))
        return (GameObject) null;
      Tween.cameraFade = new GameObject("iTween Camera Fade");
      Tween.cameraFade.get_transform().set_position(new Vector3(0.5f, 0.5f, (float) depth));
      Tween.cameraFade.AddComponent<GUITexture>();
      ((GUITexture) Tween.cameraFade.GetComponent<GUITexture>()).set_texture((Texture) texture);
      ((GUITexture) Tween.cameraFade.GetComponent<GUITexture>()).set_color(new Color(0.5f, 0.5f, 0.5f, 0.0f));
      return Tween.cameraFade;
    }

    public static GameObject CameraFadeAdd(Texture2D texture)
    {
      if (Object.op_Implicit((Object) Tween.cameraFade))
        return (GameObject) null;
      Tween.cameraFade = new GameObject("iTween Camera Fade");
      Tween.cameraFade.get_transform().set_position(new Vector3(0.5f, 0.5f, (float) Tween.Defaults.cameraFadeDepth));
      Tween.cameraFade.AddComponent<GUITexture>();
      ((GUITexture) Tween.cameraFade.GetComponent<GUITexture>()).set_texture((Texture) texture);
      ((GUITexture) Tween.cameraFade.GetComponent<GUITexture>()).set_color(new Color(0.5f, 0.5f, 0.5f, 0.0f));
      return Tween.cameraFade;
    }

    public static GameObject CameraFadeAdd()
    {
      if (Object.op_Implicit((Object) Tween.cameraFade))
        return (GameObject) null;
      Tween.cameraFade = new GameObject("iTween Camera Fade");
      Tween.cameraFade.get_transform().set_position(new Vector3(0.5f, 0.5f, (float) Tween.Defaults.cameraFadeDepth));
      Tween.cameraFade.AddComponent<GUITexture>();
      ((GUITexture) Tween.cameraFade.GetComponent<GUITexture>()).set_texture((Texture) Tween.CameraTexture(Color.get_black()));
      ((GUITexture) Tween.cameraFade.GetComponent<GUITexture>()).set_color(new Color(0.5f, 0.5f, 0.5f, 0.0f));
      return Tween.cameraFade;
    }

    public static void Resume(GameObject target)
    {
      foreach (Behaviour component in target.GetComponents(typeof (Tween)))
        component.set_enabled(true);
    }

    public static void Resume(GameObject target, bool includechildren)
    {
      Tween.Resume(target);
      if (!includechildren)
        return;
      IEnumerator enumerator = target.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Tween.Resume(((Component) enumerator.Current).get_gameObject(), true);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void Resume(GameObject target, string type)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          ((Behaviour) component).set_enabled(true);
      }
    }

    public static void Resume(GameObject target, string type, bool includechildren)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          ((Behaviour) component).set_enabled(true);
      }
      if (!includechildren)
        return;
      IEnumerator enumerator = target.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Tween.Resume(((Component) enumerator.Current).get_gameObject(), type, true);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void Resume()
    {
      for (int index = 0; index < Tween.tweens.Count; ++index)
        Tween.Resume((GameObject) ((Hashtable) Tween.tweens[index])[(object) "target"]);
    }

    public static void Resume(string type)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < Tween.tweens.Count; ++index)
      {
        GameObject gameObject = (GameObject) ((Hashtable) Tween.tweens[index])[(object) "target"];
        arrayList.Insert(arrayList.Count, (object) gameObject);
      }
      for (int index = 0; index < arrayList.Count; ++index)
        Tween.Resume((GameObject) arrayList[index], type);
    }

    public static void Pause(GameObject target)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((double) component.delay > 0.0)
        {
          component.delay -= Time.get_time() - component.delayStarted;
          component.StopCoroutine("TweenDelay");
        }
        component.isPaused = true;
        ((Behaviour) component).set_enabled(false);
      }
    }

    public static void Pause(GameObject target, bool includechildren)
    {
      Tween.Pause(target);
      if (!includechildren)
        return;
      IEnumerator enumerator = target.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Tween.Pause(((Component) enumerator.Current).get_gameObject(), true);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void Pause(GameObject target, string type)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
        {
          if ((double) component.delay > 0.0)
          {
            component.delay -= Time.get_time() - component.delayStarted;
            component.StopCoroutine("TweenDelay");
          }
          component.isPaused = true;
          ((Behaviour) component).set_enabled(false);
        }
      }
    }

    public static void Pause(GameObject target, string type, bool includechildren)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
        {
          if ((double) component.delay > 0.0)
          {
            component.delay -= Time.get_time() - component.delayStarted;
            component.StopCoroutine("TweenDelay");
          }
          component.isPaused = true;
          ((Behaviour) component).set_enabled(false);
        }
      }
      if (!includechildren)
        return;
      IEnumerator enumerator = target.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Tween.Pause(((Component) enumerator.Current).get_gameObject(), type, true);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void Pause()
    {
      for (int index = 0; index < Tween.tweens.Count; ++index)
        Tween.Pause((GameObject) ((Hashtable) Tween.tweens[index])[(object) "target"]);
    }

    public static void Pause(string type)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < Tween.tweens.Count; ++index)
      {
        GameObject gameObject = (GameObject) ((Hashtable) Tween.tweens[index])[(object) "target"];
        arrayList.Insert(arrayList.Count, (object) gameObject);
      }
      for (int index = 0; index < arrayList.Count; ++index)
        Tween.Pause((GameObject) arrayList[index], type);
    }

    public static int Count()
    {
      return Tween.tweens.Count;
    }

    public static int Count(string type)
    {
      int num = 0;
      for (int index = 0; index < Tween.tweens.Count; ++index)
      {
        Hashtable tween = (Hashtable) Tween.tweens[index];
        if (((string) tween[(object) nameof (type)] + (string) tween[(object) "method"]).Substring(0, type.Length).ToLower() == type.ToLower())
          ++num;
      }
      return num;
    }

    public static int Count(GameObject target)
    {
      return target.GetComponents(typeof (Tween)).Length;
    }

    public static int Count(GameObject target, string type)
    {
      int num = 0;
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          ++num;
      }
      return num;
    }

    public static void Stop()
    {
      for (int index = 0; index < Tween.tweens.Count; ++index)
        Tween.Stop((GameObject) ((Hashtable) Tween.tweens[index])[(object) "target"]);
      Tween.tweens.Clear();
    }

    public static void Stop(string type)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < Tween.tweens.Count; ++index)
      {
        GameObject gameObject = (GameObject) ((Hashtable) Tween.tweens[index])[(object) "target"];
        arrayList.Insert(arrayList.Count, (object) gameObject);
      }
      for (int index = 0; index < arrayList.Count; ++index)
        Tween.Stop((GameObject) arrayList[index], type);
    }

    public static void StopByName(string name)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < Tween.tweens.Count; ++index)
      {
        GameObject gameObject = (GameObject) ((Hashtable) Tween.tweens[index])[(object) "target"];
        arrayList.Insert(arrayList.Count, (object) gameObject);
      }
      for (int index = 0; index < arrayList.Count; ++index)
        Tween.StopByName((GameObject) arrayList[index], name);
    }

    public static void Stop(GameObject target)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
        component.Dispose();
    }

    public static void Stop(GameObject target, bool includechildren)
    {
      Tween.Stop(target);
      if (!includechildren)
        return;
      IEnumerator enumerator = target.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Tween.Stop(((Component) enumerator.Current).get_gameObject(), true);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void Stop(GameObject target, string type)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          component.Dispose();
      }
    }

    public static void StopByName(GameObject target, string name)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if (component._name == name)
          component.Dispose();
      }
    }

    public static void Stop(GameObject target, string type, bool includechildren)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          component.Dispose();
      }
      if (!includechildren)
        return;
      IEnumerator enumerator = target.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Tween.Stop(((Component) enumerator.Current).get_gameObject(), type, true);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void StopByName(GameObject target, string name, bool includechildren)
    {
      foreach (Tween component in target.GetComponents(typeof (Tween)))
      {
        if (component._name == name)
          component.Dispose();
      }
      if (!includechildren)
        return;
      IEnumerator enumerator = target.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Tween.StopByName(((Component) enumerator.Current).get_gameObject(), name, true);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static Hashtable Hash(params object[] args)
    {
      Hashtable hashtable = new Hashtable(args.Length / 2);
      if (args.Length % 2 != 0)
      {
        Debug.LogError((object) "Tween Error: Hash requires an even number of arguments!");
        return (Hashtable) null;
      }
      for (int index = 0; index < args.Length - 1; index += 2)
        hashtable.Add(args[index], args[index + 1]);
      return hashtable;
    }

    private void Awake()
    {
      this.RetrieveArgs();
      this.lastRealTime = Time.get_realtimeSinceStartup();
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Tween.\u003CStart\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void Update()
    {
      if (!this.isRunning || this.physics)
        return;
      if (!this.reverse)
      {
        if ((double) this._percentage < 1.0)
          this.TweenUpdate();
        else
          this.TweenComplete();
      }
      else if ((double) this._percentage > 0.0)
        this.TweenUpdate();
      else
        this.TweenComplete();
    }

    private void FixedUpdate()
    {
      if (!this.isRunning || !this.physics)
        return;
      if (!this.reverse)
      {
        if ((double) this._percentage < 1.0)
          this.TweenUpdate();
        else
          this.TweenComplete();
      }
      else if ((double) this._percentage > 0.0)
        this.TweenUpdate();
      else
        this.TweenComplete();
    }

    private void LateUpdate()
    {
      if (!this.tweenArguments.Contains((object) "looktarget") || !this.isRunning || !(this.type == "move") && !(this.type == "shake") && !(this.type == "punch"))
        return;
      Tween.LookUpdate(((Component) this).get_gameObject(), this.tweenArguments);
    }

    private void OnEnable()
    {
      if (this.isRunning)
        this.EnableKinematic();
      if (!this.isPaused)
        return;
      this.isPaused = false;
      if ((double) this.delay <= 0.0)
        return;
      this.wasPaused = true;
      this.ResumeDelay();
    }

    private void OnDisable()
    {
      this.DisableKinematic();
    }

    private static void DrawLineHelper(Vector3[] line, Color color, string method)
    {
      Gizmos.set_color(color);
      for (int index = 0; index < line.Length - 1; ++index)
      {
        if (method == "gizmos")
          Gizmos.DrawLine(line[index], line[index + 1]);
        else if (method == "handles")
          Debug.LogError((object) "iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
      }
    }

    private static void DrawPathHelper(Vector3[] path, Color color, string method)
    {
      Vector3[] pts = Tween.PathControlPointGenerator(path);
      Vector3 vector3_1 = Tween.Interp(pts, 0.0f);
      Gizmos.set_color(color);
      int num = path.Length * 20;
      for (int index = 1; index <= num; ++index)
      {
        float t = (float) index / (float) num;
        Vector3 vector3_2 = Tween.Interp(pts, t);
        if (method == "gizmos")
          Gizmos.DrawLine(vector3_2, vector3_1);
        else if (method == "handles")
          Debug.LogError((object) "iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
        vector3_1 = vector3_2;
      }
    }

    private static Vector3[] PathControlPointGenerator(Vector3[] path)
    {
      Vector3[] vector3Array1 = path;
      int num = 2;
      Vector3[] vector3Array2 = new Vector3[vector3Array1.Length + num];
      Array.Copy((Array) vector3Array1, 0, (Array) vector3Array2, 1, vector3Array1.Length);
      vector3Array2[0] = Vector3.op_Addition(vector3Array2[1], Vector3.op_Subtraction(vector3Array2[1], vector3Array2[2]));
      vector3Array2[vector3Array2.Length - 1] = Vector3.op_Addition(vector3Array2[vector3Array2.Length - 2], Vector3.op_Subtraction(vector3Array2[vector3Array2.Length - 2], vector3Array2[vector3Array2.Length - 3]));
      if (Vector3.op_Equality(vector3Array2[1], vector3Array2[vector3Array2.Length - 2]))
      {
        Vector3[] vector3Array3 = new Vector3[vector3Array2.Length];
        Array.Copy((Array) vector3Array2, (Array) vector3Array3, vector3Array2.Length);
        vector3Array3[0] = vector3Array3[vector3Array3.Length - 3];
        vector3Array3[vector3Array3.Length - 1] = vector3Array3[2];
        vector3Array2 = new Vector3[vector3Array3.Length];
        Array.Copy((Array) vector3Array3, (Array) vector3Array2, vector3Array3.Length);
      }
      return vector3Array2;
    }

    private static Vector3 Interp(Vector3[] pts, float t)
    {
      int num1 = pts.Length - 3;
      int index = Mathf.Min(Mathf.FloorToInt(t * (float) num1), num1 - 1);
      float num2 = t * (float) num1 - (float) index;
      Vector3 pt1 = pts[index];
      Vector3 pt2 = pts[index + 1];
      Vector3 pt3 = pts[index + 2];
      Vector3 pt4 = pts[index + 3];
      return Vector3.op_Multiply(0.5f, Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_UnaryNegation(pt1), Vector3.op_Multiply(3f, pt2)), Vector3.op_Multiply(3f, pt3)), pt4), num2 * num2 * num2), Vector3.op_Multiply(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Multiply(2f, pt1), Vector3.op_Multiply(5f, pt2)), Vector3.op_Multiply(4f, pt3)), pt4), num2 * num2)), Vector3.op_Multiply(Vector3.op_Addition(Vector3.op_UnaryNegation(pt1), pt3), num2)), Vector3.op_Multiply(2f, pt2)));
    }

    private static Tween Launch(GameObject target, Hashtable args)
    {
      if (!args.Contains((object) "id"))
        args[(object) "id"] = (object) Tween.GenerateID();
      if (!args.Contains((object) nameof (target)))
        args[(object) nameof (target)] = (object) target;
      Tween.tweens.Insert(0, (object) args);
      return (Tween) target.AddComponent<Tween>();
    }

    private static Hashtable CleanArgs(Hashtable args)
    {
      Hashtable hashtable1 = new Hashtable(args.Count);
      Hashtable hashtable2 = new Hashtable(args.Count);
      foreach (DictionaryEntry dictionaryEntry in args)
        hashtable1.Add(dictionaryEntry.Key, dictionaryEntry.Value);
      foreach (DictionaryEntry dictionaryEntry in hashtable1)
      {
        if (dictionaryEntry.Value.GetType() == typeof (int))
        {
          float num = (float) (int) dictionaryEntry.Value;
          args[dictionaryEntry.Key] = (object) num;
        }
        if (dictionaryEntry.Value.GetType() == typeof (double))
        {
          float num = (float) (double) dictionaryEntry.Value;
          args[dictionaryEntry.Key] = (object) num;
        }
      }
      foreach (DictionaryEntry dictionaryEntry in args)
        hashtable2.Add((object) dictionaryEntry.Key.ToString().ToLower(), dictionaryEntry.Value);
      args = hashtable2;
      return args;
    }

    private static string GenerateID()
    {
      int num1 = 15;
      char[] chArray = new char[61]
      {
        'a',
        'b',
        'c',
        'd',
        'e',
        'f',
        'g',
        'h',
        'i',
        'j',
        'k',
        'l',
        'm',
        'n',
        'o',
        'p',
        'q',
        'r',
        's',
        't',
        'u',
        'v',
        'w',
        'x',
        'y',
        'z',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8'
      };
      int num2 = chArray.Length - 1;
      string empty = string.Empty;
      for (int index = 0; index < num1; ++index)
        empty += (string) (object) chArray[(int) Mathf.Floor((float) Random.Range(0, num2))];
      return empty;
    }

    private void RetrieveArgs()
    {
      foreach (Hashtable tween in Tween.tweens)
      {
        if (Object.op_Equality((Object) tween[(object) "target"], (Object) ((Component) this).get_gameObject()))
        {
          this.tweenArguments = tween;
          break;
        }
      }
      this.id = (string) this.tweenArguments[(object) "id"];
      this.type = (string) this.tweenArguments[(object) "type"];
      this._name = (string) this.tweenArguments[(object) "name"];
      this.method = (string) this.tweenArguments[(object) "method"];
      this.time = !this.tweenArguments.Contains((object) "time") ? Tween.Defaults.time : (float) this.tweenArguments[(object) "time"];
      if (Object.op_Inequality((Object) ((Component) this).GetComponent<Rigidbody>(), (Object) null))
        this.physics = true;
      this.delay = !this.tweenArguments.Contains((object) "delay") ? Tween.Defaults.delay : (float) this.tweenArguments[(object) "delay"];
      if (this.tweenArguments.Contains((object) "namedcolorvalue"))
      {
        if (this.tweenArguments[(object) "namedcolorvalue"].GetType() == typeof (Tween.NamedValueColor))
        {
          this.namedcolorvalue = (Tween.NamedValueColor) this.tweenArguments[(object) "namedcolorvalue"];
        }
        else
        {
          try
          {
            this.namedcolorvalue = (Tween.NamedValueColor) Enum.Parse(typeof (Tween.NamedValueColor), (string) this.tweenArguments[(object) "namedcolorvalue"], true);
          }
          catch
          {
            Debug.LogWarning((object) "iTween: Unsupported namedcolorvalue supplied! Default will be used.");
            this.namedcolorvalue = Tween.NamedValueColor._Color;
          }
        }
      }
      else
        this.namedcolorvalue = Tween.Defaults.namedColorValue;
      if (this.tweenArguments.Contains((object) "looptype"))
      {
        if (this.tweenArguments[(object) "looptype"].GetType() == typeof (Tween.LoopType))
        {
          this.loopType = (Tween.LoopType) this.tweenArguments[(object) "looptype"];
        }
        else
        {
          try
          {
            this.loopType = (Tween.LoopType) Enum.Parse(typeof (Tween.LoopType), (string) this.tweenArguments[(object) "looptype"], true);
          }
          catch
          {
            Debug.LogWarning((object) "iTween: Unsupported loopType supplied! Default will be used.");
            this.loopType = Tween.LoopType.none;
          }
        }
      }
      else
        this.loopType = Tween.LoopType.none;
      if (this.tweenArguments.Contains((object) "easetype"))
      {
        if (this.tweenArguments[(object) "easetype"].GetType() == typeof (Tween.EaseType))
        {
          this.easeType = (Tween.EaseType) this.tweenArguments[(object) "easetype"];
        }
        else
        {
          try
          {
            this.easeType = (Tween.EaseType) Enum.Parse(typeof (Tween.EaseType), (string) this.tweenArguments[(object) "easetype"], true);
          }
          catch
          {
            Debug.LogWarning((object) "iTween: Unsupported easeType supplied! Default will be used.");
            this.easeType = Tween.Defaults.easeType;
          }
        }
      }
      else
        this.easeType = Tween.Defaults.easeType;
      if (this.tweenArguments.Contains((object) "space"))
      {
        if (this.tweenArguments[(object) "space"].GetType() == typeof (Space))
        {
          this.space = (Space) this.tweenArguments[(object) "space"];
        }
        else
        {
          try
          {
            this.space = (Space) Enum.Parse(typeof (Space), (string) this.tweenArguments[(object) "space"], true);
          }
          catch
          {
            Debug.LogWarning((object) "iTween: Unsupported space supplied! Default will be used.");
            this.space = Tween.Defaults.space;
          }
        }
      }
      else
        this.space = Tween.Defaults.space;
      this.isLocal = !this.tweenArguments.Contains((object) "islocal") ? Tween.Defaults.isLocal : (bool) this.tweenArguments[(object) "islocal"];
      this.useRealTime = !this.tweenArguments.Contains((object) "ignoretimescale") ? Tween.Defaults.useRealTime : (bool) this.tweenArguments[(object) "ignoretimescale"];
      this.GetEasingFunction();
    }

    private void GetEasingFunction()
    {
      switch (this.easeType)
      {
        case Tween.EaseType.easeInQuad:
          this.ease = new Tween.EasingFunction(this.easeInQuad);
          break;
        case Tween.EaseType.easeOutQuad:
          this.ease = new Tween.EasingFunction(this.easeOutQuad);
          break;
        case Tween.EaseType.easeInOutQuad:
          this.ease = new Tween.EasingFunction(this.easeInOutQuad);
          break;
        case Tween.EaseType.easeInCubic:
          this.ease = new Tween.EasingFunction(this.easeInCubic);
          break;
        case Tween.EaseType.easeOutCubic:
          this.ease = new Tween.EasingFunction(this.easeOutCubic);
          break;
        case Tween.EaseType.easeInOutCubic:
          this.ease = new Tween.EasingFunction(this.easeInOutCubic);
          break;
        case Tween.EaseType.easeInQuart:
          this.ease = new Tween.EasingFunction(this.easeInQuart);
          break;
        case Tween.EaseType.easeOutQuart:
          this.ease = new Tween.EasingFunction(this.easeOutQuart);
          break;
        case Tween.EaseType.easeInOutQuart:
          this.ease = new Tween.EasingFunction(this.easeInOutQuart);
          break;
        case Tween.EaseType.easeInQuint:
          this.ease = new Tween.EasingFunction(this.easeInQuint);
          break;
        case Tween.EaseType.easeOutQuint:
          this.ease = new Tween.EasingFunction(this.easeOutQuint);
          break;
        case Tween.EaseType.easeInOutQuint:
          this.ease = new Tween.EasingFunction(this.easeInOutQuint);
          break;
        case Tween.EaseType.easeInSine:
          this.ease = new Tween.EasingFunction(this.easeInSine);
          break;
        case Tween.EaseType.easeOutSine:
          this.ease = new Tween.EasingFunction(this.easeOutSine);
          break;
        case Tween.EaseType.easeInOutSine:
          this.ease = new Tween.EasingFunction(this.easeInOutSine);
          break;
        case Tween.EaseType.easeInExpo:
          this.ease = new Tween.EasingFunction(this.easeInExpo);
          break;
        case Tween.EaseType.easeOutExpo:
          this.ease = new Tween.EasingFunction(this.easeOutExpo);
          break;
        case Tween.EaseType.easeInOutExpo:
          this.ease = new Tween.EasingFunction(this.easeInOutExpo);
          break;
        case Tween.EaseType.easeInCirc:
          this.ease = new Tween.EasingFunction(this.easeInCirc);
          break;
        case Tween.EaseType.easeOutCirc:
          this.ease = new Tween.EasingFunction(this.easeOutCirc);
          break;
        case Tween.EaseType.easeInOutCirc:
          this.ease = new Tween.EasingFunction(this.easeInOutCirc);
          break;
        case Tween.EaseType.linear:
          this.ease = new Tween.EasingFunction(this.linear);
          break;
        case Tween.EaseType.spring:
          this.ease = new Tween.EasingFunction(this.spring);
          break;
        case Tween.EaseType.easeInBounce:
          this.ease = new Tween.EasingFunction(this.easeInBounce);
          break;
        case Tween.EaseType.easeOutBounce:
          this.ease = new Tween.EasingFunction(this.easeOutBounce);
          break;
        case Tween.EaseType.easeInOutBounce:
          this.ease = new Tween.EasingFunction(this.easeInOutBounce);
          break;
        case Tween.EaseType.easeInBack:
          this.ease = new Tween.EasingFunction(this.easeInBack);
          break;
        case Tween.EaseType.easeOutBack:
          this.ease = new Tween.EasingFunction(this.easeOutBack);
          break;
        case Tween.EaseType.easeInOutBack:
          this.ease = new Tween.EasingFunction(this.easeInOutBack);
          break;
        case Tween.EaseType.easeInElastic:
          this.ease = new Tween.EasingFunction(this.easeInElastic);
          break;
        case Tween.EaseType.easeOutElastic:
          this.ease = new Tween.EasingFunction(this.easeOutElastic);
          break;
        case Tween.EaseType.easeInOutElastic:
          this.ease = new Tween.EasingFunction(this.easeInOutElastic);
          break;
      }
    }

    private void UpdatePercentage()
    {
      this.runningTime += !this.useRealTime ? Time.get_deltaTime() : Time.get_realtimeSinceStartup() - this.lastRealTime;
      this._percentage = !this.reverse ? this.runningTime / this.time : (float) (1.0 - (double) this.runningTime / (double) this.time);
      this.lastRealTime = Time.get_realtimeSinceStartup();
    }

    private void CallBack(string callbackType)
    {
      if (!this.tweenArguments.Contains((object) callbackType) || this.tweenArguments.Contains((object) "ischild"))
        return;
      GameObject gameObject = !this.tweenArguments.Contains((object) (callbackType + "target")) ? ((Component) this).get_gameObject() : (GameObject) this.tweenArguments[(object) (callbackType + "target")];
      if (this.tweenArguments[(object) callbackType].GetType() == typeof (string))
      {
        gameObject.SendMessage((string) this.tweenArguments[(object) callbackType], this.tweenArguments[(object) (callbackType + "params")], (SendMessageOptions) 1);
      }
      else
      {
        Debug.LogError((object) "iTween Error: Callback method references must be passed as a String!");
        Object.Destroy((Object) this);
      }
    }

    private void Dispose()
    {
      for (int index = 0; index < Tween.tweens.Count; ++index)
      {
        if ((string) ((Hashtable) Tween.tweens[index])[(object) "id"] == this.id)
        {
          Tween.tweens.RemoveAt(index);
          break;
        }
      }
      Object.Destroy((Object) this);
    }

    private void ConflictCheck()
    {
      foreach (Tween component in ((Component) this).GetComponents(typeof (Tween)))
      {
        if (component.type == "value")
          break;
        if (component.isRunning && component.type == this.type)
        {
          if (component.method != this.method)
            break;
          if (component.tweenArguments.Count != this.tweenArguments.Count)
          {
            component.Dispose();
            break;
          }
          foreach (DictionaryEntry tweenArgument in this.tweenArguments)
          {
            if (!component.tweenArguments.Contains(tweenArgument.Key))
            {
              component.Dispose();
              return;
            }
            if (!component.tweenArguments[tweenArgument.Key].Equals(this.tweenArguments[tweenArgument.Key]) && (string) tweenArgument.Key != "id")
            {
              component.Dispose();
              return;
            }
          }
          this.Dispose();
        }
      }
    }

    private void EnableKinematic()
    {
    }

    private void DisableKinematic()
    {
    }

    private void ResumeDelay()
    {
      this.StartCoroutine("TweenDelay");
    }

    private float linear(float start, float end, float value)
    {
      return Mathf.Lerp(start, end, value);
    }

    private float clerp(float start, float end, float value)
    {
      float num1 = 0.0f;
      float num2 = 360f;
      float num3 = Mathf.Abs((float) (((double) num2 - (double) num1) / 2.0));
      float num4;
      if ((double) end - (double) start < -(double) num3)
      {
        float num5 = (num2 - start + end) * value;
        num4 = start + num5;
      }
      else if ((double) end - (double) start > (double) num3)
      {
        float num5 = (float) -((double) num2 - (double) end + (double) start) * value;
        num4 = start + num5;
      }
      else
        num4 = start + (end - start) * value;
      return num4;
    }

    private float spring(float start, float end, float value)
    {
      value = Mathf.Clamp01(value);
      value = (float) (((double) Mathf.Sin((float) ((double) value * 3.14159274101257 * (0.200000002980232 + 2.5 * (double) value * (double) value * (double) value))) * (double) Mathf.Pow(1f - value, 2.2f) + (double) value) * (1.0 + 1.20000004768372 * (1.0 - (double) value)));
      return start + (end - start) * value;
    }

    private float easeInQuad(float start, float end, float value)
    {
      end -= start;
      return end * value * value + start;
    }

    private float easeOutQuad(float start, float end, float value)
    {
      end -= start;
      return (float) (-(double) end * (double) value * ((double) value - 2.0)) + start;
    }

    private float easeInOutQuad(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value + start;
      --value;
      return (float) (-(double) end / 2.0 * ((double) value * ((double) value - 2.0) - 1.0)) + start;
    }

    private float easeInCubic(float start, float end, float value)
    {
      end -= start;
      return end * value * value * value + start;
    }

    private float easeOutCubic(float start, float end, float value)
    {
      --value;
      end -= start;
      return end * (float) ((double) value * (double) value * (double) value + 1.0) + start;
    }

    private float easeInOutCubic(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value * value + start;
      value -= 2f;
      return (float) ((double) end / 2.0 * ((double) value * (double) value * (double) value + 2.0)) + start;
    }

    private float easeInQuart(float start, float end, float value)
    {
      end -= start;
      return end * value * value * value * value + start;
    }

    private float easeOutQuart(float start, float end, float value)
    {
      --value;
      end -= start;
      return (float) (-(double) end * ((double) value * (double) value * (double) value * (double) value - 1.0)) + start;
    }

    private float easeInOutQuart(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value * value * value + start;
      value -= 2f;
      return (float) (-(double) end / 2.0 * ((double) value * (double) value * (double) value * (double) value - 2.0)) + start;
    }

    private float easeInQuint(float start, float end, float value)
    {
      end -= start;
      return end * value * value * value * value * value + start;
    }

    private float easeOutQuint(float start, float end, float value)
    {
      --value;
      end -= start;
      return end * (float) ((double) value * (double) value * (double) value * (double) value * (double) value + 1.0) + start;
    }

    private float easeInOutQuint(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value * value * value * value + start;
      value -= 2f;
      return (float) ((double) end / 2.0 * ((double) value * (double) value * (double) value * (double) value * (double) value + 2.0)) + start;
    }

    private float easeInSine(float start, float end, float value)
    {
      end -= start;
      return -end * Mathf.Cos((float) ((double) value / 1.0 * 1.57079637050629)) + end + start;
    }

    private float easeOutSine(float start, float end, float value)
    {
      end -= start;
      return end * Mathf.Sin((float) ((double) value / 1.0 * 1.57079637050629)) + start;
    }

    private float easeInOutSine(float start, float end, float value)
    {
      end -= start;
      return (float) (-(double) end / 2.0 * ((double) Mathf.Cos((float) (3.14159274101257 * (double) value / 1.0)) - 1.0)) + start;
    }

    private float easeInExpo(float start, float end, float value)
    {
      end -= start;
      return end * Mathf.Pow(2f, (float) (10.0 * ((double) value / 1.0 - 1.0))) + start;
    }

    private float easeOutExpo(float start, float end, float value)
    {
      end -= start;
      return end * (float) (-(double) Mathf.Pow(2f, (float) (-10.0 * (double) value / 1.0)) + 1.0) + start;
    }

    private float easeInOutExpo(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * Mathf.Pow(2f, (float) (10.0 * ((double) value - 1.0))) + start;
      --value;
      return (float) ((double) end / 2.0 * (-(double) Mathf.Pow(2f, -10f * value) + 2.0)) + start;
    }

    private float easeInCirc(float start, float end, float value)
    {
      end -= start;
      return (float) (-(double) end * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) - 1.0)) + start;
    }

    private float easeOutCirc(float start, float end, float value)
    {
      --value;
      end -= start;
      return end * Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) + start;
    }

    private float easeInOutCirc(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return (float) (-(double) end / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) - 1.0)) + start;
      value -= 2f;
      return (float) ((double) end / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) + 1.0)) + start;
    }

    private float easeInBounce(float start, float end, float value)
    {
      end -= start;
      float num = 1f;
      return end - this.easeOutBounce(0.0f, end, num - value) + start;
    }

    private float easeOutBounce(float start, float end, float value)
    {
      value /= 1f;
      end -= start;
      if ((double) value < 0.363636374473572)
        return end * (121f / 16f * value * value) + start;
      if ((double) value < 0.727272748947144)
      {
        value -= 0.5454546f;
        return end * (float) (121.0 / 16.0 * (double) value * (double) value + 0.75) + start;
      }
      if ((double) value < 10.0 / 11.0)
      {
        value -= 0.8181818f;
        return end * (float) (121.0 / 16.0 * (double) value * (double) value + 15.0 / 16.0) + start;
      }
      value -= 0.9545454f;
      return end * (float) (121.0 / 16.0 * (double) value * (double) value + 63.0 / 64.0) + start;
    }

    private float easeInOutBounce(float start, float end, float value)
    {
      end -= start;
      float num = 1f;
      return (double) value < (double) num / 2.0 ? this.easeInBounce(0.0f, end, value * 2f) * 0.5f + start : (float) ((double) this.easeOutBounce(0.0f, end, value * 2f - num) * 0.5 + (double) end * 0.5) + start;
    }

    private float easeInBack(float start, float end, float value)
    {
      end -= start;
      value /= 1f;
      float num = 1.70158f;
      return (float) ((double) end * (double) value * (double) value * (((double) num + 1.0) * (double) value - (double) num)) + start;
    }

    private float easeOutBack(float start, float end, float value)
    {
      float num = 1.70158f;
      end -= start;
      value = (float) ((double) value / 1.0 - 1.0);
      return end * (float) ((double) value * (double) value * (((double) num + 1.0) * (double) value + (double) num) + 1.0) + start;
    }

    private float easeInOutBack(float start, float end, float value)
    {
      float num1 = 1.70158f;
      end -= start;
      value /= 0.5f;
      if ((double) value < 1.0)
      {
        float num2 = num1 * 1.525f;
        return (float) ((double) end / 2.0 * ((double) value * (double) value * (((double) num2 + 1.0) * (double) value - (double) num2))) + start;
      }
      value -= 2f;
      float num3 = num1 * 1.525f;
      return (float) ((double) end / 2.0 * ((double) value * (double) value * (((double) num3 + 1.0) * (double) value + (double) num3) + 2.0)) + start;
    }

    private float punch(float amplitude, float value)
    {
      if ((double) value == 0.0 || (double) value == 1.0)
        return 0.0f;
      float num1 = 0.3f;
      float num2 = num1 / 6.283185f * Mathf.Asin(0.0f);
      return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((float) (((double) value * 1.0 - (double) num2) * 6.28318548202515) / num1);
    }

    private float easeInElastic(float start, float end, float value)
    {
      end -= start;
      float num1 = 1f;
      float num2 = num1 * 0.3f;
      float num3 = 0.0f;
      if ((double) value == 0.0)
        return start;
      if ((double) (value /= num1) == 1.0)
        return start + end;
      float num4;
      if ((double) num3 == 0.0 || (double) num3 < (double) Mathf.Abs(end))
      {
        num3 = end;
        num4 = num2 / 4f;
      }
      else
        num4 = num2 / 6.283185f * Mathf.Asin(end / num3);
      return (float) -((double) num3 * (double) Mathf.Pow(2f, 10f * --value) * (double) Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.28318548202515) / num2)) + start;
    }

    private float easeOutElastic(float start, float end, float value)
    {
      end -= start;
      float num1 = 1f;
      float num2 = num1 * 0.3f;
      float num3 = 0.0f;
      if ((double) value == 0.0)
        return start;
      if ((double) (value /= num1) == 1.0)
        return start + end;
      float num4;
      if ((double) num3 == 0.0 || (double) num3 < (double) Mathf.Abs(end))
      {
        num3 = end;
        num4 = num2 / 4f;
      }
      else
        num4 = num2 / 6.283185f * Mathf.Asin(end / num3);
      return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.28318548202515) / num2) + end + start;
    }

    private float easeInOutElastic(float start, float end, float value)
    {
      end -= start;
      float num1 = 1f;
      float num2 = num1 * 0.3f;
      float num3 = 0.0f;
      if ((double) value == 0.0)
        return start;
      if ((double) (value /= num1 / 2f) == 2.0)
        return start + end;
      float num4;
      if ((double) num3 == 0.0 || (double) num3 < (double) Mathf.Abs(end))
      {
        num3 = end;
        num4 = num2 / 4f;
      }
      else
        num4 = num2 / 6.283185f * Mathf.Asin(end / num3);
      return (double) value < 1.0 ? (float) (-0.5 * ((double) num3 * (double) Mathf.Pow(2f, 10f * --value) * (double) Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.28318548202515) / num2))) + start : (float) ((double) num3 * (double) Mathf.Pow(2f, -10f * --value) * (double) Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.28318548202515) / num2) * 0.5) + end + start;
    }

    private delegate float EasingFunction(float start, float end, float value);

    private delegate void ApplyTween();

    public enum EaseType
    {
      easeInQuad,
      easeOutQuad,
      easeInOutQuad,
      easeInCubic,
      easeOutCubic,
      easeInOutCubic,
      easeInQuart,
      easeOutQuart,
      easeInOutQuart,
      easeInQuint,
      easeOutQuint,
      easeInOutQuint,
      easeInSine,
      easeOutSine,
      easeInOutSine,
      easeInExpo,
      easeOutExpo,
      easeInOutExpo,
      easeInCirc,
      easeOutCirc,
      easeInOutCirc,
      linear,
      spring,
      easeInBounce,
      easeOutBounce,
      easeInOutBounce,
      easeInBack,
      easeOutBack,
      easeInOutBack,
      easeInElastic,
      easeOutElastic,
      easeInOutElastic,
    }

    public enum LoopType
    {
      none,
      loop,
      pingPong,
    }

    public enum NamedValueColor
    {
      _Color,
      _SpecColor,
      _Emission,
      _ReflectColor,
    }

    public delegate bool CompleteFunction();

    public static class Defaults
    {
      public static float time = 1f;
      public static float delay = 0.0f;
      public static Tween.NamedValueColor namedColorValue = Tween.NamedValueColor._Color;
      public static Tween.LoopType loopType = Tween.LoopType.none;
      public static Tween.EaseType easeType = Tween.EaseType.easeOutExpo;
      public static float lookSpeed = 3f;
      public static bool isLocal = false;
      public static Space space = (Space) 1;
      public static bool orientToPath = false;
      public static Color color = Color.get_white();
      public static float updateTimePercentage = 0.05f;
      public static float updateTime = 1f * Tween.Defaults.updateTimePercentage;
      public static int cameraFadeDepth = 999999;
      public static float lookAhead = 0.05f;
      public static bool useRealTime = false;
      public static Vector3 up = Vector3.get_up();
    }

    private class CRSpline
    {
      public Vector3[] pts;

      public CRSpline(params Vector3[] pts)
      {
        this.pts = new Vector3[pts.Length];
        Array.Copy((Array) pts, (Array) this.pts, pts.Length);
      }

      public Vector3 Interp(float t)
      {
        int num1 = this.pts.Length - 3;
        int index = Mathf.Min(Mathf.FloorToInt(t * (float) num1), num1 - 1);
        float num2 = t * (float) num1 - (float) index;
        Vector3 pt1 = this.pts[index];
        Vector3 pt2 = this.pts[index + 1];
        Vector3 pt3 = this.pts[index + 2];
        Vector3 pt4 = this.pts[index + 3];
        return Vector3.op_Multiply(0.5f, Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_UnaryNegation(pt1), Vector3.op_Multiply(3f, pt2)), Vector3.op_Multiply(3f, pt3)), pt4), num2 * num2 * num2), Vector3.op_Multiply(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Multiply(2f, pt1), Vector3.op_Multiply(5f, pt2)), Vector3.op_Multiply(4f, pt3)), pt4), num2 * num2)), Vector3.op_Multiply(Vector3.op_Addition(Vector3.op_UnaryNegation(pt1), pt3), num2)), Vector3.op_Multiply(2f, pt2)));
      }
    }
  }
}
