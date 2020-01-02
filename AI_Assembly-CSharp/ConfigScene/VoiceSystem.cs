// Decompiled with JetBrains decompiler
// Type: ConfigScene.VoiceSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace ConfigScene
{
  public class VoiceSystem : BaseSystem
  {
    public SoundData PCM = new SoundData();
    public Dictionary<int, VoiceSystem.Voice> chara;

    public VoiceSystem(string elementName, Dictionary<int, string> dic)
      : base(elementName)
    {
      this.PCM.ChangeEvent += (Action<SoundData>) (sd =>
      {
        MixerVolume.Set(Manager.Voice.Mixer, MixerVolume.Names.PCMVolume, sd.GetVolume());
        Debug.Log((object) ("PCM : " + (string) sd));
      });
      this.chara = dic.ToDictionary<KeyValuePair<int, string>, int, VoiceSystem.Voice>((Func<KeyValuePair<int, string>, int>) (v => v.Key), (Func<KeyValuePair<int, string>, VoiceSystem.Voice>) (v => new VoiceSystem.Voice(v.Value, new SoundData())));
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      this.chara.Select<KeyValuePair<int, VoiceSystem.Voice>, \u003C\u003E__AnonType7<\u003C\u003E__AnonType6<int, SoundData>>>((Func<KeyValuePair<int, VoiceSystem.Voice>, \u003C\u003E__AnonType7<\u003C\u003E__AnonType6<int, SoundData>>>) (p => new \u003C\u003E__AnonType7<\u003C\u003E__AnonType6<int, SoundData>>(new \u003C\u003E__AnonType6<int, SoundData>(p.Key, p.Value.sound)))).ToList<\u003C\u003E__AnonType7<\u003C\u003E__AnonType6<int, SoundData>>>().ForEach((Action<\u003C\u003E__AnonType7<\u003C\u003E__AnonType6<int, SoundData>>>) (p => p.sd.sound.ChangeEvent += (Action<SoundData>) (sd =>
      {
        float volume = sd.GetVolume();
        int key = p.sd.Key;
        Singleton<Manager.Voice>.Instance.GetPlayingList(key).ForEach((Action<AudioSource>) (playing => playing.set_volume(volume)));
        string str = (string) sd;
        Debug.Log((object) (string.Format("no:{0}", (object) key) + str));
      })));
    }

    public override void Init()
    {
      this.PCM.Mute = false;
      this.PCM.Volume = 100;
      foreach (KeyValuePair<int, VoiceSystem.Voice> keyValuePair in this.chara)
      {
        SoundData sound = keyValuePair.Value.sound;
        sound.Mute = false;
        sound.Volume = 100;
      }
    }

    public override void Read(string rootName, XmlDocument xml)
    {
      try
      {
        string str = rootName + "/" + this.elementName + "/";
        XmlNodeList xmlNodeList = xml.SelectNodes(str + "PCM");
        if (xmlNodeList != null && xmlNodeList.Item(0) is XmlElement xmlElement)
          this.PCM.Parse(xmlElement.InnerText);
        foreach (KeyValuePair<int, VoiceSystem.Voice> keyValuePair in this.chara)
        {
          if (xml.SelectNodes(str + keyValuePair.Value.file).Item(0) is XmlElement xmlElement)
            keyValuePair.Value.sound.Parse(xmlElement.InnerText);
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
      }
    }

    public override void Write(XmlWriter writer)
    {
      writer.WriteStartElement(this.elementName);
      writer.WriteStartElement("PCM");
      writer.WriteValue((string) this.PCM);
      writer.WriteEndElement();
      foreach (KeyValuePair<int, VoiceSystem.Voice> keyValuePair in this.chara)
      {
        VoiceSystem.Voice voice = keyValuePair.Value;
        writer.WriteStartElement(voice.file);
        writer.WriteValue((string) voice.sound);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    public class Voice
    {
      public Voice(string file, SoundData sound)
      {
        this.file = file;
        this.sound = sound;
      }

      public string file { get; private set; }

      public SoundData sound { get; private set; }
    }
  }
}
