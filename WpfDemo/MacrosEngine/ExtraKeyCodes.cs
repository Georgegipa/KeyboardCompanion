using System;
using System.Collections.Generic;
using InputSimulatorStandard.Native;

namespace MacrosEngine.Codes
{
    public static class ExtraKeyCodes
    {
        private static readonly Dictionary<string, VirtualKeyCode> keys = new Dictionary<string, VirtualKeyCode>
        {
            {"CTRL", VirtualKeyCode.CONTROL},
            {"SHIFT", VirtualKeyCode.SHIFT},
            {"ALT", VirtualKeyCode.MENU},
            {"WIN", VirtualKeyCode.LWIN},
            {"UP", VirtualKeyCode.UP},
            {"DOWN", VirtualKeyCode.DOWN},
            {"LEFT", VirtualKeyCode.LEFT},
            {"RIGHT", VirtualKeyCode.RIGHT},
            {"BACKSPACE", VirtualKeyCode.BACK},
            {"TAB", VirtualKeyCode.TAB},
            {"RETURN", VirtualKeyCode.RETURN},
            {"ESC", VirtualKeyCode.ESCAPE},
            {"INSERT", VirtualKeyCode.INSERT},
            {"DELETE", VirtualKeyCode.DELETE},
            {"PUP", VirtualKeyCode.PRIOR},
            {"PDOWN", VirtualKeyCode.NEXT},
            {"HOME", VirtualKeyCode.HOME},
            {"END", VirtualKeyCode.END},
            {"CAPS", VirtualKeyCode.CAPITAL},
            {"F1", VirtualKeyCode.F1},
            {"F2", VirtualKeyCode.F2},
            {"F3", VirtualKeyCode.F3},
            {"F4", VirtualKeyCode.F4},
            {"F5", VirtualKeyCode.F5},
            {"F6", VirtualKeyCode.F6},
            {"F7", VirtualKeyCode.F7},
            {"F8", VirtualKeyCode.F8},
            {"F9", VirtualKeyCode.F9},
            {"F10", VirtualKeyCode.F10},
            {"F11", VirtualKeyCode.F11},
            {"F12", VirtualKeyCode.F12},
            {"F13", VirtualKeyCode.F13},
            {"F14", VirtualKeyCode.F14},
            {"F15", VirtualKeyCode.F15},
            {"F16", VirtualKeyCode.F16},
            {"F17", VirtualKeyCode.F17},
            {"F18", VirtualKeyCode.F18},
            {"F19", VirtualKeyCode.F19},
            {"F20", VirtualKeyCode.F20},
            {"F21", VirtualKeyCode.F21},
            {"F22", VirtualKeyCode.F22},
            {"F23", VirtualKeyCode.F23},
            {"F24", VirtualKeyCode.F24}
        };

        public static VirtualKeyCode FindKey(string key)
        {
            return key.Contains(key) ? keys[key] : (VirtualKeyCode) 0;
        }

        public static VirtualKeyCode CharToVirtualKeyCode(char c)
        {
            if (char.IsLetter(c))
            {
                if (char.IsLower(c))
                    return (VirtualKeyCode) char.ToUpper(c);
                else
                    return (VirtualKeyCode)c;
            }
            else if (char.IsNumber(c))
                return (VirtualKeyCode) c;
            else if (char.IsSeparator(c))
                return VirtualKeyCode.SPACE;
            else if (char.IsSymbol(c))
            {
                return c switch
                {
                    '`' => VirtualKeyCode.OEM_3,
                    '-' => VirtualKeyCode.OEM_MINUS,
                    '=' => VirtualKeyCode.OEM_PLUS,
                    '[' => VirtualKeyCode.OEM_4,
                    ']' => VirtualKeyCode.OEM_6,
                    ';' => VirtualKeyCode.OEM_1,
                    '\'' => VirtualKeyCode.OEM_7,
                    '\\' => VirtualKeyCode.OEM_5,
                    ',' => VirtualKeyCode.OEM_COMMA,
                    '.' => VirtualKeyCode.OEM_PERIOD,
                    '/' => VirtualKeyCode.OEM_2,
                };
            }
            return (VirtualKeyCode) 0;
        }

        public static char RemapSecondaryKeys(char c)
        {
            //press shift
            return c switch
            {
                '~' => '`',
                '!' => '1',
                '@' => '2',
                '#' => '3',
                '$' => '4',
                '%' => '5',
                '^' => '6',
                '&' => '7',
                '*' => '8',
                '(' => '9',
                ')' => '0',
                '_' => '-',
                '+' => '=',
                '{' => '[',
                '}' => ']',
                ':' => ';',
                '"' => '\'',
                '|' => '\\',
                '<' => ',',
                '>' => '.',
                '?' => '/',
                _ => (char) 0 //null
            };
        }

        public static VirtualKeyCode StringToKey(string inputKey,bool shiftkeys = false)
        {
            VirtualKeyCode temp;
            if (inputKey.Length == 1)
            {//convert char to keyCode
                temp = CharToVirtualKeyCode(Convert.ToChar(inputKey));
                if (shiftkeys && temp==0)
                {
                    temp = CharToVirtualKeyCode(RemapSecondaryKeys(Convert.ToChar(inputKey)));
                }
            }
            else
            {
                temp = FindKey(inputKey);
            }

            if (temp == 0)
                throw new Exception("KeyNotFound");
            return temp;
        }

    }
}