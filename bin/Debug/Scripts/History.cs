public class Script
{
        public string Help
        {
            get { return "Tells the history of how Blizzardo1 created and upgraded me"; }
        }

        public string More
        {
            get { return "Null Method Exception! WHY A NULL METHOD EXCEPTION?!?!!?"; }
        }

        public string Name
        {
            get { return "History"; }
        }

        public string Usage
        {
            get { return "(History)"; }
        }

        public string Version
        {
            get { return "1.0.0.0"; }
        }

        public Permissions Permission
        {
            get { return Permissions.Guest; }
        }

        public bool IsPublic
        {
            get { return true; }
        }

        public void Load(ref ScriptContext Context)
        {
            try
            {
                List<object> Args = (List<object>)Context.Arguments;
                object[] Params = Args.ToArray();
			    string channel = Context.channel.Name;

                Context.server.SendMessage(channel, "The Version history of Blizzeta to Date:");
                Context.server.SendMessage(channel, "Blizzeta was first known as VBBot(Visual Basic), then moved to BlizzyBot, then BlizzyBotPT, BlizzyBotPT2, BlizzyBotNT, BlizzyBotNT6 <endVB> Blizzeta 1.0(C#), Blizzeta 1.1, Blizzeta 2(Thresher), Blizzeta 3, Blizzeta 3.1, Blizzeta 3.2, Blizzeta 3.3, Blizzeta 3.4, Blizzeta 4.0, Blizzeta 5.0, Blizzeta 6.0, Blizzeta 6.1, Blizzeta 6.2, Blizzeta 6.3, Blizzeta 6.4, Blizzeta 6.5, Blizzeta 6.6, Blizzeta 7 Codenamed Zero");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
}
