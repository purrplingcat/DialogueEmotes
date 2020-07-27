using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace DialogueEmotes
{
    public class DialogueEmotesMod : Mod
    {
        private string lastEmotion;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Display.MenuChanged += this.OnDialogueClose;
        }

        private void OnDialogueClose(object sender, StardewModdingAPI.Events.MenuChangedEventArgs e)
        {
            if (e.NewMenu is DialogueBox)
                this.lastEmotion = null;
        }

        private void OnUpdateTicked(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (Game1.activeClickableMenu is DialogueBox dialogueBox && Game1.CurrentEvent == null)
            {
                var dialogue = this.Helper.Reflection.GetField<Dialogue>(dialogueBox, "characterDialogue").GetValue();

                if (dialogue != null)
                {
                    var currentEmotion = this.GetEmotion(dialogue);

                    if (currentEmotion != this.lastEmotion)
                    {
                        this.lastEmotion = currentEmotion;
                        this.ShowLastEmote(dialogue.speaker);
                    }
                }
            }
        }

        private string GetEmotion(Dialogue dialogue)
        {
            if (dialogue.getNPCResponseOptions()?.Count > 0 && dialogue.CurrentEmotion == "$neutral")
            {
                return "$q";
            }

            return dialogue.CurrentEmotion;
        }

        private void ShowLastEmote(NPC speaker)
        {
            int whichEmote;
            switch (this.lastEmotion)
            {
                case "$1":
                case "$h":
                    whichEmote = 32;
                    break;
                case "$2":
                case "$s":
                    whichEmote = 28;
                    break;
                case "$4":
                case "$l":
                    whichEmote = 20;
                    break;
                case "$5":
                case "$a":
                    whichEmote = 12;
                    break;
                case "$q":
                    whichEmote = 8;
                    break;
                default:
                    whichEmote = 0;
                    break;
            }

            if (whichEmote > 0)
                speaker.doEmote(whichEmote);
        }
    }
}
