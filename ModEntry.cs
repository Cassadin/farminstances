using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.Tiles;

namespace FarmInstances
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        public static IModHelper helper = null;
        public static ModEntry instance;
        public override void Entry(IModHelper helper) {
            instance = this;
            ModEntry.helper = helper;
            new Events(); // alt: new Events(helper);

            helper.Events.Display.Rendered += Rendered;
        }

        public string currentMenu = "";
        private bool invcode_added = false;
        private void Rendered(object sender, RenderedEventArgs e)
        {
            // Am i in a SubMenu?
            if (StardewValley.Menus.TitleMenu.subMenu != null)
            {
                // ist it FarmhandMenu (picking of my farmer)
                if (StardewValley.Menus.TitleMenu.subMenu.GetType() == typeof(StardewValley.Menus.FarmhandMenu))
                {
                    if (invcode_added == false)
                    {
                        StardewValley.Menus.FarmhandMenu menu = ((StardewValley.Menus.FarmhandMenu)StardewValley.Menus.TitleMenu.subMenu);
                        // create new Button for an invitation-code
                        InviteToFarmSlot inv = new InviteToFarmSlot(menu, "Einladungscode erhalten?");

                        // MenuSlots is part of LoadGameMenu ... menu is an instance of FarmhandMenu ...
                        // Reflection dont work. How can i still use it?
                        List<LoadGameMenu.MenuSlot> slots = helper.Reflection.GetField<List<LoadGameMenu.MenuSlot>>(menu, "menuSlots").GetValue();
                        if (slots.Count > 0)
                        {
                            slots.Add(inv);
                            invcode_added = true;
                        }
                        // save the name of the current menu
                        this.currentMenu = menu.ToString();
                    }
                }
                else
                {
                    // did my menu changed?
                    if (this.currentMenu != StardewValley.Menus.TitleMenu.subMenu.ToString())
                    {
                        // save the name of the current menu
                        this.currentMenu = StardewValley.Menus.TitleMenu.subMenu.ToString();
                        invcode_added = false;
                    }
                }
            }
            else
            {
                // is an Menu Active? ... is null when closing the game
                if (Game1.activeClickableMenu != null)
                {
                    // did my menu change?
                    if (this.currentMenu != Game1.activeClickableMenu.ToString())
                    {
                        // save the name of the current menu
                        this.currentMenu = Game1.activeClickableMenu.ToString();
                    }
                }
                invcode_added = false;
            }

        }
    }

    internal class InviteToFarmSlot : StardewValley.Menus.LoadGameMenu.MenuSlot
    {
        internal FarmhandMenu menu;
        private string message;

        /// <summary>
        /// Button for entering a Farmcode
        /// </summary>
        /// <param name="menu">The menu, which shall hold the Button</param>
        /// <param name="message">The message of the button</param>
        internal InviteToFarmSlot(FarmhandMenu menu, string message) : base((LoadGameMenu)menu)
        {
            this.menu = menu;
            this.message = message;
        }

        // positioning of that button
        public override void Draw(SpriteBatch b, int i)
        {
            int widthOfString = SpriteText.getWidthOfString(this.message);
            int heightOfString = SpriteText.getHeightOfString(this.message);
            Microsoft.Xna.Framework.Rectangle bounds = this.menu.slotButtons[i].bounds;
            int x = bounds.X + (bounds.Width - widthOfString) / 2;
            int y = bounds.Y + (bounds.Height - heightOfString) / 2;
            SpriteText.drawString(b, this.message, x, y);
        }
        public override void Activate()
        {
            // what happens, when i click that button?
            ModEntry.instance.Monitor.Log("Open the Code-Entering!!!!");
        }
    }

}
