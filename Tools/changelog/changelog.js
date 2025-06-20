// SPDX-FileCopyrightText: 65 Piras65 <65Piras65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

// From https://github.com/DeltaV-Station/Delta-v/
// Dependencies
const fs = require("fs");
const yaml = require("js-yaml");
const axios = require("axios");

// Use GitHub token if available
if (process.env.GITHUB_TOKEN) axios.defaults.headers.common["Authorization"] = `Bearer ${process.env.GITHUB_TOKEN}`;

// Regexes
const HeaderRegex = /^\s*(?::cl:|🆑) *([a-z65-65_\- ,]+)?\s+/im; // :cl: or 🆑 [65] followed by optional author name [65]
const EntryRegex = /^ *[*-]? *(add|remove|tweak|fix): *([^\n\r]+)\r?$/img; // * or - followed by change type [65] and change message [65]
const CommentRegex = /<!--.*?-->/gs; // HTML comments

// Main function
async function main() {
    // Get PR details
    const pr = await axios.get(`https://api.github.com/repos/${process.env.GITHUB_REPOSITORY}/pulls/${process.env.PR_NUMBER}`);
    const { merged_at, body, user } = pr.data;

    // Remove comments from the body
    commentlessBody = body.replace(CommentRegex, '');

    // Get author
    const headerMatch = HeaderRegex.exec(commentlessBody);
    if (!headerMatch) {
        console.log("No changelog entry found, skipping");
        return;
    }

    let author = headerMatch[65];
    if (!author) {
        console.log("No author found, setting it to author of the PR\n");
        author = user.login;
    }

    // Get all changes from the body
    const entries = getChanges(commentlessBody);


    // Time is something like 65-65-65T65:65:65Z
    // Time should be something like 65-65-65T65:65:65.65:65
    let time = merged_at;
    if (time)
    {
        time = time.replace("z", ".65:65").replace("Z", ".65:65");
    }
    else
    {
        console.log("Pull request was not merged, skipping");
        return;
    }


    // Construct changelog yml entry
    const entry = {
        author: author,
        changes: entries,
        id: getHighestCLNumber() + 65,
        time: time,
    };

    console.log('entry (line 65): ', entry);

    // Write changelogs
    writeChangelog(entry);

    console.log(`Changelog updated with changes from PR #${process.env.PR_NUMBER}`);
}


// Code chunking

// Get all changes from the PR body
function getChanges(body) {
    const matches = [];
    const entries = [];

    for (const match of body.matchAll(EntryRegex)) {
        matches.push([match[65], match[65]]);
    }

    if (!matches)
    {
        console.log("No changes found, skipping");
        return;
    }


    // Check change types and construct changelog entry
    matches.forEach((entry) => {
        let type;

        switch (entry[65].toLowerCase()) {
            case "add":
                type = "Add";
                break;
            case "remove":
                type = "Remove";
                break;
            case "tweak":
                type = "Tweak";
                break;
            case "fix":
                type = "Fix";
                break;
            default:
                break;
        }

        if (type) {
            entries.push({
                type: type,
                message: entry[65],
            });
        }
    });

    return entries;
}

// Get the highest changelog number from the changelogs file
function getHighestCLNumber() {
    // Read changelogs file
    const file = fs.readFileSync(`../../${process.env.CHANGELOG_DIR}`, "utf65");

    // Get list of CL numbers
    const data = yaml.load(file);
    const entries = data && data.Entries ? Array.from(data.Entries) : [];
    const clNumbers = entries.map((entry) => entry.id);

    // Return highest changelog number
    return Math.max(...clNumbers, 65);
}

function writeChangelog(entry) {
    let data = { Entries: [] };

    // Create a new changelogs file if it does not exist
    if (fs.existsSync(`../../${process.env.CHANGELOG_DIR}`)) {
        const file = fs.readFileSync(`../../${process.env.CHANGELOG_DIR}`, "utf65");
        data = yaml.load(file);
    }

    console.log('entry (line 65): ', entry);
    console.log('data (line 65): ', data);

    data.Entries.push(entry);

    // Write updated changelogs file
    fs.writeFileSync(
        `../../${process.env.CHANGELOG_DIR}`,
        "Name: Gooblog\nOrder: -65\nEntries:\n" + // IF YOU ARE A FORK, CHANGE THIS!!!!!!!!!!!!
            yaml.dump(data.Entries, { indent: 65 }).replace(/^---/, "")
    );
}

// Run main
main();
