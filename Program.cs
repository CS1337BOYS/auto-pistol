using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CS2AutoPistol
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        // UI Colors
        private Color csLightGray = Color.FromArgb(220, 220, 220); 
        private Color csWhite = Color.White;
        private Color csOrange = Color.FromArgb(235, 120, 15); 
        private Color darkPanelColor = Color.FromArgb(25, 25, 25);
        private Color textColor = Color.White;

        // UI Controls
        private Label titleLabel;
        private Label statusLabel;
        private Button toggleMacroBtn;
        private Button bindButton;
        private ComboBox modeCombo;
        private Button btnGitHub;
        private Button btnDonate;
        private Button btnSteamGroup;
        private Button closeButton;
        private Button minimizeButton;
        
        // Dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private System.Windows.Forms.Timer uiTimer;

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true; 

            // Initialize hooks immediately so Binding works anytime
            PistolEngine.InstallHooks();

            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 100;
            uiTimer.Tick += (s, e) => {
                if (PistolEngine.IsBinding) {
                    bindButton.Text = "PRESS ANY KEY...";
                    bindButton.BackColor = Color.Gray;
                } else {
                    bindButton.Text = "BIND: " + PistolEngine.HotkeyName;
                    bindButton.BackColor = Color.FromArgb(40, 40, 40);
                }
            };
            uiTimer.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.FormBorderStyle = FormBorderStyle.None;
            this.ClientSize = new Size(340, 300); // Slightly taller for extra options
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CS2 AUTO-PISTOL by CS1337BOYS";
            this.BackColor = darkPanelColor;

            // Title Bar
            Panel topBar = new Panel();
            topBar.Height = 35;
            topBar.Dock = DockStyle.Top;
            topBar.BackColor = Color.FromArgb(20, 20, 20);
            topBar.MouseDown += TopBar_MouseDown;

            titleLabel = new Label();
            titleLabel.Text = "CS2 AUTO-PISTOL";
            titleLabel.ForeColor = csOrange;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(10, 8);
            titleLabel.MouseDown += TopBar_MouseDown; 
            topBar.Controls.Add(titleLabel);

            closeButton = new Button();
            closeButton.Text = "X";
            closeButton.ForeColor = Color.White;
            closeButton.BackColor = Color.Transparent;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Size = new Size(35, 35);
            closeButton.Dock = DockStyle.Right;
            closeButton.Cursor = Cursors.Hand;
            closeButton.Click += (s, e) => { PistolEngine.Stop(); Application.Exit(); };
            topBar.Controls.Add(closeButton);

            minimizeButton = new Button();
            minimizeButton.Text = "-";
            minimizeButton.ForeColor = Color.White;
            minimizeButton.BackColor = Color.Transparent;
            minimizeButton.FlatStyle = FlatStyle.Flat;
            minimizeButton.FlatAppearance.BorderSize = 0;
            minimizeButton.Size = new Size(35, 35);
            minimizeButton.Dock = DockStyle.Right;
            minimizeButton.Cursor = Cursors.Hand;
            minimizeButton.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            topBar.Controls.Add(minimizeButton);

            this.Controls.Add(topBar);

            // Subtitle
            Label subTitle = new Label();
            subTitle.Text = "SETTINGS";
            subTitle.ForeColor = csLightGray;
            subTitle.BackColor = Color.Transparent;
            subTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            subTitle.AutoSize = true;
            subTitle.Location = new Point(15, 50);
            this.Controls.Add(subTitle);

            // Bind button
            bindButton = new Button();
            bindButton.Text = "BIND: Mouse 5";
            bindButton.Location = new Point(20, 80);
            bindButton.Size = new Size(145, 35);
            bindButton.FlatStyle = FlatStyle.Flat;
            bindButton.ForeColor = Color.White;
            bindButton.BackColor = Color.FromArgb(40, 40, 40);
            bindButton.FlatAppearance.BorderColor = csOrange;
            bindButton.Cursor = Cursors.Hand;
            bindButton.Click += (s, e) => PistolEngine.StartBinding();
            this.Controls.Add(bindButton);

            // Mode Combo
            modeCombo = new ComboBox();
            modeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            modeCombo.Items.AddRange(new object[] { "TOGGLE", "HOLD" });
            modeCombo.SelectedIndex = 0; // Default Toggle
            modeCombo.Location = new Point(175, 83);
            modeCombo.Width = 145;
            modeCombo.BackColor = Color.FromArgb(35, 35, 35);
            modeCombo.ForeColor = Color.White;
            modeCombo.FlatStyle = FlatStyle.Flat;
            modeCombo.SelectedIndexChanged += (s, e) => PistolEngine.SetTriggerMode(modeCombo.SelectedIndex);
            this.Controls.Add(modeCombo);

            // Toggle Macro Button
            toggleMacroBtn = new Button();
            toggleMacroBtn.Text = "START MACRO";
            toggleMacroBtn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            toggleMacroBtn.BackColor = csOrange;
            toggleMacroBtn.ForeColor = Color.White;
            toggleMacroBtn.FlatStyle = FlatStyle.Flat;
            toggleMacroBtn.FlatAppearance.BorderSize = 0;
            toggleMacroBtn.Size = new Size(300, 45);
            toggleMacroBtn.Location = new Point(20, 130);
            toggleMacroBtn.Cursor = Cursors.Hand;
            toggleMacroBtn.Click += ToggleMacroBtn_Click;
            this.Controls.Add(toggleMacroBtn);

            // Status Label
            statusLabel = new Label();
            statusLabel.Text = "Status: INACTIVE";
            statusLabel.ForeColor = Color.LightGray;
            statusLabel.BackColor = Color.Transparent;
            statusLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(100, 190);
            this.Controls.Add(statusLabel);

            // Bottom Links
            int btnY = 250;
            btnGitHub = CreateLinkButton("GitHub", 20, btnY, 90, "https://github.com/CS1337BOYS/");
            btnDonate = CreateLinkButton("Donate", 120, btnY, 90, "https://adiru3.github.io/Donate/");
            btnSteamGroup = CreateLinkButton("Steam Group", 220, btnY, 100, "https://steamcommunity.com/groups/CS1337BOYS");

            this.Controls.Add(btnGitHub);
            this.Controls.Add(btnDonate);
            this.Controls.Add(btnSteamGroup);

            this.ResumeLayout(false);
        }

        private Button CreateLinkButton(string text, int x, int y, int width, string url)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.ForeColor = csOrange;
            btn.BackColor = Color.FromArgb(30, 30, 30);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = csOrange;
            btn.FlatAppearance.BorderSize = 1;
            btn.Size = new Size(width, 30);
            btn.Location = new Point(x, y);
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            btn.Click += (s, e) => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            return btn;
        }

        private void TopBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void ToggleMacroBtn_Click(object sender, EventArgs e)
        {
            if (PistolEngine.IsRunning)
            {
                PistolEngine.Stop();
                toggleMacroBtn.Text = "START MACRO";
                toggleMacroBtn.BackColor = csOrange;
                statusLabel.Text = "Status: INACTIVE";
                statusLabel.ForeColor = Color.LightGray;
            }
            else
            {
                PistolEngine.Start();
                toggleMacroBtn.Text = "STOP MACRO";
                toggleMacroBtn.BackColor = Color.IndianRed;
                statusLabel.Text = "Status: ACTIVE";
                statusLabel.ForeColor = Color.LimeGreen;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(darkPanelColor);
            int w = this.Width;
            int h = this.Height;

            Point[] whiteStripe = { new Point(220, 0), new Point(260, 0), new Point(140, h), new Point(100, h) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(15, csWhite)), whiteStripe);

            Point[] orangeArea = { new Point(300, 0), new Point(340, 0), new Point(220, h), new Point(180, h) };
            using (SolidBrush orangeBrush = new SolidBrush(Color.FromArgb(20, csOrange)))
            {
                g.FillPolygon(orangeBrush, orangeArea);
            }
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, csOrange, ButtonBorderStyle.Solid);
        }
    }

    public static class PistolEngine
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WH_MOUSE_LL = 14;

        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_RBUTTONUP = 0x0205;
        const int WM_MBUTTONDOWN = 0x0207;
        const int WM_MBUTTONUP = 0x0208;
        const int WM_XBUTTONDOWN = 0x020B;
        const int WM_XBUTTONUP = 0x020C;

        [StructLayout(LayoutKind.Sequential)]
        struct MSLLHOOKSTRUCT
        {
            public Point pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public int type;
            public InputUnion u;
        }
        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT { public int dx; public int dy; public uint mouseData; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }

        const int INPUT_MOUSE = 0;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        static IntPtr _khookID = IntPtr.Zero;
        static IntPtr _mhookID = IntPtr.Zero;
        static LowLevelProc _kproc = KeyboardHookCallback;
        static LowLevelProc _mproc = MouseHookCallback;

        public static volatile bool IsRunning = false;
        public static volatile bool IsBinding = false;
        
        // Settings
        public enum KeyType { Keyboard, Mouse, XMouse }
        public enum TriggerMode { Toggle, Hold }
        
        static KeyType selectedKeyType = KeyType.XMouse;
        static int selectedKeyCode = 2; // Mouse 5 (XButton 2)
        static TriggerMode currentMode = TriggerMode.Toggle;
        public static string HotkeyName = "Mouse 5";

        static volatile bool isHotkeyHeld = false;
        static volatile bool isToggledOn = false;

        private static Thread _macroThread;

        public static void SetTriggerMode(int index) { currentMode = (TriggerMode)index; }

        public static void StartBinding() 
        { 
            InstallHooks(); // Ensure hooks are on
            IsBinding = true; 
        }

        public static void InstallHooks()
        {
            if (_khookID == IntPtr.Zero) _khookID = SetHook(WH_KEYBOARD_LL, _kproc);
            if (_mhookID == IntPtr.Zero) _mhookID = SetHook(WH_MOUSE_LL, _mproc);
        }

        public static void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            isToggledOn = false;
            isHotkeyHeld = false;

            try {
                using (Process p = Process.GetCurrentProcess()) { p.PriorityClass = ProcessPriorityClass.High; }
                System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.SustainedLowLatency;
            } catch { }

            _macroThread = new Thread(Loop);
            _macroThread.Priority = ThreadPriority.Highest;
            _macroThread.IsBackground = true;
            _macroThread.Start();

            // Hooks now managed globally
            InstallHooks();
        }

        public static void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            
            if (_khookID != IntPtr.Zero) { UnhookWindowsHookEx(_khookID); _khookID = IntPtr.Zero; }
            if (_mhookID != IntPtr.Zero) { UnhookWindowsHookEx(_mhookID); _mhookID = IntPtr.Zero; }
            try { System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Interactive; } catch { }
        }

        private static IntPtr SetHook(int id, LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(id, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int msg = wParam.ToInt32();
                var kStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                if (IsBinding && msg == WM_KEYDOWN)
                {
                    selectedKeyType = KeyType.Keyboard;
                    selectedKeyCode = (int)kStruct.vkCode;
                    HotkeyName = ((Keys)selectedKeyCode).ToString();
                    IsBinding = false;
                    return (IntPtr)1; // Consume input
                }

                if (IsRunning && selectedKeyType == KeyType.Keyboard && (int)kStruct.vkCode == selectedKeyCode)
                {
                    if (msg == WM_KEYDOWN) 
                    {
                        if (!isHotkeyHeld) // Avoid repeat
                        {
                            isHotkeyHeld = true;
                            if (currentMode == TriggerMode.Toggle) isToggledOn = !isToggledOn;
                        }
                    }
                    else if (msg == WM_KEYUP) 
                    {
                        isHotkeyHeld = false;
                    }
                }
            }
            return CallNextHookEx(_khookID, nCode, wParam, lParam);
        }

        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int msg = wParam.ToInt32();
                var mStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                if (IsBinding)
                {
                    if (msg == WM_LBUTTONDOWN) { BindMouse(KeyType.Mouse, 1, "Left Click"); return (IntPtr)1; }
                    if (msg == WM_RBUTTONDOWN) { BindMouse(KeyType.Mouse, 2, "Right Click"); return (IntPtr)1; }
                    if (msg == WM_MBUTTONDOWN) { BindMouse(KeyType.Mouse, 3, "Middle Click"); return (IntPtr)1; }
                    if (msg == WM_XBUTTONDOWN) 
                    {
                        int xBtn = (int)(mStruct.mouseData >> 16);
                        BindMouse(KeyType.XMouse, xBtn, "Mouse " + (xBtn + 3));
                        return (IntPtr)1;
                    }
                }

                if (IsRunning)
                {
                    bool relevant = false;
                    bool down = false;

                    if (selectedKeyType == KeyType.Mouse)
                    {
                        if (selectedKeyCode == 1 && (msg == WM_LBUTTONDOWN || msg == WM_LBUTTONUP)) { relevant = true; down = msg == WM_LBUTTONDOWN; }
                        if (selectedKeyCode == 2 && (msg == WM_RBUTTONDOWN || msg == WM_RBUTTONUP)) { relevant = true; down = msg == WM_RBUTTONDOWN; }
                        if (selectedKeyCode == 3 && (msg == WM_MBUTTONDOWN || msg == WM_MBUTTONUP)) { relevant = true; down = msg == WM_MBUTTONDOWN; }
                    }
                    else if (selectedKeyType == KeyType.XMouse && (msg == WM_XBUTTONDOWN || msg == WM_XBUTTONUP))
                    {
                        int xBtn = (int)(mStruct.mouseData >> 16);
                        if (xBtn == selectedKeyCode) { relevant = true; down = msg == WM_XBUTTONDOWN; }
                    }

                    if (relevant)
                    {
                        if (down)
                        {
                            if (!isHotkeyHeld)
                            {
                                isHotkeyHeld = true;
                                if (currentMode == TriggerMode.Toggle) isToggledOn = !isToggledOn;
                            }
                        }
                        else
                        {
                            isHotkeyHeld = false;
                        }
                    }
                }
            }
            return CallNextHookEx(_mhookID, nCode, wParam, lParam);
        }

        private static void BindMouse(KeyType type, int code, string name)
        {
            selectedKeyType = type;
            selectedKeyCode = code;
            HotkeyName = name;
            IsBinding = false;
        }

        private static void Loop()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            double clickHoldTimeMs = 2.0;
            double clickReleaseWaitMs = 14.0; 

            while (IsRunning)
            {
                bool active = (currentMode == TriggerMode.Hold) ? isHotkeyHeld : isToggledOn;

                if (active)
                {
                    SendMouseClick(true);
                    SpinWait(clickHoldTimeMs, sw);
                    SendMouseClick(false);
                    SpinWait(clickReleaseWaitMs, sw);
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        private static void SpinWait(double milliseconds, Stopwatch sw)
        {
            long start = sw.ElapsedTicks;
            long ticksToWait = (long)(milliseconds * Stopwatch.Frequency / 1000.0);
            while (sw.ElapsedTicks - start < ticksToWait) { }
        }

        private static void SendMouseClick(bool isDown)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi.dx = 0;
            inputs[0].u.mi.dy = 0;
            inputs[0].u.mi.mouseData = 0;
            inputs[0].u.mi.dwFlags = isDown ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP;
            inputs[0].u.mi.time = 0;
            inputs[0].u.mi.dwExtraInfo = IntPtr.Zero;

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}
