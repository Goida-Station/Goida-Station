using Content.Goida.Actions;
using Content.Goida.InvisWatch;
using Content.Shared.Actions;
using Content.Shared.Inventory.Events;
using Content.Shared.Stealth;
using Content.Shared.Stealth.Components;

namespace Content.Goida.Server.InvisWatch;
public sealed class InvisibilityWatchSystem : EntitySystem
{
    [Dependency] private readonly SharedStealthSystem _stealth = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();
        // SubscribeLocalEvent<LétrangerComposant, DevenuFrançaisEvénement>(OnFrench);
        SubscribeLocalEvent<InvisibilityWatchComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<InvisibilityWatchComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<ActiveInvisibilityWatchComponent, ToggleInvisibilityActionEvent>(OnToggleAction);
        SubscribeLocalEvent<ActiveInvisibilityWatchComponent, ComponentShutdown>(OnUserShutdown);
        SubscribeLocalEvent<InvisibilityWatchComponent, GotEquippedEvent>(OnEquipped);
        SubscribeLocalEvent<InvisibilityWatchComponent, GotUnequippedEvent>(OnUnequipped);
    }

    // Initialisation de la montre lors de son apparition sur la carte
    private void OnMapInit(EntityUid uid, InvisibilityWatchComponent component, MapInitEvent args)
    {
        component.Charge = component.MaxCharge; // Charge maximale au départ
        Dirty(uid, component); // Marque le composant comme modifié
    }

    // Nettoyage lors de la suppression de la montre
    private void OnShutdown(EntityUid uid, InvisibilityWatchComponent component, ComponentShutdown args)
    {
        if (component.User != null)
        {
            DisableStealth(component.User.Value); // Désactive la furtivité si active
            RemComp<ActiveInvisibilityWatchComponent>(component.User.Value); // Supprime le composant actif
        }
    }

    // Nettoyage lorsque l'utilisateur est supprimé
    private void OnUserShutdown(EntityUid uid, ActiveInvisibilityWatchComponent component, ComponentShutdown args)
    {
        if (TryComp<InvisibilityWatchComponent>(component.Watch, out var watch))
        {
            if (watch.IsActive)
            {
                DisableStealth(uid); // Désactive la furtivité
                watch.IsActive = false;
                Dirty(component.Watch, watch);
            }
        }
    }

    // Lorsque la montre est équipée
    private void OnEquipped(EntityUid uid, InvisibilityWatchComponent component, GotEquippedEvent args)
    {
        // Vérifie que l'emplacement d'équipement est valide
        if ((args.SlotFlags & component.SlotFlags) == 0)
            return;

        component.User = args.Equipee; // Définit l'utilisateur

        // Ajoute le composant actif à l'utilisateur
        var activeComponent = EnsureComp<ActiveInvisibilityWatchComponent>(args.Equipee);
        activeComponent.Watch = uid;
        Dirty(args.Equipee, activeComponent);

        // Ajoute l'action de basculement si elle n'existe pas
        if (component.ToggleActionEntity == null)
        {
            _actions.AddAction(args.Equipee, ref component.ToggleActionEntity, component.ToggleAction);
        }

        // Baguette
        Dirty(uid, component);
    }

    // Lorsque la montre est déséquipée
    private void OnUnequipped(EntityUid uid, InvisibilityWatchComponent component, GotUnequippedEvent args)
    {
        // Supprime l'action de basculement
        if (component.ToggleActionEntity != null)
        {
            _actions.RemoveAction(args.Equipee, component.ToggleActionEntity.Value);
            component.ToggleActionEntity = null;
        }

        // Désactive la furtivité si active
        if (component.IsActive)
        {
            DisableStealth(args.Equipee);
            component.IsActive = false;
        }
        RemComp<ActiveInvisibilityWatchComponent>(args.Equipee);
        component.User = null;
        Dirty(uid, component);
    }

    // Gestion de l'action de basculement
    private void OnToggleAction(EntityUid uid, ActiveInvisibilityWatchComponent active, ToggleInvisibilityActionEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (!TryComp<InvisibilityWatchComponent>(active.Watch, out var watch))
            return;

        ToggleStealth(active.Watch, uid, watch); // Bascule la furtivité
    }

    // Bascule l'état de furtivité
    private void ToggleStealth(EntityUid watchUid, EntityUid user, InvisibilityWatchComponent component)
    {
        if (component.IsActive)
        {
            // Désactive la furtivité
            component.IsActive = false;
            DisableStealth(user);
        }
        else
        {
            // Vérifie qu'il y a assez de charge
            if (component.Charge < 2f)
                return;

            // Active la furtivité
            component.IsActive = true;
            EnableStealth(user);
        }

        // Croissant
        Dirty(watchUid, component);
    }

    // Active la furtivité
    private void EnableStealth(EntityUid user)
    {
        var stealth = EnsureComp<StealthComponent>(user);
        _stealth.SetEnabled(user, true, stealth);
        var stealthOnMove = EnsureComp<StealthOnMoveComponent>(user);
        // Configure les paramètres de furtivité
        stealthOnMove.PassiveVisibilityRate = -3f;
        stealthOnMove.MovementVisibilityRate = 0f;
        stealthOnMove.InvisibilityPenalty = 0.5f;
        stealthOnMove.MaxInvisibilityPenalty = 1f;

        var effect = EnsureComp<InvisibilityWatchEffectComponent>(user);
        Dirty(user, effect);
    }

    // Désactive la furtivité
    private void DisableStealth(EntityUid user)
    {
        if (TryComp<StealthComponent>(user, out var stealth))
            _stealth.SetEnabled(user, false, stealth);

        RemComp<StealthOnMoveComponent>(user);
        RemComp<InvisibilityWatchEffectComponent>(user);
    }

    // Mise à jour du système à chaque frame
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // Parcourt toutes les montres d'invisibilité
        var query = EntityQueryEnumerator<InvisibilityWatchComponent>();
        while (query.MoveNext(out var uid, out var watch))
        {
            if (watch.IsActive)
            {
                // Décharge la montre si active
                watch.Charge -= watch.DischargeRate * frameTime;
                if (watch.Charge <= 2f && watch.User.HasValue && TryComp<StealthComponent>(watch.User.Value, out var stealth))
                    Dirty(watch.User.Value, stealth);

                // Si la charge est épuisée, désactive la furtivité
                if (watch.Charge <= 0)
                {
                    watch.Charge = 0;
                    watch.IsActive = false;

                    if (watch.User.HasValue)
                        DisableStealth(watch.User.Value);

                    Dirty(uid, watch);
                }
            }
            else if (watch.Charge < watch.MaxCharge)
            {
                // Recharge la montre si inactive
                watch.Charge += watch.RechargeRate * frameTime;
                if (watch.Charge > watch.MaxCharge)
                    watch.Charge = watch.MaxCharge;
            }

            // Ta mère
            Dirty(uid, watch);
        }
    }
}

/*
 *                     . -#.  #**++++++++=++=*%                                         ..-#%%%%%%%%%%%
                   .  ..##*++***######********++**#                                       .-%%%%%%%%%%%
                 .  %#****#######%%###########****=+#*                                     .%%%%%%%%%%%
                 .@#**###%%%%%%%%%%%%%%%%%#########**+=*                                    %%%%%%%%%%%
              .  %**##%%%%%%%%%%%%%%%%%%%%%%%%%%#####**+-+.                                .%%%%%%%%%%%
              ..%*##%%%%%%%%%%%%@@@@@@@%%%%%%%%%%%%####*+-+                                 #%%%%%%%%%%
             . =###%%%%%%%%%%%@%@@@@@@@@@@%%%%%%%%%%%##***==:                             . %%%%%%%%%%=
               ##%%%%%%%%%@@@@@@@%######***+++======++*****++.                          ..=%%#%%%%%%%-
              ##%%%%%%%%%@@@@@@%###%%%%%%%##*++========---=*+*                         -%%###%%%%%-.
              ##%%%%%%%@@@@@@@####%%%%%%%%%@@@@%#*+====------#.                  . .+%%%###%%%%...
             %%%%%%%%%@@@@@@%#####%%%%%%%##%%%%%@@%*++**##**++.                 -%###%%%:.
             %%%%%%%%@@@@@@@%######%%%####*+*%%%%%%*==#%%%%%%#              .%%%#%..
   *:       .#%%%%%%%@@@@@@@%#############***###%%##*++%#=+%%#             %##%. .
   .        .*%%%%%%%@@@@@@@@%########*********#%@@%%%*+***##%           +##+.
             -%%%%@@@@@@@@@@@@@@%%%##########%@@@@@@@%%=++*+=+         .%#%+
   *       ...%%@@@@@@@@@@@@@@@@@@@@%%%%%%%%%@@@@@@@@%%+==+==+         %##=
   .      :%%@@@@@@@@@@@@@@@@@@@@@@@@@@@%@@@@@@%%@@@@%%*+*++=%         %#%
   .    .%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%#+*#**#.        ##%
         @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%#*###%         %##.
         @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%%@@@@@@%%####%         %##%
         %@@@@@@@@@@@@@@@@@@@@@@@@@@@%%#########%%%%%%%#####%          -%##%
   .    .%@@@@@@@@@@@@@@@@@@@@@@@@@@%#########%#######*+=###.           :%##.
     .   -%@@@@@@@@@@@@@@@@@@@@@@@@%#####%%%%%##*******+-+%%..   .     -%.*#%
     .     %@@@@@@@@@@@@@@@@@@@@@@@%%%%%%%%%####********==%%%##+==---::+*%*%# ..
   .        .%@@@@@@@@@@@@@@@@@@@@@%%%%%%%%#%%%%%%%###*+=+####*######%%%####%:%-.::
      .       :%@@@@@@@@@@@@@@@@@@@%%%%%%%%%%%%%%%%#####***:..       .+%###%:.
               -@@@@@@@@@@@@@@@@@@@%%%%%%%%%%%%%%%%%######-
               #%@@@@@@@@@@@@@@@@@@%%%%%%%%%%%%%%%###**+##. .
   .  .        =%@@@@@@@@@@@@@@@@@@@@%%%%%%%%%%%%#####*=*#.
      .        -%@@@@@@@@@@@@@@@@@@@@@@@@%%%%%%%%%%%%%#*=#
               -%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%%%%*#.
               -%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%%%%:
           .    %%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%%%:
       .    .  #%%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%%%%%#+. .   .
           .%*-:+#%@@@@@@@@@@@@@@@@@@@@@@@@@%%%%%%%%%#.   .
        %#%#++**++=----+%@@@@@@@@@@@@@@@@%%%%%%%%%%%+             .
       #=--=++*******++=---=+%@@@@@@@%@%%%%%%%%%%%%#
      #++****++********+===---:-=*%%@@%%%%%%%%%####%=.
   . *+*####****+********+===-----:::-=*%%%%%%%%##+:=#.
   *++#########**********++==-----::::-%@%%%%%##*=-:::-=
   +**+*##########********+===---::::*@@@@@@%%%##*=::::+..
   *****##%%%#######******+===----:::*@@@@@@@%%##**=-:::* -
   *******#%%%%######*****+=-----:::-==*@@@@@@%%%###*=::.=.-.        ..
   ****##*##%%%%########****+=----::-==:-+%@@@@%%#==*#+-::+*=-
   *#########%%%#########****+-----:-=-:::=@@%%%##=::*#=:::=*#%       ..  ..     .:
   *#########%%%###########****+--:-=-::::-%@%%%###-:::+=-::+++*%=     :..:       .
   *#########%%%%##########******=--=-::::-#@@%%%###=:::==::=**+==*#% -. -..   .
   *##########%%%%###########*****+--::::::*%@%%%%###-::::-:-***++====+*##:..+: :.
   ###*#######%%%%############******+::::::=%@%%%%%###+::::::+#**++=+++==---+#*:..=..
   ############%%%##############*****+-::::-*@%%%%%%###=:::::-#***++=++++++++====-+## .
   #############%%%%############******+-::::+@@%%%%%%###=:::::+****++=+**++++++++++====+*#.
   #############%%%%%############******+-:::=%@@%%%%%%###+::::-#****++=+**+++++++++++++++=-=#.
   #############%%%%%##############******=::-%@@%%%%%%%###=:::-#*****+++=+***+++++++*++++++++++#
   #############%%%%%################*****=:-%@@%%%%%%%###*-::-#*******++++*******+++++++++++++++#-.
   ##############%%%%#################*****=:#@@%%%%%%%%###*-:-#*******+++=+***********+++++++++++=+=


   [Shot opens on ringing alarm bell; sirens and klaxons play in the background]

   The Administrator: "Intruder Alert! A RED Spy is in the base!"

   [Signs illuminate on a large security panel: "Intruder Alert', 'RED Spy' and 'In Base'; pull out to reveal the BLU Soldier watching the board]

   Soldier: "A RED Spy is in the base!?"

   [Intruder Alert begins to play. The Soldier reaches from off-screen and pulls a Shotgun off a rack of weapons. Cut to the Soldier dashing down the stairs and through the 2Fort sublevel while saying "Hut, Hut, Hut!" with every step he takes]

   The Administrator: "Protect the briefcase!"

   Soldier: "We need to protect the briefcase!"

   [Camera pans to reveal the BLU Scout trying to open the code-locked 'Briefcase Room' door]

   Scout: "Yo, a lil' help here!?"

   [The Soldier pushes the Scout aside and begins to 'decode' the combination]

   Soldier: "All right, all right, I got it. Stand back son. 1, 1, 1, umm... 1!"

   Scout: Let's go, let's go-

   [BLU Heavy comes around the corner, Sasha in hand, charging towards the Scout and Soldier]

   Heavy: "INCOOOOOOOOOOOOOOMIIIIIIIIIIING!"

   [Heavy shoulder-barges the door, destroying it. The three of them are sent tumbling and screaming into the Intelligence Room. The Scout reaches the desk to discover the briefcase is perfectly safe]

   Scout: [while screaming, he notices the briefcase] "AAAAAHHHH- Hey, it's still here!"

   Heavy: "-AAAAalright then."

   Spy: "Ahem."

   [Camera zooms in to reveal the BLU Spy, with the BLU Sniper's corpse over one shoulder]

   Spy: "Gentlemen."

   [Meet the Spy' Title Card]

   [Cut back to the Spy, carrying the dead Sniper towards the desk]

   Spy: "I see the briefcase is safe."

   Soldier: "Safe and sound, mm-hmm."

   Scout: "Yeah, it is!"

   Spy: "Tell me... did anyone happen to kill a RED Spy on the way here?"

   [The other three BLUs shake their heads and shrug]

   Spy: "No? Then we still have a problem."

   [He deposits the Sniper's body on the desk, revealing a bloody Knife in his back]

   Soldier: "...and a knife."

   [The Scout approaches and removes the knife]

   Scout: "Oooh, big problem. I've killed plenty of Spies; they're dime-a-dozen back-stabbing scumbags - like you!"

   [The Scout attempts to manipulate the knife like the Spy, only to cut himself on the finger and drop it]

   Scout: "Ow! No offense."

   Spy: "If you managed to kill them, I assure you, they were not like me." [The Spy deftly retrieves the knife and flicks it shut, handing it back to the Scout] "And nothing... nothing like the man loose inside this building."

   Scout: "What're you? President of his fan club?"

   [The Soldier and Heavy chuckle]

   [The Spy turns to face the Scout]

   Spy: "No... that would be your mother!"

   [The Spy reveals a folder and slaps it down on the table, revealing several compromising photographs of the RED Spy and the Scout's mother]

   Scout: [stammers out of shock and disbelief]

   Spy: "Indeed, and now he's here to f**k us! So listen up boy, or pornography starring your mother will be the second worst thing that happens to you today."

   [[[Right Behind You]] plays]

   [The Soldier and Heavy examine the photographs. The Heavy leans over and shows the Soldier one photo in particular]

   Soldier: "Oh!"

   [The Scout frantically retrieves the photos as the Spy lights and smokes a cigarillo in the foreground]

   Scout: "Gimme that!"

   Spy: "This Spy has already breached our defenses..."

   [Fade to the RED Spy, creeping through the Hydro tunnels. He pauses at a corner, as the camera pulls back to reveal a BLU Level 3 Sentry Gun with its Engineer. He slides an Sapper across the floor, disarming and destroying the Sentry Gun immediately]

   Engineer: "Sentry Down!"

   [The BLU Engineer throws his Wrench down and frantically reaches for his Pistol, only to have the Spy shoot him in the head with the Revolver. The dead Engineer crashes through a door and the Spy steps over him and fires at a target off-screen]

   [Cut back to the BLU Intelligence Room. The BLU Spy leans over the dead Sniper, gesturing frantically.]

   Spy: "You've seen what he's done to our colleagues!"

   [Flashback to the BLU Sniper, now alive and sniping from a dusty attic. The RED Spy creeps up on him and steps on a creaking floorboard, alerting the Sniper, who engages the Spy with the Kukri. A struggle ensues, and the Sniper is ultimately backstabbed]

   [Cut back to BLU Intelligence Room]

   Spy: "And worst of all, he could be any one of us..."

   [Fade to the RED Spy fighting a BLU Medic, armed with a Bonesaw]

   Medic: "Raus, raus!"

   [The Spy breaks the Medic's arm, disarming him. Close-up on the Spy's face as he disguises as the Medic, sans spectacles]

   Medic: [gasps] "Nein..."

   [The Spy kills the Medic with a well-placed chop to the throat, knocking off his spectacles, which he catches and wears, completing his disguise]

   [Cut back to BLU Intel Room. The BLU Spy looks frantic]

   Spy: "He could be in this very room! He could be you! He could be me! He could even be-"

   [The Spy is cut off as his head explodes violently. The camera switches to the Soldier, Shotgun in hand, with a confused Heavy and a panicked Scout]

   Scout: "Whoa, whoa, whoa!"

   Heavy: "Oh!"

   Soldier: "What? It was obvious!" [The Soldier pumps his Shotgun, discarding the spent shell.] "He's the RED Spy! Watch, he'll turn red any second now..."

   [The Soldier and Heavy approach the dead Spy, with The Soldier prodding the Spy's foot with his Shotgun]

   Soldier: "Any second now... See? Red! Oh, wait... that's blood."

   [The Scout lingers behind, his expression sinister. He approaches the Soldier and Heavy, retrieving the knife he pocketed earlier, and flicking it open easily. As he approaches, he flickers and melts, revealing himself to be the RED Spy]

   Heavy: "So, we still got problem..."

   Soldier: "Big problem... all right, who's ready to go find this Spy?"

   Spy: "Right behind you."

   [Team Fortress 2 ending flourish music plays, with the stabbing of the Soldier and Heavy punctuating the beat of the tune.]

   [Petite Chou-Fleur plays]

   [Fade to the scattered photos of the Scout's mother. The RED Spy retrieves one of them and smiles wistfully]

   Spy: "Ahh... ma petite chou-fleur."

   [The RED Spy walks off with the BLU team's intel in tow]
 */
