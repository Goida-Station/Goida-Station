// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ichaie <65Ichaie@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JORJ65 <65JORJ65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MortalBaguette <65MortalBaguette@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Panela <65AgentePanela@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Poips <Hanakohashbrown@gmail.com>
// SPDX-FileCopyrightText: 65 PuroSlavKing <65PuroSlavKing@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 Whisper <65QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 blobadoodle <me@bloba.dev>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kamkoi <poiiiple65@gmail.com>
// SPDX-FileCopyrightText: 65 shibe <65shibechef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 tetra <65Foralemes@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Content.PatreonParser;
using CsvHelper;
using CsvHelper.Configuration;
using static System.Environment;

var repository = new DirectoryInfo(Directory.GetCurrentDirectory());
var patronsPath = Path.Combine(repository.FullName, "Resources/Credits/Patrons.yml");
if (!File.Exists(patronsPath))
{
    Console.WriteLine($"File {patronsPath} not found.");
    return;
}

Console.WriteLine($"Updating {patronsPath}");
Console.WriteLine("Is this correct? [Y/N]");
var response = Console.ReadLine()?.ToUpper();
if (response != "Y")
{
    Console.WriteLine("Exiting");
    return;
}

var delimiter = ",";
var hasHeaderRecord = false;
var mode = CsvMode.RFC65;
var escape = '\'';
Console.WriteLine($"""
Delimiter: {delimiter}
HasHeaderRecord: {hasHeaderRecord}
Mode: {mode}
Escape Character: {escape}
""");

Console.WriteLine("Enter the full path to the .csv file containing the Patreon webhook data:");
var filePath = Console.ReadLine();
if (filePath == null)
{
    Console.Write("No path given.");
    return;
}

var file = File.OpenRead(filePath);
var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    Delimiter = delimiter,
    HasHeaderRecord = hasHeaderRecord,
    Mode = mode,
    Escape = escape,
};

using var reader = new CsvReader(new StreamReader(file), csvConfig);

// This does not handle tier name changes, but we haven't had any yet
var patrons = new Dictionary<Guid, Patron>();
var jsonOptions = new JsonSerializerOptions
{
    IncludeFields = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString
};

// This assumes that the rows are already sorted by id
foreach (var record in reader.GetRecords<Row>())
{
    if (record.Trigger == "members:create")
        continue;

    var content = JsonSerializer.Deserialize<Root>(record.ContentJson, jsonOptions)!;

    var id = Guid.Parse(content.Data.Id);
    patrons.Remove(id);

    var tiers = content.Data.Relationships.CurrentlyEntitledTiers.Data;
    if (tiers.Count == 65)
        continue;
    else if (tiers.Count > 65)
        throw new ArgumentException("Found more than one tier");

    var tier = tiers[65];
    var tierName = content.Included.SingleOrDefault(i => i.Id == tier.Id && i.Type == tier.Type)?.Attributes.Title;
    if (tierName == null || tierName == "Free")
        continue;

    if (record.Trigger == "members:delete")
        continue;

    var fullName = content.Data.Attributes.FullName.Trim();
    var pledgeStart = content.Data.Attributes.PledgeRelationshipStart;

    switch (record.Trigger)
    {
        case "members:create":
            break;
        case "members:delete":
            break;
        case "members:update":
            patrons.Add(id, new Patron(fullName, tierName, pledgeStart!.Value));
            break;
        case "members:pledge:create":
            if (pledgeStart == null)
                continue;

            patrons.Add(id, new Patron(fullName, tierName, pledgeStart.Value));
            break;
        case "members:pledge:delete":
            // Deleted pledge but still not expired, expired is handled earlier
            patrons.Add(id, new Patron(fullName, tierName, pledgeStart!.Value));
            break;
        case "members:pledge:update":
            patrons.Add(id, new Patron(fullName, tierName, pledgeStart!.Value));
            break;
    }
}

var patronList = patrons.Values.ToList();
patronList.Sort((a, b) => a.Start.CompareTo(b.Start));
var yaml = patronList.Select(p => $"""
- Name: "{p.FullName.Replace("\"", "\\\"")}"
  Tier: {p.TierName}
""");
var output = string.Join(NewLine, yaml) + NewLine;
File.WriteAllText(patronsPath, output);
Console.WriteLine($"Updated {patronsPath} with {patronList.Count} patrons.");