using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SokudaKun
{
    public enum keycode
    {
        LMouse = 1,
        RMouse = 2,
        CANCEL = 3,
        MMouse = 4,
        XMouse1 = 5,
        XMouse2 = 6,
        Back = 8,
        Tab = 9,
        Clear = 12,
        Return = 13,
        Shift = 16,
        Ctrl = 17,
        Alt = 18,
        Pause = 19,
        Capital = 20,
        Kana = 21,
        Junja = 23,
        Final = 24,
        Kanji = 25,
        Escape = 27,
        Convert = 28,
        NonConvert = 29,
        Accept = 30,
        ModeChange = 31,
        Space = 32,
        Prior = 33,
        Next = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        Snapshot = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        Zero = 48,
        One = 49,
        Two = 50,
        Three = 51,
        Four = 52,
        Five = 53,
        Six = 54,
        Seven = 55,
        Eight = 56,
        Nine = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LWin = 91,
        RWin = 92,
        Apps = 93,
        Sleep = 95,
        Num0 = 96,
        Num1 = 97,
        Num2 = 98,
        Num3 = 99,
        Num4 = 100,
        Num5 = 101,
        Num6 = 102,
        Num7 = 103,
        Num8 = 104,
        Num9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        Numlock = 144,
        Scroll = 145,
        LShift = 160,
        RShift = 161,
        LCtrl = 162,
        RCtrl = 163,
        LAlt = 164,
        RAlt = 165,
        FullChar = 243,
        HalfChar = 244,
        Comma = 188,
        Period = 190,
        Slash = 191,
        BackSlash = 226,
        Yen = 220,
        LBracket = 219,
        RBracket = 221,
        AtSign = 192,
        Dash = 189,
        Colon = 186,
        SemiColon = 187,
        Hiragana = 240,
        Caret = 222
    }
    public class Utils
    {
        private List<keycode> _key_buff = new List<keycode>();
        private uint _key_buff_id = 0;

        public List<keycode> GetKeysWithBuffer(uint uiKey)
        {
            keycode key = (keycode) Enum.ToObject(typeof(keycode), uiKey);
            this._key_buff.Add(key);
            this._key_buff_id++;
            Task.Run(() => {
                uint _id = (this._key_buff_id + 0);
                Thread.Sleep(400);
                if (this._key_buff_id == _id)
                {
                    this._key_buff.Clear();
                }
            });
            return this._key_buff;
        }

        public string KeysToStr(List<keycode> keys)
        {
            List<string> ret = new List<string>();
            keys.ForEach(item =>
            {
                ret.Add(item.ToString());
            });
            return string.Join("+", ret);
        }
    }

    public class Settings
    {
        private List<keycode> _start_keys;
        private List<keycode> _stop_keys;
        private uint _cycle;

        public List<keycode> StartKeys
        {
            get { return _start_keys; }
            set { _start_keys = value; }
        }
        public List<keycode> StopKeys
        {
            get { return _stop_keys; }
            set { _stop_keys = value; }
        }

        public uint Cycle
        {
            get { return _cycle; }
            set { _cycle = value; }
        }

        public Settings()
        {
            _start_keys = new List<keycode>() { keycode.F10 };
            _stop_keys = new List<keycode>() { keycode.LAlt };
            _cycle = 1000;
        }
    }
}
