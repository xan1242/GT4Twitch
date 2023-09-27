using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDTools.SaveFile;
using PDTools.SaveFile.GT4;
using PDTools.SaveFile.GT4.UserProfile;
using PDTools.Enums.PS2;
using GT4SaveEditor.Database;
using System.Diagnostics;
using System.Globalization;
using gt4pcsx2launcher;
using System.Threading.Channels;
using System.Web;
using System.IO;

namespace GT4Twitch
{
    internal class Core
    {
        static IntPtr pcsx2handle = IntPtr.Zero;
        static IntPtr pcsx2mod = IntPtr.Zero;

        static EventDatabase _eventDb;
        static GT4Database _gt4Database;

        public static bool bIsOnline = false;
        public static bool bIsSpec2 = false;

        static long USCheckAddr = 0x006BA380;
        static long OnlineCheckAddr = 0x721DD0;

        static string USSaveName = "BA" + "SCUS-97328" + "GAMEDATA";
        static string OnlineSaveName = "BA" + "SCUS-97436" + "GAMEDATA";

        public static double percentGameCompletion = 0;
        static int percentGameCompletionInt = 0;

        static string saveFileLocation;
        static string titleFormatString;

        static string clientId;
        static string authToken;
        static string channelId;
        static bool bAuthorized = false;

        static async Task UpdateTwitchThing(string newTitle)
        {
            if (bAuthorized)
            {
                TwitchApiClient twitchApiClient = new TwitchApiClient(clientId, authToken);

                bAuthorized = await twitchApiClient.ValidateAccessTokenAsync();

                if (!bAuthorized)
                {
                    Console.WriteLine("Lost authorization! You will have to restart GT4Twitch and re-log in!");
                    return;
                }

                bool success = await twitchApiClient.UpdateStreamTitleAsync(channelId, newTitle);

                if (success)
                {
                    Console.WriteLine("Stream title updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to update stream title.");
                }
            }
        }

        static bool TestIsSaved(long addr)
        {
            UInt32 val = 0;

            if (Win32.ReadUInt32(pcsx2handle, addr, out val))
            {
                if (val != 0)
                {
                    return true;
                }
            }
            return false;
        }

        static bool CheckIfReady(long addr)
        {
            UInt32 val = 0;

            if (Win32.ReadUInt32(pcsx2handle, addr, out val))
            {
                if (val == 0x6E697474)
                    return true;
            }

            return false;
        }

        // patches the save game procedure to write the address of save data at 0x006BA380
        // it was originally planned to read out the save data directly from memory but that proved to be extremely difficult
        static void PatchGame(long baseaddr)
        {
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x43AB18, 0x0C1AE8D8); // jal 0x6BA360

            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x6BA360, 0x3C03006C); // lui v1, 0x6C    
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x6BA364, 0x2463A380); // addiu v1, v1, 0xA380
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x6BA368, 0xAC640000); // sw a0, (v1) 
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x6BA36C, 0x0810EC5A); // j 0x43B168
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x6BA370, 0x00000000); // nop 
        }

        // patches the save game procedure to write the address of save data at 0x721DD0
        static void PatchGameUSOnline(long baseaddr)
        {
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x316BE4, 0x0C1C876C); // jal 0x721DB0

            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x721DB0, 0x3C030072); // lui v1, 0x72    
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x721DB4, 0x24631DD0); // addiu v1, v1, 0x1DD0
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x721DB8, 0xAC640000); // sw a0, (v1) 
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x721DBC, 0x080C4D7E); // j 0x3135F8
            Win32.WriteUInt32(pcsx2handle, baseaddr + 0x721DC0, 0x00000000); // nop 
        }

        static void ClearMcData()
        {
            if (File.Exists(USSaveName))
                File.Delete(USSaveName);
            if (File.Exists(OnlineSaveName))
                File.Delete(OnlineSaveName);
            if (File.Exists("garage"))
                File.Delete("garage");
        }

        static void DumpFiles(double percent, int win, int total, int gold, int totalLicenses, int totalScore, int winMission, int totalMissions, int goldCoffee, int totalCoffeeBrakes)
        {
            if (!Directory.Exists("texts"))
                Directory.CreateDirectory("texts");

            string path = "texts/percent.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                CultureInfo customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";

                string str = percentGameCompletion.ToString("0.0", customCulture) + "%";

                Byte[] info = new UTF8Encoding(true).GetBytes(str);
                fs.Write(info, 0, info.Length);
                fs.Flush();
                fs.Close();
            }

            path = "texts/win.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                string str = win.ToString() + "/" + total.ToString();

                Byte[] info = new UTF8Encoding(true).GetBytes(str);
                fs.Write(info, 0, info.Length);
                fs.Flush();
                fs.Close();
            }

            path = "texts/gold.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                string str = gold.ToString() + "/" + totalLicenses.ToString();

                Byte[] info = new UTF8Encoding(true).GetBytes(str);
                fs.Write(info, 0, info.Length);
                fs.Flush();
                fs.Close();
            }

            path = "texts/mission.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                string str = winMission.ToString() + "/" + totalMissions.ToString();

                Byte[] info = new UTF8Encoding(true).GetBytes(str);
                fs.Write(info, 0, info.Length);
                fs.Flush();
                fs.Close();
            }

            path = "texts/coffee.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                string str = goldCoffee.ToString() + "/" + totalCoffeeBrakes.ToString();

                Byte[] info = new UTF8Encoding(true).GetBytes(str);
                fs.Write(info, 0, info.Length);
                fs.Flush();
                fs.Close();
            }

            path = "texts/totalScore.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                string str = totalScore.ToString();

                Byte[] info = new UTF8Encoding(true).GetBytes(str);
                fs.Write(info, 0, info.Length);
                fs.Flush();
                fs.Close();
            }
        }

        static int ExtractSaveData(string InFilename, string mcName)
        {
            string mcdPath = InFilename;
            string memcardname = mcName;

            ClearMcData();

            string mymccmd = "extract " + memcardname + "/" + memcardname;

            var mymcproc = new Process { StartInfo = new ProcessStartInfo { UseShellExecute = false, CreateNoWindow = false, FileName = "myMC/myMC.exe", Arguments = "\"" + mcdPath + "\"" + " " + mymccmd } };
            mymcproc.Start();
            mymcproc.WaitForExit();

            if (mymcproc.ExitCode != 0)
            {
                Console.WriteLine("GT4Twitch: myMC failed to extract the save data!");
                return mymcproc.ExitCode;
            }

            mymccmd = "extract " + memcardname + "/garage";
            mymcproc = new Process { StartInfo = new ProcessStartInfo { UseShellExecute = false, CreateNoWindow = false, FileName = "myMC/myMC.exe", Arguments = "\"" + mcdPath + "\"" + " " + mymccmd } };
            mymcproc.Start();
            mymcproc.WaitForExit();

            if (mymcproc.ExitCode != 0)
            {
                Console.WriteLine("GT4Twitch: myMC failed to extract the save data!");
                return mymcproc.ExitCode;
            }

            return mymcproc.ExitCode;
        }

        static void HandleSaveLoad()
        {
            GT4Save save = GT4Save.Load(".");
            Console.WriteLine("GT4Twitch: PDTools detected save type: " + save.Type.ToString());
            Console.WriteLine("GT4Twitch: Calculating percentage");
            switch (save.Type)
            {
                case GT4SaveType.Unknown:
                    break;
                case GT4SaveType.GT4_US:
                case GT4SaveType.GT4O_US:
                    if (bIsSpec2)
                        _gt4Database.CreateConnection("Resources/Databases/GT4_PREMIUM_US2560-Spec2.sqlite");
                    else
                        _gt4Database.CreateConnection("Resources/Databases/GT4_PREMIUM_US2560.sqlite");
                    break;
                default:
                    break;
            }

            _eventDb.LoadEventIndices(save.Type, _gt4Database);
            int win = 0;
            int winMission = 0;
            int gold = 0;
            int goldCoffee = 0;
            int total = 0;
            int totaltotal = 0;
            int totalScore = 0;
            int totalLicenses = 0;
            int totalMissions = 0;
            int totalCoffeeBrakes = 0;

            foreach (EventCategory category in _eventDb.Categories)
            {
                foreach (var @event in category.Events)
                {
                    RaceRecordUnit unit = save.GameData.Profile.RaceRecords.Records[@event.DbIndex];
                    var evMode = @event.GameMode;

                    if (evMode == "RACE_MODE_SINGLE")
                    {
                        EventType type = unit.GetEventType();
                        if (((type == EventType.Event) || (type != EventType.Mission)) && (type != EventType.License))
                        {
                            Result _resP = unit.GetPermanentResult();
                            Result _resE = unit.GetCurrentResult();
                            Result _resM = unit.GetUnknownLicenseOrMissionResult();

                            if ((_resP == Result._1) || (_resE == Result._1) || (_resM == Result._1))
                                win++;
                            totalScore += unit.ASpecScore;
                        }
                        total++;
                    }
                    if (evMode == "RACE_MODE_LICENSE")
                    {
                        EventType type = unit.GetEventType();

                        if (category.Name.Contains("Mission"))
                        {
                            if ((type == EventType.Mission) && (type != EventType.License))
                            {
                                Result _resP = unit.GetPermanentResult();
                                Result _resE = unit.GetCurrentResult();
                                Result _resM = unit.GetUnknownLicenseOrMissionResult();

                                if ((_resP == Result._1) || (_resE == Result._1) || (_resM == Result._1))
                                {
                                    winMission++;
                                    win++;
                                }
                                totalScore += unit.ASpecScore;
                            }
                            totalMissions++;
                            total++;
                        }
                        else
                        {
                            if (category.Name.Contains("Coffee"))
                            {
                                if ((type == EventType.License) && (type != EventType.Mission))
                                {
                                    Result _resP = unit.GetPermanentResult();
                                    if (_resP == Result.gold)
                                        goldCoffee++;
                                    totalScore += unit.ASpecScore;
                                }
                                totalCoffeeBrakes++;
                            }
                            else
                            {
                                if ((type == EventType.License) && (type != EventType.Mission))
                                {
                                    Result _resP = unit.GetPermanentResult();
                                    if (_resP == Result.gold)
                                        gold++;
                                    totalScore += unit.ASpecScore;
                                }
                                totalLicenses++;
                            }
                        }
                    }
                    totaltotal++;
                }
            }

            //if (bIsSpec2)
            //    total -= 9;

            if ((win > 0) && (total > 0))
                percentGameCompletionInt = win * 1000 / total;
            else
                percentGameCompletionInt = 0;

            percentGameCompletion = (double)percentGameCompletionInt * 0.1;

            CultureInfo customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            Console.WriteLine("GT4Twitch: Percent complete: " + percentGameCompletion.ToString("0.0", customCulture));
            //Console.WriteLine("GT4Twitch: AllEv: " + totaltotal + "\nGT4Twitch: EvMis: " + total + "\nGT4Twitch: Wins: " + win);
            string OutString = string.Format(titleFormatString, percentGameCompletion.ToString("0.0", customCulture), win.ToString(), total.ToString(), gold.ToString(), totalScore.ToString(), totalLicenses.ToString(), winMission.ToString(), totalMissions.ToString(), goldCoffee.ToString(), totalCoffeeBrakes.ToString());
            string limitedOutString = new string(OutString.Take(140).ToArray());
            Console.WriteLine("GT4Twitch: Title: " + limitedOutString);
            UpdateTwitchThing(OutString);
            DumpFiles(percentGameCompletion, win, total, gold, totalLicenses, totalScore, winMission, totalMissions, goldCoffee, totalCoffeeBrakes);
        }

        public static void Run(IntPtr handle, string McPath, string TitleFormat, string inAuthToken, string inChannelId, string inClientId, bool bIsAuthorized)
        {
            pcsx2handle = handle;
            saveFileLocation = McPath;
            titleFormatString = TitleFormat;

            authToken = inAuthToken;
            channelId = inChannelId;
            clientId = inClientId;
            bAuthorized = bIsAuthorized;

            pcsx2mod = Win32.GetFirstProcMod(pcsx2handle);
            if (pcsx2mod == IntPtr.Zero)
                return;

            long ps2addr = ((long)pcsx2mod & 0xFFFFF0000000);
            ps2addr += 0x40000000;
            Console.WriteLine("GT4Twitch: PCSX2 base: 0x" + pcsx2mod.ToString("X"));
            Console.WriteLine("GT4Twitch: PS2 base: 0x" + ps2addr.ToString("X"));

            long testaddr2 = (ps2addr + USCheckAddr);
            if (bIsOnline)
                testaddr2 = (ps2addr + OnlineCheckAddr);

            Console.WriteLine("GT4Twitch: Waiting for GT4 to load...");
            while (!CheckIfReady(testaddr2)) 
            {
                if (win32process.IsProcessActive(pcsx2handle) == false)
                {
                    Console.WriteLine("GT4Twitch: PCSX2 not active anymore, shutting down...");
                    Application.Exit();
                    break;
                }
                Thread.Sleep(1); 
            }

            Console.WriteLine("GT4Twitch: GT4 found. Patching the game & watching...");

            Win32.WriteUInt32(pcsx2handle, testaddr2, 0);
            if (bIsOnline)
                PatchGameUSOnline(ps2addr);
            else
                PatchGame(ps2addr);

            _gt4Database = new();
            _eventDb = new();
            if (bIsSpec2)
                _eventDb.Load("Resources/EventList-Spec2.txt");
            else
                _eventDb.Load("Resources/EventList.txt");

            while (true)
            {
                if (win32process.IsProcessActive(pcsx2handle) == false)
                {
                    Console.WriteLine("GT4Twitch: PCSX2 not active anymore, shutting down...");
                    Application.Exit();
                    break;
                }

                if (CheckIfReady(testaddr2))
                {
                    Console.WriteLine("GT4Twitch: Game reset detected! Restarting...");
                    
                    Thread runThread = new Thread(() => Core.Run(pcsx2handle, saveFileLocation, titleFormatString, authToken, channelId, clientId, bAuthorized));
                    runThread.Start();
                    break;
                }

                if (TestIsSaved(testaddr2))
                {
                    Console.WriteLine("GT4Twitch: Save game detected! Waiting 2secs before extraction...");

                    Thread.Sleep(2000);
                    Console.WriteLine("GT4Twitch: Requesting myMC to extract save data");
                    if (bIsOnline)
                        ExtractSaveData(saveFileLocation, OnlineSaveName);
                    else
                        ExtractSaveData(saveFileLocation, USSaveName);
                    HandleSaveLoad();
                    Win32.WriteUInt32(pcsx2handle, testaddr2, 0);
                }

                Thread.Sleep(1);
            }
        }
    }
}
