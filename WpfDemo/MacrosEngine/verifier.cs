namespace MacrosEngine.Verifier
{
    using System;
    using MacrosEngine.Codes;
    using System.IO;
    using System.Collections.Generic;

    public static class Verifier
    {
        enum MacroCommandType
        {
            MACRO_NOT_FOUND = 0,
            DEFAULT_MACRO,
            WINDOWS_EXECUTE,
            PRINT
        }

        enum CharType
        {
            CHAR_NOT_SUPPORTED,
            LOW_LETTER,
            SECONDARY_KEY,
            CHAR_ACCEPTED
        }

        public enum MacroErrors
        {
            CHAR_NOT_ACCEPTED,
            MODIFIER_KEY_NOT_FOUND,
            MACROCOMMAND_NOT_FOUND,
            MACRO_MISFORMED,
            //Warnings:
            LOW_LETTER,
            SECONDARY_KEY,
            STARTING_WITH_DELIMETER,
            ENDING_WITH_DELIMETER
        }

        private static MacroCommandType MacroType(string line)
        {
            char[] macroCommand = line[..2].ToCharArray();
            if (macroCommand[1] == MacroRegex.MacroCommand) //check the available macrocommands
                return macroCommand[0] switch
                {
                    'W' => MacroCommandType.WINDOWS_EXECUTE,
                    'P' => MacroCommandType.PRINT,
                    _ => MacroCommandType.MACRO_NOT_FOUND //macrocommand not found return error
                };

            return MacroCommandType.DEFAULT_MACRO; //macrocommands isn't used then is a normal macro
        }

        private static CharType CheckLetter(char letter)
        {
            if (ExtraKeyCodes.CharToVirtualKeyCode(letter) == 0)
            {
                //check if it is a secondary key
                if (ExtraKeyCodes.RemapSecondaryKeys(letter) == 0)
                {
                    return CharType.CHAR_NOT_SUPPORTED; //char that cannot be remapped
                }
                else return CharType.SECONDARY_KEY; //secondary keys should use shift
            }
            else if (char.IsLower(letter))
                return CharType.LOW_LETTER;
            else return CharType.CHAR_ACCEPTED; //char is correctly formed
        }

        public static string DisplayErrorCodes(MacroErrors e)
        {
            string errorType = "";
            if (e <= MacroErrors.LOW_LETTER)
                errorType = "ERROR:";
            else
                errorType = "WARNING:";

            return e switch
            {
                MacroErrors.CHAR_NOT_ACCEPTED => errorType + "Char is not acceptable",
                MacroErrors.MODIFIER_KEY_NOT_FOUND => errorType + "Modifier Key Not found",
                MacroErrors.MACROCOMMAND_NOT_FOUND => errorType + "MacroCommand Not found",
                MacroErrors.MACRO_MISFORMED => errorType + "Macro is misformed",
                MacroErrors.LOW_LETTER => errorType + "Capital Letters should be preferred ",
                MacroErrors.SECONDARY_KEY => errorType + "Secondary Key detected",
                MacroErrors.STARTING_WITH_DELIMETER => errorType + "Macro starts with delimeter:" + MacroRegex.Macro,
                MacroErrors.ENDING_WITH_DELIMETER => errorType + "Macro ends with delimeter:" + MacroRegex.Macro,
                _ => throw new ArgumentOutOfRangeException(nameof(e), e, null)
            };
        }

        public static List<MacroErrors> CheckMacro(string line)
        {
            List<MacroErrors> errors = new();
            if (line.StartsWith(MacroRegex.Macro))
            {
                errors.Add(MacroErrors.STARTING_WITH_DELIMETER);
            }
            else if (line.EndsWith(MacroRegex.Macro))
            {
                errors.Add(MacroErrors.ENDING_WITH_DELIMETER);
            }

            switch (MacroType(line))
            {
                case MacroCommandType.DEFAULT_MACRO:
                    string[] keys = line.Split(MacroRegex.Macro);
                    foreach (var key in keys)
                    {
                        if (key.Length == 1) //char
                        {
                            CharType type = CheckLetter(Convert.ToChar(key));
                            switch (type)
                            {
                                case CharType.LOW_LETTER:
                                    errors.Add(MacroErrors.LOW_LETTER);
                                    break;
                                case CharType.SECONDARY_KEY:
                                    errors.Add(MacroErrors.SECONDARY_KEY);
                                    break;
                                case CharType.CHAR_NOT_SUPPORTED:
                                    errors.Add(MacroErrors.CHAR_NOT_ACCEPTED);
                                    break;
                            }
                        }
                        else if(key.Length > 1) //check modifier keys
                        {
                            if (ExtraKeyCodes.FindKey(key) == 0)
                            {
                                errors.Add(MacroErrors.MODIFIER_KEY_NOT_FOUND);
                            }
                        }
                    }

                    break;
                case MacroCommandType.MACRO_NOT_FOUND:
                    errors.Add(MacroErrors.MACROCOMMAND_NOT_FOUND);
                    break;
                //Print and Windows Execute can have any string 
                case MacroCommandType.WINDOWS_EXECUTE:
                    break;
                case MacroCommandType.PRINT:
                    break;
            }

            return errors;
        }

        public static Dictionary<int, List<MacroErrors>> VerifyFile(string path, bool Verbose = false)
        {
            Dictionary<int, List<MacroErrors>> errors = new();
            string[] macros = File.ReadAllLines(path);
            int line = 0;
            foreach (var macro in macros)
            {
                List<MacroErrors> err = CheckMacro(macro);
                if (Verbose)
                {
                    Console.Write((line+1)+"." + macro);
                    if (err.Count != 0)
                    {
                        foreach (var error in err)
                        {
                            Console.WriteLine("\n>" + DisplayErrorCodes(error));
                        }
                    }
                    else Console.WriteLine("-Correct");
                }

                errors.Add(line, err);
                line++;
            }
            return errors;
        }
    }
}