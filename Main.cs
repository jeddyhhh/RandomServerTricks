using System;
using System.Threading.Tasks;
using XLMultiplayerServer;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RandomServerTricks
{
    public class Main
    {
        private static Plugin pluginInfo;
        private static bool result = true;
        private static int secondsLeft = 0;
        private static readonly string sep = Path.DirectorySeparatorChar.ToString();
        private static readonly string path = Directory.GetCurrentDirectory() + sep + "Plugins" + sep + "RandomServerTricks";
        private static readonly Random rnd = new Random();

        private static string myJsonString6 = File.ReadAllText(path + sep + "config.json"); //i'm world champion of naming variables
        private static dynamic colourSettings = JsonConvert.DeserializeObject(myJsonString6);
        private static string serverTrickColour = colourSettings.serverTrickColour;
        private static string privateTrickColour = colourSettings.privateTrickColour;
        private static string customTrickColour = colourSettings.customTrickColour;


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
            if (message.ToLower().Contains("https://youtu.be/kYtltAFHDtA?t=508"))
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
            if (File.Exists(path + sep + "tricks.json"))
            {
                //reads tricks.json file and creates a object with the values loaded.
                var myJsonString = File.ReadAllText(path + sep + "tricks.json");
                var myJObject = JObject.Parse(myJsonString);

                var myJsonString2 = File.ReadAllText(path + sep + "config.json"); 
                var myJObject2 = JObject.Parse(myJsonString2);

                //grabs the json objects and converts them to lists
                var rotationList = myJObject.SelectTokens("$.Rotation").Values<string>().ToList();
                var bTrickList = myJObject.SelectTokens("$.bTrick").Values<string>().ToList();
                var aTrickList = myJObject.SelectTokens("$.aTrick").Values<string>().ToList();
                var vaTrickList = myJObject.SelectTokens("$.vaTrick").Values<string>().ToList();
                var Mod1List = myJObject.SelectTokens("$.Mod1").Values<string>().ToList();
                var Mod2List = myJObject.SelectTokens("$.Mod2").Values<string>().ToList();
                var Mod3List = myJObject.SelectTokens("$.Mod3").Values<string>().ToList();
                var Mod4List = myJObject.SelectTokens("$.Mod4").Values<string>().ToList();
                var Mod5List = myJObject.SelectTokens("$.Mod5").Values<string>().ToList();

                var basicTrickChance = Int16.Parse(myJObject2["basicTrickChance"].ToString());
                var advancedTrickChance = Int16.Parse(myJObject2["advancedTrickChance"].ToString());
                var veryAdvancedTrickChance = Int16.Parse(myJObject2["veryAdvancedTrickChance"].ToString());
                var mod1Chance = Int16.Parse(myJObject2["mod1Chance"].ToString());
                var mod2Chance = Int16.Parse(myJObject2["mod2Chance"].ToString());
                var mod3Chance = Int16.Parse(myJObject2["mod3Chance"].ToString());

                //counts entries within catagories
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

                //Mod 1/Modifier
                if (rnd.Next(0, 100) > mod1Chance)
                {
                    firstSlot = Mod1List[rnd.Next(0, Mod1L)].ToString() + "-";
                }

                //Rotation Slot
                if (rnd.Next(0, 100) > 70)
                {   
                    //frontside/backside
                    secondSlot = Mod2List[rnd.Next(0, Mod2L)].ToString() + "-";
                    //rotation in degrees
                    thirdSlot = rotationList[rnd.Next(0, rL)].ToString() + "-";
                    //gives a lower chance of 540 coming up
                    if (thirdSlot == "540-")
                    {
                        if (rnd.Next(0, 100) > 75)
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
                if (rnd.Next(0, 100) > basicTrickChance)
                {
                    //selects BASIC tricks
                    fourthSlot = bTrickList[rnd.Next(0, bTL)].ToString() + "-";
                    //chance of a double or triple flip +  the biggerspin co-efficient
                    if (fourthSlot == "Kickflip" || fourthSlot == "Heelflip")
                    {
                        if (rnd.Next(0, 100) > mod3Chance)
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
                    if (rnd.Next(0, 100) > advancedTrickChance)
                    {
                        fourthSlot = aTrickList[rnd.Next(0, aTL)].ToString() + "-";

                        //figures out what a bigspin/biggerspin is - probably wrong
                        if (fourthSlot == "Laser Heel-" && thirdSlot == "360-" && (secondSlot == "Frontside-" || secondSlot == "Backside-"))
                        {
                            thirdSlot = "";
                            fourthSlot = "Biggerspin-Heel-";
                        }
                        else
                        {
                            if (fourthSlot == "Laser Heel-" && thirdSlot == "180-" && (secondSlot == "Frontside-" || secondSlot == "Backside-"))
                            {
                                thirdSlot = "";
                                fourthSlot = "Biggerspin-Heel-";
                            }
                        }
                        if (fourthSlot == "Tre Flip-" && thirdSlot == "360-" && (secondSlot == "Frontside-" || secondSlot == "Backside-"))
                        {
                            thirdSlot = "";
                            fourthSlot = "360 Hardflip-";
                        }
                        else
                        {
                            if (fourthSlot == "Tre Flip-" && thirdSlot == "180-" && (secondSlot == "Frontside-" || secondSlot == "Backside-"))
                            {
                                thirdSlot = "";
                                fourthSlot = "Biggerspin-Flip-";
                            }
                        }
                        //the gazelle hypothesis
                        if (fourthSlot == "360 Shuvit-" && thirdSlot == "360-" && secondSlot == "Frontside-")
                        {
                            secondSlot = "";
                            thirdSlot = "";
                            fourthSlot = "Frontside-Gazellespin-";
                        }
                        if (fourthSlot == "360 Shuvit-" && thirdSlot == "360-" && secondSlot == "Backside-")
                        {
                            secondSlot = "";
                            thirdSlot = "";
                            fourthSlot = "Backside-Gazellespin-";
                        }
                        //adds direction to 360 shuvits
                        if (fourthSlot == "360 Shuvit-")
                        {
                            fourthSlot = secondSlot + "360 Shuvit-";

                        }
                    }
                    else if (rnd.Next(0, 100) > veryAdvancedTrickChance)
                    {
                        fourthSlot = vaTrickList[rnd.Next(0, vaTL)].ToString() + "-";
                    }
                    else {
                        fourthSlot = aTrickList[rnd.Next(0, aTL)].ToString() + "-";
                    }
                }

                //Mod2 Slot
                if (rnd.Next(0, 100) > mod2Chance)
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

                //TRICK CORRECTIONS

                //ghetto birds
                if (firstSlot == "Fakie-" && secondSlot == "Backside-" && thirdSlot == "180-" && fourthSlot == "Hardflip-") {
                    firstSlot = "";
                    secondSlot = "";
                    thirdSlot = "";
                    fourthSlot = "Ghetto Bird";
                    fifthSlot = "";
                }

                //give reverts a direction
                if (fifthSlot == "180 Revert-")
                {
                    fifthSlot = Mod2List[rnd.Next(0, Mod2L)].ToString() + " 180 Revert-";
                }

                //correcting for biggerspin flip/heel
                if (fourthSlot == "Biggerspin-" || fourthSlot == "Bigspin-" || fourthSlot == "Biggerspin-Heel-" || fourthSlot == "Biggerspin-Flip-" || fourthSlot == "Shuvit-" || fourthSlot == "Bigspin-Heel-" || fourthSlot == "Bigspin-Flip-")
                {
                    thirdSlot = "";
                }

                //shuvit variations
                if (fourthSlot == "Shuvit-") {
                    secondSlot = "";
                    fourthSlot = Mod2List[rnd.Next(0, Mod2L)].ToString() + " Shuvit-"; 
                }
                if (fourthSlot == "360 Shuvit-") {
                    secondSlot = "";
                    thirdSlot = "";
                    secondSlot = Mod2List[rnd.Next(0, Mod2L)].ToString() + "-";
                }


                //the gazelleflip conjecture
                if (fourthSlot == "Frontside-Gazellespin-" || fourthSlot == "Backside-Gazellespin-" && fifthSlot == "Flip-") {
                    thirdSlot = "";
                    fifthSlot = "";
                    fourthSlot = fourthSlot.Replace("spin-", "flip-");
                }

                //corrects switch manuals, i think.
                if ((secondSlot == "180-" || firstSlot == "Fakie") && (fifthSlot == "To Manual-" || fifthSlot == "To Nose Manual-")) {
                    var tempFifthSlot = fifthSlot;
                    fifthSlot = "Switch " + tempFifthSlot.Substring(3);
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
                        //figures out if its a server or private trick, also sorts out duplicate tricks.
                        if (isPrivate == false)
                        {
                            if (!File.Exists(path + sep + "trickLog.txt")) {
                                File.AppendAllText(path + sep + "trickLog.txt", "Kickflip" + "\n" + "Kickflip" + "\n" + "Kickflip" + "\n" + "Kickflip" + "\n" + "Kickflip" + "\n" + "Kickflip" + "\n" + "Kickflip" + "\n" + "Kickflip" + "\n");
                            }

                            //grabs the last 7 tricks generates from tricklog.txt and checks them with the newest generated trick
                            List<string> last7Tricks = File.ReadLines(path + sep + "trickLog.txt").Reverse().Take(7).ToList();

                            if (last7Tricks.Contains(ftM.ToString()))
                            {
                                trickGenerator(sender, false);
                                return true;
                            }
                            else {
                                pluginInfo.SendServerAnnouncement("Server Trick: " + ftM.ToString(), 10, serverTrickColour);
                                File.AppendAllText(path + sep + "trickLog.txt", ftM.ToString() + "\n");
                                return true;
                            }
                        }
                        else if (isPrivate == true) {
                                pluginInfo.SendImportantMessageToPlayer("Private Trick: " + ftM.ToString(), 10, privateTrickColour, sender.GetPlayer());
                                return true;
                        }
                    }
                }
            }
            else
            {
                pluginInfo.SendImportantMessageToPlayer("tricks.json does not exist or is not loading", 10, "f00", sender.GetPlayer()); //RED
                return true;
            }
            return false;
        }

        private static async Task<bool> RmSecondsToGo(int rushDelay) {
            await Task.Delay((int)(rushDelay * 0.70));
            pluginInfo.SendServerAnnouncement((Math.Ceiling((rushDelay / 1000) * 0.30)) + " seconds to go.", 10, "f00");
            return true;
        }

        //rushMode async rewrite
        static async Task<bool> rushMode(PluginPlayer sender) {
            var myJsonString4 = File.ReadAllText(path + sep + "config.json");
            var myJObject4 = JObject.Parse(myJsonString4);
            var rushDelay = Int32.Parse(myJObject4["rushDelay"].ToString());
            var rushAmount = Int16.Parse(myJObject4["rushAmount"].ToString());
            var showRushCountdown = Int16.Parse(myJObject4["showRushCountdown"].ToString());

            int index = 0;
            while (index < rushAmount)
            {
                if (showRushCountdown == 1)
                {
                    if (index == 0)
                    {
                        pluginInfo.SendServerAnnouncement("RUSH MODE: " + (rushDelay / 1000) + " second delay. " + rushAmount + " tricks.", 10, "f93");
                    }
                    _ = RmSecondsToGo(rushDelay);
                }
                trickGenerator(sender, false);
                await Task.Delay(rushDelay);
                index++;
            }
            pluginInfo.SendServerAnnouncement("RUSH MODE ENDED", 10, "f93");
            return true;
        }

        //Random trick async rewrite
        static async Task<bool> randomTrickMode(PluginPlayer sender, int genDelay, bool isAdmin) {

            if (isAdmin == true)
            {
                trickGenerator(sender, false);
                return true;
            }
            else
            {
                trickGenerator(sender, false);
                int index2 = 0;
                while (index2 < genDelay)
                {
                    result = false;
                    secondsLeft = (genDelay - index2);
                    await Task.Delay(1000);
                    index2++;
                }
                result = true;
                return true;
            }
        }


        public static bool PlayerCommand(string message, PluginPlayer sender)
        {
            var playersIPAdd = sender.GetIPAddress();
            var myJsonString2 = File.ReadAllText(path + sep + "config.json");
            var myJObject2 = JObject.Parse(myJsonString2);
            var genDelay = Int16.Parse(myJObject2["genDelay"].ToString());
            var enablePublicRush = Int16.Parse(myJObject2["enablePublicRush"].ToString());
            var rushDelay = Int32.Parse(myJObject2["rushDelay"].ToString());
            var rushAmount = Int16.Parse(myJObject2["rushAmount"].ToString());
            var showRushCountdown = Int16.Parse(myJObject2["showRushCountdown"].ToString());
            var enableCustomTricks = Int16.Parse(myJObject2["enableCustomTricks"].ToString());
            var adminIPAddrs = myJObject2.SelectTokens("$.adminIPs").Values<string>().ToList();
            var isAdmin = false;

            if (adminIPAddrs.Contains(playersIPAdd))
            {
                isAdmin = true;
            }

            //RANDOM TRICK MODE
            if (message.ToLower().Equals("/rt"))
            {
                if (isAdmin == true)
                {
                    _ = randomTrickMode(sender, genDelay, true);
                    return true;
                }
                else if (isAdmin == false) {
                    if (result == false)
                    {
                        pluginInfo.SendImportantMessageToPlayer("Wait " + secondsLeft + " seconds then try again", 10, "f00", sender.GetPlayer());
                        return true;
                    }
                    else if (result == true)
                    {
                        _ = randomTrickMode(sender, genDelay, false);
                        return true;
                    }
                }
                
            }

            //PRIVATE TRICK MODE
            if (message.ToLower().Equals("/prt"))
            {
                trickGenerator(sender, true);
                return true;
            }

            //7 RANDOM TRICKS MODE
            if (message.ToLower().Equals("/7rt"))
            {
                int index = 0;
                while (index < 7)
                {
                    trickGenerator(sender, true);
                    System.Threading.Thread.Sleep(1000);
                    index++;
                }
                pluginInfo.SendImportantMessageToPlayer("Tricks Generated - Have Fun", 10, "f00", sender.GetPlayer());
                return true;
            }

            //RUSH MODE
            if (message.ToLower().Equals("/rush") && enablePublicRush == 1 && isAdmin == false)
            {
                _ = rushMode(sender);
                return true;
            }
            if (message.ToLower().Equals("/rush") && enablePublicRush == 0 && isAdmin == false)
            {
                pluginInfo.SendImportantMessageToPlayer("Public Rush is not enabled", 10, "f00", sender.GetPlayer());
                return true;
            }
            if (message.ToLower().Equals("/rush") && isAdmin == true)
            {
                _ = rushMode(sender);
                return true;
            }

            //CUSTOM TRICK LIST MODE
            if (message.ToLower().StartsWith("/tl") && enableCustomTricks == 1 && isAdmin == true)
            {
                Regex rg = new Regex(@"^[a-zA-Z\s,]*$");
                var customTrickSel = message.Substring(3);
                if (!rg.IsMatch(customTrickSel))
                {
                    var myJsonString3 = File.ReadAllText(path + sep + "customTricks.json");
                    var myJObject3 = JObject.Parse(myJsonString3);
                    var customTrickList = myJObject3.SelectTokens("$.customTricks").Values<string>().ToList();
                    var customTrickCount = customTrickList.Count;
                    if (Int16.Parse(customTrickSel) >= customTrickCount)
                    {
                        pluginInfo.SendImportantMessageToPlayer("Invalid Custom Trick List Selection", 10, "f00", sender.GetPlayer());
                        return true;
                    }
                    else
                    {
                        pluginInfo.SendServerAnnouncement("Server Trick: " + customTrickList[Int16.Parse(customTrickSel)], 10, customTrickColour);
                        return true;
                    }
                }
                else if (enableCustomTricks == 0)
                {
                    pluginInfo.SendImportantMessageToPlayer("Custom Tricks Lists are not enabled", 10, "f00", sender.GetPlayer());
                    return true;
                }
                else if (isAdmin == false)
                {
                    pluginInfo.SendImportantMessageToPlayer("You are not admin", 10, "f00", sender.GetPlayer());
                    return true;
                }
                else if (enableCustomTricks == 0 && isAdmin == true)
                {
                    pluginInfo.SendImportantMessageToPlayer("Custom Trick List is not enabled", 10, "f00", sender.GetPlayer());
                    return true;
                }
                else
                {
                    pluginInfo.SendImportantMessageToPlayer("Invalid Custom Trick List Selection", 10, "f00", sender.GetPlayer());
                    return true;
                }
            }

            //CUSTOM TRICK BROADCAST
            if (message.ToLower().StartsWith("/ct") && isAdmin == true)
            {
                var customTrick = message.Substring(3);
                if (customTrick == "")
                {
                    pluginInfo.SendImportantMessageToPlayer("Invalid custom trick entry", 10, "f00", sender.GetPlayer());
                    return true;
                }
                else
                {
                    pluginInfo.SendServerAnnouncement("Server Trick:" + customTrick, 10, customTrickColour);
                    return true;
                }
            }
            else if (isAdmin == false)
            {
                pluginInfo.SendImportantMessageToPlayer("You are not admin", 10, "f00", sender.GetPlayer());
                return true;
            }
            return false;
        }
    }
}
