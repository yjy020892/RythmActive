using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums_Game;
using Enums_Common;

namespace DATA
{
    public struct UIValue
    {
        public float _Speed;
        public float _MaxValue;
    }

    public struct Stamp
    {
        public Sprite[] _StampSprites;
    }

    public struct NoteData
    {
        public float _PosiX;
        public float _PosiY;
        public float _NoteTime;
        public NoteBody _NoteBody;
    }

    public struct SongData
    {
        // SheetInfo
        public string _AudioFileName;
        public int _BPM;
        public int _Offset;

        // ContentInfo
        public string _Artist;
        public string _SongName;
        public SongDifficult _Difficult;

        // Game
        public float _SPB4;
        public float _SPB8;
        public float _SPB16;
        public float _SPB32;

        //public int _Perfect;
        //public int _Good;
        //public int _Miss;
        public int _Combo;
        public int _Score;
        public int _Rank;
        public bool _FullCombo;
    }

    public struct Pay
    {
        public string _Port;
    }

    public struct GameData
    {
        public int _Silver;
        public int _Gold;
        public int _Diamond;
        public int _Ruby;
        public int _Life;

        public int _Goal; // (0 : New, 1 : Silver, 2 : Gold, 3 : Diamond, 4 : Ruby)
        public int _SelectTime;
        public int _WarningTime;
        public int _EndTime;
    }
}