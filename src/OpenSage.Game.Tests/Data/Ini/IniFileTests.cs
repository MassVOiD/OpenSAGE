﻿using System.IO;
using System.Linq;
using OpenSage.Data;
using OpenSage.Data.Ini;
using OpenSage.Mods.BuiltIn;
using Xunit;
using Xunit.Abstractions;

namespace OpenSage.Tests.Data.Ini
{
    public class IniFileTests
    {
        private readonly ITestOutputHelper _output;

        public IniFileTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanReadIniFiles()
        {
            // TODO: Finish INI parsing for BFME2 and subsequent games.
            var gameDefinitions = new[]
            {
                GameDefinition.FromGame(SageGame.CncGenerals),
                GameDefinition.FromGame(SageGame.CncGeneralsZeroHour),
                GameDefinition.FromGame(SageGame.Bfme),
                GameDefinition.FromGame(SageGame.Bfme2),
                GameDefinition.FromGame(SageGame.Bfme2Rotwk),
            };

            foreach(var gameDefinition in gameDefinitions)
            {
                var rootDirectories = InstallationLocators.FindAllInstallations(gameDefinition)
                .Select(i => i.Path)
                .ToList();

                foreach (var rootDirectory in rootDirectories)
                {
                    using (var fileSystem = new FileSystem(rootDirectory))
                    {
                        var dataContext = new IniDataContext(fileSystem, gameDefinition.Game);

                        switch (gameDefinition.Game)
                        {
                            case SageGame.Bfme:
                            case SageGame.Bfme2:
                            case SageGame.Bfme2Rotwk:
                                dataContext.LoadIniFile(@"Data\INI\GameData.ini");
                                break;
                        }
                        

                        foreach (var file in fileSystem.Files)
                        {
                            if (Path.GetExtension(file.FilePath).ToLowerInvariant() != ".ini")
                            {
                                continue;
                            }

                            var filename = Path.GetFileName(file.FilePath).ToLowerInvariant();

                            switch (filename)
                            {
                                case "buttonsets.ini": // Doesn't seem to be used?
                                case "scripts.ini": // Only needed by World Builder?
                                case "commandmapdebug.ini": // Only applies to DEBUG and INTERNAL builds
                                case "fxparticlesystemcustom.ini": // Don't know if this is used, it uses Emitter property not used elsewhere
                                case "lightpoints.ini": // Don't know if this is used.
                                    continue;

                                case "optionregistry.ini": // Don't know if this is used
                                case "localization.ini": // Don't know if we need this
                                    switch(gameDefinition.Game)
                                    {
                                        case SageGame.Bfme:
                                        case SageGame.Bfme2:
                                        case SageGame.Bfme2Rotwk:
                                            continue;
                                    }
                                    break;
                            }

                            //TODO: parse these files
                            switch (gameDefinition.Game)
                            {
                                case SageGame.Bfme2:
                                    switch (filename)
                                    {
                                        case "ingamenotificationbox.ini":
                                        case "meshinstancingmanager.ini":
                                        case "commandset.ini": //problem
                                        case "riskcampaign.ini":
                                        case "createaherosystem.ini":
                                        case "skirmishaidata.ini":
                                        case "firelogicsystem.ini":
                                        case "formationassistant.ini":
                                        case "linearcampaign.ini":
                                        case "scoredkillevaannouncer.ini":
                                        case "strategichud.ini":

                                        case "livingworldaitemplate.ini":
                                        case "livingworldautoresolvearmor.ini":
                                        case "livingworldautoresolvebody.ini":
                                        case "livingworldautoresolvecombatchain.ini":
                                        case "livingworldautoresolvehandicaps.ini":
                                        case "livingworldautoresolveleadership.ini":
                                        case "livingworldautoresolvereinforcementschedule.ini":
                                        case "livingworldautoresolveresourcebonus.ini":
                                        case "livingworldautoresolvesciencepurchasepointbonus.ini":
                                        case "livingworldautoresolveweapon.ini":
                                        case "livingworldbuildingicons.ini":
                                        case "livingworldbuildings.ini":
                                        case "livingworldbuildploticons.ini":
                                        case "livingworldplayers.ini":
                                        case "livingworldregioneffects.ini":
                                            continue;
                                    }
                                    break;
                                case SageGame.Bfme2Rotwk:
                                    switch (filename)
                                    {
                                        case "attributemodifier.ini":
                                        case "awardsystem.ini":
                                        case "riskcampaign.ini":
                                        case "commandbutton.ini":
                                        case "commandset.ini":
                                        case "crate.ini":
                                        case "createaherosystem.ini":
                                        case "credits.ini":
                                        case "damagefx.ini":
                                        case "skirmishaidata.ini":
                                        case "firelogicsystem.ini":
                                        case "formationassistant.ini":
                                        case "ingamenotificationbox.ini":
                                        case "largegroupaudio.ini":
                                        case "linearcampaign.ini":
                                        case "linearcampaignexpansion1.ini":

                                        case "livingworld.ini":
                                        case "livingworldaitemplate.ini":
                                        case "livingworldautoresolvearmor.ini":
                                        case "livingworldautoresolvebody.ini":
                                        case "livingworldautoresolvecombatchain.ini":
                                        case "livingworldautoresolvehandicaps.ini":
                                        case "livingworldautoresolveleadership.ini":
                                        case "livingworldautoresolvereinforcementschedule.ini":
                                        case "livingworldautoresolveresourcebonus.ini":
                                        case "livingworldautoresolvesciencepurchasepointbonus.ini":
                                        case "livingworldautoresolveweapon.ini":
                                        case "livingworldbuildingicons.ini":
                                        case "livingworldbuildings.ini":
                                        case "livingworldbuildploticons.ini":
                                        case "livingworldplayers.ini":
                                        case "livingworldregioneffects.ini":
                                        case "angmaricons.ini":
                                        case "arnoricons.ini":
                                        case "dwarficons.ini":
                                        case "elficons.ini":
                                        case "isengardicons.ini":
                                        case "mordoricons.ini":
                                        case "mowicons.ini":
                                        case "wildicons.ini":

                                        case "meshinstancingmanager.ini":
                                        case "scoredkillevaannouncer.ini":
                                        case "specialpower.ini":
                                        case "strategichud.ini":
                                        case "upgrade.ini":
                                        case "weapon.ini":
                                        case "map.ini":
                                            continue;
                                    }
                                    break;
                            }

                            _output.WriteLine($"Reading file {file.FilePath}.");

                            dataContext.LoadIniFile(file);
                        }
                    }
                }
            }
        }
    }
}
