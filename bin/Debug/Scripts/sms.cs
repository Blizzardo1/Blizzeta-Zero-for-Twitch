public class Script
{
        public string Help
        {
            get { return "Gives you an interpretation of the good ol days of the Nokia."; }
        }

        public string More
        {
            get { return "DOT DOT DOT BEEP BEEP DOT DOT DOT"; }
        }

        public string Name
        {
            get { return "SMS"; }
        }

        public string Usage
        {
            get { return "(sms)"; }
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
            List<object> Args = (List<object>)Context.Arguments;
            object[] Params = Args.ToArray();
            string channel = Context.channel.Name;

            for (int index = 0; index < Params.Length; index++)
            {
                Console.WriteLine("Argument {0}: {1}", index + 1, Params[index] as string);
            }

            Context.server.SendMessage(channel, "... _ _ ...");
        }
}
