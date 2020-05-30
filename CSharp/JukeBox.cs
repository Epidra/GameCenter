using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameCenter {
    public class JukeBox {

        FileManager Filemanager;

        int current_track = 0;

        Song[] song = new Song[5];
        SoundEffect sound_coin;
        SoundEffect sound_explosion;
        SoundEffect sound_place;
        SoundEffect sound_cleared;
        SoundEffect sound_card_start;
        SoundEffect sound_card_draw;
        SoundEffect sound_dice;

        public JukeBox() {
            MediaPlayer.IsRepeating = true;
        }

        public void Load_Content(ContentManager Content, FileManager fm) {
            Filemanager = fm;
            song[0] = Content.Load<Song>("Audio/Music/GC01");
            song[1] = Content.Load<Song>("Audio/Music/GC02");
            song[2] = Content.Load<Song>("Audio/Music/DST02");
            song[3] = Content.Load<Song>("Audio/Music/DST01");
            song[4] = Content.Load<Song>("Audio/Music/DST03");
            //song[5] = Content.Load<Song>("Audio/Music/Lunar Harvest");
            //song[6] = Content.Load<Song>("Audio/Music/Blue Space");
            //song[7] = Content.Load<Song>("Audio/Music/Through Space");
            //song[8] = Content.Load<Song>("Audio/Music/TechnoGEEK");
            //song[9] = Content.Load<Song>("Audio/Music/Lunar Harvest");
            //song[10] = Content.Load<Song>("Audio/Music/Blue Space");
            //song[11] = Content.Load<Song>("Audio/Music/Through Space");
            //song[12] = Content.Load<Song>("Audio/Music/TechnoGEEK");
            //song[13] = Content.Load<Song>("Audio/Music/Lunar Harvest");
            sound_coin = Content.Load<SoundEffect>("Audio/Sound/Coin");
            sound_explosion = Content.Load<SoundEffect>("Audio/Sound/Explosion");
            sound_place = Content.Load<SoundEffect>("Audio/Sound/Place");
            sound_cleared = Content.Load<SoundEffect>("Audio/Sound/Cleared");
            sound_card_start = Content.Load<SoundEffect>("Audio/Sound/CardStart");
            sound_card_draw = Content.Load<SoundEffect>("Audio/Sound/CardDraw");
            sound_dice = Content.Load<SoundEffect>("Audio/Sound/Dice");
        }

        public void Apply_Volume() {
            float f = Filemanager == null ? 0 : (float)Filemanager.system_volume_music / 100;
            MediaPlayer.Volume = f;
            if(MediaPlayer.Volume <= 0) {
                Pause();
                Filemanager.system_active_music = false;
            }
            f = Filemanager == null ? 0 : (float)Filemanager.system_volume_sound / 100;
            SoundEffect.MasterVolume = f;
        }

        public void Noise(string s) {
            if(Filemanager.system_active_sound) {
                switch(s) {
                    case "Coin": sound_coin.Play(); break;
                    case "Explosion": sound_explosion.Play(); break;
                    case "Place": sound_place.Play(); break;
                    case "Cleared": sound_cleared.Play(); break;
                    case "CardStart": sound_card_start.Play(); break;
                    case "CardDraw": sound_card_draw.Play(); break;
                    case "Dice": sound_dice.Play(); break;
                }
            }
        }

        public void Select_Track(int i) {
            if(Filemanager.system_volume_music != 0.00f) {
                if(i == 0) { // Previous Track
                    current_track--;
                    if(current_track < 0) current_track = song.Length - 1;
                    while(!Filemanager.music_active[current_track]) {
                        current_track--;
                        if(current_track < 0) current_track = song.Length - 1;
                    }
                    MediaPlayer.Play(song[current_track]);
                }
                if(i == 1) { // Next Track
                    current_track++;
                    if(current_track >= song.Length) current_track = 0;
                    while(!Filemanager.music_active[current_track]) {
                        current_track++;
                        if(current_track >= song.Length) current_track = 0;
                    }
                    MediaPlayer.Play(song[current_track]);
                }
                if(i == 2) { // Random Track
                    Random random = new Random();
                    current_track = random.Next(song.Length);
                    while(!Filemanager.music_active[current_track]) {
                        current_track = random.Next(song.Length);
                    }
                    MediaPlayer.Play(song[current_track]);
                }
            }
        }

        public void Pause() {
            MediaPlayer.Stop();
        }

        public void Resume() {
            if(Filemanager.system_volume_music != 0.00f) {
                MediaPlayer.Play(song[current_track]);
            }
        }

        public string Get_TrackName() {
            return Filemanager.music_title[current_track];
        }

    }
}
