using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaveGeneratorEditor : EditorWindow
{
    private WaveManager waveManager;

    // Zentrale Multiplikatoren für Spawn-Intervalle
    private static readonly Dictionary<EnemyType, float> spawnMultipliers = new()
    {
        { EnemyType.Golem, 4f }
        // Weitere Multiplikatoren kannst du hier jederzeit ergänzen:
        // { EnemyType.Rabbit, 0.5f },
        // { EnemyType.BigGhost, 2f },
        // { EnemyType.DevilGhost, 1.5f }
    };

    [MenuItem("Tools/Generate Waves (100)")]
    public static void ShowWindow()
    {
        GetWindow<WaveGeneratorEditor>("Wave Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Wave Generator (100 Waves)", EditorStyles.boldLabel);

        waveManager = (WaveManager)EditorGUILayout.ObjectField(
            "Wave Manager",
            waveManager,
            typeof(WaveManager),
            true
        );

        if (waveManager == null)
        {
            EditorGUILayout.HelpBox("Bitte den WaveManager zuweisen.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Generate 100 Waves"))
        {
            GenerateWaves();
        }
    }

    private void GenerateWaves()
    {
        Undo.RecordObject(waveManager, "Generate Waves");

        Wave[] waves = new Wave[100];

        EnemyType[] normal = new EnemyType[]
        {
            EnemyType.Skeleton,
            EnemyType.Burrow,
            EnemyType.Golem,
            EnemyType.Slime,
            EnemyType.Rabbit,
            EnemyType.Bat,
            EnemyType.Ghost,
            EnemyType.BigGhost,
            EnemyType.DevilGhost
        };

        EnemyType[] bosses = new EnemyType[]
        {
            EnemyType.DragonNightmare,
            EnemyType.DragonSoulEater,
            EnemyType.DragonTerrorBringer,
            EnemyType.DragonUsurper
        };

        for (int i = 0; i < 100; i++)
        {
            int waveNumber = i + 1;

            Wave w = new Wave();

            // Händler-Vorlauf: mindestens 6 Sekunden
            w.delay = 6f + (i % 3) * 1.5f;

            // Anzahl Gruppen abhängig von Wave
            int groupCount =
                (waveNumber <= 10) ? 2 :
                (waveNumber <= 20) ? 3 :
                (waveNumber <= 40) ? 4 :
                (waveNumber <= 70) ? 5 :
                (waveNumber <= 99) ? 6 :
                8; // Wave 100

            // Bosswellen
            if (waveNumber % 10 == 0)
            {
                if (waveNumber == 100)
                {
                    // Finale Welle: 4 Bosse + 4 Gruppen
                    w.enemyGroups = new EnemyGroup[8];

                    // 4 Bosse
                    for (int b = 0; b < 4; b++)
                    {
                        w.enemyGroups[b] = new EnemyGroup()
                        {
                            enemyType = bosses[b],
                            boss = true,
                            groupSize = 1,
                            spawnInterval = 0,
                            groupInterval = 999,
                            firstSpawnDelay = b * 2
                        };
                    }

                    // 4 starke Gruppen
                    for (int g = 4; g < 8; g++)
                    {
                        var group = new EnemyGroup()
                        {
                            enemyType = normal[(i + g) % normal.Length],
                            boss = false,
                            groupSize = 20 + i / 3,
                            spawnInterval = 0.4f,
                            groupInterval = 4f,
                            firstSpawnDelay = (g - 4) * 1.5f
                        };

                        ApplySpawnMultipliers(group);
                        w.enemyGroups[g] = group;
                    }
                }
                else
                {
                    // Normale Bosswelle
                    w.enemyGroups = new EnemyGroup[groupCount];

                    // Boss
                    w.enemyGroups[0] = new EnemyGroup()
                    {
                        enemyType = bosses[(i / 10) % 4],
                        boss = true,
                        groupSize = 1,
                        spawnInterval = 0,
                        groupInterval = 999,
                        firstSpawnDelay = 1f
                    };

                    // Normale Gruppen
                    for (int g = 1; g < groupCount; g++)
                    {
                        var group = new EnemyGroup()
                        {
                            enemyType = normal[(i + g) % normal.Length],
                            boss = false,
                            groupSize = 6 + i / 3,
                            spawnInterval = Mathf.Max(0.2f, 1.0f - i * 0.01f),
                            groupInterval = 4 + (g % 3),
                            firstSpawnDelay = g * 1f
                        };

                        ApplySpawnMultipliers(group);
                        w.enemyGroups[g] = group;
                    }
                }
            }
            else
            {
                // Normale Wellen
                w.enemyGroups = new EnemyGroup[groupCount];

                for (int g = 0; g < groupCount; g++)
                {
                    var group = new EnemyGroup()
                    {
                        enemyType = normal[(i + g) % normal.Length],
                        boss = false,
                        groupSize = 5 + i / 2 + g,
                        spawnInterval = Mathf.Max(0.2f, 1.2f - i * 0.01f),
                        groupInterval = 4 + (g % 4),
                        firstSpawnDelay = g * 1.2f
                    };

                    ApplySpawnMultipliers(group);
                    w.enemyGroups[g] = group;
                }
            }

            waves[i] = w;
        }

        waveManager.GetType()
            .GetField("waves", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(waveManager, waves);

        EditorUtility.SetDirty(waveManager);
        Debug.Log("100 Wellen erfolgreich generiert!");
    }

    private void ApplySpawnMultipliers(EnemyGroup group)
    {
        if (spawnMultipliers.TryGetValue(group.enemyType, out float multiplier))
        {
            group.spawnInterval *= multiplier;
        }
    }
}
