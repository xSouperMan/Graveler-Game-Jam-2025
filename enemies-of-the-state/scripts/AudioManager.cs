
using Godot;
using System;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }

    // --- Audio Players ---
    private AudioStreamPlayer _musicPlayer;
    private AudioStreamPlayer _footstepPlayer;
    private AudioStreamPlayer _sfx1;
    private AudioStreamPlayer _sfx2;
    private AudioStreamPlayer _sfx3;

    // --- Streams ---
    private AudioStream footstepsSound;
    private AudioStream musicSound;

    private AudioStream[] greetings;
    private AudioStream[] speech;
    private AudioStream[] door;

    private AudioStream gameOverSound;
    private AudioStream paperSound;
    private AudioStream idPleaseSound;


    public override void _Ready()
    {
        Instance = this;
        // get players
        _musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        _footstepPlayer = GetNode<AudioStreamPlayer>("FootstepPlayer");
        _sfx1 = GetNode<AudioStreamPlayer>("SFXPlayer1");
        _sfx2 = GetNode<AudioStreamPlayer>("SFXPlayer2");
        _sfx3 = GetNode<AudioStreamPlayer>("SFXPlayer3");

        // load all sounds
        footstepsSound = GD.Load<AudioStream>("res://assets/sounds/steps.mp3");
        musicSound = GD.Load<AudioStream>("res://assets/sounds/8-bit-march-rem.mp3");

        greetings = new AudioStream[]
        {
            GD.Load<AudioStream>("res://assets/sounds/greet2.mp3"),
            GD.Load<AudioStream>("res://assets/sounds/greeting1.mp3")
        };

        speech = new AudioStream[]
        {
            GD.Load<AudioStream>("res://assets/sounds/speech1.mp3"),
            GD.Load<AudioStream>("res://assets/sounds/speech2.mp3"),
            GD.Load<AudioStream>("res://assets/sounds/speech3.mp3")
        };

        door = new AudioStream[]
        {
            GD.Load<AudioStream>("res://assets/sounds/door1.mp3"),
            GD.Load<AudioStream>("res://assets/sounds/door2.mp3")
        };

        gameOverSound = GD.Load<AudioStream>("res://assets/sounds/gameover.mp3");
        paperSound = GD.Load<AudioStream>("res://assets/sounds/paper.mp3");
        idPleaseSound = GD.Load<AudioStream>("res://assets/sounds/IDplease.mp3");
    }


    // ---------------------------------------
    //                   MUSIC
    // ---------------------------------------
    public void PlayMusic()
    {
        if (!_musicPlayer.Playing)
        {
            _musicPlayer.Stream = musicSound;
            _musicPlayer.VolumeDb = -14;
            _musicPlayer.Play();
        }
    }

    public void StopMusic()
    {
        _musicPlayer.Stop();
    }


    // ---------------------------------------
    //                 FOOTSTEPS
    // ---------------------------------------
    public void StartFootsteps()
    {
        if (!_footstepPlayer.Playing)
        {
            _footstepPlayer.Stream = footstepsSound;
            _footstepPlayer.PitchScale = 1.0f;
            _footstepPlayer.Play();
        }
    }

    public void StopFootsteps()
    {
        _footstepPlayer.Stop();
    }


    // ---------------------------------------
    //            GENERIC SFX FUNCTIONS
    // ---------------------------------------
    public void PlayGreeting(int index = 0)
    {
        _sfx1.Stream = greetings[index];
        _sfx1.Play();
    }

    public void PlaySpeech(int index = 0)
    {
        _sfx2.Stream = speech[index];
        _sfx2.Play();
    }

    public void PlayDoor(int index = 0)
    {
        _sfx3.Stream = door[index];
        _sfx3.Play();
    }

    public void PlayGameOver()
    {
        _sfx1.Stream = gameOverSound;
        _sfx1.Play();
    }

    public void PlayPaper()
    {
        _sfx2.Stream = paperSound;
        _sfx2.Play();
    }

    public void PlayIdPlease()
    {
        _sfx3.Stream = idPleaseSound;
        _sfx3.Play();
    }
}
