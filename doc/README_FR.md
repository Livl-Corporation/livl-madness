<br/>
<br/>
<h1 align="center"><img src="./picture/Livl_Madness.png" width="400px"/>
<br/><br/>
  Livl Madness
</h1>
<p align="center">Dans Livl Madness, vous allez scanner des produits dans la folie d'un magasin Livl ! 🎮</p>

[Site web de Livl Madness](https://livl.franck-g.fr/)

🇬🇧 This readme is available in english [just here](../README.md) !

# Table des matières

- [🎮 Spécifications du jeu](#spécifications-du-jeu)
- [🕹️ Comment jouer ?](#comment-jouer-?)
  - [🙋 Jouer en solo](#jouer-en-solo)
  - [🏚️ Jouer en multijoueur local](#jouer-en-multijoueur-local)
  - [🌍 Jouer en multijoueur via Internet](#jouer-en-multijoueur-via-Internet)
  - [🙅 Dépannage](#dépannage)
- [⚙️ Spécifications techniques](#spécifications-techniques)
- [💖 Crédits](#crédits)

# 🎮 Spécifications du jeu

## Objectif

Les joueurs doivent se battre les uns contre les autres pour scanner autant de produits que possible dans un temps limité. Ils partagent la même liste de courses et les mêmes produits. Le joueur qui scanne le plus de produits gagne.

## Règles du jeu

Chaque partie dure 3 minutes.

Chaque joueur dans le magasin Livl partage la même liste de produits. Lorsqu'un joueur scanne un produit, il est retiré de la liste. Lorsqu'un produit est scanné, il est marqué comme "Épuisé" pendant un certain temps.

Lorsqu'un joueur scanne un produit, il gagne un point. S'il scanne le mauvais produit, il perd un point.

Il s'agit d'un jeu multijoueur, mais vous pouvez également y jouer en solo si vous n'avez pas d'amis fans de Livl près de vous.

## Configuration système requise

Nous fournissons un client pour les systèmes d'exploitation Windows, Linux et MacOsx.

Nous fournissons un serveur pour les systèmes d'exploitation Windows et Linux.

Plus d'informations à ce sujet dans la section [comment jouer](#Comment-jouer-?) .

# 🕹️ Comment jouer ?

Tout d'abord, vous devez télécharger votre client ou votre serveur à partir de notre [page de publication](https://github.com/Livl-Corporation/livl-madness/releases).

## 🙋 Jouer en solo

Pour jouer en solo, il vous suffit de télécharger le client correspondant à votre système d'exploitation et de l'exécuter.
Ensuite, lancez une partie et amusez-vous !

## 🏚️ Jouer en multijoueur local

Pour jouer en multijoueur en utilisant votre réseau local, assurez-vous que tous les ordinateurs des joueurs sont connectés au même réseau.

Le joueur hôte doit démarrer le jeu, entrer son pseudonyme et cliquer sur "Héberger une partie". Le joueur hôte doit ensuite partager son adresse IP locale aux autres joueurs.

Pour obtenir votre adresse IP locale, vous pouvez utiliser la commande `ipconfig` sur Windows ou `ifconfig` sur Linux dans votre terminal. Ensuite, partagez l'adresse IPv4 aux autres joueurs.

Les autres joueurs doivent cliquer sur le bouton d'engrenage en haut à droite de l'écran et entrer l'adresse IP de l'hôte.
Ensuite, choisissez un pseudonyme et cliquez sur "Rejoindre une partie".

## 🌍 Jouer en multijoueur via Internet

Un serveur public est disponible à l'adresse suivante : `chouffe.tgimenez.fr`.
Si vous ne pouvez pas rejoindre, c'est qu'une partie est déjà en cours ou que le serveur et éteint. Bonne chance pour deviner 🤓 !

Pour jouer à travers Internet, vous devez avoir un serveur en cours d'exécution sur un ordinateur connecté à Internet.

Il peut s'agir d'une version dédiée du serveur, ou via le client de jeu, en choisissant "Héberger une partie" après avoir entré votre pseudonyme.

Vous pouvez télécharger la version serveur sur notre [page de publication](https://github.com/Livl-Corporation/livl-madness/releases).

Pour démarrer le serveur, exécutez simplement l'exécutable.

Le port du jeu est le port miroir par défaut `7778`. Vous devez ouvrir ce port sur votre routeur et le configurer correctement pour permettre aux joueurs de se connecter à votre serveur.

Une fois que le serveur est en cours d'exécution, vous pouvez partager votre adresse IP publique à partir de [ce site](https://whatismyipaddress.com/) aux joueurs.

## 🙅 Dépannage

Si par hasard, il y a un problème lorsque vous démarrez votre jeu (le scan ne fonctionne pas, des plantages, etc...), surtout si c'est le deuxième jeu auquel vous jouez, n'hésitez pas à redémarrer TOUT (client et serveur)...

# ⚙️ Spécifications techniques

## Développement du jeu

Le jeu est développé en utilisant [Unity](https://unity.com/fr) comme moteur de jeu, et [Mirror](https://mirror-networking.com/) pour gérer l'aspect multijoueur, avec douleur... Maintenant vous savez pourquoi le jeu s'appelle Livl Madness !

## Ressources

Presque toutes les ressources de produits du jeu ont été réalisées à la main avec [Blender](https://www.blender.org/) en prenant des photos de produits réels dans le magasin Livl d'Eckbolsheim. Merci à notre photographe qui possède une galerie remplie de magnifiques photos de produits !

# 💖 Crédits

Voici nos quatre ingénieurs Livl qui ont travaillé sur ce merveilleux projet ! N'hésitez pas à les remercier pour leur travail acharné et leur dévouement à ce projet incroyable !

<table align="center">
  <tr>
    <th><img src="https://avatars.githubusercontent.com/u/19238963?v=4?v=4?size=115" width="115"><br><strong>@FranckG28</strong></th>
    <th><img  src="https://avatars.githubusercontent.com/u/62793491?v=4?size=115" width="115"><br><strong>@jvondermarck</strong></th>
    <th><img  src="https://avatars.githubusercontent.com/u/67447144?v=4?size=115" width="115"><br><strong>@Augustin68</strong></th>
    <th><img  src="https://avatars.githubusercontent.com/u/51646882?v=4?size=115" width="115"><br><strong>@Kayn017</strong></th>
  </tr>
  <tr align="center">
    <td><b><a href="https://github.com/FranckG28" style="color: white">Franck Gutmann</a></b></td>
    <td><b><a href="https://github.com/jvondermarck" style="color: white">Julien Von Der Marck</a></b></td>
    <td><b><a href="https://github.com/Augustin68" style="color: white">Raffael Di Pietro</a></b></td>
    <td><b><a href="https://github.com/Kayn017" style="color: white">Tanguy Gimenez</a></b></td>
  </tr>
</table>
