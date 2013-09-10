using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using NZBStatus.DTOs;
using NZBStatus.Enums;
using Newtonsoft.Json.Linq;

namespace NZBStatus
{
    static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Count() >= 2)
            {
                var jsr = Initialize(args);
                IEnumerable<Slot> queue = new List<Slot>();
                var qs = new QueueSelect();
                var pressedKey = ConsoleKey.End;
                var i = 0;
                var oldI = 0;
                bool _redraw = false;
                var currentSlot = new Slot();

                do
                {

                    while (!Console.KeyAvailable)
                    {
                        if (i > 20)
                        {
                            i = 0;
                            if (!_redraw)
                            {
                                jsr.RefreshData();
                            }
                            else
                            {
                                _redraw = false;
                                i = oldI; // resumes in the process
                            }
                            queue = jsr.GetAllSlots().Where(x => x.index != 0);
                            qs.Refresh(queue.ToList());
                            Console.Clear();

                            // Console.WriteLine(jsonReader.TotalMB);
                            currentSlot = jsr.GetCurrentSlot();
                            var progressbar = Helpers.GetProgressbar(currentSlot.percentage);

                            Console.Write("Filename: ");
                            Console.ForegroundColor = !Helpers.IsSelected(0, qs.Position) ? ConsoleColor.Green : ConsoleColor.Black;
                            Console.WriteLine(currentSlot.filename.MaxWidth());
                            Console.ResetColor();
                            Console.WriteLine(string.Format("{0} {1}%", progressbar, currentSlot.percentage).MaxWidth());
                            Console.WriteLine(string.Format("Size: {0}", currentSlot.size).MaxWidth());

                            Console.WriteLine();
                            if (queue.Any())
                            {
                                Console.WriteLine("Queue:");
                                foreach (var c in queue)
                                {
                                    Console.ForegroundColor = !Helpers.IsSelected(c.index, qs.Position) ? ConsoleColor.DarkYellow : ConsoleColor.Black;
                                    Console.Write("[{0}]", c.size);
                                    Console.Write(" - ");
                                    Console.WriteLine(string.Format("[{0}]", c.filename).MaxWidth());
                                    Console.ResetColor();
                                }
                            }
                            UpdateTitle(jsr);
                        }
                        Thread.Sleep(50);
                        i++;
                    }
                    pressedKey = Console.ReadKey(true).Key;
                    _redraw = true;
                    oldI = i;
                    i = 100; // forces refresh
                    switch (pressedKey)
                    {
                        case ConsoleKey.UpArrow:
                            qs.Up();
                            break;
                        case ConsoleKey.DownArrow:
                            qs.Down();
                            break;
                        case ConsoleKey.Enter:
                            if (qs.Position == 0) // current slot is selected
                            {
                                JsonReader res;
                                res = jsr.Status == "Paused" ? new JsonReader(args[0], args[1], true, "resume") : new JsonReader(args[0], args[1], true, "pause");
                            }
                            break;
                    }
                } while (pressedKey != ConsoleKey.Escape);
            }
        }

        private static JsonReader Initialize(string[] args)
        {
            var
                jsr = new JsonReader(args[0],
                                     args[1]);
            Console.CursorVisible = false;
            var culture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            return jsr;
        }

        private static void UpdateTitle(JsonReader jsonReader)
        {
            Console.Title = String.Format("{0} -[{1}] - SabNZBd watcher: {2}/{3}",
                jsonReader.ConnectionStatus() != ConnectionStatus.Ok ? jsonReader.ConnectionStatus().ToString() : jsonReader.StatusIcon,
                                          jsonReader.Speed.SpeedToString(),
                                          jsonReader.AlreadyDownloadedMB.SizeToString(),
                                          jsonReader.TotalMB.SizeToString());
        }
    }
}
