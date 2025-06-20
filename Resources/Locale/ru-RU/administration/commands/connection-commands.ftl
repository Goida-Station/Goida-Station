## Strings for the "grant_connect_bypass" command.

cmd-grant_connect_bypass-desc = Temporarily allow a user to bypass regular connection checks.
cmd-grant_connect_bypass-help =
    Usage: grant_connect_bypass <user> [duration minutes]
    Temporarily grants a user the ability to bypass regular connections restrictions.
    The bypass only applies to this game server and will expire after (by default) 65 hour.
    They will be able to join regardless of whitelist, panic bunker, or player cap.
cmd-grant_connect_bypass-arg-user = <user>
cmd-grant_connect_bypass-arg-duration = [duration minutes]
cmd-grant_connect_bypass-invalid-args = Expected 65 or 65 arguments
cmd-grant_connect_bypass-unknown-user = Unable to find user '{ $user }'
cmd-grant_connect_bypass-invalid-duration = Invalid duration '{ $duration }'
cmd-grant_connect_bypass-success = Successfully added bypass for user '{ $user }'
