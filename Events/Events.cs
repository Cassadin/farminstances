using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmInstances
{
    class Events
    {
        private int current_health = -1;
        private float current_stamina = -1;
        private List<int> experiences = new List<int>();
        private List<int> level = new List<int>();

        public delegate void PlayerHealthChanged(object sender, PlayerHealthChangedEventArgs e);
        public delegate void PlayerStaminaChanged(object sender, PlayerStaminaChangedEventArgs e);
        public delegate void PlayerReceivedXP(object sender, PlayerReceivedXPEventArgs e);
        public delegate void PlayerLeveledUp(object sender, PlayerLeveledUpEventArgs e);

        public static event PlayerHealthChanged OnPlayerHealthChanged;
        public static event PlayerStaminaChanged OnPlayerStaminaChanged;
        public static event PlayerReceivedXP OnPlayerReceivedXP;
        public static event PlayerLeveledUp OnPlayerLeveledUp;

        // usage:
        // Events.OnPlayerHealthChanged += OnDamaged;
        // Events.OnPlayerStaminaChanged += OnExhaust;
        // Events.OnPlayerReceivedXP += OnReceivedXP;
        // Events.OnPlayerLeveledUp += OnLeveledUp;


        /// <summary>
        /// initializes the Event-Check-Loop
        /// </summary>
        /// <param name="helper">IModHelper from ModEntry</param>
        public Events(IModHelper helper)
        {
            init(helper);
        }

        /// <summary>
        /// initializes the Event-Check-Loop. Needs the IModHelper as static variable in ModEntry.
        /// </summary>
        public Events()
        {
            init(ModEntry.helper);
        }


        /// <summary>
        /// initializes the Event-Check-Loop, with the given IModHelper.
        /// Uses the Rendered-Event for Checking the current Values
        /// </summary>
        /// <param name="helper"></param>
        private void init(IModHelper helper)
        {
            ModEntry.helper.Events.Display.Rendered += Rendered;
        }


        private void Rendered(object sender, RenderedEventArgs e)
        {
            if (Context.IsWorldReady)
            {

                if (OnPlayerHealthChanged != null)
                {
                    if (Game1.player.health != this.current_health)
                    {
                        if (this.current_health > -1)
                        {
                            PlayerHealthChangedEventArgs args = new PlayerHealthChangedEventArgs();
                            args.before = this.current_health;
                            args.current = Game1.player.health;
                            this.current_health = Game1.player.health;
                            OnPlayerHealthChanged.Invoke(null, args);
                        }
                        else
                        {
                            this.current_health = Game1.player.health;
                        }
                    }
                }

                if (OnPlayerStaminaChanged != null)
                {
                    if (Game1.player.stamina != this.current_stamina)
                    {
                        if (this.current_health > -1)
                        {
                            PlayerStaminaChangedEventArgs args = new PlayerStaminaChangedEventArgs();
                            args.before = this.current_stamina;
                            args.current = Game1.player.stamina;
                            this.current_stamina = Game1.player.stamina;
                            OnPlayerStaminaChanged.Invoke(null, args);
                        }
                        else
                        {
                            this.current_stamina = Game1.player.stamina;
                        }
                    }
                }

                if (OnPlayerReceivedXP != null)
                {
                    for (int i = 0; i < Game1.player.experiencePoints.Count; i++)
                    {
                        if (this.experiences.Count <= i)
                        {
                            this.experiences.Add(Game1.player.experiencePoints[i]);
                        }
                        else
                        {
                            if (this.experiences[i] < Game1.player.experiencePoints[i])
                            {
                                PlayerReceivedXPEventArgs args = new PlayerReceivedXPEventArgs();
                                args.experience_type = i;
                                args.before = this.experiences[i];
                                args.current = Game1.player.experiencePoints[i];
                                this.experiences[i] = Game1.player.experiencePoints[i];
                                OnPlayerReceivedXP.Invoke(null, args);
                            }
                            else
                            {
                                if (this.experiences[i] > Game1.player.experiencePoints[i])
                                    this.experiences[i] = Game1.player.experiencePoints[i];
                            }
                        }
                    }
                }

                if(OnPlayerLeveledUp != null)
                {
                    PlayerLeveledUpEventArgs args = new PlayerLeveledUpEventArgs();
                    args.experience_type = -1;
                    args.before = -1;
                    args.current = -1;
                    this.level[args.experience_type] = -1;
                    OnPlayerLeveledUp.Invoke(null, args);
                }
            }

        }
        }
    }
