using System;
using System.Collections.Generic;
using System.Threading;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using MacrosEngine.Codes;

namespace MacrosEngine
{
    public class Engine
    {
        private InputSimulator MK;
        private List<VirtualKeyCode> _keysPressed = new();
        private int SelectedProfile { get; set; } //currently, selected profile
        public int ProfileCount { get; private set; } //number of profiles
        public int ButtonCount { get; private set; }

        private readonly string[] defaultBindings ={
            "CTRL+C",
            "CTRL+V",
            "CTRL+Z",
            "CTRL+Y",
            "ALT+SHIFT+A",
            "CTRL+SHIFT+P",
            "W,notepad",
            "P,Hello from c#"
        };

        public Engine(int buttons)
        {
            MK = new InputSimulator();
        }

        ~Engine()
        {
            //release all the keys to avoid being clicked after program execution
            foreach (VirtualKeyCode key in Enum.GetValues(typeof(VirtualKeyCode)))
            {
                MK.Keyboard.KeyUp(key);
            }
        }

        int FindMacroID(int profile_id, int button_id)
        {
            return (ButtonCount - 1) * profile_id + button_id;
        }

        private void Press(VirtualKeyCode k)
        {
            _keysPressed.Add(k);
            MK.Keyboard.KeyDown(k);
        }

        private void PrintText(string text,int printdelay=10)
        {
            Thread.Sleep(printdelay);
            MK.Keyboard.TextEntry(text);
        }

        private void ReleaseAll()
        {
            Console.WriteLine("List:");
            foreach (var key in _keysPressed)
            {
                MK.Keyboard.KeyUp(key);
                Console.WriteLine(key);
            }

            _keysPressed.Clear();
        }

        private void ExecuteMacro(string macro)
        {
            string[] keys = macro.Split(MacroRegex.Macro);
            foreach (var key in keys)
            {
                Press(ExtraKeyCodes.StringToKey(key));
            }
            ReleaseAll();
        }

        private void ChangeProfile()
        {
            if (SelectedProfile < SelectedProfile - 1)
                SelectedProfile++;
            else
                SelectedProfile = 0;
        }

        public void ButtonAction(int buttonId)
        {
            if (buttonId == 0)
                ChangeProfile();
            else
            {
                string macro = defaultBindings[FindMacroID(SelectedProfile,buttonId)];
                char[] macroCommand = macro[..2].ToCharArray();//same as Substring(0,2)
                if(macroCommand[1]==MacroRegex.MacroCommand)
                {
                    macro = macro[2..];//macro start from position 2 of String (same as Substring(2))
                    switch (macroCommand[0]){
                        case 'W':
                            Press(VirtualKeyCode.LWIN);//windows key
                            Press(VirtualKeyCode.VK_R);
                            ReleaseAll();
                            Console.WriteLine(macro);
                            PrintText(macro);
                            Press(VirtualKeyCode.RETURN);
                            ReleaseAll();
                            break;
                        case 'P':
                            PrintText(macro);
                            break;
                    }
                }
                else ExecuteMacro(macro);
            }
        }
    }
}