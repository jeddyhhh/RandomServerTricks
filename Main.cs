using System;
using System.Threading.Tasks;
using XLMultiplayerServer;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Diagnostics;

namespace RandomServerTricks
{
    public class Main
    {
        private static Plugin pluginInfo;
        private static readonly Stopwatch stopWatch = new Stopwatch();

        public static void Load(Plugin info)
        {
            pluginInfo = info;
            pluginInfo.OnConnect = OnConnection;
            pluginInfo.OnChatMessage = OnChatMessage;
            pluginInfo.ProcessMessage = ProccessMessage;
            pluginInfo.PlayerCommand = PlayerCommand;
        }

        private static bool OnConnection(string ip)
        {
            return true;
        }

        private static bool OnChatMessage(PluginPlayer player, string message)
        {
            if (message.ToLower().Contains("racist stuff"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void ProccessMessage(PluginPlayer sender, byte[] message)
        {
            string output = "Player " + sender.username + " sent bytes ";
            foreach (byte b in message)
            {
                output += b.ToString() + ", ";
            }
            pluginInfo.LogMessage(output, ConsoleColor.Blue);
        }

        private static bool trickGenerator(PluginPlayer sender, bool isPrivate)
        {
            string sep = Path.DirectorySeparatorChar.ToString();
            string path = Directory.GetCurrentDirectory() + sep + "Plugins" + sep + "RandomServerTricks";
            if (File.Exists(path + sep + "tricks.json"))
            {
                Random rnd = new Random();
                var myJsonString = File.ReadAllText(path + sep + "tricks.json");
                var myJObject = JObject.Parse(myJsonString);

                var rotationList = myJObject.SelectTokens("$.Rotation").Values<string>().ToList();
                var bTrickList = myJObject.SelectTokens("$.bTrick").Values<string>().ToList();
                var aTrickList = myJObject.SelectTokens("$.aTrick").Values<string>().ToList();
                var vaTrickList = myJObject.SelectTokens("$.vaTrick").Values<string>().ToList();
                var Mod1List = myJObject.SelectTokens("$.Mod1").Values<string>().ToList();
                var Mod2List = myJObject.SelectTokens("$.Mod2").Values<string>().ToList();
                var Mod3List = myJObject.SelectTokens("$.Mod3").Values<string>().ToList();
                var Mod4List = myJObject.SelectTokens("$.Mod4").Values<string>().ToList();
                var Mod5List = myJObject.SelectTokens("$.Mod5").Values<string>().ToList();

                var rL = rotationList.Count;
                var bTL = bTrickList.Count;
                var aTL = aTrickList.Count;
                var vaTL = vaTrickList.Count;
                var Mod1L = Mod1List.Count;
                var Mod2L = Mod2List.Count;
                var Mod3L = Mod3List.Count;
                var Mod4L = Mod4List.Count;
                var Mod5L = Mod5List.Count;


                var firstSlot = "";
                var secondSlot = "";
                var thirdSlot = "";
                var fifthSlot = "";
                var fourthSlot = "";

                var TempfourthSlot = "";
                var tempMod = "";

                //I heard you like if statements.

                //First Slot/Modifier
                if (rnd.Next(0, 100) < 50)
                {
                    firstSlot = Mod1List[rnd.Next(0, Mod1L)].ToString() + "-";
                }

                //Mod 2/Rotation Slot
                if (rnd.Next(0, 120) < 50)
                {
                    secondSlot = Mod2List[rnd.Next(0, Mod2L)].ToString() + "-";
                    thirdSlot = rotationList[rnd.Next(0, rL)].ToString() + "-";
                    //gives a lower chance of 540 coming up
                    if (thirdSlot == "540-")
                    {
                        if (rnd.Next(0, 200) < 50)
                        {
                            thirdSlot = "540-";
                        }
                        else
                        {
                            thirdSlot = "180-";
                        }
                    }
                }

                //Trick Slot
                if (rnd.Next(0, 100) < 65)
                {
                    //selects BASIC tricks
                    fourthSlot = bTrickList[rnd.Next(0, bTL)].ToString() + "-";
                    //chance of a double or triple flip +  the biggerspin co-efficient
                    if (fourthSlot == "Kickflip" || fourthSlot == "Heelflip")
                    {
                        if (rnd.Next(0, 200) < 50)
                        {
                            tempMod = Mod4List[rnd.Next(0, Mod4L)].ToString();
                            TempfourthSlot = tempMod + " " + fourthSlot;
                            fourthSlot = TempfourthSlot;
                        }
                    }
                    else
                    {
                        if (fourthSlot == "Shuvit-" && thirdSlot == "180-" && secondSlot == "Frontside-" || secondSlot == "Backside-")
                        {
                            if (rnd.Next(0, 100) < 50)
                            {
                                fourthSlot = "Bigspin-";
                            }
                        }
                        else
                        {
                            if (fourthSlot == "Shuvit-" && thirdSlot == "360-" && secondSlot == "Frontside-" || secondSlot == "Backside-")
                            {
                                if (rnd.Next(0, 100) < 50)
                                {
                                    fourthSlot = "Biggerspin-";
                                }
                                else
                                {
                                    fourthSlot = "Biggerspin-" + Mod5List[rnd.Next(0, Mod5L)].ToString() + "-";
                                }
                            }
                        }
                    }
                }
                else
                {
                    //selects ADVANCED or VERY ADVANCED tricks
                    if (rnd.Next(0, 100) < 40)
                    {
                        fourthSlot = aTrickList[rnd.Next(0, aTL)].ToString() + "-";
                    }
                    else {
                        fourthSlot = vaTrickList[rnd.Next(0, vaTL)].ToString() + "-";
                    }

                    //figures out what a bigspin/biggerspin is
                    if (fourthSlot == "Laser Heel-" && thirdSlot == "360-" && secondSlot == "Frontside-" || secondSlot == "Backside-")
                    {
                        fourthSlot = "Biggerspin-Heel-";
                    }
                    else
                    {
                        if (fourthSlot == "Laser Heel-" && thirdSlot == "180-" && secondSlot == "Frontside-" || secondSlot == "Backside-")
                        {
                            fourthSlot = "Bigspin-Heel-";
                        }
                    }
                    if (fourthSlot == "Tre Flip-" && thirdSlot == "360-" && secondSlot == "Frontside-" || secondSlot == "Backside-")
                    {
                        fourthSlot = "Biggerspin-Flip-";
                    }
                    else
                    {
                        if (fourthSlot == "Tre Flip-" && thirdSlot == "180-" && secondSlot == "Frontside-" || secondSlot == "Backside-")
                        {
                            fourthSlot = "Bigspin-Flip-";
                        }
                    }
                    //the gazelle hypothesis
                    if (fourthSlot == "360 Shuvit-" && thirdSlot == "360-" && secondSlot == "Frontside-")
                    {
                        secondSlot = "";
                        fourthSlot = "Frontside-Gazellespin-";
                    }
                    if (fourthSlot == "360 Shuvit-" && thirdSlot == "360-" && secondSlot == "Backside-")
                    {
                        secondSlot = "";
                        fourthSlot = "Backside-Gazellespin-";
                    }
                    //adds direction to 360 shuvits
                    if (fourthSlot == "360 Shuvit-")
                    {
                        fourthSlot = secondSlot + "360 Shuvit-";

                    }
                }

                //Mod3 Slot
                if (rnd.Next(0, 100) > 70)
                {
                    if (fourthSlot == "Shuvit-" || fourthSlot == "Kickflip-" || fourthSlot == "Heelflip-")
                    {
                        fifthSlot = Mod3List[rnd.Next(0, Mod3L)].ToString() + "-";
                    }
                    if (fourthSlot == "Frontside-Gazellespin-" || fourthSlot == "Backside-Gazellespin-")
                    {
                        fifthSlot = Mod3List[rnd.Next(0, Mod3L)].ToString() + "-";
                    }

                }

                //correcting for biggerspin flip/heel
                if (fourthSlot == "Biggerspin-" || fourthSlot == "Bigspin-" || fourthSlot == "Biggerspin-Heel-" || fourthSlot == "Biggerspin-Flip-" || fourthSlot == "Shuvit-")
                {
                    thirdSlot = "";
                }
                if (fourthSlot == "360 Shuvit-" || fourthSlot == "Bigspin-Heel-" || fourthSlot == "Bigspin-Flip-")
                {
                    thirdSlot = "";
                }

                //shuvit variations
                if (fourthSlot == "Shuvit-") {
                    secondSlot = "";
                    fourthSlot = Mod2List[rnd.Next(0, Mod2L)].ToString() + " Shuvit-"; 
                }

                //the gazelleflip conjecture
                if (fourthSlot == "Frontside-Gazellespin-" || fourthSlot == "Backside-Gazellespin-" && fifthSlot == "Flip-") {
                    fifthSlot = "";
                    fourthSlot = fourthSlot.Replace("spin-", "flip-");
                }

                //compiles a final trick string
                var final = firstSlot + secondSlot + thirdSlot + fourthSlot + fifthSlot;
                final = final.ToString();

                //checks for weird tricks and doesn't display them
                if (final == "" || final == firstSlot || final == fifthSlot || final == firstSlot + fifthSlot || final == firstSlot + secondSlot || final == secondSlot)
                {
                    pluginInfo.SendServerAnnouncement("Dang 'puter broke, try again.", 10, "f00");
                    return true;
                }
                else
                {
                    var lC = final.Substring(final.Length - 1);
                    if (lC == "-")
                    {
                        var ftM = final.Remove(final.Length - 1, 1);
                        if (isPrivate == false)
                        {
                            pluginInfo.SendServerAnnouncement("Server Trick: " + ftM.ToString(), 10, "0f0");
                            File.AppendAllText(path + sep + "trickLog.txt", ftM.ToString() + "\n");
                            return true;
                        }
                        else if (isPrivate == true) {
                            pluginInfo.SendImportantMessageToPlayer("Private Trick: " + ftM.ToString(), 10, "2ff", sender.GetPlayer());
                            return true;
                        }
                    }
                }
            }
            else
            {
                pluginInfo.SendImportantMessageToPlayer("tricks.json does not exist or is not loading", 10, "f00", sender.GetPlayer());
                return true;
            }
            return false;
        }

        private static bool PlayerCommand(string message, PluginPlayer sender)
        {

            if (message.ToLower().StartsWith("/rt"))
            {
                if (stopWatch.IsRunning && stopWatch.Elapsed.Seconds < 30)
                {
                    if (stopWatch.Elapsed.Seconds < 30)
                    {
                        pluginInfo.SendImportantMessageToPlayer("Wait " + (stopWatch.Elapsed.Seconds - 30) + " seconds then try again", 10, "f00", sender.GetPlayer());
                        //pluginInfo.LogMessage(stopWatch.Elapsed.Seconds.ToString(), ConsoleColor.Blue);
                        return true;
                    }
                    else 
                    {
                        if (stopWatch.Elapsed.Seconds > 30)
                        {
                            stopWatch.Stop();
                            stopWatch.Reset();
                            trickGenerator(sender,false);
                            stopWatch.Start();
                        }
                        return true;
                    }
                }
                else {
                    stopWatch.Stop();
                    stopWatch.Reset();
                    trickGenerator(sender,false);
                    stopWatch.Start();
                    return true;
                }
            }
            if (message.ToLower().StartsWith("/prt")) {
                trickGenerator(sender, true);
                return true;
            }
            if (message.ToLower().StartsWith("/7rt"))
            {
                int index = 0;
                while (index < 7) {
                    trickGenerator(sender, true);
                    System.Threading.Thread.Sleep(2000);
                    index++;
                }
                pluginInfo.SendImportantMessageToPlayer("7 Tricks Generated - Have Fun", 10, "f00", sender.GetPlayer());
                return true;
            }
            return false;
        }
    }
}
