# FamiLife

Jérôme Tanghe, Quentin Van de Kadsye

FamiLife est une application de gestion de tâches familiales, dans laquelle des parents peuvent affecter des tâches de la vie quotidienne à leurs enfants et vérifier que ces tâches ont bien été faites.

## Mode d'emploi

Cette application est livrée avec une base de données par défaut. Celle-ci contient un utilisateur parent dont voici les identifiants :

- **Nom d'utilisateur :** `admin`
- **Mot de passe :** `admin`

Si la base de données n'est pas compatible et qu'une nouvelle base de données est créée, il faut se rendre sur la section _Rôles_ (http://localhost:1234/Roles) et créer deux nouveaux rôles, impérativement dans cet ordre :

- Parent
- Enfant

L'intitulé des rôles peut être différent, mais ils **doivent** être enregistrés dans cet ordre.

Créez ensuite un nouvel utilisateur dont le rôle est _Parent_ dans la section _Utilisateurs_ (http://localhost:1234/Utilisateurs).
