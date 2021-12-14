using System;
using System.IO;
using System.Linq;
using System.Text;

public class Looti
{
    public static bool beep = true;
    public static bool IDE = false;
    public static bool hello = true;
    public static int xint;
    public static bool click = false;
    public static bool lockf = false;
    public static bool wrap = true;
    static string tosav;
    static string cursor = " ";
    static double ver = 4;

    public static string Run(string[] argv)
    {
        Editor(argv);
        Console.CursorVisible = true;
        Console.CursorSize = 16;
        return null;
    }


    public static string Editor(string[] argv)
    {
        Console.CursorVisible = false;
        string PATH = argv[0];
        if (!File.Exists("looti.scf"))
        {
            File.Create("looti.scf");
            File.AppendAllText("looti.scf", "beep=yes\nhello=yes\nwraplines=yes\nIDE=no");
            beep = true;
            hello = true;
        }
        else
        {
            string[] scf = File.ReadAllLines("looti.scf");
            for (int i = 0; i < scf.Length; i++)
            {
                if (scf[i] == "beep=yes") { beep = true; }
                if (scf[i] == "beep=no") { beep = false; }
                if (scf[i] == "hello=yes") { hello = true; }
                if (scf[i] == "hello=no") { hello = false; }
                if (scf[i] == "click=yes") { click = true; }
                if (scf[i] == "click=no") { click = false; }
                if (scf[i] == "wraplines=yes") { wrap = true; }
                if (scf[i] == "wraplines=no") { wrap = false; }
                if (scf[i] == "IDE=yes") { IDE = true; }
                if (scf[i] == "IDE=no") { IDE = false; }
                if (scf[i].StartsWith("cursor=")) { scf[i] = scf[i].Remove(0, 7); if (scf[i].Length == 1) cursor = scf[i]; }
            }
        }
        PATH = PATH.ToLower();
        Console.Clear();

        for (int i = 1; i < argv.Length; i++)
        {
            if (argv[i] == "--help") { Console.WriteLine("Looti Perfect " + ver + ", MIT license.\nhttps://github.com/luftkatzeBASIC/\nLooti " + ver + ", 2021 Luftkatze\n"); Console.CursorVisible = true; return null; }
            else if (argv[i] == "--lock") lockf = true;
            else if (argv[i] == "--ide") IDE = true;
            else { Console.Write("\nUnknow argument - "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(argv[i]); Console.ForegroundColor = ConsoleColor.White; Console.CursorVisible = true; return null; }
        }


        if (PATH == "conf")
        {
            PATH = "Looti.scf";
        }
        if (!File.Exists(@PATH))
        {
            File.Create(PATH);
        }

        if (hello == true) DrawLogo();
        Console.BackgroundColor = ConsoleColor.Blue;
        string arrow = "";
        string old = File.ReadAllText(Directory.GetCurrentDirectory() + @PATH);
        tosav = File.ReadAllText(Directory.GetCurrentDirectory() + @PATH);
        for (; ; )
        {
            DrawBar(@PATH, tosav, arrow, old);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(@tosav);
            xint = Console.CursorLeft;
            int yint = Console.CursorTop;
            Console.Write(@arrow);
            Console.SetCursorPosition(xint, yint);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(cursor);
            xint = Console.CursorLeft;
            yint = Console.CursorTop;

            Console.SetCursorPosition(xint, yint);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            ConsoleKeyInfo input;
            if (lockf == false) { input = Console.ReadKey(true); }
            else if (lockf == true)
            {
                Console.SetCursorPosition(0, 24);
                Console.Write("[Document locked] - Press INSERT to exit");
                ConsoleKey lockv;
                for (; ; )
                {
                    lockv = Console.ReadKey().Key;
                    if (lockv == ConsoleKey.Insert) { Console.CursorVisible = true; return null; }
                    else Console.CursorLeft--;
                }
            }
            if (click == true) Console.Beep(900, 35);
            if (tosav.Contains("%DATE%"))
            {
                var d = DateTime.Now.ToShortDateString();
                tosav = tosav.Replace("%DATE%", d.ToString());
            }
            else if (tosav.Contains("%TIME%"))
            {
                DateTime dt = DateTime.Now;
                var t = dt.ToShortTimeString();
                tosav = tosav.Replace("%TIME%", t.ToString());
            }
            else if (tosav.Contains("%NOW%"))
            {
                DateTime dt = DateTime.Now;

                tosav = tosav.Replace("%NOW%", dt.ToString());
            }

            if (input.Key == ConsoleKey.Enter)
            {
                tosav += "\n";

            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.S) != 0)
                {
                    File.Delete(@PATH);
                    File.AppendAllText(@PATH, tosav + arrow);
                    old = tosav + arrow;
                }
            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.O) != 0)
                {
                    string open = Open();
                    if (open != null && File.Exists(open))
                    {
                        tosav = File.ReadAllText(open);
                        arrow = "";
                        PATH = open;
                    }
                }
            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.N) != 0)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.SetCursorPosition(20, 13);
                    Console.WriteLine("[New file name]");
                    Console.SetCursorPosition(20, 14);
                    string newflnam = LootiTerminal();
                    if (newflnam != null)
                    {
                        tosav = File.ReadAllText(newflnam);
                        arrow = "";
                        PATH = newflnam;
                    }
                }
            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.Q) != 0)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.SetCursorPosition(20, 13);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("[Save as...]");
                    Console.SetCursorPosition(20, 14);
                    string savas = LootiTerminal();
                    if (savas != null)
                    {
                        File.AppendAllText(@savas, old);
                        PATH = savas;
                    }
                }

            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.H) != 0)
                {
                    Replace(tosav, old, arrow, PATH);
                }

            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.F) != 0)
                {
                    Find(tosav, old, arrow, PATH);
                }

            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.X) != 0)
                {
                    Exit(tosav, old, arrow, PATH);
                    Console.CursorVisible = true; return null;
                }
            }
            else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
            {
                if ((input.Key & ConsoleKey.K) != 0)
                {
                    if (@PATH.EndsWith(".TXT"))
                    {
                        PATH = PATH.Replace(".TXT", ".LTI");
                    }
                    else if (@PATH.EndsWith(".LTI"))
                    {
                        PATH = PATH.Replace(".LTI", ".TXT");

                    }
                    else if (@PATH.EndsWith(".DOC"))
                    {
                        PATH = PATH.Replace(".DOC", ".LTI");
                    }
                }
            }
            else if (input.Key == ConsoleKey.F1)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 2);
                Console.Write("| File            \n");
                Console.Write("| F1  [SAVE]     S\n");
                Console.Write("| F2  [OPEN]     O\n");
                Console.Write("| F3  [NEW]      N\n");
                Console.Write("| F4  [SAVE AS]  Q\n");
                Console.Write("| F12 [EXIT]     X");
                Console.ForegroundColor = ConsoleColor.Gray;

                input = Console.ReadKey();
                if (input.Key == ConsoleKey.F1)
                {
                    File.Delete(@PATH);
                    File.AppendAllText(@PATH, tosav + arrow);
                    old = tosav + arrow;
                }
                else if (input.Key == ConsoleKey.F2)
                {
                    string open = Open();
                    if (open != null && File.Exists(open))
                    {
                        tosav = File.ReadAllText(open);
                        arrow = "";
                        PATH = open;
                    }

                }
                else if (input.Key == ConsoleKey.F3)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.SetCursorPosition(20, 13);
                    Console.WriteLine("[New file name]");
                    Console.SetCursorPosition(20, 14);
                    string newflnam = LootiTerminal();
                    if (newflnam != null)
                    {
                        tosav = File.ReadAllText(newflnam);
                        arrow = "";
                        PATH = newflnam;
                    }
                }
                else if (input.Key == ConsoleKey.F4)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.SetCursorPosition(20, 13);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("[Save as...]");
                    Console.SetCursorPosition(20, 14);
                    string savas = LootiTerminal();
                    if (savas != null)
                    {
                        File.AppendAllText(@savas, old);
                        PATH = savas;
                    }
                }

                else if (input.Key == ConsoleKey.F12)
                {
                    Exit(tosav, old, arrow, PATH);
                    Console.CursorVisible = true; return null;
                }
                else
                {
                    DrawBar(@PATH, tosav, arrow, old);
                }


            }
            else if (input.Key == ConsoleKey.F2)

            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(8, 2);
                Console.Write("| Edit           \n");
                Console.SetCursorPosition(8, 3);
                Console.Write("| F1  [REPLACE] H\n");
                Console.SetCursorPosition(8, 4);
                Console.Write("| F2  [CONVERT] K\n");
                Console.SetCursorPosition(8, 5);
                Console.Write("| F3  [FIND]    F\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.F2)
                {
                    if (@PATH.EndsWith(".TXT"))
                    {
                        PATH = PATH.Replace(".TXT", ".LTI");
                    }
                    else if (@PATH.EndsWith(".LTI"))
                    {
                        PATH = PATH.Replace(".LTI", ".TXT");

                    }
                    else if (@PATH.EndsWith(".DOC"))
                    {
                        PATH = PATH.Replace(".DOC", ".LTI");
                    }
                }
                else if (input.Key == ConsoleKey.F1)
                {
                    Replace(tosav, old, arrow, PATH);
                }
                else if (input.Key == ConsoleKey.F3)
                {
                    Find(tosav, old, arrow, PATH);

                }
                else
                {
                    DrawBar(@PATH, tosav, arrow, old);
                }
            }
            else if (input.Key == ConsoleKey.F3)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.SetCursorPosition(19, 2);
                Console.Write("| Other          \n");
                Console.SetCursorPosition(19, 3);
                Console.Write("| F1   [CRET]   \n");
                Console.SetCursorPosition(19, 4);
                Console.Write("| F2   [ABOUT]   \n");
                Console.SetCursorPosition(19, 5);
                Console.Write("| F3   [PATH]    \n");
                Console.SetCursorPosition(19, 6);
                Console.Write("| F4   [CONF]    \n");
                Console.ForegroundColor = ConsoleColor.Gray;
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.F1)
                {
                    return null;
                }
                else if (input.Key == ConsoleKey.F2)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.WriteLine(
                        "Looti text editor" +
                        "\nVersion" + ver +
                        "\nCreated by:\n" +
                        "Programming: Luftkatze\n" +
                        "Name: BaseMax\n" +
                        "Few ideas: Ilobilo\n" +
                        "2021, Luftkatze\n\n\n\n" +
                        "Default Looti file extension: * .LTI\n"
                        );
                    Console.ReadKey();
                }
                else if (input.Key == ConsoleKey.F3)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.SetCursorPosition(20, 13);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(20, 14);
                    Console.WriteLine("[New path...]");
                    string np = LootiTerminal();
                    if (np != null)
                    {
                        File.AppendAllText(np, old);
                        PATH = np;
                    }
                }
                else if (input.Key == ConsoleKey.F4)
                {


                    if (Exit(tosav, old, arrow, PATH) != null)
                    {
                        tosav = File.ReadAllText("looti.scf");
                        old = tosav;
                        arrow = "";
                    }
                }
                else
                {
                    DrawBar(@PATH, tosav, arrow, old);
                }
            }
            else if (input.Key == ConsoleKey.F4)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.SetCursorPosition(29, 2);
                Console.Write("| Insert          \n");
                Console.SetCursorPosition(29, 3);
                Console.Write("| F1  [NOW]       \n");
                Console.SetCursorPosition(29, 4);
                Console.Write("| F2  [TIME]      \n");
                Console.SetCursorPosition(29, 5);
                Console.Write("| F3  [DATE]      \n");
                Console.SetCursorPosition(29, 6);
                Console.Write("| F4  [FILE]      \n");
                Console.SetCursorPosition(29, 7);
                Console.Write("| F5  [ASCII]     \n");
                Console.ForegroundColor = ConsoleColor.Gray;
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.F1)
                {
                    DateTime dt = DateTime.Now;

                    tosav += dt.ToString();
                }
                else if (input.Key == ConsoleKey.F2)
                {
                    DateTime dt = DateTime.Now;
                    var t = dt.ToShortTimeString();
                    tosav += t.ToString();
                }
                else if (input.Key == ConsoleKey.F3)
                {
                    var dt = DateTime.Now;
                    var d = dt.ToShortDateString();
                    tosav += d.ToString();
                }
                else if (input.Key == ConsoleKey.F4)
                {

                    string open = Open();
                    if (open != null)
                    {
                        if (File.Exists(open))
                        {
                            string toconcat = File.ReadAllText(open);
                            tosav += toconcat;
                        }
                        else
                        {
                            DrawBar(@PATH, tosav, arrow, old);
                            Console.SetCursorPosition(20, 13);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("CANNOT FIND FILE!"); Console.SetCursorPosition(27, 13);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.ReadKey();
                            DrawBar(@PATH, tosav, arrow, old);
                        }
                    }
                }
                else if (input.Key == ConsoleKey.F5)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    DrawBar(@PATH, tosav, arrow, old);
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(@tosav);
                    Console.Write(@arrow);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(20, 13);
                    Console.Write("CHAR [                              ]"); Console.SetCursorPosition(26, 13);
                    string chr = LootiTerminal();
                    Int32.TryParse(chr, out int ascii);
                    tosav += (char)ascii;
                    DrawBar(@PATH, tosav, arrow, old);
                }
                else
                {
                    DrawBar(@PATH, tosav, arrow, old);
                }
            }
            else if (input.Key == ConsoleKey.Home)
            {
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = true; return null;
            }

            else if (input.Key == ConsoleKey.Escape)
            {

                Console.CursorLeft--;
            }

            else if (input.Key == ConsoleKey.Tab)
            {
                tosav += "\t";

            }
            else if (input.Key == ConsoleKey.Spacebar)
            {
                tosav += " ";
            }
            else if (input.KeyChar == '{')
            {

                tosav += "{";
                if (IDE == true) tosav += "\n"; arrow = "\n}" + arrow;

            }
            else if (input.KeyChar == '(')
            {


                tosav += "(";
                if (IDE == true) arrow = ")" + arrow;

            }
            else if (input.KeyChar == '<')
            {

                tosav += "<";
                if (IDE == true) arrow = ">" + arrow;

            }
            else if (input.KeyChar == '"')
            {


                tosav += "\"";
                if (IDE == true) arrow = "\"" + arrow;

            }
            else if (input.KeyChar == '\'')
            {

                tosav += "'";
                if (IDE == true) arrow = "'" + arrow;

            }
            else if (input.KeyChar == '[')
            {

                tosav += "[";
                if (IDE == true) arrow = "]" + arrow;

            }
            else if (input.Key == ConsoleKey.Backspace || input.Key == ConsoleKey.Delete)
            {
                string back = tosav;
                if (back.Length != 0)
                {
                    int del = back.Length;
                    back = back.Remove(del - 1, 1);
                    tosav = back;
                }
            }
            else if (input.Key == ConsoleKey.Insert)
            {
                Console.SetCursorPosition(0, 24);
                Console.Write("[Document locked]");
                ConsoleKey lockv;
                for (; ; )
                {
                    lockv = Console.ReadKey().Key;
                    if (lockv == ConsoleKey.Insert) break;
                    else Console.CursorLeft--;
                }
            }
            else if (input.Key == ConsoleKey.LeftArrow)
            {

                if (tosav.Length > 0)
                {
                    arrow = tosav[tosav.Length - 1] + arrow;
                    tosav = tosav.Remove(tosav.Length - 1, 1);
                }
                else if (tosav.EndsWith("\n"))
                {
                    arrow = "\n" + arrow;
                    tosav = tosav.Remove(tosav.Length - 1, 1);
                }
            }
            else if (input.Key == ConsoleKey.RightArrow && arrow.Length > 0)
            {
                tosav += arrow[0];
                arrow = arrow.Remove(0, 1);
            }
            else if (input.Key == ConsoleKey.RightArrow && arrow.Length == 0)
            {
                Console.CursorLeft--;
            }

            else if (input.Key == ConsoleKey.UpArrow)
            {
                if (tosav.Length != 0 && Console.CursorTop != 2)
                {
                    if (tosav.Contains("\n"))
                    {
                        for (; ; )
                        {
                            arrow = tosav[tosav.Length - 1] + arrow;
                            tosav = tosav.Remove(tosav.Length - 1, 1);
                            if (arrow.StartsWith("\n")) break;
                        }
                    }
                }
            }
            else if (input.Key == ConsoleKey.DownArrow)
            {
                if (arrow.Contains("\n"))
                {
                    for (; ; )
                    {
                        tosav += arrow[0];
                        arrow = arrow.Remove(0, 1);
                        if (tosav.EndsWith("\n")) break;
                    }

                }
            }
            else if (input.Key == ConsoleKey.PageUp)
            {
                if (tosav.Length != 0)
                {
                    arrow = tosav + arrow;
                    tosav = "";
                }
            }

            else if (input.Key == ConsoleKey.PageDown)
            {
                if (arrow.Length != 0)
                {
                    tosav += arrow;
                    arrow = "";
                }
            }

            else if (input.Key == ConsoleKey.Escape && Console.CursorLeft != 0)
            {
                Console.CursorLeft--;
            }
            else if (input.Key == ConsoleKey.F12)
            {
                File.Delete(@PATH);
                File.AppendAllText(@PATH, tosav + arrow);
                old = tosav + arrow;
            }
            else
            {
                tosav += input.KeyChar.ToString();
                if (wrap == true)
                {
                    if (Console.CursorLeft == 79 && tosav.Contains(" "))
                    {
                        int LastIndex = 0;
                        string lio = tosav;
                        for (int i = 0; i < lio.Length; i++) if (lio[i] == ' ') LastIndex = i;
                        tosav = tosav.Remove(LastIndex, " ".Length).Insert(LastIndex, "\n");
                    }
                }
            }

        }
    }
    public static void DrawBar(string PATH, string tosav, string arrow, string old)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.SetCursorPosition(0, 0);
        for (int i = 0; i < 80; i++) Console.Write(" ");
        Console.SetCursorPosition(0, 0);
        Console.Write("Looti Perfect - " + @PATH);
        if (old != tosav + arrow) Console.Write("*");
        Console.Write("\n");
        Console.SetCursorPosition(35, 0);
        string tolenght = tosav.Replace("\n", "") + arrow.Replace("\n", "");
        int lines = tosav.Split('\n').Length + arrow.Split('\n').Length;
        lines--;
        string[] tarr = tosav.Split('\n');
        string t = tarr[tarr.Length - 1];
        string pos = (tosav.Split('\n').Length - 1).ToString() + "," + t.Length;
        Console.Write("Pos: [" + pos + "]  |  Lines: [" + lines + "] Chars: [" + tolenght.Length + "]");
        Console.SetCursorPosition(0, 1);
        for (int i = 0; i < 80; i++) Console.Write(" ");
        Console.SetCursorPosition(0, 1);
        Console.Write("F1 FILE | F2 EDIT | F3 OTHER | F4 INSERT");
        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    static void Replace(string tosav, string old, string arrow, string PATH)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        DrawBar(@PATH, tosav, arrow, old);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Write(@tosav);
        Console.Write(@arrow);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.SetCursorPosition(20, 13);
        Console.Write("Old [                          ]"); Console.SetCursorPosition(20, 14);
        Console.Write("New [                          ]");
        Console.SetCursorPosition(25, 13);
        string oldrep = LootiTerminal();
        if (oldrep != null)
        {
            Console.SetCursorPosition(25, 14);
            string newrep = LootiTerminal();
            if (newrep != null)
            {
                if (tosav.Contains(oldrep))
                {
                    tosav = tosav.Replace(oldrep, newrep);
                }
                if (arrow.Contains(oldrep))
                {
                    arrow = arrow.Replace(oldrep, newrep);
                }
                else
                {
                    DrawBar(@PATH, tosav, arrow, old);
                    Console.SetCursorPosition(25, 8);
                    Console.WriteLine("CANNOT FIND WORD TO REPLACE!");
                    Console.ReadKey();
                }
            }

        }
        DrawBar(@PATH, tosav, arrow, old);
    }

    static string Exit(string tosav, string old, string arrow, string PATH)
    {
        if (tosav != old)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            bool shouldSave;
            Console.SetCursorPosition(20, 13);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("[Do you want to save changes? Y/N ]");
            Console.SetCursorPosition(20, 14);
            string answer = Console.ReadKey().KeyChar.ToString();
            if (answer.ToLower() == "y") shouldSave = true;
            else shouldSave = false;
            if (shouldSave == false)
            {
                File.Delete(@PATH);
                File.AppendAllText(@PATH, old);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = true; return null;
            }
            else if (shouldSave == true)
            {
                File.Delete(@PATH);
                File.AppendAllText(@PATH, tosav + arrow);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = true; return null;
            }
        }
        else
        {
            Console.ResetColor();
            Console.Clear();
            Console.CursorVisible = true; return null;
        }
        Console.CursorVisible = true; return null;
    }

    public static void DrawLogo()
    {

        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\n\n\n\n\n\n");
        Console.WriteLine("        &@@@@,                                                          (@@@");
        Console.WriteLine("        @@@@@                                               @@@@@       &@@@");
        Console.WriteLine("       @@@@@.                                              ,@@@@(           ");
        Console.WriteLine("      .@@@@@           (@@@@@@@@@@%      *@@@@@@@@@@&    @@@@@@@@@@@  #@@@@.");
        Console.WriteLine("      @@@@@.         #@@@@@   .@@@@@   *@@@@@    @@@@@    *@@@@/      @@@@@");
        Console.WriteLine("     .@@@@&         #@@@@/      #@@@@.*@@@@%     /@@@@*   @@@@@      &@@@@.");
        Console.WriteLine("     @@@@@          @@@@@       @@@@@ &@@@@      @@@@@   *@@@@*      @@@@&");
        Console.WriteLine("    .@@@@%          @@@@@/   *@@@@@   #@@@@%   .@@@@@    @@@@@      #@@@@,");
        Console.WriteLine("    @@@@@@@@@@@@@.   *@@@@@@@@@@@      .@@@@@@@@@@@.     %@@@@@@@   ,@@@@@@&");
        if (beep == true) Cosmos.System.PCSpeaker.Beep(500, 50);
    }

    static void Find(string tosav, string old, string arrow, string PATH)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        DrawBar(@PATH, tosav, arrow, old);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Write(@tosav);
        Console.Write(@arrow);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.SetCursorPosition(20, 13);
        Console.Write("FIND [                              ]"); Console.SetCursorPosition(26, 13);
        string find = LootiTerminal();
        if (find != null)
        {
            if (tosav.Contains(find) || arrow.Contains(find))
            {
                string tofind = "";
                if (tosav.Contains(find)) for (int i = 0; i < find.Length; i++) tofind += tosav.Replace(find, "!");
                if (arrow.Contains(find)) for (int i = 0; i < find.Length; i++) tofind += arrow.Replace(find, "!");
                Console.BackgroundColor = ConsoleColor.Blue;
                DrawBar(@PATH, tofind, arrow, old);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(tofind);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadKey();

                DrawBar(@PATH, tofind, arrow, old);

            }
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            DrawBar(@PATH, tosav, arrow, old);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(@tosav);
            Console.Write(@arrow);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(20, 13);
            Console.Write("CANNOT FIND WORD!"); Console.SetCursorPosition(27, 13);
            Console.ReadKey();
            DrawBar(@PATH, tosav, arrow, old);
        }
    }

    static string Open()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Console.SetCursorPosition(20, 0);
        Console.Write("Files:");
        string[] Fils = Directory.GetFiles(Directory.GetCurrentDirectory());
        int numoffiles = Fils.Length;
        for (int i = 0; i < numoffiles; i++)
        {
            Console.SetCursorPosition(20, i + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(Fils[i]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("]\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("[File to open]");
        Console.SetCursorPosition(0, 1);
        string open = LootiTerminal();
        return open;
    }

    static public string LootiTerminal()
    {
        string toreturn = "";
        for (; ; )
        {
            string arrow = "";
            ConsoleKeyInfo input = Console.ReadKey();
            if (input.Key == ConsoleKey.Enter)
            {
                return toreturn + arrow;
            }
            else if (input.Key == ConsoleKey.Backspace)
            {
                if (toreturn.Length != 0)
                {

                    Console.CursorLeft--;
                    toreturn = toreturn.Remove(toreturn.Length - 1, 1);
                    Console.Write(" ");
                    Console.CursorLeft--;
                }
                else
                {
                    Console.CursorLeft++;
                }
            }
            else if (input.Key == ConsoleKey.LeftArrow)
            {

                if (toreturn.Length > 0)
                {
                    arrow = toreturn[toreturn.Length - 1] + arrow;
                    toreturn = toreturn.Remove(toreturn.Length - 1, 1);
                }
            }
            else if (input.Key == ConsoleKey.RightArrow && arrow.Length > 0)
            {
                toreturn += arrow[0];
                arrow = arrow.Remove(0, 1);
            }
            else if (input.Key == ConsoleKey.RightArrow && arrow.Length == 0)
            {
                Console.CursorLeft--;
            }
            else if (input.Key == ConsoleKey.Escape)
            {
                return null;
            }
            else
            {
                toreturn += input.KeyChar.ToString();
            }
        }
    }

}
