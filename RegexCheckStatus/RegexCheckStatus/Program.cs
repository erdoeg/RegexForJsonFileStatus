using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexCheckStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length == 0 || args.Length > 1) || !args[0].EndsWith("json"))
            {
                System.Console.WriteLine("*************** Invalid Parameter. ***************");
                System.Console.WriteLine("*** Please enter the path to check .json file. ***");
            }
            else
            {
                try
                {

                    var pathJson = Path.GetFullPath(args[0]);
                    string jsonData = File.ReadAllText(pathJson);
                    Console.WriteLine(jsonData);

                    string pattern = @"(""statusSignature"":) (""\w*"")";
                    RegexOptions options = RegexOptions.Multiline;
                    bool isNone = false;
                    bool isSigned = false;

                    foreach (Match element in Regex.Matches(jsonData, pattern, options))
                    {
                        var elmSign = element.Value.Split(':')[1].Split('"')[1];

                        Console.WriteLine("'{0}' found at index {1}.", element.Value, element.Index);
                        Console.WriteLine("\n**'{0}'** -- Value length {1}.\n", elmSign, elmSign.Length);
                        if (elmSign.Length > 0)
                        {
                            isSigned = true;
                        }
                        else
                        {
                            isNone = true;
                        }
                    }

                    Console.WriteLine("------------ Contract Status ------------");

                    if (isNone && isSigned)
                    {
                        Console.WriteLine("\n-------- *** PARTALLY SIGNED *** --------");
                    }
                    else if (isNone && !isSigned)
                    {
                        Console.WriteLine("\n----------- *** UNSIGNED *** ------------");
                    }
                    else
                    {
                        Console.WriteLine("\n--------- *** FULLY SIGNED *** ----------\n");
                    }

                }                
                catch (Exception e)                {
                    Console.WriteLine("Error -> {0}", e);
                    throw;
                }
            }
        }
    }
}
