using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;

namespace MacrosEngine.UKP;

public static class UKP
{

    public static void GenerateDefaultMacros(string[] macros, string path = null)
    {
        
        List<string> lines = new();
        lines.Add("//Auto generated from KeyboardCompanion");
        lines.Add("#include <Arduino.h>");
        lines.Add("#include \"macros.h\"");
        lines.Add("#define MACRO const char PROGMEM");
        lines.Add("");

        int len = 0;
        foreach (var macro in macros)
        {
            lines.Add("MACRO macro_"+(len+1)+"[] = \""+macro+"\";");
            len++;
        }
        
        lines.Add("");
        lines.Add("const char *const defaultMacros[] PROGMEM = {");

        for (int i = 0; i < macros.Length; i++)
        {
            if(i != macros.Length-1)
                lines.Add("macro_"+(i+1)+",");
            else 
                lines.Add("macro_"+(i+1));
        }
        lines.Add("};");
        lines.Add("");
        lines.Add("//this variable saves the size of the array in order to calculate default_profiles_num in macrosengine");
        lines.Add("const int dp_num = ARR_SIZE(defaultMacros);");
        File.WriteAllLines("test.h",lines);
    }
}