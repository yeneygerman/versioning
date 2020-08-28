/* Developed by https://github.com/yeneygerman */
using System;
using System.IO;

namespace increment_version
{
    class Program
    {
        // Current versioning is A(Major version)-B(Minor version)-C(Patch)
        // A can go beyond 100
        // Both B and C will increment up to 100 only then back to 1 if it reaches 100
        static void Main(string[] args)
        {
            // Command line arguments
            // [0] -file
            // [1] filepath
            // [2] -version
            // [3] version string to be searched
            // [4] -incrementtype
            // [5] increment type (it could be major, minor, or patch. Patch is default)

            try
            {
                string line = string.Empty, file = string.Empty, toBeSearched = string.Empty, incrementType = "patch";

                //file = "D:\\SampleProject\\sample-cloudbuild.yaml";
                //toBeSearched = "project-service:live-";

                if (args.Length > 0)
                {
                    if (args[0] == "-file" && !string.IsNullOrEmpty(args[1]))
                    {
                        file = args[1];
                    }

                    if (args[2] == "-version" && !string.IsNullOrEmpty(args[3]))
                    {
                        toBeSearched = args[3];
                    }

                    if (args.Length > 4)
                    {
                        if (args[4] == "-incrementtype" && !string.IsNullOrEmpty(args[5]))
                        {
                            var tempType = args[5].ToLower();

                            if (tempType == "major" || tempType == "minor")
                            {
                                incrementType = tempType;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(toBeSearched))
                    {
                        StreamReader myFile = new StreamReader(file);

                        bool firstLoop = false;

                        string newVersion = string.Empty;
                        string oldVersion = string.Empty;

                        int lineCount = 1;
                        int totalReplacedLines = 0;

                        while ((line = myFile.ReadLine()) != null)
                        {
                            if (line.Contains(toBeSearched))
                            {
                                int pFrom = line.IndexOf(toBeSearched);
                                int pTo = line.LastIndexOf("'");
                                int pToto = line.LastIndexOf(",\"");

                                if (pFrom != -1)
                                {
                                    var combined = pFrom + (toBeSearched.Length);
                                    string code = line.Substring(combined);

                                    code = pToto != -1 ? code.GetUntilOrEmpty(",\"") : code.GetUntilOrEmpty();

                                    var codeD = code.Split("-");

                                    if (codeD != null && firstLoop == false)
                                    {
                                        var inclusiveArray = codeD.Length - 1;
                                        var prevNum = 0;

                                        oldVersion = toBeSearched + code;

                                        Console.WriteLine("Current version: " + oldVersion);

                                        if (incrementType == "patch")
                                        {
                                            for (var i = inclusiveArray; i > -1; i--)
                                            {
                                                var parsed = int.Parse(codeD[i]);

                                                if (parsed < 100 && prevNum == 0 || parsed < 100 && prevNum == 100 || i == 0 && prevNum == 100)
                                                {
                                                    prevNum = parsed;

                                                    var newV = parsed + 1;
                                                    codeD[i] = newV.ToString();
                                                }
                                                else if (parsed == 100 && i != 0 && prevNum == 100 || i == inclusiveArray && parsed == 100)
                                                {
                                                    prevNum = parsed;

                                                    codeD[i] = "1";
                                                }
                                            }
                                        }
                                        else if (incrementType == "minor")
                                        {
                                            var parsed = int.Parse(codeD[1]);

                                            if (parsed < 100)
                                            {
                                                var newV = parsed + 1;
                                                codeD[1] = newV.ToString(); // Increase current minor version
                                            }
                                            else
                                            {
                                                codeD[1] = "1"; // Reset minor to 1
                                                var newV = codeD[0] + 1;
                                                codeD[0] = newV.ToString(); // Increase current major version
                                            }

                                            codeD[2] = "1"; // Reset patch to 1
                                        }
                                        else if (incrementType == "major")
                                        {
                                            var parsed = int.Parse(codeD[0]);

                                            var newV = parsed + 1;
                                            codeD[0] = newV.ToString(); // Increase current major version
                                            codeD[1] = "1"; // Reset minor to 1
                                            codeD[2] = "1"; // Reset patch to 1
                                        }

                                        firstLoop = true;
                                        newVersion = string.Format("{0}-{1}-{2}", codeD[0], codeD[1], codeD[2]);

                                        newVersion = toBeSearched + newVersion;
                                    }

                                    totalReplacedLines++;
                                    Console.WriteLine("Replaced line[" + lineCount + "]: " + newVersion);
                                }
                            }

                            lineCount++;
                        }

                        myFile.Close();

                        if (!string.IsNullOrEmpty(oldVersion) && !string.IsNullOrEmpty(newVersion))
                        {
                            string str = File.ReadAllText(file);
                            str = str.Replace(oldVersion, newVersion);
                            File.WriteAllText(file, str);

                            Console.WriteLine("Total of lines replaced: " + totalReplacedLines + ". Version incremented successfully from " + oldVersion + " to " + newVersion + "!");

                            return;
                        }

                        Console.WriteLine("Failed to increment version.");

                        return;
                    }
                }

                Console.WriteLine("Please provide line arguments. Sample argument: -file <filepath> -version <version to be searched>");

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex.Message);
            }
        }
    }

    static class Helper
    {
        public static string GetUntilOrEmpty(this string text, string stopAt = "'")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
    }
}
