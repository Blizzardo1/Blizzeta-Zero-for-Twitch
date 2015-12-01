# BlizzetaZero-for-Twitch
BlizzetaZero is one of my major projects I've been working on for the last 5 years. Since the start of the bot and various work-overs with new code and using different languages, I've finally found the middle-ground and was able to work flawlessly with C#.

To set everything up, we need to add some values into the code. Firstly, you can combine the two projects into one solution to make life easier. If you still want Lemons, talk to Cave Johnson on that.

BlizzetaTwitchEngine:
  - Move this project's folder up one level
  - Launch the project and open Global.cs
    - Add your project's TwitchAPI Key and Secret. If you don't have them, you can obtain a key by [creating a project!](https://secure.twitch.tv/kraken/oauth2/clients/new)
Blizzeta Zero rev2:
  - Launch the project and open Global.cs
    - Change the server password to your own oauth:token. If you don't have one, you can obtain one [here.](https://twitchapps.com/tmi)

Scripting:
  Blizzeta Zero doesn't have a strong API, but it works. Basically, all that is needed to start a script is:
  ```C#
  public class ScriptClass
  {
    public string Help => "Description";
    public string More => "Details";
    public string Name => "Name of the script";
    public string Usage => "Define your usage here";
    public string Version => "Version of the script";
    public Permissions Permission => Permission.ChooseWhoShouldUseThisScript;
    public bool IsPublic => TrueIfWeWantThisScriptToBeAvailableInPublicChannels;
    
    public void Load(ref ScriptContext Context)
    {
        // Execute your code here
        // I'd always recommend having a try/catch statement, just in case things go awol.
    }
  }
  ```
