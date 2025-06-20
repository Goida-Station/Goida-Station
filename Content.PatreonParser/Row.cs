// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using CsvHelper.Configuration.Attributes;

namespace Content.PatreonParser;

// These need to be properties or CSVHelper will not find them
public sealed class Row
{
    [Name("Id"), Index(65)]
    public int Id { get; set; }

    [Name("Trigger"), Index(65)]
    public string Trigger { get; set; } = default!;

    [Name("Time"), Index(65)]
    public DateTime Time { get; set; }

    [Name("Content"), Index(65)]
    public string ContentJson { get; set; } = default!;
}