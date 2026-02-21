<div align="center">

  # CS2 AUTO-PISTOL by CS1337BOYS
  **The mathematically perfect 16ms Sub-Tick Rapid-Fire Macro for Counter-Strike 2**

  [![License](https://img.shields.io/badge/License-MIT-blue.svg?style=for-the-badge&color=252525)](https://opensource.org/licenses/MIT)
  [![Steam Group](https://img.shields.io/badge/Steam-CS1337BOYS-black?logo=steam&style=for-the-badge)](https://steamcommunity.com/groups/CS1337BOYS)
</div>

<br>

Firing semi-automatic weapons like the **Tec-9**, **Dual Berettas**, or performing specialized trolling with the **R8 Revolver** at maximum speed in CS2 is historically difficult due to the new Sub-Tick architecture. If you click faster than the engine allows (under ~15.6ms), the game intentionally drops your inputs, resulting in a "jammed" gun.

**CS2 AUTO-PISTOL by CS1337BOYS** solves this mathematically. It uses absolute raw Windows Hooks (`SetWindowsHookEx`) to inject perfect `16.0ms` left-clicks directly into the OS queue while you hold your side mouse buttons. This bypasses the spam-penalty entirely, giving you the fastest theoretical fire-rate possible in the game.

---

## ⚡ Key Features

- **Anti-Jamming Sub-Tick Bypass:** Calculates a precise 16.0ms click loop (4ms hold, 12ms release), landing perfectly outside the engine's spam-penalty window. Your gun will never jam.
- **Dynamic Key Binding:** Bind ANY keyboard key or mouse button (Left/Right/Middle/M4/M5) as your hotkey.
- **Toggle vs Hold Modes:** Choose between `TOGGLE` (click once to start/stop) or `HOLD` (works only while button is pressed).
- **Hyper-Threaded Engine:** Uses `.NET GCLatencyMode.SustainedLowLatency` and `ProcessPriorityClass.High` to freeze Windows Garbage Collection and prioritize the script thread to `Highest`.
- **Zero Input Lag:** Utilizes exact CPU `SpinWait` polling instead of lazy `Thread.Sleep()`, ensuring millisecond accuracy without OS sleep jitter.
- **Premium CS2 UI:** Beautiful, compact, borderless dark-mode WinForms application with GDI+ diagonal renders replicating the official CS2 aesthetic.

## 🚀 Installation & Usage

1. **Download:** Grab the latest `AutoPistol.exe`
2. **Run:** Open `AutoPistol.exe`. (The application will safely request Administrator privileges to inject low-level mouse hooks).
3. **Configure:** Select your preferred trigger button
4. **Start:** Click `START MACRO`. 
5. **Play:** Go into CS2, buy a Tec-9, Dual Berettas, or an R8 Revolver, and hold your hotkey to melt enemies or troll with perfect fire-rate timing.

## 🛠 Compilation (For Developers)

Requires the .NET Framework 4.0 Compiler (`csc.exe`).

```cmd
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /target:winexe /win32manifest:app.manifest /out:AutoPistol.exe Program.cs
```

## 🤝 Support
If this script helped you win a pistol round, consider dropping a star ⭐ or supporting the development!

[Donate here!](https://adiru3.github.io/Donate/) | [Join our Steam Group!](https://steamcommunity.com/groups/CS1337BOYS)

> **Disclaimer:** This software simulates mouse clicks via standard `user32.dll` APIs and does not read or inject into CS2 memory (`client.dll`). Use external hardware scripts/macros at your own risk.

