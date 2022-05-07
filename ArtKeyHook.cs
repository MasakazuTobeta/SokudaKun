using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace SokudaKun
{
    // Reference https://www.artistics.co.jp/blog/2019/06/1483/
    class ArtKeyHook
    {
        //構造定義
        [Flags]
        private enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }

        [StructLayout(LayoutKind.Sequential)]
        private class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        //関数定義
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(
            int hookType,
            HookHandler hookDelegate,
            IntPtr module,
            uint threadId);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(
            IntPtr hook,
            int code,
            IntPtr message,
            IntPtr state);

        private delegate int HookHandler(int code, IntPtr message, IntPtr state);

        //キーイベント
        private HookHandler m_EventKey;
        private IntPtr m_Handle;

        int EventKey(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KBDLLHOOKSTRUCT oKB = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            if (oKB.flags == KBDLLHOOKSTRUCTFlags.LLKHF_UP)
            {
                Console.WriteLine(oKB.flags + "    " + oKB.vkCode);
                m_KeyEventDelegate(oKB.vkCode);
            }

            return CallNextHookEx(m_Handle, nCode, wParam, lParam);
        }

        //-------------------------------------
        //-------------------------------------
        //-------------------------------------

        //インスタンスの取得
        private static ArtKeyHook m_Instance = null;

        public static ArtKeyHook GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new ArtKeyHook();
            }

            return m_Instance;
        }

        //キーイベントの開始
        public delegate void KeyEventDelegate(uint uiKey);
        private KeyEventDelegate m_KeyEventDelegate = null;

        public void StartKeyEvent(KeyEventDelegate oKeyEventDelegate)
        {
            IntPtr hInstance = Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]);

            // KEYBOARD_LL = 13;
            m_KeyEventDelegate = oKeyEventDelegate;
            m_EventKey = new HookHandler(EventKey);
            m_Handle = SetWindowsHookEx(13, m_EventKey, hInstance, 0);

            if (m_Handle == IntPtr.Zero)
            {
                Console.WriteLine("ERROR ArtKeyHook");
            }
        }

    }
}
