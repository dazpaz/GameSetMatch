# Admin Console
The Admin Console is used by the following personas:

- Tour Administrator
- Tournament Director
- Umpire
- GameSetMatch (GSM) Administrator

## Access to the Admin Console

GSM Administrators can invite users to Admin Console. Inviting a user causes an invitation e-mail to be sent the user. This email contains a link that allows the user to access the console for the first time and set their initial password. Following that, users authenticate with a username and password.

Access to the various functions of the Admin Console is controlled by role assignment. Users with no roles assigned have read only access to the console, able to view the tour, scheduled seasons, tournaments, venues, players, and users. They can manage thier own profile.

GSM Administrators can assign roles to users, and these roles give users to perform actions. The roles are:

- Tour Administrator
- Tournament Director
- Umpire
- GameSetMatch Administrator

## List of use cases

The table below lists the set of use cases, along with the role needed for that use case.

| Use Case | Anyone | Tour Administator | Tournament Director | Umpire | GSM Administator |
|----------|--------|-------------------|---------------------|--------|------------------|
|Accept invite and set initial password |X|||||
|Login to the admin console |X|||||
|View the list of tournaments |X|||||
|View the details of a tournament |X|||||
|View the details of an instance of a tournament |X|||||
|View the season schedule for current season |X|||||
|View the season schedule for previous seasons |X|||||
|View the list of venues |X|||||
|View the details of a venue |X|||||
|Add a new tournament ||X||||
|Edit a tournament ||X||||
|Remove a tournament ||X||||
|Schedule the tournaments for a season ||X||||
|Defined the details of the events for the tournament ||X||||
|Alter the schedule of a season ||X||||
|Assign the tournament directors for a tournament  ||X||||
|Add a venue ||X||||
|Edit a venue ||X||||
|Remove a Venue ||X||||
|See a list of the tournaments I am directing |||X|||
|Open the tournament for entries |||X|||
|Invite players to an event (bulk & individual) |||X|||
|See list of entrants to each event |||X|||
|Close the entry, selecting who has been successful |||X|||
|Conduct the draw for each event |||X|||
|Schedule matches – day, court, order, umpire |||X|||
|Complete the tournament |||X|||
|Apply to be an umpire for a tournament ||||X||
|View the list of players registered for the tour |X|||||
|View a list of all the users in the system |X|||||
|Handle registration applications from players |||||X|
|Invite users to the admin console |||||X|
|Remove users from the admin console |||||X|
|Assign roles to users |||||X|
|View my own profile details |X|||||
|Change my own profile details |X|||||
|Change password |X|||||
|Logout |X|||||
|||||||