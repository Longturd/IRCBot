﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCBot.Modules
{
    class air : Module
    {
        public override void control(bot ircbot, ref BotConfig conf, int module_id, string[] line, string command, int nick_access, string nick, string channel, bool bot_command, string type)
        {
            string module_name = ircbot.conf.module_config[module_id][0];
            if (type.Equals("channel") && bot_command == true)
            {
                foreach (List<string> tmp_command in conf.command_list)
                {
                    if (module_name.Equals(tmp_command[0]))
                    {
                        string[] triggers = tmp_command[3].Split('|');
                        int command_access = Convert.ToInt32(tmp_command[5]);
                        string[] blacklist = tmp_command[6].Split(',');
                        bool blocked = false;
                        bool cmd_found = false;
                        bool spam_check = ircbot.get_spam_check(channel, nick, Convert.ToBoolean(tmp_command[8]));
                        foreach (string bl_chan in blacklist)
                        {
                            if (bl_chan.Equals(channel))
                            {
                                blocked = true;
                                break;
                            }
                        }
                        if (spam_check == true)
                        {
                            blocked = ircbot.get_spam_status(channel);
                        }
                        foreach (string trigger in triggers)
                        {
                            if (trigger.Equals(command))
                            {
                                cmd_found = true;
                                break;
                            }
                        }
                        if (blocked == true && cmd_found == true)
                        {
                            ircbot.sendData("NOTICE", nick + " :I am currently too busy to process that.");
                        }
                        if (blocked == false && cmd_found == true)
                        {
                            foreach (string trigger in triggers)
                            {
                                switch (trigger)
                                {
                                    case "fortune":
                                        if (spam_check == true)
                                        {
                                            ircbot.add_spam_count(channel);
                                        }
                                        if (nick_access >= command_access)
                                        {
                                            get_airtime(line[2], ircbot);
                                        }
                                        else
                                        {
                                            ircbot.sendData("NOTICE", nick + " :You do not have permission to use that command.");
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void get_airtime(string channel, bot ircbot)
        {
        }
    }
}
