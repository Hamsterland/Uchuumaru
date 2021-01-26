### Moderation made kind of easy 
*Uchuumaru* is a moderation bot for the MyAnimeList Discord server. It encompasses infractions, filters, and user information inside an extensible command framework.

#### Help
Please see the [Commands](https://github.com/Hamsterland/Uchuumaru/wiki/Commands) page.

#### Premium
There is no premium.

#### Invite
There is no invite either. See the instructions below on running your own instance.

#### Support
Seek answers for your God. Sorry, atheists.

### Self Hosting
1. Create a new bot application in the Discord Developer Portal.
2. Create a PostgreSQL database.  
3. Create an appsettings.json file in the Uchuumaru/ directory. 
4. Populate the file with the following content
```json
{
    "Developers": [
        ""
    ],

    "Discord": {
        "Token":"",
        "Prefix":""
    },

    "Postgres": {
        "Connection":""
    }
}
```
5. `dotnet run -c Release`