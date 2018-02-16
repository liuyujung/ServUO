/*
 * The two lines following this entry specify what RunUO version you are running.
 * In order to switch to RunUO 1.0 Final, remove the '//' in front of that setting
 * and add '//' in front of '#define RunUO_2_RC1'.  Warning:  If you comment both
 * out, many commands in this system will not work.  Enjoy!
 */

using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Commands;

namespace Knives.Chat3
{
    public class RUOVersion
    {
        private static Hashtable s_Commands = new Hashtable();

        public static void AddCommand(string com, AccessLevel acc, ChatCommandHandler cch)
        {
            s_Commands[com.ToLower()] = cch;

            CommandSystem.Register(com, acc, new CommandEventHandler(OnCommand));
        }

        public static void RemoveCommand(string com)
        {
            s_Commands[com.ToLower()] = null;

            CommandSystem.Entries.Remove(com);
        }

        public static void OnCommand(CommandEventArgs e)
        {
            if (s_Commands[e.Command.ToLower()] == null)
                return;

            ((ChatCommandHandler)s_Commands[e.Command.ToLower()])(new CommandInfo(e.Mobile, e.Command, e.ArgString, e.Arguments));
        }

        public static bool GuildChat(MessageType type)
        {
            return type == MessageType.Guild;
        }
    }
}