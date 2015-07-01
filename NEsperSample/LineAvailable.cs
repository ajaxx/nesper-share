using System;
using System.IO;
using System.Text;

namespace NEsperSample
{
    class LineAvailable
    {
        public static void ReadLines(string sourceFile, Action<string> lineEvent)
        {
            using (var sr = new StreamReader(sourceFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lineEvent(line);
                }
            }
        }

        public static void ReadParts(string sourceFile, Action<string[]> partsEvent)
        {
            var space = new char[] { ' ' };
            ReadLines(sourceFile, line => partsEvent(line.Split(space)));
        }

        public static void ReadEvents(string sourceFile, Action<TotalEvent> routeEvent)
        {
            ReadParts(sourceFile, temp =>
            {
                try
                {
                    int days;
                    string desIp = "";
                    int size = temp.Length;
                    TotalEvent ev;

                    if (temp[1] != "")
                    {
                        days = Int32.Parse(temp[1]);
                        if (temp[6] == "ET")
                        {
                            desIp = temp[size - 2];
                        }
                        else
                        {
                            if (temp[5] == "last")
                                desIp = "";
                            else
                                desIp = temp[size - 1];
                        }
                        for (var m = size - 1; m > 1; m--)
                        {
                            temp[m] = temp[m - 1];
                        }
                        temp[1] = "";

                    }
                    else
                    {
                        days = Int32.Parse(temp[2]);
                        if (temp[7] == "ET")
                        {
                            desIp = temp[size - 3];
                        }
                        else
                        {
                            if (temp[6] != "last")
                                desIp = temp[size - 1];
                            else
                                desIp = "";
                        }
                    }

                    if (temp[6] == "last") //Branch A
                    {
                        ev = BranchA(temp, days);
                    }
                    else // Branch B
                    {
                        if (temp[7] == "(http_inspect)") //Branch 1
                        {
                            ev = BranchB1(temp, days, desIp);
                        }
                        else if (temp[7] == "ET") //Branch B-3 
                        {
                            ev = BranchB3(temp, days, desIp);
                        }
                        else // Branch B-2
                        {
                            ev = BranchB2(temp, days, desIp);
                        }
                    }

                    if (routeEvent != null)
                        routeEvent(ev);
                }
                catch (FormatException)
                {
                    // ignore lines that are not properly formatted (e.g. headers)
                }
            });
        }

        private static TotalEvent BranchA(string[] temp, int days)
        {
            return new TotalEvent()
            {
                Month = temp[0],
                Day = days,
                Time = temp[3],
                Ids = temp[4],
                Cheetah = temp[5],
                Info = temp[6] + " " + temp[7] + " " + temp[8] + " " + temp[9] + " " + "times"
            };
        }

        private static TotalEvent BranchB1(string[] temp, int days, string desIp)
        {
            if (temp[8] == "NO") // Branch 1-a
            {
                return new TotalEvent()
                {
                    Month = temp[0],
                    Day = days,
                    Time = temp[3],
                    Ids = temp[4],
                    Cheetah = temp[5],
                    Numbers = temp[6],
                    HttpInspect = temp[7],
                    Message =
                        temp[8] + " " + temp[9] + " " + temp[10] + " " + temp[11] + " " + temp[12] +
                        " " +
                        temp[13] + " " + temp[14] + " ",
                    Periority = temp[15] + temp[16],
                    TCP = temp[17],
                    SourceIP = temp[18],
                    Way = temp[19],
                    DestinationIP = desIp
                };
            }
            else //Branch 1-b-c-d                        
            {
                return new TotalEvent()
                {
                    Month = temp[0],
                    Day = days,
                    Time = temp[3],
                    Ids = temp[4],
                    Cheetah = temp[5],
                    Numbers = temp[6],
                    HttpInspect = temp[7],
                    Message = temp[8] + " " + temp[9],
                    Periority = temp[10] + temp[11],
                    TCP = temp[12],
                    SourceIP = temp[13],
                    Way = temp[14],
                    DestinationIP = desIp
                };
            }
        }

        private static TotalEvent BranchB2(string[] temp, int days, string desIp)
        {
            if (temp[7] == "Reset") //branch B-2-a
            {
                return new TotalEvent()
                {
                    Month = temp[0],
                    Day = days,
                    Time = temp[3],
                    Ids = temp[4],
                    Cheetah = temp[5],
                    Numbers = temp[6],
                    Info = temp[7] + " " + temp[8] + " " + temp[9],
                    Periority = temp[10] + temp[11],
                    TCP = temp[12],
                    SourceIP = temp[13],
                    Way = temp[14],
                    DestinationIP = desIp
                };
            }
            else if (temp[7] == "TCP") //branch B-2-b,c
            {
                if (temp[10] == "outside")
                {
                    return new TotalEvent()
                    {
                        Month = temp[0],
                        Day = days,
                        Time = temp[3],
                        Ids = temp[4],
                        Cheetah = temp[5],
                        Numbers = temp[6],
                        Info =
                            temp[7] + " " + temp[8] + " " + temp[9] + " " + temp[10] + " " +
                            temp[11] + " " + temp[12] + " " + temp[13],
                        Periority = temp[14] + temp[15],
                        TCP = temp[16],
                        SourceIP = temp[17],
                        Way = temp[18],
                        DestinationIP = desIp
                    };
                }
                else
                {
                    return new TotalEvent()
                    {
                        Month = temp[0],
                        Day = days,
                        Time = temp[3],
                        Ids = temp[4],
                        Cheetah = temp[5],
                        Numbers = temp[6],
                        Info = temp[7] + " " + temp[8] + " " + temp[9] + " " + temp[10],
                        Periority = temp[11] + temp[12],
                        TCP = temp[13],
                        SourceIP = temp[14],
                        Way = temp[15],
                        DestinationIP = desIp
                    };
                }
            }
            else if (temp[7] == "Bad") //branch B-2-b,c
            {
                return new TotalEvent()
                {
                    Month = temp[0],
                    Day = days,
                    Time = temp[3],
                    Ids = temp[4],
                    Cheetah = temp[5],
                    Numbers = temp[6],
                    Info =
                        temp[7] + " " + temp[8] + " " + temp[9] + " " + temp[10] + " " + temp[11] +
                        " " +
                        temp[12],
                    Periority = temp[13] + temp[14],
                    TCP = temp[15],
                    SourceIP = temp[16],
                    Way = temp[17],
                    DestinationIP = desIp
                };
            }
            else //branch B-2-d
            {
                return new TotalEvent()
                {
                    Month = temp[0],
                    Day = days,
                    Time = temp[2],
                    Ids = temp[3],
                    Cheetah = temp[4],
                    Numbers = temp[5],
                    Info = temp[6] + temp[7] + temp[8] + temp[9] + temp[10] + temp[11] + temp[12],
                    Periority = temp[13],
                    TCP = temp[14],
                    SourceIP = temp[15],
                    Way = temp[16],
                    DestinationIP = desIp
                };
            }
        }

        public static TotalEvent BranchB3(string[] temp, int days, string desIp)
        {
            var information = new StringBuilder();
            var classif = new StringBuilder();
            int j, i;

            for (i = 7; i < temp.Length; i++) //information
            {
                if (temp[i] == "[Classification:")
                    break;
                information.Append(temp[i] + " ");
            }
            for (j = i; j < temp.Length; j++) //classification
            {
                if (temp[j] == "[Priority:")
                    break;
                classif.Append(temp[j] + " ");
            }

            return new TotalEvent()
            {

                Month = temp[0],
                Day = days,
                Time = temp[3],
                Ids = temp[4],
                Cheetah = temp[5],
                Numbers = temp[6],
                Info = information.ToString(),
                Classification = classif.ToString(),
                Periority = temp[j] + temp[j + 1],
                TCP = temp[j + 2],
                SourceIP = temp[j + 3],
                Way = temp[j + 4],
                DestinationIP = desIp,
                Characters = temp[j + 6]
            };
        }

    }
}