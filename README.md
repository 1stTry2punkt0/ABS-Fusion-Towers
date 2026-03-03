# ABS-Fusion-Towers
Philon Hauk
▪ Eine Liste aller third-party Assets und vorgenommene Änderungen falls vorhanden.
▪ Jegliche Anweisungen zum Starten des Projekts. Dazu zählt auch welche Szene
geöffnet werden muss!
▪ Allgemeine Informationen zu dem Projekt, die ihr vielleicht sagen möchtet.
▪ Es empfiehlt sich möglichst aufzulisten, wie und wo welche Vorgaben erfüllt
wurden
Vorgaben:
Gameplay
o Der Spieler platziert automatische Geschütztürme, welche Wellen an Gegner bekämpfen.
o Es gibt eine Auswahl von verschiedenen Türmen und eine strategische Mischung ist nötig,
um das Spiel zu bestehen.
o Gegner Laufen einen Pfad entlang um ihr Ziel, der Basis des Spielers, zu erreichen und zu
beschädigen.
o Das Setzen von Türmen muss durch eine Ressource, meistens Geld, begrenzt werden.
o *Klassisch werden TD-Games aus einer top-down Perspektive gespielt, doch Variationen
sind erlaubt. (z.B. Spieler ist ein First Person Controller und muss Geschütze reparieren oder
kann selbst begrenzt mitkämpfen)
o Es gibt eine UI oder ein HUD das Infromationen zu Wellen, Lebenspunkten, Geldstand etc.
anzeigt.
− Content
o Mindesten 3 Spielkarten
o Mindestens 3 Türme
o Mindestens 3 Typen von Gegnern
o Content sollte, wo angemessen, skalierbar implementiert werden. (Polymorphie,
ScriptableObjects, Prefabs)

Tower Defense
Setting: Fantasy Mittelalter.
Türme: Elementar (bsp. Blitz, Feuer, Eis) und Waffen (bsp. Ballisten, Bomben) (ggf. mit skalierbaren Unterarten)
Besonderheit: Spieler kann Elementar- und Waffentürme miteinander zu stärkeren varianten kombinieren.
Gegner: Fußsoldaten, Fliegende (skalierbar mit Scriptable Objects für Geschwindigkeit und Leben pro Level) und Bosse(bsp. Mit de/buff Aura, Manipulation der Zielsuche)
Ressourcen:
1. Gold: wird über den zu bewachenden Pfad von Händlern generiert und für die Waffentürme gebraucht.
2. Glaube: wird durch die zu beschützenden Einwohner generiert und für Elementartürme gebraucht.
Spielkarten:
Umgebung beeinflusst Effektivität der Elementareffekte.
(Spieler manipuliert den Pfad mittels Sehenswürdigkeiten vor Spielstart)
