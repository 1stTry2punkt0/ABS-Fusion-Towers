# ABS – Fusion Towers
Philon Hauk

## Projektbeschreibung
ABS – Fusion Towers ist ein Fantasy‑Mittelalter Tower‑Defense‑Spiel, in dem der Spieler Waffentürme und Elementartürme kombiniert, um stärkere Fusionstürme zu erschaffen. Gegner laufen einen dynamisch generierten Pfad entlang, während der Spieler Ressourcen verwaltet, strategisch baut und seine Verteidigung optimiert. Das Spiel erfüllt alle Vorgaben des Arbeitsauftrags (Unity 6, TD‑Gameplay, skalierbarer Content, mehrere Karten, mehrere Türme, mehrere Gegnertypen).

---

## Projektstart
**Unity Version:** Unity 6.0 – Editor Version 6000.0.65f1  
**Startszene:**  
`Assets/Scenes/MainMenu.unity`

Von dort aus lässt sich über **Spielen → Start** eine Runde beginnen.  
Im Hauptmenü können außerdem **Karte** und **Schwierigkeitsgrad** gewählt werden.

Der Schwierigkeitsgrad beeinflusst:
- Anzahl der Wellen (bis zu 100 auf Imperator)
- Startressourcen
- Level der Monster

---

## Allgemeine Informationen zum Projekt
- ESC öffnet während einer Runde das Pausenmenü.
- Das Spiel basiert vollständig auf **Point & Click**.
- Freie Felder (nicht Weg und nicht Bewachen/Blegt) öffnen das **Baumenü**.
- Bilder im Baumenü zeigen Gebäudebeschreibungen.
- Der Button unter dem Bild (mit Kosten) baut das Gebäude.
- Anklicken eines gebauten Gebäudes öffnet das Menü zum **Verkaufen oder Upgraden**.
- Upgrades erfolgen über den Kosten‑Button.
- Oben links unter dem HUD erscheinen Hinweise/Fehlermeldungen (z. B. fehlende Ressourcen).
- Oben rechts unter dem HUD stehen Spielstatus und Fusionierungsanweisungen.
- Play‑ oder Weiter‑Button startet die nächste Phase/Welle.
- Automatischer Rundenstart kann im Pausenmenü aktiviert/deaktiviert werden.
- Spulen‑Button beschleunigt das Spiel.
- Höhere Auflösungen zeigen mehr Umgebung, die Spielfläche bleibt jedoch gleich.

---

## Erfüllte Vorgaben

### Gameplay

#### Automatische Geschütztürme
Nach der Wegbauphase können Türme durch Anklicken eines freien Feldes und anschließenden Klick auf den Button im Baumenü platziert werden (sofern Ressourcen vorhanden sind).  
Türme schießen automatisch auf Gegner in Reichweite.

#### Verschiedene Türme & strategische Mischung
Es gibt **6 Türme** mit **9 Fusionsmöglichkeiten**.  
Der Spieler muss Kosten, Stärken und Statuseffekte berücksichtigen.  
Gleiche Statuseffekte lösen sich ab und können sich abschwächen.

#### Gegner laufen einen Pfad entlang
- Die meisten Gegner folgen dem gebauten Pfad zum Tor.
- Fliegende Gegner ignorieren den Pfad und fliegen direkt zum Tor.
- Am Tor verursachen alle Gegner einmalig Schaden.

#### Ressourcenbegrenzung
- **Münzen** (durch Händler pro Welle)  
- **Glaube** (wird während der Wellen automatisch generiert)

#### Perspektive
Klassische isometrische Vogelperspektive.

#### HUD
Am oberen Bildschirmrand befindet sich eine Leiste mit Informationen über Ressourcen, Leben und Wellenfortschritt.

---

### Content

#### Mindestens 3 Spielkarten
Es gibt **4 Spielkarten**, auswählbar im Dropdown vor Spielstart.  
Der Weg wird dynamisch generiert, Karten unterscheiden sich optisch und beeinflussen Elementartürme leicht.

**Projektpfad:**  
`Assets/Scenes/MapCave.unity`
`Assets/Scenes/MapDesert.unity`
`Assets/Scenes/MapForest.unity`
`Assets/Scenes/MapWinter.unity`

#### Mindestens 3 Türme
Es gibt **6 Türme** und **9 Fusionen**:

- Bogenschützen (schnell, Single Target)  
- Katapult (AoE)  
- Balliste (hohe Reichweite, Durchschuss)  

Alle können mit **Blitz**, **Eis** oder **Feuer** fusioniert werden.

**Projektpfad:**  
`Assets/Prefabs/Tower/`  
`Assets/Scripts/Tower/TowerSO/`

#### Mindestens 3 Gegnertypen
- 9 Standardgegner (2 fliegend)  
- Unterschiede in Werten und Resistenzen  
- 4 Bosse (2 fliegend) mit Fähigkeiten wie Heilung, Buffs oder Verwirrung der Türme  

**Projektpfad:**  
`Assets/Prefabs/Enemys/`  
`Assets/Scripts/Enemys/EnemySO`

#### Skalierbarer Content
Projektile, Gegner und Türme sind polymorph aufgebaut und nutzen Scriptable Objects.

**Projektpfad:**  
`Assets/Scripts/Enemys/`
`Assets/Scripts/Tower/`
`Assets/Scripts/Tower/Projectiles`

---

## Tower Defense – Setting

### Setting
Fantasy‑Mittelalter.  
Beispiele: Burgmauer, Händlerkarren, Pfeile, Katapult, Monolithen mit Runen, Drachen als Bosse, Reichweitenanzeige als weißer transparenter Kreis.

### Türme
- **Elementar:** Blitz, Feuer, Eis  
- **Waffen:** Bogenschützen, Ballisten, Bomben/Katapult  
- Skalierbare Unterarten möglich (Scriptable Objects, Prefabs)

### Fusionen
- Elementar‑ und Waffentürme können kombiniert werden.
- Beide Türme müssen **Stufe 6** sein.
- Nach Klick auf den Fusionsbutton wird das Zielturmfeld ausgewählt.

### Gegner
- 7 Fußsoldaten  
- 2 Fliegende  
- 4 Bosse  
Skalierbar über Scriptable Objects.

---

## Ressourcen

### Gold
Wird über Händler generiert und für Waffentürme benötigt.

### Glaube
Wird durch die zu beschützenden Einwohner generiert und für Elementartürme benötigt.  
Die Herkunft wird in den Beschreibungen der Elementartürme angedeutet.  
Glaube steigt während der Wellen automatisch.

---

## Spielkarten & Wegmanipulation

### Elementareffekte pro Karte
| Karte | Buff |
|-------|------|
| Grünland/Wald | Blitz |
| Schneeland/Winter | Eis |
| Vulkan/Höhlen | Feuer |
| Wüste | Kein Buff |

### Wegmanipulation
In der Wegbauphase können nur **Sehenswürdigkeiten** gebaut werden.  
Diese verändern den Weg so, dass er an ihnen vorbeiführt.

Einschränkungen:
- Nicht an den seitlichen Extremen platzierbar  
- Nicht direkt neben anderen Sehenswürdigkeiten  

Strategische Überlegung:
- Längere Wege → mehr Zeit für Türme  
- Aber auch mehr Zeit für Bosse und Händler  

**Projektpfad:**  
`Assets/Scripts/Map/`
`Assets/Scripts/Manager/`

---

## Third‑Party Assets  
## Änderungen an Third‑Party‑Assets

Im Projekt wurden mehrere Third‑Party‑Assets angepasst, erweitert oder in eigene Systeme integriert. Die Änderungen betreffen ganze Asset‑Kategorien und nicht einzelne Dateien.

## Änderungen an Third‑Party‑Assets

Im Projekt wurden mehrere Third‑Party‑Assets angepasst, erweitert oder in eigene Systeme integriert. Die Änderungen betreffen ganze Asset‑Kategorien und nicht einzelne Dateien.

### Partikeleffekte
- Anpassung von Emission, Lifetime, Start Size, Farben und Intensität zur besseren Lesbarkeit im Spiel.
- Entfernen überflüssiger Effekt‑Komponenten, um Performance und Übersichtlichkeit zu verbessern.
- Vereinheitlichung der Skalierung für das interne VFX‑Scaling‑System.
- Integration der Effekte in ein eigenes VFX‑Management‑ und Object‑Pooling‑System, wodurch Effekte nicht mehr direkt über die Original‑Prefabs, sondern über eigene Controller‑Skripte gestartet und verwaltet werden.
- Erstellung eigener VFX‑Prefabs auf Basis der gelieferten Partikel‑Assets.
- Nutzung eines einzelnen Third‑Party‑Scripts aus dem Lightning‑Bolt‑Assetpack ausschließlich zur **visuellen Darstellung** des Blitz‑Effekts (keine mechanische Funktion, keine Gameplay‑Relevanz).
- Beim Despawn‑Effekt aus dem Unity Particle Pack konnte das mitgelieferte Script **nicht 1:1 übernommen** werden; es wurde jedoch als **Orientierung** genutzt, um ein eigenes, angepasstes Script zu erstellen.

**Eigene relevante Script‑Pfadangaben:**  
`Assets/Scripts/Particle/`  
`Assets/Scripts/Tower/Fusion/FusionEffect.cs`

### Environment‑Assets
- Die Umgebung wurde nicht als fertige Szene übernommen, sondern aus einzelnen Third‑Party‑Modellen und Modulprefabs neu zusammengesetzt.
- Erstellung eigener zusammengesetzter Prefabs (z. B. Mauerelemente, Dekorationen, Landschaftsobjekte), um sie effizienter wiederverwenden zu können.
- Anpassung von Skalierung, Materialien und Platzierung für ein einheitliches Erscheinungsbild und zur besseren Integration in das Gameplay‑Layout.

### Gegner‑ und Turmmodelle
- Third‑Party‑Modelle wurden in eigene Enemy‑ und Tower‑Prefabs integriert.
- Zuweisung eigener Scripts für Bewegung, Angriff, Status‑Effekte, Fusionen und Lebenspunkteverwaltung.
- Anpassung der Pivot‑Points, Skalierung und Collider‑Strukturen für korrekte Treffererkennung.
- Anpassung oder Erstellung neuer Animator‑Controller, falls nötig (z. B. für den Händler).
- Mixamo‑Animationen wurden neu zugewiesen und in eigene Animator‑Controller eingebunden.

### UI‑ und Icon‑Assets
- Icons wurden in eigene UI‑Layouts integriert.
- Erstellung eigener UI‑Prefabs auf Basis der gelieferten Grafiken.

**Eigene Prefab‑Struktur:**  
`Assets/Prefabs/`

Es wurden keine Meshes oder Texturen direkt verändert, sondern ausschließlich technische Anpassungen, Prefab‑Erweiterungen und Integrationen vorgenommen, um die Assets in das eigene Spielsystem einzubinden.

### Models
https://kenney.nl/assets/tower-defense-kit  
https://assetstore.unity.com/packages/3d/environments/forest-low-poly-toon-battle-arena-tower-defense-pack-100080  
https://assetstore.unity.com/packages/3d/environments/winter-forest-low-poly-toon-battle-arena-tower-defense-pack-150432  
https://assetstore.unity.com/packages/3d/environments/dungeons/dungeon-low-poly-toon-battle-arena-tower-defense-pack-109791  
https://assetstore.unity.com/packages/3d/environments/desert-low-poly-toon-battle-arena-tower-defense-pack-124507  
https://assetstore.unity.com/packages/3d/environments/fantasy/knight-statue-295214  
https://assetstore.unity.com/packages/3d/characters/animals/mammals/stylized-low-poly-animated-horse-pack-137631  
https://assetstore.unity.com/packages/3d/props/low-poly-medieval-market-stalls-314286  
https://assetstore.unity.com/packages/3d/characters/humanoids/humans/lowpoly-medieval-peasants-free-low-poly-medieval-fantasy-series-122225  
https://www.mixamo.com/#/?page=3&query=sitting&type=Motion%2CMotionPack  

### Gegner
https://assetstore.unity.com/packages/3d/characters/creatures/meshtint-free-burrow-cute-series-184837  
https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-rock-golem-pbr-hp-polyart-94707  
https://assetstore.unity.com/packages/3d/characters/creatures/level-1-monster-pack-77703  
https://assetstore.unity.com/packages/3d/characters/creatures/stylized-free-skeleton-298650  
https://assetstore.unity.com/packages/3d/characters/creatures/dragon-for-boss-monster-hp-79398  
https://assetstore.unity.com/packages/3d/characters/dark-big-ghosts-lite-362212  

### VFX
https://assetstore.unity.com/packages/3d/props/potions-coin-and-box-of-pandora-pack-71778  
https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325  
https://assetstore.unity.com/packages/vfx/particles/free-quick-effects-vol-1-304424  
https://assetstore.unity.com/packages/vfx/particles/spells/zap-vfx-urp-303479  
https://assetstore.unity.com/packages/vfx/particles/spells/spells-pack-2-free-version-310764  
https://assetstore.unity.com/packages/vfx/trails-vfx-urp-242574  
https://assetstore.unity.com/packages/tools/particles-effects/lightning-bolt-effect-for-unity-59471

### UI
https://assetstore.unity.com/packages/2d/gui/icons/stone-ui-182526  
https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811  


### Font
https://www.1001freefonts.com/futhark.font?utm_source=copilot.com

### Sound
https://assetstore.unity.com/packages/audio/sound-fx/regular-impact-sounds-sound-effects-278024  
https://assetstore.unity.com/packages/audio/music/free-music-pack-1-207141  
https://pixabay.com/de/sound-effects/menschen-female-wilhelm-scream-audacity-86903/  
https://assetstore.unity.com/packages/audio/sound-fx/creatures/monster-roars-audio-pack-301118  

---

## USK‑12 Hinweis
Das Spiel erfüllt die **USK‑12‑Vorgaben**:  
Keine realistische Gewalt, keine Blutdarstellung, keine verstörenden Inhalte.
