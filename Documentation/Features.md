# Features

## Server

### Configuration Reload

The server supports live reloading of configuration changes without requiring a full restart. This feature allows operators to apply certain configuration changes at runtime while the server continues to operate.

#### Usage

To reload the server configuration, use the `reload` command in the server console:

```
> reload
```

#### Behavior

When the `reload` command is executed:

1. **Configuration Re-parsing**: The server reads the current `config.json` file from disk
2. **Selective Application**: Only reload-safe settings are applied to the running server
3. **Change Reporting**: The command provides clear feedback about:
   - Settings that were successfully applied
   - Settings that changed but require a restart to take effect

#### Example Output

```
> reload
    Configuration reload completed.
    Applied reload-safe settings:
      - AdminOnly
      - Logging.Level
      - Sprites.IdleFrameDuration
    Changed settings still requiring restart:
      - Equipment
      - Sprites.AttackFrames
```

#### Reload-Safe vs Restart-Required Settings

- **Reload-Safe Settings**: These can be applied immediately without restarting. Examples include logging levels, display settings, and certain gameplay parameters.
- **Restart-Required Settings**: These affect core server initialization or state that cannot be safely changed at runtime. Examples include database configuration, network ports, and certain structural game settings.

The distinction between reload-safe and restart-required settings is determined by internal annotations in the codebase. When a restart-required setting is changed, the reload command will report it but not apply it, ensuring server stability.

#### Benefits

- **Reduced Downtime**: Apply configuration tweaks without disconnecting players
- **Faster Iteration**: Test configuration changes more quickly during development
- **Clear Feedback**: Know exactly which changes were applied and which require restart

### Homepage

_**Requires the [API][API] to be enabled**_

This is a sample preview of the server built-in homepage, which includes a very basic leaderboard as well as user login/logout.

The homepage is accessible at [http://localhost:5400](http://localhost:5400) for development environments, but is also accessible at [https://localhost:5433](https://localhost:5433) (exposed as `https://*:5443` which can be reconfigured for your environment) in published release builds.

**By default the server does not have the assets to serve the in-game graphics as shown in the screenshot below, it requires a manual step. If you would like to see the in-game graphics please follow the instructions for the [AvatarController](./AvatarController/AvatarController.md).**

![Homepage Leaderboard](Features.HomepageLeaderboard.png)

[API]: https://docs.freemmorpgmaker.com/en-US/api/v1/introduction/setup/