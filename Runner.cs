using static System.Console;
using System.Threading;
using System;
using System.Linq;
using static System.ConsoleKey;
using System.Drawing;

internal class BITMAP
{
    public Bitmap BmpSrc { get; set; }
    public Bitmap BmpMax {
        get {
            decimal Percent =
                Math.Min(decimal.Divide(39, BmpSrc.Width), decimal.Divide(39, BmpSrc.Height));
            Size DSize =
                new Size((int)(BmpSrc.Width * Percent), (int)(BmpSrc.Height * Percent));
            return new Bitmap(BmpSrc, DSize.Width, DSize.Height);
        }
    }
}
class Runner
{
    private Random random;
    private Color[,] plane;
    private BITMAP[] bitmaps;
    private int x, y;
    private int score = 0;
    public int obsticleRange = 10;
    private static readonly int[] cColors =
            {
                0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0,
                0x808080, 0x0000FF, 0x00FF00, 0x00FFFF,
                0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF
            };

    public Runner()
    {
        bitmaps = new BITMAP[] {
            new BITMAP
            {
                        BmpSrc = new Bitmap(@"C:\Users\Simon\Desktop\Program.png", true)
            },
            new BITMAP
            {
                        BmpSrc = new Bitmap(@"C:\Users\Simon\Desktop\Program.png", true)
            },
            new BITMAP
            {
                        BmpSrc = new Bitmap(@"C:\Users\Simon\Desktop\Program.png", true)
            },
            new BITMAP
            {
                        BmpSrc = new Bitmap(@"C:\Users\Simon\Desktop\Program.png", true)
            },
            new BITMAP
            {
                        BmpSrc = new Bitmap(@"C:\Users\Simon\Desktop\Program.png", true)
            },
            new BITMAP
            {
                        BmpSrc = new Bitmap(@"C:\Users\Simon\Desktop\Program.png", true)
            }
        };
        y = bitmaps[0].BmpMax.Height;
        x = bitmaps[0].BmpMax.Width;
        plane = new Color[x, y];
        Bitmap ts = bitmaps[0].BmpMax;

        for (int i = 0; i < y; i++)
            for (int j = 0; j < x; j++)
                plane[j, i] = ts.GetPixel(j, i);
        Play();
    }
    public void Play()
    {
        int[,] obsPoints = new int[x, y];
        int[,] candyPoints = new int[x, y];
        var color = Color.Blue;
        int playerAtX = 0;
        int playerAtY = 0;
        random = new Random();
        int bitmrange = random.Next(0, bitmaps.Length - 1);
        int candyLen = random.Next(1, 2);
        int obsticleLen = random.Next(obsticleRange, obsticleRange + 1);

        for (int i = 0; i < obsticleLen; i++) //Obs
        {
            int rnbX = random.Next(1, x), rnbY = random.Next(1, y);
            plane[rnbX, rnbY] = Color.Black;
            obsPoints[rnbX, rnbY] = 1;
        }
        for (int i = 0; i < candyLen; i++) //candy
        {
            int rnbX = random.Next(1, x), rnbY = random.Next(1, y);
            plane[rnbX, rnbY] = Color.Gold;
            candyPoints[rnbX, rnbY] = 1;
        }
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
                WritePixel(plane[j, i]);
            WriteLine();
        }
        ResetColor();
        while (true)
        {
            if (candyPoints[playerAtX, playerAtY] == 1)
            {
                score++;
                Clear();
                WriteLine("Congrats!...");
                Thread.Sleep(2000);
                Clear();
                y = bitmaps[bitmrange].BmpMax.Height;
                x = bitmaps[bitmrange].BmpMax.Width;
                plane = new Color[x, y];

                y = bitmaps[bitmrange].BmpMax.Height;
                x = bitmaps[bitmrange].BmpMax.Width;
                plane = new Color[x, y];

                var ts = bitmaps[bitmrange].BmpMax;
                for (int i = 0; i < y; i++)
                    for (int j = 0; j < x; j++)
                        plane[j, i] = ts.GetPixel(j, i);

                obsticleRange *= 2;
                Play();
            }

            if (obsPoints[playerAtX, playerAtY] == 1)
            {
                Kill();
                return;
            }

            switch (ReadKey().Key)
            {
                case RightArrow:
                    if (playerAtX == (x - 1))//wall
                    {
                        Kill();
                        continue;
                    }
                    else
                    {
                        playerAtX++;
                        var getPrevColor = plane[playerAtX - 1, playerAtY]; WriteAt(playerAtX - 1, playerAtY, getPrevColor);
                        WriteAt(playerAtX - 1, playerAtY, getPrevColor);
                        WriteAt(playerAtX, playerAtY, color);
                    }
                    break;

                case LeftArrow:
                    if (playerAtX == 0)//wall
                    {
                        Kill();
                        continue;
                    }
                    else
                    {
                        playerAtX--;
                        var getPrevColor = plane[playerAtX + 1, playerAtY];
                        WriteAt(playerAtX + 1, playerAtY, getPrevColor);
                        WriteAt(playerAtX + 1, playerAtY, getPrevColor);
                        WriteAt(playerAtX, playerAtY, color);
                    }

                    break;

                case DownArrow:
                    if (playerAtY == (y - 1))//wall
                    {
                        Kill();
                        continue;
                    }
                    else
                    {
                        playerAtY++;
                        var getPrevColor = plane[playerAtX, playerAtY - 1];
                        WriteAt(playerAtX, playerAtY - 1, getPrevColor);
                        WriteAt(playerAtX, playerAtY, color);
                    }
                    break;

                case UpArrow:
                    if (playerAtY == 0)//wall
                    {
                        Kill();
                        continue;
                    }
                    else
                    {
                        playerAtY--;
                        var getPrevColor = plane[playerAtX, playerAtY + 1];
                        WriteAt(playerAtX, playerAtY + 1, getPrevColor);
                        WriteAt(playerAtX, playerAtY, color);
                    }
                    break;
            }
        }
    }

    private void Kill()
    {
        Clear();
        Write("over: " + score);
        WriteLine();
        Write("You want to share the game?");
        ReadLine();
    }

    public static void WriteAt(int left, int top, Color color)
    {
        int currentLeft = CursorLeft,
            currentTop = CursorTop;
        CursorVisible = false;
        SetCursorPosition(left, top);
        WritePixel(color);
        SetCursorPosition(currentLeft, currentTop);

    }
    public static void WritePixel(Color cValue)
    {
        Color[] cTable =
            cColors.Select(x => Color.FromArgb(x)).ToArray();

        char[] rList =
            new char[] { (char)9617, (char)9618, (char)9619, (char)9608 };

        int[] bestHit =
            new int[] { 0, 0, 4, int.MaxValue };

        for (int rChar = rList.Length; rChar > 0; rChar--)
        {
            for (int cFore = 0; cFore < cTable.Length; cFore++)
            {
                for (int cBack = 0; cBack < cTable.Length; cBack++)
                {
                    int R = (cTable[cFore].R * rChar + cTable[cBack].R * (rList.Length - rChar)) / rList.Length;
                    int G = (cTable[cFore].G * rChar + cTable[cBack].G * (rList.Length - rChar)) / rList.Length;
                    int B = (cTable[cFore].B * rChar + cTable[cBack].B * (rList.Length - rChar)) / rList.Length;
                    int iScore = (cValue.R - R) * (cValue.R - R) + (cValue.G - G) * (cValue.G - G) + (cValue.B - B) * (cValue.B - B);
                    if (!(rChar > 1 && rChar < 4 && iScore > 50000))
                    {
                        if (iScore < bestHit[3])
                        {
                            bestHit[3] = iScore;
                            bestHit[0] = cFore;
                            bestHit[1] = cBack;
                            bestHit[2] = rChar;
                        }
                    }
                }
            }
        }

        ForegroundColor = (ConsoleColor)bestHit[0];
        BackgroundColor = (ConsoleColor)bestHit[1];
        Write(rList[bestHit[2] - 1]);
    }
}
